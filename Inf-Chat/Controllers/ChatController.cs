using Inf_Chat.Data.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Inf_Chat.Controllers
{
    [Authorize]
    public class ChatController : ApiController
    {
        ApplicationDbContext _dbContext;
        public ChatController()
        {
            _dbContext = new ApplicationDbContext();
        }

        public IHttpActionResult GetMessages()
        {
            var res = new List<MessageViewModel>();
            foreach(var item in _dbContext.Messages.OrderBy(m => m.AddedDate).ToList())
            {
                res.Add(new MessageViewModel()
                {
                    AuthorName = item.Author.UserName,
                    Date = item.AddedDate.ToString(),
                    Body = item.Text
                });
            }
            return Ok(res);
        }
    }
}
