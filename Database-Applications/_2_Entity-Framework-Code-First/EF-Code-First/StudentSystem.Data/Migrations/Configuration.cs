namespace StudentSystem.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<StudentSystemContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(StudentSystemContext context)
        {
            Random random = new Random();
            FillCourses(context, random);
            FillStudents(context, random);
            FillHomeworks(context, random);
            FillLicenses(context);
            FillResources(context, random);
        }

        private static void FillCourses(StudentSystemContext context, Random random)
        {
            if (!context.Courses.Any())
            {
                using (var reader = new StreamReader("courses.txt"))
                {
                    var line = reader.ReadLine();
                    while (line != null)
                    {
                        var data = line.Split(',');
                        context.Courses.Add(new Course()
                        {
                            Name = data[0],
                            Description = data[1],
                            StartDate = DateTime.ParseExact(data[2], "d/M/yyyy", CultureInfo.InvariantCulture),
                            EndDate = DateTime.ParseExact(data[3], "d/M/yyyy", CultureInfo.InvariantCulture),
                            Price = random.Next(100, 201)
                        });

                        line = reader.ReadLine();
                    }
                }

                context.SaveChanges();
            }
        }

        private static void FillStudents(StudentSystemContext context, Random random)
        {
            if (!context.Students.Any())
            {
                using (var reader = new StreamReader("students.txt"))
                {
                    var line = reader.ReadLine();
                    while (line != null)
                    {
                        var data = line.Split(',');
                        var name = data[0];
                        var phoneNumber = !data[1].Equals("NULL") ? data[1] : null;
                        var registeredOn = DateTime.ParseExact(data[2], "d/M/yyyy", CultureInfo.InvariantCulture);
                        var birthDate = !data[3].Equals("NULL")
                            ? (DateTime?)DateTime.ParseExact(data[3], "d/M/yyyy", CultureInfo.InvariantCulture)
                            : null;

                        var courses = new HashSet<Course>();
                        var coursesCount = random.Next(1, 3);
                        for (int index = 0; index < coursesCount; index++)
                        {
                            var newCourseId = random.Next(1, context.Courses.Count() + 1);
                            courses.Add(context.Courses.First(c => c.Id == newCourseId));
                        }

                        context.Students.Add(new Student()
                        {
                            Name = name,
                            PhoneNumber = phoneNumber,
                            RegisteredOn = registeredOn,
                            BirthDay = birthDate,
                            Courses = courses
                        });

                        line = reader.ReadLine();
                    }
                }

                context.SaveChanges();
            }
        }

        private static void FillResources(StudentSystemContext context, Random random)
        {
            if (!context.Resources.Any())
            {
                using (var reader = new StreamReader("resources.txt"))
                {
                    var line = reader.ReadLine();
                    while (line != null)
                    {
                        var data = line.Split(',');
                        var name = data[0];
                        var type = (ResourceType)int.Parse(data[1]);
                        var url = data[2];

                        var newCourseId = random.Next(1, 4);
                        var newCourse = context.Courses.First(c => c.Id == newCourseId);

                        var licenses = new HashSet<License>();
                        var licensesCount = random.Next(1, 3);
                        for (int i = 0; i < licensesCount; i++)
                        {
                            var newLicenseId = random.Next(1, 11);
                            licenses.Add(context.Licenses.First(l => l.Id == newLicenseId));
                        }

                        context.Resources.Add(new Resource()
                        {
                            Name = name,
                            Type = type,
                            URL = url,
                            Course = newCourse,
                            Licenses = licenses

                        });

                        line = reader.ReadLine();
                    }
                }

                context.SaveChanges();
            }
        }

        private static void FillHomeworks(StudentSystemContext context, Random random)
        {
            if (!context.Homeworks.Any())
            {
                using (var reader = new StreamReader("homeworks.txt"))
                {
                    var line = reader.ReadLine();
                    while (line != null)
                    {
                        var data = line.Split(',');
                        var content = data[0];
                        var type = (ContentType)int.Parse(data[1]);
                        var submissionDate = DateTime.ParseExact(data[2], "d/M/yyyy", CultureInfo.InvariantCulture);
                        var newCourseId = random.Next(1, 4);
                        var newCourse = context.Courses.First(c => c.Id == newCourseId);
                        var newStudentId = random.Next(1, 11);
                        var newStudent = context.Students.First(s => s.Id == newStudentId);

                        context.Homeworks.Add(new Homework()
                        {
                            Content = content,
                            ContentType = type,
                            SubmissionDate = submissionDate,
                            Course = newCourse,
                            Student = newStudent
                        });

                        line = reader.ReadLine();
                    }
                }

                context.SaveChanges();
            }
        }

        private static void FillLicenses(StudentSystemContext context)
        {
            if (!context.Licenses.Any())
            {
                using (var reader = new StreamReader("licenses.txt"))
                {
                    var line = reader.ReadLine();
                    while (line != null)
                    {
                        var name = line;

                        context.Licenses.Add(new License()
                        {
                            Name = name
                        });

                        line = reader.ReadLine();
                    }
                }

                context.SaveChanges();
            }
        }
    }
}
