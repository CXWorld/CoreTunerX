using CoreTunerX;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ReadEventLogExample
{
    class Program
    {
        static void Main(string[] args)
        {
            EventLog eventLog = new EventLog
            {
                Log = "System"
            };

            int threadCount = Environment.ProcessorCount;

            try
            {
                var filteredEntries = eventLog.Entries
                    .Cast<EventLogEntry>()
                    .Where(logEntry => logEntry.InstanceId == 55)
                    .TakeLast(threadCount)
                    .OrderBy(logEntry => Convert.ToInt32(logEntry.ReplacementStrings[1]));

                using (StreamWriter file =
                    new StreamWriter("results.txt"))
                {
                    foreach (var entry in filteredEntries)
                    {
                        file.WriteLine($"Core {entry.ReplacementStrings[1]} with performance number {entry.ReplacementStrings[5]}");
                    }
                }

                Process.Start("results.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine("Press any key...");
                Console.ReadKey();
            }
        }
    }
}