using Repositories.Interfaces;
using Repositories;
using Services;
using Services.Interfaces;
using DataAccessLayer;
using BusinessObjects;


namespace BBMSSolution
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();
            builder.Services.AddDbContext<BadmintonBookingSystemContext>();

            // Xóa DAO, link page xuống service layers
            //builder.Services.AddScoped<UserDAO>();
            //builder.Services.AddScoped<BookingDAO>();
            //builder.Services.AddScoped<BookingServiceDAO>();
            //builder.Services.AddScoped<CourtDAO>();


            //BookingService
            builder.Services.AddScoped<IBookingServiceService, BookingServiceService>();
            builder.Services.AddScoped<IBookingServiceRepository, BookingServiceRepository>();
            //User
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            //Booking
            builder.Services.AddScoped<IBookingService, Services.BookingService>();
            builder.Services.AddScoped<IBookingReppository, BookingRepository>();
            //Court
            builder.Services.AddScoped<ICourtRepository, CourtRepository>();
            builder.Services.AddScoped<ICourtService, CourtService>();
            //Service
            /*builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
            builder.Services.AddScoped<IServiceService, ServiceService>();*/

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}