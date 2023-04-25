using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace EjemploFechasHoras
{
    class Fichaje
    {
        private String nif;
        private String nombre;
        private DateTime fecha;
        private DateTime horaEntrada;
        private DateTime horaSalida;
        private bool finalizado;  // False (no finalizado) - True (finalizado)
        private float tiempoTotal;

        public String Nif { get { return nif; } set { nif = value; } }
        public String Nombre { get { return nombre; } set { nombre = value; } }
        public DateTime Fecha { get { return fecha; } set { fecha = value; } }
        public DateTime HEntrada { get { return horaEntrada; } set { horaEntrada = value; } }
        public DateTime HSalida { get { return horaSalida; } set { horaSalida = value; } }
        public bool Finalizado { get { return finalizado; } set { finalizado = value; } }
        public float TiempoTotal { get { return tiempoTotal; } set { tiempoTotal = value; } }

        public Fichaje(String dni)
        {
            nif = dni;
            HSalida = DateTime.MinValue;  // Valor mínimo posible -no admite NULL-
            finalizado = false;  // Cuando se crea el fichaje está abierto
        }

        public int DarEntrada()
        {
            int retorno;
            String consulta = "INSERT INTO fichajes (nif,nombre, fecha,horaentrada,horasalida,finalizado, tiempoTotal) " +
                              "VALUES (@NIF,@Nombre, @Dia,@Entrada,@Salida,@Finalizado, @TiempoTotal)";

            MySqlCommand comando = new MySqlCommand(consulta, ConBD.Conexion);
            comando.Parameters.AddWithValue("NIF", this.nif);
            comando.Parameters.AddWithValue("Nombre", this.nombre);
            comando.Parameters.AddWithValue("Dia", this.fecha.ToString("yyyy/MM/dd"));
            comando.Parameters.AddWithValue("Entrada", this.horaEntrada);
            comando.Parameters.AddWithValue("Salida", this.horaSalida);
            comando.Parameters.AddWithValue("Finalizado", this.finalizado);
            comando.Parameters.AddWithValue("TiempoTotal", this.tiempoTotal);

            retorno = comando.ExecuteNonQuery();
            return retorno;
        }

        public int DarSalida()
        {

            int retorno;
            String consulta = "UPDATE fichajes SET Salida=@Salida,Finalizado=@Finalizado WHERE Nombre=@Nombre AND Finalizado=0";
                        
            MySqlCommand comando = new MySqlCommand(consulta, ConBD.Conexion);
            comando.Parameters.AddWithValue("Nombre", this.nombre);
            comando.Parameters.AddWithValue("Salida", this.horaSalida);
            comando.Parameters.AddWithValue("Finalizado", this.finalizado);

            retorno = comando.ExecuteNonQuery();
            return retorno;
        }

        public static List<Fichaje> VerFichajesTotales()
        {
            List<Fichaje> totalFichajes = new List<Fichaje>();
            String consulta = "SELECT * FROM fichajes";
            MySqlCommand comando = new MySqlCommand(consulta, ConBD.Conexion);
            MySqlDataReader reader = comando.ExecuteReader();

            while (reader.Read())
            {
                Fichaje nuevo = new Fichaje(reader.GetString(0));   
                nuevo.fecha = reader.GetDateTime(1);   
                nuevo.horaEntrada = Convert.ToDateTime(reader["Entrada"]);
                nuevo.horaSalida = Convert.ToDateTime(reader["Salida"]);
                nuevo.finalizado = reader.GetBoolean(4);
              
                totalFichajes.Add(nuevo);
            }

            return totalFichajes;
        }


    }
}
