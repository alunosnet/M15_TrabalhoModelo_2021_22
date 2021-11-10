using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
        public C_Emprestimo(int nleitor, int nlivro,DateTime dataemp)
        {
            this.nleitor = nleitor;
            this.nlivro = nlivro;
            this.estado = true;
            this.data_emprestimo =dataemp;
            this.data_devolve = data_emprestimo.AddDays(10);
        }
        public C_Emprestimo(int nemprestimo, int nleitor, string nomeLeitor, int nlivro, string nomeLivro, DateTime data_emprestimo, DateTime data_devolve, bool estado)
        {
            this.nemprestimo = nemprestimo;
            this.nomeLeitor = nomeLeitor;
            this.nlivro = nlivro;
            this.nleitor = nleitor;
            this.nomeLivro = nomeLivro;
            this.data_emprestimo = data_emprestimo;
            this.data_devolve = data_devolve;
            this.estado = estado;
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

        internal void Receber(BaseDados bd)
        {
            //iniciar transação
            SqlTransaction transacao = bd.IniciarTransacao();
            string sql = $@"UPDATE Emprestimos
                                SET data_devolve=@data_devolve,
                                    estado=@estado
                            WHERE nemprestimo=@nemprestimo";
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName="@data_devolve",
                    SqlDbType=System.Data.SqlDbType.Date,
                    Value=DateTime.Now
                },
                new SqlParameter()
                {
                    ParameterName="@estado",
                    SqlDbType=System.Data.SqlDbType.Bit,
                    Value=false
                },
                new SqlParameter()
                {
                    ParameterName="@nemprestimo",
                    SqlDbType=System.Data.SqlDbType.Int,
                    Value=this.nemprestimo
                }
            };
            //registar o empréstimo
            bd.executaSQL(sql, parametros, transacao);
            //alterar o estado do livro
            sql = $"UPDATE Livros SET estado=1 WHERE nlivro={this.nlivro}";
            bd.executaSQL(sql, null, transacao);
            //commit
            transacao.Commit();
        }

        internal static IEnumerable ListaEmprestimosPorConcluir(BaseDados bd)
        {
            string sql = $@"SELECT Emprestimos.*,
                    leitores.nome as [NomeLeitor],
                    livros.nome as [NomeLivro]
                    FROM Emprestimos 
                    INNER JOIN Leitores ON emprestimos.nleitor=Leitores.nleitor
                    INNER JOIN Livros ON emprestimos.nlivro=Livros.nlivro
                    WHERE emprestimos.estado=1";
            var dados = bd.devolveSQL(sql);
            List<C_Emprestimo> lista = new List<C_Emprestimo>();
            foreach(DataRow linha in dados.Rows)
            {
                int nemprestimo = int.Parse(linha["nemprestimo"].ToString());
                int nleitor = int.Parse(linha["nleitor"].ToString());
                int nlivro = int.Parse(linha["nlivro"].ToString());
                DateTime data_emp = DateTime.Parse(linha["data_emprestimo"].ToString());
                DateTime data_devolve = DateTime.Parse(linha["data_devolve"].ToString());
                bool estado = bool.Parse(linha["estado"].ToString());
                string nomeleitor = linha["NomeLeitor"].ToString();
                string nomelivro = linha["NomeLivro"].ToString();
                C_Emprestimo emp = new C_Emprestimo(nemprestimo, nleitor, nomeleitor, 
                    nlivro, nomelivro, data_emp, data_devolve, estado);
                lista.Add(emp);
            }
            return lista;
        }
        internal static IEnumerable ListaEmprestimosTodos(BaseDados bd)
        {
            string sql = $@"SELECT Emprestimos.*,
                    leitores.nome as [NomeLeitor],
                    livros.nome as [NomeLivro]
                    FROM Emprestimos 
                    INNER JOIN Leitores ON emprestimos.nleitor=Leitores.nleitor
                    INNER JOIN Livros ON emprestimos.nlivro=Livros.nlivro";
            var dados = bd.devolveSQL(sql);
            List<C_Emprestimo> lista = new List<C_Emprestimo>();
            foreach (DataRow linha in dados.Rows)
            {
                int nemprestimo = int.Parse(linha["nemprestimo"].ToString());
                int nleitor = int.Parse(linha["nleitor"].ToString());
                int nlivro = int.Parse(linha["nlivro"].ToString());
                DateTime data_emp = DateTime.Parse(linha["data_emprestimo"].ToString());
                DateTime data_devolve = DateTime.Parse(linha["data_devolve"].ToString());
                bool estado = bool.Parse(linha["estado"].ToString());
                string nomeleitor = linha["NomeLeitor"].ToString();
                string nomelivro = linha["NomeLivro"].ToString();
                C_Emprestimo emp = new C_Emprestimo(nemprestimo, nleitor, nomeleitor,
                    nlivro, nomelivro, data_emp, data_devolve, estado);
                lista.Add(emp);
            }
            return lista;
        }
    }
}
