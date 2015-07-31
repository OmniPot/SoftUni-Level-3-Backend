namespace StudentSystem.CClient
{
    using System;
    using System.Linq;
    using Data;

    public class StudentSystemMain
    {
        public static void Main()
        {
            var ctx = new StudentSystemContext();

            ListStudentsAndTheirSubmissions(ctx);
            Console.WriteLine(new string('-', 70));

            ListCoursesWithResources(ctx);
            Console.WriteLine(new string('-', 70));

            ListCoursesWithMoreThan3Students(ctx);
            Console.WriteLine(new string('-', 70));

            ListActiveCoursesOnAGivenDate(ctx);
            Console.WriteLine(new string('-', 70));

            ListStudentsNumberOfCoursesAndPrice(ctx);
            Console.WriteLine(new string('-', 70));
        }

        private static void ListStudentsAndTheirSubmissions(StudentSystemContext ctx)
        {
            var students = ctx.Students
                .Select(s => new
                {
                    s.Name,
                    Homeworks = s.Homeworks.Select(h => new
                    {
                        h.Content,
                        h.ContentType
                    })
                });

            foreach (var student in students)
            {
                Console.WriteLine("--" + student.Name);

                foreach (var homework in student.Homeworks)
                {
                    Console.WriteLine(homework.Content + "; " + homework.ContentType);
                }

                Console.WriteLine();
            }
        }

        private static void ListCoursesWithResources(StudentSystemContext ctx)
        {
            var courses = ctx.Courses
                .OrderBy(c => c.StartDate)
                .ThenByDescending(c => c.EndDate)
                .Select(c => new
                {
                    c.Name,
                    c.Description,
                    c.Resources
                });

            foreach (var course in courses)
            {
                Console.WriteLine("--" + course.Name);

                foreach (var res in course.Resources)
                {
                    Console.WriteLine(res.Name + "; " + res.Type + "; " + res.URL);
                }

                Console.WriteLine();
            }
        }

        private static void ListCoursesWithMoreThan3Students(StudentSystemContext ctx)
        {
            var courses = ctx.Courses
                .Where(c => c.Students.Count > 3)
                .OrderByDescending(c => c.Resources.Count)
                .ThenByDescending(c => c.StartDate)
                .Select(c => new
                {
                    c.Name,
                    ResourceCount = c.Resources.Count
                });

            foreach (var course in courses)
            {
                Console.WriteLine("--" + course.Name);
                Console.WriteLine("Resource count: " + course.ResourceCount);
            }
        }

        private static void ListActiveCoursesOnAGivenDate(StudentSystemContext ctx)
        {
            var activeOn = new DateTime(2014, 2, 5);
            var courses = ctx.Courses
                .Where(c => c.StartDate < activeOn && c.EndDate > activeOn)
                .ToList()
                .Select(c => new
                {
                    c.Name,
                    c.StartDate,
                    c.EndDate,
                    Duration = (c.EndDate - c.StartDate).Days,
                    StudentsCount = c.Students.Count
                })
                .OrderByDescending(c => c.StudentsCount)
                .ThenByDescending(c => c.Duration);

            foreach (var course in courses)
            {
                Console.WriteLine("--" + course.Name);
                Console.WriteLine("Start date: " + course.StartDate.ToShortDateString());
                Console.WriteLine("End date: " + course.EndDate.ToShortDateString());
                Console.WriteLine("Duration: " + course.Duration + " days");
                Console.WriteLine("Students enrolled: " + course.StudentsCount);

                Console.WriteLine();
            }
        }

        private static void ListStudentsNumberOfCoursesAndPrice(StudentSystemContext ctx)
        {
            var students = ctx.Students
                .Select(s => new
                {
                    s.Name,
                    CoursesCount = s.Courses.Count,
                    TotalPrice = s.Courses.Sum(c => c.Price),
                    AveragePricePerCourse = s.Courses.Sum(c => c.Price) / s.Courses.Count
                })
                .OrderByDescending(s => s.TotalPrice)
                .ThenByDescending(s => s.CoursesCount)
                .ThenBy(s => s.Name);

            foreach (var student in students)
            {
                Console.WriteLine("--" + student.Name);
                Console.WriteLine("Courses count: " + student.CoursesCount);
                Console.WriteLine("Total price: " + student.TotalPrice);
                Console.WriteLine("Average price per course: " + student.AveragePricePerCourse);
            }
        }
    }
}
