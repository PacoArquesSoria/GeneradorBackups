using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FSBitBossSolutions;

namespace GeneradorBackups
{
   public partial class FormAgregarFicherosACScs : Form
   {
      /****** CONSTANTES DE LA VENTANA ******/
      /**************************************/
      private const string kBotonSeleccionarTodoAnularSeleccion_SeleccionarTodo = "Seleccionar todo";                // etiqueta del botón "Seleccionar todo/Anular selección" cuando está en modo "Seleccionar todo"
      private const string kBotonSeleccionarTodoAnularSeleccion_AnularSeleccion = "Anular selección";                // etiqueta del botón "Seleccionar todo/Anular selección" cuando está en modo "Anular selección"

      /****** VARIABLES DE LA VENTANA ******/
      /*************************************/
      private string? directorio;                                                                     // directorio donde nos ubicamos
      private bool seleccionarAnular;                                                                 // flag que nos dice si hemos de seleccionar o hemos de anular la selección

      /****** PROPIEDADES DE LA VENTANA ******/
      /***************************************/
      // ventanaPadre -> Puntero a la ventana padre
      public FormEditarAgregarCopiaSeguridad? ventanaPadre
      {
         set; private get;
      }

      // gestorBD -> Puntero al gestor de la base de datos de la aplicación
      public ClassGestorBD? gestorBD
      {
         set; private get;
      }

      // ficherosSeleccionados -> Lista de ficheros seleccionados
      public List<string>? ficherosSeleccionados
      {
         set; get;
      }

      // operacionCancelada -> Nos devuelve un booleano indicando si la operación ha sido cancelada (valor true) o no (valor false).
      public bool operacionCancelada
      {
         private set; get;
      }

      /****** CONSTRUCTOR DE LA CLASE ******/
      /*************************************/
      // por defecto
      public FormAgregarFicherosACScs()
      {
         InitializeComponent();
         // inicializamos las variables de la ventana
         directorio = null;
         seleccionarAnular = true;
         // inicializamos las propiedades de la ventana
         ventanaPadre = null;
         gestorBD = null;
         ficherosSeleccionados = null;
         operacionCancelada = true;
      }

      /****** MÉTODOS DE LA CLASE ******/
      /*********************************/
      // función que inicializa el directorio
      private bool inicializarDirectorio()
      {
         directorio = Environment.GetEnvironmentVariable("USERPROFILE");
         if (string.IsNullOrEmpty(directorio))
         {
            MessageBox.Show("No se ha podido leer la variable de entorno USERPROFILE", "ERROR INTERNO DE LA APLICACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
         }
         string mensajeError;
         if (!ClassFileSystem.cdhome(out mensajeError))
         {
            MessageBox.Show(mensajeError, "NO SE PUEDE ACCEDER AL DIRECTORIO HOME DEL USUARIO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
         }
         textBoxDirectorio.Text = directorio;
         return true;
      }

      // función que inicializa el widget encargado de seleccionar el directorio
      private void inicializarWidgetDirectorio()
      {
         folderBrowserDialogSelectorDirectorios.InitialDirectory = directorio;
         folderBrowserDialogSelectorDirectorios.SelectedPath = directorio;
      }

      // función que inicializa el selector de ficheros y directorios
      private bool inicializarSelectorFicherosYDirectorios()
      {
         bool resultado;
         string mensajeError;
         clbFicherosYDirectorios.Items.Clear();
         // obtenemos la lista de subdirectorios

         List<string>? contenidoDirectorio = ClassFileSystem.listaDirectorios(directorio, null, out resultado, out mensajeError);
         if (resultado)
         {
            if (contenidoDirectorio != null && contenidoDirectorio.Count > 0)
            {
               foreach (string dir in contenidoDirectorio)
               {
                  string nombreRelativo = ClassFileSystem.nombreRelativoArchivo(dir, out resultado, out mensajeError);
                  if (resultado)
                  {
                     clbFicherosYDirectorios.Items.Add(nombreRelativo);
                     if (ficherosSeleccionados != null && ficherosSeleccionados.Count > 0)
                     {
                        if (ficherosSeleccionados.Contains(dir))
                           clbFicherosYDirectorios.SetItemChecked(clbFicherosYDirectorios.Items.IndexOf(nombreRelativo), true);
                        else
                           clbFicherosYDirectorios.SetItemChecked(clbFicherosYDirectorios.Items.IndexOf(nombreRelativo), false);
                     }
                  }
                  else
                  {
                     // no se ha podido obtener el nombre relativo de un archivo o carpeta
                     MessageBox.Show(mensajeError, "ERROR AL OBTENER EL NOMBRE RELATIVO DE UN ARCHIVO/CARPETA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                     return false;
                  }
               }
            }
            // obtenemos la lista de ficheros ordinarios
            contenidoDirectorio = ClassFileSystem.listaFicheros(directorio, null, false, out resultado, out mensajeError);
            if (resultado)
            {
               if (contenidoDirectorio != null && contenidoDirectorio.Count > 0)
               {
                  foreach (string dir in contenidoDirectorio)
                  {
                     string nombreRelativo = ClassFileSystem.nombreRelativoArchivo(dir, out resultado, out mensajeError);
                     if (resultado)
                     {
                        clbFicherosYDirectorios.Items.Add(nombreRelativo);
                        if (ficherosSeleccionados != null && ficherosSeleccionados.Count > 0)
                        {
                           if (ficherosSeleccionados.Contains(nombreRelativo))
                              clbFicherosYDirectorios.SetItemChecked(clbFicherosYDirectorios.Items.IndexOf(nombreRelativo), true);
                           else
                              clbFicherosYDirectorios.SetItemChecked(clbFicherosYDirectorios.Items.IndexOf(nombreRelativo), false);
                        }
                     }
                     else
                     {
                        // no se ha podido obtener el nombre relativo de un archivo o carpeta
                        MessageBox.Show(mensajeError, "ERROR AL OBTENER EL NOMBRE RELATIVO DE UN ARCHIVO/CARPETA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                     }
                  }
               }
               return true;
            }
            else
            {
               // no se ha podido leer el contenido del directorio
               MessageBox.Show(mensajeError, "ERROR AL LEER EL CONTENIDO DEL DIRECTORIO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
               return false;
            }
         }
         else
         {
            // no se ha podido leer el contenido del directorio
            MessageBox.Show(mensajeError, "ERROR AL LEER EL CONTENIDO DEL DIRECTORIO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
         }
      }

      // función que inicializa el listado de los ficheros/directorios seleccionados
      private void inicializarListadoFicherosDirectoriosSeleccionados()
      {
         string contenido = "";
         if (ficherosSeleccionados != null && ficherosSeleccionados.Count > 0)
         {
            foreach (string fichero in ficherosSeleccionados)
            {
               if (contenido == "")
                  contenido = fichero;
               else
                  contenido += "\r\n" + fichero;
            }
         }
         textBoxFicherosDirectoriosSeleccionados.Text = contenido;
      }

      // función que establece el título del botón "Seleccionar todo/Anular selección"
      private void establecerEtiquetaBotonSeleccionarTodoAnularSeleccion()
      {
         switch (seleccionarAnular)
         {
            case false:
               buttonSeleccionarTodoAnularSeleccion.Text = kBotonSeleccionarTodoAnularSeleccion_AnularSeleccion;
               break;
            case true:
               buttonSeleccionarTodoAnularSeleccion.Text = kBotonSeleccionarTodoAnularSeleccion_SeleccionarTodo;
               break;
         }
      }

      // función que nos dice si hay elementos seleccionados o no en el selector de ficheros
      private bool hayElementosSeleccionadosEnElSelectorFicheros()
      {
         // verificamos si existen elementos seleccionados
         if (clbFicherosYDirectorios.CheckedItems == null || (clbFicherosYDirectorios.CheckedItems != null && clbFicherosYDirectorios.CheckedItems.Count == 0))
            // no existen -> devolveremos false al procedimiento invocante
            return false;
         else
            // existen -> devolveremos true al procedimiento invocante
            return true;
      }

      // función que establece el modo del botón "Seleccionar todo/Anular selección"
      private void establecerModoBotonSeleccionarTodoAnularSeleccion()
      {
         seleccionarAnular = !hayElementosSeleccionadosEnElSelectorFicheros();
         establecerEtiquetaBotonSeleccionarTodoAnularSeleccion();
      }

      // función que actualiza la lista de ficheros y directorios seleccionados
      private void actualizarListaFicherosYDirectoriosSeleccionados()
      {
         bool nuevaLista = (ficherosSeleccionados == null || (ficherosSeleccionados != null && ficherosSeleccionados.Count == 0));
         if (ficherosSeleccionados == null)
            ficherosSeleccionados = new List<string>();
         for (int i = 0; i < clbFicherosYDirectorios.Items.Count; i++)
         {
            string? nombreAbsoluto = directorio + "\\" + clbFicherosYDirectorios.Items[i].ToString();
            if (clbFicherosYDirectorios.GetItemChecked(i))
            {
               if (nuevaLista)
                  ficherosSeleccionados.Add(nombreAbsoluto);
               else
               {
                  if (!ficherosSeleccionados.Contains(nombreAbsoluto))
                     ficherosSeleccionados.Add(nombreAbsoluto);
               }
            }
            else
            {
               if (!nuevaLista)
               {
                  if (ficherosSeleccionados.Contains(nombreAbsoluto))
                     ficherosSeleccionados.Remove(nombreAbsoluto);
               }
            }
         }
         inicializarListadoFicherosDirectoriosSeleccionados();
      }

      // función que activa o desactiva los widgets de la ventana excepto el botón "Cancelar". Como parámetro se le pasa un booleano que nos indica si hemos de activarlos
      // (valor true) o hemos de desactivarlo (valor false). Por defecto, valdrá true.
      private void activarWidgets(bool onoff = true)
      {
         labelDirectorio.Enabled = onoff;
         textBoxDirectorio.Enabled = onoff;
         buttonExaminar.Enabled = onoff;
         clbFicherosYDirectorios.Enabled = onoff;
         buttonSeleccionarTodoAnularSeleccion.Enabled = onoff;
         buttonAplicar.Enabled = onoff;
         labelFicherosYDirectoriosSeleccionados.Enabled = onoff;
         textBoxFicherosDirectoriosSeleccionados.Enabled = onoff;
         buttonAceptar.Enabled = onoff;
         buttonCancelar.Enabled = true;
      }

      // función que inicializa el tamaño del textBox del directorio. Devuelve un booleano indicando si la operación se ha efectuado correctamente (true) o no (false).
      private bool inicializarTextBoxDirectorio()
      {
         if (gestorBD != null)
         {
            int tamDirectorio = gestorBD.obtenerTamannoColumnaTabla(sDatosTablaFicheros.nombreTabla, sDatosTablaFicheros.colNombreAbsoluto);
            if (gestorBD.resultado)
            {
               // se ha leído correctamente el tamaño de la columna de la tabla -> estableceremos la máxima longitud del textBox y devolveremos true al procedimiento invocante
               textBoxDirectorio.MaxLength = tamDirectorio;
               return true;
            }
            else
            {
               // no se ha leído correctamente el tamaño de la columna de la tabla -> mostraremos un cuadro de diálogo indicándolo y devolveremos false al procedimiento invocante
               MessageBox.Show(gestorBD.mensajeError, "ERROR AL OBTENER EL TAMAÑO DE LA COLUMNA DE LA TABLA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
               return false;
            }
         }
         else
            return false;
      }

      /****** MANEJADORES DE LA VENTANA ******/
      /***************************************/
      // al abrir la ventana
      private void FormAgregarFicherosACScs_Load(object sender, EventArgs e)
      {
         // desactivamos la ventana padre que lo invocó
         if (ventanaPadre != null)
            ventanaPadre.Enabled = false;
      }

      // al cierre de la ventana
      private void FormAgregarFicherosACScs_FormClosed(object sender, FormClosedEventArgs e)
      {
         // activamos la ventana padre que lo invocó
         if (ventanaPadre != null)
            ventanaPadre.Enabled = true;
      }

      // al pulsar el botón "Aceptar"
      private void buttonAceptar_Click(object sender, EventArgs e)
      {
         // indicamos que la operación no ha sido cancelada
         operacionCancelada = false;
         // cerramos la ventana
         Close();
      }

      // al pulsar el botón "Cancelar"
      private void buttonCancelar_Click(object sender, EventArgs e)
      {
         // se limita a cerrar la ventana
         Close();
      }

      // al mostrar por primera vez la ventana
      private void FormAgregarFicherosACScs_Shown(object sender, EventArgs e)
      {
         if (gestorBD == null)
         {
            MessageBox.Show("No está definido el gestor de la base de datos de la aplicación.", "ERROR INTERNO DE LA APLICACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Close();
            return;
         }
         if (!inicializarTextBoxDirectorio())
         {
            Close();
            return;
         }
         if (!inicializarDirectorio())
         {
            Close();
            return;
         }
         if (!inicializarSelectorFicherosYDirectorios())
         {
            Close();
            return;
         }
         inicializarWidgetDirectorio();
         inicializarListadoFicherosDirectoriosSeleccionados();
         establecerModoBotonSeleccionarTodoAnularSeleccion();
         activarWidgets();
      }

      // se ha pulsado el botón "Seleccionar todo/Anular selección"
      private void buttonSeleccionarTodoAnularSeleccion_Click(object sender, EventArgs e)
      {
         // análisis del modo en el que estamos
         bool status;
         switch (seleccionarAnular)
         {
            case false:                                                                   // modo anular selección
               status = false;
               break;
            case true:                                                                    // modo seleccionar todo
               status = true;
               break;
         }
         // cambia el estado del check de cada uno de los elementos
         for (int i = 0; i < clbFicherosYDirectorios.Items.Count; i++)
         {
            clbFicherosYDirectorios.SetItemChecked(i, status);
         }
         // finalmente, cambia la etiqueta del botón "Seleccionar todo/Anular selección"
         establecerModoBotonSeleccionarTodoAnularSeleccion();
      }

      // se ha pulsado el botón "Aplicar"
      private void buttonAplicar_Click(object sender, EventArgs e)
      {
         actualizarListaFicherosYDirectoriosSeleccionados();
      }

      // se ha pulsado el botón "Examinar"
      private void buttonExaminar_Click(object sender, EventArgs e)
      {
         folderBrowserDialogSelectorDirectorios.InitialDirectory = directorio;
         // mostramos el widget de selección de directorios
         switch (folderBrowserDialogSelectorDirectorios.ShowDialog())
         {
            case DialogResult.OK:                                                                              // se ha pulsado Ok
               textBoxDirectorio.Text = folderBrowserDialogSelectorDirectorios.SelectedPath;
               break;
            case DialogResult.Cancel:                                                                          // se ha pulsado Cancelar
               break;
            default:                                                                                           // otras opciones: error gordo
               activarWidgets(false);
               MessageBox.Show("Opción desconocida al procesar el botón Examinar.", "ERROR INTERNO DE LA APLICACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Error);
               break;
         }
      }

      // se ha cambiado el valor del selector de ficheros y directorios
      private void clbFicherosYDirectorios_SelectedValueChanged(object sender, EventArgs e)
      {
         // establece el modo botón en el que estamos
         establecerModoBotonSeleccionarTodoAnularSeleccion();
      }

      // se ha validado el texto del cambio de directorio
      private void textBoxDirectorio_Validated(object sender, EventArgs e)
      {
         // leemos lo que hemos tecleado o hemos puesto empleando el selector de directorios
         string? nuevoDirectorio = textBoxDirectorio.Text;
         if (!string.IsNullOrEmpty(nuevoDirectorio))
         {
            if (ClassFileSystem.existeDirectorio(nuevoDirectorio))
            {
               bool resultado;
               string mensajeError;
               if (ClassFileSystem.chdir(nuevoDirectorio, out mensajeError))
               {
                  // se ha cambiado al nuevo directorio -> obtendremos el path absoluto del nuevo directorio
                  directorio = ClassFileSystem.pwd(out resultado, out mensajeError);
                  if (resultado)
                  {
                     inicializarSelectorFicherosYDirectorios();
                     textBoxDirectorio.Text = directorio;
                     establecerModoBotonSeleccionarTodoAnularSeleccion();
                  }
                  else
                  {
                     activarWidgets(false);
                     MessageBox.Show(mensajeError, "ERROR AL OBTENER EL NOMBRE DE LA CARPETA ACTUAL DE TRABAJO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  }
               }
            }
         }
      }
   }
}
