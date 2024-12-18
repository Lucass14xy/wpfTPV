using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTPV.Persistence.Manage;

namespace WpfTPV.Domain
{
    public class Producto_Ticket
    {
        private int idproducto_ticket;
        private int idticket;
        private int idproducto;
        private int unidades;
        private double total;
        private Producto producto;
        private Ticket ticket;

        private ProductManager pm = new ProductManager();
        
        public Producto_Ticket(int idticket, int idproducto, int unidades)
        {
            this.idticket = idticket;
            this.idproducto = idproducto;
            this.unidades = unidades;
            this.producto = pm.listProducto.Find(p => p.Idproducto == idproducto);
            this.total = Math.Round((producto.Precio * unidades),3);
        }
        public Producto_Ticket(int unidades)
        {
            this.unidades = unidades;
        }
        public int Idticket
        {
            get { return idticket; }
            set { idticket = value; }
        }
        public int Idproducto
        {
            get { return idproducto; }
            set { idproducto = value; }
        }
        public int Unidades
        {
            get { return unidades; }
            set { unidades = value; }
        }
        public double Total
        {
            get { return total; }
            set { total = Math.Round(value,3); }
        }
        public Producto Producto
        {
            get { return producto; }
            set { producto = value; }
        }
    }
}
