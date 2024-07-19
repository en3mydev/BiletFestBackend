using System.ComponentModel.DataAnnotations;

namespace BiletFest.Models
{
    public class OrderRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public bool HasVoucher { get; set; }
        public List<TicketCode> TicketCodes { get; set; }
    }

    public class TicketCode
    {
        public int TicketId { get; set; }
        public List<string> Codes { get; set; }
    }
}
