// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Orpheo.Data;
using Orpheo.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ORPHEO.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public string? ProfileImagePath { get; set; }
        public RoleRequest RoleRequest { get; set; }
        public bool IsArtist { get; set; }
        public bool IsAdmin { get; set; }

        public bool CanReapply { get; set; }

        public IndexModel(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ApplicationDbContext context)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _context = context;
            }


        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

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
        public class InputModel
        {
            [Display(Name = "Name")]
            public string? Name { get; set; }

            [EmailAddress]
            [Display(Name = "Email")]
            public string? Email { get; set; }

            [Display(Name = "About")]
            public string? About { get; set; }

            [Display(Name = "Profile photo")]
            public IFormFile? ProfilePhoto { get; set; }

            // DOAR AFIȘARE
            public string? UserCode { get; set; }
        }


        private async Task LoadAsync(ApplicationUser user)
        {
            Username = await _userManager.GetUserNameAsync(user);
            ProfileImagePath = user.ProfileImage;

            Input = new InputModel
            {
                Name = user.Name,
                Email = user.Email,
                About = user.About,
                UserCode = user.UserCode
            };
        }


        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            // rolul curent
            IsArtist = await _userManager.IsInRoleAsync(user, "Artist");
            IsAdmin = await _userManager.IsInRoleAsync(user, "Admin");


            // cererea de rol
            RoleRequest = _context.RoleRequests
                .FirstOrDefault(r => r.UserId == user.Id);

            IsArtist = await _userManager.IsInRoleAsync(user, "Artist");

            CanReapply = false;

            if (RoleRequest == null)
            {
                CanReapply = true;
            }
            else if (RoleRequest.Status == "Rejected")
            {
                var oneYearLater = RoleRequest.CreatedAt.AddYears(1);

                if (DateTime.Now >= oneYearLater)
                {
                    CanReapply = true;
                }
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            user.Name = Input.Name;
            user.About = Input.About;

            if (user.Email != Input.Email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set email.";
                    return RedirectToPage();
                }
            }
            if (Input.ProfilePhoto != null && Input.ProfilePhoto.Length > 0)
            {
                var uploadsFolder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "uploads",
                    "profile-images"
                );

                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{user.Id}{Path.GetExtension(Input.ProfilePhoto.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await Input.ProfilePhoto.CopyToAsync(stream);

                // path salvat în DB
                user.ProfileImage = "/uploads/profile-images/" + fileName;
            }


            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);

            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}

