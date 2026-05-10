using System.Transactions;
using AutoMapper;
using BLL.DTO;
using BLL.Services;
using Domain.Entities;
using Domain.Interfaces;
using Transaction = Domain.Entities.Transaction;

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
        var transactions = _uow.Transactions.GetAll().ToList();
        var dtos = _mapper.Map<List<TransactionDTO>>(transactions);

        foreach (var dto in dtos)
        {
            dto.AccountName = _uow.Accounts.GetById(dto.AccountId)?.Name ?? "Unknown";
            dto.CategoryName = _uow.Categories.GetById(dto.CategoryId)?.Name ?? "Unknown";
        }

        return dtos;
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

    public void UpdateAccount(int id, string newName)
    {
        var account = _uow.Accounts.GetById(id);
        if (account == null)
            throw new Exception($"Account with ID {id} not found.");

        if (string.IsNullOrWhiteSpace(newName))
            throw new Exception("Account name cannot be empty.");

        account.Name = newName;

        _uow.Accounts.Update(account);
        _uow.Save();
    }

    public void UpdateCategory(int id, string newName)
    {
        var category = _uow.Categories.GetById(id);
        if (category == null)
            throw new Exception($"Category with ID {id} not found.");

        if (string.IsNullOrWhiteSpace(newName))
            throw new Exception("Category name cannot be empty.");

        category.Name = newName;

        _uow.Categories.Update(category);
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

        var transaction = _mapper.Map<Transaction>(dto);
        transaction.Date = DateTime.Now;

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

    public void TransferFunds(TransferDTO dto)
    {
        // Базові перевірки
        if (dto.FromAccountId == dto.ToAccountId)
            throw new Exception("Cannot transfer funds to the same account.");

        if (dto.Amount <= 0)
            throw new Exception("Transfer amount must be greater than zero.");

        // Отримуємо рахунки
        var fromAccount = _uow.Accounts.GetById(dto.FromAccountId);
        var toAccount = _uow.Accounts.GetById(dto.ToAccountId);

        if (fromAccount == null || toAccount == null)
            throw new Exception("One or both accounts not found.");

        // Перевірка балансу
        if (fromAccount.Balance < dto.Amount)
            throw new Exception($"Insufficient funds on account '{fromAccount.Name}'.");

        // 1. Створюємо транзакцію списання
        var expenseTx = new Transaction
        {
            AccountId = dto.FromAccountId,
            CategoryId = dto.ExpenseCategoryId,
            Amount = dto.Amount,
            Date = DateTime.Now,
            Description = string.IsNullOrWhiteSpace(dto.Description)
                ? $"Transfer to {toAccount.Name}"
                : dto.Description
        };

        // 2. Створюємо транзакцію зарахування
        var incomeTx = new Transaction
        {
            AccountId = dto.ToAccountId,
            CategoryId = dto.IncomeCategoryId,
            Amount = dto.Amount,
            Date = DateTime.Now,
            Description = string.IsNullOrWhiteSpace(dto.Description)
                ? $"Transfer from {fromAccount.Name}"
                : dto.Description
        };

        // 3. Оновлюємо фізичні баланси рахунків
        fromAccount.Balance -= dto.Amount;
        toAccount.Balance += dto.Amount;

        // 4. Додаємо все в репозиторії
        _uow.Transactions.Add(expenseTx);
        _uow.Transactions.Add(incomeTx);
        _uow.Accounts.Update(fromAccount);
        _uow.Accounts.Update(toAccount);

        // 5. Зберігаємо всі 4 дії (2 інсерти, 2 апдейти) ОДНІЄЮ транзакцією в БД
        _uow.Save();
    }
}
