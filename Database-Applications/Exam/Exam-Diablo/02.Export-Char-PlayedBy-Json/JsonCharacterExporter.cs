namespace _02.Export_Char_PlayedBy_Json
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;
    using _01.EF_Mapping_Db_First;

    public class JsonCharacterExporter
    {
        public static void Main()
        {
            var filePath = @"..\..\..\02.Export-Char-PlayedBy-Json";
            var fileName = @"\characters.json";

            var context = new DiabloEntities();

            var charsPlayedByUsers = context.Characters
                .Select(c => new
                {
                    name = c.Name,
                    playedBy = c.UsersGames.Select(g => g.User.Username)
                })
                .OrderBy(c => c.name)
                .ToList();

            var jsonResult = JsonConvert.SerializeObject(charsPlayedByUsers);
            File.WriteAllText(filePath + fileName, jsonResult);

            Process.Start(filePath);
        }
    }
}
