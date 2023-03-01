using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WOM_EYE.Models.Projects;

namespace WOM_EYE.Models.Dashboard
{
    public class DashboardModel : ResponseModel
    {
        //public int M_WOMEYE_STATUS_PROJECT_ID { get; set; }
        public string GROUP_NAME { get; set; }
        public string JUMLAH { get; set; }

        public List<DashboardModel> listDashboard { get; set; }
        public List<ProjectModel> listProject { get; set; }

    }
}
