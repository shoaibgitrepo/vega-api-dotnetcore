using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vega_api_dotnetcore.Models
{
    public class Make
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        public ICollection<Model> Models { get; set; }

        public Make()
        {
            Models = new Collection<Model>();
        }
    }
}