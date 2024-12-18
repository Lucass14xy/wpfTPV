using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfTPV.Domain;

namespace WpfTPV.View
{
    /// <summary>
    /// Lógica de interacción para TicketWindow.xaml
    /// </summary>
    public partial class TicketWindow : Window
    {
        public TicketWindow(List<Producto_Ticket> lista)
        {
            InitializeComponent();
            mostrarTicket(lista);
        }

        private void mostrarTicket(List<Producto_Ticket> lista)
        { 
            TextBlock tbTicket = new TextBlock();
            tbTicket.FontSize = 12;
            tbTicket.FontFamily = new FontFamily("Courier New");
            tbTicket.Margin = new Thickness(10);
            double total = lista.Sum(p => p.Total);
            string outStr = "-----------------------------------------------\n    -----------TICKET -----------\n-----------------------------------------------\n";
        
  
            foreach (Producto_Ticket pt in lista)
            {
                outStr += "#" + pt.Producto.Nombre + ", Uds: " + pt.Unidades + ", Precio: " + pt.Producto.Precio + " €, Total: " + pt.Total + " €\n";
            }
            outStr += "\n*******************************************************";
            outStr += "\n***************************-> TOTAL A PAGAR: " + total.ToString("C2");
            tbTicket.Text = outStr;
            ticketDescGrid.Children.Add(tbTicket);
        }
    }
}

