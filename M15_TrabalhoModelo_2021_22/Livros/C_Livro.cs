using System;
using System.Collections.Generic;
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
    }
}
