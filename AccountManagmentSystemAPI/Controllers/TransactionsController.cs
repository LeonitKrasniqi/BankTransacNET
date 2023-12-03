using AccountManagmentSystemAPI.CustomActionFilter;
using AccountManagmentSystemAPI.Model.Domain;
using AccountManagmentSystemAPI.Model.Dto;
using AccountManagmentSystemAPI.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountManagmentSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionRepository transactionRepository;

        public TransactionsController(ITransactionRepository transactionRepository, IMapper mapper)
        {
            this.transactionRepository = transactionRepository;
        }


        [HttpPost("transfer")]
        [ValidateModel]
        public async Task<IActionResult> TransferMoney([FromBody] AddTransferRequestDto request)
        {
            try
            {
              var success = await transactionRepository.TransferMoneyAsync(request.SenderAccountId, request.ReceiverAccountId, request.Amount);
          

                if (success)
                {
                    return Ok(new { Message = "Money transferred successfully." });
                }
                else
                {
                    return BadRequest(new { Message = "Transfer failed. Check your input or account balances." });
                }
            }
            catch (Exception)
            {
              
                return StatusCode(500, new { Message = "An error occurred while processing the request." });
            }
        }
    }
}
