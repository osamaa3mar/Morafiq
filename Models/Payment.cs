using System;
using System.ComponentModel.DataAnnotations;

namespace _Morafiq.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required(ErrorMessage = "Order ID is required")]
        public int OrderId { get; set; }

        public Order Order { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }


        [Required(ErrorMessage = "Transaction date is required")]
        [DataType(DataType.DateTime)]
        public DateTime TransactionDate { get; set; }
    }
}