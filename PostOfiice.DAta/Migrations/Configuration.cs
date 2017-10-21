namespace PostOfiice.DAta.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using PostOffice.Model.Models;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PostOfiice.DAta.PostOfficeDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;            
        }

        protected override void Seed(PostOfiice.DAta.PostOfficeDbContext context)
        {
            //var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new PostOfficeDbContext()));

            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new PostOfficeDbContext()));

            //var user = new ApplicationUser()
            //{
            //    UserName = "admin",
            //    Email = "postoffice.international@gmail.com",
            //    EmailConfirmed = true,
            //    BirthDay = DateTime.Now,
            //    FullName = "Post Office",
            //    POID = 4
            //};

            //manager.Create(user, "123456");

            //if (!roleManager.Roles.Any())
            //{
            //    roleManager.Create(new IdentityRole { Name = "Admin" });
            //    roleManager.Create(new IdentityRole { Name = "User" });
            //}

            //var adminUser = manager.FindByEmail("postoffice.international@gmail.com");

            //manager.AddToRoles(adminUser.Id, new string[] { "Admin", "User" });
        }
    }
}