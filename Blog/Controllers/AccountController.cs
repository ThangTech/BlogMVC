using Blog.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    // GET
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        var identityUser = new IdentityUser
        {
            UserName = registerViewModel.Username,
            Email = registerViewModel.Email
        };
        var result = await _userManager.CreateAsync(identityUser, registerViewModel.Password);
        if (result.Succeeded)
        {
            var resultSuccess = await _userManager.AddToRoleAsync(identityUser, "User");
            if (resultSuccess.Succeeded)
            {
                return RedirectToAction("Register");
            }
        }

        return View();
    }
    
    [HttpGet]
    public IActionResult Login(string ReturnUrl)
    {
        var model = new LoginViewModel
        {
            ReturnUrl = ReturnUrl
        };
        return View(model);
    }

    [HttpPost]

    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        var result = await _signInManager.PasswordSignInAsync
            (loginViewModel.Username, loginViewModel.Password, false, false);
        if (result != null && result.Succeeded)
        {
            if (!string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl))
            {
                return RedirectToPage(loginViewModel.ReturnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}