using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly IWebHostEnvironment hostEnvironment;

        public HomeController(IConfiguration config, IJWTService tokenService, IUserService userService, IPublicationService publicationService, IWebHostEnvironment hostEnvironment)
        {
            this.config = config;
            this.tokenService = tokenService;
            this.userService = userService;
            this.publicationService = publicationService;
            this.hostEnvironment = hostEnvironment;
        }

        public IActionResult Home(UserViewModel user)
        {
            ViewBag.UserId = user.UserId;
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

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("/Home/Index")]
        [HttpPost]
        public IActionResult Login(User userLogin)
        {
            if (string.IsNullOrEmpty(userLogin.Email) || string.IsNullOrEmpty(userLogin.Password))
            {
                return (RedirectToAction("Error"));
            }

            var user = userService.Get(userLogin.Email, userLogin.Password);
            var validUser = new UserViewModel { Username = user.Username, UserId = user.UserId, FirstName = user.FirstName, LastName = user.LastName, Mobile = user.Mobile, Gender = user.Gender, Email = user.Email, ProfileImage = user.ProfileImage };

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
                    return RedirectToAction("Home", validUser); //validUser
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
        public IActionResult SignUp(User user)
        {
            var username = userService.GetByUsername(user);
            var email = userService.FindByEmail(user.Email);

            if (username == null && email == null)
            {
                if (ModelState.IsValid)
                {
                    userService.Create(user);
                    return RedirectToAction("Home");
                }
                return View(user);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        public IActionResult Profile()
        {
                string token = HttpContext.Session.GetString("Token");

                if (token == null)
                {
                    return (RedirectToAction("Index"));
                }
                else
                {
                    var id = tokenService.GetJWTTokenClaim(token);
                    var user = userService.GetById(Convert.ToInt32(id));

                    var postsByUser = publicationService.GetPostById(user.UserId);
                    var profileViewModel = new ProfileViewModel { Email = user.Email, UserId = user.UserId, Username = user.Username, FirstName = user.FirstName, LastName = user.LastName, Gender = user.Gender, Mobile = user.Mobile, Publications = postsByUser };
                    
                    return View("Profile", profileViewModel);
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
            string token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return (RedirectToAction("Index"));
            }

            else
            {
                var id = tokenService.GetJWTTokenClaim(token);
                var user = userService.GetById(Convert.ToInt32(id));
                var userViewModel = new UserViewModel { Email = user.Email, UserId = user.UserId, Username = user.Username, FirstName = user.FirstName, LastName = user.LastName, Gender = user.Gender, Mobile = user.Mobile };

                ViewBag.UserId = userViewModel.UserId;

                return View();
            }
        }

        
        [HttpPost]
        public async Task<IActionResult> CreatePost(PostViewModel publication)
        {
            if (ModelState.IsValid)
            {
                User user = userService.GetById(publication.UserId);
                Publication post = new Publication { Text = publication.Text, IdPub = publication.IdPub, Img = publication.Img, Time = publication.Time, User = user, Username = user.Username };
                publicationService.Create(post);
                return RedirectToAction("Home");
            }
            else
            {
                return View(publication);
            }
        }

        public IActionResult EditPublication(int id)
        {
            if(id==null || id == 0)
            {
                return NotFound();
            }
            var pubFromDb = publicationService.GetById(id);
            ViewBag.User = pubFromDb.User;
      
            return View(pubFromDb);
        }


        [HttpPost]
        public async Task<IActionResult> EditPublication(int id, Publication publication)
        {
            publicationService.EditPublication(id, publication);
            return RedirectToAction(nameof(Profile));
        }

        public IActionResult ConfirmDeletePost(int id)
        {
            var publication = publicationService.GetById(id);
            return View(publication);
        }


        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            publicationService.Delete(id);
            return RedirectToAction(nameof(Profile));
        }

        public IActionResult DeleteUser()
        {
            string token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return (RedirectToAction("Index"));
            }

            else
            {
                var id = tokenService.GetJWTTokenClaim(token);
                var user = userService.GetById(Convert.ToInt32(id));
                var userViewModel = new UserViewModel { Email = user.Email, UserId = user.UserId, Username = user.Username, FirstName = user.FirstName, LastName = user.LastName, Gender = user.Gender, Mobile = user.Mobile, ProfileImage = user.ProfileImage };

                return View(userViewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            User userToDelete = userService.GetById(id);
            userService.Delete(userToDelete);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult UpdateUser(int id)
        {
            var user = userService.GetById(id);
            return View(user);
        }

        public IActionResult Update(User user)
        {
            var userToUpdate = userService.GetById(user.UserId);
            if (user is not null && userToUpdate is not null)
            {
                userService.Edit(user.UserId, user);
                return RedirectToAction(nameof(Profile));
            }
            else
            {
                return RedirectToAction(nameof(Error));
            }
        }

        // Upload Image
        [HttpPost("FileUpload")]
        public IActionResult UploadImage(IFormFile file)
        {
            string token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return (RedirectToAction("Index"));
            }
            else
            {
                var id = tokenService.GetJWTTokenClaim(token);
                var user = userService.GetById(Convert.ToInt32(id));
                var userViewModel = new UserViewModel { Email = user.Email, UserId = user.UserId, Username = user.Username, FirstName = user.FirstName, LastName = user.LastName, Gender = user.Gender, Mobile = user.Mobile, ProfileImage = user.ProfileImage };

                string path = Path.Combine(this.hostEnvironment.WebRootPath, "images");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fileName = Path.GetFileName(file.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    file.CopyTo(stream);
                    userService.UpdateImage(userViewModel.UserId, fileName);
                    return RedirectToAction("Profile", new ProfileViewModel { ProfileImage = file.FileName });
                }

                return RedirectToAction("Error");
            }
        }
    }
}