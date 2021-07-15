using System;

namespace Services.Identity.Core.Entities
{
    public static class Role
    {
        public const string User = "user";
        public const string Admin = "admin";
        
        private const StringComparison StringComparison = System.StringComparison.Ordinal;
        
        public static bool IsValid(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                return false;
            }

            return string.Equals(role, User, StringComparison) 
                   || string.Equals(role, Admin, StringComparison);
        }
    }
}