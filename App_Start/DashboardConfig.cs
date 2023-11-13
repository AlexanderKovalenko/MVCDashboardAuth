using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DashboardWeb.Mvc;
using DevExpress.DataAccess.Sql;
using DevExpress.DataAccess.Web;
using System.Web.Routing;

namespace MVCDashboardAuth {
    public static class DashboardConfig {
        public static void RegisterService(RouteCollection routes) {
            routes.MapDashboardRoute("dashboardControl", "DefaultDashboard");

            DashboardConfigurator.Default.SetConnectionStringsProvider(new ConfigFileConnectionStringsProvider());
            DashboardConfigurator.Default.SetDashboardStorage(new DashboardFileStorage(@"~/App_Data/Dashboards"));
            DashboardConfigurator.Default.SetDataSourceStorage(CreateDataSourceStorage());

            DevExpress.Utils.DeserializationSettings.RegisterTrustedClass(typeof(ProductSales));
        }

        public static DataSourceInMemoryStorage CreateDataSourceStorage() {
            DataSourceInMemoryStorage dataSourceStorage = new DataSourceInMemoryStorage();
           
            var sqlDataSource = new DashboardSqlDataSource("SQL Data Source", "NWindConnectionStringSQL");
            SelectQuery query = SelectQueryFluentBuilder
                .AddTable("Products")
                .SelectAllColumnsFromTable()
                .Build("Products");
            sqlDataSource.Queries.Add(query);
            dataSourceStorage.RegisterDataSource("sqlDataSource", sqlDataSource.SaveToXml());

            var objDataSource = new DashboardObjectDataSource("Object Data Source") { DataId = "odsSales" };
            objDataSource.DataMember = "GetProductSales";
            dataSourceStorage.RegisterDataSource("objDataSource", objDataSource.SaveToXml());

            return dataSourceStorage;
        }
    }
}