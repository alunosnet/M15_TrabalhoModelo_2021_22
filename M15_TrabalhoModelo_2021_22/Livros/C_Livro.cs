using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace M15_TrabalhoModelo_2021_22.Livros
{
    public class C_Livro
    {
        public int nlivro { get; set; }
        public string nome { get; set; }
        public int ano { get; set; }
        public DateTime data_aquisicao { get; set; }
        public decimal preco { get; set; }
        public string capa { get; set; }
        public bool estado { get; set; }

        public C_Livro(int nlivro, string nome, int ano, DateTime data_aquisicao, decimal preco, string capa, bool estado)
        {
            this.nlivro = nlivro;
            this.nome = nome;
            this.ano = ano;
            this.data_aquisicao = data_aquisicao;
            this.preco = preco;
            this.capa = capa;
            this.estado = estado;
        }
        public void Adicionar(BaseDados bd)
        {
            string sql = $@"INSERT INTO Livros(nome,ano,
                    data_aquisicao,preco,capa,estado) VALUES
                (@nome,@ano,@data_aquisicao,@preco,@capa,@estado)";
            //parametros
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
                    ParameterName="@ano",
                    SqlDbType=System.Data.SqlDbType.Int,
                    Value=this.ano
                },
                new SqlParameter()
                {
                    ParameterName="@data_aquisicao",
                    SqlDbType=System.Data.SqlDbType.Date,
                    Value=this.data_aquisicao
                },
                new SqlParameter()
                {
                    ParameterName="@preco",
                    SqlDbType=System.Data.SqlDbType.Decimal,
                    Value=this.preco
                },
                new SqlParameter()
                {
                    ParameterName="@capa",
                    SqlDbType=System.Data.SqlDbType.VarChar,
                    Value=this.capa
                },
                new SqlParameter()
                {
                    ParameterName="@estado",
                    SqlDbType=System.Data.SqlDbType.Bit,
                    Value=true
                },
            };
            //executar
            bd.executaSQL(sql, parametros);
        }
        //remover
        public static void Remover(BaseDados bd, int nlivro)
        {
            string sql = "DELETE FROM Livros WHERE nlivro=" + nlivro;
            bd.executaSQL(sql);
        }
        //atualizar
        public void Atualizar(BaseDados bd)
        {
            string sql = $@"UPDATE Livros
                            SET nome=@nome,ano=@ano,
                                data_aquisicao=@data,preco=@preco,
                                capa=@capa,estado=@estado
                            WHERE nlivro=@nlivro";
            //parametros
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
                    ParameterName="@ano",
                    SqlDbType=System.Data.SqlDbType.Int,
                    Value=this.ano
                },
                new SqlParameter()
                {
                    ParameterName="@data",
                    SqlDbType=System.Data.SqlDbType.Date,
                    Value=this.data_aquisicao
                },
                new SqlParameter()
                {
                    ParameterName="@preco",
                    SqlDbType=System.Data.SqlDbType.Decimal,
                    Value=this.preco
                },
                new SqlParameter()
                {
                    ParameterName="@capa",
                    SqlDbType=System.Data.SqlDbType.VarChar,
                    Value=this.capa
                },
                new SqlParameter()
                {
                    ParameterName="@estado",
                    SqlDbType=System.Data.SqlDbType.Bit,
                    Value=true
                },
                new SqlParameter()
                {
                    ParameterName="@nlivro",
                    SqlDbType=System.Data.SqlDbType.Int,
                    Value=this.nlivro
                },
            };
            //executar
            bd.executaSQL(sql, parametros);
        }
        //listar todos
        public static List<C_Livro> ListarTodos(BaseDados bd,bool Disponiveis=false)
        {
            List<C_Livro> lista = new List<C_Livro>();
            string sql = "SELECT * FROM Livros ORDER BY Nome";
            if(Disponiveis==true)
                sql = "SELECT * FROM Livros WHERE Estado=1 ORDER BY Nome";
            var dados = bd.devolveSQL(sql);
            foreach (DataRow linha in dados.Rows)
            {
                int nlivro = int.Parse(linha["nlivro"].ToString());
                string nome = linha["nome"].ToString();
                int ano = int.Parse(linha["ano"].ToString());
                DateTime data = DateTime.Parse(linha["data_aquisicao"].ToString());
                decimal preco = decimal.Parse(linha["preco"].ToString());
                string capa = linha["capa"].ToString();
                bool estado = bool.Parse(linha["estado"].ToString());
                C_Livro novo = new C_Livro(nlivro, nome,ano,data,preco,capa,estado);
                lista.Add(novo);
            }
            return lista;
        }
        //pesquisar
        public static List<C_Livro> PesquisarPorNome(BaseDados bd, string nomePesquisar)
        {
            List<C_Livro> lista = new List<C_Livro>();
            string sql = @"SELECT * FROM Livros WHERE nome 
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
                int nlivro = int.Parse(linha["nlivro"].ToString());
                string nome = linha["nome"].ToString();
                int ano = int.Parse(linha["ano"].ToString());
                DateTime data = DateTime.Parse(linha["data_aquisicao"].ToString());
                decimal preco = decimal.Parse(linha["preco"].ToString());
                string capa = linha["capa"].ToString();
                bool estado = bool.Parse(linha["estado"].ToString());
                C_Livro novo = new C_Livro(nlivro, nome, ano, data, preco, capa, estado);
                lista.Add(novo);
            }

            return lista;
        }

        //listar paginado
        public static List<C_Livro> ListarTodos(BaseDados bd, int primeiro,
            int ultimo)
        {
            List<C_Livro> lista = new List<C_Livro>();
            string sql = $@"SELECT nlivro,nome,ano,data_aquisicao,preco,capa,estado
                            FROM (SELECT row_number() over (order by nome) as num,
                                nlivro,nome,ano,data_aquisicao,preco,capa,estado
                                FROM Livros) as p
                            WHERE num>={primeiro} AND num<={ultimo}";
            var dados = bd.devolveSQL(sql);
            foreach (DataRow linha in dados.Rows)
            {
                int nlivro = int.Parse(linha["nlivro"].ToString());
                string nome = linha["nome"].ToString();
                int ano = int.Parse(linha["ano"].ToString());
                DateTime data = DateTime.Parse(linha["data_aquisicao"].ToString());
                decimal preco = decimal.Parse(linha["preco"].ToString());
                string capa = linha["capa"].ToString();
                bool estado = bool.Parse(linha["estado"].ToString());
                C_Livro novo = new C_Livro(nlivro, nome, ano, data, preco, capa, estado);
                lista.Add(novo);
            }
            return lista;
        }
        //nr de leitores
        public static int NrLivros(BaseDados bd)
        {
            DataTable dados = bd.devolveSQL("Select count(*) FROM Livros");
            int nr = int.Parse(dados.Rows[0][0].ToString());
            return nr;
        }
        public override string ToString()
        {
            return this.nome;
        }
    }
}
