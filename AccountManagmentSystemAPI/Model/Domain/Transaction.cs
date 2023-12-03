using System.ComponentModel.DataAnnotations;

namespace AccountManagmentSystemAPI.Model.Domain
{
    public class Transaction
    {
        [Key]
        public Guid TransactionId { get; set; }
     
        public decimal Amount {  get; set; }
      
        public DateTime TransactionDate { get; set; }
      
        public bool IsDebit { get; set; }

        public Guid SenderId { get; set; }

        public Guid ReceiverId { get; set; }
       
       public virtual Account Account{ get; set; }

    }
}
