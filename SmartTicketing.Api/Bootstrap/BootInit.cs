using SmartTicketing.Domain.Enums;
using SmartTicketing.Domain;
using SmartTicketing.Application.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace SmartTicketing.Api.Bootstrap
{
    internal sealed class BootInit : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;

        public BootInit(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Dispose()
        {
            // TODO: dispose
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

            var user = new User
            {
                FirstName = "First",
                LastName = "Test",
                Username = "test@ticketing.com",
                Password = ComputeSha("testPw"),
                Role = EUserRole.Admin
            };

            await context.Users.AddAsync(user);

            for (int i = 0; i < 300; i++)
            {
                var ticket = new Ticket
                {
                    AssignToUserId = 1,
                    CreatedByUserId = 1,
                    Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    Status = ETicketStatus.New,
                    Title = $"Ticket Title {i}"
                };

                await context.Tickets.AddAsync(ticket);
            }

            await context.SaveChangesAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private string ComputeSha(string password)
        {
            using (SHA512 sha = SHA512.Create())
            {
                byte[] hashValue = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToHexString(hashValue);
            }
        }
    }
}
