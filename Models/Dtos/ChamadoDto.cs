using System.ComponentModel.DataAnnotations;

namespace ApiServico.Models.Dtos
{
    public class ChamadoDto
    {
        [Required(ErrorMessage = "O titulo é obrigatorio!")]
        //[MinLength(10)]
        [Length(10, 100, ErrorMessage = "O titulo deve ter no minimo 10 e no máximo 100 caracteres.")]
        public required string Titulo { get; set; }

        public required string Descricao { get; set; }

        public int PrioridadeId { get; set; }
    }
}
