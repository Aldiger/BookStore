﻿namespace BookStore.Infrastructure.Common.Persistence;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Domain.Common;
using Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using static Domain.Common.Models.ModelConstants.Common;

internal class BookStoreDbInitializer : IDbInitializer
{
    private readonly BookStoreDbContext db;
    private readonly UserManager<User> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly IEnumerable<IInitialData> initialDataProviders;

    public BookStoreDbInitializer(
        BookStoreDbContext db,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        IEnumerable<IInitialData> initialDataProviders)
    {
        this.db = db;
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.initialDataProviders = initialDataProviders;
    }

    public void Initialize()
    {
        this.db.Database.Migrate();

        this.SeedAdministrator();

        foreach (var initialDataProvider in this.initialDataProviders)
        {
            if (this.DataSetIsEmpty(initialDataProvider.EntityType))
            {
                var data = initialDataProvider.GetData();

                foreach (var entity in data)
                {
                    this.db.Add(entity);
                }
            }
        }

        this.db.SaveChanges();
    }

    private void SeedAdministrator()
        => Task
            .Run(async () =>
            {
                var adminRoleExists = await this.roleManager.RoleExistsAsync(AdministratorRoleName);

                if (adminRoleExists)
                {
                    return;
                }

                var adminRole = new IdentityRole(AdministratorRoleName);

                await this.roleManager.CreateAsync(adminRole);

                var adminUser = new User("admin@bookstore.com");

                await this.userManager.CreateAsync(adminUser, "admin123456");
                await this.userManager.AddToRoleAsync(adminUser, AdministratorRoleName);
            })
            .GetAwaiter()
            .GetResult();

    private bool DataSetIsEmpty(Type type)
    {
        var setMethod = this.GetType()
            .GetMethod(nameof(this.GetSet), BindingFlags.Instance | BindingFlags.NonPublic)!
            .MakeGenericMethod(type);

        var set = setMethod.Invoke(this, Array.Empty<object>());

        var countMethod = typeof(Queryable)
            .GetMethods()
            .First(m => m.Name == nameof(Queryable.Count) && m.GetParameters().Length == 1)
            .MakeGenericMethod(type);

        var result = (int)countMethod.Invoke(null, new[] { set })!;

        return result == 0;
    }

    private DbSet<TEntity> GetSet<TEntity>()
        where TEntity : class
        => this.db.Set<TEntity>();
}