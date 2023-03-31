using System;
using System.Threading;


namespace дз4
{
    class Status
    {
        public int EventIndex { get; }
        public ManualResetEvent ResetEvent { get; }//событие синхронизации потока

        public Status(int primitiveEventIndex)
        {

            EventIndex = primitiveEventIndex;
            ResetEvent = new ManualResetEvent(false);

        }
    }
}
