using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webProgProje.Models
{
    public class Kullanici
    {
        [Key]
        [MaxLength(11)]
        [MinLength(11)]
        public string TC { get; set; }

        [Required]
        [MaxLength(10)]
        public string KullaniciTipi { get; set; }

        [Required]
        [MaxLength(100)]
        public string Ad { get; set; }

        [Required]
        [MaxLength(100)]
        public string Soyad { get; set; }

        [Phone]
        [Required]
        [MaxLength(100)]
        public string Telefon { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Sifre {  get; set; }

        [NotMapped]
        public int AnadalID {  get; set; }

    }
}
