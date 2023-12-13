using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace adeliamara.Models
{
    public class PagamentoComCheque : TipoDePagamento
    {
        [Display(Name = "Nome do Banco")]
        public string? NomeDoBanco { get; set; }

        public int Banco { get; set; }
    }

}