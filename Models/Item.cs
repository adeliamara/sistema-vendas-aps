using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace adeliamara.Models
{
    public class Item
    {        
        [Display(Name = "Código do Item")]
        public int Id {get; set;}
        
        [Display(Name = "Preço")]
        public double Preco {get; set;}

        public double Percentual {get; set;}

        public int Quantidade {get; set;}

        [Display(Name = "Nota de Venda")]
        [ForeignKey("NotaDeVenda")]
        public int NotaDeVendaId {get; set;}
        public NotaDeVenda? NotaDeVenda {get; set;}

        [Display(Name = "Produto")]

        [ForeignKey("Produto")]
        public int ProdutoId {get; set;}
        public Produto? Produto {get; set;}

        
    }
}