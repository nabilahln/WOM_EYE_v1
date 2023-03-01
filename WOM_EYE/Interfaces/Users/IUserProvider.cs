using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WOM_EYE.Models.User;

namespace WOM_EYE.Interfaces.Users
{
	public interface IUserProvider
	{
		Boolean checkUserSession(String myUserId, String myMUserId);

		UserModel getDataUser(string userId);
	}
}
