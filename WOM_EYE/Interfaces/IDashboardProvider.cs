using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WOM_EYE.Models.Dashboard;

namespace WOM_EYE.Interfaces.Dashboard
{
    public interface IDashboardProvider
    {
        List<DashboardModel> getAllDashboard();
    }
}
