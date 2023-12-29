﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webProgProje.Models
{
    public class Hasta
    {
        [Key]
        public int HastaID {  get; set; }

        [ForeignKey("Kullanici")]
        public string Id {  get; set; }
        public Kullanici ?Kullanici { get; set; }

        public List<Randevu> ?AktifRandevular { get; set; }
    }
}
