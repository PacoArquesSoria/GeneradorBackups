namespace GeneradorBackups
{
   partial class FormAcercaDe
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
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

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         labelGeneradorDeBackups = new Label();
         labelVersion = new Label();
         labelCopyright = new Label();
         groupBoxInfoSistema = new GroupBox();
         labelCantidadMemoria = new Label();
         labelResolucion = new Label();
         labelProceso = new Label();
         labelNumProcesadores = new Label();
         labelSistemaOperativo = new Label();
         groupBoxInfoSistema.SuspendLayout();
         SuspendLayout();
         // 
         // labelGeneradorDeBackups
         // 
         labelGeneradorDeBackups.AutoSize = true;
         labelGeneradorDeBackups.Font = new Font("Century Gothic", 27.75F, FontStyle.Bold, GraphicsUnit.Point);
         labelGeneradorDeBackups.ForeColor = Color.Red;
         labelGeneradorDeBackups.Location = new Point(35, 35);
         labelGeneradorDeBackups.Name = "labelGeneradorDeBackups";
         labelGeneradorDeBackups.Size = new Size(436, 44);
         labelGeneradorDeBackups.TabIndex = 0;
         labelGeneradorDeBackups.Text = "Generador de Backups";
         // 
         // labelVersion
         // 
         labelVersion.AutoSize = true;
         labelVersion.Font = new Font("Lucida Sans Unicode", 9F, FontStyle.Regular, GraphicsUnit.Point);
         labelVersion.Location = new Point(44, 79);
         labelVersion.Name = "labelVersion";
         labelVersion.Size = new Size(74, 16);
         labelVersion.TabIndex = 1;
         labelVersion.Text = "Versión 1.1";
         // 
         // labelCopyright
         // 
         labelCopyright.AutoSize = true;
         labelCopyright.Font = new Font("Lucida Sans Unicode", 9F, FontStyle.Regular, GraphicsUnit.Point);
         labelCopyright.Location = new Point(44, 95);
         labelCopyright.Name = "labelCopyright";
         labelCopyright.Size = new Size(201, 16);
         labelCopyright.TabIndex = 2;
         labelCopyright.Text = "© 2022, 2023 Paco Arques Soria";
         // 
         // groupBoxInfoSistema
         // 
         groupBoxInfoSistema.Controls.Add(labelCantidadMemoria);
         groupBoxInfoSistema.Controls.Add(labelResolucion);
         groupBoxInfoSistema.Controls.Add(labelProceso);
         groupBoxInfoSistema.Controls.Add(labelNumProcesadores);
         groupBoxInfoSistema.Controls.Add(labelSistemaOperativo);
         groupBoxInfoSistema.ForeColor = Color.Red;
         groupBoxInfoSistema.Location = new Point(35, 158);
         groupBoxInfoSistema.Name = "groupBoxInfoSistema";
         groupBoxInfoSistema.Size = new Size(750, 143);
         groupBoxInfoSistema.TabIndex = 3;
         groupBoxInfoSistema.TabStop = false;
         groupBoxInfoSistema.Text = "Información del sistema";
         // 
         // labelCantidadMemoria
         // 
         labelCantidadMemoria.AutoSize = true;
         labelCantidadMemoria.Font = new Font("Lucida Sans Unicode", 9F, FontStyle.Regular, GraphicsUnit.Point);
         labelCantidadMemoria.ForeColor = Color.SkyBlue;
         labelCantidadMemoria.Location = new Point(17, 104);
         labelCantidadMemoria.Name = "labelCantidadMemoria";
         labelCantidadMemoria.Size = new Size(349, 16);
         labelCantidadMemoria.TabIndex = 4;
         labelCantidadMemoria.Text = "Total memoria RAM: X Mb (X Mb usados / Mb bytes libres)";
         // 
         // labelResolucion
         // 
         labelResolucion.AutoSize = true;
         labelResolucion.Font = new Font("Lucida Sans Unicode", 9F, FontStyle.Regular, GraphicsUnit.Point);
         labelResolucion.ForeColor = Color.Green;
         labelResolucion.Location = new Point(17, 84);
         labelResolucion.Name = "labelResolucion";
         labelResolucion.Size = new Size(328, 16);
         labelResolucion.TabIndex = 3;
         labelResolucion.Text = "Resolución de la pantalla principal: XxY X bits por píxel";
         // 
         // labelProceso
         // 
         labelProceso.AutoSize = true;
         labelProceso.Font = new Font("Lucida Sans Unicode", 9F, FontStyle.Regular, GraphicsUnit.Point);
         labelProceso.ForeColor = Color.Green;
         labelProceso.Location = new Point(17, 64);
         labelProceso.Name = "labelProceso";
         labelProceso.Size = new Size(141, 16);
         labelProceso.TabIndex = 2;
         labelProceso.Text = "Proceso XXXXX (X bits)";
         // 
         // labelNumProcesadores
         // 
         labelNumProcesadores.AutoSize = true;
         labelNumProcesadores.Font = new Font("Lucida Sans Unicode", 9F, FontStyle.Regular, GraphicsUnit.Point);
         labelNumProcesadores.ForeColor = Color.Green;
         labelNumProcesadores.Location = new Point(17, 44);
         labelNumProcesadores.Name = "labelNumProcesadores";
         labelNumProcesadores.Size = new Size(168, 16);
         labelNumProcesadores.TabIndex = 1;
         labelNumProcesadores.Text = "X procesadores disponibles";
         // 
         // labelSistemaOperativo
         // 
         labelSistemaOperativo.AutoSize = true;
         labelSistemaOperativo.Font = new Font("Lucida Sans Unicode", 9F, FontStyle.Regular, GraphicsUnit.Point);
         labelSistemaOperativo.ForeColor = Color.Green;
         labelSistemaOperativo.Location = new Point(17, 24);
         labelSistemaOperativo.Name = "labelSistemaOperativo";
         labelSistemaOperativo.Size = new Size(109, 16);
         labelSistemaOperativo.TabIndex = 0;
         labelSistemaOperativo.Text = "Sistema operativo";
         // 
         // FormAcercaDe
         // 
         AutoScaleDimensions = new SizeF(7F, 15F);
         AutoScaleMode = AutoScaleMode.Font;
         ClientSize = new Size(819, 336);
         Controls.Add(groupBoxInfoSistema);
         Controls.Add(labelCopyright);
         Controls.Add(labelVersion);
         Controls.Add(labelGeneradorDeBackups);
         FormBorderStyle = FormBorderStyle.FixedSingle;
         MaximizeBox = false;
         MinimizeBox = false;
         Name = "FormAcercaDe";
         ShowIcon = false;
         ShowInTaskbar = false;
         StartPosition = FormStartPosition.CenterScreen;
         Text = "Acerca de ...";
         FormClosed += FormAcercaDe_FormClosed;
         Load += FormAcercaDe_Load;
         Shown += FormAcercaDe_Shown;
         groupBoxInfoSistema.ResumeLayout(false);
         groupBoxInfoSistema.PerformLayout();
         ResumeLayout(false);
         PerformLayout();
      }

      #endregion

      private Label labelGeneradorDeBackups;
      private Label labelVersion;
      private Label labelCopyright;
      private GroupBox groupBoxInfoSistema;
      private Label labelSistemaOperativo;
      private Label labelNumProcesadores;
      private Label labelProceso;
      private Label labelResolucion;
      private Label labelCantidadMemoria;
   }
}