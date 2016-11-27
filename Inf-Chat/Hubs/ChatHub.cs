using System.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.Identity;
using Inf_Chat.Data.Entities;

namespace Inf_Chat.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        ApplicationDbContext _dbContext;
        public ChatHub()
        {
            _dbContext = new ApplicationDbContext();
        }

        public void SendMessage(string messageBody)
        {
            if (string.IsNullOrEmpty(messageBody))
                return;

            var userId = Context.User.Identity.GetUserId();
            var user = _dbContext.Users.FirstOrDefault(item => item.Id == userId);
            var mess = new Message()
            {
                Author = user,
                Text = messageBody
            };
            Clients.All.broadcastMessage(user.UserName, messageBody, mess.AddedDate.ToString());

            _dbContext.Messages.Add(mess);
            _dbContext.SaveChanges();
        }
    }
}