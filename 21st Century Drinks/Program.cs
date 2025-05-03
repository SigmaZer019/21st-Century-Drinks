using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _21st_Century_Drinks
{
    internal class Program
    {
        static string[] items = { "Iced Coffee", "Milk Tea", "Fruit Shake", "Lemonade", "Ice Tea" };
        static double[] prices = { 89.00, 99.00, 79.00, 59.00, 49.00 };
        static List<int> cart = new List<int>();
        static double total = 0.00;

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
                        cart.Add(index);
                        total += prices[index];
                    }
                    else if (char.ToUpper(key.KeyChar) == 'A')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        if (cart.Count == 0)
                        {
                            Console.WriteLine("\n\nCannot proceed to checkout. Your cart is empty.");
                            Console.ReadKey();
                        }
                        else if (ConfirmAction("Proceed to checkout? (Y/N): "))
                        {
                            Checkout();
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
                            total = 0.00;
                        }
                    }
                    else if (char.ToUpper(key.KeyChar) == 'D')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        if (ConfirmAction("Are you sure you want to logout? (Y/N): "))
                        {
                            Console.Write("\nLogging out");
                            for (int i = 0; i < 3; i++)
                            {
                                Thread.Sleep(500);
                                Console.Write(".");
                            }
                            Thread.Sleep(500);
                            loggedInCashier = "";
                            cart.Clear();
                            total = 0.00;
                            break; // Ends inner loop and goes back to login
                        }
                    }
                    else if (char.ToUpper(key.KeyChar) == 'E')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        if (ConfirmAction("Are you sure you want to exit? (Y/N): "))
                        {
                            ExitApp();
                            return;
                        }
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
            Console.WriteLine("========================================================");
            Console.Write(" 21ST CENTURY DRINKS POS");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\t     Cashier: {loggedInCashier}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("========================================================\n");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.ResetColor();
            Console.WriteLine(" No.      ITEM           PRICE         QTY       STATUS");
            Console.WriteLine("────────────────────────────────────────────────────────");

            for (int i = 0; i < items.Length; i++)
            {
                int qty = cart.FindAll(x => x == i).Count;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($" {i + 1}.    {items[i],-14}  Php {prices[i],6:0.00}     {qty,3}      Available");
                Console.WriteLine("────────────────────────────────────────────────────────");
            }

            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Total payment: {total:0.00} Php");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("========================================================");
            Console.WriteLine("[A] Checkout  [B] Undo last add  [C] Clear  [D] Logout  [E] Exit");
            Console.WriteLine("========================================================");
            Console.Write("Press 1-5 to add item instantly, or A/B/C/D/E: ");
        }

        static void UndoLastAdd()
        {
            if (cart.Count > 0)
            {
                int lastItem = cart[cart.Count - 1];
                total -= prices[lastItem];
                cart.RemoveAt(cart.Count - 1);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nLast item removed from cart.");
                Console.ResetColor();
                Thread.Sleep(1000);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nNo item to undo.");
                Console.ResetColor();
                Thread.Sleep(1000);
            }
        }

        static void Checkout()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("========================================");
            Console.WriteLine("             RECEIPT");
            Console.WriteLine("========================================");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Cashier: {loggedInCashier}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("────────────────────────────────────────");

            for (int i = 0; i < items.Length; i++)
            {
                int qty = cart.FindAll(x => x == i).Count;
                if (qty > 0)
                {
                    double itemTotal = qty * prices[i];
                    Console.WriteLine($"{items[i],-15} x {qty} = Php {itemTotal:0.00}");
                }
            }

            Console.WriteLine("────────────────────────────────────────");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"TOTAL: Php {total:0.00}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("========================================");
            Console.WriteLine("Thank you for your purchase!");
            Console.WriteLine();
            Console.WriteLine("Press any key to return to menu...");
            Console.ReadKey();

            cart.Clear();
            total = 0.00;
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
