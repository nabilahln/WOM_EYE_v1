using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WOM_EYE.Models.Projects
{
	public class ProjectModel : ProjectAddSaveViewModel
	{
		#region Atribut

		public int T_WOMEYE_PROJECT_ID { get; set; }

		[DisplayName("Sol Leader")]
		public string SOL_LEADER { get; set; }

		[DisplayName("Project Leader")]
		public string PROJECT_LEADER { get; set; }

		[DisplayName("Jenis Project")]
		public string JENIS_PROJECT { get; set; }

		[DisplayName("Nomor Project")]
		public string NO_PROJECT { get; set; }

		[DisplayName("Deskripsi Project")]
		public string DESKRIPSI { get; set; }

		[DisplayName("Function Analyst")]
		public string FUNCTION_ANALYST { get; set; }

		[DisplayName("Programmer")]
		public string PROGRAMMER { get; set; }

		[DisplayName("Status")]
		public string STATUS { get; set; }

		[DisplayName("Catatan")]
		public string CATATAN { get; set; }

		[DisplayName("Tanggal Dibuat")]
		public string DTM_CRT { get; set; }

		public string GROUP_NAME { get; set; }

		public string USR_CRT { get; set; }

		public string USR_UPD { get; set; }

		#endregion
		public List<ProjectModel> ListProject { get; set; }
	}

	public class ResponseMessage : FlagButton
	{
		public string responseCodeProject { get; set; }
		public string responseMessageProject { get; set; }
		public string errorMessageProject { get; set; }

	}

	public class SelectListUser
	{
		public string key { get; set; }
		public string value { get; set; }
		public string role { get; set; }
	}

	public class SelectListStatus
	{
		public string key { get; set; }
		public string value { get; set; }
		public string detailFor { get; set; }
	}

	public class SelectListJenis
	{
		public string key { get; set; }
		public string value { get; set; }
	}

	public class ProjectAddSaveViewModel : ResponseMessage
	{
		public List<SelectListUser> ddlUser { get; set; }
		public List<SelectListStatus> ddlStatus { get; set; }

		public List<SelectListJenis> ddlJenis { get; set; }
	}

	public class FlagButton
	{
		public string btnSaveProject { get; set; }
		public string btnUpdateProject { get; set; }
	}
}

