namespace _04.Import_Man_Lenses_XML
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using _01.Entity_Framework_Mapping;

    public class XmlImporter
    {
        public static void Main()
        {
            var context = new PhotographySystemEntities();
            var doc = XDocument.Load("../../manufacturers-and-lenses.xml");

            ProcessManufacturers(doc, context);
            context.SaveChanges();
        }

        private static void ProcessManufacturers(XDocument doc, PhotographySystemEntities context)
        {
            foreach (var man in doc.Root.Descendants("manufacturer"))
            {
                var manName = man.Element("manufacturer-name").Value;
                Console.WriteLine("Processing manufacurer: " + manName);
                var manufacturer = context.Manufacturers.FirstOrDefault(m => m.Name.Equals(manName));

                if (manufacturer == null)
                {
                    Console.WriteLine("Created manufacurer: " + manName);
                    manufacturer = new Manufacturer()
                    {
                        Name = manName,
                        Lenses = new HashSet<Lens>(),
                        Cameras = new HashSet<Camera>()
                    };
                }
                else
                {
                    Console.WriteLine("Existing manufacurer: " + manufacturer.Name);
                }

                ProcessLenses(man, context, manufacturer);
                Console.WriteLine();
            }
        }

        private static void ProcessLenses(XElement man, PhotographySystemEntities context, Manufacturer manufacturer)
        {
            foreach (var xmlLens in man.Element("lenses").Descendants("lens"))
            {
                var lensModel = xmlLens.Attribute("model").Value;
                var lensType = xmlLens.Attribute("type").Value;
                var lensPrice = xmlLens.Attribute("price");

                var lens = context.Lenses.FirstOrDefault(l => l.Model.Equals(lensModel));

                if (lens == null)
                {
                    Console.WriteLine("Created lense: " + lensModel);
                    lens = new Lens()
                    {
                        Model = lensModel,
                        Type = lensType
                    };

                    if (lensPrice != null)
                    {
                        lens.Price = decimal.Parse(lensPrice.Value);
                    }
                }
                else
                {
                    Console.WriteLine("Existing lense: " + lensModel);
                }

                manufacturer.Lenses.Add(lens);
            }
        }
    }
}
