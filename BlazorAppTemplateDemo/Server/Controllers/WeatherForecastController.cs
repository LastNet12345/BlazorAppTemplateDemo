using BlazorAppTemplateDemo.Server.Models;
using BlazorAppTemplateDemo.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
//using static Duende.IdentityServer.IdentityServerConstants;

namespace BlazorAppTemplateDemo.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly UserManager<ApplicationUser> userManager;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
           //Test my Theory before add AddSubTransformation class
           // var copy = User.Clone();
           // var newIdentity = (ClaimsIdentity)copy.Identity;

           // var userId1 = User.FindFirstValue(ClaimTypes.NameIdentifier);
           //// var id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

           // var claim = new Claim("sub", userId1);
           // newIdentity.AddClaim(claim);

           // var principal = new ClaimsPrincipal(newIdentity);
           // var loggedinuser = await userManager.GetUserAsync(principal);



            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = userManager.Users.FirstOrDefault(u => u.Id == userId);
            var id1 = await userManager.GetUserIdAsync(user); 
            var userInRoles = await userManager.GetUsersInRoleAsync("Admin");
            var userAgain = await userManager.FindByIdAsync(userId);

            //Sulution add sub as a cliam with id as value

            //Not working!!!
            //ToDo Fix this!
            //This method runs internal when calling GetUserAsync
            var idstring = userManager.Options.ClaimsIdentity.UserIdClaimType;  //returns "sub",  sub claim transforms to NameIdentifier by default
                                                                                // Some methods use NameIdentifier so can't change that

            var userId2 = User.FindFirstValue(idstring);                        //null
            var user2 = await userManager.GetUserAsync(User);                   //null

            var id = userManager.GetUserId(User);                               //null


            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}