using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace M15_TrabalhoModelo_2021_22.Leitores
{
    public class C_Leitor
    {
        public int nleitor;
        public string nome;
        public DateTime data_nascimento;
        public byte[] fotografia;
        public bool estado;

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
    }
}
