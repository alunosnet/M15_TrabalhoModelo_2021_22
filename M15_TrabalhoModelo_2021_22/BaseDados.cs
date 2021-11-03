using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace M15_TrabalhoModelo_2021_22
{
    public class BaseDados
    {
        string BDName = "M15_TrabalhoModelo";
        string caminho;
        string strLigacao;
        SqlConnection ligacaoBD;

        public BaseDados()
        {
            strLigacao = ConfigurationManager.ConnectionStrings["servidor"].ToString();
            caminho = Utils.pastaDoPrograma() + @"\" + BDName + ".mdf";
            if (File.Exists(caminho) == false)
            {
                CriarBD();
            }
            ligacaoBD = new SqlConnection(strLigacao);
            ligacaoBD.Open();
            ligacaoBD.ChangeDatabase(BDName);
        }
        ~BaseDados()
        {
            try
            {
                ligacaoBD.Close();
            }
            catch { }
        }
        public SqlTransaction IniciarTransacao()
        {
            return ligacaoBD.BeginTransaction();
        }
        private void CriarBD()
        {
            ligacaoBD = new SqlConnection(strLigacao);
            ligacaoBD.Open();
            //criar bd
            string strSQL = $"CREATE DATABASE {BDName} ON PRIMARY (NAME={BDName},FILENAME='{caminho}')";
            executaSQL(strSQL);
            //criar as tabelas
            ligacaoBD.ChangeDatabase(BDName);
            strSQL = @"create table leitores(
	                        nleitor int identity primary key,
	                        nome varchar(40) not null,
	                        data_nasc date,
	                        fotografia varbinary(max),
	                        estado bit
                        )

                        create table livros(
	                        nlivro int identity primary key,
	                        nome varchar(100),
	                        ano int,
	                        data_aquisicao date,
	                        preco decimal(4,2),
	                        capa varchar(300),
	                        estado bit
                        )

                        create table emprestimos(
	                        nemprestimo int identity primary key,
	                        nlivro int references livros(nlivro),
	                        nleitor int references leitores(nleitor),
	                        data_emprestimo date,
	                        data_devolve date,
	                        estado bit
                        )";
            executaSQL(strSQL);
            ligacaoBD.Close();
        }

        public void executaSQL(string strSQL,List<SqlParameter> parametros=null,SqlTransaction transacao=null)
        {
            SqlCommand comando = new SqlCommand(strSQL, ligacaoBD);
            if (parametros != null)
                comando.Parameters.AddRange(parametros.ToArray());
            if (transacao != null)
                comando.Transaction = transacao;
            comando.ExecuteNonQuery();
            comando.Dispose();
        }
        public DataTable devolveSQL(string strSQL,List<SqlParameter> parametros=null)
        {
            SqlCommand comando = new SqlCommand(strSQL, ligacaoBD);
            if (parametros != null)
                comando.Parameters.AddRange(parametros.ToArray());
            DataTable dados = new DataTable();
            SqlDataReader registos = comando.ExecuteReader();
            dados.Load(registos);
            registos.Close();
            registos = null;
            comando.Dispose();

            return dados;
        }
    }
}
