﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AdminPolizasAPI.Entidades
{
    public class PolizasCoberturas
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public int PolizaId { get; set; }

        public int CoberturaId { get; set; }

        public Poliza Poliza { get; set; }

        public Cobertura Cobertura { get; set; }

        public decimal MontoAsegurado { get; set; }
    }
}
