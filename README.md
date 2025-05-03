# 21st Century Drinks - Console POS System

A simple and beginner-friendly **Point of Sale (POS)** system for a fictional drinks shop called **"21st Century Drinks"**, created using **C# (Console Application)**. This program simulates a basic cashier interface that handles login, order selection, and checkout.

## Features

- Cashier login system (with multiple accounts)
- Item selection via keypress (1–5)
- Live cart tracking and total computation
- Undo last added item
- Clear cart
- Checkout and receipt generation
- Logout and Exit options
- Console-based UI with color coding for better visibility

## How to Use

1. **Compile and run** the program in any C# compatible IDE (e.g., Visual Studio).
2. You'll be prompted with a **Login Screen**.
   - Use any of the default accounts:
     - `cashier1 / 1234`
     - `cashier2 / 5678`
     - `cashier3 / abcd`
3. Once logged in, you'll be redirected to the **Dashboard** where:
   - You can press `1–5` to add drinks to your cart.
   - Press:
     - `A` to checkout,
     - `B` to undo the last added item,
     - `C` to clear the cart,
     - `D` to logout,
     - `E` to exit the program.
4. Upon checkout, a receipt is shown and the cart is reset.

## Program Structure

### Main Components:
- `items[]` and `prices[]`: Arrays holding drink names and prices.
- `cart`: A list tracking selected item indices.
- `total`: Running total of the cart.
- `cashierAccounts`: Dictionary holding predefined login credentials.

### Key Methods:
- `Main()`: Entry point, manages login and dashboard loop.
- `ShowLogin()`: Handles user authentication.
- `ShowDashboard()`: Displays available options and item status.
- `ConfirmAction(string)`: Utility to prompt for user confirmation.
- `UndoLastAdd()`: Removes the last item added to the cart.
- `Checkout()`: Finalizes the purchase and shows a receipt.
- `ExitApp()`: Displays a goodbye message and closes the app.
- `DrawHeader(string)`: Displays section titles with styling.

## Limitations / Notes

- No persistence (data resets every run).
- No input validation for non-supported keys beyond menu options.
- Console UI only.

## Author

This project is proudly created by ***Zcyn Milan, Ken Jorique, and Jerry Dave Quintana***.

> Feel free to use, modify, and learn from this code! Just don't forget to credit the original author when sharing.

---
