using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Group14_BevoBooks.DAL;
using Group14_BevoBooks.Models;


namespace Group14_BevoBooks.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private SignInManager<AppUser> _signInManager;
        private UserManager<AppUser> _userManager;
        private PasswordValidator<AppUser> _passwordValidator;
        private AppDbContext _db;

        public AccountController(AppDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signIn)
        {
            _db = context;
            _userManager = userManager;
            _signInManager = signIn;
           
            //user manager only has one password validator
            _passwordValidator = (PasswordValidator<AppUser>)userManager.PasswordValidators.FirstOrDefault();
        }

        
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated) //user has been redirected here from a page they're not authorized to see
            {
                return View("Error", new string[] { "Access Denied" });
            }
            _signInManager.SignOutAsync(); //this removes any old cookies hanging around
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return Redirect(returnUrl ?? "/");
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }
        }

       
        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        //TODO: This is the method where you create a new user
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    City = model.City,
                    State = model.State,
                    Zipcode = model.Zipcode,

                };
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //TODO: Add user to desired role
                    //This will not work until you have seeded Identity OR added the "Customer" role 
                    //by navigating to the RoleAdmin controller and manually added the "Customer" role
                
                    await _userManager.AddToRoleAsync(user, "Customer");
                    //another example
                    //await _userManager.AddToRoleAsync(user, "Manager");

                    Microsoft.AspNetCore.Identity.SignInResult result2 = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);
                    String strEmailEmail = model.Email;
                    String strSubjectLine = "Account Registered";
                    String strEmailBody = "Congratulations - You are registered as a new user.";
                    Utilities.EmailMessaging.SendEmail(strEmailEmail, strSubjectLine, strEmailBody);

                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

            }
            return View(model);
        }

        //GET: Account/Index
        public ActionResult Index()
        {
            IndexViewModel ivm = new IndexViewModel();

            //get user info
            String id = User.Identity.Name;
            AppUser user = _db.Users.FirstOrDefault(u => u.UserName == id);

            //populate the view model
            ivm.Email = user.Email;
            ivm.HasPassword = true;
            ivm.UserID = user.Id;
            ivm.UserName = user.UserName;
            ivm.NewPhoneNumber = user.PhoneNumber;
            ivm.NewEmail = user.Email;
            ivm.NewAddress = user.Address;
            ivm.NewLastName = user.LastName;
            ivm.NewFirstName = user.FirstName;
            


            return View(ivm);
        }

        public ActionResult ChangePhoneNumber()
        {
            ChangePhoneNumber cpn = new ChangePhoneNumber();

            return View(cpn);
        }

        public ActionResult ChangeFirstName()
        {
            return View();
        }

        public ActionResult ChangeLastName()
        {
            return View();
        }

        public ActionResult ChangeEmail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeEmail(ChangeEmail model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            AppUser userLoggedIn = await _userManager.FindByNameAsync(User.Identity.Name);

            userLoggedIn.Email = model.NewEmail;

            _db.Users.Update(userLoggedIn);
            _db.SaveChanges();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeLastName(ChangeLastName model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            AppUser userLoggedIn = await _userManager.FindByNameAsync(User.Identity.Name);

            userLoggedIn.LastName = model.NewLastName;

            _db.Users.Update(userLoggedIn);
            _db.SaveChanges();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeFirstName(ChangeFirstName model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            AppUser userLoggedIn = await _userManager.FindByNameAsync(User.Identity.Name);

            _db.Users.Update(userLoggedIn);
            _db.SaveChanges();

            userLoggedIn.FirstName = model.NewFirstName;
            return View(model);
        }

            //Logic for change password
            // GET: /Account/ChangePassword
            public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePhoneNumber(ChangePhoneNumber model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            AppUser userLoggedIn = await _userManager.FindByNameAsync(User.Identity.Name);

            userLoggedIn.PhoneNumber = model.NewPhoneNumber;

            _db.Users.Update(userLoggedIn);
            _db.SaveChanges();

            return RedirectToAction("Index");

        }

        public ActionResult ChangeAddress()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeAddress(ChangeAddress model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            AppUser userLoggedIn = await _userManager.FindByNameAsync(User.Identity.Name);

            _db.Users.Update(userLoggedIn);
            _db.SaveChanges();

            userLoggedIn.Address = model.NewAddress;
            return View(model);

        }


        //
        // POST: /Account/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            AppUser userLoggedIn = await _userManager.FindByNameAsync(User.Identity.Name);
            var result = await _userManager.ChangePasswordAsync(userLoggedIn, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(userLoggedIn, isPersistent: false);
                String strEmailEmail = GetPasswordEmail();
                String strEmailBody = "Congratulations - You are successfully changed your password to " + model.NewPassword;
                Utilities.EmailMessaging.SendEmail(strEmailEmail, "Password Change", strEmailBody);
                return RedirectToAction("Index", "Home");
            }
            AddErrors(result);

            return View(model);

        }

        public String GetPasswordEmail()
        {
            IndexViewModel ivmm = new IndexViewModel();

            //get user info
            String id = User.Identity.Name;
            AppUser user = _db.Users.FirstOrDefault(u => u.UserName == id);

            //populate the view model
            ivmm.Email = user.Email;
            ivmm.HasPassword = true;
            ivmm.UserID = user.Id;
            ivmm.UserName = user.UserName;

            String WOWEmail;
            WOWEmail = ivmm.Email;
            return WOWEmail;
        }

        

        //GET:/Account/AccessDenied
        public ActionResult AccessDenied(String ReturnURL)
        {
            return View("Error", new string[] { "Access is denied" });
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
           

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}