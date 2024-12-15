namespace ProjektPlaner.Models
{
    public class CalendarDayViewModel
    {
        public DateTime Date { get; set; }
        public List<string> EventNames { get; set; } = new List<string>();
    }


}
