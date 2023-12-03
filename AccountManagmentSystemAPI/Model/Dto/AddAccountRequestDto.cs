using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace AccountManagmentSystemAPI.Model.Dto
{
    public class AddAccountRequestDto
    {
   
        [Required]
        [Range(1000000, 9999999, ErrorMessage= "CardNumber must be a 7-digit number")]
        public int CardNumber { get; set; }

        [Required]
        public bool IsDebit { get; set; }
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Balance cannot be 0 or negative.")]
        public decimal Balance { get; set; }

    }
}
