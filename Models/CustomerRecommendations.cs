using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntexBrickwell.Models;

public partial class CustomerRecommendations
{
    [Key]
    public int Customer_ID { get; set; }
    public string? Recommendation_1 { get; set; }
    public string? Recommendation_2 { get; set; }
    public string? Recommendation_3 { get; set; }
    public string? Recommendation_4 { get; set; }
    public string? Recommendation_5 { get; set; }
}