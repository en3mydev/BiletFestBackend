// OrderController.cs
using BiletFest.Models;
using BiletFest.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly BiletFestServices _biletFestServices;

    public OrderController(BiletFestServices biletFestServices)
    {
        _biletFestServices = biletFestServices;
    }

    [HttpGet("GetAllOrders")]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _biletFestServices.GetOrders();
        return Ok(orders);
    }

    [HttpPost("CreateOrder")]
    public async Task<IActionResult> CreateOrder([FromBody] OrderRequest orderRequest)
    {
        try
        {
            var order = new Order
            {
                FirstName = orderRequest.FirstName,
                LastName = orderRequest.LastName,
                Email = orderRequest.Email,
                Phone = orderRequest.Phone,
                TotalPrice = orderRequest.TotalPrice,
                DiscountedPrice = orderRequest.DiscountedPrice,
                HasVoucher = orderRequest.HasVoucher,
                CreatedAt = DateTime.Now
            };

            // Mapping ticketCodes
            var ticketCodes = orderRequest.TicketCodes.ToDictionary(
                t => t.TicketID,
                t => t.Codes
            );

            var result = await _biletFestServices.CreateOrder(order, ticketCodes);
            if (result)
            {
                return Ok(new { message = "Order created successfully" });
            }
            else
            {
                return BadRequest(new { message = "Failed to create order" });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An internal server error occurred.", detail = ex.Message });
        }
    }


}