using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;

namespace VenturaHR
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
            #region [CultureInfo]
            var cultureInfo = new CultureInfo("pt-BR");

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            #endregion

            #region [ApplicationDbContext]
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext.Context.ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            #endregion

            #region [AspNetIdentity]
            services.AddDbContext<AspNetIdentityDbContext.ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            services.AddIdentity<AspNetIdentityDbContext.User, AspNetIdentityDbContext.Role>()
                    .AddEntityFrameworkStores<AspNetIdentityDbContext.ApplicationDbContext>();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

            });
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/Home";
                options.SlidingExpiration = true;
            });
            #endregion

            #region [Services]
            services.AddScoped<Helpers.DropDownListHelper>();
            services.AddScoped<Helpers.ViewEngineHelper>();

            services.AddScoped<Services.User.UserService>();
            services.AddScoped<Services.User.UserListService>();

            services.AddScoped<Services.Interface.IPersonService,Services.Person.PersonService>();
            services.AddScoped<Services.Person.PersonTypeService>();

            services.AddScoped<Services.Interface.IOpportunityService, Services.Opportunity.OpportunityService>();
            services.AddScoped<Services.Opportunity.OpportunityListService>();

            services.AddScoped <Services.Response.ResponseService>();
            services.AddScoped <Services.OpportunityCriterion.OpportunityCriterionService>();
            services.AddScoped <Services.ResponseCriterion.ResponseCriterionService>();

            services.AddScoped<Services.Mail.MailService>();

            services.AddHostedService<Services.BackgroundServices.ExpiredOpportunityService>();
            services.AddHostedService<Services.BackgroundServices.FinishedOpportunityService>();
            #endregion

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RoleManager<AspNetIdentityDbContext.Role> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

            CreateAspNetIdentityRoles(roleManager);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void CreateAspNetIdentityRoles(RoleManager<AspNetIdentityDbContext.Role> roleManager)
        {
            try
            {
                foreach (var role in new string[] { "Administrator", "Company", "Candidate"})
                    if (!roleManager.RoleExistsAsync(role).Result) { var identityResult = roleManager.CreateAsync(new AspNetIdentityDbContext.Role() { Name = role, Description = role }).Result; }
            }
            catch (Exception exception) { }
        }
    }
}
