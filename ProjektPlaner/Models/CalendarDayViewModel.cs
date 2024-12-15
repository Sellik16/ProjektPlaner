namespace ProjektPlaner.Models
{
    public class CalendarDayViewModel
    {
        public DateTime Date { get; set; } // Data tego dnia w kalendarzu
        public string? EventName { get; set; } // Nazwa wydarzenia, które odbywa się tego dnia
    }
}
