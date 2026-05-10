using System.Transactions;
using AutoMapper;
using BLL.DTO;
using BLL.Services;
using Domain.Entities;
using Domain.Interfaces;

public class FinanceService : IFinanceService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public FinanceService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public IEnumerable<AccountDTO> GetAllAccounts()
    {
        var accounts = _uow.Accounts.GetAll();
        return _mapper.Map<IEnumerable<AccountDTO>>(accounts);
    }

    public IEnumerable<CategoryDTO> GetAllCategories()
    {
        var categories = _uow.Categories.GetAll();
        return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
    }

    public IEnumerable<TransactionDTO> GetTransactionHistory()
    {
        var transactions = _uow.Transactions.GetAll();
        return _mapper.Map<IEnumerable<TransactionDTO>>(transactions);
    }

    public IEnumerable<TransactionDTO> GetTransactionsByAccount(int accountId)
    {
        var transactions = _uow.Transactions.Find(t => t.AccountId == accountId);
        return _mapper.Map<IEnumerable<TransactionDTO>>(transactions);
    }

    public void CreateAccount(AccountDTO accountDto)
    {
        var account = _mapper.Map<Account>(accountDto);
        _uow.Accounts.Add(account);
        _uow.Save();
    }

    public void CreateCategory(CategoryDTO categoryDto)
    {
        var category = _mapper.Map<Category>(categoryDto);
        _uow.Categories.Add(category);
        _uow.Save();
    }

    public void MakeTransaction(TransactionDTO dto)
    {
        var account = _uow.Accounts.GetById(dto.AccountId);
        var category = _uow.Categories.GetById(dto.CategoryId);

        if (account == null || category == null)
            throw new Exception("Account or category not found");

        if (category.Type == TransactionType.Expense && account.Balance < dto.Amount)
        {
            throw new Exception("Not enough money on account!");
        }

        if (category.Type == TransactionType.Expense)
            account.Balance -= dto.Amount;
        else
            account.Balance += dto.Amount;

        var transaction = _mapper.Map<Domain.Entities.Transaction>(dto);
        transaction.Date = DateTime.Now;

        // 5. Зберігаємо все через Unit of Work
        _uow.Transactions.Add(transaction);
        _uow.Accounts.Update(account);
        _uow.Save();
    }

    public Dictionary<string, decimal> GetStatisticsByCategory(TransactionType type)
    {
        var transactions = _uow.Transactions.Find(t => t.Category.Type == type);
        return transactions
            .GroupBy(t => t.Category.Name)
            .ToDictionary(g => g.Key, g => g.Sum(t => t.Amount));
    }

    public Dictionary<string, decimal> GetStatisticsByAccount(int accID, TransactionType type)
    {
        var transactions = _uow.Transactions.Find(t => t.AccountId == accID && t.Category.Type == type);
        return transactions
            .GroupBy(t => t.Category.Name)
            .ToDictionary(a => a.Key, a => a.Sum(t => t.Amount));
    }
}
