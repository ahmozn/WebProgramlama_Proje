﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webProgProje.Models
{
    public class Admin
    {
        [Key]
        public int AdminID { get; set; }

        [ForeignKey("Kullanici")]
        public string Id {  get; set; }
        public Kullanici Kullanici { get; set; }

    }
}
