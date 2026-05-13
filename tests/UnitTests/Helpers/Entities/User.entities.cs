using ExpenseTracker.Domain.Models.Entities;

namespace ExpenseTracker.UnitTests.Helpers.Entities
{
    public class CreateUserEntities
    {
        public static User User(string email = "test@email.com", string password = "secret", string firstName = "User", string lastName = "name", string currency = "Eur")
        {
            return new User(email, password, firstName, lastName, currency);
        }
    }
}