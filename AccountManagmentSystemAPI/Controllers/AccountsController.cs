using AccountManagmentSystemAPI.CustomActionFilter;
using AccountManagmentSystemAPI.Data;
using AccountManagmentSystemAPI.Model.Domain;
using AccountManagmentSystemAPI.Model.Dto;
using AccountManagmentSystemAPI.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace AccountManagmentSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly FinancialDbContext dbContext;
        private readonly IAccountRepository accountRepository;
        private readonly IMapper mapper;

        public AccountsController(FinancialDbContext dbContext, IAccountRepository accountRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.accountRepository = accountRepository;
            this.mapper = mapper;
        }

        [HttpPost]
        [ValidateModel] 
        public async Task<IActionResult> Create([FromBody] AddAccountRequestDto addAccountRequestDto)
        {
            try
            {
                var accountDomainModel = mapper.Map<Account>(addAccountRequestDto);

                accountDomainModel = await accountRepository.CreateAsync(accountDomainModel);

                var accountDto = mapper.Map<AccountDto>(accountDomainModel);
                return CreatedAtAction(nameof(GetById), new { id = accountDto.AccountId }, accountDto);
            }
            catch (Exception ex)
            {
             
                return StatusCode(500, new { Message = $"An error occurred while processing the request. {ex.Message}" });
            }
        }




        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var accountsDomain = await accountRepository.GetAllAsync();

                return Ok(mapper.Map<List<AccountDto>>(accountsDomain));
            }
            catch (Exception ex)
            {
            
                return StatusCode(500, new { Message = $"An error occurred while processing the request. {ex.Message}" });
            }
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            try
            {
                var accountDomain = await accountRepository.GetByIdAsync(id);

                if (accountDomain == null)
                {
                    return NotFound();
                }

                return Ok(mapper.Map<AccountDto>(accountDomain));
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, new { Message = $"An error occurred while processing the request. {ex.Message}" });
            }
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            try
            {
                var accountDomain = await accountRepository.DeleteAsync(id);

                if (accountDomain == null)
                {
                    return BadRequest();
                }

                return Ok("Account was deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred while processing the request. {ex.Message}" });
            }
        }



    }
}
