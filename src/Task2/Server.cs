using System.Threading;

namespace Task2
{
    public static class Server
    {
        private static int _count = 0;
        private static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        /// <summary>
        /// Reads the current value of count. Multiple threads can read concurrently.
        /// </summary>
        /// <returns>The current count.</returns>
        public static int GetCount()
        {
            _lock.EnterReadLock();

            try
            {
                return _count;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Adds a value to count. 
        /// Only one thread can write at a time.
        /// While a write is in progress, readers will be blocked.
        /// </summary>
        /// <param name="value">The value to add.</param>
        public static void AddToCount(int value)
        {
            _lock.EnterWriteLock();

            try
            {
                _count += value;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }
}
