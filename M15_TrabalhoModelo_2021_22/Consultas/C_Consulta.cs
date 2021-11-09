using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace M15_TrabalhoModelo_2021_22.Consultas
{
    public class C_Consulta
    {
        public static DataTable ExecutaConsulta(BaseDados bd,int i)
        {
            string sql = "";
            switch (i)
            {
                case 0:
                    sql = "Select * FROM Leitores";
                    break;
                /*1 - Nº de empréstimos por leitor (mostrar o nome de todos leitores)*/
                /*2 - Nº de empréstimos por mês*/
                /*3 - Média de duração dos empréstimos em dias*/
                /*4 - Top 3 dos livros mais emprestados (mostrar o nome)*/
                /*5 - Livros nunca emprestados*/
                /*6 - Lista dos livros cujo preço é superior à média*/
                /*7 - Lista dos leitores que nasceram na década de 1970*/
                /*8 - Top 3 dos livros que estiveram mais tempo emprestados*/
                /*9 - Top 3 dos livros mais emprestados no mês anterior*/
                /*10 - Lista dos livros cujo empréstimo está fora do prazo*/
                /*11 - Lista dos livros emprestados nos últimos 7 dias*/
                /*12 - Top 5 dos últimos livros adquiridos*/

            }
            return bd.devolveSQL(sql);
        }
    }
}
