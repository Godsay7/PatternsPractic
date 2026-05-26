using BLL.DTO;
using Domain.Entities;

namespace BLL.Services
{
    public interface IFinanceService
    {
        IEnumerable<AccountDTO> GetAllAccounts();
        IEnumerable<CategoryDTO> GetAllCategories();
        IEnumerable<TransactionDTO> GetTransactionHistory();

        void CreateAccount(AccountDTO accountDto);
        void CreateCategory(CategoryDTO categoryDto);
        void UpdateAccount(int id, string newName);
        void UpdateCategory(int id, string newName);
        void DeleteAccount(int id);
        void DeleteCategory(int id);
        void MakeTransaction(TransactionDTO transactionDto);
        void TransferFunds(TransferDTO dto);

        Dictionary<string, decimal> GetStatisticsByCategory(TransactionType type);
        Dictionary<string, decimal> GetStatisticsByAccount(int accID, TransactionType type);
        
    }
}
