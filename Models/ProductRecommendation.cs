using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntexBrickwell.Models;

public partial class ProductRecommendation
{
    [Key]
    public byte ProductID { get; set; }
    public string? ProductName { get; set; }
    public byte? Rec1ID { get; set; }
    public string? Rec1Name { get; set; }
    public byte? Rec2ID { get; set; }
    public string? Rec2Name { get; set; }
    public byte? Rec3ID { get; set; }
    public string? Rec3Name { get; set; }
    public byte? Rec4ID { get; set; }
    public string? Rec4Name { get; set; }
    public byte? Rec5ID { get; set; }
    public string? Rec5Name { get; set; }
}