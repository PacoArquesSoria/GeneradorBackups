using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DBBitBossSolutions;

namespace GeneradorBackups
{
   public partial class FormConfiguracionAplicacion : Form
   {
      /****** CONSTANTES DE LA CLASE ******/
      /************************************/
      private const string kEtiquetaBotonVerOcultarContrasenna_Ver = "Ver contraseña";                                  // etiqueta de los botones de ver/ocultar contraseña cuando hay que mostrar contraseña
      private const string kEtiquetaBotonVerOcultarContrasenna_Ocultar = "Ocultar contraseña";                          // etiqueta de los botones de ver/ocultar contraseña cuando hay que ocultar contraseña
      private struct kOpcionesSQLServerPorDefecto                                                                       // opciones SQL Server por opciones
      {
         public const int timeout = 120;                                                                                // timeout
         public const int commandTimeout = 120;                                                                         // commandTimeout
         public const bool azure = false;                                                                               // azure
         public const bool seguridadIntegrada = true;                                                                   // seguridad integrada
         public const bool conexionConfiada = false;                                                                    // conexión confiada
      }

      /****** VARIABLES DE LA CLASE ******/
      /***********************************/
      private bool verOcultar;                                                                                          // ver u ocultar contraseña

      /****** PROPIEDADES DE LA CLASE ******/
      /*************************************/
      // ventanaPrincipal -> Puntero a la ventana principal de la aplicación
      public FormPrincipal? ventanaPrincipal
      {
         private get; set; 
      }

      // datosConfiguracion -> Datos de la configuración
      public ClassSQLServerOptions? datosConfiguracion
      {
         get; set;
      }

      // operacionCancelada -> Indica si la operación ha sido cancelada o no
      public bool operacionCancelada
      {
         get; private set;
      }

      /****** MÉTODOS DE LA CLASE ******/
      /*********************************/
      // activamos o desactivamos el grupo "Login usuario SQL Server". Como parámetro le pasamos un booleano que nos indica si hemos de activar (valor true) o desactivar
      // (valor false) el grupo.
      private void activarGrupoLoginUsuarioSQLServer(bool onoff)
      {
         groupBoxUsuarioSQLServer.Enabled = onoff;
      }

      // establece la etiqueta del botón de ver/ocultar contraseña. No se le pasa nada como parámetro y no devuelve nada.
      private void establecerEtiquetaBotonVerOcultarContrasenna()
      {
         buttonVerOcultarContrasenna.Text = (verOcultar) ? kEtiquetaBotonVerOcultarContrasenna_Ver : kEtiquetaBotonVerOcultarContrasenna_Ocultar;
      }

      // indica si hemos de activar o desactivar el botón "Aceptar"
      private bool hayQueActivarBotonAceptar()
      {
         bool parteComun = !string.IsNullOrEmpty(textBoxServidor.Text) && !string.IsNullOrEmpty(textBoxTimeOut.Text) && !string.IsNullOrEmpty(textBoxCommandTimeOut.Text);
         if (parteComun)
         {
            if (cbAutenticacionSQLServer.Checked)
               return !string.IsNullOrEmpty(textBoxUsuario.Text) && !string.IsNullOrEmpty(textBoxContrasenna.Text);
            else
               return true;
         }
         else
            return false;
      }

      // función que lee una cadena de texto y la convierte en entero. Si es entero, verifica si es mayor o igual que 0. Como parámetro se le pasa la cadena de texto a convertir. Devuelve el número
      // si es válido o -1 en caso contrario.
      private int cadenaAEnteroPositivo(string texto)
      {
         if (string.IsNullOrEmpty(texto))
            return -1;
         else
         {
            int valor;
            if (int.TryParse(texto, out valor))
               // se ha convertido
               return (valor >= 0) ? valor : -1;
            else
               // no se ha convertido
               return -1;
         }
      }

      // función que lee del formulario las opciones del SQL Server. No se le pasa nada como parámetro. Devuelve la información obtenida.
      private ClassSQLServerOptions? obtenerOpcionesSQLServer()
      {
         ClassSQLServerOptions opciones = new ClassSQLServerOptions();
         opciones.servidor = textBoxServidor.Text;
         if (cbAutenticacionSQLServer.Checked)
         {
            opciones.usuario = textBoxUsuario.Text;
            opciones.contrasenna = textBoxContrasenna.Text;
         }
         string valor = textBoxTimeOut.Text;
         int timeout = cadenaAEnteroPositivo(valor);
         if (timeout == -1)
         {
            string mensaje = "No se ha podido convertir " + ((string.IsNullOrEmpty(valor)) ? "cadena vacía" : "la cadena " + valor) + " a entero mayor o igual que 0.";
            MessageBox.Show(mensaje, "ERROR EN TIMEOUT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return null;
         }
         else
            opciones.timeout = timeout;
         valor = textBoxCommandTimeOut.Text;
         int commandTimeout = cadenaAEnteroPositivo(valor);
         if (commandTimeout == -1)
         {
            string mensaje = "No se ha podido convertir " + ((string.IsNullOrEmpty(valor)) ? "cadena vacía" : "la cadena " + valor) + " a entero mayor o igual que 0.";
            MessageBox.Show(mensaje, "ERROR EN COMMANDTIMEOUT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return null;
         }
         else
            opciones.commandTimeout = commandTimeout;
         opciones.azure = cbAzure.Checked;
         opciones.seguridadIntegrada = cbSeguridadIntegrada.Checked;
         opciones.conexionConfiada = cbConexionConfiada.Checked;
         // finalmente, devolveremos lo obtenido
         return opciones;
      }

      // obtiene el nombre del ordenador local
      private string? obtenerNombreOrdenadorLocal()
      {
         return Environment.GetEnvironmentVariable("COMPUTERNAME");
      }

      // inicializa la ventana con los datos que hemos leído con anterioridad
      private void inicializarVentana()
      {
         if (datosConfiguracion != null)
         {
            textBoxServidor.Text = datosConfiguracion.servidor;
            cbAutenticacionSQLServer.Checked = datosConfiguracion.autenticacionSQLServer;
            if (cbAutenticacionSQLServer.Checked)
            {
               textBoxUsuario.Text = datosConfiguracion.usuario;
               textBoxContrasenna.Text = datosConfiguracion.contrasenna;
            }
            else
            {
               textBoxUsuario.Text = "";
               textBoxContrasenna.Text = "";
            }
            textBoxTimeOut.Text = datosConfiguracion.timeout.ToString();
            textBoxCommandTimeOut.Text = datosConfiguracion.commandTimeout.ToString();
            cbAzure.Checked = datosConfiguracion.azure;
            cbSeguridadIntegrada.Checked = datosConfiguracion.seguridadIntegrada;
            cbConexionConfiada.Checked = datosConfiguracion.conexionConfiada;
         }
         else
         {
            textBoxServidor.Text = obtenerNombreOrdenadorLocal();
         }
      }
      /****** CONSTRUCTORES DE LA CLASE ******/
      /***************************************/
      // por defecto
      public FormConfiguracionAplicacion()
      {
         InitializeComponent();
         ventanaPrincipal = null;
         datosConfiguracion = null;
         operacionCancelada = true;
         verOcultar = true;
      }

      /****** MANEJADORES DE LA VENTANA ******/
      /***************************************/
      // al cargar la ventana
      private void FormConfiguracionAplicacion_Load(object sender, EventArgs e)
      {
         // desactivamos la ventana principal de la aplicación
         if (ventanaPrincipal != null)
            ventanaPrincipal.Enabled = false;
      }

      // al cerrar la ventana
      private void FormConfiguracionAplicacion_FormClosed(object sender, FormClosedEventArgs e)
      {
         // activamos la ventana principal de la aplicación
         if (ventanaPrincipal != null)
            ventanaPrincipal.Enabled = true;
      }

      // al mostrar por primera vez el formulario
      private void FormConfiguracionAplicacion_Shown(object sender, EventArgs e)
      {
         cbAutenticacionSQLServer.Checked = false;
         activarGrupoLoginUsuarioSQLServer(false);
         establecerEtiquetaBotonVerOcultarContrasenna();
         inicializarVentana();
         // activa o desactiva el botón Aceptar dependiendo de sí se han introducido todos los datos
         buttonAceptar.Enabled = hayQueActivarBotonAceptar();
      }

      // se ha pulsado el botón Aceptar
      private void buttonAceptar_Click(object sender, EventArgs e)
      {
         ClassSQLServerOptions? datos = obtenerOpcionesSQLServer();
         if (datos != null)
         {
            datosConfiguracion = datos;
            operacionCancelada = false;
            Close();
         }
      }

      // se ha pulsado el botón Cancelar
      private void buttonCancelar_Click(object sender, EventArgs e)
      {
         // se limitará a cerrar la ventana ignorando lo que se ha introducido
         Close();
      }

      // se ha cambiado el estado de la "Autenticación SQL Server"
      private void cbAutenticacionSQLServer_CheckedChanged(object sender, EventArgs e)
      {
         // activamos o desactivamos el grupo "Login Usuario SQL Server"
         activarGrupoLoginUsuarioSQLServer(cbAutenticacionSQLServer.Checked);
         // activa o desactiva el botón Aceptar dependiendo de sí se han introducido todos los datos
         buttonAceptar.Enabled = hayQueActivarBotonAceptar();
      }

      // se ha pulsado el botón "Ver/Ocultar contraseña"
      private void buttonVerOcultarContrasenna_Click(object sender, EventArgs e)
      {
         verOcultar = !verOcultar;
         establecerEtiquetaBotonVerOcultarContrasenna();
         textBoxContrasenna.UseSystemPasswordChar = verOcultar;
      }

      // se han modificado las entradas de texto
      private void verificarEntradaTexto(object sender, EventArgs e)
      {
         // activa o desactiva el botón Aceptar dependiendo de sí se han introducido todos los datos
         buttonAceptar.Enabled = hayQueActivarBotonAceptar();
      }

      // se ha pulsado el botón "Valores por defecto"
      private void buttonValoresPorDefecto_Click(object sender, EventArgs e)
      {
         textBoxTimeOut.Text = kOpcionesSQLServerPorDefecto.timeout.ToString();
         textBoxCommandTimeOut.Text = kOpcionesSQLServerPorDefecto.commandTimeout.ToString();
         cbAzure.Checked = kOpcionesSQLServerPorDefecto.azure;
         cbSeguridadIntegrada.Checked = kOpcionesSQLServerPorDefecto.seguridadIntegrada;
         cbConexionConfiada.Checked = kOpcionesSQLServerPorDefecto.conexionConfiada;
      }
   }
}
