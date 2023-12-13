using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace adeliamara.Models
{
    public class Cliente
    {
        [Display (Name = "Código do Cliente")]
        public int Id {get; set;}
        public string? Nome {get; set;}
        public virtual ICollection<Produto>? Produtos {get;set;}

    }
}