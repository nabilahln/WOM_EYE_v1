using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WOM_EYE.Interfaces.Projects;
using WOM_EYE.Models.Projects;

namespace WOM_EYE.Providers.Projects
{
	public class ProjectProvider : IProjectProvider
	{
		private readonly IDbConnection _dbConnection;
		private ProjectModel _projectModel;
		private List<ProjectModel> _listProject;
		private List<SelectListUser> _listUser;
		List<SelectListStatus> _listStatus;
		private List<SelectListJenis> _listJenis;

		public ProjectProvider(
			IDbConnection dbConnection,
			ProjectModel projectModel,
			List<ProjectModel> listProject,
			List<SelectListUser> listUser, List<SelectListStatus> listStatus, List<SelectListJenis> listJenis
			)
		{
			this._dbConnection = dbConnection;
			this._projectModel = projectModel;
			this._listProject = listProject;
			this._listUser = listUser;
			this._listStatus = listStatus;
			this._listJenis = listJenis;
		} 

		public List<SelectListStatus> ddlStatus()
		{
			string sp = "spWOMEYE_GetStatusProject";
			var resp = _dbConnection.ExecuteReader(sp, commandType: CommandType.StoredProcedure, commandTimeout: 30);
			while (resp.Read())
			{
				SelectListStatus model = new SelectListStatus();
				model.key = resp[0].ToString();
				model.value = resp[1].ToString();
				model.detailFor = resp[2].ToString();
				_listStatus.Add(model);
			}
			resp.Close();
			return _listStatus;
		}

		public List<SelectListJenis> ddlJenis()
		{
			string sp = "spWOMEYE_GetJenisProject";
			var resp = _dbConnection.ExecuteReader(sp, commandType: CommandType.StoredProcedure, commandTimeout: 30);
			while (resp.Read())
			{
				SelectListJenis model = new SelectListJenis();
				model.key = resp[0].ToString();
				model.value = resp[1].ToString();
				_listJenis.Add(model);
			}
			resp.Close();
			return _listJenis;
		}

		public List<SelectListUser> ddlUser()
		{
			string sp = "spWOMEYE_GetUser";
			var resp = _dbConnection.ExecuteReader(sp, commandType: CommandType.StoredProcedure, commandTimeout: 30);
			while (resp.Read())
			{
				SelectListUser model = new SelectListUser();
				model.key = resp[0].ToString();
				model.value = resp[1].ToString();
				model.role = resp[2].ToString();
				_listUser.Add(model);
			}
			resp.Close();
			return _listUser;
		}

		public List<ProjectModel> getAllProject()
		{
			string spName = "spWOMEYE_AllProject_New";

			var dataProject = _dbConnection.ExecuteReader(spName, commandType: CommandType.StoredProcedure, commandTimeout: 30);

			while (dataProject.Read())
			{
				ProjectModel project = new ProjectModel();
				project.T_WOMEYE_PROJECT_ID = System.Convert.ToInt32(dataProject[0]);
				project.SOL_LEADER = dataProject[1].ToString();
				project.PROJECT_LEADER = dataProject[2].ToString();
				project.JENIS_PROJECT = dataProject[3].ToString();
				project.NO_PROJECT = dataProject[4].ToString();
				project.DESKRIPSI = dataProject[5].ToString();
				project.FUNCTION_ANALYST = dataProject[6].ToString();
				project.PROGRAMMER = dataProject[7].ToString();
				project.STATUS = dataProject[8].ToString();
				project.CATATAN = dataProject[9].ToString();
				project.DTM_CRT = dataProject[10].ToString();
				project.GROUP_NAME = dataProject[11].ToString();
				_listProject.Add(project);
			}

			dataProject.Close();
			return _listProject;
		}

		public ProjectModel getDataProjectById(int id)
		{
			string sp = "spWOMEYE_GetProject_New";

			var resp = _dbConnection.QueryFirstOrDefault<ProjectModel>(sp, new { @id = id }, commandType: CommandType.StoredProcedure, commandTimeout: 30);

			return resp;
		}

		public ProjectModel getDataProjectByIdEdit(int id)
		{
			string sp = "spWOMEYE_GetProjectById_New";

			var resp = _dbConnection.QueryFirstOrDefault<ProjectModel>(sp, new { @id = id }, commandType: CommandType.StoredProcedure, commandTimeout: 30);

			return resp;
		}

		public ResponseMessage UpdateProject(ProjectModel form)
		{
			string sp = "spWOMEYE_UpdateProject_New";
			try
			{
				var resp = _dbConnection.QueryFirstOrDefault<ResponseMessage>(sp, new
				{
					@T_WOMEYE_PROJECT_ID = form.T_WOMEYE_PROJECT_ID,
					@SOL_LEADER = form.SOL_LEADER,
					@PROJECT_LEADER = form.PROJECT_LEADER,
					@JENIS_PROJECT_ID = form.JENIS_PROJECT,
					@NO_PROJECT = form.NO_PROJECT,
					@DESKRIPSI = form.DESKRIPSI,
					@FUNCTION_ANALYST = form.FUNCTION_ANALYST,
					@PROGRAMMER = form.PROGRAMMER,
					@STATUS = form.STATUS,
					@CATATAN = form.CATATAN,
					@USR_UPD = form.USR_UPD
				}, commandType: CommandType.StoredProcedure, commandTimeout: 30);
				return resp;
			}
			catch (Exception e)
			{
				ResponseMessage resp = new ResponseMessage();
				resp.responseCodeProject = "500";
				resp.responseMessageProject = e.Message;
				return resp;
			}
		}

		public ResponseMessage InsertProject(ProjectModel form)
		{
			string sp = "spWOMEYE_InsertProject_New";
			try
			{
				var resp = _dbConnection.QueryFirstOrDefault<ResponseMessage>(sp, new
				{
					@SOL_LEADER = form.SOL_LEADER,
					@PROJECT_LEADER = form.PROJECT_LEADER,
					@JENIS_PROJECT = form.JENIS_PROJECT,
					@NO_PROJECT = form.NO_PROJECT,
					@DESKRIPSI = form.DESKRIPSI,
					@FUNCTION_ANALYST = form.FUNCTION_ANALYST,
					@PROGRAMMER = form.PROGRAMMER,
					@USR_CRT = form.USR_CRT
				}, commandType: CommandType.StoredProcedure, commandTimeout: 30);
				return resp;
			}
			catch (Exception e)
			{
				ResponseMessage resp = new ResponseMessage();
				resp.responseCodeProject = "500";
				resp.responseMessageProject = e.Message;
				return resp;
			}
		}
	}
}
