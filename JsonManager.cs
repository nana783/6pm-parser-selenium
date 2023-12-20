using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6pm_parser_selenium
{
    public static class JsonManager
    {
        public static void CompareAndWriteDifference(string firstFilePath, string secondFilePath, string outputFilePath)
        {
            try
            {
                // Зчитуємо вміст файлів
                string firstFileContent = File.ReadAllText(firstFilePath);
                string secondFileContent = File.ReadAllText(secondFilePath);

                // Перетворюємо JSON-рядки у об'єкти JToken
                JToken firstJson = JToken.Parse(firstFileContent);
                JToken secondJson = JToken.Parse(secondFileContent);

                // Визначаємо різницю між першим і другим JSON
                JToken difference = FindDifference(firstJson, secondJson);

                // Записуємо різницю у третій файл
                File.WriteAllText(outputFilePath, difference.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static JToken FindDifference(JToken first, JToken second)
        {
            // Рекурсивно порівнюємо елементи JSON
            if (first is JObject && second is JObject)
            {
                var firstObject = (JObject)first;
                var secondObject = (JObject)second;

                var diffObject = new JObject();

                foreach (var property in firstObject.Properties())
                {
                    var propertyName = property.Name;

                    if (secondObject.ContainsKey(propertyName))
                    {
                        var propertyDifference = FindDifference(property.Value, secondObject[propertyName]);
                        if (propertyDifference.HasValues)
                        {
                            diffObject.Add(propertyName, propertyDifference);
                        }
                    }
                    else
                    {
                        diffObject.Add(propertyName, property.Value);
                    }
                }

                return diffObject;
            }
            else if (first is JArray && second is JArray)
            {
                var firstArray = (JArray)first;
                var secondArray = (JArray)second;

                var diffArray = new JArray();

                foreach (var item in firstArray)
                {
                    var itemDifference = secondArray.FirstOrDefault(x => JToken.DeepEquals(x, item)) == null
                        ? item
                        : null;

                    if (itemDifference != null)
                    {
                        diffArray.Add(itemDifference);
                    }
                }

                return diffArray;
            }
            else
            {
                return JToken.DeepEquals(first, second) ? null : first;
            }
        }
    }
}
