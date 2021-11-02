using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace M15_TrabalhoModelo_2021_22.Leitores
{
    public class C_Leitor
    {
        public int nleitor { get; set; }
        public string nome { get; set; }
        public DateTime data_nascimento { get; set; }
        public byte[] fotografia { get; set; }
        public bool estado { get; set; }

        public C_Leitor(int nleitor, string nome, DateTime data_nascimento, byte[] fotografia, bool estado)
        {
            this.nleitor = nleitor;
            this.nome = nome;
            this.data_nascimento = data_nascimento;
            this.fotografia = fotografia;
            this.estado = estado;
        }
        public void Adicionar(BaseDados bd)
        {
            string sql = $@"INSERT INTO Leitores(nome,data_nasc,fotografia,estado)
                            VALUES (@nome,@data_nasc,@fotografia,@estado)";
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName="@nome",
                    SqlDbType=System.Data.SqlDbType.VarChar,
                    Value=this.nome
                },
                new SqlParameter()
                {
                    ParameterName="@data_nasc",
                    SqlDbType=System.Data.SqlDbType.Date,
                    Value=this.data_nascimento
                },
                new SqlParameter()
                {
                    ParameterName="@fotografia",
                    SqlDbType=System.Data.SqlDbType.VarBinary,
                    Value=this.fotografia
                },
                new SqlParameter()
                {
                    ParameterName="@estado",
                    SqlDbType=System.Data.SqlDbType.Bit,
                    Value=true
                },
            };
            bd.executaSQL(sql, parametros);
        }
    
        //remover
        public static void Remover(BaseDados bd,int nleitor)
        {
            string sql = "DELETE FROM Leitores WHERE nleitor=" + nleitor;
            bd.executaSQL(sql);
        }
        //atualizar
        public void Atualizar(BaseDados bd)
        {
            string sql = $@"UPDATE Leitores
                            SET nome=@nome, data_nasc=@data_nasc,
                                fotografia=@fotografia
                            WHERE nleitor=@nleitor";
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName="@nome",
                    SqlDbType=System.Data.SqlDbType.VarChar,
                    Value=this.nome
                },
                new SqlParameter()
                {
                    ParameterName="@data_nasc",
                    SqlDbType=System.Data.SqlDbType.Date,
                    Value=this.data_nascimento
                },
                new SqlParameter()
                {
                    ParameterName="@fotografia",
                    SqlDbType=System.Data.SqlDbType.VarBinary,
                    Value=this.fotografia
                },
                new SqlParameter()
                {
                    ParameterName="@nleitor",
                    SqlDbType=System.Data.SqlDbType.Int,
                    Value=this.nleitor
                },
            };
            bd.executaSQL(sql, parametros);
        }
        //listar todos
        public static List<C_Leitor> ListarTodos(BaseDados bd)
        {
            List<C_Leitor> lista = new List<C_Leitor>();
            string sql = "SELECT * FROM Leitores ORDER BY Nome";
            var dados = bd.devolveSQL(sql);
            foreach(DataRow linha in dados.Rows)
            {
                int nleitor = int.Parse(linha["nleitor"].ToString());
                string nome = linha["nome"].ToString();
                DateTime data = DateTime.Parse(linha["data_nasc"].ToString());
                byte[] fotografia = (byte[])linha["fotografia"];
                bool estado = bool.Parse(linha["estado"].ToString());
                C_Leitor novo = new C_Leitor(nleitor, nome, data, fotografia, estado);
                lista.Add(novo);
            }
            return lista;
        }
        //pesquisar
        public static List<C_Leitor> PesquisarPorNome(BaseDados bd,string nomePesquisar)
        {
            List<C_Leitor> lista = new List<C_Leitor>();
            string sql = @"SELECT * FROM Leitores WHERE nome 
                        LIKE @nomePesquisar ORDER BY Nome";
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName = "@nomePesquisar",
                    SqlDbType=SqlDbType.VarChar,
                    Value="%"+nomePesquisar+"%"
                }
            };
            var dados = bd.devolveSQL(sql,parametros);
            foreach (DataRow linha in dados.Rows)
            {
                int nleitor = int.Parse(linha["nleitor"].ToString());
                string nome = linha["nome"].ToString();
                DateTime data = DateTime.Parse(linha["data_nasc"].ToString());
                byte[] fotografia = (byte[])linha["fotografia"];
                bool estado = bool.Parse(linha["estado"].ToString());
                C_Leitor novo = new C_Leitor(nleitor, nome, data, fotografia, estado);
                lista.Add(novo);
            }
            return lista;
        }
        //listar paginado
        public static List<C_Leitor> ListarTodos(BaseDados bd,int primeiro,
            int ultimo)
        {
            List<C_Leitor> lista = new List<C_Leitor>();
            string sql = $@"SELECT nleitor,nome,data_nasc,fotografia,estado
                            FROM (SELECT row_number() over (order by nome) as num,
                                nleitor,nome,data_nasc,fotografia,estado
                                FROM Leitores) as p
                            WHERE num>={primeiro} AND num<={ultimo}";
            var dados = bd.devolveSQL(sql);
            foreach (DataRow linha in dados.Rows)
            {
                int nleitor = int.Parse(linha["nleitor"].ToString());
                string nome = linha["nome"].ToString();
                DateTime data = DateTime.Parse(linha["data_nasc"].ToString());
                byte[] fotografia = (byte[])linha["fotografia"];
                bool estado = bool.Parse(linha["estado"].ToString());
                C_Leitor novo = new C_Leitor(nleitor, nome, data, fotografia, estado);
                lista.Add(novo);
            }
            return lista;
        }
        //nr de leitores
        public static int NrLeitores(BaseDados bd)
        {
            DataTable dados = bd.devolveSQL("Select count(*) FROM Leitores");
            int nr = int.Parse(dados.Rows[0][0].ToString());
            return nr;
        }
    }
}
