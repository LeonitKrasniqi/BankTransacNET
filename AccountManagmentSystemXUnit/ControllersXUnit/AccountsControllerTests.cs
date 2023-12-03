using AccountManagmentSystemAPI.Controllers;
using AccountManagmentSystemAPI.Data;
using AccountManagmentSystemAPI.Mappings;
using AccountManagmentSystemAPI.Model.Domain;
using AccountManagmentSystemAPI.Model.Dto;
using AccountManagmentSystemAPI.Repositories;
using AutoMapper;
using AutoMapper.Execution;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AccountManagmentSystemXUnit.ControllersXUnit
{
    public class AccountsControllerTests
    {
        [Fact]
        public async Task Create_Account_Success()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<FinancialDbContext>()
                              .UseInMemoryDatabase(databaseName: "TestDatabase")
                              .Options;

            var dbContextMock = new Mock<FinancialDbContext>(options);
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var mapperMock = new Mock<IMapper>();

 
            var controller = new AccountsController(dbContextMock.Object, accountRepositoryMock.Object, mapperMock.Object);

            accountRepositoryMock
                .Setup(repo => repo.CreateAsync(It.IsAny<Account>()))
                .ReturnsAsync((Account createdAccount) => createdAccount);

            mapperMock
                .Setup(mapper => mapper.Map<AccountDto>(It.IsAny<Account>()))
                .Returns((Account account) =>
                {
                    var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfiles>());
                    var mapper = mapperConfig.CreateMapper();
                    return mapper.Map<AccountDto>(account);
                });

            // Act
            var result = await controller.Create(new AddAccountRequestDto());

   
            // Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);


        }




        [Fact]
        public async Task Create_Account_Unsuccess()
        {
            // Arrange  
            var options = new DbContextOptionsBuilder<FinancialDbContext>()
                              .UseInMemoryDatabase(databaseName: "TestDatabase")
                              .Options;

            var dbContextMock = new Mock<FinancialDbContext>(options);
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var mapperMock = new Mock<IMapper>();

            var controller = new AccountsController(dbContextMock.Object, accountRepositoryMock.Object, mapperMock.Object);

            accountRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Account>()))
                                 .ThrowsAsync(new Exception("Simulated repository failure"));

            // Act
            var result = await controller.Create(new AddAccountRequestDto());

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);

            var objectResult = result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);

        }

       

        [Fact]
        public async Task CardNumber_UniqueConstraint_Test()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<FinancialDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var dbContext = new FinancialDbContext(options))
            {
                
                var uniqueCardNumber = 1543567;
                var uniqueAccount = new Account
                {
                    CardNumber = uniqueCardNumber,
                    IsDebit = true,
                    Balance = 100.0m
                };

                await dbContext.Accounts.AddAsync(uniqueAccount);
                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new FinancialDbContext(options))
            {
                // Act
                var duplicateCardNumber = 1543567;
                var duplicateAccount = new Account
                {
                    CardNumber = duplicateCardNumber,
                    IsDebit = true,
                    Balance = 200.0m
                };

                // Assert
                var isCardNumberUnique = await dbContext.Accounts.AllAsync(a => a.CardNumber != duplicateCardNumber);

                Assert.False(isCardNumberUnique);
            }
        }
       


        [Fact]
        public async Task GetAll_Success()
        {
            // Arrange

            var options = new DbContextOptionsBuilder<FinancialDbContext>()
                            .UseInMemoryDatabase(databaseName: "TestDatabase")
                            .Options;

            var dbContextMock = new Mock<FinancialDbContext>(options);
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var mapperMock = new Mock<IMapper>();

            var fakeAccounts = new List<Account>
            {
                new Account { AccountId = Guid.NewGuid(), CardNumber = 123456, IsDebit = true, Balance = 100.0m },
                new Account { AccountId = Guid.NewGuid(), CardNumber = 227652, IsDebit = true, Balance = 200.0m },
               
            };

            accountRepositoryMock.Setup(repo => repo.GetAllAsync())
                                .ReturnsAsync(fakeAccounts);

            mapperMock.Setup(mapper => mapper.Map<List<AccountDto>>(It.IsAny<List<Account>>()))
                      .Returns(new List<AccountDto>());

            var controller = new AccountsController(dbContextMock.Object, accountRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = await controller.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var objectResult = result as OkObjectResult;
            Assert.Equal(200, objectResult.StatusCode);

            var accountDtos = objectResult.Value as List<AccountDto>;
            Assert.NotNull(accountDtos);
            Assert.Empty(accountDtos);
        }


        [Fact]
        public async Task GetById_ExistingAccount_Success()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<FinancialDbContext>()
                            .UseInMemoryDatabase(databaseName: "TestDatabase")
                            .Options;

            var dbContextMock = new Mock<FinancialDbContext>(options);
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var mapperMock = new Mock<IMapper>();

            var accountId = Guid.NewGuid();
            var fakeAccount = new Account { AccountId = accountId, CardNumber = 123456, IsDebit = true, Balance = 100.0m };

            accountRepositoryMock.Setup(repo => repo.GetByIdAsync(accountId))
                                .ReturnsAsync(fakeAccount);

            mapperMock.Setup(mapper => mapper.Map<AccountDto>(It.IsAny<Account>()))
                      .Returns(new AccountDto());

            var controller = new AccountsController(dbContextMock.Object, accountRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = await controller.GetById(accountId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var objectResult = result as OkObjectResult;
            Assert.Equal(200, objectResult.StatusCode);

            var accountDto = objectResult.Value as AccountDto;
            Assert.NotNull(accountDto);
        }


        [Fact]
        public async Task GetById_NonexistentAccount_ReturnsNotFound()
        {
            // Arrange

            var options = new DbContextOptionsBuilder<FinancialDbContext>()
                            .UseInMemoryDatabase(databaseName: "TestDatabase")
                            .Options;

            var dbContextMock = new Mock<FinancialDbContext>(options);
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var mapperMock = new Mock<IMapper>();

            var nonExistentAccountId = Guid.NewGuid();

            // Configure repository mock to return null for a non-existent account
            accountRepositoryMock.Setup(repo => repo.GetByIdAsync(nonExistentAccountId))
                                .ReturnsAsync((Account)null);

            var controller = new AccountsController(dbContextMock.Object, accountRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = await controller.GetById(nonExistentAccountId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);

            var notFoundResult = result as NotFoundResult;
            Assert.Equal(404, notFoundResult.StatusCode);
        }


        [Fact]
        public async Task DeleteById_ExistingAccount_Success()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<FinancialDbContext>()
                            .UseInMemoryDatabase(databaseName: "TestDatabase")
                            .Options;

            var dbContextMock = new Mock<FinancialDbContext>(options);
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var mapperMock = new Mock<IMapper>();

            var accountId = Guid.NewGuid();
            var fakeAccount = new Account { AccountId = accountId, CardNumber = 123456, IsDebit = true, Balance = 100.0m };

            accountRepositoryMock.Setup(repo => repo.DeleteAsync(accountId))
                                .ReturnsAsync(fakeAccount);

            var controller = new AccountsController(dbContextMock.Object, accountRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = await controller.DeleteById(accountId);

            
            //Asserts
            Assert.NotNull(result);
            Assert.Equal(200, (result as OkObjectResult)?.StatusCode);
            Assert.Equal("Account was deleted", (result as OkObjectResult)?.Value);



        }


    }
}


