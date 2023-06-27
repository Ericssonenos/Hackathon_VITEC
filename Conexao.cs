using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace c153201_Hackathon_VITEC
{
    public class Conexao
    {
        public SqlCommand Conectar()
        {

            // Conexões
            string Local =
                     @"dbhackathon.database.windows.net; 
                                            Initial Catalog = hack; 
                                            Integrated Security=SSPI;
                                            Min Pool Size=5;
                                            Max Pool Size=250;
                                            Connect Timeout=3";
            string Caixa =
                     @"data source = dbhackathon.database.windows.net, 1433; " +
                     "initial catalog = hack; " +
                     "user id = hack; " +
                     "pwd = Password23; ";
    
           
            //Gerar conexão
            var Conexao = new SqlConnection(Caixa);

            if (Conexao.State == System.Data.ConnectionState.Open)
                Conexao.Dispose();

            Conexao.Open();

            return new SqlCommand() { Connection = Conexao, CommandTimeout = 0 };
        }
    }
}
