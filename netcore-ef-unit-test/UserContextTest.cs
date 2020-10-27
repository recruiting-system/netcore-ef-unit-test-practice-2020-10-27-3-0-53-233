using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using netcore_ef.Models;
using System.Collections.Generic;

namespace netcore_ef_unit_test
{
    public class UserContextTest
    {
        public static DbContextOptions<UserContext> CreateDbContextOptions(string databaseName)
        {
            var serviceProvider = new ServiceCollection().
                AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<UserContext>();
            builder.UseInMemoryDatabase(databaseName)
                .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        [Fact]
        public async void test_should_return_user_when_the_user_exist()
        {
            // given
            var options = CreateDbContextOptions("batabase");
            var context = new UserContext(options);
            context.Users.Add(new User() { Name = "ef core" });
            context.SaveChanges();

            // when
            List<User> users = await context.Users.ToListAsync<User>();

            // then
            Assert.Single(users);
            Assert.Equal("ef core1", users[0].Name);
        }
    }
}