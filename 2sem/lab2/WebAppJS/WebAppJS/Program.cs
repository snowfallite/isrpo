using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace WebAppJS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                )
            );
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".glb"] = "model/gltf+binary";
            provider.Mappings[".gltf"] = "model/gltf+json";
            provider.Mappings[".stl"] = "application/vnd.ms-pki.stl";
            app.UseStaticFiles(new StaticFileOptions
                {
                    ContentTypeProvider = provider
                }
            );

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
