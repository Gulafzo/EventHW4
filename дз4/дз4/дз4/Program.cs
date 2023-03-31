using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace дз4
{
    class Program
    {
        private static readonly object _indexLockObj = new object();
        static int index = 0;

        static void Modification(int namber, int primitiveEndedEventIndex)
        {

            lock (_indexLockObj)                                                          //  lock это оператор, который явно упрощает код
            {                                                                             //без вызова метода Monitor.Exit. для этого у lock 
                index += namber;                                                           //есть { }
                Console.WriteLine($"[{primitiveEndedEventIndex}] -> index = {index}");
            }
        }
        static void FunctionModification(object statusObj)
        {
            Status currentStatus = (Status)statusObj;
            int primitiveEndedEventIndex = currentStatus.EventIndex;
            ManualResetEvent primitiveEndedEvent = currentStatus.ResetEvent;

            if ((primitiveEndedEventIndex >= 0 && primitiveEndedEventIndex < 50)
                || (primitiveEndedEventIndex >= 100 && primitiveEndedEventIndex < 150))
                Modification(1, primitiveEndedEventIndex);
            else
                Modification(-1, primitiveEndedEventIndex);
            primitiveEndedEvent.Set();
        }
        static void Main(string[] args)
        {
            Status[] status = new Status[200];
            var tasks = new Task[100];
            for (int count = 0; count < 100; count++)
            {
                int ID = count;
                status[ID] = new Status(ID);
                tasks[ID] = Task.Run(() => FunctionModification(status[ID]));
            }
            var threads = new Thread[100];

            for (int count = 100; count < 200; count++)
            {
                status[count] = new Status(count);
                threads[count - 100] = new Thread(FunctionModification);
                threads[count - 100].Start(status[count]);
            }
            WaitHandle.WaitAll(status.Take(64).Select(x => x.ResetEvent).ToArray());
            WaitHandle.WaitAll(status.Skip(64).Take(64).Select(x => x.ResetEvent).ToArray());
            WaitHandle.WaitAll(status.Skip(128).Take(64).Select(x => x.ResetEvent).ToArray());
            WaitHandle.WaitAll(status.Skip(192).Take(64).Select(x => x.ResetEvent).ToArray());

            Console.ReadLine();

        }
    }
}
