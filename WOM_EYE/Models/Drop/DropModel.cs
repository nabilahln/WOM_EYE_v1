using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WOM_EYE.Models.Drop
{
	public class DropModel : ResponseMessageDrop
	{
		#region Atribut

		public int T_WOMEYE_DROP_ID { get; set; }

		[DisplayName("Project")]
		public string PROJECT_ID { get; set; }

		[DisplayName("Alasan")]
		public string ALASAN { get; set; }

		[DisplayName("Tanggal Drop")]
		public string DTM_CRT { get; set; }

		#endregion

		public List<DropModel> ListDrop;

	}

	public class ResponseMessageDrop
	{
		public string responseCodeDrop { get; set; }
		public string responseMessageDrop { get; set; }
		public string errorMessageDrop { get; set; }

	}
}
