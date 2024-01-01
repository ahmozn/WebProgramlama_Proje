using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace webProgProje.Models
{
    public class Randevu
    {
        [Key]
        public int RandevuID { get; set; }
        [Required]
        public DateOnly RandevuDate { get; set; }
        [Required]
        public TimeOnly RandevuTime { get; set; }


        [Required]
        public bool Durum { get; set; }

        [ForeignKey("Doktor")]
        public int DoktorID { get; set; }
        public Doktor ?Doktor { get; set; }

        [ForeignKey("Anadal")]
        public int AnadalID { get; set; }
        public Anadal ?Anadal { get; set; }

        [ForeignKey("Hasta")]
        public int? HastaID { get; set; }
        public Hasta ?Hasta { get; set; }

        [NotMapped]
        public DateTime dateANDtime { get; set; }
        [NotMapped]
        public List<Anadal> ?AnadalList { get; set; }
        [NotMapped]
        public List<Doktor> ?DoktorList { get; set; }

    }
}
