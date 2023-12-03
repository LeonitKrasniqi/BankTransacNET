using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AccountManagmentSystemAPI.Model.Domain
{
    public class Account
    {
        [Key]
        public Guid AccountId { get; set; }


        public int CardNumber {  get; set; }

        public bool IsDebit { get; set; }

        public decimal Balance {  get;  set; }

        [JsonIgnore]
       public ICollection<Transaction> Transactions { get; set; }
   


    }
}
