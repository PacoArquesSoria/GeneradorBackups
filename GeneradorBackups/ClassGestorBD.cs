using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBBitBossSolutions;

namespace GeneradorBackups
{
   /****** ENUMERACIONES ******/
   /***************************/
   public enum estadoTablaConfiguracion:byte                                               // estado de la tabla Configuración
   {
      Ok = 0,
      FaltanDatos,
      SobranDatos,
      Desconocido = 255
   }

   /****** CONSTANTES ******/
   /************************/
   public struct sDatosTablaConfiguracion                                              // datos de la tabla Configuración
   {
      public const string nombreTabla = "Configuración";                               // nombre de la tabla
      public const string colEjecutable = "Ejecutable";                                // columna ejecutable
      public const string colOpciones = "Opciones";                                    // columna opciones
      public const string colOpcionListaFicheros = "Opción lista ficheros";            // columna opción lista ficheros
   }
   public struct sDatosTablaDatosCS                                                    // datos de la tabla "Datos CS"
   {
      public const string nombreTabla = "Datos CS";                                    // nombre de la tabla
      public const string colIdentificador = "Identificador";                          // columna identificador
      public const string colNombre = "Nombre";                                        // columna nombre de la copia de seguridad
      public const string colDescripcion = "Descripción";                              // columna descripción
      public const string colDestino = "Destino";                                      // columna destino
      public const string colSeleccionada = "Seleccionada";                            // columna seleccionada
   }

   public struct sDatosTablaFicheros                                                   // datos de la tabla "Ficheros"
   {
      public const string nombreTabla = "Ficheros";                                    // nombre de la tabla
      public const string colIdentificador = "Identificador";                          // columna identificador
      public const string colNombreAbsoluto = "Nombre absoluto";                       // columna nombre absoluto del fichero
      public const string colIdentificadorCS = "Identificador CS";                     // columna del identificador de la copia de seguridad
   }

   /****** CLASE ClassGestorBD ******/
   /*********************************/
   // clase encargada de gestionar la base de datos de la aplicación
   public class ClassGestorBD
   {
      /****** CONSTANTES DE LA CLASE ******/
      /************************************/
      private const string kNombreBD = "BD del Generador de Backups";                     // nombre relativo de la base de datos
      private const string kNombreAplicacion = "Generador de Backups";                    // nombre relativo de la carpeta de ProgramData donde almacenaremos la base de datos

      public struct sValoresPorDefectoTablaConfiguracion                                 // valores por defecto de la tabla de configuración
      {
         public const string ejecutable = "winrar.exe";
         public const string opciones = "a -scf -m5 -ma5 -htb -r -tsm -tsc -tsa";
         public const string opcionListaFicheros = "@";
      }

      /****** VARIABLES DE LA CLASE ******/
      /***********************************/
      private ClassDataBase?  baseDatos;                                                  // base de datos de la aplicación

      /****** PROPIEDADES DE LA CLASE ******/
      /*************************************/
      // datosConfiguracion -> Datos de configuración de la base de datos
      public ClassSQLServerOptions? datosConfiguracion
      {
         set; private get;
      }

      // dirBaseDatos -> Directorio donde se almacenará la base de datos
      public string? dirBaseDatos
      {
         private set; get;
      }

      // nombreBD -> Nombre absoluto del fichero donde se almacena la base de datos
      public string? nombreBD
      {
         private set; get;
      }

      // dirTemporal -> Nombre absoluto del directorio temporal
      public string? dirTemporal
      {
         private set; get;
      }

      // dirHome -> Nombre absoluto del directorio HOME del usuario
      public string? dirHome
      {
         private set; get;
      }

      // resultado -> Devuelve el resultado de la operación
      public bool resultado
      {
         private set; get;
      }

      // mensajeError -> Devuelve el mensaje de error cometido
      public string? mensajeError
      {
         private set; get;
      }

      /****** CONSTRUCTORES DE LA CLASE ******/
      /***************************************/
      // por defecto. A partir de los datos de configuración de la base de datos
      public ClassGestorBD(ClassSQLServerOptions? _datosConfiguracion = null)
      {
         reset();
         datosConfiguracion = _datosConfiguracion;
         if (_datosConfiguracion == null)
         {
            baseDatos = null;
            dirBaseDatos = null;
            nombreBD = null;
            dirTemporal = null;
         }
         else
         {
            configConexion? configuracion = generarConfiguracionConexion();
            if (configuracion != null)
            {
               baseDatos = new ClassDataBase((configConexion)configuracion);
               if (_datosConfiguracion.autenticacionSQLServer)
               {
                  SecureString contrasenna = new SecureString();
                  if (!string.IsNullOrEmpty(_datosConfiguracion.contrasenna))
                     for (int i = 0; i < _datosConfiguracion.contrasenna.Length; ++i) contrasenna.AppendChar(_datosConfiguracion.contrasenna[i]);
                  SqlCredential credencial = new SqlCredential(_datosConfiguracion.usuario, contrasenna);
                  baseDatos.credenciales = credencial;
               }
               obtenerDatosBaseDatos();
               if (resultado)
                  obtenerDirectorioTemporal();
               if (resultado)
                  obtenerDirectorioHOMEDelUsuario();
            }
            else
            {
               baseDatos = null;
               resultado = false;
               mensajeError = "No se ha leído ninguna configuración para la base de datos.";
            }
         }
      }

      /****** MÉTODOS DE LA CLASE ******/
      /*********************************/
      // función que inicializa los valores de resultado y mensaje de error a sus valores por defecto
      private void reset()
      {
         resultado = true;
         mensajeError = "";
      }

      // función que obtiene el nombre absoluto de la base de datos y el directorio donde se ubica
      private void obtenerDatosBaseDatos()
      {
         dirBaseDatos = Environment.GetEnvironmentVariable("ProgramData");
         if (string.IsNullOrEmpty(dirBaseDatos))
         {
            resultado = false;
            mensajeError = "No está definida la variable de entorno ProgramData.";
         }
         else
         {
            dirBaseDatos += "\\" + kNombreAplicacion;                            // a dirBaseDatos le añadimos el nombre de la aplicación
            nombreBD = dirBaseDatos + "\\" + kNombreBD;                          // generamos el nombre absoluto de la base de datos
         }
      }

      // función que obtiene el directorio temporal de la aplicación
      private void obtenerDirectorioTemporal()
      {
         dirTemporal = Environment.GetEnvironmentVariable("TMP");                // leemos la variable de entorno TMP
         // comprobamos si la hemos podido leer
         if (string.IsNullOrEmpty(dirTemporal))
         {
            // no la hemos podido leer -> intentaremos leer la variable de entorno TEMP
            dirTemporal = Environment.GetEnvironmentVariable("TEMP");
            // comprobamos si está definida o no
            if (string.IsNullOrEmpty(dirTemporal))
            {
               // no lo está -> devolverá error
               resultado = false;
               mensajeError = "No se ha podido leer la variable de entorno que indica la carpeta de los ficheros temporales.";
            }
         }
      }

      // función que obtiene el directorio HOME del usuario
      private void obtenerDirectorioHOMEDelUsuario()
      {
         dirHome = Environment.GetEnvironmentVariable("USERPROFILE");            // leemos la variable de entorno USERPROFILE
         // comprobamos si lo hemos podido leer
         if (string.IsNullOrEmpty(dirHome))
         {
            // no lo está -> devolverá error
            resultado = false;
            mensajeError = "No se ha podido leer la variable de entorno que indica el directorio Home del usuario.";
         }
      }

      // función que comprueba si existe el directorio donde almacenaremos la base de datos. Si no existe, lo creará.
      public void verificarExistenciaDirectorioDatosAplicacion()
      {
         reset();
         try
         {
            if (dirBaseDatos != null)
            {
               // primeramente, comprobaremos si existe como fichero
               if (File.Exists(dirBaseDatos))
               {
                  // existe como fichero -> error
                  resultado = false;
                  mensajeError = "Existe un fichero ordinario llamado " + dirBaseDatos + ". No se puede usar como directorio. Para usar el programa, borre ese fichero.";
               }
               else
               {
                  // no existe como fichero -> verificaremos su existencia como directorio
                  if (!Directory.Exists(dirBaseDatos))
                  {
                     Directory.CreateDirectory(dirBaseDatos);
                  }
               }
            }
            else
               throw new Exception("La variable de entorno de la aplicación dirBaseDatos no está definida.");
         } catch (Exception excepcion)
         {
            resultado = false;
            mensajeError = "No se ha podido ni verificar la existencia del directorio de datos de la aplicación ni crearlo.\r\n";
            mensajeError += "Se ha producido el siguiente error:\r\n";
            mensajeError += excepcion.Message;
         }
      }

      // función que genera la configuración de conexión
      private configConexion? generarConfiguracionConexion()
      {
         if (datosConfiguracion == null)
            return null;
         else
         {
            configConexion configConexion = new configConexion();
            configConexion.nombreInstanciaMotorBaseDatos = (datosConfiguracion.servidor == null) ? "" : datosConfiguracion.servidor;
            configConexion.timeout = datosConfiguracion.timeout;
            configConexion.commandTimeout = datosConfiguracion.commandTimeout;
            configConexion.seguridadIntegrada = datosConfiguracion.seguridadIntegrada;
            configConexion.conexionConfiada = datosConfiguracion.conexionConfiada;
            return configConexion;
         }
      }

      // función que abre la conexión con el motor de la base de datos de la aplicación. Como parámetro se le pasa un booleano que nos indica si hemos de abrir la base de
      // datos de la aplicación (valor true) o si hemos de abrir la conexión vacía (valor false). El valor por defecto será true.
      public void abrirConexion(bool abrirBD = true)
      {
         reset();
         if (baseDatos != null)
         {
            string mError;
            bool resOperacion;
            if (abrirBD)
               resOperacion = baseDatos.abrirConexionBD(nombreBD, out mError);
            else
               resOperacion = baseDatos.abrirConexionBD(null, out mError);
            if (!resOperacion)
            {
               resultado = false;
               mensajeError = "No se ha podido abrir la conexión con el motor de la base de datos de la aplicación.\r\n";
               mensajeError += "Se ha producido el siguiente error: ";
               mensajeError += mError;
            }
         }
         else
         {
            resultado = false;
            mensajeError = "No se ha podido abrir la conexión con el motor de la base de datos de la aplicación porque no está definido su gestor.";
         }
      }

      // función que cierra la conexión con el motor de la base de datos de la aplicación
      public void cerrarConexion()
      {
         reset();
         if (baseDatos != null)
         {
            string mError;
            bool resOperacion;
            resOperacion = baseDatos.cerrarConexionBD(out mError);
            if (!resOperacion)
            {
               resultado = false;
               mensajeError = "No se ha podido cerrar la conexión con el motor de la base de datos de la aplicación.\r\n";
               mensajeError += "Se ha producido el siguiente error: ";
               mensajeError += mError;
            }
         }
         else
         {
            resultado = false;
            mensajeError = "No se ha podido cerrar la conexión con el motor de la base de datos de la aplicación porque no está definido su gestor.";
         }
      }

      // función que crea una transacción de la base de datos
      public void crearTransaccion()
      {
         reset();
         if (baseDatos != null)
         {
            string mError;
            if (!baseDatos.crearTransaccion(out mError))
            {
               resultado = false;
               mensajeError = "No se ha podido crear la transacción encargada de efectuar la pertinente operación.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += mError;
            }
         }
         else
         {
            resultado = false;
            mensajeError = "No se ha podido crear una transacción en la base de datos porque no está definido su gestor.";
         }
      }

      // función que da por buena una transacción de la base de datos
      public void aceptarTransaccion()
      {
         reset();
         if (baseDatos != null)
         {
            string mError;
            if (!baseDatos.aceptarTransaccion(out mError))
            {
               resultado = false;
               mensajeError = "No se ha podido dar por válida la transacción encargada de efectuar la pertinente operación.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += mError;
            }
         }
         else
         {
            resultado = false;
            mensajeError = "No se ha podido dar por válida una transacción en la base de datos porque no está definido su gestor.";
         }
      }

      // función que anula una transacción de la base de datos
      public void anularTransaccion()
      {
         reset();
         if (baseDatos != null)
         {
            string mError;
            if (!baseDatos.anularTransaccion(out mError))
            {
               resultado = false;
               mensajeError = "No se ha podido anular la transacción encargada de efectuar la pertinente operación.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += mError;
            }
         }
         else
         {
            resultado = false;
            mensajeError = "No se ha podido anular una transacción en la base de datos porque no está definido su gestor.";
         }
      }

      // función que comprueba la existencia de la base de datos
      public bool existeBaseDatos()
      {
         reset();
         if (baseDatos != null)
         {
            string mError;
            bool resOperacion,
                 existeBD = baseDatos.existeBaseDatos(nombreBD, out resOperacion, out mError);
            if (!resOperacion)
            {
               resultado = false;
               mensajeError = "No se ha podido comprobar la existencia de la base de datos de la aplicación.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += mError;
               return false;
            }
            else
               return existeBD;
         }
         else
         {
            resultado = false;
            mensajeError = "No se ha podido comprobar la existencia de la base de datos de la aplicación porque no está definido el gestor de la base de datos de la ";
            mensajeError += "aplicación.";
            return false;
         }
      }

      // función que crea la base de datos
      public void crearBaseDatos()
      {
         reset();
         if (baseDatos != null)
         {
            string mError;
            if (!baseDatos.crearBaseDeDatos(nombreBD, out mError))
            {
               resultado = false ;
               mensajeError = mError;
            }
         }
         else
         {
            resultado = false;
            mensajeError = "No se ha podido crear la base de datos de la aplicación porque no está definido el gestor de la base de datos de la ";
            mensajeError += "aplicación.";
         }
      }

      // función que nos dice si existe la tabla Configuración
      public bool existeTablaConfiguracion()
      {
         reset();
         if (baseDatos != null)
         {
            string mError;
            bool existeTabla;
            existeTabla = baseDatos.comprobarExistenciaTabla(sDatosTablaConfiguracion.nombreTabla, out mError);
            if (existeTabla)
               return true;
            else
            {
               if (string.IsNullOrEmpty(mError))
                  return false;
               else
               {
                  resultado = false;
                  mensajeError = "No se ha podido comprobar la existencia de la tabla Configuración.\r\n";
                  mensajeError += "Se ha producido el siguiente error:\r\n";
                  mensajeError += mError;
                  return false;
               }
            }
         }
         else
         {
            resultado = false;
            mensajeError = "No se ha podido comprobar la existencia de la tabla de Configuración porque no está definido el gestor de la base de datos de la aplicación.";
            return false;
         }
      }

      // función que crea la tabla Configuración
      public void crearTablaConfiguracion()
      {
         reset();
         if (baseDatos != null)
         {
            // creamos la tabla
            ClassCreateTable generadorCrearTabla = new ClassCreateTable(sDatosTablaConfiguracion.nombreTabla, motoresBD.SQLServer);
            if (!generadorCrearTabla.resultado)
            {
               resultado = false;
               mensajeError = "No se ha podido crear la tabla de Configuración.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += generadorCrearTabla.mensajeError;
               return;
            }
            generadorCrearTabla.annadirColumna(sDatosTablaConfiguracion.colEjecutable, "varchar(1024)", true, false);
            generadorCrearTabla.annadirColumna(sDatosTablaConfiguracion.colOpciones, "varchar(1024)", false, true);
            generadorCrearTabla.annadirColumna(sDatosTablaConfiguracion.colOpcionListaFicheros, "varchar(10)", false, true);
            generadorCrearTabla.generarSentenciaSQL();
            if (generadorCrearTabla.resultado)
            {
               string mError;
               if (!baseDatos.ejecutarSentenciaSQL(generadorCrearTabla.sentenciaSQL, out mError))
               {
                  resultado = false;
                  mensajeError = "No se ha podido crear la tabla de Configuración.\r\n";
                  mensajeError += "Se ha producido el siguiente error:\r\n";
                  mensajeError += mError;
               }
            }
            else
            {
               resultado = false;
               mensajeError = "No se ha podido crear la tabla de Configuración.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += generadorCrearTabla.mensajeError;
            }
         }
         else
         {
            resultado = false;
            mensajeError = "No se ha podido crear la tabla de Configuración porque no está definido el gestor de la base de datos de la aplicación.";
         }
      }

      // función que almacena los valores por defecto en la tabla Configuración
      public void almacenarValoresPorDefectoEnTablaConfiguracion()
      {
         reset();
         if (baseDatos != null)
         {
            // creamos el generador de la sentencia Insert
            ClassInsertValues generadorInsert = new ClassInsertValues(sDatosTablaConfiguracion.nombreTabla, motoresBD.SQLServer);
            if (!generadorInsert.resultado)
            {
               resultado = false;
               mensajeError = "No se ha podido almacenar los datos por defecto de la tabla Configuración.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += generadorInsert.mensajeError;
               return;
            }
            generadorInsert.annadirCampoValor(sDatosTablaConfiguracion.colEjecutable, ClassDataBase.prepararCadenaSentenciaSQL(sValoresPorDefectoTablaConfiguracion.ejecutable));
            generadorInsert.annadirCampoValor(sDatosTablaConfiguracion.colOpciones, ClassDataBase.prepararCadenaSentenciaSQL(sValoresPorDefectoTablaConfiguracion.opciones));
            generadorInsert.annadirCampoValor(sDatosTablaConfiguracion.colOpcionListaFicheros, ClassDataBase.prepararCadenaSentenciaSQL(sValoresPorDefectoTablaConfiguracion.opcionListaFicheros));
            generadorInsert.generarSentenciaSQL();
            if (!generadorInsert.resultado)
            {
               resultado = false;
               mensajeError = "No se ha podido almacenar los datos por defecto de la tabla Configuración.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += generadorInsert.mensajeError;
               return;
            }
            // finalmente, almacenamos los datos en la tabla
            string mError;
            if (!baseDatos.ejecutarSentenciaSQL(generadorInsert.sentenciaSQL, out mError))
            {
               resultado = false;
               mensajeError = "No se ha podido almacenar los datos por defecto de la tabla Configuración.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += mError;
            }
         }
         else
         {
            resultado = false;
            mensajeError = "No se ha podido almacenar los datos por defecto de la tabla Configuración porque no está definido el gestor de la base de datos de la ";
            mensajeError += "aplicación.";
         }
      }

      // función que borra los datos de la tabla Configuración
      public void borrarDatosTablaConfiguracion()
      {
         reset();
         if (baseDatos != null)
         {
            string mError;
            if (!baseDatos.borrarDatosTabla(sDatosTablaConfiguracion.nombreTabla, null, out mError))
            {
               resultado = false;
               mensajeError = "No se ha podido borrar los datos de la tabla Configuración.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += mError;
            }
         }
         else
         {
            resultado = false;
            mensajeError = "No se ha podido borrar los datos de la tabla de Configuración porque no está definido el gestor de la base de datos de la aplicación.";
         }
      }

      // función que nos dice el estado de la tabla Configuración
      public estadoTablaConfiguracion verificarEstadoTablaConfiguracion()
      {
         reset();
         if (baseDatos != null)
         {
            // generamos la sentencia SQL que nos permitirá comprobar el estado de la tabla de Configuración
            ClassSelect generadorSelect = new ClassSelect(motoresBD.SQLServer);
            generadorSelect.agregarTabla(sDatosTablaConfiguracion.nombreTabla);
            generadorSelect.agregarExpresion("count(*) as Total");
            generadorSelect.generarSentenciaSQL();
            if (generadorSelect.resultado)
            {
               bool resOperacion;
               string mError;
               SqlDataReader puntero = baseDatos.obtenerPunteroTablaDatos(generadorSelect.sentenciaSQL, out resOperacion, out mError);
               if (resOperacion)
               {
                  if (puntero.Read())
                  {
                     int total = (int)puntero["Total"];
                     puntero.Close();
                     switch (total)
                     {
                        case 0:                                         // no hay ningún dato almacenado en la tabla -> faltan datos
                           return estadoTablaConfiguracion.FaltanDatos;
                        case 1:                                         // hay un único dato -> todo ok
                           return estadoTablaConfiguracion.Ok;
                        default:                                        // hay más de un dato -> sobran datos
                           return estadoTablaConfiguracion.SobranDatos;
                     }
                  }
                  else
                  {
                     puntero.Close();
                     resultado = false;
                     mensajeError = "No se ha podido verificar el estado de la tabla de Configuración.\r\n";
                     mensajeError += "Se ha producido el siguiente error:\r\n";
                     mensajeError += "No se ha podido leer el número de filas de la tabla.";
                     return estadoTablaConfiguracion.Desconocido;
                  }
               }
               else
               {
                  resultado = false;
                  mensajeError = "No se ha podido verificar el estado de la tabla de Configuración.\r\n";
                  mensajeError += "Se ha producido el siguiente error:\r\n";
                  mensajeError += mError;
                  return estadoTablaConfiguracion.Desconocido;
               }
            }
            else
            {
               resultado = false;
               mensajeError = "No se ha podido verificar el estado de la tabla de Configuración.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += generadorSelect.mensajeError;
               return estadoTablaConfiguracion.Desconocido;
            }
         }
         else
         {
            resultado = false;
            mensajeError = "No se ha podido verificar el estado de la tabla de Configuración porque no está definido el gestor de la base de datos de la aplicación.";
            return estadoTablaConfiguracion.Desconocido;
         }
      }

      // función que nos dice si existe la tabla de los datos de la copia de seguridad
      public bool existeTablaDatosCS()
      {
         reset();
         if (baseDatos != null)
         {
            string mError;
            bool existeTabla;
            existeTabla = baseDatos.comprobarExistenciaTabla(sDatosTablaDatosCS.nombreTabla, out mError);
            if (existeTabla)
               return true;
            else
            {
               if (string.IsNullOrEmpty(mError))
                  return false;
               else
               {
                  resultado = false;
                  mensajeError = "No se ha podido comprobar la existencia de la tabla de los datos de la copia de seguridad.\r\n";
                  mensajeError += "Se ha producido el siguiente error:\r\n";
                  mensajeError += mError;
                  return false;
               }
            }
         }
         else
         {
            resultado = false;
            mensajeError = "No se ha podido comprobar la existencia de la tabla de los datos de la copia de seguridad porque no está definido el gestor de la base de ";
            mensajeError += "datos de la aplicación.";
            return false;
         }
      }

      // función que crea la tabla de los datos de la copia de seguridad
      public void crearTablaDatosCS()
      {
         reset();
         if (baseDatos != null)
         {
            ClassCreateTable generadorCreateTable = new ClassCreateTable(sDatosTablaDatosCS.nombreTabla, motoresBD.SQLServer);
            if (!generadorCreateTable.resultado)
            {
               resultado = false;
               mensajeError = "No se ha podido crear la tabla de los datos de la copia de seguridad.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += generadorCreateTable.mensajeError;
               return;
            }
            generadorCreateTable.annadirColumna(sDatosTablaDatosCS.colIdentificador, "varchar(40)", true, false);
            generadorCreateTable.annadirColumna(sDatosTablaDatosCS.colNombre, "varchar(100)", false, false);
            generadorCreateTable.annadirColumna(sDatosTablaDatosCS.colDescripcion, "varchar(max)", false, true);
            generadorCreateTable.annadirColumna(sDatosTablaDatosCS.colDestino, "varchar(1024)", false, false);
            generadorCreateTable.annadirColumna(sDatosTablaDatosCS.colSeleccionada,"bit",false,false);
            generadorCreateTable.generarSentenciaSQL();
            if (!generadorCreateTable.resultado)
            {
               resultado = false;
               mensajeError = "No se ha podido crear la tabla de los datos de la copia de seguridad.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += generadorCreateTable.mensajeError;
               return;
            }
            string mError;
            if (!baseDatos.ejecutarSentenciaSQL(generadorCreateTable.sentenciaSQL, out mError))
            {
               resultado = false;
               mensajeError = "No se ha podido crear la tabla de los datos de la copia de seguridad.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += mError;
               return;
            }
         }
         else
         {
            resultado = false;
            mensajeError = "No se ha podido crear la tabla de los datos de la copia de seguridad porque no está definido el gestor de la base de datos de la aplicación.";
         }
      }

      // función que nos dice si existe la tabla de los ficheros que se almacenarán en la copia de seguridad
      public bool existeTablaFicheros()
      {
         {
            reset();
            if (baseDatos != null)
            {
               string mError;
               bool existeTabla;
               existeTabla = baseDatos.comprobarExistenciaTabla(sDatosTablaFicheros.nombreTabla, out mError);
               if (existeTabla)
                  return true;
               else
               {
                  if (string.IsNullOrEmpty(mError))
                     return false;
                  else
                  {
                     resultado = false;
                     mensajeError = "No se ha podido comprobar la existencia de la tabla Ficheros.\r\n";
                     mensajeError += "Se ha producido el siguiente error:\r\n";
                     mensajeError += mError;
                     return false;
                  }
               }
            }
            else
            {
               resultado = false;
               mensajeError = "No se ha podido comprobar la existencia de la tabla de Ficheros porque no está definido el gestor de la base de datos de la aplicación.";
               return false;
            }
         }
      }

      // función que crea la tabla de los ficheros que componen la copia de seguridad
      public void crearTablaFicheros()
      {
         reset();
         if (baseDatos != null)
         {
            ClassCreateTable generadorCreateTable = new ClassCreateTable(sDatosTablaFicheros.nombreTabla, motoresBD.SQLServer);
            if (!generadorCreateTable.resultado)
            {
               resultado = false;
               mensajeError = "No se ha podido crear la tabla de los ficheros que componen la copia de seguridad.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += generadorCreateTable.mensajeError;
               return;
            }
            generadorCreateTable.annadirColumna(sDatosTablaFicheros.colIdentificador, "bigint", true, false);
            generadorCreateTable.annadirColumna(sDatosTablaFicheros.colNombreAbsoluto, "varchar(1024)",false, false);
            generadorCreateTable.annadirColumna(sDatosTablaFicheros.colIdentificadorCS, "varchar(40)", false, false);
            generadorCreateTable.generarSentenciaSQL();
            if (!generadorCreateTable.resultado)
            {
               resultado = false;
               mensajeError = "No se ha podido crear la tabla de los ficheros que componen la copia de seguridad.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += generadorCreateTable.mensajeError;
               return;
            }
            string mError;
            if (!baseDatos.ejecutarSentenciaSQL(generadorCreateTable.sentenciaSQL, out mError))
            {
               resultado = false;
               mensajeError = "No se ha podido crear la tabla de los ficheros que componen la copia de seguridad.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += mError;
               return;
            }
         }
         else
         {
            resultado = false;
            mensajeError = "No se ha podido crear la tabla de los ficheros que componen la copia de seguridad porque no está definido el gestor de la base de datos ";
            mensajeError += "de la aplicación.";
         }
      }

      // función que obtiene el tamaño de una columna de una tabla si ésta es de tipo char. Como parámetros se le pasan: el nombre de la tabla y el nombre de la columna.
      // Devuelve el tamaño de la columna.
      public int obtenerTamannoColumnaTabla(string? tabla, string? columna)
      {
         reset();
         if (baseDatos != null)
         {
            if (string.IsNullOrEmpty(tabla) && string.IsNullOrEmpty(columna))
            {
               resultado = false;
               mensajeError = "No se ha podido obtener el tamaño de la columna de la tabla.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += "No está definido el nombre de la tabla.\r\n";
               mensajeError += "No está definido el nombre de la columna.\r\n";
               return -1;
            }
            else if (string.IsNullOrEmpty(tabla))
            {
               resultado = false;
               mensajeError = "No se ha podido obtener el tamaño de la columna de la tabla.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += "No está definido el nombre de la tabla.\r\n";
               return -1;
            }
            else if (string.IsNullOrEmpty(columna))
            {
               resultado = false;
               mensajeError = "No se ha podido obtener el tamaño de la columna de la tabla.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += "No está definido el nombre de la columna.\r\n";
               return -1;
            }
            bool resOperacion;
            string mError;
            SqlDataReader puntero = baseDatos.obtenerPunteroDatosInformacionTablaBD(tabla, out resOperacion, out mError);
            if (resOperacion)
            {
               int tamanno = -2;
               // búsqueda y localización de la columna de la tabla
               while (puntero.Read() && tamanno == -2)
               {
                  string? col = (puntero[sBaseDatos.kTablaInformacionColumnasTabla_nombreColumna] != null) ? puntero[sBaseDatos.kTablaInformacionColumnasTabla_nombreColumna].ToString() : null;
                  if (!string.IsNullOrEmpty(col) && col == columna)
                     tamanno = (int)puntero[sBaseDatos.kTablaInformacionColumnasTabla_longitudMaximaEnBytes];
               }
               puntero.Close();
               if (tamanno == -2)
               {
                  resultado = false;
                  mensajeError = "No se ha podido obtener el tamaño de la columna de la tabla.\r\n";
                  mensajeError += "Se ha producido el siguiente error:\r\n";
                  mensajeError += "No se ha localizado la columna en la tabla.";
                  puntero.Close();
                  return -1;
               }
               else
                  return tamanno;
            }
            else
            {
               resultado = false;
               mensajeError = "No se ha podido obtener el tamaño de la columna de la tabla.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += mError;
               puntero.Close();
               return -1;
            }
         }
         else
         {
            resultado = false;
            mensajeError = "No se ha podido obtener el tamaño de la columna de la tabla porque no está definido el gestor de la base de datos.";
            return -1;
         }
      }

      // función que obtiene los datos de configuración del programa de copia de seguridad que emplearemos para efectuar las copias de seguridad
      public Dictionary<string, string?>? obtenerDatosConfiguracionProgramaCopiaSeguridad()
      {
         if (baseDatos != null)
         {
            ClassSelect generadorSelect = new ClassSelect(motoresBD.SQLServer);
            generadorSelect.agregarTabla(sDatosTablaConfiguracion.nombreTabla);
            generadorSelect.agregarExpresion(generadorSelect.prepararNombre(sDatosTablaConfiguracion.colEjecutable));
            generadorSelect.agregarExpresion(generadorSelect.prepararNombre(sDatosTablaConfiguracion.colOpciones));
            generadorSelect.agregarExpresion(generadorSelect.prepararNombre(sDatosTablaConfiguracion.colOpcionListaFicheros));
            generadorSelect.generarSentenciaSQL();
            if (generadorSelect.resultado)
            {
               bool resOperacion;
               string mError;
               SqlDataReader puntero = baseDatos.obtenerPunteroTablaDatos(generadorSelect.sentenciaSQL, out resOperacion, out mError);
               if (resOperacion)
               {
                  Dictionary<string, string?>? resultados = null;
                  if (puntero.Read())
                  {
                     resultados = new Dictionary<string, string?>();
                     resultados[sDatosTablaConfiguracion.colEjecutable] = (puntero[sDatosTablaConfiguracion.colEjecutable] != null) ? puntero[sDatosTablaConfiguracion.colEjecutable].ToString() : null;
                     resultados[sDatosTablaConfiguracion.colOpciones] = (puntero[sDatosTablaConfiguracion.colOpciones] != null) ? puntero[sDatosTablaConfiguracion.colOpciones].ToString() : null;
                     resultados[sDatosTablaConfiguracion.colOpcionListaFicheros] = (puntero[sDatosTablaConfiguracion.colOpcionListaFicheros] != null) ? puntero[sDatosTablaConfiguracion.colOpcionListaFicheros].ToString() : null;
                  }
                  puntero.Close();
                  return resultados;
               }
               else
               {
                  resultado = false;
                  mensajeError = "No se ha podido obtener los datos de configuración del programa de copia de seguridad.\r\n";
                  mensajeError += "Se ha producido el siguiente error:\r\n";
                  mensajeError += mError;
                  puntero.Close();
                  return null;
               }
            }
            else
            {
               resultado = false;
               mensajeError = "No se ha podido obtener los datos de configuración del programa de copia de seguridad.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += generadorSelect.mensajeError;
               return null;
            }
         }
         else
         {
            resultado = false;
            mensajeError = "No se ha podido obtener los datos de configuración del programa de copia de seguridad porque no está definido el gestor de la base de datos.";
            return null;
         }
      }

      // función que almacena los datos de configuración de la aplicación de la copia de seguridad en la base de datos
      public void almacenarDatosConfiguracionProgramaCopiaSeguridad(string ejecutable, string opciones, string? opcionListaFicheros)
      {
         if (baseDatos != null)
         {
            reset();
            if (string.IsNullOrEmpty(ejecutable) && string.IsNullOrEmpty(opciones))
            {
               resultado = false;
               mensajeError = "No se ha podido almacenar los datos de configuración del programa de copia de seguridad.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += "El ejecutable no está definido.\r\n";
               mensajeError += "Las opciones no están definidas.\r\n";
               return;
            }
            else if (string.IsNullOrEmpty(ejecutable))
            {
               resultado = false;
               mensajeError = "No se ha podido almacenar los datos de configuración del programa de copia de seguridad.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += "El ejecutable no está definido.\r\n";
               return;
            }
            else if (string.IsNullOrEmpty(opciones))
            {
               resultado = false;
               mensajeError = "No se ha podido almacenar los datos de configuración del programa de copia de seguridad.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += "Las opciones no están definidas.\r\n";
               return;
            }
            ClassInsertValues generadorInsert = new ClassInsertValues(sDatosTablaConfiguracion.nombreTabla, motoresBD.SQLServer);
            if (!generadorInsert.resultado)
            {
               resultado = false;
               mensajeError = "No se ha podido almacenar los datos de configuración del programa de copia de seguridad.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += generadorInsert.mensajeError;
               return;
            }
            generadorInsert.annadirCampoValor(sDatosTablaConfiguracion.colEjecutable, ClassDataBase.prepararCadenaSentenciaSQL(ejecutable));
            generadorInsert.annadirCampoValor(sDatosTablaConfiguracion.colOpciones, ClassDataBase.prepararCadenaSentenciaSQL(opciones));
            if (!string.IsNullOrEmpty(opcionListaFicheros))
               generadorInsert.annadirCampoValor(sDatosTablaConfiguracion.colOpcionListaFicheros, ClassDataBase.prepararCadenaSentenciaSQL(opcionListaFicheros));
            generadorInsert.generarSentenciaSQL();
            if (!generadorInsert.resultado)
            {
               resultado = false;
               mensajeError = "No se ha podido almacenar los datos de configuración del programa de copia de seguridad.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += generadorInsert.mensajeError;
               return;
            }
            string mError;
            if (!baseDatos.ejecutarSentenciaSQL(generadorInsert.sentenciaSQL, out mError))
            {
               resultado = false;
               mensajeError = "No se ha podido almacenar los datos de configuración del programa de copia de seguridad.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += mError;
               return;
            }
         }
         else
         {
            resultado = false;
            mensajeError = "No se ha podido almacenar los datos de configuración del programa de copia de seguridad porque no está definido el gestor de la base de datos.";
         }
      }

      // función que obtiene los datos básicos (el identificador, el nombre, la descripción y el bit seleccionada) de la copia de seguridad a partir de un filtro
      public List<Dictionary<string, string?>>? obtenerDatosBasicosCopiaSeguridad(filtro filtroDatos)
      {
         reset();
         if (baseDatos != null)
         {
            // generamos la sentencia SQL básica
            ClassSelect generadorSelect = new ClassSelect(motoresBD.SQLServer);
            generadorSelect.agregarTabla(sDatosTablaDatosCS.nombreTabla);
            generadorSelect.agregarExpresion(generadorSelect.prepararNombre(sDatosTablaDatosCS.colIdentificador));
            generadorSelect.agregarExpresion(generadorSelect.prepararNombre(sDatosTablaDatosCS.colNombre));
            generadorSelect.agregarExpresion(generadorSelect.prepararNombre(sDatosTablaDatosCS.colDescripcion));
            generadorSelect.agregarExpresion(generadorSelect.prepararNombre(sDatosTablaDatosCS.colSeleccionada));
            // comprobamos si hemos de establecer algún filtro
            if (!string.IsNullOrEmpty(filtroDatos.valor))
            {
               if (filtroDatos.valor != "*" && filtroDatos.valor != "%*")
               {
                  string definicionFiltro = generadorSelect.prepararNombre(filtroDatos.campo) + " like '%" + filtroDatos.valor + "%\'";
                  generadorSelect.agregarCondicionWhere(definicionFiltro);
               }
               else if (filtroDatos.valor == "%*")
               {
                  string definicionFiltro = generadorSelect.prepararNombre(filtroDatos.campo) + " like '%*%\'";
                  generadorSelect.agregarCondicionWhere(definicionFiltro);
               }
            }
            else
            {
               string condicionWhere = generadorSelect.prepararNombre(filtroDatos.campo) + " is null";
               generadorSelect.agregarCondicionWhere(condicionWhere);
            }
            // finalmente, generaremos la sentencia SQL
            generadorSelect.generarSentenciaSQL();
            if (!generadorSelect.resultado)
            {
               resultado = false;
               mensajeError = "No se ha podido obtener los datos básicos de la copia de seguridad.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += generadorSelect.mensajeError;
               return null;
            }
            // si llegamos aquí -> todo ok -> ejecutaremos la sentencia SQL generada
            bool resOperacion;
            string mError;
            SqlDataReader puntero = baseDatos.obtenerPunteroTablaDatos(generadorSelect.sentenciaSQL, out resOperacion, out mError);
            if (resOperacion)
            {
               List<Dictionary<string, string?>>? datosADevolver = null;
               while (puntero.Read())
               {
                  Dictionary<string, string?> datoFila = new Dictionary<string, string?>();
                  string? identificador = (puntero[sDatosTablaDatosCS.colIdentificador] != null) ? puntero[sDatosTablaDatosCS.colIdentificador].ToString() : null;
                  string? nombre = (puntero[sDatosTablaDatosCS.colNombre] != null) ? puntero[sDatosTablaDatosCS.colNombre].ToString() : null;
                  string? descripcion = (puntero[sDatosTablaDatosCS.colDescripcion] != null) ? puntero[sDatosTablaDatosCS.colDescripcion].ToString() : null;
                  string? seleccionada = (puntero[sDatosTablaDatosCS.colSeleccionada] != null) ? puntero[sDatosTablaDatosCS.colSeleccionada].ToString() : null;
                  // almacenamos los datos obtenidos
                  datoFila[sDatosTablaDatosCS.colIdentificador] = identificador;
                  datoFila[sDatosTablaDatosCS.colNombre] = nombre;
                  datoFila[sDatosTablaDatosCS.colDescripcion] = descripcion;
                  datoFila[sDatosTablaDatosCS.colSeleccionada] = seleccionada;
                  // almacenamos la fila en la lista de los datos a devolver
                  if (datosADevolver == null)
                     datosADevolver = new List<Dictionary<string, string?>>();
                  datosADevolver.Add(datoFila);
               }
               puntero.Close();
               return datosADevolver;
            }
            else
            {
               resultado = false;
               mensajeError = "No se ha podido obtener los datos básicos de la copia de seguridad.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += mError;
               return null;
            }
         }
         else
         {
            resultado = false;
            mensajeError = "No se ha podido obtener los datos básicos de la copia de seguridad porque no está definido el gestor de la base de datos.";
            return null;
         }
      }
      // función que obtiene los datos mínimos (el identificador, el nombre, la descripción y el bit seleccionada) de la copia de seguridad a partir de un filtro
      public List<Dictionary<string, string?>>? obtenerDatosMinimosCopiasSeguridadSeleccionadas(filtro filtroDatos)
      {
         reset();
         if (baseDatos != null)
         {
            // generamos la sentencia SQL básica
            ClassSelect generadorSelect = new ClassSelect(motoresBD.SQLServer);
            generadorSelect.agregarTabla(sDatosTablaDatosCS.nombreTabla);
            generadorSelect.agregarExpresion(generadorSelect.prepararNombre(sDatosTablaDatosCS.colIdentificador));
            generadorSelect.agregarExpresion(generadorSelect.prepararNombre(sDatosTablaDatosCS.colNombre));
            generadorSelect.agregarExpresion(generadorSelect.prepararNombre(sDatosTablaDatosCS.colDestino));
            // comprobamos si hemos de establecer algún filtro
            if (!string.IsNullOrEmpty(filtroDatos.valor))
            {
               if (filtroDatos.valor != "*" && filtroDatos.valor != "%*")
               {
                  string definicionFiltro = generadorSelect.prepararNombre(filtroDatos.campo) + " like '%" + filtroDatos.valor + "%\'";
                  generadorSelect.agregarCondicionWhere(definicionFiltro);
               }
               else if (filtroDatos.valor == "%*")
               {
                  string definicionFiltro = generadorSelect.prepararNombre(filtroDatos.campo) + " like '%*%\'";
                  generadorSelect.agregarCondicionWhere(definicionFiltro);
               }
            }
            else
            {
               string condicionWhere = generadorSelect.prepararNombre(filtroDatos.campo) + " is null";
               generadorSelect.agregarCondicionWhere(condicionWhere);
            }
            generadorSelect.agregarCondicionWhere(generadorSelect.prepararNombre(sDatosTablaDatosCS.colSeleccionada) + "=1");
            // finalmente, generaremos la sentencia SQL
            generadorSelect.generarSentenciaSQL();
            if (!generadorSelect.resultado)
            {
               resultado = false;
               mensajeError = "No se ha podido obtener los datos mínimos de la copia de seguridad.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += generadorSelect.mensajeError;
               return null;
            }
            // si llegamos aquí -> todo ok -> ejecutaremos la sentencia SQL generada
            bool resOperacion;
            string mError;
            SqlDataReader puntero = baseDatos.obtenerPunteroTablaDatos(generadorSelect.sentenciaSQL, out resOperacion, out mError);
            if (resOperacion)
            {
               List<Dictionary<string, string?>>? datosADevolver = null;
               while (puntero.Read())
               {
                  Dictionary<string, string?> datoFila = new Dictionary<string, string?>();
                  string? identificador = (puntero[sDatosTablaDatosCS.colIdentificador] != null) ? puntero[sDatosTablaDatosCS.colIdentificador].ToString() : null;
                  string? nombre = (puntero[sDatosTablaDatosCS.colNombre] != null) ? puntero[sDatosTablaDatosCS.colNombre].ToString() : null;
                  string? destino = (puntero[sDatosTablaDatosCS.colDestino] != null) ? puntero[sDatosTablaDatosCS.colDestino].ToString() : null;
                  // almacenamos los datos obtenidos
                  datoFila[sDatosTablaDatosCS.colIdentificador] = identificador;
                  datoFila[sDatosTablaDatosCS.colNombre] = nombre;
                  datoFila[sDatosTablaDatosCS.colDestino] = destino;
                  // almacenamos la fila en la lista de los datos a devolver
                  if (datosADevolver == null)
                     datosADevolver = new List<Dictionary<string, string?>>();
                  datosADevolver.Add(datoFila);
               }
               puntero.Close();
               return datosADevolver;
            }
            else
            {
               resultado = false;
               mensajeError = "No se ha podido obtener los datos básicos de la copia de seguridad.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += mError;
               return null;
            }
         }
         else
         {
            resultado = false;
            mensajeError = "No se ha podido obtener los datos básicos de la copia de seguridad porque no está definido el gestor de la base de datos.";
            return null;
         }
      }

      // función que obtiene todos los datos (el identificador, el nombre, la descripción, el destino y el bit seleccionada) de la copia de seguridad a partir del
      // identificador de la copia de seguridad.
      public Dictionary<string, string?>? obtenerDatosCopiaSeguridad(string idCopiaSeguridad)
      {
         reset();
         if (baseDatos != null)
         {
            // generamos la sentencia SQL básica
            ClassSelect generadorSelect = new ClassSelect(motoresBD.SQLServer);
            generadorSelect.agregarTabla(sDatosTablaDatosCS.nombreTabla);
            generadorSelect.agregarExpresion(generadorSelect.prepararNombre(sDatosTablaDatosCS.colIdentificador));
            generadorSelect.agregarExpresion(generadorSelect.prepararNombre(sDatosTablaDatosCS.colNombre));
            generadorSelect.agregarExpresion(generadorSelect.prepararNombre(sDatosTablaDatosCS.colDescripcion));
            generadorSelect.agregarExpresion(generadorSelect.prepararNombre(sDatosTablaDatosCS.colDestino));
            generadorSelect.agregarExpresion(generadorSelect.prepararNombre(sDatosTablaDatosCS.colSeleccionada));
            string condicionWhere = generadorSelect.prepararNombre(sDatosTablaDatosCS.colIdentificador) + "=" + ClassDataBase.prepararCadenaSentenciaSQL(idCopiaSeguridad);
            generadorSelect.agregarCondicionWhere(condicionWhere);
            // finalmente, generaremos la sentencia SQL
            generadorSelect.generarSentenciaSQL();
            if (!generadorSelect.resultado)
            {
               resultado = false;
               mensajeError = "No se ha podido obtener los datos completos de la copia de seguridad cuyo identificador es "+idCopiaSeguridad+".\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += generadorSelect.mensajeError;
               return null;
            }
            // si llegamos aquí -> todo ok -> ejecutaremos la sentencia SQL generada
            bool resOperacion;
            string mError;
            SqlDataReader puntero = baseDatos.obtenerPunteroTablaDatos(generadorSelect.sentenciaSQL, out resOperacion, out mError);
            if (resOperacion)
            {
               Dictionary<string, string?> datoFila = null;
               if (puntero.Read())
               {
                  string? identificador = (puntero[sDatosTablaDatosCS.colIdentificador] != null) ? puntero[sDatosTablaDatosCS.colIdentificador].ToString() : null;
                  string? nombre = (puntero[sDatosTablaDatosCS.colNombre] != null) ? puntero[sDatosTablaDatosCS.colNombre].ToString() : null;
                  string? descripcion = (puntero[sDatosTablaDatosCS.colDescripcion] != null) ? puntero[sDatosTablaDatosCS.colDescripcion].ToString() : null;
                  string? destino = (puntero[sDatosTablaDatosCS.colDestino] != null) ? puntero[sDatosTablaDatosCS.colDestino].ToString() : null;
                  string? seleccionada = (puntero[sDatosTablaDatosCS.colSeleccionada] != null) ? puntero[sDatosTablaDatosCS.colSeleccionada].ToString() : null;
                  // almacenamos los datos obtenidos
                  if (datoFila == null)
                     datoFila = new Dictionary<string, string?>();
                  datoFila[sDatosTablaDatosCS.colIdentificador] = identificador;
                  datoFila[sDatosTablaDatosCS.colNombre] = nombre;
                  datoFila[sDatosTablaDatosCS.colDescripcion] = descripcion;
                  datoFila[sDatosTablaDatosCS.colDestino] = destino;
                  datoFila[sDatosTablaDatosCS.colSeleccionada] = seleccionada;
               }
               puntero.Close();
               return datoFila;
            }
            else
            {
               resultado = false;
               mensajeError = "No se ha podido obtener los datos completos de la copia de seguridad " + idCopiaSeguridad + ".\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += mError;
               return null;
            }
         }
         else
         {
            resultado = false;
            mensajeError = "No se ha podido obtener los datos básicos de la copia de seguridad " + idCopiaSeguridad + "porque no está definido el gestor de la base de datos.";
            return null;
         }
      }

      // función que almacena una copia de seguridad en la base de datos. Como parámetros se le pasan: el identificador de la copia de seguridad, el nombre de la copia de
      // seguridad, el destino y la descripción. No devuelve nada.
      public void almacenarCopiaSeguridad(string? identificador, string? nombreCS, string? destino, string? descripcion)
      {
         if (baseDatos != null)
         {
            reset();
            // comprobamos si el identificador, el nombre de la copia de seguridad y el destino están definidos o no
            if (string.IsNullOrEmpty(identificador) || string.IsNullOrEmpty(nombreCS) || string.IsNullOrEmpty(destino))
            {
               // alguno de esos componentes o los tres, no están definidos -> error
               resultado = false;
               mensajeError = "No se puede almacenar la copia de seguridad.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += "Uno de los siguientes componentes no está definido: identificador, nombre de la copia de seguridad y el destino.";
               return;
            }
            // todos los componentes obligatorios están definidos -> almacenaremos la información
            ClassInsertValues generadorInsertValues = new ClassInsertValues(sDatosTablaDatosCS.nombreTabla, motoresBD.SQLServer);
            if (generadorInsertValues.resultado)
            {
               generadorInsertValues.annadirCampoValor(sDatosTablaDatosCS.colIdentificador, ClassDataBase.prepararCadenaSentenciaSQL(identificador));
               generadorInsertValues.annadirCampoValor(sDatosTablaDatosCS.colNombre, ClassDataBase.prepararCadenaSentenciaSQL(nombreCS));
               generadorInsertValues.annadirCampoValor(sDatosTablaDatosCS.colDestino, ClassDataBase.prepararCadenaSentenciaSQL(destino));
               if (!string.IsNullOrEmpty(descripcion))
                  generadorInsertValues.annadirCampoValor(sDatosTablaDatosCS.colDescripcion, ClassDataBase.prepararCadenaSentenciaSQL(descripcion));
               generadorInsertValues.annadirCampoValor(sDatosTablaDatosCS.colSeleccionada, "0");
               generadorInsertValues.generarSentenciaSQL();
               if (generadorInsertValues.resultado)
               {
                  string mError;
                  if (!baseDatos.ejecutarSentenciaSQL(generadorInsertValues.sentenciaSQL, out mError))
                  {
                     resultado = false;
                     mensajeError = "No se puede almacenar la copia de seguridad.\r\n";
                     mensajeError += "Se ha producido el siguiente error:\r\n";
                     mensajeError += mError;
                  }
               }
               else
               {
                  resultado = false;
                  mensajeError = "No se puede almacenar la copia de seguridad.\r\n";
                  mensajeError += "Se ha producido el siguiente error:\r\n";
                  mensajeError += generadorInsertValues.mensajeError;
               }
            }
            else
            {
               resultado = false;
               mensajeError = "No se puede almacenar la copia de seguridad.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += generadorInsertValues.mensajeError;
            }
         }
         else
         {
            resultado = false;
            mensajeError = "No se puede almacenar la copia de seguridad porque no está definido el gestor de la base de datos.";
         }
      }

      // función que actualiza una copia de seguridad en la base de datos. Como parámetros se le pasan: el identificador de la copia de seguridad, el nombre de la copia de
      // seguridad, el destino y la descripción. No devuelve nada.
      public void actualizarCopiaSeguridad(string? identificador, string? nombreCS, string? destino, string? descripcion)
      {
         if (baseDatos != null)
         {
            reset();
            // comprobamos si el identificador, el nombre de la copia de seguridad y el destino están definidos o no
            if (string.IsNullOrEmpty(identificador) || string.IsNullOrEmpty(nombreCS) || string.IsNullOrEmpty(destino))
            {
               // alguno de esos componentes o los tres, no están definidos -> error
               resultado = false;
               mensajeError = "No se puede actualizar la copia de seguridad.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += "Uno de los siguientes componentes no está definido: identificador, nombre de la copia de seguridad y el destino.";
               return;
            }
            ClassUpdateSet generadorUpdateSet = new ClassUpdateSet(sDatosTablaDatosCS.nombreTabla, motoresBD.SQLServer);
            if (!generadorUpdateSet.resultado)
            {
               resultado = false;
               mensajeError = "No se puede actualizar la copia de seguridad.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += generadorUpdateSet.mensajeError;
               return;
            }
            generadorUpdateSet.annadirCampoValor(sDatosTablaDatosCS.colNombre, ClassDataBase.prepararCadenaSentenciaSQL(nombreCS));
            generadorUpdateSet.annadirCampoValor(sDatosTablaDatosCS.colDestino, ClassDataBase.prepararCadenaSentenciaSQL(destino));
            generadorUpdateSet.annadirCampoValor(sDatosTablaDatosCS.colDescripcion, ClassDataBase.prepararCadenaSentenciaSQL(descripcion));
            string condicion = sDatosTablaDatosCS.colIdentificador + "=" + ClassDataBase.prepararCadenaSentenciaSQL(identificador);
            generadorUpdateSet.agregarCondicionWhere(condicion);
            // generamos la sentencia SQL
            generadorUpdateSet.generarSentenciaSQL();
            if (!generadorUpdateSet.resultado)
            {
               resultado = false;
               mensajeError = "No se puede actualizar la copia de seguridad.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += generadorUpdateSet.mensajeError;
               return;
            }
            string mError;
            if (!baseDatos.ejecutarSentenciaSQL(generadorUpdateSet.sentenciaSQL, out mError))
            {
               resultado = false;
               mensajeError = "No se puede actualizar la copia de seguridad.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += mError;
               return;
            }
         }
         else
         {
            resultado = false;
            mensajeError = "No se puede actualizar la copia de seguridad porque no está definido el gestor de la base de datos.";
         }
      }

      // función que borra los ficheros y directorios antiguos de una determinada copia de seguridad. Como parámetro se le pasa el identificador de la copia de seguridad.
      public void borrarFicherosYDirectoriosSeleccionadosDeCS(string? idSeguridad)
      {
         reset();
         if (baseDatos != null)
         {
            if (string.IsNullOrEmpty(idSeguridad))
            {
               resultado = false;
               mensajeError = "No se puede borrar los ficheros y directorios antiguos de una determinada copia de seguridad.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += "No está definido el identificador de la copia de seguridad a la que aplicar el borrado.";
               return;
            }
            string condicion = ClassDataBase.annadirCorchetesAlNombre(sDatosTablaFicheros.colIdentificadorCS) + "=" + ClassDataBase.prepararCadenaSentenciaSQL(idSeguridad);
            string mError;
            if (!baseDatos.borrarDatosTabla(sDatosTablaFicheros.nombreTabla, condicion, out mError))
            {
               resultado = false;
               mensajeError = "No se puede borrar los ficheros y directorios antiguos de una determinada copia de seguridad.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += mError;
            }
         }
         else
         {
            resultado = false;
            mensajeError = "No se puede borrar los ficheros y directorios antiguos de una determinada copia de seguridad porque no está definido el gestor de la base de ";
            mensajeError += "datos de la aplicación.";
         }
      }

      // función que almacena la lista de ficheros y directorios a agregar a la copia de seguridad. Como parámetro se le pasan: el identificador de la copia de seguridad
      // y el contenido de la lista de ficheros y directorios a agregar
      public void almacenarListaFicherosYDirectoriosSeleccionadosDeUnaCopiaDeSeguridad(string? idSeguridad, List<string>? listaFicherosYDirectorios)
      {
         reset();
         if (baseDatos != null)
         {
            if (string.IsNullOrEmpty(idSeguridad))
            {
               resultado = false;
               mensajeError = "No se puede almacenar la lista de ficheros y directorios seleccionados de una determinada copia de seguridad.\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += "No está definido el identificador de la copia de seguridad a la que aplicar el almacenamiento.";
               return;
            }
            if (listaFicherosYDirectorios != null && listaFicherosYDirectorios.Count > 0)
            {
               int tamNombreAbsoluto = obtenerTamannoColumnaTabla(sDatosTablaFicheros.nombreTabla, sDatosTablaFicheros.colNombreAbsoluto);
               if (!resultado)
               {
                  string mError = "No se puede almacenar la lista de ficheros y directorios seleccionados de una determinada copia de seguridad.\r\n";
                  mError += "Se ha producido el siguiente error:\r\n";
                  mError += mensajeError;
                  mensajeError = mError;
                  return;
               }
               foreach (string item in listaFicherosYDirectorios)
               {
                  // antes de nada, comprobaremos la longitud del nombre absoluto
                  if (item.Length > tamNombreAbsoluto)
                  {
                     resultado = false;
                     mensajeError = "No se puede almacenar la lista de ficheros y directorios seleccionados de una determinada copia de seguridad.\r\n";
                     mensajeError += "Se ha producido el siguiente error:\r\n";
                     mensajeError += "La longitud del nombre absoluto del fichero o directorio \"" + item + "\" es mayor de " + tamNombreAbsoluto + " caracteres.";
                     return;
                  }
                  bool resOperacion;
                  string mError;
                  long idFichero = baseDatos.siguienteIdentificadorLong(sDatosTablaFicheros.nombreTabla, sDatosTablaFicheros.colIdentificador, out resOperacion, out mError);
                  if (!resOperacion)
                  {
                     resultado = false;
                     mensajeError = "No se puede almacenar la lista de ficheros y directorios seleccionados de una determinada copia de seguridad.\r\n";
                     mensajeError += "Se ha producido el siguiente error:\r\n";
                     mensajeError += mError;
                     return;
                  }
                  ClassInsertValues generadorInsert = new ClassInsertValues(sDatosTablaFicheros.nombreTabla, motoresBD.SQLServer);
                  if (!generadorInsert.resultado)
                  {
                     resultado = false;
                     mensajeError = "No se puede almacenar la lista de ficheros y directorios seleccionados de una determinada copia de seguridad.\r\n";
                     mensajeError += "Se ha producido el siguiente error:\r\n";
                     mensajeError += generadorInsert.mensajeError;
                     return;
                  }
                  generadorInsert.annadirCampoValor(sDatosTablaFicheros.colIdentificador, idFichero.ToString());
                  generadorInsert.annadirCampoValor(sDatosTablaFicheros.colNombreAbsoluto, ClassDataBase.prepararCadenaSentenciaSQL(item));
                  generadorInsert.annadirCampoValor(sDatosTablaFicheros.colIdentificadorCS, ClassDataBase.prepararCadenaSentenciaSQL(idSeguridad));
                  generadorInsert.generarSentenciaSQL();
                  if (!generadorInsert.resultado)
                  {
                     resultado = false;
                     mensajeError = "No se puede almacenar la lista de ficheros y directorios seleccionados de una determinada copia de seguridad.\r\n";
                     mensajeError += "Se ha producido el siguiente error:\r\n";
                     mensajeError += generadorInsert.mensajeError;
                     return;
                  }
                  if (!baseDatos.ejecutarSentenciaSQL(generadorInsert.sentenciaSQL, out mError))
                  {
                     resultado = false;
                     mensajeError = "No se puede almacenar la lista de ficheros y directorios seleccionados de una determinada copia de seguridad.\r\n";
                     mensajeError += "Se ha producido el siguiente error:\r\n";
                     mensajeError += mError;
                  }
               }
            }
         }
         else
         {
            resultado = false;
            mensajeError = "No se puede almacenar la lista de ficheros y directorios seleccionados de una copia de seguridad porque no está definido el gestor de la base ";
            mensajeError += "de datos de la aplicación.";
         }
      }

      // función que obtiene la lista de ficheros y directorios seleccionados para la copia de seguridad. Devuelve esa lista. Como parámetro se le pasa el identificador de
      // la copia de seguridad.
      public List<string>? obtenerListaFicherosYDirectoriosSeleccionados(string idCopiaSeguridad)
      {
         reset();
         if (baseDatos != null)
         {
            ClassSelect generadorSelect = new ClassSelect(motoresBD.SQLServer);
            if (!generadorSelect.resultado)
            {
               resultado = false;
               mensajeError = "No se puede obtener la lista de ficheros y directorios seleccionados para la copia de seguridad " + idCopiaSeguridad + ".\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += generadorSelect.mensajeError;
               return null;
            }
            generadorSelect.agregarTabla(sDatosTablaFicheros.nombreTabla);
            generadorSelect.agregarExpresion(generadorSelect.prepararNombre(sDatosTablaFicheros.colNombreAbsoluto));
            string condicion = generadorSelect.prepararNombre(sDatosTablaFicheros.colIdentificadorCS) + "=" + ClassDataBase.prepararCadenaSentenciaSQL(idCopiaSeguridad);
            generadorSelect.agregarCondicionWhere(condicion);
            generadorSelect.generarSentenciaSQL();
            if (!generadorSelect.resultado)
            {
               resultado = false;
               mensajeError = "No se puede obtener la lista de ficheros y directorios seleccionados para la copia de seguridad " + idCopiaSeguridad + ".\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += generadorSelect.mensajeError;
               return null;
            }
            bool resOperacion;
            string mError;
            SqlDataReader puntero = baseDatos.obtenerPunteroTablaDatos(generadorSelect.sentenciaSQL, out resOperacion, out mError);
            if (resOperacion)
            {
               List<string>? lista = null;
               while (puntero.Read())
               {
                  string? ficheroODirectorio = (puntero[sDatosTablaFicheros.colNombreAbsoluto] != null) ? puntero[sDatosTablaFicheros.colNombreAbsoluto].ToString() : null;
                  if (!string.IsNullOrEmpty(ficheroODirectorio))
                  {
                     if (lista == null)
                        lista = new List<string>();
                     lista.Add(ficheroODirectorio);
                  }
               }
               puntero.Close();
               return lista;
            }
            else
            {
               resultado = false;
               mensajeError = "No se puede obtener la lista de ficheros y directorios seleccionados para la copia de seguridad " + idCopiaSeguridad + ".\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += mError;
               return null;
            }

         }
         else
         {
            resultado = false;
            mensajeError = "No se puede obtener la lista de ficheros y directorios seleccionados para la copia de seguridad " + idCopiaSeguridad + " porque no está definido ";
            mensajeError += "el gestor de la base de datos de la aplicación.";
            return null;
         }
      }

      // función que actualiza el campo "Seleccionada" de la tabla "Datos CS" por el valor que hemos puesto en el pertinente datagrid. Como parámetros se le pasan: el
      // identificador de la copia de seguridad afectada y el nuevo valor. No devuelve nada.
      public void actualizarCampoSeleccionadaEnDatosCS(string idCS, bool valor)
      {
         reset();
         if (baseDatos != null)
         {
            // generaremos la sentencia SQL encargada del proceso
            ClassUpdateSet generadorUpdateSet = new ClassUpdateSet(sDatosTablaDatosCS.nombreTabla, motoresBD.SQLServer);
            if (!generadorUpdateSet.resultado)
            {
               resultado = false;
               mensajeError = "No se puede actualizar el campo seleccionado para la copia de seguridad " + idCS + ".\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += generadorUpdateSet.mensajeError;
               return;
            }
            generadorUpdateSet.annadirCampoValor(sDatosTablaDatosCS.colSeleccionada, (valor) ? "1" : "0");
            string condicionWhere = generadorUpdateSet.prepararNombre(sDatosTablaDatosCS.colIdentificador) + "=" + ClassDataBase.prepararCadenaSentenciaSQL(idCS);
            generadorUpdateSet.agregarCondicionWhere(condicionWhere);
            generadorUpdateSet.generarSentenciaSQL();
            if (generadorUpdateSet.resultado)
            {
               // se ha generado sin ningún error la sentencia SQL -> procederemos con la ejecución de la sentencia
               string mError;
               if (!baseDatos.ejecutarSentenciaSQL(generadorUpdateSet.sentenciaSQL, out mError))
               {
                  resultado = false;
                  mensajeError = "No se puede actualizar el campo seleccionado para la copia de seguridad " + idCS + ".\r\n";
                  mensajeError += "Se ha producido el siguiente error:\r\n";
                  mensajeError += mError;
               }
            }
            else
            {
               resultado = false;
               mensajeError = "No se puede actualizar el campo seleccionado para la copia de seguridad " + idCS + ".\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += generadorUpdateSet.mensajeError;
               return;
            }
         }
         else
         {
            resultado = false;
            mensajeError = "No se puede actualizar el campo seleccionado para la copia de seguridad " + idCS + " porque no está definido el gestor de la base de datos de la ";
            mensajeError += "aplicación.";
         }
      }

      // función que borra una determinada copia de seguridad. Como parámetro se le pasa el identificador de la copia de seguridad.
      public void borrarCopiaSeguridad(string idCS)
      {
         reset();
         if (baseDatos != null)
         {
            // generamos la condición de borrado
            string condicion = ClassDataBase.annadirCorchetesAlNombre(sDatosTablaDatosCS.colIdentificador) + "=" + ClassDataBase.prepararCadenaSentenciaSQL(idCS);
            string mError;
            if (!baseDatos.borrarDatosTabla(sDatosTablaDatosCS.nombreTabla, condicion, out mError))
            {
               resultado = false;
               mensajeError = "No se ha podido borrar la copia de seguridad cuyo identificador es " + idCS + ".\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += mError;
            }
         }
         else
         {
            resultado = false;
            mensajeError = "No se ha podido borrar la copia de seguridad cuyo identificador es " + idCS + " porque no está definido el gestor de la base de datos de la aplicación.";
         }
      }
   }
}
