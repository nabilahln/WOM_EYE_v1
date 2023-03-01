    using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WOM_EYE.Models.Progress
{
	public class ProgressModel : ResponseMessage
    {

        #region Atribut

        public int T_WOMEYE_PROGRESS_ID { get; set; }

        [DisplayName("Project")]
        public string PROJECT_ID { get; set; }

        [DisplayName("Penanggung Jawab")]
        public string PENANGGUNG_JAWAB { get; set; }
        
        [DisplayName("Deskripsi")]
        public string DESKRIPSI { get; set; }
        
        [DisplayName("User Create")]
        public string USR_CRT { get; set; }
        
        [DisplayName("Date Create")]
        public string DTM_CRT { get; set; }
        
        [DisplayName("User Update")]
        public string USR_UPD { get; set; }
        
        [DisplayName("Date Update")]
        public string DTM_UPD { get; set; }
        
        public List<ProgressModel> ListProgress { get; set; }

        #endregion

    }

	public class ResponseMessage
	{
		public string responseCodeProgress { get; set; }
		public string responseMessageProgress { get; set; }
		public string errorMessageDetailProgress { get; set; }

	}
}
