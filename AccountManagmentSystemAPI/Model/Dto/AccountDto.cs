using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AccountManagmentSystemAPI.Model.Dto
{
    public class AccountDto
    {
     
        public Guid AccountId { get; set; }

     
        public int CardNumber { get; set; }

       
        public bool IsDebit { get; set; }

        public decimal Balance { get; set; }

    }
}
