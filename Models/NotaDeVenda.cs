using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace adeliamara.Models
{
    public class NotaDeVenda
    {        
        
        [Display(Name = "Codigo da Nota]")]
        public int Id {get; set;}
        
       // [DataType(DataType.Date)]
        public DateTime Data { get; set; }
        public Boolean Status {get; set;}

        [Display(Name = " Cliente")]
        public int ClienteId {get; set;}
        public Cliente? Cliente {get; set;}

        [Display(Name = "Vendedor")]
        public int VendedorId {get; set;}
        public Vendedor? Vendedor {get; set;}

        [Display(Name = "Transportadora")]
        public int TransportadoraId {get; set;}
        public Transportadora? Transportadora {get; set;}

        [Display(Name = "Tipo De Pagamento")]
        public int TipoDePagamentoId {get; set;}
        public TipoDePagamento? TipoDePagamento {get; set;}
    
        public virtual ICollection<Pagamento>? Pagamentos {get;set;}

    }
}