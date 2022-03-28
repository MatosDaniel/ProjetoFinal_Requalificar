using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Models;
using ProjetoFinal.Service;
using System.Diagnostics;

namespace ProjetoFinal.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration config;
        private readonly IJWTService tokenService;
        private readonly IUserService userService;
        private readonly IPublicationService publicationService;

        public HomeController(IConfiguration config, IJWTService tokenService, IUserService userService, IPublicationService publicationService)
        {
            this.config = config;
            this.tokenService = tokenService;
            this.userService = userService;
            this.publicationService = publicationService;
        }

        public IActionResult Index()
        {
            IEnumerable<Publication> publicationList = publicationService.GetAll();
            return View(publicationList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("/Home/Login")]
        [HttpPost]
        public IActionResult Login(User userLogin)
        {
            if (string.IsNullOrEmpty(userLogin.Email) || string.IsNullOrEmpty(userLogin.Password))
            {
                return (RedirectToAction("Error"));
            }

            var user = userService.Get(userLogin.Email, userLogin.Password);
            var validUser = new UserViewModel { Username = user.Username, Password = user.Password, FirstName = user.FirstName, LastName = user.LastName, Mobile = user.Mobile, Gender = user.Gender, Email = user.Email };

            if (validUser != null)
            {

                string generatedToken = tokenService.GenerateToken(
                    config["Jwt:Key"].ToString(),
                    config["Jwt:Issuer"].ToString(),
                    config["Jwt:Audience"].ToString(),
                user);

                if (generatedToken != null)
                {
                    HttpContext.Session.SetString("Token", generatedToken);
                    return RedirectToAction("Profile", user); //validUser
                }
                else
                {
                    return (RedirectToAction("Error"));
                }
            }
            else
            {
                return (RedirectToAction("Error"));
            }
}

        public IActionResult SignUp()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignUp(User user)
        {
            if (ModelState.IsValid)
            {
                var userExists = userService.FindByEmail(user.Email);
                if (userExists != null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "User already exists!");

                var newUser = userService.Create(user);
                if (newUser is not null)
                    return RedirectToAction(nameof(Index));
                else
                    return RedirectToAction(nameof(Error));
            }
            else
            {
                return RedirectToAction(nameof(Error));
            }
        }

        [HttpGet]
        public IActionResult Profile(User user_)
        {
            var user = userService.GetById(user_.UserId);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                string token = HttpContext.Session.GetString("Token");

                if (token == null)
                {
                    return (RedirectToAction("Index"));
                }

                if (!tokenService.IsTokenValid(
                    config["Jwt:Key"].ToString(),
                    config["Jwt:Issuer"].ToString(),
                    config["Jwt:Audience"].ToString(),
                    token))
                {
                    return (RedirectToAction("Index"));
                }

                ViewBag.Token = token;
                return View(user);
            }
        }

        public IActionResult LogOut()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult LogoutUser()
        {
            HttpContext.Session.Remove("Token");
            return (RedirectToAction("Index"));
        }

        public IActionResult CreatePublication()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> CreatePublication(Publication publication)
        {
            if (ModelState.IsValid)
            {
                publicationService.Create(publication);
                return RedirectToAction("Index");
            }
            return View(publication);
        }
        public IActionResult EditPublication(int id)
        {
            if(id==null || id == 0)
            {
                return NotFound();
            }
            var pubFromDb = publicationService.GetById(id);
            return View(pubFromDb);
        }


        [HttpPost]
        public async Task<IActionResult> EditPublication(Publication publication)
        {
            
                publicationService.EditPublication(publication);
                return RedirectToAction(nameof(Index));
        }
    }
}