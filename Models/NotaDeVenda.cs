using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace adeliamara.Models
{
    public class NotaDeVenda
    {

        [Display(Name = "Codigo da Nota")]
        public int Id { get; set; }

        public DateTime Data { get; set; }
        public Situacao Situacao { get; set; }
        public DateTime? DataCancelamento { get; set; }
        public DateTime? DataDevolucao { get; set; }
        public NotaDeVenda()
        {
            Data = DateTime.Now;
            Situacao = Situacao.Processada;
        }

        [Display(Name = " Cliente")]
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        [Display(Name = "Vendedor")]
        public int VendedorId { get; set; }
        public Vendedor? Vendedor { get; set; }

        [Display(Name = "Transportadora")]
        public int TransportadoraId { get; set; }
        public Transportadora? Transportadora { get; set; }

        [Display(Name = "Tipo De Pagamento")]
        public int TipoDePagamentoId { get; set; }
        public TipoDePagamento? TipoDePagamento { get; set; }

        public virtual ICollection<Pagamento>? Pagamentos { get; set; }
        public ICollection<Item> Itens { get; set; } = new Collection<Item>();

        public void Cancelar()
        {
            if (Situacao != Situacao.Cancelada)
            {
                Situacao = Situacao.Cancelada;
                DataCancelamento = DateTime.Now;
            }
        }

        public void Devolver()
        {
            if (Situacao != Situacao.Devolvida)
            {
                if ((DateTime.Now - Data).Days > 7)
                {
                    throw new InvalidOperationException("A devolução não é permitida após 7 dias da data de compra.");

                }
                Situacao = Situacao.Devolvida;
                DataDevolucao = DateTime.Now;
            }
        }

    }
}