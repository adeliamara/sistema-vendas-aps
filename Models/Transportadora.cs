using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace adeliamara.Models
{
    public class Transportadora
    {
        [Display (Name = "Código da Transportadora")]
        public int Id {get; set;}
        public string? Nome {get; set;}

        public virtual ICollection<Produto>? Produtos {get;set;}
    }
}