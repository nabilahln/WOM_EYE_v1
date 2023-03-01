using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WOM_EYE.Interfaces.AddUser;
using WOM_EYE.Interfaces.Users;
using WOM_EYE.Models.AddUser;
using WOM_EYE.Models.User;

namespace WOM_EYE.Controllers
{
    public class AddUserController : Controller
    {

        private string myUserId;
        private string myMUserId;
        private string myUsername;

        private readonly IDbConnection _dbConnection;
        private AddUserModel _AddUserModel;
        private IAddUserProvider _AddUserProvider;
        private IUserProvider _userProvider;
        private UserModel _userLogin;

        public AddUserController(IDbConnection dbConnection, 
                                AddUserModel AddUserModel,
                                IAddUserProvider AddUserProvider,
                                IUserProvider userProvider,
                                UserModel userLogin)
        {
            this._dbConnection = dbConnection;
            this._AddUserModel = AddUserModel;
            this._AddUserProvider = AddUserProvider;
            this._userProvider = userProvider;
            this._userLogin = userLogin;

        }
        public IActionResult Index()
        {
            _AddUserModel.listAddUser = _AddUserProvider.getAllAddUser();
            return View(_AddUserModel);
        }

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
            // _AddUserModel.ddlUser = _AddUserProvider.ddlUser();
            _AddUserModel.ddlPosition = _AddUserProvider.ddlPosition();
            return View(_AddUserModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] AddUserModel form)
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

            if (string.IsNullOrEmpty(form.USER_ID))
            {
                ModelState.AddModelError("USER_ID", "USER ID is required");
            }
            if (string.IsNullOrEmpty(form.USER_PASS))
            {
                ModelState.AddModelError("USER_PASS", "USER PASS is required");
                
            }

            #endregion

            #region Insert

            //int noProject = Int32.Parse(form.PROJECT_ID);

            if (ModelState.IsValid)
            {
                var resp = _AddUserProvider.InsertAddUser(form);

                if (resp.responseCodeAddUser == "200")
                {
                    _AddUserModel.responseCodeAddUser = resp.responseCodeAddUser;
                    _AddUserModel.responseMessageAddUser = resp.responseMessageAddUser;
                    _AddUserModel.listAddUser = _AddUserProvider.getAllAddUser();
                    return View("Index", _AddUserModel);
                }
                else 
                {
                    _AddUserModel.responseCodeAddUser = resp.responseCodeAddUser;
                    _AddUserModel.responseMessageAddUser = resp.responseMessageAddUser;
                    _AddUserModel.listAddUser = _AddUserProvider.getAllAddUser();
                    return View("Index", _AddUserModel);
                }
            }
            else
            {
                _AddUserModel.responseCodeAddUser = "400";
                _AddUserModel.responseMessageAddUser = "Validation Error";
                _AddUserModel.listAddUser = _AddUserProvider.getAllAddUser();
                return View(_AddUserModel);
            }
            #endregion

        }

        [HttpGet]
        public IActionResult Update(String id)
        {
            int UserId = Convert.ToInt32(id);
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
            _AddUserModel = _AddUserProvider.getDataUserById(UserId);
            _AddUserModel.ddlPosition = _AddUserProvider.ddlPosition();
            return View(_AddUserModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update([Bind] AddUserModel form)
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

            if (string.IsNullOrEmpty(form.USER_ID))
            {
                ModelState.AddModelError("USER_ID", "USER ID is required");
            }
            //if (string.IsNullOrEmpty(form.USER_PASS))
            //{
            //    ModelState.AddModelError("USER_PASS", "USER PASS is required");

            //}

            #endregion

            #region Update

            //int noProject = Int32.Parse(form.PROJECT_ID);

            if (ModelState.IsValid)
            {
                var resp = _AddUserProvider.UpdateAddUser(form);

                if (resp.responseCodeAddUser == "200")
                {
                    _AddUserModel.responseCodeAddUser = resp.responseCodeAddUser;
                    _AddUserModel.responseMessageAddUser = resp.responseMessageAddUser;
                    _AddUserModel.listAddUser = _AddUserProvider.getAllAddUser();
                    _AddUserModel.ddlPosition = _AddUserProvider.ddlPosition();
                    return View("Index", _AddUserModel);
                }
                else
                {
                    _AddUserModel.responseCodeAddUser = resp.responseCodeAddUser;
                    _AddUserModel.responseMessageAddUser = resp.responseMessageAddUser;
                    _AddUserModel.listAddUser = _AddUserProvider.getAllAddUser();
                    _AddUserModel.ddlPosition = _AddUserProvider.ddlPosition();
                    return View("Index", _AddUserModel);
                }
            }
            else
            {
                _AddUserModel.responseCodeAddUser = "400";
                _AddUserModel.responseMessageAddUser = "Validation Error";
                //_AddUserModel.listAddUser = _AddUserProvider.getAllAddUser();
                _AddUserModel.ddlPosition = _AddUserProvider.ddlPosition();
                return View(_AddUserModel);
            }
            #endregion

        }
    }
}
