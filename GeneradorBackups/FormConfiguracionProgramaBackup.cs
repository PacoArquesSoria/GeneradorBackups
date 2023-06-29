using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinRT;

namespace GeneradorBackups
{
   public partial class FormConfiguracionProgramaBackup : Form
   {
      /****** VARIABLES DE LA CLASE ******/
      /***********************************/
      private Dictionary<string, string?>? datosConfiguracion;                                                         // datos de configuración
      private string? directorio;                                                                                      // directorio de trabajo

      /****** PROPIEDADES DE LA VENTANA ******/
      /***************************************/
      // ventanaPrincipal -> Puntero a la ventana principal de la aplicación
      public FormPrincipal? ventanaPrincipal
      {
         set; private get;
      }

      // gestorBD -> Puntero al gestor de la base de datos de la aplicación
      public ClassGestorBD? gestorBD
      {
         set; private get;
      }

      /****** CONSTRUCTORES DE LA VENTANA ******/
      /*****************************************/
      // por defecto
      public FormConfiguracionProgramaBackup()
      {
         InitializeComponent();
         datosConfiguracion = null;
         ventanaPrincipal = null;
         gestorBD = null;
      }

      /****** MÉTODOS DE LA VENTANA ******/
      /***********************************/
      // función que obtiene los tamaños de los textboxes de la ventana
      private (int tamEjecutable, int tamOpciones, int tamOpcionListaFicheros, int tamExtensión) obtenerTamannoTextBoxes()
      {
         int tamEjecutable, tamOpciones, tamOpcionListaFicheros, tamExtensión;
         // obtenemos el tamaño máximo del nombre del ejecutable
         tamEjecutable = gestorBD.obtenerTamannoColumnaTabla(sDatosTablaConfiguracion.nombreTabla, sDatosTablaConfiguracion.colEjecutable);
         if (!gestorBD.resultado)
         {
            MessageBox.Show(gestorBD.mensajeError, "ERROR AL OBTENER EL TAMAÑO DE LA COLUMNA DE LA TABLA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return (-1, -1, -1, -1);
         }
         // obtenemos el tamaño máximo de las opciones del ejecutable
         tamOpciones = gestorBD.obtenerTamannoColumnaTabla(sDatosTablaConfiguracion.nombreTabla, sDatosTablaConfiguracion.colOpciones);
         if (!gestorBD.resultado)
         {
            MessageBox.Show(gestorBD.mensajeError, "ERROR AL OBTENER EL TAMAÑO DE LA COLUMNA DE LA TABLA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return (-1, -1, -1, -1);
         }
         // obtenemos el tamaño máximo de las opciones para la lista de ficheros
         tamOpcionListaFicheros = gestorBD.obtenerTamannoColumnaTabla(sDatosTablaConfiguracion.nombreTabla, sDatosTablaConfiguracion.colOpcionListaFicheros);
         if (!gestorBD.resultado)
         {
            MessageBox.Show(gestorBD.mensajeError, "ERROR AL OBTENER EL TAMAÑO DE LA COLUMNA DE LA TABLA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return (-1, -1, -1, -1);
         }
         // obtenemos el tamaño máximo de la opción para la extensión por defecto
         tamExtensión = gestorBD.obtenerTamannoColumnaTabla(sDatosTablaConfiguracion.nombreTabla, sDatosTablaConfiguracion.colExtensionPorDefecto);
         if (!gestorBD.resultado)
         {
            MessageBox.Show(gestorBD.mensajeError, "ERROR AL OBTENER EL TAMAÑO DE LA COLUMNA DE LA TABLA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return (-1, -1, -1, -1);
         }
         return (tamOpcionListaFicheros, tamOpciones, tamOpcionListaFicheros, tamExtensión);
      }

      // función que inicializa el selector de ficheros y la variable donde se almacenará el directorio raíz. Devuelve true si se ha inicializado correctamente y false, en
      // caso contrario.
      private bool inicializarSelectorFicheros()
      {
         directorio = Environment.GetEnvironmentVariable("ProgramFiles");
         if (directorio == null)
         {
            MessageBox.Show("No se ha podido leer la variable de entorno ProgramFiles.", "ERROR INTERNO DE LA APLICACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return false;
         }
         openFileDialogSelectorEjecutables.InitialDirectory = directorio;
         openFileDialogSelectorEjecutables.Multiselect = false;
         openFileDialogSelectorEjecutables.Filter = "Archivos binarios (*.exe; *.com)|*.exe;*.com |Ficheros por lotes (*.ps1;*.bat;*.cmd)|*.ps1;*.bat;*.cmd";
         openFileDialogSelectorEjecutables.FileName = "";
         return true;
      }

      // función que inicializa los textboxes de la ventana. Devuelve true si se han inicializado correctamente y false en caso contrario.
      private bool inicializarTextBoxes()
      {
         int tamEjecutable, tamOpciones, tamOpcionListaFicheros, tamExtensión;
         (tamEjecutable, tamOpciones, tamOpcionListaFicheros, tamExtensión) = obtenerTamannoTextBoxes();
         if (tamEjecutable == -1 && tamOpciones == -1 && tamOpcionListaFicheros == -1 && tamExtensión == -1)
            return false;
         else
         {
            textBoxNombreEjecutable.MaxLength = tamEjecutable;
            textBoxOpciones.MaxLength = tamOpciones;
            textBoxOpcionListaFicheros.MaxLength = tamOpcionListaFicheros;
            textBoxExtensiónPorDefecto.MaxLength = tamExtensión;
            return true;
         }
      }

      // función que inicializa la ventana con los datos que tenemos en la base de datos. Devuelve true si se han inicializado correctamente y false en caso contrario.
      private bool inicializarVentana()
      {
         datosConfiguracion = gestorBD.obtenerDatosConfiguracionProgramaCopiaSeguridad();
         if (gestorBD.resultado)
         {
            if (datosConfiguracion != null && datosConfiguracion.Count > 0)
            {
               textBoxNombreEjecutable.Text = datosConfiguracion[sDatosTablaConfiguracion.colEjecutable];
               textBoxOpciones.Text = datosConfiguracion[sDatosTablaConfiguracion.colOpciones];
               textBoxOpcionListaFicheros.Text = datosConfiguracion[sDatosTablaConfiguracion.colOpcionListaFicheros];
               textBoxExtensiónPorDefecto.Text = datosConfiguracion[sDatosTablaConfiguracion.colExtensionPorDefecto];
               return true;
            }
            else
            {
               textBoxNombreEjecutable.Text = "";
               textBoxOpciones.Text = "";
               textBoxOpcionListaFicheros.Text = "";
               textBoxExtensiónPorDefecto.Text = "";
               return true;
            }
         }
         else
         {
            MessageBox.Show(gestorBD.mensajeError, "ERROR AL OBTENER LOS DATOS DE CONFIGURACIÓN DEL PROGRAMA DE BACKUP", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
         }
      }

      // función que nos dice si hemos de activar o desactivar el botón Aceptar de la ventana. No se le pasa nada como parámetro.
      private bool hayQueActivarBotonAceptar()
      {
         // obtenemos los valores que hay introducidos en la actualidad
         string ejecutable = textBoxNombreEjecutable.Text;
         string opciones = textBoxOpciones.Text;
         string opcionListaFicheros = textBoxOpcionListaFicheros.Text;
         string extensión = textBoxExtensiónPorDefecto.Text;
         // comprobamos si los datos de configuración de la base de datos están definidos
         if (datosConfiguracion != null && datosConfiguracion.Count > 0)
         {
            // comprobamos si están definidos y tienen valores los campos obligatorios (ejecutable, opciones y extensión por defecto)
            if (!string.IsNullOrEmpty(ejecutable) && !string.IsNullOrEmpty(opciones) && !string.IsNullOrEmpty(extensión))
            {
               // los campos obligatorios (ejecutable, opciones y extensión por defecto) están definidos -> comprobaremos si son iguales a los que tenemos almacenados
               if (ejecutable == datosConfiguracion[sDatosTablaConfiguracion.colEjecutable] && opciones == datosConfiguracion[sDatosTablaConfiguracion.colOpciones] &&
                   opcionListaFicheros == datosConfiguracion[sDatosTablaConfiguracion.colOpcionListaFicheros] && extensión == datosConfiguracion[sDatosTablaConfiguracion.colExtensionPorDefecto])
                  // son iguales -> el botón Aceptar no se activará
                  return false;
               else
                  // son distintos -> el botón Aceptar se activará
                  return true;
            }
            else
               return false;
         }
         else
            // no están definidos -> no se activará el botón Aceptar
            return false;
      }

      // botón que activa o desactiva el botón Aceptar si se cumple algunas series de comprobaciones
      private void activarBotonAceptar()
      {
         buttonAceptar.Enabled = hayQueActivarBotonAceptar();
      }

      // función que comprueba si se ha introducido el carácter punto que separa el nombre del fichero de su extensión
      private string extensiónSinPunto(string extensión)
      {
         // comprueba si la extensión comienza por punto
         if (extensión.StartsWith('.'))
            // si comienza por punto, la elimina
            return extensión.Substring(1);
         else
            return extensión;
      }

      /****** MANEJADORES DE LA VENTANA ******/
      /***************************************/
      // al cargar la ventana
      private void FormConfiguracionProgramaBackup_Load(object sender, EventArgs e)
      {
         // desactivamos la funcionalidad de la ventana principal
         if (ventanaPrincipal != null)
            ventanaPrincipal.Enabled = false;
      }

      // al cierre de la ventana
      private void FormConfiguracionProgramaBackup_FormClosed(object sender, FormClosedEventArgs e)
      {
         // activamos la funcionalidad de la ventana principal
         if (ventanaPrincipal != null)
            ventanaPrincipal.Enabled = true;
      }

      // al mostrar la ventana por primera vez
      private void FormConfiguracionProgramaBackup_Shown(object sender, EventArgs e)
      {
         // comprobamos si está definido el gestor de la base de datos
         if (gestorBD == null)
         {
            // no está definido -> mostraremos un cuadro de diálogo indicándolo y cerraremos la ventana
            MessageBox.Show("No se puede configurar el programa de Backup.\r\nEl gestor de la base de datos no está definido.", "ERROR INTERNO", MessageBoxButtons.OK,
                            MessageBoxIcon.Stop);
            Close();
            return;
         }
         if (!inicializarSelectorFicheros())
         {
            Close();
            return;
         }
         if (!inicializarTextBoxes())
         {
            Close();
            return;
         }
         if (!inicializarVentana())
         {
            Close();
            return;
         }
         activarBotonAceptar();
      }

      // al pulsar el botón "Aceptar"
      private void buttonAceptar_Click(object sender, EventArgs e)
      {
         // obtenemos los valores introducidos en el texto
         string ejecutable = textBoxNombreEjecutable.Text;                                // ejecutable
         string opciones = textBoxOpciones.Text;                                          // opciones
         string opcionesListaFicheros = textBoxOpcionListaFicheros.Text;                  // opción de lista de ficheros
         string extensión = extensiónSinPunto(textBoxExtensiónPorDefecto.Text);           // extensión por defecto
         textBoxExtensiónPorDefecto.Text = extensión;
         // operaciones con la base de datos
         gestorBD.crearTransaccion();                                                     // creamos la transacción
         if (!gestorBD.resultado)
         {
            MessageBox.Show(gestorBD.mensajeError, "ERROR AL CREAR TRANSACCIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
         }
         gestorBD.borrarDatosTablaConfiguracion();                                        // borramos el contenido actual de la tabla de configuración
         if (!gestorBD.resultado)
         {
            MessageBox.Show(gestorBD.mensajeError, "ERROR AL BORRAR DATOS ANTIGUOS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            gestorBD.anularTransaccion();
            if (!gestorBD.resultado)
               MessageBox.Show(gestorBD.mensajeError, "ERROR AL ANULAR TRANSACCIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
         }
         gestorBD.almacenarDatosConfiguracionProgramaCopiaSeguridad(ejecutable, opciones, opcionesListaFicheros, extensión);
         if (gestorBD.resultado)
         {
            gestorBD.aceptarTransaccion();
            if (!gestorBD.resultado)
               MessageBox.Show(gestorBD.mensajeError, "ERROR AL DAR POR VÁLIDA LA TRANSACCIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            Close();
         }
         else
         {
            MessageBox.Show(gestorBD.mensajeError, "ERROR AL ALMACENAR LOS DATOS DE CONFIGURACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            gestorBD.anularTransaccion();
            if (!gestorBD.resultado)
               MessageBox.Show(gestorBD.mensajeError, "ERROR AL ANULAR TRANSACCIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
         }
      }

      // al pulsar el botón "Cancelar"
      private void buttonCancelar_Click(object sender, EventArgs e)
      {
         Close();
      }

      // al cambiar los textos en los textboxes de la ventana
      private void cambiarTexto(object sender, EventArgs e)
      {
         // activamos o desactivamos el botón de Aceptar
         activarBotonAceptar();
      }

      private void buttonExaminar_Click(object sender, EventArgs e)
      {
         switch (openFileDialogSelectorEjecutables.ShowDialog())
         {
            case DialogResult.OK:
               textBoxNombreEjecutable.Text = openFileDialogSelectorEjecutables.FileName;
               break;
            case DialogResult.Cancel:
               break;
            default:
               MessageBox.Show("Se ha leído una opción desconocida para el selector de ficheros.", "ERROR INTERNO DE LA APLICACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Stop);
               break;
         }
      }

      // se ha pulsado el botón "Valores por defecto"

      private void buttonValoresPorDefecto_Click(object sender, EventArgs e)
      {
         textBoxNombreEjecutable.Text = ClassGestorBD.sValoresPorDefectoTablaConfiguracion.ejecutable;
         textBoxOpciones.Text = ClassGestorBD.sValoresPorDefectoTablaConfiguracion.opciones;
         textBoxOpcionListaFicheros.Text = ClassGestorBD.sValoresPorDefectoTablaConfiguracion.opcionListaFicheros;
      }
   }
}
