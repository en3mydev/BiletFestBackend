using BiletFest.Models;
using BiletFest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BiletFest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FestivalController : ControllerBase
    {
        private readonly BiletFestServices _biletFestServices;

        public FestivalController(BiletFestServices biletFestServices)
        {
            _biletFestServices = biletFestServices;
        }

        [HttpGet("GetAllFestivals")]
        public async Task<IActionResult> GetFestivals()
        {
            var festivals = await _biletFestServices.GetFestivalsAsync();
            return Ok(festivals);
        }

        [HttpGet("GetFestivalById/{id}")]
        public async Task<IActionResult> GetFestivalById(int id)
        {
            var festival = await _biletFestServices.GetFestivalByIdAsync(id);
            if (festival == null)
            {
                return NotFound();
            }
            return Ok(festival);
        }

        [HttpGet("GetFestivalByLink/{link}")]
        public IActionResult GetFestivalByLink(string link)
        {
            var festival = _biletFestServices.GetFestivalByLink(link);

            if (festival == null)
            {
                return NotFound();
            }

            return Ok(festival);
        }

        [HttpPost("AddFestival")]
        public IActionResult AddFestival([FromBody] Festival festival)
        {
            if (festival == null)
            {
                return BadRequest("Festival cannot be null");
            }

            // Verifică dacă biletele sunt setate corect
            if (festival.Tickets != null && festival.Tickets.Any())
            {
                foreach (var ticket in festival.Tickets)
                {
                    // Setează FestivalID pentru bilete
                    ticket.FestivalID = festival.FestivalID;
                }
            }
            else
            {
                festival.Tickets = new List<Ticket>();
            }

            _biletFestServices.AddFestival(festival);
            return CreatedAtAction(nameof(GetFestivalById), new { id = festival.FestivalID }, festival);
        }



        [HttpPost("AddTickets")]
        public async Task<IActionResult> AddTickets(List<Ticket> tickets)
        {
            if (tickets == null || !tickets.Any())
            {
                return BadRequest("No tickets to add.");
            }

            foreach (var ticket in tickets)
            {
                _biletFestServices.AddTicket(ticket);
            }

            await _biletFestServices.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("DeleteTicket/{id}")]
        public IActionResult DeleteTicket(int id)
        {
            var deleted = _biletFestServices.DeleteTicket(id);
            if (!deleted)
            {
                return NotFound();
            }
            return Ok();
        }


        [HttpDelete("DeleteFestival/{id}")]
        public IActionResult DeleteFestival(int id)
        {
            var deleted = _biletFestServices.DeleteFestival(id);
            if(!deleted)
            {
                return NotFound();
            }
            return Ok(deleted);
        }

        [HttpGet("GetTicketByCode/{code}")]
        public IActionResult GetTicketByCode(string code)
        {
            var ticket = _biletFestServices.GetTicket(code);

            if (ticket == null)
            {
                return NotFound();
            }

            return Ok(ticket);
        }
    }
}
