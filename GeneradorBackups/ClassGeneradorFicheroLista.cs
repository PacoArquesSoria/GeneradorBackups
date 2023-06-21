using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneradorBackups
{
   /****** CLASE ClassGeneradorFicheroLista ******/
   /**********************************************/
   // clase encargada de generar el fichero con la lista de ficheros y directorios cuya copia de seguridad queremos efectuar.
   public class ClassGeneradorFicheroLista
   {
      /****** VARIABLES DE LA CLASE ******/
      /***********************************/
      private ClassGestorBD? gestorBD;                                                  // gestor de base de datos con el que trabajaremos
      private string? nombreRelativo;                                                   // nombre relativo del archivo con la lista
      private string? idCS;                                                             // identificador de la copia de seguridad
      filtro filtro;                                                                    // filtro de selección de archivos

      /****** PROPIEDADES DE LA CLASE ******/
      /*************************************/
      // nombreAbsoluto -> Devuelve el nombre absoluto del archivo con la lista almacenada
      public string? nombreAbsoluto
      {
         private set; get;
      }

      // ficheroVacio -> Devuelve un booleano indicando si el fichero generado está vacío
      public bool ficheroVacio
      {
         private set; get;
      }

      // resultado -> Devuelve el booleano que nos dice si la operación se ha efectuado correctamente o no
      public bool resultado
      {
         private set; get;
      }

      // mensajeError -> Devuelve el mensaje de error que se haya podido generar
      public string mensajeError
      {
         private set; get;
      }

      /****** CONSTRUCTORES DE LA CLASE ******/
      /***************************************/
      // por defecto. A partir del gestor de la base de datos, el identificador de la copia de seguridad.y el filtro de búsqueda
      public ClassGeneradorFicheroLista(ClassGestorBD? _gestorBD, string? _idCS, filtro _filtro)
      {
         gestorBD = _gestorBD;
         idCS = _idCS;
         filtro = _filtro;
         nombreRelativo = null;
         nombreAbsoluto = null;
         ficheroVacio = true;
         reset();
         if (_gestorBD == null && string.IsNullOrEmpty(_idCS))
         {
            resultado = false;
            mensajeError = "No se ha podido generar el fichero donde se almacenará la lista de ficheros y carpetas que se ejecutarán en la copia de seguridad.\r\n";
            mensajeError += "Se ha producido el siguiente error:\r\n";
            mensajeError += "Gestor de bases de datos indefinido.\r\n";
            mensajeError += "Identificador de la copia de seguridad indefinido.";
         }
         else if (_gestorBD == null)
         {
            resultado = false;
            mensajeError = "No se ha podido generar el fichero donde se almacenará la lista de ficheros y carpetas que se ejecutarán en la copia de seguridad.\r\n";
            mensajeError += "Se ha producido el siguiente error:\r\n";
            mensajeError += "Gestor de bases de datos indefinido.";
         }
         else if (string.IsNullOrEmpty(_idCS))
         {
            resultado = false;
            mensajeError = "No se ha podido generar el fichero donde se almacenará la lista de ficheros y carpetas que se ejecutarán en la copia de seguridad.\r\n";
            mensajeError += "Se ha producido el siguiente error:\r\n";
            mensajeError += "Identificador de la copia de seguridad indefinido.";
         }
         else
         {
            generarNombreRelativoFichero();
            generarNombreAbsolutoFichero();
            if (resultado)
               generarContenidoFichero();
         }
      }

      /****** MÉTODOS DE LA CLASE ******/
      /*********************************/
      // función que inicializa las propiedades resultado y mensajeError a un valor por defecto.
      private void reset()
      {
         resultado = true;
         mensajeError = "";
      }

      // función que genera el nombre relativo del fichero donde se almacenará la lista
      private void generarNombreRelativoFichero()
      {
         nombreRelativo = idCS + ".lst";
      }

      // función que genera el nombre absoluto del fichero donde se almacenará la lista
      private void generarNombreAbsolutoFichero()
      {
         reset();
         // obtenemos el directorio temporal
         string? dirTemporal = gestorBD.dirTemporal;
         if (!string.IsNullOrEmpty(dirTemporal))
            nombreAbsoluto = dirTemporal + "\\" + nombreRelativo;
         else
         {
            nombreAbsoluto = null;
            resultado = false;
            mensajeError = "No se ha podido generar el fichero donde se almacenará la lista de ficheros y carpetas que se ejecutarán en la copia de seguridad.\r\n";
            mensajeError += "Se ha producido el siguiente error:\r\n";
            mensajeError += "La variable de entorno que indica el directorio temporal de Windows no está definida.";
            return;
         }
      }

      // función que genera el contenido del fichero lista
      private void generarContenidoFichero()
      {
         reset();
         List<string> listaFicherosDirectorios = gestorBD.obtenerListaFicherosYDirectoriosSeleccionados(idCS);
         if (!gestorBD.resultado)
         {
            resultado = false;
            mensajeError = "No se ha podido generar el fichero donde se almacenará la lista de ficheros y carpetas que se ejecutarán en la copia de seguridad.\r\n";
            mensajeError += "Se ha producido el siguiente error:\r\n";
            mensajeError += gestorBD.mensajeError;
            return;
         }
         try
         {
            if (nombreAbsoluto != null)
            {
               StreamWriter sw = new StreamWriter(nombreAbsoluto);
               if (listaFicherosDirectorios != null && listaFicherosDirectorios.Count > 0)
               {
                  ficheroVacio = false;
                  foreach (string fichero in listaFicherosDirectorios)
                     sw.WriteLine(fichero);
               }
               sw.Close();
            }
            else
               throw new Exception("No se ha establecido el nombre absoluto del fichero con la lista de ficheros y directorios a aplicar en la copia de seguridad.");

         } catch (Exception excepcion)
         {
            resultado = false;
            mensajeError = "No se ha podido generar el fichero donde se almacenará la lista de ficheros y carpetas que se ejecutarán en la copia de seguridad.\r\n";
            mensajeError += "Se ha producido el siguiente error:\r\n";
            mensajeError += excepcion.Message;
         }
      }
   }
}
