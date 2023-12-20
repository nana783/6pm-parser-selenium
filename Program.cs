using _6pm_parser_selenium;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V118.Browser;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;

class Start{
    static void Main(string[] args)
    {
        /*string parsedProducctsJSON = "C:\\Users\\Дмитро\\source\\6pm-parser-selenium\\parsedProducts.json";
        string beforeParsedProducctsJSON = "C:\\Users\\Дмитро\\source\\6pm-parser-selenium\\beforeParsedProducts.json";
        string result = "C:\\Users\\Дмитро\\source\\6pm-parser-selenium\\result.json";*/

        string parsedProducctsJSON = Path.Combine(Environment.CurrentDirectory, "parsedProducts.json");
        string beforeParsedProducctsJSON = Path.Combine(Environment.CurrentDirectory, "beforeParsedProducts.json");
        string result = Path.Combine(Environment.CurrentDirectory, "result.json");

        Parser.parse();
        JsonManager.CompareAndWriteDifference(parsedProducctsJSON, beforeParsedProducctsJSON, result);


        var botClient = new TelegramBotClient("6444772771:AAHLNVn_P25oyzY1PgBLfoYfArt5_9BJPpw");
        botClient.StartReceiving(Update, Error);
        Console.ReadLine();


    }
    async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
    {
       

        /*string jsonFilePath = "C:\\Users\\Дмитро\\source\\6pm-parser-selenium\\result.json";

        // Зчитування JSON з файлу
        string jsonString = System.IO.File.ReadAllText(jsonFilePath);

        // Перетворення JSON у список об'єктів
        List<Product> products = JsonConvert.DeserializeObject<List<Product>>(jsonString);*/

        var msg = update.Message;
        if (msg is not { } message)
            return;

        await botClient.SendTextMessageAsync(msg.Chat.Id, "Для старту /start");
        await botClient.SendTextMessageAsync(msg.Chat.Id, "Чекаєм завозу. Я сповіщу, не хвилюйся.");
        return;
       /* if (message.Text != null)
        {
            if (message.Text.ToLower().Contains("start"))
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Чекаєм завозу. Я сповіщу, не хвилюйся.");
                while (true)
                {
                    foreach (var product in products)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"Brand: {product.Brand}, Name: {product.ProductName}, Sale Price: {product.SalePrice}, MSRP: {product.MSRP}");
                    }
                    await Task.Delay(100000);
                }
            }
        }*/
    }
    async static Task Error(ITelegramBotClient botClient, Exception exception, CancellationToken token)
    {

    }
 
}

