using System;
using System.Threading;

namespace AgenteTickets.Jobs
{
    public abstract class Job<TState>
    {
        protected TState State { get; set; }

        private Timer timer;

        public bool Running { get; private set; }
        public bool RunningTask { get; private set; }
        protected TimeSpan? TimeOfDay { get; set; }

        private TimeSpan? period;
        protected TimeSpan Period
        {
            get
            {
                if (period == null)
                {
                    period = new TimeSpan(24, 0, 0);
                }

                return period.Value;
            }
            set => period = value;
        }

        private TimeSpan? dueTime;
        protected TimeSpan DueTime
        {
            get
            {
                if (dueTime == null)
                {
                    DateTime now = DateTime.Now;
                    dueTime = now.Date + Period - now;
                }

                return dueTime.Value;
            }
            set => dueTime = value;
        }

        public void Change(TimeSpan timeOfDay, bool force = false)
        {
            TimeOfDay = timeOfDay;

            if (Running)
            {
                if (force)
                {
                    Schedule();
                }
                else
                {
                    CalculateDueTime(timeOfDay);
                    _ = timer.Change(DueTime, Period);
                }
            }
        }

        public void Change(TimeSpan dueTime, TimeSpan period, bool force = false)
        {
            DueTime = dueTime;
            Period = period;

            if (Running)
            {
                if (force)
                {
                    Schedule();
                }
                else
                {
                    _ = timer.Change(DueTime, Period);
                }
            }
        }

        public abstract void Task(TState state);

        private void Callback(object state)
        {
            if (!RunningTask)
            {
                RunningTask = true;
                Task((TState)state);
                RunningTask = false;
            }
        }

        public void Schedule()
        {
            CalculateDueTime(TimeOfDay);
            timer = new Timer(Callback, State, DueTime, Period);
            Running = true;
        }

        private void CalculateDueTime(TimeSpan? timeOfDay)
        {
            if (timeOfDay == null)
            {
                return;
            }

            DateTime now = DateTime.Now;
            DateTime nextTime = now.Date + timeOfDay.Value;

            if (now > nextTime)
            {
                nextTime = nextTime.AddDays(1);
            }

            DueTime = nextTime - now;
        }

        public void Stop()
        {
            Running = false;
            timer?.Dispose();
        }
    }

    public abstract class Job : Job<object>
    {

    }
}
