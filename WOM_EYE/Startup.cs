using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WOM_EYE.Interfaces.AddUser;
using WOM_EYE.Interfaces.Dashboard;
using WOM_EYE.Interfaces.Drop;
using WOM_EYE.Interfaces.Login;
using WOM_EYE.Interfaces.Notes;
using WOM_EYE.Interfaces.Progress;
using WOM_EYE.Interfaces.Projects;
using WOM_EYE.Interfaces.Users;
using WOM_EYE.Models.AddUser;
using WOM_EYE.Models.Dashboard;
using WOM_EYE.Models.Drop;
using WOM_EYE.Models.Login;
using WOM_EYE.Models.Notes;
using WOM_EYE.Models.Progress;
using WOM_EYE.Models.Projects;
using WOM_EYE.Models.User;
using WOM_EYE.Providers;
using WOM_EYE.Providers.AddUser;
using WOM_EYE.Providers.Dashboard;
using WOM_EYE.Providers.Drop;
using WOM_EYE.Providers.Login;
using WOM_EYE.Providers.Notes;
using WOM_EYE.Providers.Projects;
using WOM_EYE.Providers.Users;

namespace WOM_EYE
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
			services.AddControllersWithViews();

			// Model
			services.AddScoped<LoginModel, LoginModel>();
			services.AddScoped<UserModel, UserModel>();
			services.AddScoped<ProjectModel, ProjectModel>();
			services.AddScoped<DetailProjectModel, DetailProjectModel>();
			services.AddScoped<NoteModel, NoteModel>();
			services.AddScoped<ProgressModel, ProgressModel>();
			services.AddScoped<DropModel, DropModel>();
			services.AddScoped<DashboardModel, DashboardModel>();
			services.AddScoped<AddUserModel, AddUserModel>();

			// List Model
			services.AddScoped<List<UserModel>, List<UserModel>>();
			services.AddScoped<List<ProjectModel>, List<ProjectModel>>();
			services.AddScoped<List<DetailProjectModel>, List<DetailProjectModel>>();
			services.AddScoped<List<NoteModel>, List<NoteModel>>();
			services.AddScoped<List<ProgressModel>, List<ProgressModel>>();
			services.AddScoped<List<DropModel>, List<DropModel>>();
			services.AddScoped<List<DashboardModel>, List<DashboardModel>>();
			services.AddScoped<List<AddUserModel>, List<AddUserModel>>();

			// Select List
			services.AddScoped<List<SelectListUser>, List<SelectListUser>>();
			services.AddScoped<List<SelectListStatus>, List<SelectListStatus>>();
			services.AddScoped<List<SelectListStatus>, List<SelectListStatus>>();
			services.AddScoped<List<SelectListJenis>, List<SelectListJenis>>();
			services.AddScoped<List<SelectListStatusCatatan>, List<SelectListStatusCatatan>>();
			services.AddScoped<List<SelectListPosition>, List<SelectListPosition>>();

			// Services
			services.AddScoped<ILogin, LoginProvider>();
			services.AddScoped<IProjectProvider, ProjectProvider>();
			services.AddScoped<IUserProvider, UserProvider>();
			services.AddScoped<IDetailProjectProvider, DetailProjectProvider>();
			services.AddScoped<INoteProvider, NoteProvider>();
			services.AddScoped<IProgressProvider, ProgressProvider>();
			services.AddScoped<IDropProvider, DropProvider>();
			services.AddScoped<IDashboardProvider, DashboardProvider>();
			services.AddScoped<IAddUserProvider, AddUserProvider>();

			// Session
			services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromHours(2);
				options.Cookie.HttpOnly = true;
			});

			// Conection
			services.AddScoped<IDbConnection>((sp) => new SqlConnection(this.Configuration.GetConnectionString("DefaultConnection")));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

			app.UseAuthorization();

			app.UseSession();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Login}/{action=Index}/{id?}");
			});
		}
	}
}
