﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OrderCloud.Integrations.Library.Models;

namespace OrderCloud.Integrations.ExchangeRates.Models
{
    public class ConversionRate
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public CurrencyCode Currency { get; set; }

        public string Symbol { get; set; }

        public string Name { get; set; }

        public double? Rate { get; set; }

        public string Icon { get; set; }
    }
}
