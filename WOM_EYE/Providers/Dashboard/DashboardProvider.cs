using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WOM_EYE.Interfaces.Dashboard;
using WOM_EYE.Models.Dashboard;

namespace WOM_EYE.Providers.Dashboard
{
    public class DashboardProvider : IDashboardProvider
    {
        private IDbConnection _dbConnection;
        private DashboardModel _dashboardModel;
        private List<DashboardModel> _listDashboard;

        public DashboardProvider(IDbConnection dbConnection, DashboardModel dashboardModel, List<DashboardModel> listDashboard)
        {
            _dbConnection = dbConnection;
            _dashboardModel = dashboardModel;
            _listDashboard = listDashboard;
        }
        public List<DashboardModel> getAllDashboard()
        {
            string spName = "spWOMEYE_Dashboard";

            var dataDashboard = _dbConnection.ExecuteReader(spName, commandType: CommandType.StoredProcedure, commandTimeout: 30);

            while (dataDashboard.Read())
            {
                DashboardModel Dashboard = new DashboardModel();
                Dashboard.GROUP_NAME = dataDashboard[0].ToString();
                Dashboard.JUMLAH = dataDashboard[1].ToString();

                _listDashboard.Add(Dashboard);
            }

            dataDashboard.Close();
            return _listDashboard;
        }


    }
}
