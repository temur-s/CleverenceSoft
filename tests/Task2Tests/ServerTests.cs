using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Task2;
using Xunit;

namespace Task2Tests
{
    public class ServerTests
    {
        [Fact]
        public void Server_SequentialAdd_Success()
        {
            int initialCount = Server.GetCount();
            
            Server.AddToCount(10);
            
            Assert.Equal(initialCount + 10, Server.GetCount());
        }

        [Fact]
        public async Task Server_ConcurrentReads_Success()
        {
            Server.AddToCount(42);
            int seededCount = Server.GetCount();

            int readersCount = 50;
            var tasks = new List<Task>();

            for (int i = 0; i < readersCount; i++)
            {
                tasks.Add(Task.Run(() => 
                {
                    int currentCount = Server.GetCount();
                    Assert.Equal(seededCount, currentCount);
                }));
            }

            await Task.WhenAll(tasks);
        }

        [Fact]
        public async Task Server_ConcurrentWrites_Success()
        {
            int initialCount = Server.GetCount();
            int writersCount = 50;
            int addValue = 3;

            var tasks = new List<Task>();

            for (int i = 0; i < writersCount; i++)
            {
                tasks.Add(Task.Run(() => Server.AddToCount(addValue)));
            }

            await Task.WhenAll(tasks);

            int expectedFinalCount = initialCount + (writersCount * addValue);
            Assert.Equal(expectedFinalCount, Server.GetCount());
        }
    }
}
