using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WOM_EYE.Interfaces.Login;
using WOM_EYE.Models.Login;
using Microsoft.AspNetCore.Http;
using WOM_EYE.DTOs.Auth;
using WOM_EYE.Interfaces.Users;
using WOM_EYE.Models.User;
using System.Text.RegularExpressions;

namespace WOM_EYE.Controllers
{
	public class LoginController : Controller
	{
		private string myUserId;
		private string myMUserId;

		private ILogin _loginProvider;
		private LoginModel _loginModel;
		private IUserProvider _userProvider;
		private UserModel _userModel;

		public LoginController(ILogin loginProvider, LoginModel loginModel, IUserProvider userProvider, UserModel userModel)
		{
			_loginProvider = loginProvider;
			_loginModel = loginModel;
			_userProvider = userProvider;
			_userModel = userModel;
		}

		[HttpGet]
		public IActionResult Index()
		{
			var user_id = HttpContext.Session.GetString("USER_ID");
			if (user_id == null)
			{
				_loginModel.responseCode = "xx";
				_loginModel.responseMessage = "xx";
				return View(_loginModel);
			}
			else
			{
				return RedirectToAction("Index", "Dashboard");
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Index([Bind]LoginModel form)
		{
			#region Validation Mandatory

			if (string.IsNullOrEmpty(form.USER_ID))
			{
				ModelState.AddModelError("USER_ID", "User ID is required");
			}
			if (string.IsNullOrEmpty(form.USER_PASS))
			{
				ModelState.AddModelError("USER_PASS", "User Pass is required");
			}
			#endregion
			if (ModelState.IsValid)
			{
				var resp = _loginProvider.getDataUser(form);

				if (resp.responseCode == "404")
				{
					_loginModel.responseMessage = resp.responseMessage;
					return View(_loginModel);
				}
				else if (resp.responseCode == "200")
				{
					_userModel = _userProvider.getDataUser(form.USER_ID);

					HttpContext.Session.SetString("M_WOMEYE_USER_ID", _userModel.M_WOMEYE_USER_ID.ToString());
					HttpContext.Session.SetString("USER_ID", _userModel.USER_ID);
					HttpContext.Session.SetString("USER_NIK_EMP", _userModel.USER_NIK_EMP);
					HttpContext.Session.SetString("USER_NIK_KTP", _userModel.USER_NIK_KTP);
					HttpContext.Session.SetString("USER_NAME", _userModel.USER_NAME);
					HttpContext.Session.SetString("USER_POSITION", _userModel.USER_POSITION);
					HttpContext.Session.SetString("LAST_USER_LOGIN", _userModel.LAST_USER_LOGIN);
					HttpContext.Session.SetString("LAST_USER_UPDATE", _userModel.LAST_USER_UPDATE);
					TempData["MyResponseCode"] = resp.responseCode;
					TempData["MyResponseMessage"] = resp.responseMessage;
					return RedirectToAction("Index", "Dashboard");
				}
				else if (resp.responseCode == "500")
				{
					_loginModel.responseMessage = resp.responseMessage;
					return View(_loginModel);

				}

			}
			_loginModel.responseCode = "xx";
			_loginModel.responseMessage = "xx";
			return View(_loginModel);
		}
	
		public IActionResult Logout()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("Index","Login");
		}

		[HttpGet]
		public IActionResult ChangePassword()
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

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult ChangePassword([Bind] ChangePasswordDTO form)
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
			if (string.IsNullOrEmpty(form.passOld))
			{
				ModelState.AddModelError("passOld", "Password Lama is required");
			}
			if (string.IsNullOrEmpty(form.passNew))
			{
				ModelState.AddModelError("passNew", "Password Baru is required");
			}
			if (string.IsNullOrEmpty(form.passConfirm))
			{
				ModelState.AddModelError("passConfirm", "Password Confirm is required");
				
			}
			if (!string.IsNullOrEmpty(form.passConfirm))
			{
				if (form.passNew != form.passConfirm)
				{
					ModelState.AddModelError("passConfirm", "Password Comfirm must same with password Baru");

				}
			}
			if (!string.IsNullOrEmpty(form.passNew))
			{

                var hasNumber = new Regex(@"[0-9]+");
                var hasUpperChar = new Regex(@"[A-Z]+");
                var hasMiniMaxChars = new Regex(@".{8,12}");
                var hasLowerChar = new Regex(@"[a-z]+");
                var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");


                List<string> errorMessages = new List<string>();

                if (!hasLowerChar.IsMatch(form.passNew))
                {
                    errorMessages.Add("Password Must Have Lower Case");
                }

                if (!hasUpperChar.IsMatch(form.passNew))
                {
                    errorMessages.Add("Password Must Have Upper Case");
                }

                if (!hasNumber.IsMatch(form.passNew))
                {
                    errorMessages.Add("Password Must Contain Number");
                }

                if (!hasSymbols.IsMatch(form.passNew))
                {
					errorMessages.Add("Password Must Contain Symbol");
                }

				if (!hasMiniMaxChars.IsMatch(form.passNew))
				{
					errorMessages.Add("Password Must Between 8 - 12 Character");
				}
				//           if (form.passNew.Length < 8)
				//           {
				//errorMessages.Add("Password Baru must have 8 Character");

				//           }
				//           if (form.passNew.Length > 12)
				//           {
				//errorMessages.Add("Password Baru just can have 12Character");

				//           }


				if (errorMessages.Any())
                {
					String allerror = "";
					// Password validation failed. Return error messages.
					foreach (var errorMessage in errorMessages)
                    {
						allerror = string.Join(", ", errorMessages);
                    }

					ModelState.AddModelError("passNew", allerror);

					return View();
                }


                //if (!hasLowerChar.IsMatch(form.passNew))
                //{
                //    ModelState.AddModelError("passNew", "Password Must Have Lower Case");
                //}
                //if (!hasUpperChar.IsMatch(form.passNew))
                //{
                //    ModelState.AddModelError("passNew", "Password Must Have Upper Case");
                //}

                //if (!hasNumber.IsMatch(form.passNew))
                //{
                //    ModelState.AddModelError("passNew", "Password Must Contain Number");
                //}

                //if (!hasSymbols.IsMatch(form.passNew))
                //{
                //    ModelState.AddModelError("passNew", "Password Must Contain Symbol");
                //}

                //if (form.passNew.Length < 8)
                //{
                //    ModelState.AddModelError("passNew", "Password Baru must have 8 Character");

                //}
                //if (form.passNew.Length > 12)
                //{
                //    ModelState.AddModelError("passNew", "Password Baru just can have 12Character");

                //}

            }
			#endregion

			if (ModelState.IsValid)
			{

				if (form.passNew.Equals(form.passConfirm))
				{
					var resp = _loginProvider.changePassword(form);
					if (resp.responseCode == "200")
					{
						TempData["MyResponseCode"] = resp.responseCode;
						TempData["MyResponseMessage"] = resp.responseMessage;
						return RedirectToAction("Index", "Dashboard");

					}
					else if (resp.responseCode == "500")
					{
						ViewBag.errorMessage = resp.errorMessage;
						return View();

					}
				}
			}
			return View();
		}
	}
}
