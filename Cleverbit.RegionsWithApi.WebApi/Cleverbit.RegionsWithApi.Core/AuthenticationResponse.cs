using Cleverbit.RegionsWithApi.Common.Exceptions;

namespace Cleverbit.RegionsWithApi.Core
{
    public class AuthenticationResponse
    {
        public string Token { get; set; }
        public DateTimeOffset ExpiryDate { get; set; }
        public string Email { get; set; }
        public Guid UserId { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Token))
                throw new CoreException("AuthenticationResponse => Token parameter should not be empty!");
            else if (string.IsNullOrEmpty(Email))
                throw new CoreException("AuthenticationResponse => Username parameter should not be empty! It should be the user's email address.");
            else if (UserId == Guid.Empty)
                throw new CoreException("AuthenticationResponse => UserId should not be empty.");
        }
    }
}
