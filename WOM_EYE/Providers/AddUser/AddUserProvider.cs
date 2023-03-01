using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WOM_EYE.Interfaces.AddUser;
using WOM_EYE.Models.AddUser;

namespace WOM_EYE.Providers.AddUser
{
    public class AddUserProvider : IAddUserProvider
    {
        private IDbConnection _dbConnection;
        private AddUserModel _AddUserModel;
        private List<AddUserModel> _listAddUser;
		private List<SelectListPosition> _listPosition;

        public AddUserProvider(IDbConnection dbConnection, AddUserModel AddUserModel, List<AddUserModel> listAddUser, List<SelectListPosition> listPosition)
        {
            _dbConnection = dbConnection;
            _AddUserModel = AddUserModel;
            _listAddUser = listAddUser;
            _listPosition = listPosition;
        }

        public List<AddUserModel> getAllAddUser()
		{
			string spName = "spWOMEYE_GetAllUser";


			var dataAddUser = _dbConnection.ExecuteReader(spName, commandType: CommandType.StoredProcedure, commandTimeout: 30);

			while (dataAddUser.Read())
			{
				AddUserModel AddUser = new AddUserModel();
				AddUser.M_WOMEYE_USER_ID = Convert.ToInt32(dataAddUser[0]);
				AddUser.USER_ID = dataAddUser[1].ToString();
				AddUser.USER_NAME = dataAddUser[2].ToString();
				AddUser.USER_NIK_EMP = dataAddUser[3].ToString();
				AddUser.USER_NIK_KTP = dataAddUser[4].ToString();
				AddUser.USER_POSITION = dataAddUser[5].ToString();
				//AddUser.USER_PASS = dataAddUser[6].ToString();
				AddUser.DIVISION = dataAddUser[6].ToString();
				AddUser.PHONE = dataAddUser[7].ToString();
				AddUser.GENDER = dataAddUser[8].ToString();
				AddUser.BOD = dataAddUser[9].ToString();
				//AddUser.LAST_USER_LOGIN = dataAddUser[3].ToString();
				//AddUser.LAST_USER_UPDATE = dataAddUser[4].ToString();

				_listAddUser.Add(AddUser);
			}

			dataAddUser.Close();
			return _listAddUser;
		}

		public ResponseMessageAddUser InsertAddUser(AddUserModel form)
		{
			string sp = "spWOMEYE_InsertUser";
			try
			{
				var resp = _dbConnection.QueryFirstOrDefault<ResponseMessageAddUser>(sp, new
				{
					@USER_ID = form.USER_ID,
					@USER_NAME = form.USER_NAME,
					@USER_NIK_EMP = form.USER_NIK_EMP,
					@USER_NIK_KTP = form.USER_NIK_KTP,
					@GENDER = form.GENDER,
					@USER_POSITION = form.USER_POSITION,
					@USER_PASS = form.USER_PASS,
					@DIVISION = form.DIVISION,
					@PHONE = form.PHONE,
					@BOD = form.BOD
					//@LAST_USER_LOGIN = form.LAST_USER_LOGIN,
					//@LAST_USER_UPDATE = form.LAST_USER_UPDATE,
				}, commandType: CommandType.StoredProcedure, commandTimeout: 30);
				return resp;
			}
			catch (Exception e)
			{
				ResponseMessageAddUser resp = new ResponseMessageAddUser();
				resp.responseCodeAddUser = "500";
				resp.responseMessageAddUser = e.Message;
				return resp;
			}
		}

		public ResponseMessageAddUser UpdateAddUser(AddUserModel form)
		{
			string sp = "spWOMEYE_UpdateUser";
			try
			{
				var resp = _dbConnection.QueryFirstOrDefault<ResponseMessageAddUser>(sp, new
				{
					@M_WOMEYE_USER_ID = form.M_WOMEYE_USER_ID,
					@USER_ID = form.USER_ID,
					@USER_NAME = form.USER_NAME,
					@USER_NIK_EMP = form.USER_NIK_EMP,
					@USER_NIK_KTP = form.USER_NIK_KTP,
					@GENDER = form.GENDER,
					@USER_POSITION = form.USER_POSITION,
					//@USER_PASS = form.USER_PASS,
					@DIVISION = form.DIVISION,
					@PHONE = form.PHONE,
					@BOD = form.BOD
					//@LAST_USER_LOGIN = form.LAST_USER_LOGIN,
					//@LAST_USER_UPDATE = form.LAST_USER_UPDATE,
				}, commandType: CommandType.StoredProcedure, commandTimeout: 30);
				return resp;
			}
			catch (Exception e)
			{
				ResponseMessageAddUser resp = new ResponseMessageAddUser();
				resp.responseCodeAddUser = "500";
				resp.responseMessageAddUser = e.Message;
				return resp;
			}
		}

		public AddUserModel getDataUserById(int id)
		{
			string sp = "spWOMEYE_GetAllUserById";

			var resp = _dbConnection.QueryFirstOrDefault<AddUserModel>(sp, new { @id = id }, commandType: CommandType.StoredProcedure, commandTimeout: 30);

			return resp;
		}

		public List<SelectListPosition> ddlPosition()
		{
			string sp = "spWOMEYE_GetPosition";
			var resp = _dbConnection.ExecuteReader(sp, commandType: CommandType.StoredProcedure, commandTimeout: 30);
			while (resp.Read())
			{
				SelectListPosition Position = new SelectListPosition();
				Position.key = resp[0].ToString();
				Position.value = resp[1].ToString();
				//Position.role = resp[2].ToString();
				_listPosition.Add(Position);
			}
			resp.Close();
			return _listPosition;
		}
	}
}
