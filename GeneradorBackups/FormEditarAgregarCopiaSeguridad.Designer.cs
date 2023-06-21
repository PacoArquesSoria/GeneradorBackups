namespace GeneradorBackups
{
   partial class FormEditarAgregarCopiaSeguridad
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
         labelIdentificador = new Label();
         textBoxIdentificador = new TextBox();
         labelNombre = new Label();
         textBoxNombre = new TextBox();
         labelDestino = new Label();
         textBoxDestino = new TextBox();
         buttonExaminar = new Button();
         labelDescripcion = new Label();
         textBoxDescripcion = new TextBox();
         buttonAceptar = new Button();
         buttonCancelar = new Button();
         buttonAgregarModificarFicheros = new Button();
         folderBrowserSelectorDestino = new FolderBrowserDialog();
         SuspendLayout();
         // 
         // labelIdentificador
         // 
         labelIdentificador.AutoSize = true;
         labelIdentificador.Location = new Point(35, 35);
         labelIdentificador.Name = "labelIdentificador";
         labelIdentificador.Size = new Size(74, 15);
         labelIdentificador.TabIndex = 0;
         labelIdentificador.Text = "Identificador";
         // 
         // textBoxIdentificador
         // 
         textBoxIdentificador.BackColor = Color.Aquamarine;
         textBoxIdentificador.Location = new Point(115, 32);
         textBoxIdentificador.Name = "textBoxIdentificador";
         textBoxIdentificador.ReadOnly = true;
         textBoxIdentificador.Size = new Size(287, 23);
         textBoxIdentificador.TabIndex = 1;
         // 
         // labelNombre
         // 
         labelNombre.AutoSize = true;
         labelNombre.Location = new Point(35, 65);
         labelNombre.Name = "labelNombre";
         labelNombre.Size = new Size(51, 15);
         labelNombre.TabIndex = 2;
         labelNombre.Text = "Nombre";
         // 
         // textBoxNombre
         // 
         textBoxNombre.Location = new Point(115, 62);
         textBoxNombre.Name = "textBoxNombre";
         textBoxNombre.Size = new Size(542, 23);
         textBoxNombre.TabIndex = 3;
         textBoxNombre.TextChanged += cambioTexto;
         // 
         // labelDestino
         // 
         labelDestino.AutoSize = true;
         labelDestino.Location = new Point(35, 95);
         labelDestino.Name = "labelDestino";
         labelDestino.Size = new Size(47, 15);
         labelDestino.TabIndex = 4;
         labelDestino.Text = "Destino";
         // 
         // textBoxDestino
         // 
         textBoxDestino.Location = new Point(115, 92);
         textBoxDestino.Name = "textBoxDestino";
         textBoxDestino.Size = new Size(413, 23);
         textBoxDestino.TabIndex = 5;
         textBoxDestino.Validated += textBoxDestino_Validated;
         // 
         // buttonExaminar
         // 
         buttonExaminar.Location = new Point(582, 95);
         buttonExaminar.Name = "buttonExaminar";
         buttonExaminar.Size = new Size(75, 23);
         buttonExaminar.TabIndex = 6;
         buttonExaminar.Text = "Examinar";
         buttonExaminar.UseVisualStyleBackColor = true;
         buttonExaminar.Click += buttonExaminar_Click;
         // 
         // labelDescripcion
         // 
         labelDescripcion.AutoSize = true;
         labelDescripcion.Location = new Point(35, 125);
         labelDescripcion.Name = "labelDescripcion";
         labelDescripcion.Size = new Size(69, 15);
         labelDescripcion.TabIndex = 7;
         labelDescripcion.Text = "Descripción";
         // 
         // textBoxDescripcion
         // 
         textBoxDescripcion.AcceptsReturn = true;
         textBoxDescripcion.AcceptsTab = true;
         textBoxDescripcion.Location = new Point(115, 122);
         textBoxDescripcion.Multiline = true;
         textBoxDescripcion.Name = "textBoxDescripcion";
         textBoxDescripcion.ScrollBars = ScrollBars.Both;
         textBoxDescripcion.Size = new Size(542, 219);
         textBoxDescripcion.TabIndex = 8;
         textBoxDescripcion.WordWrap = false;
         textBoxDescripcion.TextChanged += cambioTexto;
         // 
         // buttonAceptar
         // 
         buttonAceptar.Location = new Point(35, 347);
         buttonAceptar.Name = "buttonAceptar";
         buttonAceptar.Size = new Size(75, 23);
         buttonAceptar.TabIndex = 9;
         buttonAceptar.Text = "Aceptar";
         buttonAceptar.UseVisualStyleBackColor = true;
         buttonAceptar.Click += buttonAceptar_Click;
         // 
         // buttonCancelar
         // 
         buttonCancelar.Location = new Point(582, 347);
         buttonCancelar.Name = "buttonCancelar";
         buttonCancelar.Size = new Size(75, 23);
         buttonCancelar.TabIndex = 11;
         buttonCancelar.Text = "Cancelar";
         buttonCancelar.UseVisualStyleBackColor = true;
         buttonCancelar.Click += buttonCancelar_Click;
         // 
         // buttonAgregarModificarFicheros
         // 
         buttonAgregarModificarFicheros.Location = new Point(280, 347);
         buttonAgregarModificarFicheros.Name = "buttonAgregarModificarFicheros";
         buttonAgregarModificarFicheros.Size = new Size(169, 23);
         buttonAgregarModificarFicheros.TabIndex = 10;
         buttonAgregarModificarFicheros.Text = "Agregar /Modificar ficheros";
         buttonAgregarModificarFicheros.UseVisualStyleBackColor = true;
         buttonAgregarModificarFicheros.Click += buttonAgregarModificarFicheros_Click;
         // 
         // FormEditarAgregarCopiaSeguridad
         // 
         AutoScaleDimensions = new SizeF(7F, 15F);
         AutoScaleMode = AutoScaleMode.Font;
         ClientSize = new Size(695, 405);
         Controls.Add(buttonAgregarModificarFicheros);
         Controls.Add(buttonCancelar);
         Controls.Add(buttonAceptar);
         Controls.Add(textBoxDescripcion);
         Controls.Add(labelDescripcion);
         Controls.Add(buttonExaminar);
         Controls.Add(textBoxDestino);
         Controls.Add(labelDestino);
         Controls.Add(textBoxNombre);
         Controls.Add(labelNombre);
         Controls.Add(textBoxIdentificador);
         Controls.Add(labelIdentificador);
         FormBorderStyle = FormBorderStyle.FixedSingle;
         MaximizeBox = false;
         MinimizeBox = false;
         Name = "FormEditarAgregarCopiaSeguridad";
         ShowIcon = false;
         ShowInTaskbar = false;
         StartPosition = FormStartPosition.CenterScreen;
         Text = "Editar/Nueva copia de seguridad";
         FormClosed += FormEditarAgregarCopiaSeguridad_FormClosed;
         Load += FormEditarAgregarCopiaSeguridad_Load;
         Shown += FormEditarAgregarCopiaSeguridad_Shown;
         ResumeLayout(false);
         PerformLayout();
      }

      #endregion

      private Label labelIdentificador;
      private TextBox textBoxIdentificador;
      private Label labelNombre;
      private TextBox textBoxNombre;
      private Label labelDestino;
      private TextBox textBoxDestino;
      private Button buttonExaminar;
      private Label labelDescripcion;
      private TextBox textBoxDescripcion;
      private Button buttonAceptar;
      private Button buttonCancelar;
      private Button buttonAgregarModificarFicheros;
      private FolderBrowserDialog folderBrowserSelectorDestino;
   }
}