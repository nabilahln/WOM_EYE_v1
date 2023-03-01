using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WOM_EYE.Interfaces.Projects;
using WOM_EYE.Interfaces.Users;
using WOM_EYE.Models.Projects;

namespace WOM_EYE.Controllers
{
	public class DetailProjectController : Controller
	{
		private string myUserId;
		private string myMUserId;

		private IDetailProjectProvider _detailProjectProvider;
		private IProjectProvider _projectProvider;
		private DetailProjectModel _detailProjectModel;
		private IUserProvider _userProvider;

		public DetailProjectController(IDetailProjectProvider detailProjectProvider, IProjectProvider projectProvider, DetailProjectModel detailProjectModel, IUserProvider userProvider)
		{
			this._detailProjectProvider = detailProjectProvider;
			this._projectProvider = projectProvider;
			this._detailProjectModel = detailProjectModel;
			this._userProvider = userProvider;
		}


		[HttpGet]
		public IActionResult Edit(string id)
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

			int detailProjectId = Convert.ToInt32(id);
			_detailProjectModel = _detailProjectProvider.getDetailProjectById(detailProjectId);
			_detailProjectModel.ddlStatus = _projectProvider.ddlStatus();
			_detailProjectModel.responseCodeProject = TempData["MyResponseCode"] as string;
			_detailProjectModel.responseMessageProject = TempData["MyResponseMessage"] as string;
			return View(_detailProjectModel);
		}

		[HttpPost]
		public IActionResult Edit([Bind] DetailProjectModel form)
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
			if (string.IsNullOrEmpty(form.TAHAP))
			{
				ModelState.AddModelError("TAHAP", "Tahap tidak boleh kosong");
			}
			if (string.IsNullOrEmpty(form.DOKUMEN))
			{
				ModelState.AddModelError("DOKUMEN", "Dokumen tidak boleh kosong");
			}
			if (string.IsNullOrEmpty(form.START_DT))
			{
				ModelState.AddModelError("START_DT", "Tanggal mulai tidak boleh kosong");
			}
			if (string.IsNullOrEmpty(form.END_DT))
			{
				ModelState.AddModelError("END_DT", "Tanggal selesai tidak boleh kosong");
			}
			if (string.IsNullOrEmpty(form.STATUS_PROJECT))
			{
				ModelState.AddModelError("STATUS_PROJECT", "Status Project tidak boleh kosong");
			}
			if (string.IsNullOrEmpty(form.KETERANGAN))
			{
				ModelState.AddModelError("KETERANGAN", "Keterangan tidak boleh kosong");
			}
			if (!string.IsNullOrEmpty(form.START_DT))
			{
				if (DateTime.Parse(form.START_DT) > DateTime.Parse(form.END_DT))
				{
					ModelState.AddModelError("START_DT", "Tanggal mulai tidak bisa lebih besar dari tanggal selesai");
				}
			}
			if (!string.IsNullOrEmpty(form.END_DT))
			{
				if (DateTime.Parse(form.START_DT) == DateTime.Parse(form.END_DT))
				{
					ModelState.AddModelError("END_DT", "Tanggal Selesai tidak dapat sama dengan tanggal mulai");
				}
			}
			if (!string.IsNullOrEmpty(form.DOKUMEN))
			{
				if (form.DOKUMEN.Length > 100)
				{
					ModelState.AddModelError("DOKUMEN", "Dokumen just can have 100 character");
				}
			}

			string checkHold = "Hold " + form.TAHAP;
			_detailProjectModel.ddlStatus = _projectProvider.ddlStatus();
			string statusName = "";
			foreach (var status in _detailProjectModel.ddlStatus)
			{
				if (status.key.Equals(form.STATUS_PROJECT))
				{
					statusName = status.value;
				}
			}

			if (statusName == checkHold)
			{
				if (string.IsNullOrEmpty(form.CATATAN))
				{
					ModelState.AddModelError("CATATAN", "Catatan tidak boleh kosong");
				}

				if (!string.IsNullOrEmpty(form.CATATAN))
				{
					if (form.CATATAN.Length > 50)
					{
						ModelState.AddModelError("CATATAN", "Catatan Max 50 karakter, Gunakan fitur catatan untuk lebih detail");
					}
				}
			}

			

			#endregion

			#region Edit

			if (ModelState.IsValid)
			{
				var resp = _detailProjectProvider.UpdateDetailProject(form);

				if (resp.responseCodeProject == "200")
				{
					TempData["MyResponseCode"] = resp.responseCodeProject;
					TempData["MyResponseMessage"] = resp.responseMessageProject;
					_detailProjectModel.ddlStatus = _projectProvider.ddlStatus();
					_detailProjectModel.ddlUser = _projectProvider.ddlUser();
					return RedirectToAction("Index", "Project");
				}
				else
				{
					TempData["MyResponseCode"] = resp.responseCodeProject;
					TempData["MyResponseMessage"] = resp.responseMessageProject;
					_detailProjectModel.ddlStatus = _projectProvider.ddlStatus();
					_detailProjectModel.ddlUser = _projectProvider.ddlUser();
					//return RedirectToAction("Edit", new { id = form.T_WOMEYE_DETAIL_PROJECT_ID });
					return Edit(form.T_WOMEYE_DETAIL_PROJECT_ID.ToString());
					//return View(_detailProjectModel);
				}

			}
			else
			{
				_detailProjectModel.responseCodeProject = "400";
				_detailProjectModel.responseMessageProject = "Validation Error tes";
				//_detailProjectModel.ddlUser = _projectProvider.ddlUser();
				//_detailProjectModel.ddlStatus = _projectProvider.ddlStatus();
				//_detailProjectModel = _detailProjectProvider.getDetailProjectById(form.T_WOMEYE_DETAIL_PROJECT_ID);
				_detailProjectModel.TAHAP = form.TAHAP;

				//return View("Edit", _detailProjectModel);
				return View(_detailProjectModel);

				//return RedirectToAction("Edit", new { id = form.T_WOMEYE_DETAIL_PROJECT_ID.ToString() });
				//return Edit(form.T_WOMEYE_DETAIL_PROJECT_ID.ToString());
			}

			#endregion


		}
	}
}
