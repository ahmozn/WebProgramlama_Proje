using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webProgProje.Models
{
    public class Kullanici:IdentityUser
    {
        
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
        
        public Doktor ?Doktor { get; set; }
        
        public Hasta ?Hasta { get; set; }

        [NotMapped]
        public List<Anadal> ?AnadalList { get; set; }

        public int sifreOlustur()
        {
            Random r = new Random();
            int sifre = r.Next(1000, 9999);
            return sifre;
        }
    }
}
