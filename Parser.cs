using Newtonsoft.Json;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OpenQA.Selenium.DevTools.V118.Network;

namespace _6pm_parser_selenium
{
    internal class Parser
    {
        public static void parse()
        {
            IWebDriver driver = CreateWebDriver();
            MensShoesSortedByPercentOffPage(driver);
            ParseElementsToJSON(driver);
        }
        private static IWebDriver CreateWebDriver()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(@"https://www.6pm.com/");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(5000);
            return driver;
        }
        private static IWebDriver MensShoesSortedByPercentOffPage(IWebDriver driver)
        {           
            // Mens
            driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div[1]/header/div[2]/ul/li[6]/a")).Click();
            // Shoes
            driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div[2]/main/div/div/div[2]/div/div[2]/div/div[1]/article/a")).Click();
            // Sort by % Off
            driver.FindElement(By.XPath("//*[@id=\"searchSort\"]/option[6]")).Click();
            
            return driver;
        }
        private static void StringListToJSON(List<string> textLines)
        {
            string parsedProducctsJSON = "C:\\Users\\Дмитро\\source\\6pm-parser-selenium\\parsedProducts.json";
            
            string pattern = @"^(.+?) - (.+?)\. On sale for \$([\d.]+)\. MSRP \$([\d.]+)\.\.";

            // Створення об'єкта для зберігання результатів у форматі JSON
            var products = new System.Collections.Generic.List<Product>();

            using (StreamWriter writer = new StreamWriter(parsedProducctsJSON))
            {
                // Проходимося по кожному рядку та застосовуємо регулярний вираз
                foreach (string line in textLines)
                {
                    Match match = Regex.Match(line, pattern);

                    if (match.Success)
                    {
                        string brand = match.Groups[1].Value;
                        string product = match.Groups[2].Value;
                        string salePrice = match.Groups[3].Value;
                        string msrp = match.Groups[4].Value;

                        products.Add(new Product
                        {
                            Brand = brand,
                            ProductName = product,
                            SalePrice = salePrice,
                            MSRP = msrp
                        });
                    }
                }
            }

            string jsonOutput = JsonConvert.SerializeObject(products, Formatting.Indented);
            File.WriteAllText(parsedProducctsJSON, string.Empty);
            File.WriteAllText(parsedProducctsJSON, jsonOutput);

        }
        private static void ParseElementsToJSON(IWebDriver driver)
        {
            List<string> textLines = new List<string>();

            for (int i = 0; i < 1; i++)
            {                
                ReadOnlyCollection<IWebElement> elements = driver.FindElements(By.CssSelector(".mj-z"));
                
                List<string> elementsList = new List<string>();

                foreach (IWebElement element in elements)
                {
                    elementsList.Add(element.Text);
                }
               
                textLines.AddRange(elementsList);

                try
                {
                    driver.FindElement(By.XPath(@"/html/body/div[1]/div[1]/div[2]/main/div/div/div/div[7]/div[2]/a[2]")).Click();
                }
                catch (Exception)
                {
                    break;
                }
                StringListToJSON(textLines);

            }
        }
      
    }
}
