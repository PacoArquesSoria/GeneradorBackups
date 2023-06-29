namespace GeneradorBackups
{
   partial class FormPrincipal
   {
      /// <summary>
      ///  Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      ///  Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      private void InitializeComponent()
      {
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPrincipal));
         this.groupBoxOpcionesDisponibles = new System.Windows.Forms.GroupBox();
         this.rbConfiguracionAplicacion = new System.Windows.Forms.RadioButton();
         this.rbCopiaSeguridad = new System.Windows.Forms.RadioButton();
         this.rbConfiguracionProgramaBackup = new System.Windows.Forms.RadioButton();
         this.buttonAcercaDe = new System.Windows.Forms.Button();
         this.buttonSalir = new System.Windows.Forms.Button();
         this.buttonEjecutar = new System.Windows.Forms.Button();
         this.groupBoxOpcionesDisponibles.SuspendLayout();
         this.SuspendLayout();
         // 
         // groupBoxOpcionesDisponibles
         // 
         this.groupBoxOpcionesDisponibles.Controls.Add(this.rbConfiguracionAplicacion);
         this.groupBoxOpcionesDisponibles.Controls.Add(this.rbCopiaSeguridad);
         this.groupBoxOpcionesDisponibles.Controls.Add(this.rbConfiguracionProgramaBackup);
         this.groupBoxOpcionesDisponibles.Location = new System.Drawing.Point(35, 35);
         this.groupBoxOpcionesDisponibles.Name = "groupBoxOpcionesDisponibles";
         this.groupBoxOpcionesDisponibles.Size = new System.Drawing.Size(641, 100);
         this.groupBoxOpcionesDisponibles.TabIndex = 0;
         this.groupBoxOpcionesDisponibles.TabStop = false;
         this.groupBoxOpcionesDisponibles.Text = "Opciones disponibles";
         // 
         // rbConfiguracionAplicacion
         // 
         this.rbConfiguracionAplicacion.AutoSize = true;
         this.rbConfiguracionAplicacion.Location = new System.Drawing.Point(29, 62);
         this.rbConfiguracionAplicacion.Name = "rbConfiguracionAplicacion";
         this.rbConfiguracionAplicacion.Size = new System.Drawing.Size(186, 19);
         this.rbConfiguracionAplicacion.TabIndex = 2;
         this.rbConfiguracionAplicacion.TabStop = true;
         this.rbConfiguracionAplicacion.Text = "Configuración de la aplicación";
         this.rbConfiguracionAplicacion.UseVisualStyleBackColor = true;
         // 
         // rbCopiaSeguridad
         // 
         this.rbCopiaSeguridad.AutoSize = true;
         this.rbCopiaSeguridad.Location = new System.Drawing.Point(494, 37);
         this.rbCopiaSeguridad.Name = "rbCopiaSeguridad";
         this.rbCopiaSeguridad.Size = new System.Drawing.Size(128, 19);
         this.rbCopiaSeguridad.TabIndex = 1;
         this.rbCopiaSeguridad.TabStop = true;
         this.rbCopiaSeguridad.Text = "Copia de Seguridad";
         this.rbCopiaSeguridad.UseVisualStyleBackColor = true;
         // 
         // rbConfiguracionProgramaBackup
         // 
         this.rbConfiguracionProgramaBackup.AutoSize = true;
         this.rbConfiguracionProgramaBackup.Location = new System.Drawing.Point(29, 37);
         this.rbConfiguracionProgramaBackup.Name = "rbConfiguracionProgramaBackup";
         this.rbConfiguracionProgramaBackup.Size = new System.Drawing.Size(233, 19);
         this.rbConfiguracionProgramaBackup.TabIndex = 0;
         this.rbConfiguracionProgramaBackup.TabStop = true;
         this.rbConfiguracionProgramaBackup.Text = "Configuración del programa de Backup";
         this.rbConfiguracionProgramaBackup.UseVisualStyleBackColor = true;
         // 
         // buttonAcercaDe
         // 
         this.buttonAcercaDe.Location = new System.Drawing.Point(36, 148);
         this.buttonAcercaDe.Name = "buttonAcercaDe";
         this.buttonAcercaDe.Size = new System.Drawing.Size(75, 23);
         this.buttonAcercaDe.TabIndex = 1;
         this.buttonAcercaDe.Text = "Acerca de ...";
         this.buttonAcercaDe.UseVisualStyleBackColor = true;
         this.buttonAcercaDe.Click += new System.EventHandler(this.buttonAcercaDe_Click);
         // 
         // buttonSalir
         // 
         this.buttonSalir.Location = new System.Drawing.Point(601, 148);
         this.buttonSalir.Name = "buttonSalir";
         this.buttonSalir.Size = new System.Drawing.Size(75, 23);
         this.buttonSalir.TabIndex = 2;
         this.buttonSalir.Text = "Salir";
         this.buttonSalir.UseVisualStyleBackColor = true;
         this.buttonSalir.Click += new System.EventHandler(this.buttonSalir_Click);
         // 
         // buttonEjecutar
         // 
         this.buttonEjecutar.Location = new System.Drawing.Point(520, 148);
         this.buttonEjecutar.Name = "buttonEjecutar";
         this.buttonEjecutar.Size = new System.Drawing.Size(75, 23);
         this.buttonEjecutar.TabIndex = 3;
         this.buttonEjecutar.Text = "Ejecutar";
         this.buttonEjecutar.UseVisualStyleBackColor = true;
         this.buttonEjecutar.Click += new System.EventHandler(this.buttonEjecutar_Click);
         // 
         // FormPrincipal
         // 
         this.ClientSize = new System.Drawing.Size(712, 204);
         this.Controls.Add(this.buttonEjecutar);
         this.Controls.Add(this.buttonSalir);
         this.Controls.Add(this.buttonAcercaDe);
         this.Controls.Add(this.groupBoxOpcionesDisponibles);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.Icon = ((System.Drawing.Icon)resources.GetObject("$this.Icon"));
         this.MaximizeBox = false;
         this.Name = "FormPrincipal";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "Generador de Backups - Menú principal";
         this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormPrincipal_FormClosed);
         this.Shown += new System.EventHandler(this.FormPrincipal_Shown);
         this.groupBoxOpcionesDisponibles.ResumeLayout(false);
         this.groupBoxOpcionesDisponibles.PerformLayout();
         this.ResumeLayout(false);

      }

      private RadioButton rbConfiguracionAplicacion;

      private RadioButton rbCopiaSeguridad;
      private RadioButton rbConfiguracionProgramaBackup;
      private Button buttonAcercaDe;
      private Button buttonSalir;
      private Button buttonEjecutar;
      private GroupBox groupBoxOpcionesDisponibles;

   }
}