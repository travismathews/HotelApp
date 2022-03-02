using HotelAppLibrary.Databases;
using HotelAppLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelAppLibrary.Data
{
    public class SqlData : IDatabaseData
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


        public void BookGuest(string firstName, string lastName, DateTime startDate, DateTime endDate, int roomTypeId)
        {

            // Query the db for if the guest exists, if not create it and return the guest
            GuestModel guest = _db.LoadData<GuestModel, dynamic>("dbo.spGuests_Insert", new { firstName, lastName }, connectionStringName, true).First();

            // TODO: Create a stored procedure for this
            // Get the room type so we can calculate the total cost later from the RoomTypeModel.Price
            string roomTypeSql = @"select * from dbo.RoomTypes where Id = @Id";
            RoomTypeModel roomType = _db.LoadData<RoomTypeModel, dynamic>(roomTypeSql, new { Id = roomTypeId }, connectionStringName, false).First();

            // Calcuate number of days for booking
            // timestaying.Days returns whole number of days so we can calculate totalCost during insert
            TimeSpan timeStaying = endDate.Date.Subtract(startDate.Date);

            // Runs a stored procedure that returns a list of available room numbers of the specified type
            List<RoomModel> availableRooms = _db.LoadData<RoomModel, dynamic>("dbo.spRooms_GetAvailableRooms", new { startDate, endDate, roomTypeId }, connectionStringName, true);

            _db.SaveData("spBookings_Insert",
                         new
                         {
                             //Book the id of the first available room
                             roomId = availableRooms.First().Id,
                             guestId = guest.Id,
                             startDate = startDate,
                             endDate = endDate,
                             // Example: totalCost = 6 days * $115 
                             totalCost = timeStaying.Days * roomType.Price,
                         },
                         connectionStringName,
                         true);
        }

        public List<BookingFullModel> SearchBookings(string lastName)
        {
            // Return a list of bookings using the spbooking stored procedure
            return _db.LoadData<BookingFullModel, dynamic>("dbo.spBookings_Search",
                new
                {
                    lastName,
                    startDate = DateTime.Now.Date
                },
                connectionStringName,
                true);
        }


        public void CheckInGuest(int bookingId)
        {
            // run stored procedure to set the booking with the provided ID to "checked in"
            _db.SaveData("dbo.spBookings_CheckIn", new { Id = bookingId }, connectionStringName, true);
        }

    }
}
