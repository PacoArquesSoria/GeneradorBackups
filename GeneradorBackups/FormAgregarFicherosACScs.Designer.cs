namespace GeneradorBackups
{
   partial class FormAgregarFicherosACScs
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
         labelDirectorio = new Label();
         textBoxDirectorio = new TextBox();
         buttonExaminar = new Button();
         clbFicherosYDirectorios = new CheckedListBox();
         buttonSeleccionarTodoAnularSeleccion = new Button();
         labelFicherosYDirectoriosSeleccionados = new Label();
         textBoxFicherosDirectoriosSeleccionados = new TextBox();
         buttonAceptar = new Button();
         buttonCancelar = new Button();
         buttonAplicar = new Button();
         folderBrowserDialogSelectorDirectorios = new FolderBrowserDialog();
         SuspendLayout();
         // 
         // labelDirectorio
         // 
         labelDirectorio.AutoSize = true;
         labelDirectorio.Location = new Point(35, 35);
         labelDirectorio.Name = "labelDirectorio";
         labelDirectorio.Size = new Size(59, 15);
         labelDirectorio.TabIndex = 0;
         labelDirectorio.Text = "Directorio";
         // 
         // textBoxDirectorio
         // 
         textBoxDirectorio.Location = new Point(111, 32);
         textBoxDirectorio.Name = "textBoxDirectorio";
         textBoxDirectorio.Size = new Size(637, 23);
         textBoxDirectorio.TabIndex = 1;
         textBoxDirectorio.Validated += textBoxDirectorio_Validated;
         // 
         // buttonExaminar
         // 
         buttonExaminar.Location = new Point(765, 31);
         buttonExaminar.Name = "buttonExaminar";
         buttonExaminar.Size = new Size(75, 23);
         buttonExaminar.TabIndex = 2;
         buttonExaminar.Text = "Examinar";
         buttonExaminar.UseVisualStyleBackColor = true;
         buttonExaminar.Click += buttonExaminar_Click;
         // 
         // clbFicherosYDirectorios
         // 
         clbFicherosYDirectorios.CheckOnClick = true;
         clbFicherosYDirectorios.FormattingEnabled = true;
         clbFicherosYDirectorios.Location = new Point(35, 61);
         clbFicherosYDirectorios.Name = "clbFicherosYDirectorios";
         clbFicherosYDirectorios.RightToLeft = RightToLeft.No;
         clbFicherosYDirectorios.ScrollAlwaysVisible = true;
         clbFicherosYDirectorios.Size = new Size(805, 202);
         clbFicherosYDirectorios.TabIndex = 3;
         clbFicherosYDirectorios.SelectedValueChanged += clbFicherosYDirectorios_SelectedValueChanged;
         // 
         // buttonSeleccionarTodoAnularSeleccion
         // 
         buttonSeleccionarTodoAnularSeleccion.Location = new Point(35, 269);
         buttonSeleccionarTodoAnularSeleccion.Name = "buttonSeleccionarTodoAnularSeleccion";
         buttonSeleccionarTodoAnularSeleccion.Size = new Size(213, 23);
         buttonSeleccionarTodoAnularSeleccion.TabIndex = 4;
         buttonSeleccionarTodoAnularSeleccion.Text = "Seleccionar todo/Anular selección";
         buttonSeleccionarTodoAnularSeleccion.UseVisualStyleBackColor = true;
         buttonSeleccionarTodoAnularSeleccion.Click += buttonSeleccionarTodoAnularSeleccion_Click;
         // 
         // labelFicherosYDirectoriosSeleccionados
         // 
         labelFicherosYDirectoriosSeleccionados.AutoSize = true;
         labelFicherosYDirectoriosSeleccionados.Location = new Point(35, 304);
         labelFicherosYDirectoriosSeleccionados.Name = "labelFicherosYDirectoriosSeleccionados";
         labelFicherosYDirectoriosSeleccionados.Size = new Size(196, 15);
         labelFicherosYDirectoriosSeleccionados.TabIndex = 6;
         labelFicherosYDirectoriosSeleccionados.Text = "Ficheros y directorios seleccionados";
         // 
         // textBoxFicherosDirectoriosSeleccionados
         // 
         textBoxFicherosDirectoriosSeleccionados.AcceptsReturn = true;
         textBoxFicherosDirectoriosSeleccionados.AcceptsTab = true;
         textBoxFicherosDirectoriosSeleccionados.BackColor = Color.Aquamarine;
         textBoxFicherosDirectoriosSeleccionados.Location = new Point(37, 322);
         textBoxFicherosDirectoriosSeleccionados.MaxLength = 0;
         textBoxFicherosDirectoriosSeleccionados.Multiline = true;
         textBoxFicherosDirectoriosSeleccionados.Name = "textBoxFicherosDirectoriosSeleccionados";
         textBoxFicherosDirectoriosSeleccionados.ReadOnly = true;
         textBoxFicherosDirectoriosSeleccionados.ScrollBars = ScrollBars.Both;
         textBoxFicherosDirectoriosSeleccionados.Size = new Size(803, 225);
         textBoxFicherosDirectoriosSeleccionados.TabIndex = 7;
         textBoxFicherosDirectoriosSeleccionados.WordWrap = false;
         // 
         // buttonAceptar
         // 
         buttonAceptar.Location = new Point(39, 564);
         buttonAceptar.Name = "buttonAceptar";
         buttonAceptar.Size = new Size(75, 23);
         buttonAceptar.TabIndex = 8;
         buttonAceptar.Text = "Aceptar";
         buttonAceptar.UseVisualStyleBackColor = true;
         buttonAceptar.Click += buttonAceptar_Click;
         // 
         // buttonCancelar
         // 
         buttonCancelar.Location = new Point(765, 564);
         buttonCancelar.Name = "buttonCancelar";
         buttonCancelar.Size = new Size(75, 23);
         buttonCancelar.TabIndex = 9;
         buttonCancelar.Text = "Cancelar";
         buttonCancelar.UseVisualStyleBackColor = true;
         buttonCancelar.Click += buttonCancelar_Click;
         // 
         // buttonAplicar
         // 
         buttonAplicar.Location = new Point(254, 269);
         buttonAplicar.Name = "buttonAplicar";
         buttonAplicar.Size = new Size(75, 23);
         buttonAplicar.TabIndex = 5;
         buttonAplicar.Text = "Aplicar";
         buttonAplicar.UseVisualStyleBackColor = true;
         buttonAplicar.Click += buttonAplicar_Click;
         // 
         // FormAgregarFicherosACScs
         // 
         AutoScaleDimensions = new SizeF(7F, 15F);
         AutoScaleMode = AutoScaleMode.Font;
         ClientSize = new Size(874, 622);
         Controls.Add(buttonAplicar);
         Controls.Add(buttonCancelar);
         Controls.Add(buttonAceptar);
         Controls.Add(textBoxFicherosDirectoriosSeleccionados);
         Controls.Add(labelFicherosYDirectoriosSeleccionados);
         Controls.Add(buttonSeleccionarTodoAnularSeleccion);
         Controls.Add(clbFicherosYDirectorios);
         Controls.Add(buttonExaminar);
         Controls.Add(textBoxDirectorio);
         Controls.Add(labelDirectorio);
         FormBorderStyle = FormBorderStyle.FixedSingle;
         MaximizeBox = false;
         MinimizeBox = false;
         Name = "FormAgregarFicherosACScs";
         ShowIcon = false;
         ShowInTaskbar = false;
         StartPosition = FormStartPosition.CenterScreen;
         Text = "Agregar ficheros y directorios a la copia de seguridad";
         FormClosed += FormAgregarFicherosACScs_FormClosed;
         Load += FormAgregarFicherosACScs_Load;
         Shown += FormAgregarFicherosACScs_Shown;
         ResumeLayout(false);
         PerformLayout();
      }

      #endregion

      private Label labelDirectorio;
      private TextBox textBoxDirectorio;
      private Button buttonExaminar;
      private CheckedListBox clbFicherosYDirectorios;
      private Button buttonSeleccionarTodoAnularSeleccion;
      private Label labelFicherosYDirectoriosSeleccionados;
      private TextBox textBoxFicherosDirectoriosSeleccionados;
      private Button buttonAceptar;
      private Button buttonCancelar;
      private Button buttonAplicar;
      private FolderBrowserDialog folderBrowserDialogSelectorDirectorios;
   }
}