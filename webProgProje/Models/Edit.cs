using Azure.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.Mvc.RazorPages;

#nullable disable
namespace webProgProje.Models
{
    public class Edit:PageModel
    {
        private CombineContext _combineContext;
        private readonly SignInManager<Kullanici> _signInManager;
        private readonly UserManager<Kullanici> _userManager;
        private readonly IUserStore<Kullanici> _userStore;
        private readonly IUserEmailStore<Kullanici> _emailStore;
        private readonly ILogger<Edit> _logger;
        private readonly IEmailSender _emailSender;

        public Edit(
            CombineContext combineContext,
            UserManager<Kullanici> userManager,
            IUserStore<Kullanici> userStore,
            SignInManager<Kullanici> signInManager,
            ILogger<Edit> logger,
            IEmailSender emailSender)
        {
            _combineContext = combineContext;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            /// 
            [Required]
            [StringLength(11)]
            [Display(Name = "TC")]
            public string TC { get; set; }

            [Required]
            [MaxLength(10)]
            public string KullaniciTipi { get; set; }

            [Required]
            [MaxLength(50)]
            [Display(Name = "Ad")]
            public string Ad { get; set; }

            [Required]
            [MaxLength(50)]
            [Display(Name = "Soyad")]
            public string Soyad { get; set; }

            [Phone]
            [Required]
            [StringLength(11)]
            [Display(Name = "Telefon")]
            public string Telefon { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
            [DataType(DataType.Password)]
            [Display(Name = "Sifre")]
            public string Sifre { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Sifre tekrar")]
            [Compare("Sifre", ErrorMessage = "sifreler eslesmiyor!")]
            public string SifreOnay { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();
                var doktor = CreateUserDoktor();
                user.TC = Input.TC;
                user.KullaniciTipi = Input.KullaniciTipi;
                user.Ad = Input.Ad;
                user.Soyad = Input.Soyad;
                user.Telefon = Input.Telefon;
                user.Email = Input.Email;
                user.Sifre = Input.Sifre;
                //user.Doktor.AnadalID=Input.TC
                doktor.Id = user.Id;
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Sifre);
                _combineContext.Kullanicilar.Add(user);
                _combineContext.Doktorlar.Add(doktor);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private Kullanici CreateUser()
        {
            try
            {
                return Activator.CreateInstance<Kullanici>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(Kullanici)}'. " +
                    $"Ensure that '{nameof(Kullanici)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
        private Doktor CreateUserDoktor()
        {
            try
            {
                return Activator.CreateInstance<Doktor>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(Doktor)}'. " +
                    $"Ensure that '{nameof(Doktor)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<Kullanici> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<Kullanici>)_userStore;
        }
    }
}

