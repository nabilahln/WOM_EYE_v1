using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WOM_EYE.Interfaces.Progress;
using WOM_EYE.Models.Progress;

namespace WOM_EYE.Providers
{
	public class ProgressProvider : IProgressProvider
	{
		private IDbConnection _dbConnection;
		private ProgressModel _progressModel;
		private List<ProgressModel> _listProgress;

		public ProgressProvider(IDbConnection dbConnection, ProgressModel progressModel, List<ProgressModel> listProgress)
		{
			_dbConnection = dbConnection;
			_progressModel = progressModel;
			_listProgress = listProgress;
		}

		public ResponseMessage DeleteProgress(int id)
		{
			string sp = "spWOMEYE_DeleteProgress";

			var resp = _dbConnection.QueryFirstOrDefault<ResponseMessage>(sp, new { @id = id }, commandType: CommandType.StoredProcedure, commandTimeout: 30);

			return resp;
		}

		public List<ProgressModel> getAllProgress(int id)
		{
			string spName = "spWOMEYE_GetAllProgress";

			var dataProgress = _dbConnection.ExecuteReader(spName, new { @id = id },commandType: CommandType.StoredProcedure, commandTimeout: 30);

			while (dataProgress.Read())
			{
				ProgressModel Progress = new ProgressModel();
				Progress.T_WOMEYE_PROGRESS_ID = Convert.ToInt32(dataProgress[0]);
				Progress.PROJECT_ID = dataProgress[1].ToString();
				Progress.PENANGGUNG_JAWAB = dataProgress[2].ToString();
				Progress.DESKRIPSI = dataProgress[3].ToString();
				Progress.USR_CRT = dataProgress[4].ToString();
				Progress.DTM_CRT = dataProgress[5].ToString();
				Progress.USR_UPD = dataProgress[6].ToString();
				Progress.DTM_UPD = dataProgress[7].ToString();
				_listProgress.Add(Progress);
			}

			dataProgress.Close();
			return _listProgress;
		}

		public ProgressModel getDataProgressById(int id)
		{
			string sp = "spWOMEYE_GetProgressById";

			var resp = _dbConnection.QueryFirstOrDefault<ProgressModel>(sp, new { @id = id }, commandType: CommandType.StoredProcedure, commandTimeout: 30);

			return resp;
		}

		public ResponseMessage InsertDataProgress(ProgressModel form)
		{
			string spName = "spWOMEYE_InsertProgress";

			try
			{
				var resp = _dbConnection.QueryFirstOrDefault<ResponseMessage>(spName, new
				{
					@NO_PROJECT = form.PROJECT_ID,
					@USR_CRT = form.USR_CRT,
					@DESKRIPSI = form.DESKRIPSI
				}, commandType: CommandType.StoredProcedure, commandTimeout: 30);

				return resp;
			}
			catch (Exception e)
			{
				ResponseMessage resp = new ResponseMessage();
				resp.responseCodeProgress = "500";
				resp.responseMessageProgress = e.Message;
				return resp;
			}
		}

		public ResponseMessage UpdateProgress(ProgressModel form)
		{
			string sp = "spWOMEYE_UpdateProgress";
			try
			{
				var resp = _dbConnection.QueryFirstOrDefault<ResponseMessage>(sp, new
				{
					@T_WOMEYE_PROGRESS_ID = form.T_WOMEYE_PROGRESS_ID.ToString(),
					@DESKRIPSI = form.DESKRIPSI,
					@USR_UPD = form.USR_UPD
				}, commandType: CommandType.StoredProcedure, commandTimeout: 30);
				return resp;
			}
			catch (Exception e)
			{
				ResponseMessage resp = new ResponseMessage();
				resp.responseCodeProgress = "500";
				resp.responseMessageProgress = e.Message;
				return resp;
			}

			throw new NotImplementedException();
		}
	}
}
