using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WOM_EYE.Models.AddUser;

namespace WOM_EYE.Interfaces.AddUser
{
    public interface IAddUserProvider
    {
        public List<AddUserModel> getAllAddUser();
        public ResponseMessageAddUser InsertAddUser(AddUserModel form);
        ResponseMessageAddUser UpdateAddUser(AddUserModel form);
        AddUserModel getDataUserById(int id);
        List<SelectListPosition> ddlPosition();
    }
}
