using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTPV.Domain
{
    public class Ticket
    {
        private int idticket;
        private DateTime fecha;
        private string cliente;
        private double total;
        private List<Producto_Ticket> products;
        public Ticket(int idticket, DateTime fecha)
        {
            this.idticket = idticket;
            this.fecha = fecha;
            //this.cliente = cliente;
            //this.total = total;
        }
        public Ticket()
        {
            products = new List<Producto_Ticket>();
        }

        public int Idticket
        {
            get { return idticket; }
            set { idticket = value; }
        }
        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }
        public string Cliente
        {
            get { return cliente; }
            set { cliente = value; }
        }
        public double Total
        {
            get { return total; }
            set { total = value; }
        }
        public List<Producto_Ticket> Products { get { return products; } set { products = value; } }
    }
}
