using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace adeliamara.Models
{
    public class Marca
    {
        [Display(Name = "Código da Marca")]
        public int Id {get; set;}
        public string? Nome {get; set;}

        [Display(Name = "Descrição")]
        public string? Descricao {get; set;}
    }
}