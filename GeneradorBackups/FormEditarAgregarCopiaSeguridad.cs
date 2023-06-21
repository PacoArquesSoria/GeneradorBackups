using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FSBitBossSolutions;

namespace GeneradorBackups
{
   /****** ENUMERACIONES ******/
   /***************************/
   public enum modoEdicion : byte                                                                             // modo de edición
   {
      AgregarCS = 0,                                                          // agregar copia de seguridad
      EditarCS,                                                               // editar copia de seguridad
      Indefinido = 255                                                        // indefinido
   }

   /****** VENTANA DE EDICIÓN/AGREGAR COPIA DE SEGURIDAD ******/
   /***********************************************************/
   // ventana encargada de agregar o editar la copia de seguridad
   public partial class FormEditarAgregarCopiaSeguridad : Form
   {
      /****** CONSTANTES DE LA CLASE ******/
      /************************************/
      private const string kTitulo_ModoEdicionAgregarCS = "Nueva copia de seguridad";                          // título de la ventana en modo de edición agregar copia de seguridad
      private const string kTitulo_ModoEdicionEditarCS = "Editar copia de seguridad";                          // título de la ventana en modo de edición editar copia de seguridad
      private const string kBotonAgregarModificarFicheros_Agregar = "Agregar ficheros";                        // título del botón "Agregar/Modificar ficheros" en modo de edición agregar copia de seguridad
      private const string kBotonAgregarModificarFicheros_Modificar = "Modificar ficheros";                    // título del botón "Agregar/Modificar ficheros" en modo de edición editar copia de seguridad

      /****** VARIABLES DE LA CLASE ******/
      /***********************************/
      private bool pulsadoBotonAgregarModificarFicheros;                                                       // indica si se ha pulsado el botón "Agregar/Modificar ficheros"
      private string dirAntiguo;                                                                               // directorio antiguo
      private string dirAntesIniciar;                                                                          // directorio antes de iniciar la ventana

      /****** PROPIEDADES DE LA CLASE ******/
      /*************************************/
      // ventanaPadre -> Puntero a la ventana que invocó a esta ventana
      public FormCopiaSeguridad? ventanaPadre
      {
         set; private get;
      }

      // gestorBD -> Gestor al motor de la base de datos con la que trabajamos
      public ClassGestorBD? gestorBD
      {
         set; private get;
      }

      // operacionCancelada -> Booleano que nos dice si la operación ha sido cancelada o no
      public bool operacionCancelada
      {
         private set; get;
      }

      // identificador -> El identificador de la copia de seguridad que estamos editando o agregando
      public string? identificador
      {
         set; get;
      }

      // nombre -> El nombre relativo de la copia de seguridad que estamos editando o agregando
      public string? nombre
      {
         set; get;
      }

      // destino -> La ruta absoluta donde se almacenará la copia de seguridad
      public string? destino
      {
         set; get;
      }

      // descripcion -> La descripción de la copia de seguridad
      public string? descripcion
      {
         set; get;
      }

      // ficheros -> La lista de los ficheros cuya copia de seguridad queremos efectuar
      public List<string>? ficheros
      {
         set; get;
      }

      // modoEdicion -> El modo de edición de la ventana
      public modoEdicion modoEdicion
      {
         set; private get;
      }

      /****** CONSTRUCTORES DE LA CLASE ******/
      /***************************************/
      // por defecto
      public FormEditarAgregarCopiaSeguridad()
      {
         InitializeComponent();
         // inicializamos las propiedades de las ventanas
         ventanaPadre = null;
         gestorBD = null;
         operacionCancelada = true;
         identificador = null;
         nombre = null;
         destino = null;
         descripcion = null;
         ficheros = null;
         modoEdicion = modoEdicion.Indefinido;
         // inicializamos las variables de las ventanas
         pulsadoBotonAgregarModificarFicheros = false;
         dirAntiguo = "";
         dirAntesIniciar = "";
      }

      /****** MÉTODOS DE LA VENTANA ******/
      /***********************************/
      // función que activa o desactiva todos los widgets (excepto el de Cancelar). Como parámetro se le pasa un booleano que nos dice si hemos de activarlo (valor true) o
      // de desactivarlo (valor false). El valor por defecto será true.
      private void activarWidgets(bool onoff = true)
      {
         labelIdentificador.Enabled = onoff;
         textBoxIdentificador.Enabled = onoff;
         labelNombre.Enabled = onoff;
         textBoxNombre.Enabled = onoff;
         labelDestino.Enabled = onoff;
         textBoxDestino.Enabled = onoff;
         buttonExaminar.Enabled = onoff;
         labelDescripcion.Enabled = onoff;
         textBoxDescripcion.Enabled = onoff;
         buttonAceptar.Enabled = onoff;
         buttonAgregarModificarFicheros.Enabled = onoff;
         buttonCancelar.Enabled = true;
      }

      // función que genera el identificador de la copia de seguridad.
      private void generarIdentificadorCopiaSeguridad()
      {
         Process proceso = Process.GetCurrentProcess();                                            // obtenemos el proceso actual
         string pid = proceso.Id.ToString("D5");                                                   // obtenemos su identificador
         DateTime horaYFecha = DateTime.Now;                                                       // obtenemos la hora y fecha actual
         // generamos el identificador
         identificador = "CS" + pid + "-" + horaYFecha.Year.ToString("D4") + "-" + horaYFecha.Month.ToString("D2") + "-" + horaYFecha.Day.ToString("D2") + "-";
         identificador += horaYFecha.Hour.ToString("D2") + "-" + horaYFecha.Minute.ToString("D2") + "-" + horaYFecha.Second.ToString("D2") + "-";
         identificador += horaYFecha.Millisecond.ToString("D3");
      }

      // función que nos dice si hemos de activar el botón "Aceptar"
      private bool hayQueActivarBotonAceptar()
      {
         // en primer lugar, comprobaremos si los campos están definidos o no
         if (string.IsNullOrEmpty(textBoxNombre.Text) || string.IsNullOrEmpty(textBoxDestino.Text))
            // existe algún campo que no está definido -> no activaremos el botón "Aceptar" -> devolveremos false al procedimiento invocante
            return false;
         else
         {
            // no existe -> analizaremos en qué modo de edición estamos
            switch (modoEdicion)
            {
               case modoEdicion.AgregarCS:                                                         // modo de edición: agregar copia de seguridad
                  // no hará nada más -> devolverá true al procedimiento invocante
                  return true;
               case modoEdicion.EditarCS:                                                          // modo de edición: edición de una copia de seguridad
                  // comprobamos valores actuales con los previos
                  if (textBoxNombre.Text != nombre || textBoxDestino.Text != destino || textBoxDescripcion.Text != descripcion || pulsadoBotonAgregarModificarFicheros)
                     // existen diferencias -> devolverá true al procedimiento invocante
                     return true;
                  else
                     // no existen diferencias -> devolverá false al procedimiento invocante
                     return false;
               default:                                                                            // otro modo de edición: error interno de la aplicación
                  MessageBox.Show("Modo de edición desconocido en la función que comprueba si hay que activar el botón Aceptar.", "ERROR INTERNO DE LA APLICACIÓN",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                  activarWidgets(false);
                  return false;
            }
         }
      }

      // activamos o desactivamos el botón "Aceptar" si se dan ciertas circunstancias
      private void activarBotonAceptar()
      {
         buttonAceptar.Enabled = hayQueActivarBotonAceptar();
      }

      // establecemos el título de la ventana dependiendo del modo de edición en el que estemos
      private void establecerTituloVentana()
      {
         switch (modoEdicion)
         {
            case modoEdicion.AgregarCS:                                                            // modo de edición: agregar copia de seguridad
               Text = kTitulo_ModoEdicionAgregarCS;
               break;
            case modoEdicion.EditarCS:                                                             // modo de edición: editar copia de seguridad
               Text = kTitulo_ModoEdicionEditarCS;
               break;
         }
      }

      // establecemos la etiqueta del botón "Agregar/Modificar ficheros" dependiendo del modo de edición en el que estemos
      private void establecerEtiquetaBotonAgregarModificarFicheros()
      {
         switch (modoEdicion)
         {
            case modoEdicion.AgregarCS:                                                            // modo de edición: agregar copia de seguridad
               buttonAgregarModificarFicheros.Text = kBotonAgregarModificarFicheros_Agregar;
               break;
            case modoEdicion.EditarCS:                                                             // modo de edición: editar copia de seguridad
               buttonAgregarModificarFicheros.Text = kBotonAgregarModificarFicheros_Modificar;
               break;
         }
      }

      // inicializador del selector de directorios
      private bool inicializadorSelectorDirectorios()
      {
         if (gestorBD != null)
         {
            folderBrowserSelectorDestino.InitialDirectory = (string.IsNullOrEmpty(destino)) ? gestorBD.dirHome : destino;
            folderBrowserSelectorDestino.SelectedPath = (string.IsNullOrEmpty(destino)) ? gestorBD.dirHome : destino;
            return true;
         }
         else
            return false;
      }

      // función que obtiene los tamaños de los textboxes disponibles
      private (int tamIdentificador, int tamNombreCS, int tamDestino, int tamDescripcion) obtenerTamannoTextBoxes()
      {
         int tamIdentificador, tamNombreCS, tamDestino, tamDescripcion;
         if (gestorBD != null)
         {
            tamIdentificador = gestorBD.obtenerTamannoColumnaTabla(sDatosTablaDatosCS.nombreTabla, sDatosTablaDatosCS.colIdentificador);
            if (!gestorBD.resultado)
            {
               MessageBox.Show(gestorBD.mensajeError, "ERROR AL OBTENER EL TAMAÑO DE LOS CAMPOS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
               return (-1, -1, -1, -1);
            }
            tamNombreCS = gestorBD.obtenerTamannoColumnaTabla(sDatosTablaDatosCS.nombreTabla, sDatosTablaDatosCS.colNombre);
            if (!gestorBD.resultado)
            {
               MessageBox.Show(gestorBD.mensajeError, "ERROR AL OBTENER EL TAMAÑO DE LOS CAMPOS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
               return (-1, -1, -1, -1);
            }
            tamDestino = gestorBD.obtenerTamannoColumnaTabla(sDatosTablaDatosCS.nombreTabla, sDatosTablaDatosCS.colDestino);
            if (!gestorBD.resultado)
            {
               MessageBox.Show(gestorBD.mensajeError, "ERROR AL OBTENER EL TAMAÑO DE LOS CAMPOS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
               return (-1, -1, -1, -1);
            }
            tamDescripcion = gestorBD.obtenerTamannoColumnaTabla(sDatosTablaDatosCS.nombreTabla, sDatosTablaDatosCS.colDescripcion);
            if (!gestorBD.resultado)
            {
               MessageBox.Show(gestorBD.mensajeError, "ERROR AL OBTENER EL TAMAÑO DE LOS CAMPOS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
               return (-1, -1, -1, -1);
            }
            return (tamIdentificador, tamNombreCS, tamDestino, tamDescripcion);
         }
         else
            return (-1, -1, -1, -1);
      }

      // función que inicializa los texboxes de la ventana
      private bool inicializarTextBoxes()
      {
         int tamIdentificador, tamNombreCS, tamDestino, tamDescripcion;
         (tamIdentificador, tamNombreCS, tamDestino, tamDescripcion) = obtenerTamannoTextBoxes();
         if (tamIdentificador == -1 && tamNombreCS == -1 && tamDestino == -1 && tamDescripcion == -1)
            return false;
         else
         {
            textBoxIdentificador.MaxLength = tamIdentificador;
            textBoxNombre.MaxLength = tamNombreCS;
            textBoxDestino.MaxLength = tamDestino;
            textBoxDescripcion.MaxLength = tamDescripcion;
            return true;
         }
      }

      // función que escribe en pantalla el contenido de lo que le hemos pasado a la ventana
      private void inicializarVentana()
      {
         if (modoEdicion == modoEdicion.EditarCS)
         {
            textBoxIdentificador.Text = identificador;
            textBoxNombre.Text = nombre;
            textBoxDestino.Text = destino;
            textBoxDescripcion.Text = descripcion;
         }
      }

      // manejador del cierre de ventana de la ventana para agregar ficheros o directorios a la copia de seguridad
      private void manejadorCierreVentanaAgregarFicherosODirectoriosACS(object sender, EventArgs e)
      {
         if (sender is FormAgregarFicherosACScs)
         {
            FormAgregarFicherosACScs? ventana = sender as FormAgregarFicherosACScs;
            if (ventana != null && !ventana.operacionCancelada)
            {
               ficheros = ventana.ficherosSeleccionados;
               pulsadoBotonAgregarModificarFicheros = true;
               activarBotonAceptar();
            }
         }
      }

      /****** MANEJADORES DE LA VENTANA ******/
      /***************************************/
      // al cargar la ventana
      private void FormEditarAgregarCopiaSeguridad_Load(object sender, EventArgs e)
      {
         // desactivamos la ventana padre
         if (ventanaPadre != null)
            ventanaPadre.Enabled = false;
         // obtenemos el PWD original
         bool resultado;
         string mensajeError;
         dirAntesIniciar = ClassFileSystem.pwd(out resultado, out mensajeError);
         if (!resultado)
         {
            MessageBox.Show(mensajeError, "ERROR AL OBTENER EL PWD", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Close();
            return;
         }
      }

      // al cierre de la ventana
      private void FormEditarAgregarCopiaSeguridad_FormClosed(object sender, FormClosedEventArgs e)
      {
         // activamos la ventana padre
         if (ventanaPadre != null)
            ventanaPadre.Enabled = true;
         // restauramos el path anterior
         string mensajeError;
         if (!ClassFileSystem.chdir(dirAntesIniciar, out mensajeError))
            MessageBox.Show(mensajeError,"ERROR AL CAMBIAR DIRECTORIO",MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }

      // al mostrar por primera vez la ventana
      private void FormEditarAgregarCopiaSeguridad_Shown(object sender, EventArgs e)
      {
         if (modoEdicion == modoEdicion.Indefinido)
            activarWidgets(false);
         else
         {
            if (gestorBD == null)
            {
               MessageBox.Show("El gestor del motor de la base de datos de la aplicación no está definido.", "ERROR INTERNO DE LA APLICACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Error);
               Close();
               return;
            }
            if (!inicializarTextBoxes())
            {
               Close();
               return;
            }
            if (!inicializadorSelectorDirectorios())
            {
               activarWidgets(false);
               return;
            }
            activarWidgets();
            inicializarVentana();
            activarBotonAceptar();
            establecerTituloVentana();
            establecerEtiquetaBotonAgregarModificarFicheros();
            if (modoEdicion == modoEdicion.AgregarCS)
            {
               string mensajeError;
               if (!ClassFileSystem.cdhome(out mensajeError))
               {
                  MessageBox.Show(mensajeError, "ERROR AL CAMBIAR DIRECTORIO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                  Close();
                  return;
               }
            }
            else
            {
               if (modoEdicion == modoEdicion.EditarCS)
               {
                  string mensajeError;
                  if (!ClassFileSystem.chdir(textBoxDestino.Text, out mensajeError))
                  {
                     MessageBox.Show(mensajeError, "ERROR AL CAMBIAR DIRECTORIO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                     Close();
                     return;
                  }
               }
            }
         }
      }

      // se ha pulsado el botón "Aceptar"
      private void buttonAceptar_Click(object sender, EventArgs e)
      {
         // obtenemos los valores que hemos de devolver
         nombre = textBoxNombre.Text;
         descripcion = textBoxDescripcion.Text;
         destino = textBoxDestino.Text;
         descripcion = textBoxDescripcion.Text;
         if (modoEdicion == modoEdicion.AgregarCS)
            generarIdentificadorCopiaSeguridad();
         // indicamos que la operación no ha sido cancelada
         operacionCancelada = false;
         // finalmente, cerraremos la ventana
         Close();
      }

      // se ha pulsado el botón "Cancelar"
      private void buttonCancelar_Click(object sender, EventArgs e)
      {
         // se limita a cerrar la ventana
         Close();
      }

      // se ha cambiado el texto en la entrada del nombre de la copia de seguridad
      private void cambioTexto(object sender, EventArgs e)
      {
         // nos limitamos a activar o desactivar el botón "Aceptar"
         activarBotonAceptar();
      }

      // se ha pulsado el botón "Examinar"
      private void buttonExaminar_Click(object sender, EventArgs e)
      {
         if (gestorBD != null)
         {
            folderBrowserSelectorDestino.InitialDirectory = (string.IsNullOrEmpty(destino)) ? gestorBD.dirHome : destino;
            switch (folderBrowserSelectorDestino.ShowDialog())
            {
               case DialogResult.OK:                                                   // se ha seleccionado un directorio
                  textBoxDestino.Text = folderBrowserSelectorDestino.SelectedPath;
                  break;
               case DialogResult.Cancel:                                               // no se ha seleccionado nada
                  break;
               default:                                                                // otras opciones: error gordo
                  MessageBox.Show("Se ha leído una respuesta desconocida por el sistema.", "ERROR INTERNO DE LA APLICACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Error);
                  activarWidgets(false);
                  break;
            }
         }
      }

      // se ha pulsado el botón "Agregar/Modificar ficheros"
      private void buttonAgregarModificarFicheros_Click(object sender, EventArgs e)
      {
         // creamos la ventana de "Agregar/Modificar ficheros"
         FormAgregarFicherosACScs? ventana = new FormAgregarFicherosACScs();
         ventana.ventanaPadre = this;
         ventana.gestorBD = gestorBD;
         switch (modoEdicion)
         {
            case modoEdicion.AgregarCS:
               if (pulsadoBotonAgregarModificarFicheros)
                  ventana.ficherosSeleccionados = ficheros;
               else
                  ventana.ficherosSeleccionados = null;
               break;
            case modoEdicion.EditarCS:
               ventana.ficherosSeleccionados = ficheros;
               break;
         }
         ventana.ficherosSeleccionados = ficheros;
         ventana.FormClosed += manejadorCierreVentanaAgregarFicherosODirectoriosACS;
         ventana.Show();
      }

      // se ha validado el campo Destino
      private void textBoxDestino_Validated(object sender, EventArgs e)
      {
         string? nuevoDestino = textBoxDestino.Text;
         if (!string.IsNullOrEmpty(nuevoDestino))
         {
            if (ClassFileSystem.existeDirectorio(nuevoDestino))
            {
               bool resultado;
               string mensajeError;
               if (ClassFileSystem.chdir(nuevoDestino, out mensajeError))
               {
                  // se ha cambiado al nuevo directorio -> obtendremos el path absoluto del nuevo directorio
                  destino = ClassFileSystem.pwd(out resultado, out mensajeError);
                  if (resultado)
                  {
                     textBoxDestino.Text = destino;
                     dirAntiguo = destino;
                  }
                  else
                  {
                     activarWidgets(false);
                     MessageBox.Show(mensajeError, "ERROR AL OBTENER EL NOMBRE DE LA CARPETA ACTUAL DE TRABAJO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  }
               }
            }
            else
               textBoxDestino.Text = dirAntiguo;
         }
         // activamos o desactivamos el botón "Aceptar"
         activarBotonAceptar();
      }
   }
}
