using System.Collections.Generic;
using Headstart.Models;
using Headstart.Models.Headstart;
using OrderCloud.Integrations.Library.Models;
using OrderCloud.SDK;

namespace Headstart.Common.Services.ShippingIntegration.Models
{
    public static class HSOrderWorksheetExtensions
    {
        public static bool IsStandardOrder(this HSOrderWorksheet sheet)
        {
            return sheet.Order.xp == null || sheet.Order.xp.OrderType != OrderType.Quote;
        }
    }

    public class HSOrderWorksheet : OrderWorksheet<HSOrder, HSLineItem, HSShipEstimateResponse, HSOrderCalculateResponse, OrderSubmitResponse, OrderSubmitForApprovalResponse, OrderApprovedResponse>
    {
    }

    public class HSOrderCalculatePayload
    {
        public HSOrderWorksheet OrderWorksheet { get; set; }

        public CheckoutIntegrationConfiguration ConfigData { get; set; }
    }

    public class ShipEstimateResponseXP
    {
    }

    public class ShipEstimateXP
    {
        public List<HSShipMethod> AllShipMethods { get; set; }

        public string SupplierID { get; set; }

        public string ShipFromAddressID { get; set; }
    }

    public class ShipMethodXP
    {
        public string Carrier { get; set; } // e.g. "Fedex"

        public string CarrierAccountID { get; set; }

        public decimal ListRate { get; set; }

        public bool Guaranteed { get; set; }

        public decimal OriginalCost { get; set; }

        public bool FreeShippingApplied { get; set; }

        public int? FreeShippingThreshold { get; set; }

        public CurrencyCode? OriginalCurrency { get; set; }

        public CurrencyCode? OrderCurrency { get; set; }

        public double? ExchangeRate { get; set; }
    }

    public class HSShipMethod : ShipMethod<ShipMethodXP>
    {
    }

    public class HSShipEstimate : ShipEstimate<ShipEstimateXP, HSShipMethod>
    {
    }

    public class HSShipEstimateResponse : ShipEstimateResponse<ShipEstimateResponseXP, HSShipEstimate>
    {
    }

    public class CheckoutIntegrationConfiguration
    {
        public bool ExcludePOProductsFromShipping { get; set; }

        public bool ExcludePOProductsFromTax { get; set; }
    }
}
