using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FSBitBossSolutions;

namespace GeneradorBackups
{

   /****** ESTRUCTURAS DE DATOS ******/
   /**********************************/
   public struct filtro                                                                               // datos del filtro
   {
      public string? campo;                                                                                             // campo
      public string? valor;                                                                                             // valor
   }

   public partial class FormCopiaSeguridad : Form
   {
      /****** CONSTANTES DE LA VENTANA ******/
      /**************************************/
      private const int kColIdentificador = 0;                                                        // índice de la columna Identificador del datagrid
      private const int kColNombre = 1;                                                               // índice de la columna Nombre del datagrid
      private const int kColDescripcion = 2;                                                          // índice de la columna Descripción del datagrid
      private const int kColSeleccionada = 3;                                                         // índice de la columna Seleccionada del datagrid
      private const string kBotonActivarDesactivarBitSeleccion_Activar = "Activar bit de selección";  // etiqueta del bótón "Activar/Desactivar bit selección" en modo Activar
      private const string kBotonActivarDesactivarBitSeleccion_Desactivar = "Desactivar bit de selección";     // etiqueta del botón "Activar/Desactivar bit selección" en modo Desactivar
      private const string kBotonSeleccionarDeseleccionarFilas_Seleccionar = "Seleccionar filas";     // etiqueta del botón "Seleccionar/Deseleccionar filas" cuando hay que seleccionarlas
      private const string kBotonSeleccionarDeseleccionarFilas_Deseleccionar = "Deseleccionar filas"; // etiqueta del botón "Seleccionar/Deseleccionar filas" cuando hay que deseleccionarlas
      private const string kWinRAR = "winrar";                                                        // nombre sin extensión del compresor WinRAR
      private const string kExtensionRAR = ".rar";                                                    // extensión por defecto de los ficheros RAR

      /****** VARIABLES DE LA VENTANA ******/
      /*************************************/
      private filtro datosFiltro;                                                                     // datos del filtro
      private List<Dictionary<string, string?>>? datosCS;                                             // datos de la copia de seguridad
      private List<string>? ficherosCopiaSeguridad;                                                   // ficheros y directorios de la copia de seguridad
      private bool activarDesactivarBitSeleccion;                                                     // flag que indica si hemos de activar o desactivar el bit de selección
      private bool bloquearEdicionCeldaSeleccionada;                                                  // flag que indica si hemos de bloquear la edición de la celda de la columna Seleccionada

      /****** PROPIEDADES DE LA VENTANA ******/
      /***************************************/
      // ventanaPrincipal -> Puntero a la ventana principal de la aplicación
      public FormPrincipal? ventanaPrincipal
      {
         set; private get;
      }

      // gestorBD -> Gestor del motor de la base de datos de la aplicación
      public ClassGestorBD? gestorBD
      {
         set; private get;
      }

      /****** CONSTRUCTORES DE LA CLASE ******/
      /***************************************/
      // por defecto
      public FormCopiaSeguridad()
      {
         InitializeComponent();
         datosFiltro.campo = null;
         datosFiltro.valor = "*";
         datosCS = null;
         ficherosCopiaSeguridad = null;
         activarDesactivarBitSeleccion = true;
         bloquearEdicionCeldaSeleccionada = false;
         ventanaPrincipal = null;
         gestorBD = null;
      }

      /****** MÉTODOS DE LA VENTANA ******/
      /***********************************/
      // función que inicializa el selector de campos del filtro
      private void inicializarSelectorCamposFiltro()
      {
         cbSelectorCampo.Items.Clear();
         DataGridViewColumnCollection columnas = dgvDatosCS.Columns;
         foreach (DataGridViewColumn columna in columnas)
         {
            if (columna.HeaderText != sDatosTablaDatosCS.colSeleccionada)
               cbSelectorCampo.Items.Add(columna.HeaderText);
         }
         cbSelectorCampo.SelectedIndex = 0;
         datosFiltro.campo = cbSelectorCampo.SelectedItem as string;
         datosFiltro.valor = "*";
      }

      // función que inicializa el textbox de la entrada del valor del filtro
      private void inicializarTextBoxEntradaValorFiltro()
      {
         textBoxValor.Text = datosFiltro.valor;
      }

      // función que inicializa el datagrid con los datos de la copia de seguridad. Devuelve un booleano indicando si la operación se ha podido efectuar
      public bool inicializarDatagrid()
      {
         if (gestorBD != null)
         {
            datosCS = gestorBD.obtenerDatosBasicosCopiaSeguridad(datosFiltro);
            if (gestorBD.resultado)
            {
               dgvDatosCS.Rows.Clear();
               if (datosCS != null && datosCS.Count > 0)
               {
                  foreach (Dictionary<string, string?> datoFila in datosCS)
                  {
                     string? identificador = datoFila[sDatosTablaDatosCS.colIdentificador];
                     string? nombre = datoFila[sDatosTablaDatosCS.colNombre];
                     string? descripcion = datoFila[sDatosTablaDatosCS.colDescripcion];
                     bool seleccionada = false;
                     if (!string.IsNullOrEmpty(datoFila[sDatosTablaDatosCS.colSeleccionada]))
                     {
                        switch (datoFila[sDatosTablaDatosCS.colSeleccionada])
                        {
                           case "False":
                              seleccionada = false;
                              break;
                           case "True":
                              seleccionada = true;
                              break;
                           default:
                              MessageBox.Show("Error al convertir la cadena \"" + datoFila[sDatosTablaDatosCS.colSeleccionada] + "\" a booleano.",
                                              "ERROR INTERNO DE LA APLICACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Error);
                              return false;
                        }
                     }
                     if (!string.IsNullOrEmpty(identificador) && !string.IsNullOrEmpty(nombre))
                     {
                        bloquearEdicionCeldaSeleccionada = true;
                        dgvDatosCS.Rows.Add(new object[] { identificador, nombre, (descripcion == null) ? "" : descripcion, seleccionada });
                        bloquearEdicionCeldaSeleccionada = false;
                     }
                  }
               }
               dgvDatosCS.Refresh();
               return true;
            }
            else
            {
               MessageBox.Show(gestorBD.mensajeError, "ERROR AL OBTENER LOS DATOS BÁSICOS DE LAS COPIAS DE SEGURIDAD", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
               return false;
            }
         }
         else
            return false;
      }

      // función que inicializa la entrada del valor al seleccionar el tipo de campo. Devuelve un booleano indicando si la operación se ha podido efectuar correctamente o
      // no.
      private bool inicializarEntradaValor()
      {
         if (gestorBD != null)
         {
            int tamEntradaValor = gestorBD.obtenerTamannoColumnaTabla(sDatosTablaDatosCS.nombreTabla, datosFiltro.campo);
            if (gestorBD.resultado)
            {
               datosFiltro.valor = "*";
               textBoxValor.MaxLength = tamEntradaValor;
               textBoxValor.Text = "*";
               return true;
            }
            else
            {
               MessageBox.Show(gestorBD.mensajeError, "ERROR AL OBTENER EL TAMAÑO DE LA COLUMNA DE LA TABLA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
               return false;
            }
         }
         else
            return false;
      }

      // función que obtiene el tamaño del identificador, del nombre y de la descripción de la tabla de la copia de seguridad.
      private (int tamIdentificador, int tamNombre, int tamDescripcion) obtenerTamannhosColumnasDatagrid()
      {
         if (gestorBD != null)
         {
            int tamIdentificador = gestorBD.obtenerTamannoColumnaTabla(sDatosTablaDatosCS.nombreTabla, sDatosTablaDatosCS.colIdentificador);
            if (!gestorBD.resultado)
            {
               MessageBox.Show(gestorBD.mensajeError, "ERROR AL OBTENER EL TAMAÑO DE LA COLUMNA IDENTIFICADOR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
               return (-1, -1, -1);
            }
            int tamNombre = gestorBD.obtenerTamannoColumnaTabla(sDatosTablaDatosCS.nombreTabla, sDatosTablaDatosCS.colNombre);
            if (!gestorBD.resultado)
            {
               MessageBox.Show(gestorBD.mensajeError, "ERROR AL OBTENER EL TAMAÑO DE LA COLUMNA NOMBRE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
               return (-1, -1, -1);
            }
            int tamDescripcion = gestorBD.obtenerTamannoColumnaTabla(sDatosTablaDatosCS.nombreTabla, sDatosTablaDatosCS.colDescripcion);
            if (!gestorBD.resultado)
            {
               MessageBox.Show(gestorBD.mensajeError, "ERROR AL OBTENER EL TAMAÑO DE LA COLUMNA DESCRIPCIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
               return (-1, -1, -1);
            }
            return (tamIdentificador, tamNombre, tamDescripcion);
         }
         else
            return (-1, -1, -1);
      }

      // función que inicializa el tamaño de las columnas del datagrid. Devuelve true si la operación se ha efectuado correctamente y false, en caso contrario
      private bool inicializarTamannoColumnasDatagrid()
      {
         int tamIdentificador, tamNombre, tamDescripcion;
         (tamIdentificador, tamNombre, tamDescripcion) = obtenerTamannhosColumnasDatagrid();
         if (tamIdentificador == -1 && tamNombre == -1 && tamDescripcion == -1)
            return false;
         else
         {
            ColumnaIdentificador.MaxInputLength = tamIdentificador;
            ColumnaNombre.MaxInputLength = tamNombre;
            ColumnaDescripcion.MaxInputLength = tamDescripcion;
            return true;
         }
      }

      // función que nos dice si hemos de activar o no el botón "Editar copia seguridad"
      private bool hayQueActivarBotonEditarCopiaSeguridad()
      {
         // sólo se activará si hay una fila seleccionada
         return (dgvDatosCS.SelectedRows != null && dgvDatosCS.SelectedRows.Count == 1);
      }

      // función que activa o desactiva el botón "Editar copia seguridad"
      private void activarBotonEditarCopiaSeguridad()
      {
         buttonEditarCS.Enabled = hayQueActivarBotonEditarCopiaSeguridad();
      }

      // función que nos dice si hemos de activar el botón "Eliminar copia de seguridad"
      private bool hayQueActivarBotonEliminarCopiaSeguridad()
      {
         // sólo se activará si hay un o más filas seleccionadas en el datagrid
         return (dgvDatosCS.SelectedRows != null && dgvDatosCS.SelectedRows.Count >= 1);
      }

      // función que activa o desactiva el botón "Eliminar copia de seguridad"
      private void activarBotonEliminarCopiaSeguridad()
      {
         buttonEliminarCopiasSeguridad.Enabled = hayQueActivarBotonEliminarCopiaSeguridad();
      }

      // función que nos dice si hay una o más filas seleccionadas en el datagrid
      private bool existenFilasSeleccionadasEnElDatagrid()
      {
         return (dgvDatosCS.SelectedRows != null && dgvDatosCS.SelectedRows.Count >= 1);
      }

      // establece la etiqueta del botón "Seleccionar/Deseleccionar filas"
      private void establecerEtiquetaBotonSeleccionarDeseleccionarFilas()
      {
         switch (existenFilasSeleccionadasEnElDatagrid())
         {
            case false:                                                       // no existen filas seleccionadas en el datagrid
               buttonSeleccionarDeseleccionarFilas.Text = kBotonSeleccionarDeseleccionarFilas_Seleccionar;                 // hemos de seleccionarlas
               break;
            case true:                                                        // existen filas seleccionadas en el datagrid
               buttonSeleccionarDeseleccionarFilas.Text = kBotonSeleccionarDeseleccionarFilas_Deseleccionar;
               break;
         }
      }

      // función que obtiene los datos de la copia de seguridad seleccionada
      private bool obtenerDatosCSSeleccionada(out string? identificador, out string? nombre, out string? descripcion, out string? destino, out bool seleccionada)
      {
         seleccionada = false;
         identificador = null;
         nombre = null;
         descripcion = null;
         destino = null;
         if (gestorBD != null)
         {
            // en primer lugar, obtendremos el identificador de la columna identificador de la fila seleccionada
            string? idCS = (dgvDatosCS.SelectedRows[0].Cells[kColIdentificador].Value != null) ? dgvDatosCS.SelectedRows[0].Cells[kColIdentificador].Value.ToString() : null;
            if (!string.IsNullOrEmpty(idCS))
            {
               Dictionary<string, string?>? datosCS = gestorBD.obtenerDatosCopiaSeguridad(idCS);
               if (gestorBD.resultado)
               {
                  if (datosCS != null && datosCS.Count > 0)
                  {
                     identificador = datosCS[sDatosTablaDatosCS.colIdentificador];
                     nombre = datosCS[sDatosTablaDatosCS.colNombre];
                     descripcion = datosCS[sDatosTablaDatosCS.colDescripcion];
                     destino = datosCS[sDatosTablaDatosCS.colDestino];
                     switch (datosCS[sDatosTablaDatosCS.colSeleccionada])
                     {
                        case "False":
                           seleccionada = false;
                           break;
                        case "True":
                           seleccionada = true;
                           break;
                        default:                                                                   // error gordo
                           MessageBox.Show("No se ha podido convertir la cadena \"" + datosCS[sDatosTablaDatosCS.colSeleccionada] + "\" a booleano.",
                                           "ERROR DE CONVERSIÓN DE TIPOS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                           return false;
                     }
                     return true;
                  }
                  else
                     return true;
               }
               else
               {
                  MessageBox.Show(gestorBD.mensajeError, "ERROR AL OBTENER LOS DATOS DE LA COPIA DE SEGURIDAD", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  return false;
               }
            }
            else
            {
               MessageBox.Show("No se ha podido obtener el identificador de la columna identificador de la fila seleccionada.", "ERROR INTERNO DE LA APLICACIÓN",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
               return false;
            }
         }
         else
            return false;
      }

      // función que nos dice en qué modo está el botón "Activar/Desactivar bit de selección"
      private void modoBotonActivarDesactivarBitSeleccion()
      {
         bool cActivo = false;
         for(int i = 0; i < dgvDatosCS.Rows.Count && !cActivo; ++i)
         {
            if (dgvDatosCS.Rows[i].Cells[kColSeleccionada].Value is bool)
            {
               bool valor = (bool)dgvDatosCS.Rows[i].Cells[kColSeleccionada].Value;
               if (valor == true)
                  cActivo = true;
            }
         }
         activarDesactivarBitSeleccion = cActivo ? false : true;
      }

      // función que establece la etiqueta del botón "Activar/Desactivar bit de selección"
      private void establecerEtiquetaBotonActivarDesactivarBitSeleccion()
      {
         // antes de nada, estableceremos el modo en el qué estamos
         modoBotonActivarDesactivarBitSeleccion();
         switch (activarDesactivarBitSeleccion)
         {
            case true:                                                                                         // modo activar
               buttonActivarDesactivarSeleccionada.Text = kBotonActivarDesactivarBitSeleccion_Activar;
               break;
            case false:                                                                                        // modo desactivar
               buttonActivarDesactivarSeleccionada.Text = kBotonActivarDesactivarBitSeleccion_Desactivar;
               break;
         }
      }

      // función que nos dice si hemos de activar o desactivar el botón "Seleccionar/Deseleccionar filas"
      private bool hayQueActivarBotonSeleccionarDeseleccionarFilas()
      {
         return (dgvDatosCS.Rows != null && dgvDatosCS.Rows.Count > 0);
      }

      // función que activa o desactiva el botón "Seleccionar/Deseleccionar filas"
      private void activarBotonSeleccionarDeseleccionarFilas()
      {
         buttonSeleccionarDeseleccionarFilas.Enabled = hayQueActivarBotonSeleccionarDeseleccionarFilas();
      }

      // función que nos dice si hemos de activar o no el botón "Ejecutar"
      private bool hayQueActivarBotonEjecutar()
      {
         if (dgvDatosCS.Rows != null && dgvDatosCS.Rows.Count > 0)
         {
            bool bitSeleccionado = false;
            for (int i = 0; i < dgvDatosCS.Rows.Count && !bitSeleccionado; i++)
               bitSeleccionado = (bool)dgvDatosCS.Rows[i].Cells[kColSeleccionada].Value;
            return bitSeleccionado;
         }
         else
            return false;
      }

      // función que activa o desactiva el botón "Ejecutar"
      private void activarBotonEjecutar()
      {
         buttonEjecutar.Enabled = hayQueActivarBotonEjecutar();
      }

      // función que nos dice si hemos de activar o no el botón "Activar/Desactivar bit de selección"
      private bool hayQueActivarBotonActivarDesactivarBitSeleccion()
      {
         return (dgvDatosCS.Rows != null && dgvDatosCS.Rows.Count > 0);
      }

      // función que activa o desactiva el botón "Activar/Desactivar bit de selección"
      private void activarBotonActivarDesactivarBitSeleccion()
      {
         buttonActivarDesactivarSeleccionada.Enabled = hayQueActivarBotonActivarDesactivarBitSeleccion();
      }

      // función que obtiene los datos de la tabla Configuración para ejecutar las copias de seguridad seleccionadas en el pertinente Datagrid.
      private (string? ejecutable, string? opciones, string? opcionListaFicheros, bool errorLectura) obtenerEjecutableCS()
      {
         if (gestorBD != null)
         {
            Dictionary<string, string?>? datosEjecutable = gestorBD.obtenerDatosConfiguracionProgramaCopiaSeguridad();
            if (gestorBD.resultado)
            {
               try
               {
                  if (datosEjecutable != null && datosEjecutable.Count > 0)
                  {
                     string? ejecutable = datosEjecutable[sDatosTablaConfiguracion.colEjecutable];
                     string? opciones = datosEjecutable[sDatosTablaConfiguracion.colOpciones];
                     string? opcionListaFicheros = datosEjecutable[sDatosTablaConfiguracion.colOpcionListaFicheros];
                     return (ejecutable, opciones, opcionListaFicheros, false);
                  }
                  else
                     return (null, null, null, false);
               }
               catch (ArgumentOutOfRangeException)
               {
                  MessageBox.Show("No se ha podido leer los datos del programa de copia de seguridad.", "ERROR INTERNO DE LA APLICACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Error);
                  return (null, null, null, true);
               }
            }
            else
            {
               MessageBox.Show(gestorBD.mensajeError, "ERROR AL OBTENER LOS DATOS DEL PROGRAMA DE COPIA DE SEGURIDAD", MessageBoxButtons.OK, MessageBoxIcon.Error);
               return (null, null, null, true);
            }
         }
         else
            return (null, null, null, true);
      }

      // función que nos dice si el ejecutable es del programa WinRAR o no. Como parámetro se le pasa el nombre del ejecutable. Devuelve un booleano indicándolo.
      private bool esWinRar(string ejecutable)
      {
         return ejecutable.IndexOf(kWinRAR) != -1;
      }

      // función que genera el nombre completo del fichero RAR donde se realizará la copia de seguridad. Como parámetros se le pasan: el nombre del fichero RAR y el nombre
      // del ejecutable.
      private string generarNombreFicheroRar(string ficheroRAR, string ejecutable)
      {
         // comprobamos si el programa de copia de seguridad es WinRAR
         if (esWinRar(ejecutable))
         {
            // lo es -> comprobaremos si lleva extensión RAR
            if (ficheroRAR.IndexOf(kExtensionRAR) != -1)
               // lo lleva -> no hará nada -> devolverá el nombre del fichero RAR sin modificar
               return ficheroRAR;
            else
               // no lo lleva -> lo añadiremos
               return ficheroRAR + kExtensionRAR;
         }
         else
            // no lo es -> no hará nada -> devolverá el nombre del fichero RAR sin modificar
            return ficheroRAR;
      }


      // función que ejecuta la copia de seguridad. Como parámetros se le pasan: el nombre del ejecutable, las opciones, la opción de la lista de ficheros, el identificador
      // de la copia de seguridad, el nombre de la copia, el destino y el fichero con la lista de ficheros y directorios
      private bool ejecutarCS(string ejecutable, string opciones, string? opcionListaFicheros, string? idCS, string? nombre, string? destino, string ficheroListaFicheros)
      {
         if (gestorBD == null)
            return false;
         try
         {
            // en primer lugar, añadiremos la extensión RAR si es preciso
            string nombreFichero = generarNombreFicheroRar((nombre==null) ? "":nombre, ejecutable);
            // en primer lugar, obtendremos el nombre absoluto del fichero de la copia de seguridad y el nombre absoluto del fichero temporal de la copia de seguridad
            string nombreAbsoluto = destino + "\\" + nombreFichero;
            string nombreTemporal = gestorBD.dirTemporal + "\\" + nombreFichero;
            string mensajeError;
            // borra el archivo antiguo si lo hubiera
            if (ClassFileSystem.existeDirectorio(nombreAbsoluto))
               throw new Exception("El archivo " + nombreAbsoluto + " existe como directorio.");
            if (ClassFileSystem.existeFichero(nombreAbsoluto))
            {
               if (!ClassFileSystem.rm(nombreAbsoluto, out mensajeError))
                  throw new Exception(mensajeError);
            }
            // generamos los parámetros
            string parametros = opciones + " \"" + nombreAbsoluto + "\" " + ((!string.IsNullOrEmpty(opcionListaFicheros)) ? opcionListaFicheros : "") + "\"" + ficheroListaFicheros + "\""; 
            Process proceso = new Process();
            proceso.StartInfo.FileName = ejecutable;
            proceso.StartInfo.Arguments = parametros;
            proceso.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            proceso.Start();
            proceso.WaitForExit();
            return true;
         } catch (Exception exc)
         {
            MessageBox.Show(exc.Message, "ERROR AL EJECUTAR LA COPIA DE SEGURIDAD", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
         }
      }

      // función que ejecuta las copias de seguridad
      private void ejecutarCopiasSeguridad()
      {
         if (gestorBD != null)
         {
            List<Dictionary<string, string?>>? datosCS = gestorBD.obtenerDatosMinimosCopiasSeguridadSeleccionadas(datosFiltro);
            if (gestorBD.resultado)
            {
               if (datosCS != null && datosCS.Count > 0)
               {
                  string? ejecutable, opciones, opcionListaFicheros;
                  bool errorLectura;
                  (ejecutable, opciones, opcionListaFicheros, errorLectura) = obtenerEjecutableCS();
                  if (!errorLectura)
                  {
                     if (string.IsNullOrEmpty(ejecutable) || string.IsNullOrEmpty(opciones))
                     {
                        string mError = "No se puede ejecutar la copia de seguridad porque:";
                        if (string.IsNullOrEmpty(ejecutable))
                           mError += "\r\nNo está definido el ejecutable que lo efectuará.";
                        if (string.IsNullOrEmpty(opciones))
                           mError += "\r\nNo están definidas la opciones del ejecutable";
                        MessageBox.Show(mError, "EJECUCIÓN IMPOSIBLE DE LAS COPIAS DE SEGURIDAD", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                     }
                     foreach (Dictionary<string, string?> copiaSeguridad in datosCS)
                     {
                        // generamos el fichero con la lista de archivos y directorios cuya copia de seguridad queremos efectuar
                        ClassGeneradorFicheroLista generadorFicheroLista = new ClassGeneradorFicheroLista(gestorBD, copiaSeguridad[sDatosTablaDatosCS.colIdentificador], datosFiltro);
                        if (generadorFicheroLista.resultado)
                        {
                           if (!generadorFicheroLista.ficheroVacio)
                           {
                              string? idCS = !string.IsNullOrEmpty(copiaSeguridad[sDatosTablaDatosCS.colIdentificador]) ? copiaSeguridad[sDatosTablaDatosCS.colIdentificador] : null;
                              string? nombre = !string.IsNullOrEmpty(copiaSeguridad[sDatosTablaDatosCS.colNombre]) ? copiaSeguridad[sDatosTablaDatosCS.colNombre] : null;
                              string? destino = !string.IsNullOrEmpty(copiaSeguridad[sDatosTablaDatosCS.colDestino]) ? copiaSeguridad[sDatosTablaDatosCS.colDestino] : null;
                              string ficheroListaFicheros = string.IsNullOrEmpty(generadorFicheroLista.nombreAbsoluto) ? "" : generadorFicheroLista.nombreAbsoluto;
                              if (!ejecutarCS(ejecutable, opciones, opcionListaFicheros, idCS, nombre, destino, ficheroListaFicheros))
                                 return;
                           }
                           else
                              MessageBox.Show("Copia de seguridad sin ningún fichero a añadir.", "COPIA DE SEGURIDAD VACÍA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                           string mError;
                           if (!ClassFileSystem.rm(generadorFicheroLista.nombreAbsoluto, out mError))
                           {
                              MessageBox.Show(mError, "ERROR AL BORRAR FICHERO TEMPORAL", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                              return;
                           }
                        }
                        else
                        {
                           MessageBox.Show(generadorFicheroLista.mensajeError, "ERROR AL GENERAR FICHERO CON LISTA DE FICHEROS Y DIRECTORIOS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                           return;
                        }
                     }
                     MessageBox.Show("Las copias de seguridad se han efectuado correctamente.", "COPIAS DE SEGURIDAD OK", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  }
               }
               else
                  MessageBox.Show("No existe ninguna copia de seguridad seleccionada en la base de datos.", "FALTAN COPIAS DE SEGURIDAD", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
               MessageBox.Show(gestorBD.mensajeError, "ERROR AL OBTENER LA INFORMACIÓN DE LAS COPIAS DE SEGURIDAD SELECCIONADAS", MessageBoxButtons.OK, MessageBoxIcon.Information);
         }
      }

      // función que maneja el temporizador para actualizar el datagrid con los datos actualizados
      private void manejadorTemporizadorActualizacionDatagrid (object sender, EventArgs e)
      {
         if (sender is System.Windows.Forms.Timer)
         {
            System.Windows.Forms.Timer temporizador = (System.Windows.Forms.Timer)sender;
            temporizador.Stop();
            temporizador.Tick -= manejadorTemporizadorActualizacionDatagrid;
            if (!inicializarDatagrid())
               Close();
            ficherosCopiaSeguridad = null;
            activarBotonEditarCopiaSeguridad();
            activarBotonEliminarCopiaSeguridad();
            establecerEtiquetaBotonActivarDesactivarBitSeleccion();
            establecerEtiquetaBotonSeleccionarDeseleccionarFilas();
            activarBotonSeleccionarDeseleccionarFilas();
            activarBotonActivarDesactivarBitSeleccion();
            activarBotonEjecutar();
         }
      }

      // función que maneja la finalización de la ventana de agregar copia de seguridad
      private void manejadorCierreVentanaAgregarCopiaSeguridad(object sender, EventArgs e)
      {
         if (sender is FormEditarAgregarCopiaSeguridad)
         {
            FormEditarAgregarCopiaSeguridad? ventana = sender as FormEditarAgregarCopiaSeguridad;
            if (ventana != null && gestorBD != null)
            {
               ventana.FormClosed -= manejadorCierreVentanaAgregarCopiaSeguridad;
               // comprobamos si la operación ha sido cancelada o no
               if (!ventana.operacionCancelada)
               {
                  // operación no cancelada -> obtenemos la información que precisamos
                  string? identificador = ventana.identificador;
                  string? nombre = ventana.nombre;
                  string? destino = ventana.destino;
                  string? descripcion = ventana.descripcion;
                  ficherosCopiaSeguridad = ventana.ficheros;
                  // creamos la transacción para almacenarla
                  gestorBD.crearTransaccion();
                  if (gestorBD.resultado)
                  {
                     // se ha podido crear la transacción -> almacenaremos la información
                     gestorBD.almacenarCopiaSeguridad(identificador, nombre, destino, descripcion);
                     if (gestorBD.resultado)
                     {
                        gestorBD.borrarFicherosYDirectoriosSeleccionadosDeCS(identificador);
                        if (gestorBD.resultado)
                        {
                           gestorBD.almacenarListaFicherosYDirectoriosSeleccionadosDeUnaCopiaDeSeguridad(identificador, ficherosCopiaSeguridad);
                           if (gestorBD.resultado)
                           {
                              gestorBD.aceptarTransaccion();
                              if (gestorBD.resultado)
                              {
                                 System.Windows.Forms.Timer temporizador = new System.Windows.Forms.Timer();
                                 temporizador.Interval = 1000;
                                 temporizador.Tick += manejadorTemporizadorActualizacionDatagrid;
                                 temporizador.Start();
                              }
                              else
                                 MessageBox.Show(gestorBD.mensajeError, "ERROR AL DAR POR VÁLIDA LA TRANSACCIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                           }
                           else
                           {
                              MessageBox.Show(gestorBD.mensajeError, "ERROR AL ALMACENAR LOS FICHEROS Y DIRECTORIOS SELECCIONADOS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                              gestorBD.anularTransaccion();
                              if (!gestorBD.resultado)
                                 MessageBox.Show(gestorBD.mensajeError, "ERROR AL ANULAR TRANSACCIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                           }
                        }
                        else
                        {
                           MessageBox.Show(gestorBD.mensajeError, "ERROR AL BORRAR LOS FICHEROS Y DIRECTORIOS ANTIGUOS DE LA COPIA DE SEGURIDAD", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                           gestorBD.anularTransaccion();
                           if (!gestorBD.resultado)
                              MessageBox.Show(gestorBD.mensajeError, "ERROR AL ANULAR TRANSACCIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                     }
                     else
                     {
                        MessageBox.Show(gestorBD.mensajeError, "ERROR DE CREACIÓN DE COPIA DE SEGURIDAD", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        gestorBD.anularTransaccion();
                        if (!gestorBD.resultado)
                           MessageBox.Show(gestorBD.mensajeError, "ERROR AL ANULAR TRANSACCIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                     }
                  }
               }
            }
         }
      }

      // función que maneja la finalización de la ventana de modificar copia seguridad
      private void manejadorCierreVentanaModificarCopiaSeguridad(object sender, EventArgs e)
      {
         if (sender is FormEditarAgregarCopiaSeguridad)
         {
            FormEditarAgregarCopiaSeguridad? ventana = sender as FormEditarAgregarCopiaSeguridad;
            if (ventana != null && gestorBD != null)
            {
               ventana.FormClosed -= manejadorCierreVentanaModificarCopiaSeguridad;
               // comprobamos si la operación ha sido cancelada o no
               if (!ventana.operacionCancelada)
               {
                  // operación no cancelada -> obtenemos la información que precisamos
                  string? identificador = ventana.identificador;
                  string? nombre = ventana.nombre;
                  string? destino = ventana.destino;
                  string? descripcion = ventana.descripcion;
                  ficherosCopiaSeguridad = ventana.ficheros;
                  // creamos la transacción para almacenarla
                  gestorBD.crearTransaccion();
                  if (gestorBD.resultado)
                  {
                     // se ha podido crear la transacción -> actualizaremos la información
                     gestorBD.actualizarCopiaSeguridad(identificador, nombre, destino, descripcion);
                     if (gestorBD.resultado)
                     {
                        gestorBD.borrarFicherosYDirectoriosSeleccionadosDeCS(identificador);
                        if (gestorBD.resultado)
                        {
                           gestorBD.almacenarListaFicherosYDirectoriosSeleccionadosDeUnaCopiaDeSeguridad(identificador, ficherosCopiaSeguridad);
                           if (gestorBD.resultado)
                           {
                              gestorBD.aceptarTransaccion();
                              if (gestorBD.resultado)
                              {
                                 System.Windows.Forms.Timer temporizador = new System.Windows.Forms.Timer();
                                 temporizador.Interval = 1000;
                                 temporizador.Tick += manejadorTemporizadorActualizacionDatagrid;
                                 temporizador.Start();
                              }
                              else
                                 MessageBox.Show(gestorBD.mensajeError, "ERROR AL DAR POR VÁLIDA LA TRANSACCIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                           }
                           else
                           {
                              MessageBox.Show(gestorBD.mensajeError, "ERROR AL ALMACENAR LOS FICHEROS Y DIRECTORIOS SELECCIONADOS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                              gestorBD.anularTransaccion();
                              if (!gestorBD.resultado)
                                 MessageBox.Show(gestorBD.mensajeError, "ERROR AL ANULAR TRANSACCIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                           }
                        }
                        else
                        {
                           MessageBox.Show(gestorBD.mensajeError, "ERROR AL BORRAR LOS FICHEROS Y DIRECTORIOS ANTIGUOS DE LA COPIA DE SEGURIDAD", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                           gestorBD.anularTransaccion();
                           if (!gestorBD.resultado)
                              MessageBox.Show(gestorBD.mensajeError, "ERROR AL ANULAR TRANSACCIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                     }
                     else
                     {
                        MessageBox.Show(gestorBD.mensajeError, "ERROR AL MODIFICAR COPIA DE SEGURIDAD", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        gestorBD.anularTransaccion();
                        if (!gestorBD.resultado)
                           MessageBox.Show(gestorBD.mensajeError, "ERROR AL ANULAR TRANSACCIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                     }
                  }
               }
            }
         }
      }

      /****** MANEJADORES DE LA VENTANA ******/
      /***************************************/
      // al cargar la ventana
      private void FormCopiaSeguridad_Load(object sender, EventArgs e)
      {
         // ocultaremos la ventana principal de la aplicación
         if (ventanaPrincipal != null)
            ventanaPrincipal.Hide();
      }

      // al cierre de la ventana

      private void FormCopiaSeguridad_FormClosed(object sender, FormClosedEventArgs e)
      {
         // mostraremos la ventana principal de la aplicación
         if (ventanaPrincipal != null)
            ventanaPrincipal.Show();
      }

      // al mostrar por primera vez la ventana
      private void FormCopiaSeguridad_Shown(object sender, EventArgs e)
      {
         // comprobamos si tenemos definido el gestor del motor de la base de datos
         if (gestorBD == null)
         {
            MessageBox.Show("No se puede acceder a la copia de seguridad porque no está definido el gestor del motor de la base de datos.","ERROR INTERNO DE LA APLICACIÓN",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
            Close();
            return;
         }
         inicializarSelectorCamposFiltro();
         inicializarTextBoxEntradaValorFiltro();
         if (!inicializarTamannoColumnasDatagrid())
         {
            Close();
            return;
         }
         if (!inicializarDatagrid())
         {
            Close();
            return;
         }
         if (!inicializarEntradaValor())
         {
            Close();
            return;
         }
         establecerEtiquetaBotonActivarDesactivarBitSeleccion();
         establecerEtiquetaBotonSeleccionarDeseleccionarFilas();
         activarBotonEditarCopiaSeguridad();
         activarBotonEliminarCopiaSeguridad();
         activarBotonSeleccionarDeseleccionarFilas();
         activarBotonActivarDesactivarBitSeleccion();
         activarBotonEjecutar();
      }

      // se ha pulsado el botón "Ejecutar"
      private void buttonEjecutar_Click(object sender, EventArgs e)
      {
         ejecutarCopiasSeguridad();
      }

      // se ha pulsado el botón "Cerrar"
      private void buttonCerrar_Click(object sender, EventArgs e)
      {
         Close();
      }

      // se ha pulsado el botón "Aplicar" de la sección del filtro
      private void buttonAplicar_Click(object sender, EventArgs e)
      {
         datosFiltro.campo = cbSelectorCampo.SelectedItem as string;
         datosFiltro.valor = (string.IsNullOrEmpty(textBoxValor.Text)) ? "" : textBoxValor.Text;
         if (!inicializarDatagrid())
         {
            Close();
            return;
         }
         activarBotonEditarCopiaSeguridad();
         activarBotonEliminarCopiaSeguridad();
         establecerEtiquetaBotonActivarDesactivarBitSeleccion();
         establecerEtiquetaBotonSeleccionarDeseleccionarFilas();
         activarBotonSeleccionarDeseleccionarFilas();
         activarBotonActivarDesactivarBitSeleccion();
         activarBotonEjecutar();
      }

      // se ha cambiado la opción del selector de campo
      private void cbSelectorCampo_SelectedIndexChanged(object sender, EventArgs e)
      {
         datosFiltro.campo = cbSelectorCampo.SelectedItem as string;
         if (!inicializarEntradaValor())
         {
            Close();
            return;
         }
      }

      // se ha pulsado el botón "Agregar copìa de seguridad"
      private void buttonAgregarCS_Click(object sender, EventArgs e)
      {
         FormEditarAgregarCopiaSeguridad ventana = new FormEditarAgregarCopiaSeguridad();
         ventana.ventanaPadre = this;
         ventana.modoEdicion = modoEdicion.AgregarCS;
         ventana.gestorBD = gestorBD;
         ventana.ficheros = ficherosCopiaSeguridad;
         ventana.FormClosed += manejadorCierreVentanaAgregarCopiaSeguridad;
         ventana.Show();
      }

      // se ha pulsado el botón "Editar copia de seguridad"
      private void buttonEditarCS_Click(object sender, EventArgs e)
      {
         if (gestorBD != null)
         {
            FormEditarAgregarCopiaSeguridad ventana = new FormEditarAgregarCopiaSeguridad();
            ventana.ventanaPadre = this;
            ventana.modoEdicion = modoEdicion.EditarCS;
            ventana.gestorBD = gestorBD;
            string? identificador, nombre, descripcion, destino;
            bool seleccionada;
            if (obtenerDatosCSSeleccionada(out identificador, out nombre, out descripcion, out destino, out seleccionada))
            {
               if (!string.IsNullOrEmpty(identificador))
               {
                  ficherosCopiaSeguridad = gestorBD.obtenerListaFicherosYDirectoriosSeleccionados(identificador);
                  if (gestorBD.resultado)
                  {
                     ventana.ficheros = ficherosCopiaSeguridad;
                     ventana.identificador = identificador;
                     ventana.nombre = nombre;
                     ventana.descripcion = descripcion;
                     ventana.destino = destino;
                     ventana.FormClosed += manejadorCierreVentanaModificarCopiaSeguridad;
                     ventana.Show();
                  }
                  else
                     MessageBox.Show(gestorBD.mensajeError, "ERROR AL OBTENER LOS FICHEROS Y DIRECTORIOS DE LA COPIA DE SEGURIDAD", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
               }
            }
         }
      }

      // se ha cambiado la selección
      private void dgvDatosCS_SelectionChanged(object sender, EventArgs e)
      {
         // activamos o desactivamos el botón "Editar copia de seguridad"
         activarBotonEditarCopiaSeguridad();
         // activamos o desactivamos el botón "Eliminar copia de seguridad"
         activarBotonEliminarCopiaSeguridad();
      }

      // se ha cambiado el valor de una celda, esto es, 
      private void dgvDatosCS_CellValueChanged(object sender, DataGridViewCellEventArgs e)
      {
         if (gestorBD != null)
         {
            if (!bloquearEdicionCeldaSeleccionada)
            {
               int fila = e.RowIndex,
                   columna = e.ColumnIndex;
               try
               {
                  if (columna == kColSeleccionada)
                  {
                     string? idCS = (dgvDatosCS.Rows[fila].Cells[kColIdentificador].Value != null) ? dgvDatosCS.Rows[fila].Cells[kColIdentificador].Value.ToString() : null;
                     if (idCS != null)
                     {
                        bool valor = (bool)dgvDatosCS.Rows[fila].Cells[columna].Value;
                        gestorBD.actualizarCampoSeleccionadaEnDatosCS(idCS, valor);
                        if (gestorBD.resultado)
                        {
                           establecerEtiquetaBotonActivarDesactivarBitSeleccion();
                           activarBotonEjecutar();
                        }
                        else
                           MessageBox.Show(gestorBD.mensajeError, "ERROR AL ACTUALIZAR FLAG SELECCIONADA EN BASE DE DATOS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                     }
                  }
               }
               catch (ArgumentOutOfRangeException)
               {
                  return;
               }
            }
            else
               bloquearEdicionCeldaSeleccionada = false;
         }
      }

      // se ha pulsado el botón "Activar/Desactivar seleccionada"

      private void buttonActivarDesactivarSeleccionada_Click(object sender, EventArgs e)
      {
         if (gestorBD != null)
         {
            // activamos o desactivamos todas las celdas que aparecen en el datagrid
            for (int i = 0; i < dgvDatosCS.Rows.Count; i++)
            {
               // antes de nada, crearemos una transacción
               if (i == 0)
               {
                  gestorBD.crearTransaccion();
                  if (!gestorBD.resultado)
                  {
                     MessageBox.Show(gestorBD.mensajeError, "ERROR AL CREAR LA TRANSACCIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                     return;
                  }
               }
               // en primer lugar, activaremos o desactivaremos la celda de turno
               bloquearEdicionCeldaSeleccionada = true;
               bool valorAntiguo = (bool)dgvDatosCS.Rows[i].Cells[kColSeleccionada].Value;
               dgvDatosCS.Rows[i].Cells[kColSeleccionada].Value = activarDesactivarBitSeleccion;
               bloquearEdicionCeldaSeleccionada = false;
               // actualizamos la base de datos
               string? idCS = (dgvDatosCS.Rows[i].Cells[kColIdentificador].Value != null) ? dgvDatosCS.Rows[i].Cells[kColIdentificador].Value.ToString() : null;
               if (idCS != null)
               {
                  gestorBD.actualizarCampoSeleccionadaEnDatosCS(idCS, activarDesactivarBitSeleccion);
                  if (!gestorBD.resultado)
                  {
                     bloquearEdicionCeldaSeleccionada = true;
                     dgvDatosCS.Rows[i].Cells[kColSeleccionada].Value = valorAntiguo;
                     bloquearEdicionCeldaSeleccionada = false;
                     MessageBox.Show(gestorBD.mensajeError, "ERROR AL ACTUALIZAR FLAG SELECCIONADA EN BASE DE DATOS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                     gestorBD.anularTransaccion();
                     if (!gestorBD.resultado)
                        MessageBox.Show(gestorBD.mensajeError, "ERROR AL ANULAR LA TRANSACCIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                     return;
                  }
               }
            }
            // finalmente, daremos por válida la transacción
            gestorBD.aceptarTransaccion();
            if (gestorBD.resultado)
            {
               establecerEtiquetaBotonActivarDesactivarBitSeleccion();
               activarBotonEjecutar();
            }
            else
               MessageBox.Show(gestorBD.mensajeError, "ERROR AL DAR POR VÁLIDA LA TRANSACCIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
         }
      }

      // se ha pulsado el botón "Eliminar copia de seguridad"
      private void buttonEliminarCopiasSeguridad_Click(object sender, EventArgs e)
      {
         if (gestorBD != null)
         {
            // en primer lugar, borraremos lo seleccionado de la base de datos
            bool creadaTransaccion = false;
            for (int i = 0; i < dgvDatosCS.SelectedRows.Count; i++)
            {
               // obtenemos el pertinente identificador de la copia de seguridad
               string? idCS = (dgvDatosCS.SelectedRows[i].Cells[kColIdentificador].Value != null) ? dgvDatosCS.SelectedRows[i].Cells[kColIdentificador].Value.ToString() : null;
               if (!string.IsNullOrEmpty(idCS))
               {
                  if (i == 0)
                  {
                     gestorBD.crearTransaccion();
                     if (!gestorBD.resultado)
                     {
                        MessageBox.Show(gestorBD.mensajeError, "ERROR AL CREAR TRANSACCIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                     }
                     creadaTransaccion = true;
                  }
                  // borramos la lista de ficheros y carpetas seleccionadas de la copia de seguridad
                  gestorBD.borrarFicherosYDirectoriosSeleccionadosDeCS(idCS);
                  if (!gestorBD.resultado)
                  {
                     MessageBox.Show(gestorBD.mensajeError, "ERROR AL ELIMINAR FICHEROS Y DIRECTORIOS SELECCIONADOS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                     gestorBD.anularTransaccion();
                     if (!gestorBD.resultado)
                        MessageBox.Show(gestorBD.mensajeError, "ERROR AL ANULAR TRANSACCIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                     return;
                  }
                  gestorBD.borrarCopiaSeguridad(idCS);
                  if (!gestorBD.resultado)
                  {
                     MessageBox.Show(gestorBD.mensajeError, "ERROR AL BORRAR LA COPIA DE SEGURIDAD", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                     gestorBD.anularTransaccion();
                     if (!gestorBD.resultado)
                        MessageBox.Show(gestorBD.mensajeError, "ERROR AL ANULAR TRANSACCIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                     return;
                  }
               }
            }
            if (creadaTransaccion)
            {
               gestorBD.aceptarTransaccion();
               if (gestorBD.resultado)
               {
                  System.Windows.Forms.Timer temporizador = new System.Windows.Forms.Timer();
                  temporizador.Interval = 1000;
                  temporizador.Tick += manejadorTemporizadorActualizacionDatagrid;
                  temporizador.Start();
               }
               else
                  MessageBox.Show(gestorBD.mensajeError, "ERROR AL DAR POR VÁLIDA LA TRANSACCIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
               return;

            }
         }
      }

      // se ha pulsado el botón "Seleccionar/Deseleccionar filas"
      private void buttonSeleccionarDeseleccionarFilas_Click(object sender, EventArgs e)
      {
         // comprobamos si existen filas seleccionadas en el datagrid
         switch (existenFilasSeleccionadasEnElDatagrid())
         {
            case false:                                                                // no existen filas seleccionadas -> seleccionaremos todas
               for(int i=0;i<dgvDatosCS.Rows.Count;i++)
                  dgvDatosCS.Rows[i].Selected = true;
               break;
            case true:                                                                 // existen filas seleccionadas -> las deseleccionaremos
               for(int i=dgvDatosCS.SelectedRows.Count-1;i>=0;i--)
                  dgvDatosCS.Rows[dgvDatosCS.SelectedRows[i].Index].Selected = false;
               break;
         }
         // finalmente, cambiaremos el título del botón según el estado en el que esté
         establecerEtiquetaBotonSeleccionarDeseleccionarFilas();
      }
   }
}
