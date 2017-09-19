using System;
using ImageManager.Data;
using ImageManager.Data.Domains;
using ImageManager.Data.Seeds;
using ImageManager.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImageManager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<NeptuneContext>(options => options.UseSqlite("Data Source=neptune.db"));
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<NeptuneContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            services.AddSession();

            services.AddTransient<UserService>();
            services.AddTransient<CategoryService>();
            services.AddTransient<AlbumService>();
            services.AddTransient<ImageService>();
            services.AddTransient<Seeder>();

            var userOptions = new UserOptions {RequireUniqueEmail = true};

            var passwordOptions = new PasswordOptions
            {
                RequireDigit = false,
                RequiredLength = 1,
                RequireNonAlphanumeric = false,
                RequireUppercase = false,
                RequireLowercase = false
            };

            var lockoutOptions = new LockoutOptions
            {
                DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1),
                MaxFailedAccessAttempts = 10
            };

            services.Configure<IdentityOptions>(options =>
            {
                options.Password = passwordOptions;
                options.Lockout = lockoutOptions;
                options.User = userOptions;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, Seeder seeder,
            NeptuneContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();

            await seeder.InitializeAsync(context);
        }
    }
}