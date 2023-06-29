using System.Data;
using DBBitBossSolutions;

namespace GeneradorBackups
{
   public partial class FormPrincipal : Form
   {
      /****** VARIABLES DE LA CLASE ******/
      /***********************************/
      private ClassGestorBD? gestorBD;
      // 
      /****** PROPIEDADES DE LA CLASE ******/
      /*************************************/
      // datosConfiguracion -> Datos de configuraci�n de la aplicaci�n
      public ClassSQLServerOptions? datosConfiguracion
      {
         get; private set; 
      }

      // gestorFC -> Gestor del fichero de configuraci�n
      public ClassGestionFicheroConfiguracion? gestorFC
      {
         get; private set;
      }

      /******* M�TODOS DE LA CLASE ******/
      /**********************************/
      // activar o desactivar las opciones del programa dependiendo de si hemos le�do el fichero de configuraci�n de la aplicaci�n. No se le pasa nada como par�metro.
      // Devuelve un booleano indicando si se han activado o no las opciones del programa.
      private bool activarOpcionesProgramas()
      {
         bool onoff = (datosConfiguracion == null) ? false : true;
         rbConfiguracionProgramaBackup.Enabled = onoff;
         rbConfiguracionAplicacion.Enabled = true;
         if (rbCopiaSeguridad != null)
            rbCopiaSeguridad.Enabled = onoff;
         else
            return false;
         if (!onoff)
            rbConfiguracionAplicacion.Checked = true;
         return onoff;
      }

      // funci�n que comprueba si existe la base de datos de la aplicaci�n.
      private void comprobarExistenciaBDAplicacion()
      {
         if (gestorBD != null)
         {
            gestorBD.verificarExistenciaDirectorioDatosAplicacion();
            if (!gestorBD.resultado)
            {
               MessageBox.Show(gestorBD.mensajeError, "ERROR AL VERIFICAR EXISTENCIA DIRECTORIO DE DATOS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
               Close();
               return;
            }
            gestorBD.abrirConexion(false);
            if (!gestorBD.resultado)
            {
               MessageBox.Show(gestorBD.mensajeError, "ERROR AL CONECTARNOS CON EL MOTOR DE LA BASE DE DATOS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
               Close();
               return;
            }
            bool existeBD = gestorBD.existeBaseDatos();
            if (gestorBD.resultado)
            {
               if (!existeBD)
               {
                  gestorBD.crearBaseDatos();
                  if (!gestorBD.resultado)
                  {
                     MessageBox.Show(gestorBD.mensajeError, "ERROR AL CREAR BASE DE DATOS",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                     Close();
                     return;
                  }
               }
            }
            else
            {
               MessageBox.Show(gestorBD.mensajeError, "ERROR AL COMPROBAR EXISTENCIA BASE DE DATOS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
               Close();
               return;
            }
         }
         else
         {
            MessageBox.Show("No se ha podido comprobar la existencia de la base de datos de la aplicaci�n porque no est� definido el gestor de la base de datos de la aplicaci�n",
                            "ERROR AL COMPROBAR EXISTENCIA BASE DE DATOS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            Close();
            return;
         }
      }

      // funci�n que comprueba si existe (y si no existe, la crea) la tabla de Configuraci�n de la base de datos de la aplicaci�n
      private bool comprobarExistenciaTablaConfiguracion()
      {
         if (gestorBD != null)
         {
            bool existeTabla = gestorBD.existeTablaConfiguracion();
            if (gestorBD.resultado)
            {
               if (existeTabla)
               {
                  estadoTablaConfiguracion estadoTablaConfiguracion = gestorBD.verificarEstadoTablaConfiguracion();
                  if (gestorBD.resultado)
                  {
                     switch (estadoTablaConfiguracion)
                     {
                        case estadoTablaConfiguracion.Ok:                                          // ok -> Nada
                           break;
                        case estadoTablaConfiguracion.FaltanDatos:                                 // faltan datos -> almacenaremos los datos por defecto
                           gestorBD.almacenarValoresPorDefectoEnTablaConfiguracion();
                           if (!gestorBD.resultado)
                           {
                              MessageBox.Show(gestorBD.mensajeError, "ERROR AL ALMACENAR CONFIGURACI�N POR DEFECTO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                              return false;
                           }
                           break;
                        case estadoTablaConfiguracion.SobranDatos:                                 // sobran datos -> borraremos lo que hubiera y establecer�amos los valores por defecto
                           MessageBox.Show("Sobran datos en la tabla Configuraci�n.\r\nLos datos ser�n eliminados y se crear� una nueva configuraci�n con unos valores por defecto.",
                                           "ERROR EN LA TABLA CONFIGURACI�N", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                           gestorBD.borrarDatosTablaConfiguracion();
                           if (gestorBD.resultado)
                           {
                              gestorBD.almacenarValoresPorDefectoEnTablaConfiguracion();
                              if (!gestorBD.resultado)
                              {
                                 MessageBox.Show(gestorBD.mensajeError, "ERROR AL ALMACENAR CONFIGURACI�N POR DEFECTO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                 return false;
                              }
                           }
                           else
                           {
                              MessageBox.Show(gestorBD.mensajeError, "ERROR AL BORRAR LOS DATOS DE LA TABLA CONFIGURACI�N", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                              return false;
                           }
                           break;
                        default:                                                                   // otros valores
                           MessageBox.Show("Se ha le�do un estado de la tabla de configuraci�n desconocido.", "ERROR INTERNO DE LA APLICACI�N", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                           return false;
                     }
                     return true;
                  }
                  else
                  {
                     MessageBox.Show(gestorBD.mensajeError, "ERROR AL VERIFICAR EL ESTADO DE LA TABLA CONFIGURACI�N", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                     return false;
                  }
               }
               else
               {
                  gestorBD.crearTablaConfiguracion();
                  if (gestorBD.resultado)
                  {
                     gestorBD.almacenarValoresPorDefectoEnTablaConfiguracion();
                     if (!gestorBD.resultado)
                     {
                        MessageBox.Show(gestorBD.mensajeError, "ERROR AL ALMACENAR CONFIGURACI�N POR DEFECTO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                     }
                     else
                        return true;
                  }
                  else
                  {
                     MessageBox.Show(gestorBD.mensajeError, "ERROR AL CREAR LA TABLA DE CONFIGURACI�N", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                     return false;
                  }
               }
            }
            else
            {
               MessageBox.Show(gestorBD.mensajeError, "ERROR AL COMPROBAR LA EXISTENCIA DE LA TABLA CONFIGURACI�N", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
               return false;
            }
         }
         else
            return false;
      }

      // funci�n que comprueba si existe (y si no existe, la crea) la tabla donde se almacenan los datos de las copias de seguridad.
      private bool comprobarExistenciaTablaDatosCS()
      {
         if (gestorBD != null)
         {
            bool existeTabla = gestorBD.existeTablaDatosCS();
            if (gestorBD.resultado)
            {
               if (existeTabla)
                  return true;
               else
               {
                  gestorBD.crearTablaDatosCS();
                  if (gestorBD.resultado)
                     return true;
                  else
                  {
                     MessageBox.Show(gestorBD.mensajeError, "ERROR AL CREAR LA TABLA DE LOS DATOS DE LOS BACKUPS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                     return false;
                  }
               }
            }
            else
            {
               MessageBox.Show(gestorBD.mensajeError, "ERROR AL COMPROBAR LA EXISTENCIA DE LA TABLA DE LOS DATOS DE LOS BACKUPS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
               return false;
            }
         }
         else
            return false;
      }

      // funci�n que comprueba si existe (y si no existe, la crea) la tabla donde se almacenan los ficheros que componen la copia de seguridad
      private bool comprobarExistenciaTablaFicheros()
      {
         if (gestorBD != null)
         {
            bool existeTabla = gestorBD.existeTablaFicheros();
            if (gestorBD.resultado)
            {
               if (existeTabla)
                  return true;
               else
               {
                  gestorBD.crearTablaFicheros();
                  if (gestorBD.resultado)
                     return true;
                  else
                  {
                     MessageBox.Show(gestorBD.mensajeError, "ERROR AL CREAR LA TABLA DE LOS FICHEROS DEL BACKUP", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                     return false;
                  }
               }
            }
            else
            {
               MessageBox.Show(gestorBD.mensajeError, "ERROR AL COMPROBAR LA EXISTENCIA DE LA TABLA DE LOS FICHEROS DEL BACKUP", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
               return false;
            }
         }
         else
            return false;
      }

      // funci�n que anula una transacci�n
      private void anularTransaccion()
      {
         if (gestorBD != null)
         {
            gestorBD.anularTransaccion();
            if (!gestorBD.resultado)
               MessageBox.Show(gestorBD.mensajeError, "ERROR AL ANULAR UNA TRANSACCI�N DE LA BASE DE DATOS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
         }
      }

      // funci�n que acepta una transacci�n
      private void aceptarTransaccion()
      {
         if (gestorBD != null)
         {
            gestorBD.aceptarTransaccion();
            if (!gestorBD.resultado)
               MessageBox.Show(gestorBD.mensajeError, "ERROR AL ACEPTAR UNA TRANSACCI�N DE LA BASE DE DATOS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
         }
      }


      // funci�n que comprueba y/o crea las tablas que componen la base de datos
      private bool verificaYCreaTablasBD()
      {
         if (gestorBD != null)
         {
            // en primer lugar, crearemos la transacci�n encargada de crear e inicializar las tablas de la base de datos
            gestorBD.crearTransaccion();
            if (gestorBD.resultado)
            {
               if (!comprobarExistenciaTablaConfiguracion())
               {
                  anularTransaccion();
                  return false;
               }
               if (!comprobarExistenciaTablaDatosCS())
               {
                  anularTransaccion();
                  return false;
               }
               if (!comprobarExistenciaTablaFicheros())
               {
                  anularTransaccion();
                  return false;
               }
               aceptarTransaccion();
               return true;
            }
            else
            {
               MessageBox.Show(gestorBD.mensajeError, "ERROR AL CREAR UNA TRANSACCI�N DE LA BASE DE DATOS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
               return false;
            }
         }
         else
         {
            MessageBox.Show("No se puede verificar y crear las tablas de la base datos porque el gestor de la base de datos no est� definido.",
                            "GESTOR DE BASE DE DATOS INDEFINIDO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Close();
            return false;
         }
      }

      // manejador del cierre de ventana de las opciones de la aplicaci�n
      private void manejadorCierreVentana_OpcionesAplicacion(object sender, EventArgs eventArgs)
      {
         if (sender is FormConfiguracionAplicacion)
         {
            FormConfiguracionAplicacion? ventana = sender as FormConfiguracionAplicacion;
            if (ventana != null && !ventana.operacionCancelada)
            {
               datosConfiguracion = ventana.datosConfiguracion;
               if (gestorFC != null)
               {
                  gestorFC.generarFicheroConfiguracion(datosConfiguracion);
                  if (gestorFC.resultado)
                  {
                     gestorBD = new ClassGestorBD(datosConfiguracion);
                     if (!gestorBD.resultado)
                     {
                        MessageBox.Show(gestorBD.mensajeError, "ERROR DE CONEXI�N", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Close();
                        return;
                     }
                     if (activarOpcionesProgramas())
                     {
                        comprobarExistenciaBDAplicacion();
                        gestorBD.abrirConexion();
                        if (!gestorBD.resultado)
                        {
                           MessageBox.Show(gestorBD.mensajeError, "ERROR AL ABRIR CONEXI�N CON LA BBDD", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                           Close();
                           return;
                        }
                        if (!verificaYCreaTablasBD())
                        {
                           Close();
                           return;
                        }
                     }
                  }
                  else
                  {
                     MessageBox.Show(gestorFC.mensajeError, "ERROR AL CREAR FICHERO DE CONFIGURACI�N DEL PROGRAMA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  }
               }
               else
               {
                  MessageBox.Show("Gestor de fichero de configuraci�n sin definir.", "ERROR INTERNO DE LA APLICACI�N", MessageBoxButtons.OK, MessageBoxIcon.Error);
                  Close();
               }
            }
         }
      }

      /******* CONSTRUCTORES DE LA CLASE ******/
      /****************************************/
      // por defecto
      public FormPrincipal()
      {
         InitializeComponent();
         datosConfiguracion = null;
         gestorFC = null;
         gestorBD = null;
      }


      /******* MANEJADORES DEL FORMULARIO ******/
      /*****************************************/
      // al cerrar el formulario
      private void FormPrincipal_FormClosed(object sender, FormClosedEventArgs e)
      {
         if (gestorBD != null)
         {
            gestorBD.cerrarConexion();
            if (!gestorBD.resultado)
            {
               MessageBox.Show(gestorBD.mensajeError,"ERROR DE CONEXI�N", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
         }
      }
      // se ha pulsado el bot�n "Acerca de ..."
      private void buttonAcercaDe_Click(object sender, EventArgs e)
      {
         FormAcercaDe ventana = new();
         ventana.ventanaPrincipal = this;
         ventana.ShowDialog();
      }

      // al mostrar la ventana
      private void FormPrincipal_Shown(object sender, EventArgs e)
      {
         // establecemos la opci�n por defecto a Copia de Seguridad
         if (rbCopiaSeguridad != null)
         {
            rbCopiaSeguridad.Checked = true;
            gestorFC = new ClassGestionFicheroConfiguracion();
            if (gestorFC != null)
            {
               if (gestorFC.resultado)
               {
                  datosConfiguracion = gestorFC.leerFicheroConfiguracion();
                  if (gestorFC.resultado)
                  {
                     if (datosConfiguracion != null)
                     {
                        gestorBD = new ClassGestorBD(datosConfiguracion);
                        if (gestorBD.resultado)
                        {
                           gestorBD.abrirConexion();
                           if (gestorBD.resultado)
                           {
                              if (activarOpcionesProgramas())
                              {
                                 comprobarExistenciaBDAplicacion();
                                 gestorBD.abrirConexion();
                                 if (!gestorBD.resultado)
                                 {
                                    MessageBox.Show(gestorBD.mensajeError, "ERROR AL ABRIR CONEXI�N CON LA BBDD", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    Close();
                                    return;
                                 }
                                 if (!verificaYCreaTablasBD())
                                 {
                                    Close();
                                    return;
                                 }
                              }
                           }
                           else
                           {
                              MessageBox.Show(gestorBD.mensajeError, "ERROR DE CONEXI�N", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                              Close();
                           }
                        }
                        else
                        {
                           MessageBox.Show(gestorBD.mensajeError, "ERROR DE CONEXI�N", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                           Close();
                        }
                     }
                     else
                        activarOpcionesProgramas();
                  }
                  else
                  {
                     MessageBox.Show(gestorFC.mensajeError, "ERROR AL LEER FICHERO DE CONFIGURACI�N", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                     Close();
                  }
               }
               else
               {
                  MessageBox.Show(gestorFC.mensajeError, "ERROR AL CREAR EL GESTOR DEL FICHERO DE CONFIGURACI�N", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                  Close();
               }
            }
         }
      }

      // se ha pulsado el bot�n Salir
      private void buttonSalir_Click(object sender, EventArgs e)
      {
         Close();
      }

      // se ha pulsado el bot�n Ejecutar
      private void buttonEjecutar_Click(object sender, EventArgs e)
      {
         // comprobamos la opci�n seleccionada
         if (rbConfiguracionProgramaBackup.Checked)
         {
            FormConfiguracionProgramaBackup ventana = new();
            ventana.ventanaPrincipal = this;
            ventana.gestorBD = gestorBD;
            ventana.Show();
         }
         else if (rbConfiguracionAplicacion.Checked)
         {
            FormConfiguracionAplicacion ventana = new();
            ventana.ventanaPrincipal = this;
            ventana.datosConfiguracion = datosConfiguracion;
            ventana.FormClosed += manejadorCierreVentana_OpcionesAplicacion;
            ventana.Show();
         }
         else if (rbCopiaSeguridad.Checked)
         {
            FormCopiaSeguridad ventana = new();
            ventana.ventanaPrincipal = this;
            ventana.gestorBD = gestorBD;
            ventana.Show();
         }
         else
            MessageBox.Show("Opci�n desconocida", "ERROR INTERNO DE LA APLICACI�N", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
   }
}