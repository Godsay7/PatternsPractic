using System.Text;
using BLL.DTO;
using BLL.Services;
using Domain.Entities;

public class ConsoleMenu
{
    private readonly IFinanceService _service;

    public ConsoleMenu(IFinanceService service) => _service = service;

    public void Show()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== FINANCE MANAGER ===");
            Console.WriteLine("1. Accounts Management");
            Console.WriteLine("2. Categories Management");
            Console.WriteLine("3. New Transaction");
            Console.WriteLine("4. Analysis & Reports");
            Console.WriteLine("0. Exit");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1": AccountSubMenu(); break;
                case "2": CategorySubMenu(); break;
                case "3": CreateTransaction(); break;
                case "4": ShowAnalytics(); break;
                case "0": return;
            }
        }
    }

    private void AccountSubMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("--- ACCOUNTS ---");
            Console.WriteLine("1. View All Accounts");
            Console.WriteLine("2. Create New Account");
            Console.WriteLine("0. Back to Main Menu");

            var choice = Console.ReadLine();
            if (choice == "0") break;

            if (choice == "1")
            {
                var accounts = _service.GetAllAccounts();
                foreach (var a in accounts)
                    Console.WriteLine($"ID: {a.Id} | Name: {a.Name} | Balance: {a.Balance} UAH");
                Console.WriteLine("\nPress any key...");
                Console.ReadKey();
            }
            else if (choice == "2")
            {
                Console.Write("Enter name: ");
                var name = Console.ReadLine();
                Console.Write("Initial balance: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal bal))
                {
                    _service.CreateAccount(new AccountDTO { Name = name, Balance = bal });
                    Console.WriteLine("Account created!");
                }
                Thread.Sleep(1000);
            }
        }
    }
    
    private void CategorySubMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("--- CATEGORIES ---");
            Console.WriteLine("1. View All Categories");
            Console.WriteLine("2. Create New Category");
            Console.WriteLine("0. Back to Main Menu");

            var choice = Console.ReadLine();
            if (choice == "0") break;

            if (choice == "1")
            {
                var categories = _service.GetAllCategories();
                foreach (var c in categories)
                    Console.WriteLine($"ID: {c.Id} | Name: {c.Name} | Type: {c.Type}");
                Console.WriteLine("\nPress any key...");
                Console.ReadKey();
            }
            else if (choice == "2")
            {
                Console.Write("Enter name: ");
                var name = Console.ReadLine();

                int typeIndex;

                while (true)
                {
                    Console.Write("Type (0 - Income, 1 - Expense): ");
                    string input = Console.ReadLine();

                    if (int.TryParse(input, out typeIndex) && (typeIndex == 0 || typeIndex == 1))
                    {
                        break;
                    }
                    Console.WriteLine("Invalid input! Please enter exactly 0 or 1.");
                }

                var type = (TransactionType)typeIndex;
                _service.CreateCategory(new CategoryDTO { Name = name, Type = type });
                Console.WriteLine("Category created!");
                Thread.Sleep(2000);
            }
        }
    }


    private void CreateTransaction()
    {
        Console.Clear();
        Console.WriteLine("--- NEW TRANSACTION ---");

        var accounts = _service.GetAllAccounts().ToList();
        if (!accounts.Any())
        {
            Console.WriteLine("No accounts found! Create an account first.");
            Thread.Sleep(2000);
            return;
        }

        foreach (var a in accounts)
        {
            Console.WriteLine($"[{a.Id}] {a.Name} (Bal: {a.Balance})");
        }

        int accId;
        while (true)
        {
            Console.Write("Select Account ID: ");
            if (int.TryParse(Console.ReadLine(), out accId) && accounts.Any(a => a.Id == accId))
            {
                break;
            }
            Console.WriteLine("Invalid ID! Please enter an existing Account ID.");
        }

        var categories = _service.GetAllCategories().ToList();
        if (!categories.Any())
        {
            Console.WriteLine("No categories found! Create a category first.");
            Thread.Sleep(2000);
            return;
        }

        foreach (var c in categories)
        {
            Console.WriteLine($"[{c.Id}] {c.Name} ({c.Type})");
        }

        int catId;
        while (true)
        {
            Console.Write("Select Category ID: ");
            if (int.TryParse(Console.ReadLine(), out catId) && categories.Any(c => c.Id == catId))
            {
                break;
            }
            Console.WriteLine("Invalid ID! Please enter an existing Category ID.");
        }

        decimal amount;
        while (true)
        {
            Console.Write("Enter Amount: ");
            if (decimal.TryParse(Console.ReadLine(), out amount) && amount > 0)
            {
                break;
            }
            Console.WriteLine("Invalid amount! Please enter a valid positive number.");
        }

        Console.Write("Description (optional): ");
        string desc = Console.ReadLine();

        try
        {
            _service.MakeTransaction(new TransactionDTO
            {
                AccountId = accId,
                CategoryId = catId,
                Amount = amount,
                Description = desc
            });
            Console.WriteLine("\nSuccess! Transaction recorded.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nERROR: {ex.Message}");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    private void ShowAnalytics()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("--- STATISTICS ---");
            Console.WriteLine("1. View statistics by category");
            Console.WriteLine("2. View statistics by account");
            Console.WriteLine("0. Back to Main Menu");

            var choice = Console.ReadLine();
            if (choice == "0") break;

            if (choice == "1")
            {
                var statsExpense = _service.GetStatisticsByCategory(TransactionType.Expense);
                var statsIncome = _service.GetStatisticsByCategory(TransactionType.Income);
                if (!statsExpense.Any() && !statsIncome.Any())
                {
                    Console.WriteLine("No transactions yet.");
                }
                else
                {
                    Console.WriteLine("All users' expenses for all time:");
                    foreach (var item in statsExpense)
                    {
                        Console.WriteLine($"{item.Key}: {item.Value} UAH");
                    }
                    Console.WriteLine("\nAll users' income for all time:");
                    foreach (var item in statsIncome)
                    {
                        Console.WriteLine($"{item.Key}: {item.Value} UAH");
                    }
                }
                Console.WriteLine("\nPress any key...");
                Console.ReadKey();
            }
            else if (choice == "2")
            {
                var accounts = _service.GetAllAccounts().ToList();
                if (!accounts.Any())
                {
                    Console.WriteLine("No accounts found! Create an account first.");
                    Thread.Sleep(2000);
                    return;
                }
                Console.Clear();
                foreach (var a in accounts)
                {
                    Console.WriteLine($"[{a.Id}] {a.Name} (Bal: {a.Balance})");
                }

                int accId;
                while (true)
                {
                    Console.Write("Select Account ID: ");
                    if (int.TryParse(Console.ReadLine(), out accId) && accounts.Any(a => a.Id == accId))
                    {
                        var statsByAccExp = _service.GetStatisticsByAccount(accId, TransactionType.Expense);
                        var statsByAccIn = _service.GetStatisticsByAccount(accId, TransactionType.Income);
                        Console.WriteLine("\nUser's expenses:"); 
                        foreach (var item in statsByAccExp)
                        {
                            Console.WriteLine($"{item.Key}: {item.Value} UAH");
                        }
                        Console.WriteLine("\nUser's incomes:");
                        foreach (var item in statsByAccIn)
                        {
                            Console.WriteLine($"{item.Key}: {item.Value} UAH");
                        }
                        Console.WriteLine("\nPress any key...");
                        Console.ReadKey();
                        break;
                    }
                    Console.WriteLine("Invalid ID! Please enter an existing Account ID.");
                }
            }
        }
    }
}