using System;
using System.Collections.Generic;

namespace ExpenseTracker.DAL.Models;

public partial class CalendarEvent
{
    public long CalendarEventID { get; set; }

    public byte EventTypeID { get; set; }

    public DateTime EventStartDateTime { get; set; }

    public DateTime EventEndDateTime { get; set; }

    public string EventTitle { get; set; }

    public string EventDescription { get; set; }

    public string AssignedStaffs { get; set; }

    public int? OrgUnitID { get; set; }

    public int? RecordStatus { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
