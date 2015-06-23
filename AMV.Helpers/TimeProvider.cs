using System;

namespace AMV.Helpers
{
    /// <summary>
    /// Ambient context for providing current DateTime. 
    /// Currently will be used for testing only, but there could be use-cases for Onboard.
    /// </summary>
    public abstract class TimeProvider
    {
        private static TimeProvider current;

        public abstract DateTime UtcNow { get; }
        public abstract DateTime Today { get; }

        static TimeProvider()
        {
            current = new DefaultTimeProvider();
        }

        public static TimeProvider Current
        {
            get
            {
                return current;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                current = value;
            }
        }
        public static void ResetToDefault()
        {
            current = new DefaultTimeProvider();
        }
    }



    public class DefaultTimeProvider : TimeProvider
    {
        public override DateTime UtcNow
        {
            get
            {
                return DateTime.UtcNow;
            }
        }

        public override DateTime Today
        {
            get
            {
                return DateTime.Today;
            }
        }
    }



    public class StubTimeProvider : TimeProvider
    {
        private DateTime currentTime;


        public StubTimeProvider(DateTime currentTime)
        {
            this.currentTime = currentTime;
        }

        public override DateTime UtcNow
        {
            get
            {
                return currentTime;
            }
        }

        public override DateTime Today
        {
            get
            {
                return currentTime.Date;
            }
        }
    }
}