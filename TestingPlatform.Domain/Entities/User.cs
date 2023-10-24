using Microsoft.AspNetCore.Identity;

namespace TestingPlatform.Domain.Entities
{
    public class User: IdentityUser<int>
    {
        public DateTime DateRegistration { get; set; }

        public List<UserAnswer> UserAnswers { get; set; }
    }
}
