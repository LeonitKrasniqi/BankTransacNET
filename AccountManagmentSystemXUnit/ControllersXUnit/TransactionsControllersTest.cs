using AccountManagmentSystemAPI.Controllers;
using AccountManagmentSystemAPI.Data;
using AccountManagmentSystemAPI.Model.Domain;
using AccountManagmentSystemAPI.Model.Dto;
using AccountManagmentSystemAPI.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AccountManagmentSystemXUnit.ControllersXUnit
{
    public class TransactionsControllersTest
    {

        [Fact]
        public async Task TransferMoney_SuccessfulTransfer_ReturnsOk()
        {
            // Arrange
            var senderAccountId = Guid.NewGuid();
            var receiverAccountId = Guid.NewGuid();
            var amount = 50.0m;

            var request = new AddTransferRequestDto
            {
                SenderAccountId = senderAccountId,
                ReceiverAccountId = receiverAccountId,
                Amount = amount
            };

            var transactionRepositoryMock = new Mock<ITransactionRepository>();
            var mapperMock = new Mock<IMapper>();

            transactionRepositoryMock.Setup(repo => repo.TransferMoneyAsync(senderAccountId, receiverAccountId, amount))
                                     .ReturnsAsync(true);

            var controller = new TransactionsController(transactionRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = await controller.TransferMoney(request);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var objectResult = result as OkObjectResult;
            Assert.Equal(200, objectResult.StatusCode);

        }

        [Fact]
        public async Task TransferMoney_FailedTransfer_ReturnsBadRequest()
        {
            // Arrange
            var senderAccountId = Guid.NewGuid();
            var receiverAccountId = Guid.NewGuid();
            var amount = 50.0m;

            var request = new AddTransferRequestDto
            {
                SenderAccountId = senderAccountId,
                ReceiverAccountId = receiverAccountId,
                Amount = amount
            };

            var transactionRepositoryMock = new Mock<ITransactionRepository>();
            var mapperMock = new Mock<IMapper>();

            transactionRepositoryMock.Setup(repo => repo.TransferMoneyAsync(senderAccountId, receiverAccountId, amount))
                                     .ReturnsAsync(false);

            var controller = new TransactionsController(transactionRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = await controller.TransferMoney(request);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var objectResult = result as BadRequestObjectResult;
            Assert.Equal(400, objectResult.StatusCode);
        }

        [Fact]
        public async Task TransferMoney_InsufficientFunds_ReturnsFalse()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<FinancialDbContext>()
                       .UseInMemoryDatabase(databaseName: "TestDatabase")
                       .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                       .Options;


            using (var dbContext = new FinancialDbContext(options))
            {
                var senderAccountId = Guid.NewGuid();
                var receiverAccountId = Guid.NewGuid();
                var initialSenderBalance = 50.0m;
                var transferAmount = 75.0m;

                dbContext.Accounts.Add(new Account { AccountId = senderAccountId, Balance = initialSenderBalance });
                dbContext.Accounts.Add(new Account { AccountId = receiverAccountId, Balance = 100.0m });
                await dbContext.SaveChangesAsync();

                var transactionRepository = new SQLTransactionRepository(dbContext);

                // Act
                var transferResult = await transactionRepository.TransferMoneyAsync(senderAccountId, receiverAccountId, transferAmount);

                // Assert
                Assert.False(transferResult);

                var updatedSenderBalance = dbContext.Accounts.Single(a => a.AccountId == senderAccountId).Balance;
                var updatedReceiverBalance = dbContext.Accounts.Single(a => a.AccountId == receiverAccountId).Balance;

                Assert.Equal(initialSenderBalance, updatedSenderBalance);
                Assert.Equal(100.0m, updatedReceiverBalance);
            }
        }


    }
}
