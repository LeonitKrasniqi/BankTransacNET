using System.ComponentModel.DataAnnotations;

namespace AccountManagmentSystemAPI.Model.Dto
{
    public class AddTransferRequestDto
    {
        [Required]
        public Guid SenderAccountId { get; set; }
        [Required]
        public Guid ReceiverAccountId { get; set; }
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Amount cannot be 0 or negative.")]
        public decimal Amount { get; set; }
    }
}
