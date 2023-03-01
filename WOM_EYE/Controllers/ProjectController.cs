using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using WOM_EYE.Interfaces.Progress;
using WOM_EYE.Interfaces.Projects;
using WOM_EYE.Interfaces.Users;
using WOM_EYE.Models.Progress;
using WOM_EYE.Models.Projects;
using WOM_EYE.Models.User;

namespace WOM_EYE.Controllers
{
    public class ProjectController : Controller
	{

		private string myUserId;
		private string myMUserId;
		private string myUsername;

		private IProjectProvider _projectProvider;
		private IDetailProjectProvider _detailProjectProvider;
		private IProgressProvider _progressProvider;
		private ProjectModel _projectModel;
		private List<ProjectModel> _listProject;
		private IUserProvider _userProvider;
		private UserModel _userLogin;

		public ProjectController(IProjectProvider projectProvider
			, IDetailProjectProvider detailProjectProvider
			, ProjectModel projectModel
			, List<ProjectModel> listProject
			, IUserProvider userProvider
			, IProgressProvider progressProvider
			, UserModel userLogin)

		{
			this._projectProvider = projectProvider;
			this._detailProjectProvider = detailProjectProvider;
			this._projectModel = projectModel;
			this._listProject = listProject;
			this._userProvider = userProvider;
			this._progressProvider = progressProvider;
			this._userLogin = userLogin;
		}

		[HttpGet]
		public ActionResult Index()
		{
			_projectModel.ListProject = _projectProvider.getAllProject();
			_projectModel.ddlUser = _projectProvider.ddlUser();
			_projectModel.ddlStatus = _projectProvider.ddlStatus();
			_projectModel.responseCodeProject = TempData["MyResponseCode"] as string;
			_projectModel.responseMessageProject = TempData["MyResponseMessage"] as string;

			#region CheckSession
			myUserId = HttpContext.Session.GetString("USER_ID");
			myMUserId = HttpContext.Session.GetString("M_WOMEYE_USER_ID");
			myUsername = HttpContext.Session.GetString("USER_NAME");
			_userLogin = _userProvider.getDataUser(myUserId);

			if (_userProvider.checkUserSession(myUserId, myMUserId))
			{
				ViewBag.UserId = myUserId;
				ViewBag.MUserId = myMUserId;
				ViewBag.MUserName = myUsername;
				ViewBag.UserLogin = _userLogin;
			}
			else
			{
				return RedirectToAction("Index", "Login");
			}
			#endregion


			return View(_projectModel);

		}

		[HttpGet]
		public IActionResult Detail(int id)
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

			HttpContext.Session.SetString("idProject", id.ToString());
			_projectModel = getProject(id);
			string programmerString = _projectModel.PROGRAMMER;
			string faString = _projectModel.FUNCTION_ANALYST;
			string[] programmerSplit = String.IsNullOrEmpty(programmerString) ? new string[] { } :
	 programmerString.Split(new char[] { ',' });
			string[] faSplit = String.IsNullOrEmpty(faString) ? new string[] { } :
	 faString.Split(new char[] { ',' });

			dynamic myModel = new ExpandoObject();
			myModel.project = getProject(id);
			myModel.detailProject = getListDetailProject(id);
			myModel.progress = getListProgress(id);
			myModel.listProgrammer = programmerSplit;
			myModel.listFA = faSplit;
			myModel.responseCodeProgress = TempData["MyResponseCodeProgress"] as string;
			myModel.responseMessageProgress = TempData["MyResponseMessageProgress"] as string;
			myModel.responseCodeNote = TempData["MyResponseCodeNote"] as string;
			myModel.responseMessageNote = TempData["MyResponseMessageNote"] as string;
			return View(myModel);
		}

		#region GET DYNAMIC MODEL DATA
		public ProjectModel getProject(int id)
		{
			var DProjectModel = _projectProvider.getDataProjectById(id);
			return DProjectModel;
		}

		public List<DetailProjectModel> getListDetailProject(int id)
		{
			var LDetailProjectModel = _detailProjectProvider.getAllDetailProject(id);
			return LDetailProjectModel;
		}

		public List<ProgressModel> getListProgress (int id)
		{
			var LProgressModel = _progressProvider.getAllProgress(id);
			return LProgressModel;
		}
		#endregion

		[HttpGet]
		public IActionResult Create()
		{
			#region CheckSession
			myUserId = HttpContext.Session.GetString("USER_ID");
			myMUserId = HttpContext.Session.GetString("M_WOMEYE_USER_ID");

			if (_userProvider.checkUserSession(myUserId, myMUserId))
			{
				ViewBag.UserId = myUserId;
				ViewBag.MUserId = myMUserId;
			}
			else
			{
				return RedirectToAction("Index", "Login");
			}
			#endregion

			_projectModel.ddlUser = _projectProvider.ddlUser();
			_projectModel.ddlStatus = _projectProvider.ddlStatus();
			_projectModel.ddlJenis = _projectProvider.ddlJenis();
			return View(_projectModel);

		}

		
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind] ProjectModel form)
		{
			#region CheckSession
			myUserId = HttpContext.Session.GetString("USER_ID");
			myMUserId = HttpContext.Session.GetString("M_WOMEYE_USER_ID");

			if (_userProvider.checkUserSession(myUserId, myMUserId))
			{
				ViewBag.UserId = myUserId;
				ViewBag.MUserId = myMUserId;
			}
			else
			{
				return RedirectToAction("Index", "Login");
			}
			#endregion

			#region Validation
			if (string.IsNullOrEmpty(form.SOL_LEADER))
			{
				ModelState.AddModelError("SOL_LEADER", "Sol Leader tidak boleh kosong");
			}
			if (string.IsNullOrEmpty(form.PROJECT_LEADER))
			{
				ModelState.AddModelError("PROJECT_LEADER", "Project Leader tidak boleh kosong");
			}
			if (string.IsNullOrEmpty(form.JENIS_PROJECT))
			{
				ModelState.AddModelError("JENIS_PROJECT", "Jenis Project tidak boleh kosong");
			}
			if (string.IsNullOrEmpty(form.NO_PROJECT))
			{
				ModelState.AddModelError("NO_PROJECT", "No Project tidak boleh kosong");
			}
			if (string.IsNullOrEmpty(form.DESKRIPSI))
			{
				ModelState.AddModelError("DESKRIPSI", "Deskripsi tidak boleh kosong");
			}
            if (string.IsNullOrEmpty(Request.Form["checkFA"]))
            {
                ModelState.AddModelError("FUNCTION_ANALYST", "Function Analyst tidak boleh kosong");
            }
            if (string.IsNullOrEmpty(Request.Form["checkProgrammer"]))
            {
                ModelState.AddModelError("PROGRAMMER", "Programmer tidak boleh kosong");
            }
            if (!string.IsNullOrEmpty(form.NO_PROJECT))
			{
				if (form.NO_PROJECT.Length > 50 )
				{
					ModelState.AddModelError("NO_PROJECT", "No Project just can have 50 character");
				}
			}
			
			if (!string.IsNullOrEmpty(form.DESKRIPSI))
			{
				if (form.DESKRIPSI.Length > 50)
				{
					ModelState.AddModelError("DESKRIPSI", "Deskripsi just can have 50 character");
				}

			}


			#endregion

			#region Insert

			string[] programmerChecked = Request.Form["checkProgrammer"];
			string[] faChecked = Request.Form["checkFA"];
			string delimiter = ",";

			form.PROGRAMMER = string.Join(delimiter, programmerChecked);
			form.FUNCTION_ANALYST = string.Join(delimiter, faChecked);

			if (ModelState.IsValid)
			{
				var resp = _projectProvider.InsertProject(form);

				if(resp.responseCodeProject == "200")
                {
					TempData["MyResponseCode"] = resp.responseCodeProject;
					TempData["MyResponseMessage"] = resp.responseMessageProject;
					_projectModel.ListProject = _projectProvider.getAllProject();
					_projectModel.ddlStatus = _projectProvider.ddlStatus();
					_projectModel.ddlUser = _projectProvider.ddlUser();
					_projectModel.ddlJenis = _projectProvider.ddlJenis();
					return RedirectToAction("Index");
				}
                else
                {
					_projectModel.responseCodeProject = resp.responseCodeProject;
					_projectModel.responseMessageProject= resp.responseMessageProject;
					_projectModel.errorMessageProject= resp.errorMessageProject;
					_projectModel.ListProject = _projectProvider.getAllProject();
					_projectModel.ddlStatus = _projectProvider.ddlStatus();
					_projectModel.ddlJenis = _projectProvider.ddlJenis();
					_projectModel.ddlUser = _projectProvider.ddlUser();
					return View(_projectModel);
				}

				
			}
			else
			{
				_projectModel.responseCodeProject = "400";
				_projectModel.responseMessageProject = "Validation Error";
				_projectModel.ddlUser = _projectProvider.ddlUser();
				_projectModel.ddlJenis = _projectProvider.ddlJenis();
				_projectModel.ddlStatus = _projectProvider.ddlStatus();
				return View(_projectModel);
			}
			#endregion
		}

		[HttpPost]
		public IActionResult Edit(String id)
		{
			#region CheckSession
			myUserId = HttpContext.Session.GetString("USER_ID");
			myMUserId = HttpContext.Session.GetString("M_WOMEYE_USER_ID");

			if (_userProvider.checkUserSession(myUserId, myMUserId))
			{
				ViewBag.UserId = myUserId;
				ViewBag.MUserId = myMUserId;
			}
			else
			{
				return RedirectToAction("Index", "Login");
			}
			#endregion

			int projectId = Convert.ToInt32(id);
			_projectModel = _projectProvider.getDataProjectByIdEdit(projectId);

			string programmerString = _projectModel.PROGRAMMER;
			string faString = _projectModel.FUNCTION_ANALYST;
			string[] programmerSplit = String.IsNullOrEmpty(programmerString) ? new string[] { } :
	 programmerString.Split(new char[] { ',' });
			string[] faSplit = String.IsNullOrEmpty(faString) ? new string[] { } :
	 faString.Split(new char[] { ',' });

			ViewBag.listProgrammer = programmerSplit;
			ViewBag.listFA = faSplit;

			_projectModel.ddlUser = _projectProvider.ddlUser();
			_projectModel.ddlStatus = _projectProvider.ddlStatus();
			_projectModel.ddlJenis = _projectProvider.ddlJenis();
			return View(_projectModel);
		}

		
		[HttpPost]
		public IActionResult EditValidation([Bind] ProjectModel form)
		{

			#region Validation
			if (string.IsNullOrEmpty(form.SOL_LEADER))
			{
				ModelState.AddModelError("SOL_LEADER", "Sol Leader tidak boleh kosong");
			}
			if (string.IsNullOrEmpty(form.PROJECT_LEADER))
			{
				ModelState.AddModelError("PROJECT_LEADER", "Project Leader tidak boleh kosong");
			}
			if (string.IsNullOrEmpty(form.NO_PROJECT))
			{
				ModelState.AddModelError("NO_PROJECT", "No Project tidak boleh kosong");
			}
			if (string.IsNullOrEmpty(form.DESKRIPSI))
			{
				ModelState.AddModelError("DESKRIPSI", "Deskripsi tidak boleh kosong");
			}
            if (string.IsNullOrEmpty(Request.Form["checkFA"]))
            {
                ModelState.AddModelError("FUNCTION_ANALYST", "Function Analyst tidak boleh kosong");
            }
            if (string.IsNullOrEmpty(Request.Form["checkProgrammer"]))
            {
                ModelState.AddModelError("PROGRAMMER", "Programmer tidak boleh kosong");
            }
            if (string.IsNullOrEmpty(form.STATUS))
			{
				ModelState.AddModelError("STATUS", "Status tidak boleh kosong");
			}
			//if (!string.IsNullOrEmpty(form.STATUS))
			//{
				
				
			//		if (string.IsNullOrEmpty(form.CATATAN))
			//		{
			//			ModelState.AddModelError("CATATAN", "Catatan tidak boleh kosong");
			//		}
				
			//}
			if (!string.IsNullOrEmpty(form.NO_PROJECT))
			{
				if (form.NO_PROJECT.Length > 50)
				{
					ModelState.AddModelError("NO_PROJECT", "No Project just can have 50 character");
				}
			}
			if (!string.IsNullOrEmpty(form.DESKRIPSI))
			{
				if (form.DESKRIPSI.Length > 50)
				{
					ModelState.AddModelError("DESKRIPSI", "Deskripsi just can have 50 character");
				}

			}



			#endregion

			#region Edit

			string[] programmerChecked = Request.Form["checkProgrammer"];
			string[] faChecked = Request.Form["checkFA"];
			string delimiter = ",";

			form.PROGRAMMER = string.Join(delimiter, programmerChecked);
			form.FUNCTION_ANALYST = string.Join(delimiter, faChecked);

			if (ModelState.IsValid)
			{
				var _ddlStatus = _projectModel.ddlStatus = _projectProvider.ddlStatus();
				string projectId = form.T_WOMEYE_PROJECT_ID.ToString();
				string formStatusKey = form.STATUS;
				string checkStatusKey = "";
				string checkStatusValue = "";

				for (int i = 1; i < _ddlStatus.Count; i++)
				{
					checkStatusKey = _ddlStatus[i].key;
					checkStatusValue = _ddlStatus[i].value;

					if(checkStatusValue == "Drop")
					{
						if (checkStatusKey.Equals(formStatusKey))
						{
							return RedirectToAction("Create", "Drop", new { projectId = projectId });
						}
					}
					 
				}

				var resp = _projectProvider.UpdateProject(form);

				TempData["MyResponseCode"] = resp.responseCodeProject;
				TempData["MyResponseMessage"] = resp.responseMessageProject;
				_projectModel.ListProject = _projectProvider.getAllProject();
				_projectModel.ddlStatus = _projectProvider.ddlStatus();
				_projectModel.ddlJenis = _projectProvider.ddlJenis();
				_projectModel.ddlUser = _projectProvider.ddlUser();
				return RedirectToAction("Index");
			}
			else
			{
				_projectModel.responseCodeProject = "400";
				_projectModel.responseMessageProject = "Validation Error";
				_projectModel.ddlUser = _projectProvider.ddlUser();
				_projectModel.ddlJenis = _projectProvider.ddlJenis();
				_projectModel.ddlStatus = _projectProvider.ddlStatus();

				string programmerString = _projectModel.PROGRAMMER;
				string faString = _projectModel.FUNCTION_ANALYST;
				string[] programmerSplit = String.IsNullOrEmpty(programmerString) ? new string[] { } :
		 programmerString.Split(new char[] { ',' });
				string[] faSplit = String.IsNullOrEmpty(faString) ? new string[] { } :
		 faString.Split(new char[] { ',' });

				ViewBag.listProgrammer = programmerSplit;
				ViewBag.listFA = faSplit;


				return View("Edit", _projectModel);
			}

			#endregion
		
		}


	}
}
