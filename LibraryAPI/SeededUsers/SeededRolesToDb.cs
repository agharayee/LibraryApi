using Library.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.Authorization;

namespace LibraryAPI.SeededUsers
{
    public class SeededRolesToDb
    {
        public class ApplicationDbSeededUser
        {
            public static async Task SeedEssentialsAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
            {
                //Seed Roles
                await roleManager.CreateAsync(new IdentityRole(Autheorization.Roles.Administrator.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Autheorization.Roles.User.ToString()));

                //Seed Default User
                //var defaultUser = new ApplicationUser { UserName = Autheorization.default_username, Email = Authorization.default_email, EmailConfirmed = true, PhoneNumberConfirmed = true };

                //if (userManager.Users.All(u => u.Id != defaultUser.Id))
                //{
                //    await userManager.CreateAsync(defaultUser, Authorization.default_password);
                //    await userManager.AddToRoleAsync(defaultUser, Authorization.default_role.ToString());
                //}
            }
        }
    }
}
