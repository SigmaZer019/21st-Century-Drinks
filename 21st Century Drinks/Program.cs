using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _21st_Century_Drinks
{
    internal class Program
    {
        static string[] items = { "Iced Coffee", "Milk Tea", "Fruit Shake", "Lemonade", "Ice Tea" };
        static decimal[] prices = { 89.00m, 99.00m, 79.00m, 59.00m, 49.00m };
        const int lowStockThreshold = 2;
        static int[] inventory = { 10, 10, 10, 10, 10 }; // Initial stock for each item
        static LinkedList<int> cart = new LinkedList<int>();
        static Stack<int> undoStack = new Stack<int>();
        static Queue<string> customerQueue = new Queue<string>();

        static decimal total = 0.00m;
        static decimal grandTotalSales = 0.00m;

        static Dictionary<string, string> cashierAccounts = new Dictionary<string, string>()
        {
            { "cashier1", "1234" },
            { "cashier2", "5678" },
            { "cashier3", "abcd" }
        };

        static string loggedInCashier = "";
       

        static void Main(string[] args)
        {

           
            while (true)
            {
                ShowLogin();
                while (true)
                {
                    ShowDashboard();
                    ConsoleKeyInfo key = Console.ReadKey(true);


                    if (key.KeyChar >= '1' && key.KeyChar <= '5')
                    {
                        int index = key.KeyChar - '1';
                        if (inventory[index] > cart.Count(x => x == index))
                        {
                            cart.AddLast(index);
                            undoStack.Push(index);
                            total += prices[index];
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"\nSorry, {items[index]} is out of stock.");
                            Console.ResetColor();
                            Thread.Sleep(1000);
                        }
                    }
                    // Mark the start of the bottom dashboard section

                    else if (char.ToUpper(key.KeyChar) == 'A')
                    {
                        if (cart.Count == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nNo items in the cart to checkout.");
                            Console.ReadKey();
                            Console.ResetColor();
                            continue;
                        }

                        // Enqueue a new customer ID for the transaction
                        customerQueue.Enqueue($"{DateTime.Now:HHmmss}");

                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("Enter payment amount: ");
                        Console.ResetColor();

                        string input = "";
                        decimal payment = 0.00m;
                        while (true)
                        {
                            ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                            if (keyInfo.Key == ConsoleKey.Enter)
                            {
                                if (decimal.TryParse(input, out payment))
                                {
                                    if (payment >= total)
                                    {

                                        // Receipt
                                        foreach (int i in cart)
                                        {
                                            inventory[i]--;
                                        }
                                        PrintReceipt(payment, payment - total);
                                        grandTotalSales += total;
                                        cart.Clear();
                                        undoStack.Clear();
                                        total = 0.00m;
                                        Console.ResetColor();
                                        break;
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("\n\n Insufficient payment. Try again.");
                                        Console.ResetColor();
                                        Thread.Sleep(700);
                                        break;
                                    }
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\n\n Invalid input. Payment must be a number.");
                                    Console.ResetColor();
                                    Thread.Sleep(700);
                                    break;


                                }
                            }
                            else if (char.IsDigit(keyInfo.KeyChar) || keyInfo.KeyChar == '.' && !input.Contains('.'))
                            {
                                input += keyInfo.KeyChar;
                                Console.Write(keyInfo.KeyChar);

                                if (decimal.TryParse(input, out payment))
                                {
                                    if (payment >= total)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Green;
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                    }

                                    Console.Write($"\rEnter payment amount: {input}   ");
                                    Console.ResetColor();
                                }
                            }
                            else if (keyInfo.Key == ConsoleKey.Backspace && input.Length > 0)
                            {
                                input = input.Substring(0, input.Length - 1);
                                Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r"); // Clear line
                                Console.Write("Enter payment amount: " + input);
                            }
                        }
                    }
                    else if (char.ToUpper(key.KeyChar) == 'B')
                    {
                        UndoLastAdd();
                    }
                    else if (char.ToUpper(key.KeyChar) == 'C')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        if (ConfirmAction("Clear all items in cart? (Y/N): "))
                        {
                            cart.Clear();
                            total = 0.00m;
                        }
                    }
                    else if (char.ToUpper(key.KeyChar) == 'D')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        if (ConfirmAction("Are you sure you want to logout? (Y/N): "))
                        {
                            Console.WriteLine();
                            Console.WriteLine($"\nTotal Sales Today: Php {grandTotalSales:0.00}");
                            Console.WriteLine();
                            Console.Write("\nLogging out");

                            cart.Clear();
                            total = 0.00m;
                            break; // Ends inner loop and goes back to login
                        }
                    }
                    else if (char.ToUpper(key.KeyChar) == 'E')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        if (ConfirmAction("Are you sure you want to exit? (Y/N): "))
                        {
                            Console.ResetColor();
                            ExitApp();
                            return;
                        }
                    }
                    else if (char.ToUpper(key.KeyChar) == 'R')
                    {
                        Console.Write("\nEnter item number to remove: ");
                        if (int.TryParse(Console.ReadLine(), out int itemNum) && itemNum >= 1 && itemNum <= items.Length)
                        {
                            int index = itemNum - 1;
                            var node = cart.LastOrDefault(x => x == index);
                            if (cart.Contains(index))
                            {
                                cart.Remove(node);
                                total -= prices[index];
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"{items[index]} removed from cart.");
                                Thread.Sleep(1000);
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Item not found in cart.");
                                Thread.Sleep(1000); // Wait 1 second before refreshing
                            }
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nInvalid key! Please choose a valid option from the dashboard.");
                        Console.ResetColor();
                        Thread.Sleep(1000); // Wait 1 second before refreshing
                    }
                }
            }
        }

        static bool ConfirmAction(string message)
        {
            Console.Write($"\n{message}");
            ConsoleKey key = Console.ReadKey(true).Key;
            return key == ConsoleKey.Y;
        }

        static void ShowLogin()
        {
            string username, password;
            int attempts = 3;

            while (attempts > 0)
            {
                Console.Clear();
                DrawHeader("LOGIN - 21ST CENTURY DRINKS POS");

                customerQueue.Enqueue($"{DateTime.Now:HHmmss}"); // simulated unique ID

                Console.Write(" Username: ");
                username = Console.ReadLine();

                Console.Write(" Password: ");
                password = Console.ReadLine();

                if (cashierAccounts.ContainsKey(username) && cashierAccounts[username] == password)
                {
                    loggedInCashier = username;
                    Console.WriteLine($"\n Login successful! Welcome, {loggedInCashier}. Press any key to continue...");
                    Console.ReadKey();
                    return;
                }
                else
                {
                    attempts--;
                    Console.WriteLine($"\nInvalid credentials. Attempts left: {attempts}");
                    Console.ReadKey();
                }
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nToo many failed attempts.");
            Console.WriteLine("Try again later. Exiting program...");
            Console.ResetColor();
            Thread.Sleep(2000);
            Environment.Exit(0);
        }

        static void ShowDashboard()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("============================================================");
            Console.Write(" 21ST CENTURY DRINKS POS");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\t     Cashier: {loggedInCashier}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("============================================================\n");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" No.      ITEM           PRICE         QTY        STATUS");
            Console.WriteLine("────────────────────────────────────────────────────────────");
            Console.ResetColor();

            for (int i = 0; i < items.Length; i++)
            {
                int qty = cart.Count(x => x == i);
                int remaining = inventory[i] - qty;

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($" {i + 1}.    {items[i],-14}  Php {prices[i],6:0.00}     {qty,3}      ");

                // Set status color based on remaining stock
                if (remaining <= 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Out of stock");
                }
                else if (remaining <= lowStockThreshold)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{remaining}stock left");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{remaining} in stock");
                }

                Console.ResetColor();
                Console.WriteLine("────────────────────────────────────────────────────────────");
            }

            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Total payment: {total:0.00} Php");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("============================================================");
            Console.WriteLine("[A] Checkout  [B] Undo last add  [R] Remove specific item\n[C] Clear     [D] Logout         [E] Exit");
            Console.WriteLine("============================================================");
            Console.Write("Press 1-5 to add item instantly, or A/B/C/D/E: ");
        }
        static void UndoLastAdd()
        {
            if (undoStack.Count > 0)
            {
                int lastItem = undoStack.Pop();
                cart.Remove(cart.LastOrDefault(x => x == lastItem));
                total -= prices[lastItem];
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n{items[lastItem]} removed from cart.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nNo item to undo.");
            }
            Console.ResetColor();
            Thread.Sleep(1000);
        }
       
        static void PrintReceipt(decimal payment, decimal change)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("========================================");
            Console.WriteLine("                 RECEIPT");
            Console.WriteLine("========================================");
            Console.ResetColor();

            if (customerQueue.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Cashier: {loggedInCashier}\t" + $"Date: {DateTime.Now:yyyy-MM-dd}" +
                                  $"\n                        Time: {DateTime.Now:hh:mm:ss tt}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"\nCustomer ID: {customerQueue.Dequeue()}");
                Console.WriteLine("\n────────────────────────────────────────");
                Console.WriteLine("Please review your order:\n");

                for (int i = 0; i < items.Length; i++)
                {
                    int qty = cart.Count(x => x == i);
                    if (qty > 0)
                    {
                        decimal itemTotal = qty * prices[i];
                        Console.WriteLine($"{items[i],-15}{prices[i],6:0.00} x {qty} = Php {itemTotal:0.00}");
                    }
                }

                Console.WriteLine("────────────────────────────────────────");
                Console.WriteLine($"Subtotal:     Php {total:0.00}");
                Console.WriteLine($"Payment:      Php {payment:0.00}");
                Console.WriteLine($"Change:       Php {change:0.00}");
                Console.WriteLine("────────────────────────────────────────");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Thank you for your purchase!");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("========================================");
                Console.WriteLine("Press any key to return to the dashboard...");
                Console.ReadKey();
            }
        }

        static void ExitApp()
        {
            Console.Clear();
            Console.WriteLine("Exiting... Have a nice day!");
        }

        static void DrawHeader(string title)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("========================================");
            Console.WriteLine($" {title}");
            Console.WriteLine("========================================\n");
            Console.ResetColor();
        }

    }
}
