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
        private readonly IWebHostEnvironment hostEnvironment;

        public HomeController(IConfiguration config, IJWTService tokenService, IUserService userService, IPublicationService publicationService, IWebHostEnvironment hostEnvironment)
        {
            this.config = config;
            this.tokenService = tokenService;
            this.userService = userService;
            this.publicationService = publicationService;
            this.hostEnvironment = hostEnvironment;
        }

        //Endpoint that returns the Home Page with all the Gluglus
        [Authorize]
        public IActionResult Home(UserViewModel user)
        {
            ViewBag.UserId = user.UserId;
            IEnumerable<Publication> publicationList = publicationService.GetAll();
            return View(publicationList);
        }

        //Endpoint that returns the Error Page
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string messageError)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, MessageError = messageError });
        }

        //Endpoint that returns the Login Page
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        //Endpoint that login a User and attributes in a Token
        [AllowAnonymous]
        [Route("/Home/Index")]
        [HttpPost]
        public IActionResult Login(User userLogin)
        {
            if (string.IsNullOrEmpty(userLogin.Email) || string.IsNullOrEmpty(userLogin.Password))
            {
                var error = new ErrorViewModel { MessageError = "Both fields need to be filled. Try again." };
                return (RedirectToAction("Error", error));
            }

            var user = userService.Get(userLogin.Email, userLogin.Password);

            if(user is null)
            {
                var error = new ErrorViewModel { MessageError = "The password or email is incorrect. Try again." };
                return (RedirectToAction("Error", error));
            }

            var validUser = new UserViewModel { Username = user.Username, UserId = user.UserId, FirstName = user.FirstName, LastName = user.LastName, 
                Mobile = user.Mobile, Gender = user.Gender, Email = user.Email };

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
                    return RedirectToAction("Home", validUser);
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

        //Endpoint that returns the Sign Up Page
        [AllowAnonymous]
        public IActionResult SignUp()
        {
            return View();
        }

        //Endpoint that register a User
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
                    return RedirectToAction("Index");
                }
                return View(user);
            }
            else
            {
                var error = new ErrorViewModel { MessageError = "This email or username already exist. Try again." };
                return RedirectToAction("Error",error);
            }
        }

        //Endpoint that return a Profile Page
        [Authorize]
        [HttpGet]
        public IActionResult Profile()
        {
            string token = HttpContext.Session.GetString("Token");

            //Check if the User has a valid Token active, if not sends him to the Login Page
            if (token == null)
            {
                return (RedirectToAction("Index"));
            }

            else
            {
                var id = tokenService.GetJWTTokenClaim(token);
                var user = userService.GetById(Convert.ToInt32(id));

                var postsByUser = publicationService.GetPostById(user.UserId);

                //Count the total posts made by the user
                var totalPosts = postsByUser.Count();

                // Creates a Profile View Model based on the user and publication models
                var profileViewModel = new ProfileViewModel { Email = user.Email, UserId = user.UserId, Username = user.Username, FirstName = user.FirstName, 
                    LastName = user.LastName, Gender = user.Gender, Mobile = user.Mobile, Publications = postsByUser, ProfileImage = user.ProfileImage, TotalPostByUser = totalPosts };
                    
                return View("Profile", profileViewModel);
            }
        }

        //Endpoint that return a Logout Page
        public IActionResult LogOut()
        {
            return View();
        }

        //Endpoint that remove the Token, ending the user session
        [AllowAnonymous]
        [HttpPost]
        public IActionResult LogoutUser()
        {
            HttpContext.Session.Remove("Token");
            return (RedirectToAction("Index"));
        }

        //Endpoint that returns the page that allows you to create a new Gluglu
        [Authorize]
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
                var userViewModel = new UserViewModel { Email = user.Email, UserId = user.UserId, Username = user.Username, FirstName = user.FirstName, 
                    LastName = user.LastName, Gender = user.Gender, Mobile = user.Mobile };

                ViewBag.UserId = userViewModel.UserId;

                return View();
            }
        }

        //Endpoint that creates a new Gluglu 
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePublication(PostViewModel publication)
        {
            if (ModelState.IsValid)
            {
                User user = userService.GetById(publication.UserId);
                Publication post = new Publication { Text = publication.Text, IdPub = publication.IdPub, Time = publication.Time, User = user, Username = user.Username };
                publicationService.Create(post);
                return RedirectToAction("Home");
            }
            else
            {
                return View(publication);
            }
        }

        //Endpoint that returns the page that allows you to edit a new Gluglu
        [Authorize]
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

        //Endpoint that edits a new Gluglu 
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditPublication(int id, Publication publication)
        {
            publicationService.EditPublication(id, publication);
            return RedirectToAction(nameof(Profile));
        }

        //Endpoint that returns the page that allows you to confirm if you want to delete a Gluglu
        [Authorize]
        public IActionResult ConfirmDeletePost(int id)
        {
            var publication = publicationService.GetById(id);
            return View(publication);
        }

        //Endpoint that delete a Gluglu
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            publicationService.Delete(id);
            return RedirectToAction(nameof(Profile));
        }

        //Endpoint that returns the page that allows you to confirm if you want to delete your Fish account
        [Authorize]
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
                var userViewModel = new UserViewModel { Email = user.Email, UserId = user.UserId, Username = user.Username, FirstName = user.FirstName, 
                    LastName = user.LastName, Gender = user.Gender, Mobile = user.Mobile, ProfileImage = user.ProfileImage };

                return View(userViewModel);
            }
        }

        //Endpoint that delete a Fish account
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            User userToDelete = userService.GetById(id);
            userService.Delete(userToDelete);
            return RedirectToAction(nameof(Index));
        }

        //Endpoint that returns the page that allows you to edit your profile details
        [Authorize]
        public IActionResult UpdateUser(int id)
        {
            var user = userService.GetById(id);
            return View(user);
        }

        //Endpoint that edit your profile
        [Authorize]
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

        //Endpoint that uploads a new profile image
        [Authorize]
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
                var userViewModel = new UserViewModel { Email = user.Email, UserId = user.UserId, Username = user.Username, FirstName = user.FirstName, 
                    LastName = user.LastName, Gender = user.Gender, Mobile = user.Mobile, ProfileImage = user.ProfileImage };

                //Saves the profile pictures in a folder called Images, and creates one if there is none
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

        //Endpoint that adds a like to a post
        public async Task<IActionResult> Likes(int id)
        {
            publicationService.Likes(id);
            return RedirectToAction(nameof(Home));
        }
    }
}