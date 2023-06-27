using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace c153201_Hackathon_VITEC.Models
{
    public class Parcela
    {
        public int numero { get; set; }
        public decimal valorAmortizacao { get; set; }

        public decimal valorJuros { get; set; }

        public decimal valorPrestacao { get; set; }
    }
}
