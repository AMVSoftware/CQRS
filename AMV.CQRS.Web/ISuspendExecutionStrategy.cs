namespace AMV.CQRS
{
    public interface ISuspendExecutionStrategy
    {
        void Suspend();
        void Unsuspend();
    }

    public class NullSuspendExecutionStrategy : ISuspendExecutionStrategy
    {
        public void Suspend()
        {
            // nothing
        }


        public void Unsuspend()
        {
            // nothing
        }
    }
}