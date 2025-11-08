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
    }
}
