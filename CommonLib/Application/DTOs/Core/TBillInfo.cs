#nullable disable
using System;
using System.Collections.Generic;
namespace CommonLib.Application.DTOs.Core;

/// <summary>TBillInfo data transfer object (domain: Core).</summary>
public class TBillInfo
{
    public int BillId { get; set; }
    public int OrderId { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal Tip { get; set; }
    public decimal Total { get; set; }
    public TPaymentMethod PaymentMethod { get; set; }
    public DateTime PaidDate { get; set; }
}
