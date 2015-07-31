namespace _3_Selects
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using Ads.Data;

    public class SelectsMeasurer
    {
        public static void Main()
        {
            var ctx = new AdsEntities();
            var stopwatch = new Stopwatch();

            SelectEverythingVSSelectCertainColumns(ctx, stopwatch);
        }

        private static void SelectEverythingVSSelectCertainColumns(AdsEntities ctx, Stopwatch stopwatch)
        {
            var selectEverythingDurations = RunSelectEverythingMeasurements(ctx, stopwatch);
            var selectCertainDurations = RunSelectCertainColumnsMeasurements(ctx, stopwatch);

            var selectEverythingAverage = selectEverythingDurations.Sum(ts => ts.Ticks);
            var selectCertainAverage = selectCertainDurations.Sum(ts => ts.Ticks);

            Console.WriteLine("Select Everything query:");
            for (int i = 0; i < selectEverythingDurations.Count(); i++)
            {
                Console.WriteLine("Run" + i + " ticks: " + selectEverythingDurations[i].Ticks);
            }

            Console.WriteLine("\nSelect Certain query:");
            for (int i = 0; i < selectCertainDurations.Count(); i++)
            {
                Console.WriteLine("Run" + i + " ticks: " + selectCertainDurations[i].Ticks);
            }

            Console.WriteLine("Average time of EVERYTHING query in ticks: " + selectEverythingAverage);
            Console.WriteLine("Average time of CERTAIN query in ticks: " + selectCertainAverage);
        }

        private static TimeSpan[] RunSelectEverythingMeasurements(AdsEntities ctx, Stopwatch stopwatch)
        {
            ctx.Database.SqlQuery<Ad>("CHECKPOINT; DBCC DROPCLEANBUFFERS;");

            var everythingQueryRunDurations = new TimeSpan[10];
            for (int index = 0; index < 10; index++)
            {
                stopwatch.Reset();
                stopwatch.Start();

                var everything = ctx.Ads.ToList();
                everythingQueryRunDurations[index] = stopwatch.Elapsed;
            }

            stopwatch.Reset();

            return everythingQueryRunDurations;
        }

        private static TimeSpan[] RunSelectCertainColumnsMeasurements(AdsEntities ctx, Stopwatch stopwatch)
        {
            ctx.Database.SqlQuery<Ad>("CHECKPOINT; DBCC DROPCLEANBUFFERS;");

            var certainQueryRunDurations = new TimeSpan[10];
            for (int index = 0; index < 10; index++)
            {
                stopwatch.Reset();
                stopwatch.Start();

                var certainColumns = ctx.Ads.Select(a => a.Title).ToList();
                certainQueryRunDurations[index] = stopwatch.Elapsed;
            }

            stopwatch.Reset();

            return certainQueryRunDurations;
        }
    }
}
