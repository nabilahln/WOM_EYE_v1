using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WOM_EYE.Interfaces.Notes;
using WOM_EYE.Interfaces.Projects;
using WOM_EYE.Interfaces.Users;
using WOM_EYE.Models.Notes;
using WOM_EYE.Models.Projects;
using WOM_EYE.Models.User;

namespace WOM_EYE.Controllers
{
	public class NoteController : Controller
	{
		private string myUserId;
		private string myMUserId;

		private INoteProvider _noteProvider;
		private NoteModel _noteModel;
		private UserModel _userLogin;
		private ProjectModel _projectModel;
		private DetailProjectModel _detailProjectModel;

		private List<NoteModel> _listNote;

		private IUserProvider _userProvider;
		private IProjectProvider _projectProvider;
		private IDetailProjectProvider _detailProjectProvider;

		public NoteController(INoteProvider noteProvider, NoteModel noteModel, List<NoteModel> listNote, IUserProvider userProvider, UserModel userLogin, IProjectProvider projectProvider, ProjectModel projectModel, DetailProjectModel detailProjectModel, IDetailProjectProvider detailProjectProvider)
		{
			_noteProvider = noteProvider;
			_noteModel = noteModel;
			_listNote = listNote;
			_userProvider = userProvider;
			_userLogin = userLogin;
			_projectProvider = projectProvider;
			_projectModel = projectModel;
			_detailProjectModel = detailProjectModel;
			_detailProjectProvider = detailProjectProvider;
		}

		[HttpPost]
		public IActionResult Index(string id, string tahap)
		{
			#region CheckSession
			myUserId = HttpContext.Session.GetString("USER_ID");
			myMUserId = HttpContext.Session.GetString("M_WOMEYE_USER_ID");
			_userLogin = _userProvider.getDataUser(myUserId);

			if (_userProvider.checkUserSession(myUserId, myMUserId))
			{
				ViewBag.UserId = myUserId;
				ViewBag.MUserId = myMUserId;
				ViewBag.UserLogin = _userLogin.USER_NAME;
			}
			else
			{
				return RedirectToAction("Index", "Login");
			}
			#endregion

			int detailProjectId = Convert.ToInt32(id);
			int projectId = 0;
			TempData["TAHAP"] = tahap;

			_noteModel.ListCatatan = _noteProvider.getAllCatatan(detailProjectId);
			_detailProjectModel = _detailProjectProvider.getDetailProjectById(detailProjectId);
			Int32.TryParse(_detailProjectModel.PROJECT_ID, out projectId);
			_projectModel = _projectProvider.getDataProjectById(projectId);


			string programmerString = _projectModel.PROGRAMMER;
			string faString = _projectModel.FUNCTION_ANALYST;
			string[] programmerSplit = String.IsNullOrEmpty(programmerString) ? new string[] { } : programmerString.Split(new char[] { ',' });
			string[] faSplit = String.IsNullOrEmpty(faString) ? new string[] { } : faString.Split(new char[] { ',' });

			ViewBag.detailProjectId = id;
			ViewBag.namaTahap = tahap;
			ViewBag.project = _projectModel;
			ViewBag.listProgrammer = programmerSplit;
			ViewBag.listFa = faSplit;

			return View(_noteModel);
		}

		[HttpPost]
		public IActionResult Create(string id)
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


			_noteModel.ddlStatusCatatan = _noteProvider.ddlStatusCatatan();
			_noteModel.DETAIL_PROJECT_ID = id;
			_noteModel.USR_CRT = myUserId;
			return View(_noteModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult CreateValidation([Bind] NoteModel form)
		{
			#region Validation

			if (string.IsNullOrEmpty(form.STATUS_ID))
			{
				ModelState.AddModelError("STATUS_ID", "Status tidak boleh kosong");
			}
			if (string.IsNullOrEmpty(form.NOTES))
			{
				ModelState.AddModelError("NOTES", "NOTES is required");
			}
			if (!string.IsNullOrEmpty(form.NOTES))
			{
				if (form.NOTES.Length > 200)
				{
					ModelState.AddModelError("NOTES", "Notes just can have 200 character");
				}
			}
			#endregion

			#region Insert Note

			if (ModelState.IsValid)
			{
				var resp = _noteProvider.InsertCatatan(form);
				var idProject = HttpContext.Session.GetString("idProject");

				if (resp.responseCode == "200")
				{
					TempData["MyResponseCodeNote"] = resp.responseCode;
					TempData["MyResponseMessageNote"] = resp.responseMessage;
					_noteModel.ListCatatan = _noteProvider.getAllCatatan(Int32.Parse(form.DETAIL_PROJECT_ID));
					_noteModel.ddlStatusCatatan = _noteProvider.ddlStatusCatatan();

					return RedirectToAction("Detail", "Project", new { id = idProject });

				}
				else
				{
					TempData["MyResponseCodeNote"] = resp.responseCode;
					TempData["MyResponseMessageNote"] = resp.responseMessage;
					_noteModel.ListCatatan = _noteProvider.getAllCatatan(Int32.Parse(form.DETAIL_PROJECT_ID));
					_noteModel.ddlStatusCatatan = _noteProvider.ddlStatusCatatan();

					return RedirectToAction("Detail", "Project", new { id = idProject });
				}
			}
			else
			{
				_noteModel.responseCode = "400";
				_noteModel.responseMessage = "Validation Error";
				_noteModel.ListCatatan = _noteProvider.getAllCatatan(Int32.Parse(form.DETAIL_PROJECT_ID));
				_noteModel.ddlStatusCatatan = _noteProvider.ddlStatusCatatan();
				return View("Create", _noteModel);
			}

			#endregion


		}

		[HttpPost]
		public IActionResult Edit(int id, string tahap)
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

			ViewBag.namaTahap = tahap;
			_noteModel = _noteProvider.getDataCatatanById(id);
			_noteModel.ddlStatusCatatan = _noteProvider.ddlStatusCatatan();
			_noteModel.USR_UPD = myUserId;

			return View(_noteModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult EditValidation([Bind] NoteModel form)
		{
			#region Validation
			if (string.IsNullOrEmpty(form.STATUS_ID))
			{
				ModelState.AddModelError("STATUS_ID", "STATUS_ID tidak boleh kosong");
			}
			if (string.IsNullOrEmpty(form.NOTES))
			{
				ModelState.AddModelError("NOTES", "NOTES tidak boleh kosong");
			}
			if (!string.IsNullOrEmpty(form.NOTES))
			{
				if (form.NOTES.Length > 200)
				{
					ModelState.AddModelError("NOTES", "Notes just can have 200 character");
				}
			}

			#endregion

			#region Edit

			if (ModelState.IsValid)
			{
				var resp = _noteProvider.UpdateCatatan(form);

				var idProject = HttpContext.Session.GetString("idProject");

				if (resp.responseCode == "200")
				{
					TempData["MyResponseCodeNote"] = resp.responseCode;
					TempData["MyResponseMessageNote"] = resp.responseMessage;

					_noteModel.ListCatatan = _noteProvider.getAllCatatan(Int32.Parse(form.DETAIL_PROJECT_ID));
					_noteModel.ddlStatusCatatan = _noteProvider.ddlStatusCatatan();
					return RedirectToAction("Detail", "Project", new { id = idProject });
				}
				else
				{
					_noteModel.responseCode = resp.responseCode;
					_noteModel.responseMessage = resp.responseMessage;
					_noteModel.ListCatatan = _noteProvider.getAllCatatan(Int32.Parse(form.DETAIL_PROJECT_ID));
					_noteModel.ddlStatusCatatan = _noteProvider.ddlStatusCatatan();
					return View("Edit", _noteModel);
				}

			}
			else
			{
				_noteModel.responseCode = "400";
				_noteModel.responseMessage = "Validation Error";
				_noteModel.ListCatatan = _noteProvider.getAllCatatan(Int32.Parse(form.DETAIL_PROJECT_ID));
				_noteModel.ddlStatusCatatan = _noteProvider.ddlStatusCatatan();
				return View("Edit", _noteModel);
			}

			#endregion


		}

	}
}
