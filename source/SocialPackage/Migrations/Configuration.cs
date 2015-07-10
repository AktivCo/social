namespace SocialPackage.Migrations
{
    using SocialPackage.Infrastructure.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SocialPackage.Infrastructure.EfDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SocialPackage.Infrastructure.EfDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            if (context.UserRole.Count() == 0)
            {
                var roles = new List<UserRole> {
                    new UserRole { RoleName = "Пользователь"},
                    new UserRole { RoleName = "Администратор"},
                    new UserRole { RoleName = "Бухгалтер"},
                    new UserRole { RoleName = "Секретарь"}
                };
                context.UserRole.AddRange(roles);
            }

            if (context.Categories.Count() == 0)
            {
                var categories = new List<Category> {
                    new Category { Color = "#755bee", Name="Проездные"},
                    new Category { Color = "#3ec172", Name="Фитнес"},
                    new Category { Color = "#e73f72", Name="Соц. Мероприятия"}
                };
                context.Categories.AddRange(categories);
            }

            if (context.Limit.Count() == 0)
            {
                var firstLimit = new Limit
                {
                    CultureEvents = -1,
                    Fitnes = -1,
                    Proezd = -1,
                    Name = "Пустой лимит"
                };
                context.Limit.Add(firstLimit);
            }

            context.SaveChanges();
        }
    }
}
