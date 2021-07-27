using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Convey;
using Convey.Auth;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.CQRS.Queries;
using Convey.Discovery.Consul;
using Convey.Docs.Swagger;
using Convey.HTTP;
using Convey.LoadBalancing.Fabio;
using Convey.MessageBrokers;
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.Outbox;
using Convey.MessageBrokers.Outbox.Mongo;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Metrics.AppMetrics;
using Convey.Persistence.MongoDB;
using Convey.Persistence.Redis;
using Convey.Security;
using Convey.Tracing.Jaeger;
using Convey.Tracing.Jaeger.RabbitMQ;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Convey.WebApi.Swagger;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Services.Identity.Application;
using Services.Identity.Application.Commands;
using Services.Identity.Application.Services;
using Services.Identity.Application.Services.Identity;
using Services.Identity.Core.Repositories;
using Services.Identity.Infrastructure.Auth;
using Services.Identity.Infrastructure.Contexts;
using Services.Identity.Infrastructure.Decorators;
using Services.Identity.Infrastructure.Exceptions;
using Services.Identity.Infrastructure.Mongo;
using Services.Identity.Infrastructure.Mongo.Documents;
using Services.Identity.Infrastructure.Mongo.Repositories;
using Services.Identity.Infrastructure.Services;

namespace Services.Identity.Infrastructure
{
    public static class Extensions
    {
        public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
        {
            builder.Services.AddSingleton<IJwtProvider, JwtProvider>();
            builder.Services.AddSingleton<IPasswordService, PasswordService>();
            builder.Services.AddSingleton<IPasswordHasher<IPasswordService>, PasswordHasher<IPasswordService>>();
            builder.Services.AddTransient<IIdentityService, IdentityService>();
            builder.Services.AddTransient<IRefreshTokenService, RefreshTokenService>();
            builder.Services.AddSingleton<IRng, Rng>();
            builder.Services.AddTransient<IMessageBroker, MessageBroker>();
            builder.Services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
            builder.Services.AddTransient<IUserRepository, MongoUserRepository>();
            builder.Services.AddTransient<IUserAgentRepository, MongoUserAgentRepository>();
            builder.Services.AddTransient<IAppContextFactory, AppContextFactory>();
            builder.Services.AddTransient(ctx => ctx.GetRequiredService<IAppContextFactory>().Create());
            builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(OutboxCommandHandlerDecorator<>));
            builder.Services.TryDecorate(typeof(IEventHandler<>), typeof(OutboxEventHandlerDecorator<>));

            return builder
                .AddErrorHandler<ExceptionToResponseMapper>()
                .AddQueryHandlers()
                .AddInMemoryQueryDispatcher()
                .AddJwt()
                .AddHttpClient()
                .AddConsul()
                .AddFabio()
                .AddExceptionToMessageMapper<ExceptionToMessageMapper>()
                .AddRabbitMq(plugins: p => p.AddJaegerRabbitMqPlugin())
                .AddMessageOutbox(o => o.AddMongo())
                .AddMongo()
                .AddRedis()
                .AddMetrics()
                .AddJaeger()
                .AddMongoRepository<RefreshTokenDocument, Guid>("refreshTokens")
                .AddMongoRepository<UserDocument, Guid>("users")
                .AddMongoRepository<UserAgentDocument, Guid>("userAgents")
                .AddWebApiSwaggerDocs()
                .AddSecurity();
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseErrorHandler()
                .UseSwaggerDocs()
                .UseJaeger()
                .UseConvey()
                .UseAccessTokenValidator()
                .UseMongo()
                .UsePublicContracts<ContractAttribute>()
                .UseMetrics()
                .UseAuthentication()
                .UseRabbitMq()
                .SubscribeCommand<SignUp>();

            return app;
        }

        public static async Task<Guid> AuthenticateUsingJwtAsync(this HttpContext context)
        {
            var authentication = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);

            return authentication.Succeeded ? Guid.Parse(authentication.Principal.Identity.Name) : Guid.Empty;
        }

        internal static CorrelationContext GetCorrelationContext(this IHttpContextAccessor accessor)
        {
            var headers = accessor.HttpContext?.Request.Headers;
            if (headers != null && headers.TryGetValue("Correlation-Context", out var json))
            {
                return JsonConvert.DeserializeObject<CorrelationContext>(json.FirstOrDefault());
            }

            return null;
        }

    internal static IDictionary<string, object> GetHeadersToForward(this IMessageProperties messageProperties)
        {
            const string sagaHeader = "Saga";
            if (messageProperties?.Headers is null || !messageProperties.Headers.TryGetValue(sagaHeader, out var saga))
            {
                return null;
            }

            return saga is null
                ? null
                : new Dictionary<string, object>
                {
                    [sagaHeader] = saga
                };
        }
        
        internal static string GetSpanContext(this IMessageProperties messageProperties, string header)
        {
            if (messageProperties is null)
            {
                return string.Empty;
            }

            if (messageProperties.Headers.TryGetValue(header, out var span) && span is byte[] spanBytes)
            {
                return Encoding.UTF8.GetString(spanBytes);
            }

            return string.Empty;
        }
    }
}