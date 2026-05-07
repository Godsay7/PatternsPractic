using BLL.DTO;

namespace BLL.Services
{
    public interface IFinanceService
    {
        IEnumerable<AccountDTO> GetAllAccounts();
        IEnumerable<CategoryDTO> GetAllCategories();
        IEnumerable<TransactionDTO> GetTransactionHistory();

        void CreateAccount(AccountDTO accountDto);
        void CreateCategory(CategoryDTO categoryDto);
        void MakeTransaction(TransactionDTO transactionDto);
    }
}
