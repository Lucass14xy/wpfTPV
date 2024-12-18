using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using WpfTPV.Domain;
using WpfTPV.Persistence.Manage;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Windows.Controls.Image;

namespace WpfTPV
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Producto_Ticket productoSeleccionado;
        private List<Producto_Ticket> listTicket;
        private ProductManager pm;
        private ProductTicketManager ptm;
        private TicketManager tm;
        private Ticket ticket;
        private string imageBase64 = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            pm = new ProductManager();
            ptm = new ProductTicketManager();
            tm = new TicketManager();
            pm.ReadProduct();
            CrearBotonesDinamicos();
            listTicket = new List<Producto_Ticket>();
            dtgTicket.ItemsSource = listTicket;
            
            
        }
        #region Calculator buttons
        private void btnC_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text = "";
        }
        private void btnX_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text += "*";
        }
        private void btnD_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text += "/";
        }
        private void btnP_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text += "+";
        }
        private void btnM_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text += "-";
        }
        private void btnDot_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text += ".";
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text += "1";
        }
        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text += "2";
        }
        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text += "3";
        }
        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text += "4";
        }
        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text += "5";
        }
        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text += "6";
        }
        private void btn7_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text += "7";
        }
        private void btn8_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text += "8";
        }
        private void btn9_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text += "9";
        }
        private void btn0_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text += "0";
        }
        #endregion
        #region Product buttons
        private void logic_btnProduct(String name)
        {
            if (txtResult.Text == "" || txtResult.Text == "0.0")
            {
                foreach(Producto_Ticket pt in listTicket)
                {
                    if (pt.Producto.Nombre == name)
                    {
                        pt.Unidades++;
                        pt.Total = pt.Producto.Precio * pt.Unidades;
                        dtgTicket.Items.Refresh();
                        UpdateTotal();
                        return;
                    }
                }
                productoSeleccionado = new Producto_Ticket(1);
                //ticket = new Ticket(tm.getLastId(),DateTime.Now);
                //ticket.Cliente = "Cliente1";
                //ticket.Total = 10;
                //tm.AddTicket(ticket);
                productoSeleccionado.Producto = pm.listProducto.Find(p => p.Nombre == name);
                productoSeleccionado.Idproducto = productoSeleccionado.Producto.Idproducto;
                productoSeleccionado.Total = productoSeleccionado.Producto.Precio * productoSeleccionado.Unidades;
                //productoSeleccionado.Idticket = tm.getLastId()-1;
                //ptm.AddProductTicket(productoSeleccionado);
                listTicket.Add(productoSeleccionado);

                dtgTicket.Items.Refresh();

            }
            else if (txtResult.Text.Contains("*"))
            {
                string[] result = txtResult.Text.Split('*');
                foreach (Producto_Ticket pt in listTicket)
                {
                    if (pt.Producto.Nombre == name)
                    {
                        pt.Unidades+= Convert.ToInt32(result[0]);
                        pt.Total = pt.Producto.Precio * pt.Unidades;
                        dtgTicket.Items.Refresh();
                        UpdateTotal();
                        return;
                    }
                }
                productoSeleccionado=new Producto_Ticket(Convert.ToInt32(result[0]));
                productoSeleccionado.Producto = pm.listProducto.Find(p => p.Nombre == name);
                productoSeleccionado.Total = productoSeleccionado.Producto.Precio * productoSeleccionado.Unidades;
                listTicket.Add(productoSeleccionado);
                dtgTicket.Items.Refresh();
            }
            UpdateTotal();
        }
        private void UpdateTotal()
        {
            double total = listTicket.Sum(p => p.Total);
            TotalTextBlock.Text = total.ToString("C2");// Formato moneda
        }
        private void CrearBotonesDinamicos()
        {
            uGridBreakfast.Children.Clear();
            uGridDrink.Children.Clear();
            uGridCoffe.Children.Clear();

            pm.listProducto.Clear();
            pm.ReadProduct();

            foreach (Producto p in pm.listProducto)
            {
                StackPanel sp = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };

                // Decodificar la imagen Base64 y crear un BitmapImage
                BitmapImage bitmap = new BitmapImage();
                if (!string.IsNullOrEmpty(p.Imagen))
                {
                    try
                    {
                        byte[] imageBytes = Convert.FromBase64String(p.Imagen);
                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            bitmap.BeginInit();
                            bitmap.StreamSource = ms;
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.EndInit();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al cargar la imagen del producto '{p.Nombre}': {ex.Message}");
                        continue; 
                    }
                }

                Image img = new Image
                {
                    Source = bitmap,
                    Width = 50,
                    Height = 50,
                    Margin = new Thickness(5)
                };

                
                TextBlock tb = new TextBlock
                {
                    Text = $"{p.Nombre}\nPrecio: {p.Precio:C2}",
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 14,
                    Margin = new Thickness(5, 0, 0, 0)
                };

               
                sp.Children.Add(img);
                sp.Children.Add(tb);

           
                Button boton = new Button
                {
                    Content = sp,
                    Margin = new Thickness(5),
                    Tag = p.Nombre 
                };
       

                boton.Background = new SolidColorBrush(Colors.White);
               
                boton.Click += Boton_Click;

               
                if (p.Categoria == 1)
                {
                    uGridBreakfast.Children.Add(boton);
                }
                else if (p.Categoria == 2)
                {
                    uGridDrink.Children.Add(boton);
                }
                else if (p.Categoria == 3)
                {
                    uGridCoffe.Children.Add(boton);
                }

            }
        }

        private void Boton_Click(object sender, RoutedEventArgs e)
        { 
            Button boton = (Button)sender;
            logic_btnProduct(boton.Tag.ToString());
        }

        #endregion
        #region Aux buttons
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if(dtgTicket.SelectedItem != null)
            {
                Producto_Ticket pt = (Producto_Ticket)dtgTicket.SelectedItem;
                listTicket.Remove(pt);
                dtgTicket.Items.Refresh();
                UpdateTotal();
            }
        }

        private void btnPay_Click(object sender, RoutedEventArgs e)
        {
            if (listTicket.Count > 0)
            {
                //double total = listTicket.Sum(p => p.Total);
                //string outStr = "";
                //foreach(Producto_Ticket pt in listTicket){
                //    outStr += "# Producto: "+pt.Producto.Nombre+", Cantidad: "+pt.Unidades+", Precio: "+pt.Producto.Precio+" €, Total: "+pt.Total+ " €\n";
                //}
                //outStr += "\n" + "*************************************************************";
                //outStr += "************************************************-> TOTAL A PAGAR: " + total.ToString("F2") + " € <-************";
                //MessageBox.Show(outStr);
                View.TicketWindow ticketWindow = new View.TicketWindow(listTicket);
                ticketWindow.Left = 100;
                ticketWindow.Top = 10;
                ticketWindow.Show();
            }
            else
            {
                MessageBox.Show("No hay productos");
            }

        }

        private void btnImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Seleccionar Imagen",
                Filter = "Archivos de Imagen|*.jpg;*.jpeg;*.png;*.bmp;*.gif"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Cargar la imagen seleccionada y mostrarla en el control Image
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(openFileDialog.FileName);
                    bitmap.EndInit();

                    byte[] imageBytes = File.ReadAllBytes(openFileDialog.FileName);
                    imageBase64 = Convert.ToBase64String(imageBytes);

                    ImagenMostrada.Source = bitmap;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar la imagen: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string nombre = txtNameProduct.Text.Trim();
            string precioTexto = txtPrice.Text.Replace(',','.');
            ComboBoxItem categoriaSeleccionada = (ComboBoxItem)cbCategory.SelectedItem;

        
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(precioTexto) || categoriaSeleccionada == null)
            {
                MessageBox.Show("Por favor, complete todos los campos.");
                return;
            }
            Double precio;
            if (!Double.TryParse(precioTexto, NumberStyles.Any, CultureInfo.InvariantCulture, out precio))
            {
                MessageBox.Show("Por favor, ingrese un precio válido.");
                return;
            }
            

            int categoria = int.Parse(categoriaSeleccionada.Tag.ToString());

        
            if (string.IsNullOrEmpty(imageBase64))
            {
                MessageBox.Show("Se recomienda añadir una imagen");
                btnImage_Click(sender,e);
            }

 
            try
            {
               Producto producto = new Producto(pm.getLastId(), nombre, precio, categoria, imageBase64);
                pm.AddProduct(producto);
                MessageBox.Show("Producto añadido correctamente.");
                CrearBotonesDinamicos();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al añadir el producto: {ex.Message}");
            }

            txtNameProduct.Clear();
            txtPrice.Clear();
            cbCategory.SelectedIndex = -1;
            ImagenMostrada.Source = null;
            imageBase64 = string.Empty;
        }

        private void btncambiarImage_Click(object sender, RoutedEventArgs e)
        {
            
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Seleccionar Nueva Imagen",
                Filter = "Archivos de Imagen|*.jpg;*.jpeg;*.png;*.bmp;*.gif"
            };

          
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                   
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(openFileDialog.FileName);
                    bitmap.EndInit();

                    byte[] imageBytes = File.ReadAllBytes(openFileDialog.FileName);
                    imageBase64 = Convert.ToBase64String(imageBytes);

                    ImagenCambio.Source = bitmap;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar la imagen: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnModifyProduct_Click(object sender, RoutedEventArgs e)
        {
            string nombre = txtActNameProduct.Text.Trim();
            string nuevoNombre = txtNewNameProduct.Text.Trim();
            string precioTexto = txtNewPrice.Text.Trim().Replace(',','.');
            ComboBoxItem categoriaSeleccionada = (ComboBoxItem)cbCategoryModify.SelectedItem;

       
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(nuevoNombre) || string.IsNullOrWhiteSpace(precioTexto) || categoriaSeleccionada == null)
            {
                MessageBox.Show("Por favor, complete todos los campos.");
                return;
            }

            if (!Double.TryParse(precioTexto, NumberStyles.Any, CultureInfo.InvariantCulture , out Double precio))
            {
                MessageBox.Show("Por favor, ingrese un precio válido.");
                return;
            }

            int categoria = int.Parse(categoriaSeleccionada.Tag.ToString());

           
            if (string.IsNullOrEmpty(imageBase64))
            {
                MessageBox.Show("Se recomienda añadir una imagen");
                btncambiarImage_Click(sender, e);
            }

          
            try
            {
                Producto producto = pm.listProducto.FirstOrDefault(p => p.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
                producto.Nombre = nuevoNombre;
                producto.Precio = precio;
                producto.Categoria = categoria;
                producto.Imagen = imageBase64;
                pm.UpdateProduct(producto);
                MessageBox.Show("Producto modificado correctamente.");
                CrearBotonesDinamicos();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar el producto: {ex.Message}");
            }

          
            txtActNameProduct.Clear();
            txtNewNameProduct.Clear();
            txtNewPrice.Clear();
            cbCategory.SelectedIndex = -1;
            ImagenCambio.Source = null;
            imageBase64 = string.Empty;
        }
        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            string nombre = txtDelNameProduct.Text;
            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("Por favor, ingrese un nombre.");
                return;
            }
            try
            {
                Producto producto = pm.listProducto.FirstOrDefault(p => p.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
                pm.DeleteProduct(producto);
                MessageBox.Show("Producto eliminado correctamente.");
                CrearBotonesDinamicos();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar el producto: {ex.Message}");
            }
            txtDelNameProduct.Clear();
        }
        #endregion

    }
}