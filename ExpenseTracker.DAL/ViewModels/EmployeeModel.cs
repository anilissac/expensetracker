using ExpenseTracker.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.DAL.Models
{
    public class EmployeeView : Employee
    {
        public string ContactName
        {
            get
            {
                string _name = "";
                _name = FirstName;
                _name += " " + LastName;
                return _name;
            }
        }
        public string? enEmployeeID { get; set; }
        public string? CreatedByName { get; set; }
        public string? ModifiedByName { get; set; }
    }

    public class CalendarEventView : CalendarEvent
    {
        public string? enCalendarEventID { get; set; }

        public string[] SelectedAssignedStaffs { get; set; }

        public string? NotificationColor { get; set; }

        public long ID { get; set; }

        public string SomeImportantKeyID { get; set; }
        public string StartDateString { get; set; }
        public string EndDateString { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string EventTypeName { get; set; }
        public string StatusColor { get; set; }


        public string? CreatedByName { get; set; }
        public string? ModifiedByName { get; set; }
    }
}
