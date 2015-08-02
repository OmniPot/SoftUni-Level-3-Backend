namespace _01.EF_Mapping_Db_First
{
    using System;
    using System.Linq;

    public class EFMapping
    {
        public static void Main()
        {
            var context = new DiabloEntities();

            var characterNames = context.Characters
                .Select(c => c.Name)
                .ToList();

            foreach (var characterName in characterNames)
            {
                Console.WriteLine(characterName);
            }
        }
    }
}
