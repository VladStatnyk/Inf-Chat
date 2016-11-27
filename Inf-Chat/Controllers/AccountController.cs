using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Inf_Chat.Data.BindingModels;
using Inf_Chat.Data.Entities;
using Inf_Chat.Data.Managers;
using System.Threading.Tasks;
using System.Web.Http;

namespace Inf_Chat.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private readonly ApplicationDbContext _context;
        private readonly ApplicationUserManager _userManager;

        public AccountController()
        {
            _context = new ApplicationDbContext();
            _userManager = new ApplicationUserManager(new UserStore<User>(_context));
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool isNameUnique = _userManager.FindByName(userModel.UserName) == null;
            if (!isNameUnique)
            {
                return BadRequest("User with the same login is already registered");
            }

            var user = new User { UserName = userModel.UserName };

            IdentityResult result = await _userManager.CreateAsync(user, userModel.Password);
            if (!result.Succeeded) { return GetErrorResult(result); }

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _userManager.Dispose();
                _context.Dispose();
            }

            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
