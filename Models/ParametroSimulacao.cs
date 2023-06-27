using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace c153201_Hackathon_VITEC.Models
{
    public class ParametroSimulacao
    {
        [Required(ErrorMessage = "parametro valorDesejado Obrigatorio") ]
        public decimal valorDesejado { get; set; }

        [Required(ErrorMessage = "parametro prazo Obrigatorio")]
        public int prazo { get; set; }
    }
}
