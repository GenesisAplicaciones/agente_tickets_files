namespace AgenteTickets.Models
{
    public class TicketTaskResult
    {
        public TicketTaskResult()
        {
            Elements = 0;
            LoadsCompleted = 0;
            LoadsNotCompleted = 0;
            Cancellations = 0;
            CancellationsCompleted = 0;
            CancellationsNotCompleted = 0;
        }

        public int Elements { get; set; }
        public int LoadsCompleted { get; set; }
        public int LoadsNotCompleted { get; set; }
        public int Cancellations { get; set; }
        public int CancellationsCompleted { get; set; }
        public int CancellationsNotCompleted { get; set; }
    }
}
