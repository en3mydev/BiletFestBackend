using System;
using System.ComponentModel.DataAnnotations;


namespace BiletFest.Models
{
    public class User
    {
        public int UserID { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }

        [Required]
        public string Role { get; set; }
    }

    public class Festival
    {
        public int FestivalID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Location { get; set; }

        [Required]
        public string Date { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; } 
        public int Price { get; set; } 
        public double Rating { get; set; } 

        public string Link { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }

    public class Ticket
    {
        public int TicketID { get; set; }
        public int FestivalID { get; set; }

        [Required]
        [StringLength(50)]
        public string TicketType { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int AvailableQuantity { get; set; }

        public string Description { get; set; }

    }


    public class Order
    {
        public int OrderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        public double TotalPrice { get; set; }
        public double? DiscountedPrice { get; set; }
        public bool HasVoucher { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<OrderTicket> OrderTickets { get; set; } = new List<OrderTicket>();
    }

    public class OrderTicket
    {
        public int OrderTicketId { get; set; }
        public int OrderId { get; set; }
        public int TicketId { get; set; }
        public string UniqueCode { get; set; }
        public Order Order { get; set; }

        public Ticket Ticket { get; set; }
    }

    public class Voucher
    {
        public int VoucherId { get; set; }
        public string VoucherCode { get; set; }
        public int VoucherDiscount { get; set; }

    }


    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class CreateUserRequest
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
    }

    public class UpdatePhoneRequest
    {
        public string Phone { get; set; }
    }

}
