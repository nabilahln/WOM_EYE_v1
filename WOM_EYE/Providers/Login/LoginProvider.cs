using Dapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WOM_EYE.DTOs.Auth;
using WOM_EYE.DTOs.Login;
using WOM_EYE.Interfaces.Login;
using WOM_EYE.Models.Login;
using WOM_EYE.Models.User;

namespace WOM_EYE.Providers.Login
{
	public class LoginProvider : ILogin
	{
		private IDbConnection _dbConnection;
		private LoginModel _loginModel;
		private UserModel _userModel;

		public LoginProvider(
			IDbConnection dbConnection,
			LoginModel loginModel,
			UserModel userModel
			)
		{
			this._dbConnection = dbConnection;
			this._loginModel = loginModel;
			this._userModel = userModel;
		}

		public ResponseMessage changePassword(ChangePasswordDTO form)
		{
			string spName = "spWOMEYE_ChangePassword";

			try
			{
				var resp = _dbConnection.QueryFirstOrDefault<ResponseMessage>(spName, new
				{
					@userId = form.userId,
					@passOld = form.passOld,
					@passNew = form.passNew
				}, commandType: CommandType.StoredProcedure, commandTimeout: 30);

				return resp;
			}
			catch (Exception e)
			{
				ResponseMessage resp = new ResponseMessage();
				resp.responseCode = "500";
				resp.responseMessage = e.Message;
				return resp;
			}
		}

		public LoginModel getDataUser(LoginModel form)
		{
			string spName = "spWOMEYE_Login";

			var resp = _dbConnection.QueryFirstOrDefault<LoginModel>(spName, new
			{
				@USER_ID = form.USER_ID,
				@USER_PASS = form.USER_PASS
			}, commandType: CommandType.StoredProcedure, commandTimeout: 30);

			return resp;
		}
	}
}
