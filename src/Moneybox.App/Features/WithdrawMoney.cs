using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using System;

namespace Moneybox.App.Features
{
    public class WithdrawMoney
    {
        private readonly IAccountRepository _accountRepository;
        private readonly INotificationService _notificationService;

        public WithdrawMoney(IAccountRepository accountRepository, INotificationService notificationService)
        {
            _accountRepository = accountRepository;
            _notificationService = notificationService;
        }

        public void Execute(Guid fromAccountId, decimal amount)
        {
            var from = _accountRepository.GetAccountById(fromAccountId);
            Account.AccountCheck(from);

            from.Withdraw(amount);
            if (from.Balance < Account.NotificationLimit)
            {
                _notificationService.NotifyFundsLow(from.User.Email);
            }

            _accountRepository.Update(from);
        }
    }
}
