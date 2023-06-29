namespace GeneradorBackups
{
   partial class FormConfiguracionProgramaBackup
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
         labelNombreEjecutable = new Label();
         textBoxNombreEjecutable = new TextBox();
         buttonExaminar = new Button();
         labelOpciones = new Label();
         textBoxOpciones = new TextBox();
         labelOpcionListaFicheros = new Label();
         textBoxOpcionListaFicheros = new TextBox();
         buttonAceptar = new Button();
         buttonCancelar = new Button();
         openFileDialogSelectorEjecutables = new OpenFileDialog();
         buttonValoresPorDefecto = new Button();
         labelExtensiónPorDefecto = new Label();
         textBoxExtensiónPorDefecto = new TextBox();
         SuspendLayout();
         // 
         // labelNombreEjecutable
         // 
         labelNombreEjecutable.AutoSize = true;
         labelNombreEjecutable.Location = new Point(35, 35);
         labelNombreEjecutable.Name = "labelNombreEjecutable";
         labelNombreEjecutable.Size = new Size(127, 15);
         labelNombreEjecutable.TabIndex = 0;
         labelNombreEjecutable.Text = "Nombre del ejecutable";
         // 
         // textBoxNombreEjecutable
         // 
         textBoxNombreEjecutable.Location = new Point(168, 32);
         textBoxNombreEjecutable.Name = "textBoxNombreEjecutable";
         textBoxNombreEjecutable.Size = new Size(438, 23);
         textBoxNombreEjecutable.TabIndex = 1;
         textBoxNombreEjecutable.TextChanged += cambiarTexto;
         // 
         // buttonExaminar
         // 
         buttonExaminar.Location = new Point(621, 31);
         buttonExaminar.Name = "buttonExaminar";
         buttonExaminar.Size = new Size(75, 23);
         buttonExaminar.TabIndex = 2;
         buttonExaminar.Text = "Examinar";
         buttonExaminar.UseVisualStyleBackColor = true;
         buttonExaminar.Click += buttonExaminar_Click;
         // 
         // labelOpciones
         // 
         labelOpciones.AutoSize = true;
         labelOpciones.Location = new Point(35, 65);
         labelOpciones.Name = "labelOpciones";
         labelOpciones.Size = new Size(57, 15);
         labelOpciones.TabIndex = 3;
         labelOpciones.Text = "Opciones";
         // 
         // textBoxOpciones
         // 
         textBoxOpciones.Location = new Point(168, 62);
         textBoxOpciones.Name = "textBoxOpciones";
         textBoxOpciones.Size = new Size(528, 23);
         textBoxOpciones.TabIndex = 4;
         textBoxOpciones.TextChanged += cambiarTexto;
         // 
         // labelOpcionListaFicheros
         // 
         labelOpcionListaFicheros.AutoSize = true;
         labelOpcionListaFicheros.Location = new Point(35, 95);
         labelOpcionListaFicheros.Name = "labelOpcionListaFicheros";
         labelOpcionListaFicheros.Size = new Size(115, 15);
         labelOpcionListaFicheros.TabIndex = 5;
         labelOpcionListaFicheros.Text = "Opción lista ficheros";
         // 
         // textBoxOpcionListaFicheros
         // 
         textBoxOpcionListaFicheros.Location = new Point(168, 91);
         textBoxOpcionListaFicheros.Name = "textBoxOpcionListaFicheros";
         textBoxOpcionListaFicheros.Size = new Size(100, 23);
         textBoxOpcionListaFicheros.TabIndex = 6;
         textBoxOpcionListaFicheros.TextChanged += cambiarTexto;
         // 
         // buttonAceptar
         // 
         buttonAceptar.Location = new Point(531, 157);
         buttonAceptar.Name = "buttonAceptar";
         buttonAceptar.Size = new Size(75, 23);
         buttonAceptar.TabIndex = 10;
         buttonAceptar.Text = "Aceptar";
         buttonAceptar.UseVisualStyleBackColor = true;
         buttonAceptar.Click += buttonAceptar_Click;
         // 
         // buttonCancelar
         // 
         buttonCancelar.Location = new Point(621, 157);
         buttonCancelar.Name = "buttonCancelar";
         buttonCancelar.Size = new Size(75, 23);
         buttonCancelar.TabIndex = 11;
         buttonCancelar.Text = "Cancelar";
         buttonCancelar.UseVisualStyleBackColor = true;
         buttonCancelar.Click += buttonCancelar_Click;
         // 
         // openFileDialogSelectorEjecutables
         // 
         openFileDialogSelectorEjecutables.FileName = "openFileDialog1";
         // 
         // buttonValoresPorDefecto
         // 
         buttonValoresPorDefecto.Location = new Point(35, 157);
         buttonValoresPorDefecto.Name = "buttonValoresPorDefecto";
         buttonValoresPorDefecto.Size = new Size(142, 23);
         buttonValoresPorDefecto.TabIndex = 9;
         buttonValoresPorDefecto.Text = "Valores por defecto";
         buttonValoresPorDefecto.UseVisualStyleBackColor = true;
         buttonValoresPorDefecto.Click += buttonValoresPorDefecto_Click;
         // 
         // labelExtensiónPorDefecto
         // 
         labelExtensiónPorDefecto.AutoSize = true;
         labelExtensiónPorDefecto.Location = new Point(35, 125);
         labelExtensiónPorDefecto.Name = "labelExtensiónPorDefecto";
         labelExtensiónPorDefecto.Size = new Size(122, 15);
         labelExtensiónPorDefecto.TabIndex = 7;
         labelExtensiónPorDefecto.Text = "Extensión por defecto";
         // 
         // textBoxExtensiónPorDefecto
         // 
         textBoxExtensiónPorDefecto.Location = new Point(168, 122);
         textBoxExtensiónPorDefecto.Name = "textBoxExtensiónPorDefecto";
         textBoxExtensiónPorDefecto.Size = new Size(528, 23);
         textBoxExtensiónPorDefecto.TabIndex = 8;
         // 
         // FormConfiguracionProgramaBackup
         // 
         AutoScaleDimensions = new SizeF(7F, 15F);
         AutoScaleMode = AutoScaleMode.Font;
         ClientSize = new Size(740, 226);
         Controls.Add(textBoxExtensiónPorDefecto);
         Controls.Add(labelExtensiónPorDefecto);
         Controls.Add(buttonValoresPorDefecto);
         Controls.Add(buttonCancelar);
         Controls.Add(buttonAceptar);
         Controls.Add(textBoxOpcionListaFicheros);
         Controls.Add(labelOpcionListaFicheros);
         Controls.Add(textBoxOpciones);
         Controls.Add(labelOpciones);
         Controls.Add(buttonExaminar);
         Controls.Add(textBoxNombreEjecutable);
         Controls.Add(labelNombreEjecutable);
         FormBorderStyle = FormBorderStyle.FixedSingle;
         MaximizeBox = false;
         MinimizeBox = false;
         Name = "FormConfiguracionProgramaBackup";
         ShowIcon = false;
         ShowInTaskbar = false;
         StartPosition = FormStartPosition.CenterScreen;
         Text = "Configuración del programa de Backup";
         FormClosed += FormConfiguracionProgramaBackup_FormClosed;
         Load += FormConfiguracionProgramaBackup_Load;
         Shown += FormConfiguracionProgramaBackup_Shown;
         ResumeLayout(false);
         PerformLayout();
      }

      #endregion

      private Label labelNombreEjecutable;
      private TextBox textBoxNombreEjecutable;
      private Button buttonExaminar;
      private Label labelOpciones;
      private TextBox textBoxOpciones;
      private Label labelOpcionListaFicheros;
      private TextBox textBoxOpcionListaFicheros;
      private Button buttonAceptar;
      private Button buttonCancelar;
      private OpenFileDialog openFileDialogSelectorEjecutables;
      private Button buttonValoresPorDefecto;
      private Label labelExtensiónPorDefecto;
      private TextBox textBoxExtensiónPorDefecto;
   }
}