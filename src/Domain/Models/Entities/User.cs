using Spentir.Domain.Exceptions;

namespace Spentir.Domain.Models.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; } = string.Empty;
        public string Password { get; private set; } = string.Empty;
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Currency { get; private set; } = "EUR";
        public ICollection<Expense> Expenses { get; set; } = [];

        public User(string email, string password, string firstName, string lastName, string currency)
        {
            if (string.IsNullOrEmpty(email)) throw new DomainException("User must have an Email");
            if (string.IsNullOrEmpty(password)) throw new DomainException("User must have an Password");
            if (string.IsNullOrEmpty(firstName)) throw new DomainException("User must have an FirstName");
            if (string.IsNullOrEmpty(lastName)) throw new DomainException("User must have an LastName");

            Id = Guid.NewGuid();
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Currency = currency;
        }
    }
}