using System;

namespace Moneybox.App
{
    public class Account
    {
        public const decimal PayInLimit = 4000m;
        public const decimal NotificationLimit = 500m;
        public const decimal PaidInLimit = 500m;

        public Guid Id { get; set; }
        public User User { get; set; }
        public decimal Balance { get; set; }
        public decimal Withdrawn { get; set; }
        public decimal PaidIn { get; set; }
        public void Withdraw(decimal amount)
        {
            BalanceCheck(amount);
            AmountRemoval(amount);
        }
        public void Deposit(decimal amount)
        {
            var payInAmount = AmountAdded(amount);
            if (payInAmount > PayInLimit)
            {
                throw new InvalidCastException("Account pay in limit reached");
            }
        }
        public void BalanceCheck(decimal amount)
        {
            var newBalance = Balance - amount;
            if (newBalance < 0)
            {
                throw new InvalidOperationException("Insufficient funds to make transfer");
            }
        }
        public static void AccountCheck(Account account)
        {
            if (account == null)
            {
                throw new InvalidOperationException("Account is null.");
            }
        }

        public void AmountRemoval(decimal amount)
        {
            Balance -= amount;
            Withdrawn -= amount;
        }

        public decimal AmountAdded(decimal amount)
        {
            Balance += amount;
            PaidIn += amount;
            return PaidIn;
        }
    }
}
