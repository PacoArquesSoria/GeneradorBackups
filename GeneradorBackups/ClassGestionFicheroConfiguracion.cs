using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace GeneradorBackups
{
   /****** CLASE ClassGestionFicheroConfiguracion ******/
   /****************************************************/
   // clase que controla el fichero de configuración de Generador de Backups

   public class ClassGestionFicheroConfiguracion
   {
      /****** CONSTANTES DE LA CLASE ******/
      /************************************/

      private const string kFicheroConfiguracion = "dataConection.xml";                // fichero relativo donde se almacenará la información
      private const string kKey = "A[]sasz32121KñaÑaERXXCVSZXASWqwertyAlbaceteSoriaGumersindaY/7&%&()=+*.-,;<<<<>>>>a123321zFAaAfaFaFAfAASGOASAZ456";
      
      /****** PROPIEDADES DE LA CLASE ******/
      /*************************************/
      // dirAplicacion -> Nos devuelve el directorio de la aplicación
      public string? dirAplicacion
      {
         get; private set;
      }

      // ficheroConfiguracion -> Nos devuelve el nombre absoluto del fichero de configuración
      public string? ficheroConfiguracion
      {
         get; private set;
      }

      // resultado -> Nos devuelve un booleano indicando si la operación de inicialización de la clase es correcta o no
      public bool resultado
      {
         get; private set;
      }

      // mensajeError -> Nos devuelve una cadena con el mensaje de error cometido
      public string? mensajeError
      {
         get; private set;
      }

      /****** CONSTRUCTORES DE LA CLASE ******/
      /***************************************/
      // por defecto
      public ClassGestionFicheroConfiguracion()
      {
         reset();
         init();
      }

      /****** MÉTODOS DE LA CLASE ******/
      /*********************************/
      // función que inicializa las variables encargadas de devolvernos el resultado de la operación
      private void reset()
      {
         resultado = true;
         mensajeError = "";
      }

      // función que obtiene el directorio de la aplicación
      private void obtenerDirectorioAplicacion()
      {
         dirAplicacion = Application.StartupPath;
         if (string.IsNullOrEmpty(dirAplicacion))
         {
            resultado = false;
            mensajeError = "No se ha podido obtener el directorio de la aplicación.";
         }
      }

      // función que obtiene el nombre absoluto del fichero de configuración de la aplicación
      private void obtenerNombreAbsolutoFicheroConfiguracion()
      {
         if (string.IsNullOrEmpty(dirAplicacion))
         {
            resultado = false;
            mensajeError = "No se puede obtener el nombre absoluto del fichero de configuración puesto que no está definido el directorio de la aplicación.";
         }
         else
            ficheroConfiguracion = dirAplicacion + "\\" + kFicheroConfiguracion;
      }

      // función que inicializa la clase
      private void init()
      {
         obtenerDirectorioAplicacion();
         if (resultado)
            obtenerNombreAbsolutoFicheroConfiguracion();
      }

      // función que comprueba si un archivo es un fichero ordinario o es una carpeta. Como parámetro se le pasa el archivo a verificar. Devuelve: 0 si no existe, 1 si es un fichero ordinario y 2 si
      // es carpeta.
      private byte comprobarArchivo(string archivo)
      {
         try
         {
            if (string.IsNullOrEmpty(archivo))
               throw new Exception("No se puede comprobar qué tipo es un archivo cuyo nombre no está definido.");
            // comprobamos qué tipo de archivo es
            if (Directory.Exists(archivo))
               // es un directorio -> devuelve 2 al procedimiento invocante
               return 2;
            else
            {
               if (File.Exists(archivo))
                  // es un fichero -> devuelve 1 al procedimiento invocante
                  return 1;
               else
                  // no existe -> devuelve 0 al procedimiento invocante
                  return 0;
            }

         }
         catch (Exception excepcion)
         {
            resultado = false;
            if (string.IsNullOrEmpty(archivo))
               mensajeError = excepcion.Message;
            else
            {
               mensajeError = "No se ha podido verificar el posible archivo " + archivo + ".\r\n";
               mensajeError += "Se ha producido el siguiente error:\r\n";
               mensajeError += excepcion.Message;
            }
            return 255;
         }
      }
      // función que encripta una cadena de caracteres empleando el algoritmo de tripleDES. Como parámetro se le pasa la cadena a encriptar. Además de la cadena
      // encriptada, devuelve un booleano indicando si la operación se ha efectuado correctamente o no y, en caso de error, el mensaje de error cometido.
      public static string? encriptar(string? plainText, out bool resultado, out string mensajeError)
      {
         try
         {
            // presuponemos inicialmente que no se producirá ningún error a la hora de encriptar la información -> ponemos el flag del resultado a true y la
            // cadena del mensaje de error a cadena vacía.
            resultado = true;
            mensajeError = "";

            // comprobaremos si la cadena a encriptar está definida o no
            if (string.IsNullOrEmpty(plainText))
            {
               // la cadena está sin definir o vale cadena vacía -> error -> pondremos el flag del resultado a false y generaremos el pertinente mensaje de error
               resultado = false;
               mensajeError = "No se puede encriptar una cadena vacía o que no está definida.";
               return null;
            }

            // Instantiate a new Aes object to perform string symmetric encryption
            Aes encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;

            // Create sha256 hash
            SHA256 mySHA256 = SHA256.Create();
            byte[] key = mySHA256.ComputeHash(Encoding.Unicode.GetBytes(kKey));
            // Create secret IV
            byte[] iv = new byte[16] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };

            // Set key and IV
            byte[] aesKey = new byte[32];
            Array.Copy(key, 0, aesKey, 0, 32);
            encryptor.Key = aesKey;
            encryptor.IV = iv;

            // Instantiate a new MemoryStream object to contain the encrypted bytes
            MemoryStream memoryStream = new MemoryStream();

            // Instantiate a new encryptor from our Aes object
            ICryptoTransform aesEncryptor = encryptor.CreateEncryptor();

            // Instantiate a new CryptoStream object to process the data and write it to the 
            // memory stream
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write);

            // Convert the plainText string into a byte array
            byte[] plainBytes = Encoding.Unicode.GetBytes(plainText);

            // Encrypt the input plaintext string
            cryptoStream.Write(plainBytes, 0, plainBytes.Length);

            // Complete the encryption process
            cryptoStream.FlushFinalBlock();

            // Convert the encrypted data from a MemoryStream to a byte array
            byte[] cipherBytes = memoryStream.ToArray();

            // Close both the MemoryStream and the CryptoStream
            memoryStream.Close();
            cryptoStream.Close();

            // Convert the encrypted byte array to a base64 encoded string
            string cipherText = Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);

            // Return the encrypted data as a string
            return cipherText;
         }
         catch (Exception excepcion)
         {
            // manejador de excepciones de la función -> pone el flag del resultado a false y genera el pertinente mensaje de error
            resultado = false;
            mensajeError = "No se ha podido cifrar la cadena \"" + plainText + "\".\r\n";
            mensajeError += "Se ha producido el siguiente error:\r\n";
            mensajeError += excepcion.Message;
            return null;
         }
      }

      // función que desencripta una cadena encriptada con tripleDES. Como parámetro se le pasa el texto a desencriptar. Además de la cadena desencriptada,
      // devolverá un booleano indicando si la operación se ha efectuado correctamente o no y, en caso de error, el mensaje de error generado.
      public static string? desencriptar(string? cipherText, out bool resultado, out string mensajeError)
      {
         try
         {
            // presuponemos inicialmente que no se producirá ningún error a la hora de desencriptar la información -> ponemos el flag del resultado a true y la
            // cadena del mensaje de error a cadena vacía.
            resultado = true;
            mensajeError = "";

            // comprobaremos si la cadena a desencriptar está definida o no
            if (string.IsNullOrEmpty(cipherText))
            {
               // la cadena está sin definir o vale cadena vacía -> error -> pondremos el flag del resultado a false y generaremos el pertinente mensaje de error
               resultado = false;
               mensajeError = "No se puede desencriptar una cadena vacía o que no está definida.";
               return null;
            }

            // Instantiate a new Aes object to perform string symmetric encryption
            Aes encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;

            // Create sha256 hash
            SHA256 mySHA256 = SHA256.Create();
            byte[] key = mySHA256.ComputeHash(Encoding.Unicode.GetBytes(kKey));
            // Create secret IV
            byte[] iv = new byte[16] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };

            // Set key and IV
            byte[] aesKey = new byte[32];
            Array.Copy(key, 0, aesKey, 0, 32);
            encryptor.Key = aesKey;
            encryptor.IV = iv;

            // Instantiate a new MemoryStream object to contain the encrypted bytes
            MemoryStream memoryStream = new MemoryStream();

            // Instantiate a new encryptor from our Aes object
            ICryptoTransform aesDecryptor = encryptor.CreateDecryptor();

            // Instantiate a new CryptoStream object to process the data and write it to the 
            // memory stream
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write);

            // Will contain decrypted plaintext
            string plainText = String.Empty;

            try
            {
               // Convert the ciphertext string into a byte array
               byte[] cipherBytes = Convert.FromBase64String(cipherText);

               // Decrypt the input ciphertext string
               cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);

               // Complete the decryption process
               cryptoStream.FlushFinalBlock();

               // Convert the decrypted data from a MemoryStream to a byte array
               byte[] plainBytes = memoryStream.ToArray();

               // Convert the decrypted byte array to string
               plainText = Encoding.Unicode.GetString(plainBytes, 0, plainBytes.Length);
            }
            finally
            {
               // Close both the MemoryStream and the CryptoStream
               memoryStream.Close();
               cryptoStream.Close();
            }

            // Return the decrypted data as a string
            return plainText;
         }
         catch (Exception excepcion)
         {
            // manejador de excepciones de la función -> pone el flag del resultado a false y genera el pertinente mensaje de error
            resultado = false;
            mensajeError = "No se ha podido descifrar la cadena \"" + cipherText + "\".\r\n";
            mensajeError += "Se ha producido el siguiente error:\r\n";
            mensajeError += excepcion.Message;
            return null;
         }
      }

      // función que crea el fichero de configuración con la información que le pasamos como parámetro (un objeto de tipo ClassServerConexionOptions). No devuelve nada.
      public void generarFicheroConfiguracion(ClassSQLServerOptions? opcionesConexion)
      {
         try
         {
            // comprobamos si tenemos datos para almacenar
            if (opcionesConexion == null)
               throw new Exception("No hay datos para almacenar.");
            XmlSerializer? escritor = null;
            StreamWriter? fichero = null;
            // codificamos las contraseñas
            // en primer lugar, encriptamos la contraseña del motor de base de datos SQL Server
            if (opcionesConexion.autenticacionSQLServer)
            {
               bool resOperacion;
               string? mError;
               string? contrasennaCodificada = encriptar(opcionesConexion.contrasenna, out resOperacion, out mError);
               if (resOperacion)
                  opcionesConexion.contrasenna = contrasennaCodificada;
               else
                  throw new Exception(mError);
            }
            else
            {
               opcionesConexion.usuario = "NONE";
               opcionesConexion.contrasenna = "NONE";
            }
            // finalmente, generaremos el fichero XML donde almacenaremos toda esta información
            escritor = new XmlSerializer(typeof(ClassSQLServerOptions));
            string fc = (string.IsNullOrEmpty(ficheroConfiguracion) ? "" : ficheroConfiguracion);
            fichero = new StreamWriter(fc);
            escritor.Serialize(fichero, opcionesConexion);
            // cerramos el fichero
            fichero.Close();
         }
         catch (Exception excepcion)
         {
            resultado = false;
            mensajeError = "No se ha podido generar el fichero de configuración.\r\n";
            mensajeError += "Se ha producido el siguiente error:\r\n";
            mensajeError += excepcion.Message;
         }
      }

      // función que lee el fichero de configuración. No se le pasa nada como parámetro y devuelve la información obtenida (un objeto de tipo ClassServerConexionOptions).
      public ClassSQLServerOptions? leerFicheroConfiguracion()
      {
         try
         {
            // comprobamos si existe el fichero de configuración
            if (!File.Exists(ficheroConfiguracion))
               // no existe el fichero de configuración -> devolveremos null
               return null;
            else
            {
               // el fichero de configuración existe -> procederemos con su lectura
               XmlSerializer? lector = new XmlSerializer(typeof(ClassSQLServerOptions));
               StreamReader? fichero = new StreamReader(ficheroConfiguracion);
               ClassSQLServerOptions? opciones = (ClassSQLServerOptions?) lector.Deserialize(fichero);
               fichero.Close();
               if (opciones == null)
                  throw new Exception("No se ha podido obtener las opciones de configuración de la aplicación.");
               // descodificaremos la contraseña si hay que hacerlo
               if (opciones.autenticacionSQLServer)
               {
                  bool resOperacion;
                  string mError;
                  // desencriptamos la contraseña del motor de base de datos SQL Server
                  string? contrasenna = desencriptar(opciones.contrasenna, out resOperacion, out mError);
                  if (resOperacion)
                     opciones.contrasenna = contrasenna;
                  else
                     throw new Exception(mError);
               }
               else
               {
                  opciones.usuario = "";
                  opciones.contrasenna = "";
               }
               // si llegamos hasta aquí -> todo ok -> devolveremos lo obtenido
               resultado = true;
               mensajeError = "";
               return opciones;
            }
         }
         catch (Exception excepcion)
         {
            resultado = false;
            mensajeError = "No se ha podido leer el fichero de configuración.\r\n";
            mensajeError += "Se ha producido el siguiente error:\r\n";
            mensajeError += excepcion.Message;
            return null;
         }
      }
   }
}
