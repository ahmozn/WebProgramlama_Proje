using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webProgProje.Models
{
    public class Doktor
    {
        [Key]
        public int DoktorID { get; set; }

        [Required]
        [MaxLength(30)]
        public string DoktorDerece { get; set;}

        [ForeignKey("Kullanici")]
        public string TC {  get; set;}
        public Kullanici Kullanici { get; set;}

        public int AnadalID {  get; set; }
        public Anadal Anadal { get; set; }
        public ICollection<Randevu> AktifRandevular { get; set; }
    }
}
