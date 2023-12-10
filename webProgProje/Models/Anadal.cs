using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace webProgProje.Models
{
    public class Anadal
    {
        [Key]
        [Required]
        public int AnadalID { get; set; }

        [Required]
        [MaxLength(30)]
        public string AnadalAd { get; set; }

        public ICollection<Doktor> DoktorListesi { get; set; }
    }
}
