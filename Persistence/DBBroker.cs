using System;
using System.Collections.Generic;

namespace DataGridPersonas.persistence
{
  public class DBBroker
  {
    private static DBBroker _instancia;
    private static MySql.Data.MySqlClient.MySqlConnection conexion;
    private const String cadenaConexion = "server=localhost;database=mydb;uid=root;pwd=toor";

    private DBBroker()
    {
      DBBroker.conexion = new MySql.Data.MySqlClient.MySqlConnection(DBBroker.cadenaConexion);

    }

    public static DBBroker ObtenerAgente()
    {
      if (DBBroker._instancia == null)
      {
        DBBroker._instancia = new DBBroker();
      }
      return DBBroker._instancia;
    }

    public List<Object> Leer(String sql)
    {
      List<Object> resultado = new List<object>();
      List<Object> fila;
      int i;
      MySql.Data.MySqlClient.MySqlDataReader reader;
      MySql.Data.MySqlClient.MySqlCommand com = new MySql.Data.MySqlClient.MySqlCommand(sql, DBBroker.conexion);

      Conectar();
      reader = com.ExecuteReader();
      while (reader.Read())
      {
        fila = new List<object>();
        for (i = 0; i <= reader.FieldCount - 1; i++)
        {
          fila.Add(reader[i].ToString());

        }
        resultado.Add(fila);
      }
      Desconectar();
      return resultado;
    }
    public int Modificar(String sql)
    {
      MySql.Data.MySqlClient.MySqlCommand com = new MySql.Data.MySqlClient.MySqlCommand(sql, DBBroker.conexion);
      int resultado;
      Conectar();
      resultado = com.ExecuteNonQuery();
      Desconectar();
      return resultado;
    }
    private void Conectar()
    {
      if (DBBroker.conexion.State == System.Data.ConnectionState.Closed)
      {
        DBBroker.conexion.Open();
      }
    }
    private void Desconectar()
    {
      if (DBBroker.conexion.State == System.Data.ConnectionState.Open)
      {
        DBBroker.conexion.Close();
      }
    }
  }
}
