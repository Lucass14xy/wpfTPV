using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataGridPersonas.persistence;
using WpfTPV.Domain;

namespace WpfTPV.Persistence.Manage
{
    class TicketManager
    {
        public List<Ticket> listTicket { get; set; }
        

        public TicketManager()
        {
            listTicket = new List<Ticket>();
        }
        public int getLastId()
        {
            int lastId = 1; // Valor por defecto si la tabla está vacía

            // Ejecutar la consulta para obtener el valor máximo de idticket
            List<Object> listAux = DBBroker.ObtenerAgente().Leer("SELECT MAX(idticket) FROM ticket;");

            if (listAux.Count > 0 && listAux[0] != DBNull.Value) // Verificar si hay resultados y si no son NULL
            {
                lastId = Convert.ToInt32(listAux[0].ToString()) + 1;
            }

            return lastId;
        
        }

        //public void ReadTicket()
        //{
        //    Ticket t = null;
        //    List<Object> lTicket;
        //    lTicket = DBBroker.ObtenerAgente().Leer("select * from ticket order by idticket");
        //    foreach (List<Object> aux in lTicket)
        //    {
        //        t = new Ticket(Int32.Parse(aux[0].ToString()),DateTime.Parse(aux[1].ToString()), aux[2].ToString(), Int32.Parse(aux[3].ToString()));//
        //        this.listTicket.Add(t);
        //    }
        //}
        public void AddTicket(Ticket t)
        {
            DBBroker.ObtenerAgente().Modificar("insert into ticket values(" + t.Idticket + ",'" + t.Fecha.ToString("yyyy-MM-dd HH:mm:ss") + "','" + t.Cliente + "',"+t.Total+");");
            listTicket.Add(t);
        }
        public void DeleteTicket(Ticket t)
        {
            DBBroker.ObtenerAgente().Modificar("delete from ticket where idticket=" + t.Idticket + ";");
            listTicket.Remove(t);
        }

    }
}
