using System.ComponentModel.DataAnnotations;
namespace IntexBrickwell.Models
{
    public class TwoFactor
    {
        [Required]
        public string TwoFactorCode { get; set; }
    }
}
