using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using WOM_EYE.Models.Projects;

namespace WOM_EYE.Models.Notes
{
	public class NoteModel : NoteAddSaveViewModel
	{
		#region Atribut
		
		public int M_WOMEYE_CATATAN_ID { get; set; }

		[DisplayName("Detail Project ID")]
		public string DETAIL_PROJECT_ID { get; set; }

		[DisplayName("Status")]
		public string STATUS_ID { get; set; }

		[DisplayName("Catatan")]
		public string NOTES { get; set; }

		[DisplayName("Nama User")]
		public string USR_CRT { get; set; }

		[DisplayName("Tanggal Dibuat")]
		public string DTM_CRT { get; set; }


		[DisplayName("Pengubah")]
		public string USR_UPD { get; set; }

		[DisplayName("Tanggal Diubah")]
		public string DTM_UPD { get; set; }

		public string PROJECT_ID { get; set; }

		#endregion

		public List<NoteModel> ListCatatan { get; set; }
		public List<ProjectModel> ListProject { get; set; }
	}

	public class ResponseMessageCatatan
	{
		public string responseCode { get; set; }
		public string responseMessage { get; set; }
		public string errorMessage { get; set; }

	}
	public class SelectListStatusCatatan
	{
		public string key { get; set; }
		public string value { get; set; }
	}

	public class NoteAddSaveViewModel : ResponseMessageCatatan
	{
		public List<SelectListStatusCatatan> ddlStatusCatatan { get; set; }
	}
}
