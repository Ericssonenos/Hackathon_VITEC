using c153201_Hackathon_VITEC.Models;
using System;
using System.Collections.Generic;
using c153201_Hackathon_VITEC.Application;
using System.Linq;
using System.Threading.Tasks;

namespace c153201_Hackathon_VITEC.Application
{
    public class AppSimulacao
    {
        decimal _valorDesejado;
        int _prazo;
        RetornoSimulacao retornoSimulacao = new RetornoSimulacao();

        public AppSimulacao(ParametroSimulacao parametro)
        {
          
            _valorDesejado = (decimal)parametro.valorDesejado;
            _prazo = parametro.prazo;
        }

        public RetornoSimulacao Simular()
        {

            // Consultar um conjunto de informações parametrizadas em uma tabela de banco de 
            //dados SQL Server
            GetDados();

            //simulação utilizando dois sistemas de amortização(SAC e Price) e
            GetSimulacao();

            return retornoSimulacao;
        }

        private void GetDados()
        {
            // iniciar conexão
            var comando = new Conexao().Conectar();

            //passar parametros para o SQL de forma segura
            comando.Parameters.AddWithValue("@valorDesejado", _valorDesejado);
            comando.Parameters.AddWithValue("@prazo", _prazo);

            // Gerar script de solicitação ao BD
            //Filtrar qual produto se adequa aos parâmetros de entrada;
            comando.CommandText = ""
                + "  SELECT "
                + "  CO_PRODUTO "
                + ", NO_PRODUTO "
                + ", PC_TAXA_JUROS "
                + "  FROM dbo.PRODUTO"
                + "  WHERE "
                + "  @prazo "
                + "  BETWEEN "
                + "  NU_MINIMO_MESES AND NU_MAXIMO_MESES "
                + "  AND "
                + "  @valorDesejado "
                + "  BETWEEN "
                + "  VR_MINIMO AND VR_MAXIMO ";

            // Executar comando
            var executar = comando.ExecuteReader();

            // traspor resultado para a classe retornoSimulacao
            executar.Read();
            try
            {
                retornoSimulacao.codigoProduto = int.Parse(executar["CO_PRODUTO"].ToString());
                retornoSimulacao.descricaoProduto = executar["NO_PRODUTO"].ToString();
                retornoSimulacao.taxaJuros = decimal.Parse(executar["PC_TAXA_JUROS"].ToString());
            }
            catch
            {
                retornoSimulacao.codigoProduto = 0;
                retornoSimulacao.descricaoProduto = "Null";
                retornoSimulacao.taxaJuros = 0;
            }


            // Fechar conexão
            comando.Connection.Close();

        }

        private void GetSimulacao()
        {
            retornoSimulacao.resultadoSimulacaos = new List<resultadoSimulacao>();
            // SAC
            decimal _valorRestante = _valorDesejado;
            decimal _valorAmortizacao = _valorDesejado / _prazo;
            decimal _valorJuros = 0;

            List<Parcela> _parcelasSAC = new List<Parcela>();

            for (int _numero = 1; _numero <= _prazo; _numero++)
            {
                _valorJuros = _valorRestante * retornoSimulacao.taxaJuros;

                _parcelasSAC.Add(new Parcela()
                {
                    numero = _numero,
                    valorAmortizacao = Math.Round(_valorAmortizacao),
                    valorJuros = Math.Round(_valorJuros,2),
                    valorPrestacao = Math.Round(_valorJuros + _valorAmortizacao,2)
                });
                _valorRestante -= _valorAmortizacao;
            }
           
            retornoSimulacao.resultadoSimulacaos.Add(new resultadoSimulacao()
            {
                tipo = "SAC",
                parcelas = _parcelasSAC
            }); 
            

            // PRICE
            _valorRestante = _valorDesejado;
            decimal _valorPrestacao = ValorPrestacao(_valorDesejado);
            List<Parcela> _parcelasPRICE = new List<Parcela>();

            for (int _numero = 1; _numero <= _prazo; _numero++)
            {
                _valorJuros = Math.Round(_valorRestante * retornoSimulacao.taxaJuros,2);
                _valorAmortizacao = _valorPrestacao - _valorJuros;
                _parcelasPRICE.Add(new Parcela()
                {
                    numero = _numero,
                    valorAmortizacao = Math.Round(_valorPrestacao - _valorJuros,2),
                    valorJuros = Math.Round(_valorJuros,2),
                    valorPrestacao = Math.Round(_valorPrestacao,2)
                }); ;
                _valorRestante = _valorRestante - _valorAmortizacao;
            }

            retornoSimulacao.resultadoSimulacaos.Add(new resultadoSimulacao()
            {
                tipo = "PRICE",
                parcelas = _parcelasPRICE
            }); ;

        }

        private decimal ValorPrestacao(decimal _valorDesejado)
        {
            // valores para teste, comparando com os valores do exemplo de 
            //https://calculojuridico.com.br/calculadora-price-sac/
            // _valorDesejado = 30000;
            //retornoSimulacao.taxaJuros = 0.02M;
            // _prazo = 24;
            int potenciaNegativa = _prazo * -1;
            double divisorPotecia = Math.Pow((double)(1 + retornoSimulacao.taxaJuros), potenciaNegativa);
            decimal dividendo = _valorDesejado * retornoSimulacao.taxaJuros;
            decimal divisor =(decimal) (1- divisorPotecia);

            return  Math.Round((dividendo / divisor),2);

            
        }
    }
}
