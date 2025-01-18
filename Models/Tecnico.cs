using System.ComponentModel.DataAnnotations;

namespace RegistroTecnicos.Models
{
    public class Tecnico
    {
        public int TecnicoId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El sueldo por hora es obligatorio.")]
        [Range(1, double.MaxValue, ErrorMessage = "El sueldo debe ser mayor a 0.")]
        public decimal SueldoHora { get; set; }
    }
}
