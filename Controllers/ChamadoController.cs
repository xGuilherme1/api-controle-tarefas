using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiServico.Models.Dtos;
using ApiServico.Models;

namespace ApiServico.Controllers
{
    [Route("/chamados")]
    [ApiController]
    public class ChamadoController : ControllerBase
    {
        private static List<Chamado> _listaChamados = new List<Chamado>
        {
            new () { 
                Id = 1, Titulo = "Erro na tela de Acesso", Descricao = "O usuário não conseguiu logar." },
            new () { 
                Id = 2, Titulo = "Sistema com lentidão", Descricao = "Demora no carregamento de telas." }
        };

        private static int _proximoId = 3;

        [HttpGet("Mostrar")]
        public IActionResult BuscarTodos()
        {
            return Ok(_listaChamados);
        }

        [HttpGet("{id}/Buscar")]
        public IActionResult BuscarPorId(int id)
        {
            var chamado = _listaChamados.FirstOrDefault(x => x.Id == id);

            if (chamado == null)
            {
                return NotFound();
            }

            return Ok(chamado);
        }

        [HttpPost("Criar")]
        public IActionResult Criar([FromBody] ChamadoDto novoChamado)
        {
            var chamado = new Chamado() { Titulo = novoChamado.Titulo, Descricao = novoChamado.Descricao };
            chamado.Id = _proximoId++;
            chamado.Status = "Aberto";

            _listaChamados.Add(chamado);

            return Created("", novoChamado);
        }

        [HttpPut("{id}/Alterar")]
        public IActionResult Atualizar(int id, [FromBody] ChamadoDto novoChamado)
        {
            var chamado = _listaChamados.FirstOrDefault(item => item.Id == id);

            if (chamado == null)
            {
                return NotFound();
            }

            chamado.Titulo = novoChamado.Titulo;
            chamado.Descricao = novoChamado.Descricao;

            return Ok(chamado);
        }

        [HttpPost("{id}/finalizar)")]
        public IActionResult FinalizarChamado(int id) 
        {
            var chamado = _listaChamados.FirstOrDefault(item => item.Id == id);

            if (chamado == null)
            {
                return NotFound();
            }

            chamado.Status = "Fechado";
            chamado.DataFechamento = DateTime.Now;

            return Ok(chamado);
        }

        [HttpDelete("{id}/Deletar")]
        public IActionResult Remover(int id) 
        {
            var chamado = _listaChamados.FirstOrDefault(x => x.Id == id);

            if (chamado == null)
            {
                return NotFound();
            }

            _listaChamados.Remove(chamado);

            return NoContent();
        }
    }
}
