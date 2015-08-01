namespace _03.Export_Photographs_XML
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using _01.Entity_Framework_Mapping;

    public class XmlExporter
    {
        public static void Main()
        {
            var context = new PhotographySystemEntities();

            var photographs = GetPhotographsData(context);

            var photographsDoc = CreateXmlDocument(photographs);

            photographsDoc.Save("../../photographs.xml");
        }

        private static XDocument CreateXmlDocument(List<PhotographExportModel> photographs)
        {
            var root = new XElement("photographs");
            XDocument photographsDoc = new XDocument(root);

            foreach (var photo in photographs)
            {
                var photograph =
                    new XElement("photograph", new XAttribute("title", photo.Title),
                        new XElement("category", photo.CategoryName),
                        new XElement("link", photo.Link));

                var equipment = new XElement("equipment");
                equipment.Add(
                    new XElement("camera", photo.CameraModel,
                        new XAttribute("megapixels", photo.CameraMegaPixels)));

                var lens = new XElement("lens", photo.LensModel);
                if (photo.LensPrice != null)
                {
                    lens.Add(new XAttribute("price", Math.Round(photo.LensPrice.Value, 2)));
                }

                equipment.Add(lens);
                photograph.Add(equipment);
                photographsDoc.Root.Add(photograph);
            }
            return photographsDoc;
        }

        private static List<PhotographExportModel> GetPhotographsData(PhotographySystemEntities context)
        {
            var photographs = context.Photographs
                .Select(p => new PhotographExportModel
                {
                    Title = p.Title,
                    CategoryName = p.Category.Name,
                    Link = p.Link,
                    CameraModel = p.Equipment.Camera.Model,
                    CameraMegaPixels = p.Equipment.Camera.Megapixels,
                    LensPrice = p.Equipment.Lens.Price,
                    LensModel = p.Equipment.Lens.Model
                })
                .OrderBy(p => p.Title)
                .ToList();
            return photographs;
        }
    }
}
