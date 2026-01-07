using GymManagementDAL.Context;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymManagementDAL.GymDbContextSeeding
{
    public class GymDbContextSeeding
    {
        public static bool SeedData(GymContext dbContext, string contentRootPath)
        {
            try
            {
                bool hasCategories = dbContext.Categories.Any();
                bool hasPlans = dbContext.Plans.Any();

                //if (hasCategories && hasPlans) return false;

                if (!hasCategories)
                {
                    var categories = LoadDataFromJsonFile<Category>("categories.json", contentRootPath);
                    dbContext.Categories.AddRange(categories);
                }

                if (!hasPlans)
                {
                    var plans = LoadDataFromJsonFile<Plan>("plans.json", contentRootPath);
                    dbContext.Plans.AddRange(plans);
                }

                Console.WriteLine("=== SEED STARTED ===");

                Console.WriteLine($"HasCategories: {dbContext.Categories.Any()}");
                Console.WriteLine($"HasPlans: {dbContext.Plans.Any()}");

                Console.WriteLine($"ContentRootPath: {contentRootPath}");
                //Console.WriteLine($"Seed file path: {filePath}");

                Console.WriteLine($"Loaded categories: {dbContext.Categories.Count()}");
                //Console.WriteLine($"Loaded plans: {plans.Count}");


                return dbContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding Failed: {ex}");
                return false;
            }

        }


        private static List<T> LoadDataFromJsonFile<T>(string fileName, string contentRootPath)
        {
            var filePath = Path.Combine(contentRootPath, "wwwroot", "Files", fileName);

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Seed file not found: {filePath}");

            string data = File.ReadAllText(filePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new JsonStringEnumConverter());

            return JsonSerializer.Deserialize<List<T>>(data, options) ?? new();
        }

    }
}
