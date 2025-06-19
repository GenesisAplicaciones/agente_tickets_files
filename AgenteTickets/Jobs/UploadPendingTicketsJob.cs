using AgenteTickets.Tasks;
using log4net;
using System;
using System.Reflection;

namespace AgenteTickets.Jobs
{
    public class UploadPendingTicketsJob : Job
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public override void Task(object state)
        {
            try
            {
                TicketTask.UploadPendingTickets();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
    }
}
