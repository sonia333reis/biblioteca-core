using biblioteca.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace biblioteca.Repositories
{
    public class Connection : IDisposable
    {
        public MySqlConnection _con;

        public  Connection(string connectionString)
        {
            _con = new MySqlConnection(connectionString);
            _con.Open();
        }

        public void Open()
        {
            _con.Open();
        }

        public void Dispose()
        {
            _con.Close();
        }

        public List<string> SelectAllSimpleObjects (string objectValue) {

            //Está sendo feito um while/for para adicionar os valores do objetos na lista de string
            List<string> result = new List<string>();

            var cmd = _con.CreateCommand() as MySqlCommand;
            //precisa colocar no select os campos que precisam aparecer
            cmd.CommandText = @"select * from " + objectValue.ToLower() + "s";
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.FieldCount < 0) {
                result = null;
            }

            int counter = reader.VisibleFieldCount;

            while (reader.Read())
            {
                
                if (objectValue == "User") { 
                    counter -= 1;
                } 

                for (int i = 0; i < counter; i++)
                {
                    result.Add(reader.GetName(i) +""+ Convert.ToString(reader[i]));
                }

                //indica o fim da linha
                result.Add(Convert.ToString("endrow"));
            };

            return result;
        }

        public String rowReaderForSimpleObjects(string resultRow, string attName, bool lastField)
        {
            string columnValue = "";
            string manipulator = "";

            int begin = ((resultRow.IndexOf(attName) - 1) + (attName.Length + 1));
            manipulator = resultRow.Substring(begin, resultRow.Length - begin);
            int end = lastField ? manipulator.Length : (begin + manipulator.IndexOf(";"));

            columnValue = lastField ? resultRow.Substring(begin, end) : resultRow.Substring(begin, (end - begin));

            return columnValue;
        }
    }
}