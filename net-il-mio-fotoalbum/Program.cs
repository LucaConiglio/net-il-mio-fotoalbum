using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using net_il_mio_fotoalbum.Models;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Areas.Identity.Data;
using net_il_mio_fotoalbum.Controllers.api;

namespace net_il_mio_fotoalbum
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
                        var connectionString = builder.Configuration.GetConnectionString("ProfileContextConnection") ?? throw new InvalidOperationException("Connection string 'ProfileContextConnection' not found.");

            builder.Services.AddDbContext<FotoContext>();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<FotoContext>();
            // Add services to the container.
            builder.Services.AddControllersWithViews();

                        builder.Services.AddEndpointsApiExplorer();

                        builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

                        if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

                        

            app.Run();
        }
    }
}