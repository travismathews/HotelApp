using HotelAppLibrary.Databases;
using HotelAppLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelAppLibrary.Data
{
    public class SqlData
    {
        // Store the _db for the life of the SqlData instance
        private readonly ISqlDataAccess _db;
        // Set connection string name to the specific connection type that will be in the config file
        private const string connectionStringName = "SqlDB";

        public SqlData(ISqlDataAccess db)
        {
            // Pass in db from interface and store for life of class instance
            _db = db;
        }

        public List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate, DateTime endDate)
        {
            // Pass in Start Date and End Date for stored procedure
            // LoadData from stored procedure and return a list of available rooms
            return _db.LoadData<RoomTypeModel, dynamic>("dbo.spRoomTypes_GetAvailableTypes", new { startDate, endDate }, connectionStringName, true);
        }



    }
}
