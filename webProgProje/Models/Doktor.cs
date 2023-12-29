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
        public string Id {  get; set;}
        public Kullanici ?Kullanici { get; set;}

        public int AnadalID {  get; set; }
        public Anadal ?Anadal { get; set; }
        public List<Randevu> ?AktifRandevular { get; set; }

        public Doktor()
        {
            CombineContext _combineContext= new CombineContext();
            this.AktifRandevular = new List<Randevu>();
            var randevular = _combineContext.Randevular.Where(x => x.DoktorID == DoktorID);
            foreach(var item in randevular)
            {
                AktifRandevular.Add(item);
            }
        }
    }
}
