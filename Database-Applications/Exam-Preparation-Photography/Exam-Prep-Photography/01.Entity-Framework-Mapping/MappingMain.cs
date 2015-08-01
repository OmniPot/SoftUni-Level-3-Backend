namespace _01.Entity_Framework_Mapping
{
    using System;
    using System.Linq;

    public class MappingMain
    {
        public static void Main()
        {
            var context = new PhotographySystemEntities();

            var cameraManufacturersModels = context.Cameras
                .Select(c => new
                {
                    ManufacturerModel = c.Manufacturer.Name + " " + c.Model
                })
                .OrderBy(c => c.ManufacturerModel);

            foreach (var camera in cameraManufacturersModels)
            {
                Console.WriteLine(camera.ManufacturerModel);
            }
        }
    }
}
