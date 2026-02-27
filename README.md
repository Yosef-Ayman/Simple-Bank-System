# Bank Management System (Console Application)

## Overview

The Bank Management System is a C# console-based application that
simulates basic banking operations. It allows managing customer accounts
securely while maintaining transaction history and ensuring data
validation.

------------------------------------------------------------------------

## Features

### 1. Add New Customer

-   Create a customer account with:
    -   Unique Account Number
    -   Country ID
    -   Name
    -   Encrypted 4-digit password
    -   Initial balance

### 2. View Customer Information

-   Display account number, name, and balance.
-   Requires password verification.

### 3. Edit Customer Information

-   Update account balance.
-   Change password after verification.
-   Reset password using Country ID (recovery mechanism).

### 4. Deposit Funds

-   Add money to customer balance.
-   Record transaction with type, amount, and date.

### 5. Withdraw Funds

-   Deduct money after checking sufficient balance.
-   Record withdrawal transaction.

### 6. View Transaction History

-   Display all deposits and withdrawals.
-   Show transaction type, amount, and date.

### 7. Delete Customer

-   Remove a customer account after verification.

------------------------------------------------------------------------

## Security Features

-   Passwords stored in encrypted form.
-   Enforced 4-digit numeric password policy.
-   Maximum of 3 login attempts before account lock.
-   Password recovery using Country ID verification.

------------------------------------------------------------------------

## Error Handling & Validation

-   Input validation for numeric values.
-   Exception handling to prevent crashes.
-   Clear error messages for invalid operations.
