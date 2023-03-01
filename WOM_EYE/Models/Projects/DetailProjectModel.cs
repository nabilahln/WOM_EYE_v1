using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WOM_EYE.Models.Projects
{
	public class DetailProjectModel : ProjectModel
	{
		#region Atribut

		public int T_WOMEYE_DETAIL_PROJECT_ID { get; set; }

		[DisplayName("Project")]
		public string PROJECT_ID { get; set; }

		[DisplayName("Tahap")]
		public string TAHAP { get; set; }

		[DisplayName("Tanggal Mulai")]
		public string START_DT { get; set; }

		[DisplayName("Tanggal Selesai")]
		public string END_DT { get; set; }

		[DisplayName("Dokumen")]
		public string DOKUMEN { get; set; }

		[DisplayName("Status")]
		public string STATUS_PROJECT { get; set; }

		[DisplayName("Keterangan")]
		public string KETERANGAN { get; set; }

		[DisplayName("Tanggal Dibuat")]
		public string DTM_CRT { get; set; }

		[DisplayName("Tanggal Di Ubah")]
		public string DTM_UPD { get; set; }

		[DisplayName("Realize Start Date")]
		public string REALIZATION_START_DT { get; set; }

		[DisplayName("Realize End Date")]
		public string REALIZATION_END_DT { get; set; }
		#endregion
		public List<DetailProjectModel> ListDetailProject { get; set; }

	}

}
