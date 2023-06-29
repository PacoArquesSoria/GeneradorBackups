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
      // datosConfiguracion -> Datos de configuración de la aplicación
      public ClassSQLServerOptions? datosConfiguracion
      {
         get; private set; 
      }

      // gestorFC -> Gestor del fichero de configuración
      public ClassGestionFicheroConfiguracion? gestorFC
      {
         get; private set;
      }

      /******* MÉTODOS DE LA CLASE ******/
      /**********************************/
      // activar o desactivar las opciones del programa dependiendo de si hemos leído el fichero de configuración de la aplicación. No se le pasa nada como parámetro.
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

      // función que comprueba si existe la base de datos de la aplicación.
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
            MessageBox.Show("No se ha podido comprobar la existencia de la base de datos de la aplicación porque no está definido el gestor de la base de datos de la aplicación",
                            "ERROR AL COMPROBAR EXISTENCIA BASE DE DATOS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            Close();
            return;
         }
      }

      // función que comprueba si existe (y si no existe, la crea) la tabla de Configuración de la base de datos de la aplicación
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
                              MessageBox.Show(gestorBD.mensajeError, "ERROR AL ALMACENAR CONFIGURACIÓN POR DEFECTO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                              return false;
                           }
                           break;
                        case estadoTablaConfiguracion.SobranDatos:                                 // sobran datos -> borraremos lo que hubiera y estableceríamos los valores por defecto
                           MessageBox.Show("Sobran datos en la tabla Configuración.\r\nLos datos serán eliminados y se creará una nueva configuración con unos valores por defecto.",
                                           "ERROR EN LA TABLA CONFIGURACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                           gestorBD.borrarDatosTablaConfiguracion();
                           if (gestorBD.resultado)
                           {
                              gestorBD.almacenarValoresPorDefectoEnTablaConfiguracion();
                              if (!gestorBD.resultado)
                              {
                                 MessageBox.Show(gestorBD.mensajeError, "ERROR AL ALMACENAR CONFIGURACIÓN POR DEFECTO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                 return false;
                              }
                           }
                           else
                           {
                              MessageBox.Show(gestorBD.mensajeError, "ERROR AL BORRAR LOS DATOS DE LA TABLA CONFIGURACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                              return false;
                           }
                           break;
                        default:                                                                   // otros valores
                           MessageBox.Show("Se ha leído un estado de la tabla de configuración desconocido.", "ERROR INTERNO DE LA APLICACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                           return false;
                     }
                     return true;
                  }
                  else
                  {
                     MessageBox.Show(gestorBD.mensajeError, "ERROR AL VERIFICAR EL ESTADO DE LA TABLA CONFIGURACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                        MessageBox.Show(gestorBD.mensajeError, "ERROR AL ALMACENAR CONFIGURACIÓN POR DEFECTO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                     }
                     else
                        return true;
                  }
                  else
                  {
                     MessageBox.Show(gestorBD.mensajeError, "ERROR AL CREAR LA TABLA DE CONFIGURACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                     return false;
                  }
               }
            }
            else
            {
               MessageBox.Show(gestorBD.mensajeError, "ERROR AL COMPROBAR LA EXISTENCIA DE LA TABLA CONFIGURACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
               return false;
            }
         }
         else
            return false;
      }

      // función que comprueba si existe (y si no existe, la crea) la tabla donde se almacenan los datos de las copias de seguridad.
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

      // función que comprueba si existe (y si no existe, la crea) la tabla donde se almacenan los ficheros que componen la copia de seguridad
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

      // función que anula una transacción
      private void anularTransaccion()
      {
         if (gestorBD != null)
         {
            gestorBD.anularTransaccion();
            if (!gestorBD.resultado)
               MessageBox.Show(gestorBD.mensajeError, "ERROR AL ANULAR UNA TRANSACCIÓN DE LA BASE DE DATOS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
         }
      }

      // función que acepta una transacción
      private void aceptarTransaccion()
      {
         if (gestorBD != null)
         {
            gestorBD.aceptarTransaccion();
            if (!gestorBD.resultado)
               MessageBox.Show(gestorBD.mensajeError, "ERROR AL ACEPTAR UNA TRANSACCIÓN DE LA BASE DE DATOS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
         }
      }


      // función que comprueba y/o crea las tablas que componen la base de datos
      private bool verificaYCreaTablasBD()
      {
         if (gestorBD != null)
         {
            // en primer lugar, crearemos la transacción encargada de crear e inicializar las tablas de la base de datos
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
               MessageBox.Show(gestorBD.mensajeError, "ERROR AL CREAR UNA TRANSACCIÓN DE LA BASE DE DATOS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
               return false;
            }
         }
         else
         {
            MessageBox.Show("No se puede verificar y crear las tablas de la base datos porque el gestor de la base de datos no está definido.",
                            "GESTOR DE BASE DE DATOS INDEFINIDO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Close();
            return false;
         }
      }

      // manejador del cierre de ventana de las opciones de la aplicación
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
                        MessageBox.Show(gestorBD.mensajeError, "ERROR DE CONEXIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Close();
                        return;
                     }
                     if (activarOpcionesProgramas())
                     {
                        comprobarExistenciaBDAplicacion();
                        gestorBD.abrirConexion();
                        if (!gestorBD.resultado)
                        {
                           MessageBox.Show(gestorBD.mensajeError, "ERROR AL ABRIR CONEXIÓN CON LA BBDD", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                     MessageBox.Show(gestorFC.mensajeError, "ERROR AL CREAR FICHERO DE CONFIGURACIÓN DEL PROGRAMA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  }
               }
               else
               {
                  MessageBox.Show("Gestor de fichero de configuración sin definir.", "ERROR INTERNO DE LA APLICACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
               MessageBox.Show(gestorBD.mensajeError,"ERROR DE CONEXIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
         }
      }
      // se ha pulsado el botón "Acerca de ..."
      private void buttonAcercaDe_Click(object sender, EventArgs e)
      {
         FormAcercaDe ventana = new();
         ventana.ventanaPrincipal = this;
         ventana.ShowDialog();
      }

      // al mostrar la ventana
      private void FormPrincipal_Shown(object sender, EventArgs e)
      {
         // establecemos la opción por defecto a Copia de Seguridad
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
                                    MessageBox.Show(gestorBD.mensajeError, "ERROR AL ABRIR CONEXIÓN CON LA BBDD", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                              MessageBox.Show(gestorBD.mensajeError, "ERROR DE CONEXIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                              Close();
                           }
                        }
                        else
                        {
                           MessageBox.Show(gestorBD.mensajeError, "ERROR DE CONEXIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                           Close();
                        }
                     }
                     else
                        activarOpcionesProgramas();
                  }
                  else
                  {
                     MessageBox.Show(gestorFC.mensajeError, "ERROR AL LEER FICHERO DE CONFIGURACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                     Close();
                  }
               }
               else
               {
                  MessageBox.Show(gestorFC.mensajeError, "ERROR AL CREAR EL GESTOR DEL FICHERO DE CONFIGURACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                  Close();
               }
            }
         }
      }

      // se ha pulsado el botón Salir
      private void buttonSalir_Click(object sender, EventArgs e)
      {
         Close();
      }

      // se ha pulsado el botón Ejecutar
      private void buttonEjecutar_Click(object sender, EventArgs e)
      {
         // comprobamos la opción seleccionada
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
            MessageBox.Show("Opción desconocida", "ERROR INTERNO DE LA APLICACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
   }
}