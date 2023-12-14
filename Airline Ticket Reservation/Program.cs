using Airline_Ticket_Reservation.Controllers;
using Airline_Ticket_Reservation.Interfaces;
using Airline_Ticket_Reservation.Services;
using DataAccess.DataContexts;
using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Airline_Ticket_Reservation
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<AirlineDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<CustomUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AirlineDbContext>();
            builder.Services.AddControllersWithViews();


            string absolutePath = builder.Environment.ContentRootPath + "Data\\ticket.json";
            // builder.Services.AddScoped<ITicketRepository,TicketDbRepository>(x => new TicketFileRepository(absolutePath));
            builder.Services.AddScoped<ITicketRepository, TicketDbRepository>();
            builder.Services.AddScoped<IFlightRepository, FlightDbRepository>();
            builder.Services.AddScoped<IUserRepository, UserDbRepository>();
            builder.Services.AddScoped<IFlightsService, FlightService>();
            builder.Services.AddScoped<ITicketService, TicketService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped(typeof(FlightDbRepository));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            using(var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Admin", "Clients" };

                foreach(var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<CustomUser>>();

                string email = "admin@admin.com";
                string password = "Test1234@";
                string FirstName = "Test";
                string LastName = "Test1";
                string passportNo = "123456";

                if(await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new CustomUser();
                    user.UserName = email;
                    user.Email = email;
                    user.FirstName = FirstName;
                    user.LastName= LastName;
                    user.PassportNo = passportNo;

                    await userManager.CreateAsync(user, password);

                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            app.Run();
        }
    }
}