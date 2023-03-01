using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WOM_EYE.Models.AddUser
{
    public class AddUserModel : UserAddSaveViewModel
    {
        public int M_WOMEYE_USER_ID { get; set; }
        [DisplayName("User ID")]
        public string USER_ID { get; set; }
        [DisplayName("Employee Name")]
        public string USER_NAME { get; set; }
        [DisplayName("NIK Employee")]
        public string USER_NIK_EMP { get; set; }
        [DisplayName("NIK KTP")]
        public string? USER_NIK_KTP { get; set; }
        [DisplayName("User Position")]
        public string USER_POSITION { get; set; }
        [DisplayName("User Password")]
        public string USER_PASS { get; set; }
        public string DIVISION { get; set; }
        public string PHONE { get; set; }
        public string GENDER { get; set; }
        public string BOD { get; set; }
        [DisplayName("Last User Login")]
        public string LAST_USER_LOGIN { get; set; }
        [DisplayName("Last User Update")]
        public string LAST_USER_UPDATE { get; set; }
        public List<AddUserModel> listAddUser { get; set; }
    }

    public class ResponseMessageAddUser
    {
        public string responseCodeAddUser { get; set; }
        public string responseMessageAddUser { get; set; }
        public string errorMessageAddUser { get; set; }
    }

    public class SelectListPosition
    {
        public string key { get; set; }
        public string value { get; set; }
        public string role { get; set; }
    }

    public class UserAddSaveViewModel : ResponseMessageAddUser
    {
        public List<SelectListPosition> ddlPosition { get; set; }

        
    }
}
