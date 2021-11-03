using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace M15_TrabalhoModelo_2021_22.Emprestimos
{
    public class C_Emprestimo
    {
        public int nemprestimo { get; set; }
        public int nleitor { get; set; }
        public string nomeLeitor { get; set; }
        public int nlivro { get; set; }
        public string nomeLivro { get; set; }
        public DateTime data_emprestimo { get; set; }
        public DateTime data_devolve { get; set; }
        public bool estado { get; set; }

        public C_Emprestimo(int nleitor, int nlivro)
        {
            this.nleitor = nleitor;
            this.nlivro = nlivro;
            this.estado = true;
            this.data_emprestimo = DateTime.Now;
            this.data_devolve = data_emprestimo.AddDays(10);
        }

        public void Adicionar(BaseDados bd)
        {
            //iniciar transação
            SqlTransaction transacao = bd.IniciarTransacao();
            string sql = $@"INSERT INTO Emprestimos(nlivro,nleitor,data_emprestimo,data_devolve,estado)
                            VALUES (@nlivro,@nleitor,@data_emprestimo,@data_devolve,@estado)";
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName="@nlivro",
                    SqlDbType=System.Data.SqlDbType.Int,
                    Value=this.nlivro
                },
                new SqlParameter()
                {
                    ParameterName="@nleitor",
                    SqlDbType=System.Data.SqlDbType.Int,
                    Value=this.nleitor
                },
                new SqlParameter()
                {
                    ParameterName="@data_emprestimo",
                    SqlDbType=System.Data.SqlDbType.Date,
                    Value=this.data_emprestimo
                },
                new SqlParameter()
                {
                    ParameterName="@data_devolve",
                    SqlDbType=System.Data.SqlDbType.Date,
                    Value=this.data_devolve
                },
                new SqlParameter()
                {
                    ParameterName="@estado",
                    SqlDbType=System.Data.SqlDbType.Bit,
                    Value=this.estado
                }
            };
            //registar o empréstimo
            bd.executaSQL(sql, parametros, transacao);
            //alterar o estado do livro
            sql = $"UPDATE Livros SET estado=0 WHERE nlivro={this.nlivro}";
            bd.executaSQL(sql, null, transacao);
            //commit
            transacao.Commit();
        }
    }
}
