using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WOM_EYE.Models.User
{
	public class UserModel
	{
		#region
		public int M_WOMEYE_USER_ID { get; set; }

		public string USER_ID { get; set; }

		public string USER_NIK_EMP { get; set; }

		public string? USER_NIK_KTP { get; set; }

		public string USER_NAME { get; set; }

		public string USER_POSITION { get; set; }

		public string? LAST_USER_LOGIN { get; set; }

		public string? LAST_USER_UPDATE { get; set; }


		public List<UserModel> ListUser { get; set; }

		#endregion
	}
}
