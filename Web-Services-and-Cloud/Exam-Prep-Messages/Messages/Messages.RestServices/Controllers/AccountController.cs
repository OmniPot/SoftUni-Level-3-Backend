namespace Messages.RestServices.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Messages.Data.Models;
    using Messages.RestServices.Models;
    using Messages.RestServices.Models.BindingModels;
    using Messages.RestServices.Models.ViewModels;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Testing;

    [Authorize]
    [RoutePrefix("api/user")]
    public class AccountController : BaseApiController
    {
        public ApplicationUserManager UserManager
        {
            get { return this.Request.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
        }

        // POST api/user/register
        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IHttpActionResult> RegisterUser(UserAccountBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Invalid user data");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var user = new User
            {
                UserName = model.Username
            };

            var identityResult = await this.UserManager.CreateAsync(user, model.Password);

            if (!identityResult.Succeeded)
            {
                return this.GetErrorResult(identityResult);
            }

            // Auto login after registrаtion (successful user registration should return access_token)
            var loginResult = await this.LoginUser(new UserAccountBindingModel
            {
                Username = model.Username,
                Password = model.Password
            });
            return loginResult;
        }

        // POST api/user/login
        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IHttpActionResult> LoginUser(UserAccountBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Invalid user data");
            }

            // Invoke the "token" OWIN service to perform the login (POST /api/token)
            var testServer = TestServer.Create<Startup>();
            var requestParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", model.Username),
                new KeyValuePair<string, string>("password", model.Password)
            };
            var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
            var tokenServiceResponse = await testServer.HttpClient.PostAsync(
                Startup.TokenEndpointPath, requestParamsFormUrlEncoded);

            return this.ResponseMessage(tokenServiceResponse);
        }

        // GET api/user/personal-messages
        [HttpGet]
        [Route("personal-messages")]
        public IHttpActionResult GetPersonalMessages()
        {
            var loggedUserId = this.User.Identity.GetUserId();
            if (loggedUserId == null)
            {
                return this.Unauthorized();
            }

            var userMessages = this.Data.UserMessages.All()
                .Where(um => um.Recipient.Id.Equals(loggedUserId))
                .OrderByDescending(m => m.DateSent)
                .Select(PersonalMessageViewModel.Create);

            return this.Ok(userMessages);
        }

        // POST api/user/personal-messages
        [HttpPost]
        [AllowAnonymous]
        [Route("personal-messages/anonymous")]
        public IHttpActionResult SendAnonymousPersonalMessage(PersonalMessageBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var recipient = this.Data.Users.All()
                .FirstOrDefault(u => u.UserName.Equals(model.Recipient));
            if (recipient == null)
            {
                return this.NotFound();
            }

            var message = new UserMessage
            {
                Text = model.Text,
                DateSent = DateTime.Now,
                Sender = null,
                Recipient = recipient,
                RecipientId = recipient.Id
            };

            this.Data.UserMessages.Add(message);
            this.Data.SaveChanges();

            var responseMessage =
                string.Format("Anonymous message sent successfully to user {0}.", recipient.UserName);

            return this.Ok(new
            {
                message.Id,
                Message = responseMessage
            });
        }

        // POST api/user/personal-messages
        [HttpPost]
        [Route("personal-messages/authorized")]
        public IHttpActionResult SendPersonalMessage(PersonalMessageBindingModel model)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.Data.Users.Find(loggedUserId);
            if (loggedUserId == null)
            {
                return this.Unauthorized();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var recipient = this.Data.Users.All()
                .FirstOrDefault(u => u.UserName.Equals(model.Recipient));

            if (recipient == null)
            {
                return this.NotFound();
            }

            var message = new UserMessage
            {
                Text = model.Text,
                DateSent = DateTime.Now,
                Sender = loggedUser,
                SenderId = loggedUserId,
                Recipient = recipient,
                RecipientId = recipient.Id
            };

            this.Data.UserMessages.Add(message);
            this.Data.SaveChanges();

            var responseMessage =
                string.Format("Message sent successfully to user {0}.", recipient.UserName);

            return this.Ok(new
            {
                message.Id,
                Message = responseMessage
            });
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return this.InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        this.ModelState.AddModelError("", error);
                    }
                }

                if (this.ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return this.BadRequest();
                }

                return this.BadRequest(this.ModelState);
            }

            return null;
        }
    }
}