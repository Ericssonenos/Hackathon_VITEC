using c153201_Hackathon_VITEC.Application;
using c153201_Hackathon_VITEC.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace c153201_Hackathon_VITEC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimulacaoController : ControllerBase
    {
        // GET: api/<SimulacaoController>
        [HttpGet]
        public string Get()
        {
            return "Utilize o metodo Post para reseber um envelope JSON contendo  "
                    + " os parametros: valorDesejado e prazo em formato json. ";
        }

        // POST api/<SimulacaoController>
        [HttpPost]
        
        public async Task<RetornoSimulacao> Simular([FromBody] ParametroSimulacao parametro)
        {
            if (parametro.prazo == 0)
            {
                return new RetornoSimulacao
                {
                    codigoProduto = 0,
                    descricaoProduto = "Falta o parametro prazo",
                    taxaJuros = 0
                };
            }else if (parametro.valorDesejado ==0)
            {
                return new RetornoSimulacao
                {
                    codigoProduto = 0,
                    descricaoProduto = "Falta o parametro valorDesejado",
                    taxaJuros = 0
                };
            }
               

            RetornoSimulacao resultado = new AppSimulacao(parametro).Simular();
            //gravando este mesmo envelope JSON no Eventhub
            await AppEventHub.SendingJsonToEventHubAsync(resultado);

            return resultado;
        }

        
    }
}
