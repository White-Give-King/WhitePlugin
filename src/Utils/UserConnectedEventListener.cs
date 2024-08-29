
using System;
using System.Threading.Tasks;
using OpenMod.API.Eventing;
using OpenMod.Core.Users.Events;

namespace WhitePlugin
{
    public class UserConnectedEventListener : IEventListener<IUserConnectedEvent>
    {
        private readonly UserConnectionDbContext m_DbContext;

        public UserConnectedEventListener(UserConnectionDbContext dbContext)
        {
            m_DbContext = dbContext;
        }

        public async Task HandleEventAsync(object? sender, IUserConnectedEvent @event)
        {
            UserConnection userConnection = new UserConnection
            {
                UserId = @event.User.Id,
                UserType = @event.User.Type,
                ConnectionTime = DateTime.UtcNow
            };

            await m_DbContext.UserConnections.AddAsync(userConnection);

            await m_DbContext.SaveChangesAsync();
        }
    }
}