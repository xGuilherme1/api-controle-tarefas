using Microsoft.AspNetCore.Mvc;
using ApiServico.Models.Dtos;
using ApiServico.Models;
using ApiServico.DataContexts;
using Microsoft.EntityFrameworkCore;

namespace ApiServico.Controllers
{
    [Route("/chamados")]
    [ApiController]
    public class ChamadoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ChamadoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> BuscarTodos(
                [FromQuery] string? search,
                [FromQuery] string? situacao
            )
        {
            var query = _context.Chamados.AsQueryable();

            if(search is not null)
            {
                query = query.Where(x => x.Titulo.Contains(search));
            }

            if(situacao is not null)
            {
                query = query.Where(x => x.Status.Contains(situacao));
            }

            var chamados = await query.Include(p => p.Prioridade).ToListAsync();

            return Ok(chamados);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var chamado = await _context.Chamados.SingleOrDefaultAsync(x => x.Id == id);

            if (chamado == null)
            {
                return NotFound();
            }

            return Ok(chamado);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] ChamadoDto novoChamado)
        {
            var prioridade = await _context.Prioridades.FirstOrDefaultAsync(x => x.Id == novoChamado.PrioridadeId);

            if (prioridade is null)
            {
                return NotFound("Prioridade informada não encontrada");
            }

            var chamado = new Chamado() { 
                Titulo = novoChamado.Titulo, 
                Descricao = novoChamado.Descricao,
                PrioridadeId = novoChamado.PrioridadeId
            };

            await _context.Chamados.AddAsync(chamado);
            await _context.SaveChangesAsync();

            return Created("", chamado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] ChamadoDto atualizacaoChamado)
        {
            var chamado = await _context.Chamados.SingleOrDefaultAsync(x => x.Id == id);

            if (chamado == null)
            {
                return NotFound();
            }

            chamado.Titulo = atualizacaoChamado.Titulo;
            chamado.Descricao = atualizacaoChamado.Descricao;

            _context.Chamados.Update(chamado);
            await _context.SaveChangesAsync();

            return Ok(chamado);
        }

        [HttpPost("{id}/Finalizar")]
        public async Task<IActionResult> FinalizarChamado(int id)
        {
            var chamado = await _context.Chamados.SingleOrDefaultAsync(x => x.Id == id);

            if (chamado == null)
            {
                return NotFound();
            }

            chamado.Status = "Fechado";
            chamado.DataFechamento = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(chamado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(int id) 
        {
            var chamado = await _context.Chamados.SingleOrDefaultAsync(x => x.Id == id);

            if (chamado == null)
            {
                return NotFound();
            }

            _context.Chamados.Remove(chamado);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
