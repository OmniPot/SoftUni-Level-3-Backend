namespace _2_Play_With_ToList
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using Ads.Data;

    public class PlayWithToList
    {
        public static void Main()
        {
            var ctx = new AdsEntities();
            var stopwatch = new Stopwatch();

            ExecuteMeasurements(ctx, stopwatch);
        }

        private static void ExecuteMeasurements(AdsEntities ctx, Stopwatch stopwatch)
        {
            var firstQueryRunDurations = RunFirstQueryMeasurements(ctx, stopwatch);
            var secondQueryRunDurations = RunSecondQueryMeasurements(ctx, stopwatch);

            var firstQueryAverage = firstQueryRunDurations.Sum(ts => ts.Ticks);
            var secondQueryAverage = secondQueryRunDurations.Sum(ts => ts.Ticks);

            Console.WriteLine("Bad query:");
            for (int i = 0; i < firstQueryRunDurations.Count(); i++)
            {
                Console.WriteLine("Run" + i + " ticks: " + firstQueryRunDurations[i].Ticks);
            }

            Console.WriteLine("\nGood query:");
            for (int i = 0; i < secondQueryRunDurations.Count(); i++)
            {
                Console.WriteLine("Run" + i + " ticks: " + secondQueryRunDurations[i].Ticks);
            }

            Console.WriteLine("Average time of BAD query in ticks: " + firstQueryAverage);
            Console.WriteLine("Average time of GOOD query in ticks: " + secondQueryAverage);
        }

        private static TimeSpan[] RunFirstQueryMeasurements(AdsEntities ctx, Stopwatch stopwatch)
        {
            ctx.Database.SqlQuery<Ad>("CHECKPOINT; DBCC DROPCLEANBUFFERS;");

            var firstQueryRunDurations = new TimeSpan[10];
            for (int index = 0; index < 10; index++)
            {
                stopwatch.Reset();
                stopwatch.Start();

                var adsQueryStupid = ctx.Ads.ToList()
                    .Where(a => a.AdStatus.Status.Equals("Published"))
                    .Select(a => new
                    {
                        a.Title,
                        a.Category,
                        a.Town,
                        a.Date
                    })
                    .ToList()
                    .OrderBy(a => a.Date);

                firstQueryRunDurations[index] = stopwatch.Elapsed;
            }

            stopwatch.Reset();

            return firstQueryRunDurations;
        }

        private static TimeSpan[] RunSecondQueryMeasurements(AdsEntities ctx, Stopwatch stopwatch)
        {
            ctx.Database.SqlQuery<Ad>("CHECKPOINT; DBCC DROPCLEANBUFFERS;");

            var secondQueryRunDurations = new TimeSpan[10];
            for (int index = 0; index < 10; index++)
            {
                stopwatch.Reset();
                stopwatch.Start();

                var adsQueryGood = ctx.Ads
                .Where(a => a.AdStatus.Status.Equals("Published"))
                .OrderBy(a => a.Date)
                .Select(a => new
                {
                    a.Title,
                    a.Category,
                    a.Town,
                })
                .ToList();

                secondQueryRunDurations[index] = stopwatch.Elapsed;
            }

            stopwatch.Reset();

            return secondQueryRunDurations;
        }
    }
}
