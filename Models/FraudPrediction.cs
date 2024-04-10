using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntexBrickwell.Models;

public class FraudPrediction
{
    public Order Order { get; set; }
    public string Prediction { get; set; }
}