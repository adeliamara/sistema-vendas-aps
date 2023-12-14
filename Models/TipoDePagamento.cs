using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace adeliamara.Models
{
    public class TipoDePagamento
    {
        public int Id { get; set; }

        [Display(Name = "Nome do Cobrado")]
        public string? NomeDoCobrado { get; set; }

        [Display(Name = "Informações Adicionais")]
        public string? InformacoesAdicionais { get; set; }

        public string? Discriminator { get; set; }

    }
}