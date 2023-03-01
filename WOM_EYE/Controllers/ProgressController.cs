using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WOM_EYE.Interfaces.Progress;
using WOM_EYE.Interfaces.Users;
using WOM_EYE.Models.Progress;

namespace WOM_EYE.Controllers
{
	public class ProgressController : Controller
	{
	
		private string myUserId;
		private string myMUserId;

		private readonly IDbConnection _dbConnection;
		private ProgressModel _progressModel;
		private IProgressProvider _progressProvider;
		private IUserProvider _userProvider;

		public ProgressController(IDbConnection dbConnection, ProgressModel progressModel, IProgressProvider progressProvider, IUserProvider userProvider)
		{
			_dbConnection = dbConnection;
			_progressModel = progressModel;
			_progressProvider = progressProvider;
			_userProvider = userProvider;
		}

		[HttpPost]
		public IActionResult Create(int noProject)
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

			_progressModel.PROJECT_ID = noProject.ToString();
			_progressModel.USR_CRT = myUserId;
			return View(_progressModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult CreateValidation([Bind] ProgressModel form)
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

            #region Validation Mandatory

            if (string.IsNullOrEmpty(form.DESKRIPSI))
            {
                ModelState.AddModelError("DESKRIPSI", "DESKRIPSI is required");
            }
			if (!string.IsNullOrEmpty(form.DESKRIPSI))
			{
				if (form.DESKRIPSI.Length > 100)
				{
					ModelState.AddModelError("NOTES", "Notes just can have 200 character");
				}
			}

			#endregion

			#region Insert

			int noProject = Int32.Parse(form.PROJECT_ID);

			if (ModelState.IsValid)
			{
				var resp = _progressProvider.InsertDataProgress(form);

				if(resp.responseCodeProgress == "200")
                {
					TempData["MyResponseCodeProgress"] = resp.responseCodeProgress;
					TempData["MyResponseMessageProgress"] = resp.responseMessageProgress;
					_progressModel.ListProgress = _progressProvider.getAllProgress(noProject);
					return RedirectToAction("Detail","Project", new { id=form.PROJECT_ID});
				}
                else
                {
					_progressModel.responseCodeProgress = resp.responseCodeProgress;
					_progressModel.responseMessageProgress = resp.responseMessageProgress;
					_progressModel.ListProgress = _progressProvider.getAllProgress(noProject);
					return View("Create", _progressModel);
				}
			}
			else
			{
				_progressModel.responseCodeProgress = "400";
				_progressModel.responseMessageProgress = "Validation Error";
				_progressModel.ListProgress = _progressProvider.getAllProgress(noProject);
				return View("Create", _progressModel);
			}
			#endregion
			
		}
	
		[HttpPost]
		public IActionResult Edit(int id)
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

			_progressModel = _progressProvider.getDataProgressById(id);
			_progressModel.USR_UPD = myUserId;

			return View(_progressModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult EditValidation([Bind] ProgressModel form)
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

            #region Validasi

            if (string.IsNullOrEmpty(form.DESKRIPSI))
            {
                ModelState.AddModelError("DESKRIPSI", "DESKRIPSI is required");
            }
			if (!string.IsNullOrEmpty(form.DESKRIPSI))
			{
				if (form.DESKRIPSI.Length > 100)
				{
					ModelState.AddModelError("NOTES", "Notes just can have 200 character");
				}
			}
			#endregion

			#region Edit

			int noProject = Int32.Parse(form.PROJECT_ID);
			if (ModelState.IsValid)
			{
				var resp = _progressProvider.UpdateProgress(form);

				if (resp.responseCodeProgress == "200")
				{
					TempData["MyResponseCodeProgress"] = resp.responseCodeProgress;
					TempData["MyResponseMessageProgress"] = resp.responseMessageProgress;

					_progressModel.ListProgress = _progressProvider.getAllProgress(noProject);
					return RedirectToAction("Detail", "Project", new { id = form.PROJECT_ID });
				}
				else
				{
					_progressModel.responseCodeProgress = resp.responseCodeProgress;
					_progressModel.responseMessageProgress = resp.responseMessageProgress;
					_progressModel.ListProgress = _progressProvider.getAllProgress(noProject);
					return View("Edit", _progressModel);
				}
				
			}
			else
			{
				_progressModel.responseCodeProgress = "400";
				_progressModel.responseMessageProgress = "Validation Error";
				_progressModel.ListProgress = _progressProvider.getAllProgress(noProject);
				return View("Edit", _progressModel);
			}

			#endregion

			

		}

		public IActionResult Delete(int id)
		{
			var resp = _progressProvider.DeleteProgress(id);

			return Json(new { response = resp });
		}

	}
}
