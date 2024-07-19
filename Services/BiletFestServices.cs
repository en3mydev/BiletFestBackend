using BiletFest.Data;
using BiletFest.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BiletFest.Services
{
    public class BiletFestServices
    {
        private DataContext _context;

        public BiletFestServices(DataContext context)
        {
            _context = context;
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public async Task<bool> CreateUser(string password, string email, string fullName, string role = "User")
        {
            if (_context.Users.Any(u => u.Email == email))
            {
                return false;
            }

            // Generează un salt pentru hash-ul parolei
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string saltBase64 = Convert.ToBase64String(salt);
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            var user = new User
            {
                PasswordHash = $"{hashedPassword}:{saltBase64}",
                Email = email,
                FullName = fullName,
                Role = role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<User> LoginAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return null;
            }

            // Desparte hash-ul parolei și salt-ul
            var passwordParts = user.PasswordHash.Split(':');
            if (passwordParts.Length != 2)
            {
                return null;
            }

            var hashedPassword = passwordParts[0];
            var salt = Convert.FromBase64String(passwordParts[1]);

            // Hashing parola introdusă cu salt-ul salvat
            string hashedInputPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            if (hashedPassword != hashedInputPassword)
            {
                return null;
            }

            return user;
        }

        public async Task<User> GetUserDataAsync(string userId)
        {
            int id = int.Parse(userId);
            return await _context.Users.FirstOrDefaultAsync(u => u.UserID == id);
        }

        public async Task<IEnumerable<Festival>> GetFestivalsAsync()
        {
            return await _context.Festivals.ToListAsync();
        }

        public void AddFestival(Festival festival)
        {
            // Adaugă festivalul în context
            _context.Festivals.Add(festival);

            // Salvează schimbările în context
            _context.SaveChanges();
        }

        public async Task<Festival> GetFestivalByIdAsync(int id)
        {
            return await _context.Festivals.Include(f => f.Tickets)
                                           .FirstOrDefaultAsync(f => f.FestivalID == id);
        }

        public void AddTicket(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public bool DeleteTicket(int id)
        {
            var ticket = _context.Tickets.Find(id);
            if (ticket == null)
            {
                return false;
            }

            _context.Tickets.Remove(ticket);
            _context.SaveChanges();
            return true;
        }

        public Festival GetFestivalByLink(string link)
        {
            return _context.Festivals
                .Include(f => f.Tickets) // Include biletele asociate
                .FirstOrDefault(f => f.Link == link);
        }

        public bool DeleteFestival(int id)
        {
            var festival = _context.Festivals.Find(id);
            if (festival == null)
            {
                return false;
            }

            _context.Festivals.Remove(festival);
            _context.SaveChanges();
            return true;
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await _context.Orders
                .Include(o => o.OrderTickets)
                    .ThenInclude(ot => ot.Ticket)
                .ToListAsync();

        }


        public async Task<bool> CreateOrder(Order order, Dictionary<int, List<string>> ticketCodes)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                foreach (var ticketCode in ticketCodes)
                {
                    int ticketId = ticketCode.Key;
                    foreach (var code in ticketCode.Value)
                    {
                        var orderTicket = new OrderTicket
                        {
                            OrderId = order.OrderId,
                            TicketId = ticketId,
                            UniqueCode = code
                        };
                        _context.OrderTickets.Add(orderTicket);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

    }
}
