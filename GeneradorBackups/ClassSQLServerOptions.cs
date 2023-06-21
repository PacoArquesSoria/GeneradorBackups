using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneradorBackups
{
   /****** CLASE ClassSQLServerOptions ******/
   /*****************************************/
   // clase encargada de almacenar las opciones de configuración del motor de la base de datos SQL Server
   public class ClassSQLServerOptions
   {
      /****** PROPIEDADES DE LA CLASE ******/
      /*************************************/
      // servidor -> Almacena el servidor con el que nos conectaremos
      public string? servidor
      {
         set; get;
      }

      // autenticacionSQLServer -> Almacena un booleano que nos dice cómo hemos de autenticarnos en la base de datos (si por Windows, o por SQL Server)
      public bool autenticacionSQLServer
      {
         set; get;
      }

      // usuario -> Almacena el usuario con el que nos conectaremos
      public string? usuario
      {
         set; get;
      }

      // contrasenna -> Almacena la contraseña de acceso al motor de la base de datos
      public string? contrasenna
      {
         set; get;
      }

      // timeout -> Almacena el valor de la opción Timeout
      public int timeout
      {
         set; get;
      }

      // commandTimeout -> Almacena el valor de la opción CommandTimeout
      public int commandTimeout
      {
         set; get;
      }

      // azure -> Almacena el valor de la opción azure
      public bool azure
      {
         set; get;
      }

      // seguridadIntegrada -> Almacena el valor de la opción seguridadIntegrada
      public bool seguridadIntegrada
      {
         set; get;
      }

      // conexionConfiada -> Almacena el valor de la opción conexionConfiada
      public bool conexionConfiada
      {
         set; get;
      }
   }
}
