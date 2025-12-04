using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubtitlesTranslator.Application.Interfaces;
using SubtitlesTranslator.Application.UseCases;
using SubtitlesTranslator.Data;
using SubtitlesTranslator.Infrastructure.Repositories;

namespace SubtitlesTranslator.Controllers {
    [Authorize]
    public class TransactionHistoryController : BaseController {

        private readonly GetUserTransactionsUseCase _getUserTransactions;

        public TransactionHistoryController(IUserContextService userContext,
            GetUserTransactionsUseCase getUserTransactions)
            : base(userContext)
        {
            _getUserTransactions = getUserTransactions;
        }

        public async Task<IActionResult> Index()
        {
            var userId = await _userContext.GetCurrentUserIdAsync();
            var transactions = await _getUserTransactions.ExecuteAsync(userId);
            return View(transactions);
        }
    }
}
