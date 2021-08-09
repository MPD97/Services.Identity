namespace Services.Identity.Core.Exceptions
{
    public class InvalidPasswordLengthException : DomainException
    {
        public override string Code => "invalid_password_length";
        public int GivenPasswordLength { get; }
        public int MinPasswordLength { get; }
        public int MaxPasswordLength { get; }
        public InvalidPasswordLengthException(int givenPasswordLength, int minPasswordLength, int maxPasswordLength) : 
            base($"Password length: {givenPasswordLength} is not valid. " +
                 $"Password must be between: {minPasswordLength} and {maxPasswordLength} characters long.")
        {
            GivenPasswordLength = givenPasswordLength;
            MinPasswordLength = minPasswordLength;
            MaxPasswordLength = maxPasswordLength;
        }
    }
}