using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webProgProje.Models
{
    public class Kullanici
    {
        [Key]
        [StringLength(11)]
        public string TC { get; set; }

        [Required]
        [MaxLength(10)]
        public string KullaniciTipi { get; set; }

        [Required]
        [MaxLength(50)]
        public string Ad { get; set; }

        [Required]
        [MaxLength(50)]
        public string Soyad { get; set; }

        [Phone]
        [Required]
        [StringLength(11)]
        public string Telefon { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Sifre {  get; set; }

        [NotMapped]
        public int AnadalID {  get; set; }
        [NotMapped]
        public Doktor ?Doktor { get; set; }
        [NotMapped]
        public Hasta ?Hasta { get; set; }

        [NotMapped]
        public List<Anadal> ?AnadalList { get; set; }
    }
}
