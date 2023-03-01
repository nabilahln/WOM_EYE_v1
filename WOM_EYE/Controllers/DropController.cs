using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WOM_EYE.Interfaces.Drop;
using WOM_EYE.Interfaces.Users;
using WOM_EYE.Models.Drop;

namespace WOM_EYE.Controllers
{
	public class DropController : Controller
	{
		private string myUserId;
		private string myMUserId;

		private IDropProvider _dropProvider;
		private DropModel _dropModel;
		private List<DropModel> _listDrop;
		private IUserProvider _userProvider;

		public DropController(IDropProvider dropProvider, DropModel dropModel, List<DropModel> listDrop, IUserProvider userProvider)
		{
			_dropProvider = dropProvider;
			_dropModel = dropModel;
			_listDrop = listDrop;
			_userProvider = userProvider;
		}


		public IActionResult Index()
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

			_dropModel.ListDrop = _dropProvider.getAllDrop();

			return View(_dropModel);
		}

		[HttpGet]

		public IActionResult Undrop(string dropId)
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

			#region UnDrop

			if (ModelState.IsValid)
			{
				var resp = _dropProvider.UnDropProject(dropId);

				_dropModel.responseCodeDrop = resp.responseCodeDrop;
				_dropModel.responseMessageDrop = resp.responseMessageDrop;
				_dropModel.ListDrop = _dropProvider.getAllDrop();

			}
			else
			{
				_dropModel.responseCodeDrop = "400";
				_dropModel.responseMessageDrop = "Validation Error";
				_dropModel.ListDrop = _dropProvider.getAllDrop();

			}

			#endregion

			return View("Index", _dropModel);
		}


		public IActionResult Create(string projectId)
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

			ViewBag.idProject = projectId;

			_dropModel.PROJECT_ID = projectId;

			return View(_dropModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create([Bind] DropModel form)
		{
			#region Validation
			if (string.IsNullOrEmpty(form.ALASAN))
			{
				ModelState.AddModelError("ALASAN", "Alasan tidak boleh kosong");
			}
			#endregion

			#region InsertDrop

			if (ModelState.IsValid)
			{
				var resp = _dropProvider.DropProject(form);

				_dropModel.responseCodeDrop = resp.responseCodeDrop;
				_dropModel.responseMessageDrop = resp.responseMessageDrop;
				_dropModel.ListDrop = _dropProvider.getAllDrop();
				//return RedirectToAction("Index", "Drop", _dropModel);
				return View("Index", _dropModel);
			}
			else
			{
				_dropModel.responseCodeDrop = "400";
				_dropModel.responseMessageDrop = "Validation Error";
				_dropModel.ListDrop = _dropProvider.getAllDrop();
				return View("Create", _dropModel);
			}

			#endregion



		}

	}
}
