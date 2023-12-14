#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using adeliamara.Models;

namespace adeliamara.Controllers
{
    public class NotasDeVendaController : Controller
    {
        private readonly MyDbContext _context;

        public NotasDeVendaController(MyDbContext context)
        {
            _context = context;
        }

        // GET: NotasDeVenda
        public async Task<IActionResult> Index(string filtro)
        {
            IQueryable<NotaDeVenda> query = _context.NotaDeVenda.Include(n => n.Cliente)
                .Include(n => n.TipoDePagamento)
                .Include(n => n.Transportadora)
                .Include(n => n.Vendedor);

            switch (filtro)
            {
                case "canceladas":
                    query = query.Where(n => n.Situacao == Situacao.Cancelada);
                    break;
                case "devolvidas":
                    query = query.Where(n => n.Situacao == Situacao.Devolvida);
                    break;
                case "processadas":
                    query = query.Where(n => n.Situacao == Situacao.Processada);
                    break;
            }

            ViewData["Itens"] = new SelectList(_context.Item, "Id", "Nome");


            var notas = await query.ToListAsync();

            return View(notas);
        }

        // GET: NotasDeVenda/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notaDeVenda = await _context.NotaDeVenda
                .Include(n => n.Cliente)
                .Include(n => n.TipoDePagamento)
                .Include(n => n.Transportadora)
                .Include(n => n.Vendedor)
                 .Include(n => n.Itens)
            .ThenInclude(i => i.Produto) // Certifique-se de incluir o produto dos itens
        .FirstOrDefaultAsync(m => m.Id == id);
            if (notaDeVenda == null)
            {
                return NotFound();
            }


            return View(notaDeVenda);
        }

        // GET: NotasDeVenda/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Id");
            ViewData["TipoDePagamentoId"] = new SelectList(_context.TiposDePagamento.Select(tp => new SelectListItem
            {
                Value = tp.Id.ToString(),
                Text = tp.Discriminator // Use a propriedade apropriada para representar o tipo
            }), "Value", "Text"); ViewData["TransportadoraId"] = new SelectList(_context.Transportadora, "Id", "Id");
            ViewData["VendedorId"] = new SelectList(_context.Vendedor, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NotaDeVenda notaDeVenda, List<Item> itens)
        {
            if (ModelState.IsValid)
            {
                // Adiciona a nota de venda ao contexto
                _context.Add(notaDeVenda);

                // Itera sobre os itens da nota de venda e os adiciona ao contexto
                foreach (var itemViewModel in itens)
                {
                    var newItem = new Item
                    {
                        ProdutoId = itemViewModel.ProdutoId,
                        Quantidade = itemViewModel.Quantidade
                    };

                    // Adiciona o item à nota de venda
                    notaDeVenda.Itens.Add(newItem);

                    // Certifique-se de associar o item à nota de venda
                    newItem.NotaDeVendaId = notaDeVenda.Id;

                    // Atualiza a quantidade do produto (ou qualquer outra lógica que você precisar)
                    var produto = _context.Produto.Find(itemViewModel.ProdutoId);
                    if (produto != null)
                    {
                        produto.Quantidade -= itemViewModel.Quantidade;
                    }
                }

                // Salva as alterações no banco de dados
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Se houver algum erro de validação, preencha novamente as ViewBag e retorne a view
            ViewData["Produtos"] = new SelectList(_context.Produto, "Id", "Nome");
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Id", notaDeVenda.ClienteId);
            ViewData["TipoDePagamentoId"] = new SelectList(_context.TiposDePagamento, "Id", "Discriminator", notaDeVenda.TipoDePagamentoId);
            ViewData["TransportadoraId"] = new SelectList(_context.Transportadora, "Id", "Id", notaDeVenda.TransportadoraId);
            ViewData["VendedorId"] = new SelectList(_context.Vendedor, "Id", "Id", notaDeVenda.VendedorId);
            return View(notaDeVenda);
        }


        // GET: NotasDeVenda/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notaDeVenda = await _context.NotaDeVenda.FindAsync(id);
            if (notaDeVenda == null)
            {
                return NotFound();
            }

            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Id", notaDeVenda.ClienteId);
            ViewData["Itens"] = new SelectList(_context.Item, "Id", "Nome");
            ViewData["TipoDePagamentoId"] = new SelectList(_context.TiposDePagamento, "Id", "Discriminator", notaDeVenda.TipoDePagamentoId);
            ViewData["TransportadoraId"] = new SelectList(_context.Transportadora, "Id", "Id", notaDeVenda.TransportadoraId);
            ViewData["VendedorId"] = new SelectList(_context.Vendedor, "Id", "Id", notaDeVenda.VendedorId);
            return View(notaDeVenda);
        }

        // POST: NotasDeVenda/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Data,Situacao,DataCancelamento,DataDevolucao,ClienteId,VendedorId,TransportadoraId,TipoDePagamentoId")] NotaDeVenda notaDeVenda)
        {
            if (id != notaDeVenda.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(notaDeVenda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotaDeVendaExists(notaDeVenda.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["Produtos"] = new SelectList(_context.Produto, "Id", "Nome");
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Id", notaDeVenda.ClienteId);
            ViewData["TipoDePagamentoId"] = new SelectList(_context.TiposDePagamento, "Id", "Discriminator", notaDeVenda.TipoDePagamentoId);
            ViewData["TransportadoraId"] = new SelectList(_context.Transportadora, "Id", "Id", notaDeVenda.TransportadoraId);
            ViewData["VendedorId"] = new SelectList(_context.Vendedor, "Id", "Id", notaDeVenda.VendedorId);
            return View(notaDeVenda);
        }

        // GET: NotasDeVenda/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notaDeVenda = await _context.NotaDeVenda
                .Include(n => n.Cliente)
                .Include(n => n.TipoDePagamento)
                .Include(n => n.Transportadora)
                .Include(n => n.Vendedor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notaDeVenda == null)
            {
                return NotFound();
            }

            return View(notaDeVenda);
        }

        // POST: NotasDeVenda/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var notaDeVenda = await _context.NotaDeVenda.FindAsync(id);
            _context.NotaDeVenda.Remove(notaDeVenda);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: NotasDeVenda/Cancelar/5
        public async Task<IActionResult> Cancelar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notaDeVenda = await _context.NotaDeVenda.FindAsync(id);
            if (notaDeVenda == null)
            {
                return NotFound();
            }

            notaDeVenda.Cancelar();
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: NotasDeVenda/Devolver/5
        public async Task<IActionResult> Devolver(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notaDeVenda = await _context.NotaDeVenda.FindAsync(id);
            if (notaDeVenda == null)
            {
                return NotFound();
            }

            try
            {
                notaDeVenda.Devolver();
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                // Handle the exception (e.g., show an error message to the user)
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(nameof(Index), await _context.NotaDeVenda.ToListAsync());
            }
        }

        // GET: NotasDeVenda/Canceladas
        public async Task<IActionResult> Canceladas()
        {
            var notasCanceladas = await _context.NotaDeVenda
                .Where(n => n.Situacao == Situacao.Cancelada)
                .ToListAsync();

            return View("Index", notasCanceladas);
        }



        // GET: NotasDeVenda/Devolvidas
        public async Task<IActionResult> Devolvidas()
        {
            var notasDevolvidas = await _context.NotaDeVenda
                .Where(n => n.Situacao == Situacao.Devolvida)
                .ToListAsync();

            return View("Index", notasDevolvidas);
        }

        private bool NotaDeVendaExists(int id)
        {
            return _context.NotaDeVenda.Any(e => e.Id == id);
        }
    }
}
