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
                    .OrderBy(logEntry => Convert.ToInt32(logEntry.ReplacementStrings[1]))
                    .ToList();

                var isSmtOn = filteredEntries.Where((x, i) => i % 2 == 1)
                    .Select(logEntry => Convert.ToInt32(logEntry.ReplacementStrings[5]))
                    .SequenceEqual(filteredEntries.Where((x, i) => i % 2 == 0)
                    .Select(logEntry => Convert.ToInt32(logEntry.ReplacementStrings[5])));

                if (isSmtOn)
                    filteredEntries = filteredEntries.Where((x, i) => i % 2 == 0).ToList();

                using (StreamWriter file =
                    new StreamWriter("results.txt"))
                {
                    for (int i = 0; i < filteredEntries.Count; i++)
                    {
                        file.WriteLine($"Core {i} with performance number {filteredEntries[i].ReplacementStrings[5]}");
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