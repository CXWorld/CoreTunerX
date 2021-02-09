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
                    .Take(threadCount)
                    .OrderBy(logEntry => Convert.ToInt32(logEntry.ReplacementStrings[1]))
                    .ToList();

                using (StreamWriter file =
                    new StreamWriter("results.txt"))
                {
                    for (int i = 0; i < filteredEntries.Count; i++)
                    {
                        file.WriteLine($"Core {filteredEntries[i].ReplacementStrings[1]} with performance number {filteredEntries[i].ReplacementStrings[5]}");
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