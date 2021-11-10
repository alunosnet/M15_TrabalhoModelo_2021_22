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
                case 1:
                    /*1 - Nº de empréstimos por leitor (mostrar o nome de todos leitores)*/
                    sql = @"Select nome,count(nemprestimo) as [Nr Emp] FROM Leitores 
                            left join emprestimos on leitores.nleitor=emprestimos.nleitor
                            group by nome,leitores.nleitor";
                    break;
                case 2:
                    /*2 - Nº de empréstimos por mês*/
                    sql = @"Select month(emprestimos.data_emprestimo) as [Mês],count(nemprestimo) as [Nr Emp] FROM Emprestimos 
                            group by month(emprestimos.data_emprestimo)";
                    break;
                case 3:
                    /*3 - Média de duração dos empréstimos em dias*/
                    sql = @"Select avg(datediff(day,emprestimos.data_emprestimo,emprestimos.data_devolve)) as [Duração Média] FROM Emprestimos";
                    break;
                case 4:
                    /*4 - Top 3 dos livros mais emprestados (mostrar o nome)*/
                    sql = @"Select nome,count(nemprestimo) as [Nr Emp] FROM Livros 
                            left join emprestimos on Livros.nlivro=emprestimos.nlivro
                            group by nome,livros.nlivro
                            ORDER BY [Nr Emp]";
                    break;
                case 5:
                    /*5 - Livros nunca emprestados*/
                    sql = @"Select nome FROM Livros 
                            WHERE nlivro not in (SELECT nlivro from Emprestimos)
                            ORDER BY [nome]";
                    break;
                case 6:
                    /*6 - Lista dos livros cujo preço é superior à média*/
                    sql = @"Select nome FROM Livros 
                            WHERE preco>(SELECT avg(preco) from livros)
                            ORDER BY [preco]";
                    break;
                case 7:
                    /*7 - Lista dos leitores que nasceram na década de 1970*/
                    sql = @"Select nome FROM leitores 
                            WHERE year(data_nasc)>=1970 and year(data_nasc)<1980";
                    break;
                case 8:
                    /*8 - Top 3 dos livros que estiveram mais tempo emprestados*/
                    sql = @"Select TOP 3 nome FROM Livros 
                            INNER JOIN Emprestimos ON livros.nlivro=emprestimos.nlivro
                            GROUP BY Livros.nome,Livros.nlivro
                            ORDER BY sum(datediff(day,data_emprestimo,data_devolve)) DESC";
                    break;
                case 9:
                    /*9 - Top 3 dos livros mais emprestados no mês anterior*/
                    sql = @"Select TOP 3 nome FROM Livros 
                            INNER JOIN Emprestimos ON livros.nlivro=emprestimos.nlivro
                            WHERE datediff(month,Emprestimos.data_emprestimo,getdate())=1
                            GROUP BY Livros.nome,Livros.nlivro
                            ORDER BY count(*) DESC";
                    break;
                case 10:
                    /*10 - Lista dos livros cujo empréstimo está fora do prazo*/
                    sql = @"Select nome FROM Livros 
                            INNER JOIN Emprestimos ON livros.nlivro=emprestimos.nlivro
                            WHERE emprestimos.estado=1 and emprestimos.data_devolve<getdate()";
                    break;
                case 11:
                    /*11 - Lista dos livros emprestados nos últimos 7 dias*/
                    sql = @"Select TOP 3 nome FROM Livros 
                            INNER JOIN Emprestimos ON livros.nlivro=emprestimos.nlivro
                            WHERE datediff(day,Emprestimos.data_emprestimo,getdate())<=7";
                    break;
                case 12:
                    /*12 - Top 5 dos últimos livros adquiridos*/
                    sql = @"Select TOP 5 nome FROM Livros 
                            ORDER BY Data_aquisicao DESC";
                    break;
            }
            return bd.devolveSQL(sql);
        }
    }
}
