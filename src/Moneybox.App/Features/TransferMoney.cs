using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using System;

namespace Moneybox.App.Features
{
    public class TransferMoney
    {
        private readonly IAccountRepository _accountRepository;
        private readonly INotificationService _notificationService;

        public TransferMoney(IAccountRepository accountRepository, INotificationService notificationService)
        {
            _accountRepository = accountRepository;
            _notificationService = notificationService;
        }

        public void Execute(Guid fromAccountId, Guid toAccountId, decimal amount)
        {
            var from = _accountRepository.GetAccountById(fromAccountId);
            Account.AccountCheck(from);
            from.Withdraw(amount);

            var to = _accountRepository.GetAccountById(toAccountId);
            Account.AccountCheck(to);
            to.Deposit(amount);
            if (from.Balance < Account.NotificationLimit)
            {
                _notificationService.NotifyFundsLow(from.User.Email);
            }

            if (Account.PayInLimit - to.PaidIn < Account.PaidInLimit)
            {
                _notificationService.NotifyApproachingPayInLimit(to.User.Email);
            }

            _accountRepository.Update(from);
            _accountRepository.Update(to);
        }
    }
}
