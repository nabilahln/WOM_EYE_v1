using Dapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WOM_EYE.Interfaces.Users;
using WOM_EYE.Models.User;

namespace WOM_EYE.Providers.Users
{
	public class UserProvider : IUserProvider
	{
		private IDbConnection _dbConnection;
		private UserModel _userModel;

		public UserProvider(
			IDbConnection dbConnection
			, UserModel userModel
			)
		{
			this._dbConnection = dbConnection;
			this._userModel = userModel;
		}

		public bool checkUserSession(String myUserId, String myMUserId)
		{

			if (myUserId == null || myMUserId == null)
			{
				return false;
			}

			return true;

		}

		public UserModel getDataUser(string userId)
		{
			var spName = "spWOMEYE_GetDataUser";

			var resp = _dbConnection.QueryFirstOrDefault<UserModel>(spName, new
			{
				@userId = userId
			},commandType: CommandType.StoredProcedure,commandTimeout:30);

			return resp;
		}
	}
}
