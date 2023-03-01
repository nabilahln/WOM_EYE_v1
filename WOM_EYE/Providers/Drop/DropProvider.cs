using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WOM_EYE.Interfaces.Drop;
using WOM_EYE.Models.Drop;

namespace WOM_EYE.Providers.Drop
{
	public class DropProvider : IDropProvider
	{
		private readonly IDbConnection _dbConnection;
		private DropModel _dropModel;
		private List<DropModel> _listDrop;

		public DropProvider(IDbConnection dbConnection, DropModel dropModel, List<DropModel> listDrop)
		{
			_dbConnection = dbConnection;
			_dropModel = dropModel;
			_listDrop = listDrop;
		}

		public ResponseMessageDrop DropProject(DropModel form)
		{
			string spName = "spWOMEYE_DropProject";
			try
			{
				var resp = _dbConnection.QueryFirstOrDefault<ResponseMessageDrop>(spName, new
				{
					@PROJECT_ID = form.PROJECT_ID
					,
					@ALASAN = form.ALASAN
				}, commandType: CommandType.StoredProcedure, commandTimeout: 30);
				return resp;
			}
			catch (Exception e)
			{
				ResponseMessageDrop resp = new ResponseMessageDrop();
				resp.responseCodeDrop = "500";
				resp.responseMessageDrop = e.Message;
				return resp;
			}

		}

		public List<DropModel> getAllDrop()
		{
			string spName = "spWOMEYE_AllDrop";
			var data = _dbConnection.ExecuteReader(spName, commandType: CommandType.StoredProcedure, commandTimeout: 30);

			while (data.Read())
			{
				DropModel _drop = new DropModel();
				_drop.T_WOMEYE_DROP_ID = System.Convert.ToInt32(data[0]);
				_drop.PROJECT_ID = data[1].ToString();
				_drop.ALASAN = data[2].ToString();
				_drop.DTM_CRT = data[3].ToString();
				_listDrop.Add(_drop);
			}

			data.Close();
			return _listDrop;

		}

		public ResponseMessageDrop UnDropProject(String dropId)
		{
			string spName = "spWOMEYE_UnDropProject";
			try
			{
				var resp = _dbConnection.QueryFirstOrDefault<ResponseMessageDrop>(spName, new { @dropId = dropId }, commandType: CommandType.StoredProcedure, commandTimeout: 30);
				return resp;
			}
			catch (Exception e)
			{
				ResponseMessageDrop resp = new ResponseMessageDrop();
				resp.responseCodeDrop = "500";
				resp.responseMessageDrop = e.Message;
				return resp;
			}
			throw new NotImplementedException();
		}
	}
}
