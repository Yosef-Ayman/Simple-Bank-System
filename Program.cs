using System;
using System.Collections.Generic;
using System.Linq;

public class Bank
{
    class Customer
    {
        public int AccountNumber;
        public int CountryID;
        public string Password;
        public string Name;
        public double Balance;
        public List<Transaction> Transactions = new List<Transaction>();

        public Customer(int accountNumber, int countryID, string name, string password, double balance)
        {
            AccountNumber = accountNumber;
            CountryID = countryID;
            Name = name;
            Password = password;
            Balance = balance;
        }
    }

    enum TransactionType
    {
        Deposit,
        Withdrawal
    }

    class Transaction
    {
        public TransactionType Type;
        public double Amount;
        public DateTime Date;
    }

    static List<Customer> customers = new List<Customer>();
    static int ID = 1;

    static void Main(string[] args)
    {
        int op = -1;

        do
        {
            try
            {
                DisplayMenu();

                if (!int.TryParse(Console.ReadLine(), out op))
                {
                    Console.WriteLine("Invalid input.");
                    Continue();
                    continue;
                }

                switch (op)
                {
                    case 1: AddCustomer(); break;
                    case 2: ViewCustomer(); break;
                    case 3: EditCustomerInfo(); break;
                    case 4: Deposit(); break;
                    case 5: Withdraw(); break;
                    case 6: ViewTransactions(); break;
                    case 7: DeleteCustomer(); break;
                    case 0: break;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong: " + ex.Message);
            }

            Continue();

        } while (op != 0);
    }

    static void Continue()
    {
        Console.WriteLine("\nPress any key...");
        Console.ReadKey();
    }

    static void DisplayMenu()
    {
        Console.Clear();
        Console.WriteLine("===== BANK SYSTEM =====");
        Console.WriteLine("1 - Add New Customer");
        Console.WriteLine("2 - View Customer Info");
        Console.WriteLine("3 - Edit Customer Info");
        Console.WriteLine("4 - Deposit");
        Console.WriteLine("5 - Withdraw");
        Console.WriteLine("6 - View Transaction History");
        Console.WriteLine("7 - Delete Customer");
        Console.WriteLine("0 - Exit");
        Console.Write("Choose: ");
    }

    static string Encrypt(string text)
    {
        char key = 'Y';
        char[] result = new char[text.Length];

        for (int i = 0; i < text.Length; i++)
            result[i] = (char)(text[i] ^ key);

        return new string(result);
    }

    static bool IsValidPassword(string password)
    {
        return password.Length == 4 && password.All(char.IsDigit);
    }

    static Customer FindCustomer()
    {
        try
        {
            Console.Write("Enter Account Number: ");
            if (!int.TryParse(Console.ReadLine(), out int acc))
            {
                Console.WriteLine("Invalid Account Number.");
                return null;
            }

            Customer customer = customers.FirstOrDefault(c => c.AccountNumber == acc);

            if (customer == null)
            {
                Console.WriteLine("Customer not found.");
                return null;
            }

            for (int i = 0; i < 3; i++)
            {
                Console.Write("Enter Password (4 digits): ");
                string pass = Console.ReadLine();

                if (Encrypt(pass) == customer.Password)
                    return customer;

                Console.WriteLine("Wrong password.");
            }

            Console.WriteLine("Account locked.");
            return null;
        }
        catch
        {
            Console.WriteLine("Error while finding customer.");
            return null;
        }
    }

    static void AddCustomer()
    {
        try
        {
            Console.Clear();

            Console.Write("Enter Country ID: ");
            if (!int.TryParse(Console.ReadLine(), out int countryID))
            {
                Console.WriteLine("Invalid Country ID.");
                return;
            }

            Console.Write("Enter Name: ");
            string name = Console.ReadLine();

            string password;
            do
            {
                Console.Write("Enter Password (4 digits only): ");
                password = Console.ReadLine();

                if (!IsValidPassword(password))
                    Console.WriteLine("Password must be exactly 4 digits!");
            }
            while (!IsValidPassword(password));

            password = Encrypt(password);

            Console.Write("Enter Initial Balance: ");
            if (!double.TryParse(Console.ReadLine(), out double balance))
            {
                Console.WriteLine("Invalid balance.");
                return;
            }
            
            customers.Add(new Customer(ID++, countryID, name, password, balance));

            Console.WriteLine("Customer created successfully!");

            Console.WriteLine($"Customer ID is : {ID-1} | You must save it, it is your card.");
        }
        catch
        {
            Console.WriteLine("Error while adding customer.");
        }
    }

    static void ViewCustomer()
    {
        try
        {
            Customer customer = FindCustomer();
            if (customer == null) return;

            Console.WriteLine("\n=== Customer Info ===");
            Console.WriteLine($"Account Number: {customer.AccountNumber}");
            Console.WriteLine($"Name: {customer.Name}");
            Console.WriteLine($"Balance: {customer.Balance}");
        }
        catch
        {
            Console.WriteLine("Error displaying customer.");
        }
    }

    static void EditCustomerInfo()
    {
        try
        {
            Console.Clear();
            Console.Write("Enter Account Number: ");
            if (!int.TryParse(Console.ReadLine(), out int accNumber))
            {
                Console.WriteLine("Invalid Account Number!");
                return;
            }

            Customer customer = customers.FirstOrDefault(c => c.AccountNumber == accNumber);
            if (customer == null)
            {
                Console.WriteLine("Customer not found.");
                return;
            }

            Console.WriteLine("1 - Change Balance");
            Console.WriteLine("2 - Change Password");
            Console.Write("Choose: ");
            if (!int.TryParse(Console.ReadLine(), out int op)) return;

            if (op == 1)
            {
                Console.Write("Enter new balance: ");
                if (double.TryParse(Console.ReadLine(), out double balance))
                {
                    customer.Balance = balance;
                    Console.WriteLine("Balance updated successfully!");
                }
                else
                {
                    Console.WriteLine("Invalid balance input!");
                }
            }
            else if (op == 2)
            {
                bool passwordChanged = false;

                for (int attempt = 0; attempt < 3; attempt++)
                {
                    Console.Write("Enter current password (or type 'forgot' to reset): ");
                    string input = Console.ReadLine();
    
                    if (input.ToLower() == "forgot")
                    {
                        Console.Write("Enter your Country ID for verification: ");
                        if (int.TryParse(Console.ReadLine(), out int countryID))
                        {
                            if (customer.CountryID == countryID)
                            {
                                Console.WriteLine("Verification successful! You can set a new password.");
                                string newPassword;
                                do
                                {
                                    Console.Write("Enter new password (4 digits): ");
                                    newPassword = Console.ReadLine();
                                    if (!IsValidPassword(newPassword))
                                        Console.WriteLine("Password must be exactly 4 digits!");
                                }
                                while (!IsValidPassword(newPassword));

                                customer.Password = Encrypt(newPassword);
                                passwordChanged = true;
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Country ID does not match!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Country ID input!");
                        }
                    }
                    else
                    {
                        if (Encrypt(input) == customer.Password)
                        {
                            string newPassword;
                            do
                            {
                                Console.Write("Enter new password (4 digits): ");
                                newPassword = Console.ReadLine();
                                if (!IsValidPassword(newPassword))
                                    Console.WriteLine("Password must be exactly 4 digits!");
                            }
                            while (!IsValidPassword(newPassword));

                            customer.Password = Encrypt(newPassword);
                            passwordChanged = true;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Wrong password.");
                        }
                    }
                }

                if (passwordChanged)
                    Console.WriteLine("Password updated successfully!");
                else
                    Console.WriteLine("Failed to change password after 3 attempts.");
            }
            else
            {
                Console.WriteLine("Invalid choice!");
            }
        }
        catch
        {
            Console.WriteLine("Error editing customer.");
        }
    }

    static void Deposit()
    {
        try
        {
            Customer customer = FindCustomer();
            if (customer == null) return;

            Console.Write("Enter amount: ");
            if (!double.TryParse(Console.ReadLine(), out double amount) || amount <= 0)
            {
                Console.WriteLine("Invalid amount.");
                return;
            }

            customer.Balance += amount;

            customer.Transactions.Add(new Transaction
            {
                Type = TransactionType.Deposit,
                Amount = amount,
                Date = DateTime.Now
            });

            Console.WriteLine("Deposit successful.");
        }
        catch
        {
            Console.WriteLine("Error during deposit.");
        }
    }

    static void Withdraw()
    {
        try
        {
            Customer customer = FindCustomer();
            if (customer == null) return;

            Console.Write("Enter amount: ");
            if (!double.TryParse(Console.ReadLine(), out double amount))
            {
                Console.WriteLine("Invalid amount.");
                return;
            }

            if (amount > customer.Balance)
            {
                Console.WriteLine("Insufficient balance.");
                return;
            }

            customer.Balance -= amount;

            customer.Transactions.Add(new Transaction
            {
                Type = TransactionType.Withdrawal,
                Amount = amount,
                Date = DateTime.Now
            });

            Console.WriteLine("Withdraw successful.");
        }
        catch
        {
            Console.WriteLine("Error during withdrawal.");
        }
    }

    static void ViewTransactions()
    {
        try
        {
            Customer customer = FindCustomer();
            if (customer == null) return;

            if (customer.Transactions.Count == 0)
            {
                Console.WriteLine("No transactions yet.");
                return;
            }

            Console.WriteLine("\n=== Transaction History ===");

            foreach (var t in customer.Transactions)
                Console.WriteLine($"{t.Type} - {t.Amount} - {t.Date}");
        }
        catch
        {
            Console.WriteLine("Error viewing transactions.");
        }
    }

    static void DeleteCustomer()
    {
        try
        {
            Customer customer = FindCustomer();
            if (customer == null) return;

            customers.Remove(customer);
            Console.WriteLine("Customer deleted successfully.");
        }
        catch
        {
            Console.WriteLine("Error deleting customer.");
        }
    }
}