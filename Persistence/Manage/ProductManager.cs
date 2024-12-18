using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataGridPersonas.persistence;
using Google.Protobuf.Compiler;
using WpfTPV.Domain;

namespace WpfTPV.Persistence.Manage
{
    class ProductManager
    {
        public List<Producto> listProducto { get; set; }
        int lastId;

        public ProductManager()
        {
            listProducto = new List<Producto>();
        }
        public int getLastId()
        {
            List<Object> listAux;
            listAux = DBBroker.ObtenerAgente().Leer("SELECT MAX(idproducto) FROM producto;");

            foreach (List<Object> auxProduct in listAux)
            {
                lastId = Convert.ToInt32(auxProduct[0]) + 1;
            }

            return lastId;
        }
        
        public void ReadProduct()
        {
            Producto p = null;
            List<Object> lproductos;
            lproductos = DBBroker.ObtenerAgente().Leer("select * from producto order by idproducto");
            foreach (List<Object> aux in lproductos)
            {
                p = new Producto(Int32.Parse(aux[0].ToString()), aux[1].ToString(), Convert.ToDouble(aux[2].ToString()), Int32.Parse(aux[3].ToString()), aux[4].ToString());//
                this.listProducto.Add(p);
            }
        }
        public void AddProduct(Producto p)
        {
            DBBroker.ObtenerAgente().Modificar("insert into producto values("+p.Idproducto+",'" + p.Nombre + "'," + p.Precio.ToString().Replace(',','.') + "," + p.Categoria + ",'" + p.Imagen + "');");
            listProducto.Add(p);
        }
        public void DeleteProduct(Producto p)
        {
            DBBroker.ObtenerAgente().Modificar("delete from producto where nombre='" + p.Nombre + "';");
            listProducto.Remove(p);
        }
        public void UpdateProduct(Producto p)
        {
            DBBroker.ObtenerAgente().Modificar("update producto set nombre='" + p.Nombre + "', precio=" + p.Precio.ToString().Replace(',','.') + ", categoria=" + p.Categoria + ", imagen='" + p.Imagen + "' where idproducto=" + p.Idproducto + ";");
        }
    }
}
