using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WOM_EYE.Interfaces.Dashboard;
using WOM_EYE.Interfaces.Projects;
using WOM_EYE.Interfaces.Users;
using WOM_EYE.Models.Dashboard;
using WOM_EYE.Models.User;

namespace WOM_EYE.Controllers
{
	public class DashboardController : Controller
	{
		private string myUserId;
		private string myMUserId;
		private string myUsername;

		//private UserModel _userModel;
		private IDbConnection _dbConnection;
		private IUserProvider _userProvider;
		private IDashboardProvider _dashboardProvider;
		private List<DashboardModel> _listDashboard;
		private DashboardModel _dashboardModel;
		private IProjectProvider _projectProvider;

		public DashboardController(IDbConnection dbConnection, IUserProvider userProvider, IDashboardProvider dashboardProvider, List<DashboardModel> listDashboard, DashboardModel dashboardModel, IProjectProvider projectProvider)
		{
			_dbConnection = dbConnection;
			_userProvider = userProvider;
			_dashboardProvider = dashboardProvider;
			_listDashboard = listDashboard;
			_dashboardModel = dashboardModel;
			_projectProvider = projectProvider;
		}

		[HttpGet]
		public IActionResult Index()
		{

			#region CheckSession

			myUserId = HttpContext.Session.GetString("USER_ID");
			myMUserId = HttpContext.Session.GetString("M_WOMEYE_USER_ID");
			myUsername = HttpContext.Session.GetString("USER_NAME");

			if (_userProvider.checkUserSession(myUserId, myMUserId))
			{
				ViewBag.UserId = myUserId;
				ViewBag.MUserId = myMUserId;
				ViewBag.MUserName = myUsername;
			}
			else
			{
				return RedirectToAction("Index", "Login");
			}

			#endregion

			_dashboardModel.listDashboard = _dashboardProvider.getAllDashboard();
			_dashboardModel.listProject = _projectProvider.getAllProject();
			_dashboardModel.responseCode = TempData["MyResponseCode"] as string;
			_dashboardModel.responseMessage = TempData["MyResponseMessage"] as string;
			return View(_dashboardModel);
			//return View(_dashboardModel);
		}

	}
}
