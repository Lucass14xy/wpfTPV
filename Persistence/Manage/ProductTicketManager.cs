using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataGridPersonas.persistence;
using WpfTPV.Domain;

namespace WpfTPV.Persistence.Manage
{
    class ProductTicketManager
    {
        public List<Producto_Ticket> listProductTicket { get; set; }
        int lastId;

        public ProductTicketManager()
        {
            listProductTicket = new List<Producto_Ticket>();
        }
       

        //public void ReadProductTicket()
        //{
        //    Producto_Ticket pt = null;
        //    List<Object> lproductosTicket;
        //    lproductosTicket = DBBroker.ObtenerAgente().Leer("select * from producto_ticket order by idticket");
        //    foreach (List<Object> aux in lproductosTicket)
        //    {
        //        pt = new Producto_Ticket(Int32.Parse(aux[1].ToString()), Int32.Parse(aux[2].ToString()), Int32.Parse(aux[3].ToString()));//
        //        this.listProductTicket.Add(pt);
        //    }
        //}
        public void AddProductTicket(Producto_Ticket pt)
        {
            DBBroker.ObtenerAgente().Modificar("insert into producto_ticket (idticket,idproducto,unidades) values(" + pt.Idticket + "," + pt.Idproducto + "," + pt.Unidades+");");
            listProductTicket.Add(pt);
        }
        public void DeleteProduct(Producto_Ticket pt)
        {
            DBBroker.ObtenerAgente().Modificar("delete from producto_ticket where idproducto=" + pt.Idproducto + ";");
            listProductTicket.Remove(pt);
        }
        
    }

}
