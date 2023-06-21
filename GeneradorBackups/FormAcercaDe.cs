using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.VisualBasic.Devices;

namespace GeneradorBackups
{
   public partial class FormAcercaDe : Form
   {
      /******* CONSTANTES DE LA CLASE ******/
      /*************************************/
      private const ulong megabyte = 1048576;                                             // número de bytes que tiene un megabyte

      /******* PROPIEDADES DE LA CLASE ******/
      /**************************************/
      public FormPrincipal? ventanaPrincipal
      {
         set; private get;
      }

      /******* MÉTODOS DE LA CLASE ******/
      /**********************************/
      // establece en pantalla qué sistema operativo estamos usando
      private void establecerSistemaOperativo()
      {
         string mensaje;
         string sistemaOperativo = Environment.OSVersion.ToString();
         mensaje = sistemaOperativo + " (" + (Environment.Is64BitOperatingSystem ? "64 bits" : "32 bits") + ")";
         labelSistemaOperativo.Text = mensaje;
      }

      // establece en pantalla el número de procesadores disponibles
      private void establecerNumeroProcesadoresDisponibles()
      {
         string mensaje = Environment.ProcessorCount + " procesadores disponibles";
         labelNumProcesadores.Text = mensaje;
      }

      // establece en pantalla el PID del proceso y si éste es de 32 o de 64 bits
      private void establecerProceso()
      {
         Process procesoActual = Process.GetCurrentProcess();
         string mensaje = "Proceso " + procesoActual.Id.ToString("D5") + " (" + (Environment.Is64BitProcess ? "64 bits" : "32 bits") + ")";
         labelProceso.Text = mensaje;
      }

      // establece en pantalla la resolución de la pantalla principal
      private void establecerResolucionPantallaPrincipal()
      {
         Size resolucion = Screen.PrimaryScreen.Bounds.Size;
         int bits = Screen.PrimaryScreen.BitsPerPixel;
         string mensaje = "Resolución de la pantalla principal: " + resolucion.Width + "x" + resolucion.Height + " " + bits + " bits por píxel";
         labelResolucion.Text = mensaje;
      }

      // establece en pantalla la cantidad de memoria RAM disponible
      private void establecerCantidadMemoriaRAM()
      {
         ComputerInfo informacionSistema = new ComputerInfo();
         ulong totalMemoriaRAM = informacionSistema.TotalPhysicalMemory;
         ulong memoriaRAMDisponible = informacionSistema.AvailablePhysicalMemory;
         ulong memoriaRAMUtilizada = totalMemoriaRAM - memoriaRAMDisponible;
         double totalMemoriaRAMEnMegas = (double)totalMemoriaRAM / (double)megabyte;
         double memoriaRAMDisponibleEnMegas = (double)memoriaRAMDisponible / (double)megabyte;
         double memoriaRAMUtilizadaEnMegas = (double)memoriaRAMUtilizada / (double)megabyte;
         string mensaje = "Total memoria RAM: " + totalMemoriaRAMEnMegas.ToString("N2") + " Mb (" + memoriaRAMUtilizadaEnMegas.ToString("N2") + " Mb utilizados / ";
         mensaje += memoriaRAMDisponibleEnMegas.ToString("N2") + " Mb libres)";
         labelCantidadMemoria.Text = mensaje;
      }

      // inicializa la ventana con la información que precisa
      private void inicializarVentana()
      {
         // establecemos el sistema operativo que estamos utilizando
         establecerSistemaOperativo();
         // establecemos el número de procesadores disponibles
         establecerNumeroProcesadoresDisponibles();
         // establecemos la información del proceso
         establecerProceso();
         // establecemos la resolución de la pantalla
         establecerResolucionPantallaPrincipal();
         // establecemos la cantidad de memoria RAM
         establecerCantidadMemoriaRAM();
      }

      /******* CONSTRUCTORES DE LA CLASE ******/
      /****************************************/
      // por defecto
      public FormAcercaDe()
      {
         InitializeComponent();
         ventanaPrincipal = null;
      }

      /****** MANEJADORES DE LA VENTANA ******/
      /***************************************/
      // al cargar la ventana
      private void FormAcercaDe_Load(object sender, EventArgs e)
      {
         if (ventanaPrincipal != null)
            ventanaPrincipal.Enabled = false;
      }

      // al cerrar la ventana
      private void FormAcercaDe_FormClosed(object sender, FormClosedEventArgs e)
      {
         if (ventanaPrincipal != null)
            ventanaPrincipal.Enabled = true;
      }

      // cuando se muestra la ventana
      private void FormAcercaDe_Shown(object sender, EventArgs e)
      {
         inicializarVentana();
      }
   }
}
