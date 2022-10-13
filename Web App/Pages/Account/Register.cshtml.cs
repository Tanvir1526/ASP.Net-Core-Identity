using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Web_App.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManger;

        public RegisterModel(UserManager<IdentityUser> userManger  )
        {
            this.userManger = userManger;
        }
        [BindProperty]
        public RegisterViewModel RegisterViewModel { get; set; }         
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page(); 
            }
            //validating Email Address(optional)

            //Create the user
            var user = new IdentityUser
            {
                Email = RegisterViewModel.Email,
                UserName = RegisterViewModel.Email,
            };

            var result= await this.userManger.CreateAsync(user, RegisterViewModel.Password);
            if(result.Succeeded)
            {
                return RedirectToPage("/Account/Login");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }
                return Page();
            }

        }
    }
    public class RegisterViewModel
    {
        [Required, EmailAddress(ErrorMessage = "Invalid Email Address !!!")]
        public string Email { get; set; }
        [Required, DataType(dataType: DataType.Password)]
        public string Password { get; set; }
    }
}
