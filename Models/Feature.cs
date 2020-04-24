using System.ComponentModel.DataAnnotations;

namespace vega_api_dotnetcore.Models
{
    public class Feature
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}