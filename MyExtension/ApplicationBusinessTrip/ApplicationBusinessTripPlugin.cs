using DocsVision.BackOffice.ObjectModel;
using DocsVision.Layout.WebClient.Models;
using DocsVision.Layout.WebClient.Models.TableData;
using DocsVision.Layout.WebClient.Services;
using DocsVision.Platform.ObjectManager;
using DocsVision.Platform.StorageServer;
using DocsVision.Platform.WebClient;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.Office.CustomUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExtension.ApplicationBusinessTrip
{
    public class ApplicationBusinessTripPlugin : IDataGridControlPlugin
    {
        public string Name => "ApplicationBusinessTripPlugin";

        public TableModel GetTableData(SessionContext sessionContext, List<ParamModel> parameters)
        {
            TableModel table = new();
            string numberColumn = "number";
            string startColumn = "start";
            string cityColumn = "city";
            string reasonColumn = "reason";
            string stateColumn = "state";

            table.Columns.Add(new ColumnModel()
            {
                Id = numberColumn,
                Name = "№",
                Type = DocsVision.WebClient.Models.Grid.ColumnType.Integer
            });
            table.Columns.Add(new ColumnModel()
            {
                Id = startColumn,
                Name = "Дата выезда",
                Type = DocsVision.WebClient.Models.Grid.ColumnType.String
            });
            table.Columns.Add(new ColumnModel()
            {
                Id = cityColumn,
                Name = "Город",
                Type = DocsVision.WebClient.Models.Grid.ColumnType.String
            });
            table.Columns.Add(new ColumnModel()
            {
                Id = reasonColumn,
                Name = "Основание для поездки",
                Type = DocsVision.WebClient.Models.Grid.ColumnType.String
            });
            table.Columns.Add(new ColumnModel()
            {
                Id = stateColumn,
                Name = "Статус заявки",
                Type = DocsVision.WebClient.Models.Grid.ColumnType.String
            });

            Guid cardId = new Guid(parameters.First(x => x.Key == "CurrentCardId").Value);
            Document doc = sessionContext.ObjectContext.GetObject<Document>(cardId);
            var secondedEmployeeId = (Guid)doc.MainInfo["SecondedEmployee"];

            ExtensionMethod method = sessionContext.Session.ExtensionManager.GetExtensionMethod("MyStorageServerExtension", "GetSecondedEmployeeBusinessTripsInfo");
            method.Parameters.AddNew("EmployeeId", ParameterValueType.Guid, secondedEmployeeId);

            var tableInfo = method.ExecuteReader();

            foreach (InfoRow item in tableInfo)
            {
                table.Rows.Add(new RowModel()
                {
                    Id = Guid.NewGuid().ToString(),
                    Cells = new List<CellModel>()
                {
                    new()
                    {
                        ColumnId = numberColumn,
                        Value = item.GetInt64("rownumber")
                    },
                    new()
                    {
                        ColumnId = startColumn,
                        Value = item.GetDateTime("businesstripstart").Value.ToShortDateString()
                    },
                    new()
                    {
                        ColumnId = cityColumn,
                        Value = item.GetString("cityname")
                    },
                    new()
                    {
                        ColumnId = reasonColumn,
                        Value = item.GetString("businesstripreason")
                    },
                    new()
                    {
                        ColumnId = stateColumn,
                        Value = item.GetString("statelocalname")
                    }

                }
                });
            }

            table.Id = Guid.NewGuid().ToString();

            return table;
        }
    }
}
