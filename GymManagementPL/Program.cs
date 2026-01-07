using AutoMapper;
using GymManagementBL;
using GymManagementBL.Service.Class;
using GymManagementBL.Service.Interface;
using GymManagementDAL.Context;
using GymManagementDAL.GymDbContextSeeding;
using GymManagementDAL.Repositories.Class;
using GymManagementDAL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GymManagementPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register GymContext for Dependency Injection
            // also using the connection string placed in appsettings file
            builder.Services.AddDbContext<GymContext>(options =>
            {
                // You MUST reference the configuration key where your connection string is stored
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //builder.Services.AddScoped<IPlanRepsitory, PlanRepository>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            builder.Services.AddAutoMapper(x => x.AddProfile(new MappingProfile()));
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();


            var app = builder.Build();
            #region Data Seeding
            //using var scope = app.Services.CreateScope();
            //var dbcontext = scope.ServiceProvider.GetRequiredService<GymContext>();
            //var pendingMigrtations = dbcontext.Database.GetPendingMigrations();
            //if (pendingMigrtations?.Any() ?? false)
            //{
            //    dbcontext.Database.Migrate();
            //}
            //GymDbContextSeeding.SeedData(dbcontext, app.Environment.ContentRootPath);

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<GymContext>();
                var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

                GymDbContextSeeding.SeedData(context, env.ContentRootPath);
            }

            #endregion

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
