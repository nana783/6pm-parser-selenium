using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6pm_parser_selenium
{
    // Клас для представлення продукту
    [JsonObject]
    internal class Product
    {
        [JsonProperty("brand")]
        public string? Brand { get; set; }

        [JsonProperty("name")]
        public string? ProductName { get; set; }

        [JsonProperty("sale_price")]
        public string? SalePrice { get; set; }

        [JsonProperty("msrp")]
        public string? MSRP { get; set; }
    }
}
