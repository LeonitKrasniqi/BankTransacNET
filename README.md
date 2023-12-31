# BankTransacNET
This Bank Account Management API is developed with .NET 7.0 and leverages MSSQL for data management.
It provides essential features for creating, retrieving, and deleting bank accounts, along with the capability to create multiple accounts and facilitate money transfers between them.
Please note that this project is designed as a Web API.


## Usage


### Unique Account ID:
Every bank account created through the API is assigned a unique Account ID. 

When retrieving account details or performing transactions, always refer to the specific Account ID to ensure accuracy.

### Balance Requirements:
When registering a new account, make sure to provide a positive initial balance.

Negative balances or a balance of zero are not allowed during account registration.

### NumberCard Uniqueness:
Each bank account is associated with a numberCard, serving as a unique identifier for the account.

During the account creation process, ensure that the provided numberCard is unique and not associated with any existing account.

Attempting to register an account with a numberCard already in use will result in an error.

### Money Transfer Validation
When initiating a money transfer, ensure that the following conditions are met:
The sender's account balance is sufficient for the transfer amount.

The receiver's Account ID is valid and exists.

The sender and receiver Account IDs are different.

The sender is not attempting to transfer money to themselves.



## Unit Testing

This project includes unit tests implemented using Xunit and Moq frameworks. The tests cover various methods to ensure the reliability and correctness of the code. To run the tests, follow these steps:

1. Open the solution in your preferred IDE.

2. Locate the test project ("AccountManagmentSystemXUnit") in the solution explorer.

3. Run the tests using the testing tools provided by your IDE.

4. Review the test results to ensure all tests pass successfully.

Feel free to extend the test coverage or add additional tests as needed for your specific use cases.
