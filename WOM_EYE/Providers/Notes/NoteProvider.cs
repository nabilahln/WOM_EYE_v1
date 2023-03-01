using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WOM_EYE.Interfaces.Notes;
using WOM_EYE.Models.Notes;

namespace WOM_EYE.Providers.Notes
{
	public class NoteProvider : INoteProvider
	{
		private readonly IDbConnection _dbConnection;
		private NoteModel _noteModel;
		private List<NoteModel> _listNote;
		private List<SelectListStatusCatatan> _listStatusCatatan;

		public NoteProvider(IDbConnection dbConnection, NoteModel noteModel, List<NoteModel> listNote, List<SelectListStatusCatatan> listStatusCatatan)
		{
			this._dbConnection = dbConnection;
			this._noteModel = noteModel;
			this._listNote = listNote;
			this._listStatusCatatan = listStatusCatatan;
		}

		public List<SelectListStatusCatatan> ddlStatusCatatan()
		{
			string sp = "spWOMEYE_GetListStatusCatatan";
			var resp = _dbConnection.ExecuteReader(sp, commandType: CommandType.StoredProcedure, commandTimeout: 30);
			while (resp.Read())
			{
				SelectListStatusCatatan model = new SelectListStatusCatatan();
				model.key = resp[0].ToString();
				model.value = resp[1].ToString();
				_listStatusCatatan.Add(model);
			}
			resp.Close();
			return _listStatusCatatan;
		}

		public List<NoteModel> getAllCatatan(int detailProjectId)
		{
			string spName = "spWOMEYE_GetAllCatatan_New";

			var dataCatatan = _dbConnection.ExecuteReader(spName, new { @id = detailProjectId },  commandType: CommandType.StoredProcedure, commandTimeout: 30);

			while (dataCatatan.Read())
			{
				NoteModel catatan = new NoteModel();
				catatan.M_WOMEYE_CATATAN_ID = System.Convert.ToInt32(dataCatatan[0]);
				catatan.DETAIL_PROJECT_ID = dataCatatan[1].ToString();
				catatan.STATUS_ID = dataCatatan[2].ToString();
				catatan.NOTES = dataCatatan[3].ToString();
				catatan.USR_CRT = dataCatatan[4].ToString();
				catatan.DTM_CRT = dataCatatan[5].ToString();
				catatan.USR_UPD = dataCatatan[6].ToString();
				catatan.DTM_UPD = dataCatatan[7].ToString();
				catatan.PROJECT_ID = dataCatatan[8].ToString();

				_listNote.Add(catatan);
			}

			dataCatatan.Close();
			return _listNote;
		}

		public NoteModel getDataCatatanById(int id)
		{
			string sp = "spWOMEYE_GetCatatanById";

			var resp = _dbConnection.QueryFirstOrDefault<NoteModel>(sp, new { @id = id }, commandType: CommandType.StoredProcedure, commandTimeout: 30);

			return resp;
		}

		public ResponseMessageCatatan InsertCatatan(NoteModel form)
		{
			string sp = "spWOMEYE_InsertCatatan";
			try
			{
				var resp = _dbConnection.QueryFirstOrDefault<ResponseMessageCatatan>(sp, new
				{
					@DETAIL_PROJECT_ID = form.DETAIL_PROJECT_ID,
					@STATUS_ID = form.STATUS_ID,
					@NOTES = form.NOTES,
					@USR_CRT = form.USR_CRT
				}, commandType: CommandType.StoredProcedure, commandTimeout: 30);
				return resp;
			}
			catch (Exception e)
			{
				ResponseMessageCatatan resp = new ResponseMessageCatatan();
				resp.responseCode = "500";
				resp.responseMessage = e.Message;
				return resp;
			}
			
		}

		public ResponseMessageCatatan UpdateCatatan(NoteModel form)
		{
			string sp = "spWOMEYE_UpdateCatatan";
			try
			{
				var resp = _dbConnection.QueryFirstOrDefault<ResponseMessageCatatan>(sp, new
				{
					@M_WOMEYE_CATATAN_ID = form.M_WOMEYE_CATATAN_ID,
					@STATUS_ID = form.STATUS_ID,
					@NOTES = form.NOTES,
					@USR_UPD = form.USR_UPD
				}, commandType: CommandType.StoredProcedure, commandTimeout: 30);
				return resp;
			}
			catch (Exception e)
			{
				ResponseMessageCatatan resp = new ResponseMessageCatatan();
				resp.responseCode = "500";
				resp.responseMessage = e.Message;
				return resp;
			}
		}
	}
}
