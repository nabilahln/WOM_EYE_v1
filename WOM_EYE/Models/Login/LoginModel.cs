using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WOM_EYE.Models.Login
{
	public class LoginModel : ResponseMessage
	{
		public int M_WOMEYE_USER_ID { get; set; }

		[DisplayName("User ID")]
		public string USER_ID { get; set; }

		[DisplayName("Password")]
		public string USER_PASS { get; set; }
	}

	public class ResponseMessage
	{
		public string responseCode { get; set; }
		public string responseMessage { get; set; }
		public string errorMessage { get; set; }

	}
}
