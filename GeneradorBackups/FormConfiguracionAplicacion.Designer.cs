namespace GeneradorBackups
{
   partial class FormConfiguracionAplicacion
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
         this.labelServidor = new System.Windows.Forms.Label();
         this.textBoxServidor = new System.Windows.Forms.TextBox();
         this.groupBoxOpcionesAvanzadas = new System.Windows.Forms.GroupBox();
         this.buttonValoresPorDefecto = new System.Windows.Forms.Button();
         this.cbConexionConfiada = new System.Windows.Forms.CheckBox();
         this.cbSeguridadIntegrada = new System.Windows.Forms.CheckBox();
         this.cbAzure = new System.Windows.Forms.CheckBox();
         this.textBoxCommandTimeOut = new System.Windows.Forms.TextBox();
         this.labelCommandTimeOut = new System.Windows.Forms.Label();
         this.textBoxTimeOut = new System.Windows.Forms.TextBox();
         this.labelTimeOut = new System.Windows.Forms.Label();
         this.buttonAceptar = new System.Windows.Forms.Button();
         this.buttonCancelar = new System.Windows.Forms.Button();
         this.cbAutenticacionSQLServer = new System.Windows.Forms.CheckBox();
         this.groupBoxUsuarioSQLServer = new System.Windows.Forms.GroupBox();
         this.buttonVerOcultarContrasenna = new System.Windows.Forms.Button();
         this.textBoxContrasenna = new System.Windows.Forms.TextBox();
         this.labelContrasenna = new System.Windows.Forms.Label();
         this.textBoxUsuario = new System.Windows.Forms.TextBox();
         this.labelUsuario = new System.Windows.Forms.Label();
         this.groupBoxOpcionesAvanzadas.SuspendLayout();
         this.groupBoxUsuarioSQLServer.SuspendLayout();
         this.SuspendLayout();
         // 
         // labelServidor
         // 
         this.labelServidor.AutoSize = true;
         this.labelServidor.Location = new System.Drawing.Point(35, 35);
         this.labelServidor.Name = "labelServidor";
         this.labelServidor.Size = new System.Drawing.Size(50, 15);
         this.labelServidor.TabIndex = 0;
         this.labelServidor.Text = "Servidor";
         // 
         // textBoxServidor
         // 
         this.textBoxServidor.Location = new System.Drawing.Point(125, 35);
         this.textBoxServidor.Name = "textBoxServidor";
         this.textBoxServidor.Size = new System.Drawing.Size(547, 23);
         this.textBoxServidor.TabIndex = 1;
         this.textBoxServidor.TextChanged += new System.EventHandler(this.verificarEntradaTexto);
         // 
         // groupBoxOpcionesAvanzadas
         // 
         this.groupBoxOpcionesAvanzadas.Controls.Add(this.buttonValoresPorDefecto);
         this.groupBoxOpcionesAvanzadas.Controls.Add(this.cbConexionConfiada);
         this.groupBoxOpcionesAvanzadas.Controls.Add(this.cbSeguridadIntegrada);
         this.groupBoxOpcionesAvanzadas.Controls.Add(this.cbAzure);
         this.groupBoxOpcionesAvanzadas.Controls.Add(this.textBoxCommandTimeOut);
         this.groupBoxOpcionesAvanzadas.Controls.Add(this.labelCommandTimeOut);
         this.groupBoxOpcionesAvanzadas.Controls.Add(this.textBoxTimeOut);
         this.groupBoxOpcionesAvanzadas.Controls.Add(this.labelTimeOut);
         this.groupBoxOpcionesAvanzadas.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
         this.groupBoxOpcionesAvanzadas.ForeColor = System.Drawing.Color.Red;
         this.groupBoxOpcionesAvanzadas.Location = new System.Drawing.Point(35, 198);
         this.groupBoxOpcionesAvanzadas.Name = "groupBoxOpcionesAvanzadas";
         this.groupBoxOpcionesAvanzadas.Size = new System.Drawing.Size(637, 137);
         this.groupBoxOpcionesAvanzadas.TabIndex = 4;
         this.groupBoxOpcionesAvanzadas.TabStop = false;
         this.groupBoxOpcionesAvanzadas.Text = "Opciones avanzadas";
         // 
         // buttonValoresPorDefecto
         // 
         this.buttonValoresPorDefecto.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
         this.buttonValoresPorDefecto.ForeColor = System.Drawing.Color.Black;
         this.buttonValoresPorDefecto.Location = new System.Drawing.Point(12, 102);
         this.buttonValoresPorDefecto.Name = "buttonValoresPorDefecto";
         this.buttonValoresPorDefecto.Size = new System.Drawing.Size(154, 23);
         this.buttonValoresPorDefecto.TabIndex = 7;
         this.buttonValoresPorDefecto.Text = "Valores por defecto";
         this.buttonValoresPorDefecto.UseVisualStyleBackColor = true;
         this.buttonValoresPorDefecto.Click += new System.EventHandler(this.buttonValoresPorDefecto_Click);
         // 
         // cbConexionConfiada
         // 
         this.cbConexionConfiada.AutoSize = true;
         this.cbConexionConfiada.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
         this.cbConexionConfiada.ForeColor = System.Drawing.Color.Black;
         this.cbConexionConfiada.Location = new System.Drawing.Point(495, 67);
         this.cbConexionConfiada.Name = "cbConexionConfiada";
         this.cbConexionConfiada.Size = new System.Drawing.Size(126, 19);
         this.cbConexionConfiada.TabIndex = 6;
         this.cbConexionConfiada.Text = "Conexión confiada";
         this.cbConexionConfiada.UseVisualStyleBackColor = true;
         // 
         // cbSeguridadIntegrada
         // 
         this.cbSeguridadIntegrada.AutoSize = true;
         this.cbSeguridadIntegrada.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
         this.cbSeguridadIntegrada.ForeColor = System.Drawing.Color.Black;
         this.cbSeguridadIntegrada.Location = new System.Drawing.Point(271, 67);
         this.cbSeguridadIntegrada.Name = "cbSeguridadIntegrada";
         this.cbSeguridadIntegrada.Size = new System.Drawing.Size(132, 19);
         this.cbSeguridadIntegrada.TabIndex = 5;
         this.cbSeguridadIntegrada.Text = "Seguridad integrada";
         this.cbSeguridadIntegrada.UseVisualStyleBackColor = true;
         // 
         // cbAzure
         // 
         this.cbAzure.AutoSize = true;
         this.cbAzure.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
         this.cbAzure.ForeColor = System.Drawing.Color.Black;
         this.cbAzure.Location = new System.Drawing.Point(12, 67);
         this.cbAzure.Name = "cbAzure";
         this.cbAzure.Size = new System.Drawing.Size(56, 19);
         this.cbAzure.TabIndex = 4;
         this.cbAzure.Text = "Azure";
         this.cbAzure.UseVisualStyleBackColor = true;
         // 
         // textBoxCommandTimeOut
         // 
         this.textBoxCommandTimeOut.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
         this.textBoxCommandTimeOut.ForeColor = System.Drawing.Color.Black;
         this.textBoxCommandTimeOut.Location = new System.Drawing.Point(521, 31);
         this.textBoxCommandTimeOut.MaxLength = 10;
         this.textBoxCommandTimeOut.Name = "textBoxCommandTimeOut";
         this.textBoxCommandTimeOut.Size = new System.Drawing.Size(100, 23);
         this.textBoxCommandTimeOut.TabIndex = 3;
         this.textBoxCommandTimeOut.TextChanged += new System.EventHandler(this.verificarEntradaTexto);
         // 
         // labelCommandTimeOut
         // 
         this.labelCommandTimeOut.AutoSize = true;
         this.labelCommandTimeOut.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
         this.labelCommandTimeOut.ForeColor = System.Drawing.Color.Black;
         this.labelCommandTimeOut.Location = new System.Drawing.Point(404, 31);
         this.labelCommandTimeOut.Name = "labelCommandTimeOut";
         this.labelCommandTimeOut.Size = new System.Drawing.Size(111, 15);
         this.labelCommandTimeOut.TabIndex = 2;
         this.labelCommandTimeOut.Text = "Command Timeout";
         // 
         // textBoxTimeOut
         // 
         this.textBoxTimeOut.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
         this.textBoxTimeOut.ForeColor = System.Drawing.Color.Black;
         this.textBoxTimeOut.Location = new System.Drawing.Point(90, 28);
         this.textBoxTimeOut.MaxLength = 10;
         this.textBoxTimeOut.Name = "textBoxTimeOut";
         this.textBoxTimeOut.Size = new System.Drawing.Size(100, 23);
         this.textBoxTimeOut.TabIndex = 1;
         this.textBoxTimeOut.TextChanged += new System.EventHandler(this.verificarEntradaTexto);
         // 
         // labelTimeOut
         // 
         this.labelTimeOut.AutoSize = true;
         this.labelTimeOut.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
         this.labelTimeOut.ForeColor = System.Drawing.Color.Black;
         this.labelTimeOut.Location = new System.Drawing.Point(12, 31);
         this.labelTimeOut.Name = "labelTimeOut";
         this.labelTimeOut.Size = new System.Drawing.Size(51, 15);
         this.labelTimeOut.TabIndex = 0;
         this.labelTimeOut.Text = "Timeout";
         // 
         // buttonAceptar
         // 
         this.buttonAceptar.Location = new System.Drawing.Point(35, 351);
         this.buttonAceptar.Name = "buttonAceptar";
         this.buttonAceptar.Size = new System.Drawing.Size(75, 23);
         this.buttonAceptar.TabIndex = 5;
         this.buttonAceptar.Text = "Aceptar";
         this.buttonAceptar.UseVisualStyleBackColor = true;
         this.buttonAceptar.Click += new System.EventHandler(this.buttonAceptar_Click);
         // 
         // buttonCancelar
         // 
         this.buttonCancelar.Location = new System.Drawing.Point(597, 351);
         this.buttonCancelar.Name = "buttonCancelar";
         this.buttonCancelar.Size = new System.Drawing.Size(75, 23);
         this.buttonCancelar.TabIndex = 6;
         this.buttonCancelar.Text = "Cancelar";
         this.buttonCancelar.UseVisualStyleBackColor = true;
         this.buttonCancelar.Click += new System.EventHandler(this.buttonCancelar_Click);
         // 
         // cbAutenticacionSQLServer
         // 
         this.cbAutenticacionSQLServer.AutoSize = true;
         this.cbAutenticacionSQLServer.Location = new System.Drawing.Point(35, 70);
         this.cbAutenticacionSQLServer.Name = "cbAutenticacionSQLServer";
         this.cbAutenticacionSQLServer.Size = new System.Drawing.Size(159, 19);
         this.cbAutenticacionSQLServer.TabIndex = 2;
         this.cbAutenticacionSQLServer.Text = "Autenticación SQL Server";
         this.cbAutenticacionSQLServer.UseVisualStyleBackColor = true;
         this.cbAutenticacionSQLServer.CheckedChanged += new System.EventHandler(this.cbAutenticacionSQLServer_CheckedChanged);
         // 
         // groupBoxUsuarioSQLServer
         // 
         this.groupBoxUsuarioSQLServer.Controls.Add(this.buttonVerOcultarContrasenna);
         this.groupBoxUsuarioSQLServer.Controls.Add(this.textBoxContrasenna);
         this.groupBoxUsuarioSQLServer.Controls.Add(this.labelContrasenna);
         this.groupBoxUsuarioSQLServer.Controls.Add(this.textBoxUsuario);
         this.groupBoxUsuarioSQLServer.Controls.Add(this.labelUsuario);
         this.groupBoxUsuarioSQLServer.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
         this.groupBoxUsuarioSQLServer.ForeColor = System.Drawing.Color.Red;
         this.groupBoxUsuarioSQLServer.Location = new System.Drawing.Point(35, 92);
         this.groupBoxUsuarioSQLServer.Name = "groupBoxUsuarioSQLServer";
         this.groupBoxUsuarioSQLServer.Size = new System.Drawing.Size(637, 100);
         this.groupBoxUsuarioSQLServer.TabIndex = 3;
         this.groupBoxUsuarioSQLServer.TabStop = false;
         this.groupBoxUsuarioSQLServer.Text = "Login usuario SQL Server";
         // 
         // buttonVerOcultarContrasenna
         // 
         this.buttonVerOcultarContrasenna.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
         this.buttonVerOcultarContrasenna.ForeColor = System.Drawing.Color.Black;
         this.buttonVerOcultarContrasenna.Location = new System.Drawing.Point(360, 52);
         this.buttonVerOcultarContrasenna.Name = "buttonVerOcultarContrasenna";
         this.buttonVerOcultarContrasenna.Size = new System.Drawing.Size(150, 23);
         this.buttonVerOcultarContrasenna.TabIndex = 4;
         this.buttonVerOcultarContrasenna.Text = "Ver/Ocultar contraseña";
         this.buttonVerOcultarContrasenna.UseVisualStyleBackColor = true;
         this.buttonVerOcultarContrasenna.Click += new System.EventHandler(this.buttonVerOcultarContrasenna_Click);
         // 
         // textBoxContrasenna
         // 
         this.textBoxContrasenna.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
         this.textBoxContrasenna.ForeColor = System.Drawing.Color.Black;
         this.textBoxContrasenna.Location = new System.Drawing.Point(90, 52);
         this.textBoxContrasenna.Name = "textBoxContrasenna";
         this.textBoxContrasenna.Size = new System.Drawing.Size(251, 23);
         this.textBoxContrasenna.TabIndex = 3;
         this.textBoxContrasenna.UseSystemPasswordChar = true;
         this.textBoxContrasenna.TextChanged += new System.EventHandler(this.verificarEntradaTexto);
         // 
         // labelContrasenna
         // 
         this.labelContrasenna.AutoSize = true;
         this.labelContrasenna.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
         this.labelContrasenna.ForeColor = System.Drawing.Color.Black;
         this.labelContrasenna.Location = new System.Drawing.Point(12, 56);
         this.labelContrasenna.Name = "labelContrasenna";
         this.labelContrasenna.Size = new System.Drawing.Size(67, 15);
         this.labelContrasenna.TabIndex = 2;
         this.labelContrasenna.Text = "Contraseña";
         // 
         // textBoxUsuario
         // 
         this.textBoxUsuario.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
         this.textBoxUsuario.ForeColor = System.Drawing.Color.Black;
         this.textBoxUsuario.Location = new System.Drawing.Point(90, 23);
         this.textBoxUsuario.Name = "textBoxUsuario";
         this.textBoxUsuario.Size = new System.Drawing.Size(420, 23);
         this.textBoxUsuario.TabIndex = 1;
         this.textBoxUsuario.TextChanged += new System.EventHandler(this.verificarEntradaTexto);
         // 
         // labelUsuario
         // 
         this.labelUsuario.AutoSize = true;
         this.labelUsuario.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
         this.labelUsuario.ForeColor = System.Drawing.Color.Black;
         this.labelUsuario.Location = new System.Drawing.Point(12, 31);
         this.labelUsuario.Name = "labelUsuario";
         this.labelUsuario.Size = new System.Drawing.Size(47, 15);
         this.labelUsuario.TabIndex = 0;
         this.labelUsuario.Text = "Usuario";
         // 
         // FormConfiguracionAplicacion
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(707, 408);
         this.Controls.Add(this.groupBoxUsuarioSQLServer);
         this.Controls.Add(this.cbAutenticacionSQLServer);
         this.Controls.Add(this.buttonCancelar);
         this.Controls.Add(this.buttonAceptar);
         this.Controls.Add(this.groupBoxOpcionesAvanzadas);
         this.Controls.Add(this.textBoxServidor);
         this.Controls.Add(this.labelServidor);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "FormConfiguracionAplicacion";
         this.ShowIcon = false;
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "Configuración de la aplicación";
         this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormConfiguracionAplicacion_FormClosed);
         this.Load += new System.EventHandler(this.FormConfiguracionAplicacion_Load);
         this.Shown += new System.EventHandler(this.FormConfiguracionAplicacion_Shown);
         this.groupBoxOpcionesAvanzadas.ResumeLayout(false);
         this.groupBoxOpcionesAvanzadas.PerformLayout();
         this.groupBoxUsuarioSQLServer.ResumeLayout(false);
         this.groupBoxUsuarioSQLServer.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private Label labelServidor;
      private TextBox textBoxServidor;
      private GroupBox groupBoxOpcionesAvanzadas;
      private Button buttonValoresPorDefecto;
      private CheckBox cbConexionConfiada;
      private CheckBox cbSeguridadIntegrada;
      private CheckBox cbAzure;
      private TextBox textBoxCommandTimeOut;
      private Label labelCommandTimeOut;
      private TextBox textBoxTimeOut;
      private Label labelTimeOut;
      private Button buttonAceptar;
      private Button buttonCancelar;
      private CheckBox cbAutenticacionSQLServer;
      private GroupBox groupBoxUsuarioSQLServer;
      private Button buttonVerOcultarContrasenna;
      private TextBox textBoxContrasenna;
      private Label labelContrasenna;
      private TextBox textBoxUsuario;
      private Label labelUsuario;
   }
}