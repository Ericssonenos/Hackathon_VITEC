using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace c153201_Hackathon_VITEC.Models
{
    public class RetornoSimulacao
    {
        public int codigoProduto { get; set; }
        public string descricaoProduto { get; set; }

        public decimal taxaJuros { get; set; }

        public List<resultadoSimulacao> resultadoSimulacaos { get; set; }
    }
}
