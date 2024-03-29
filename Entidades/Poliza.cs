﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AdminPolizasAPI.Entidades
{
    public class Poliza
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Nombre { get; set; }

        public List<PolizasCoberturas> PolizasCoberturas { get; set; }
    }
}
