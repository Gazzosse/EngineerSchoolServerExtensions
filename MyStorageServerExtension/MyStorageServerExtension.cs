using DocsVision.Platform.StorageServer;
using DocsVision.Platform.StorageServer.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStorageServerExtension
{
    public class MyStorageServerExtension : StorageServerExtension
    {
        public MyStorageServerExtension() { }

        [ExtensionMethod]
        public CursorInfo GetSecondedEmployeeBusinessTripsInfo(Guid employeeId)
        {
            using (var cmd = DbRequest.DataLayer.Connection.CreateCommand("getSecondedEmployeeBusinessTripsInfo", System.Data.CommandType.StoredProcedure))
            {
                cmd.AddParameter("EmployeeId", System.Data.DbType.Guid, System.Data.ParameterDirection.Input, 0, employeeId);
                return base.ExecuteCursorCommand(cmd);
            }
        }

        [ExtensionMethod]
        public long InsertBulkObserverCityInfo(Guid[] employeeIds, Guid[] cityIds, bool[] isObserverFlags)
        {
            using (var cmd = DbRequest.DataLayer.Connection.CreateCommand("InsertBulkObserverCities", System.Data.CommandType.StoredProcedure))
            {
                cmd.AddParameter("employee_ids", System.Data.DbType.Object, System.Data.ParameterDirection.Input, 0, employeeIds);
                cmd.AddParameter("city_ids", System.Data.DbType.Object, System.Data.ParameterDirection.Input, 0, cityIds);
                cmd.AddParameter("is_observer_flags", System.Data.DbType.Object, System.Data.ParameterDirection.Input, 0, isObserverFlags);
                return cmd.ExecuteScalar<long>();
            }
        }

        [ExtensionMethod]
        public long InsertBulkGroupsInfo(string[] groupNames)
        {
            using (var cmd = DbRequest.DataLayer.Connection.CreateCommand("InsertBulkGroups", System.Data.CommandType.StoredProcedure))
            {
                cmd.AddParameter("group_names", System.Data.DbType.Object, System.Data.ParameterDirection.Input, 0, groupNames);
                return cmd.ExecuteScalar<long>();
            }
        }

        [ExtensionMethod]
        public long InsertBulkEmployeesInfo(Guid[] parentRowIds, string[] firstNames, string[] middleNames, string[] lastNames)
        {
            using (var cmd = DbRequest.DataLayer.Connection.CreateCommand("InsertBulkEmployees", System.Data.CommandType.StoredProcedure))
            {   
                cmd.AddParameter("parent_row_ids", System.Data.DbType.Object, System.Data.ParameterDirection.Input, 0, parentRowIds);
                cmd.AddParameter("first_names", System.Data.DbType.Object, System.Data.ParameterDirection.Input, 0, firstNames);
                cmd.AddParameter("middle_names", System.Data.DbType.Object, System.Data.ParameterDirection.Input, 0, middleNames);
                cmd.AddParameter("last_names", System.Data.DbType.Object, System.Data.ParameterDirection.Input, 0, lastNames);
                return cmd.ExecuteScalar<long>();
            }
        }
    }
}
