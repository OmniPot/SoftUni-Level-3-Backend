namespace _02.Export_Man_Cameras_JSON
{
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;
    using _01.Entity_Framework_Mapping;

    public class JsonExporter
    {
        public static void Main()
        {
            var context = new PhotographySystemEntities();

            var manufacturersCameras = context.Manufacturers
                .Select(m => new
                {
                    manufacturer = m.Name,
                    cameras = m.Cameras.Select(c => new
                    {
                        model = c.Model,
                        price = c.Price
                    })
                    .OrderBy(c => c.model)
                })
                .OrderBy(m => m.manufacturer);

            var jsonResult = JsonConvert.SerializeObject(manufacturersCameras);
            File.WriteAllText("../../manufactureres-and-cameras.json", jsonResult);
        }
    }
}
