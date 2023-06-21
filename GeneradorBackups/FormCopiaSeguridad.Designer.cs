namespace GeneradorBackups
{
   partial class FormCopiaSeguridad
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCopiaSeguridad));
         this.dgvDatosCS = new System.Windows.Forms.DataGridView();
         this.ColumnaIdentificador = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.ColumnaNombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.ColumnaDescripcion = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.ColumnaSeleccionada = new System.Windows.Forms.DataGridViewCheckBoxColumn();
         this.groupBoxFiltro = new System.Windows.Forms.GroupBox();
         this.buttonAplicar = new System.Windows.Forms.Button();
         this.textBoxValor = new System.Windows.Forms.TextBox();
         this.labelValor = new System.Windows.Forms.Label();
         this.cbSelectorCampo = new System.Windows.Forms.ComboBox();
         this.labelCampo = new System.Windows.Forms.Label();
         this.buttonSeleccionarDeseleccionarFilas = new System.Windows.Forms.Button();
         this.buttonEliminarCopiasSeguridad = new System.Windows.Forms.Button();
         this.buttonActivarDesactivarSeleccionada = new System.Windows.Forms.Button();
         this.buttonEditarCS = new System.Windows.Forms.Button();
         this.buttonAgregarCS = new System.Windows.Forms.Button();
         this.buttonEjecutar = new System.Windows.Forms.Button();
         this.buttonCerrar = new System.Windows.Forms.Button();
         ((System.ComponentModel.ISupportInitialize)(this.dgvDatosCS)).BeginInit();
         this.groupBoxFiltro.SuspendLayout();
         this.SuspendLayout();
         // 
         // dgvDatosCS
         // 
         this.dgvDatosCS.AllowUserToAddRows = false;
         this.dgvDatosCS.AllowUserToDeleteRows = false;
         this.dgvDatosCS.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         this.dgvDatosCS.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnaIdentificador,
            this.ColumnaNombre,
            this.ColumnaDescripcion,
            this.ColumnaSeleccionada});
         this.dgvDatosCS.Location = new System.Drawing.Point(35, 35);
         this.dgvDatosCS.Name = "dgvDatosCS";
         this.dgvDatosCS.RowTemplate.Height = 25;
         this.dgvDatosCS.Size = new System.Drawing.Size(904, 263);
         this.dgvDatosCS.TabIndex = 0;
         this.dgvDatosCS.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDatosCS_CellValueChanged);
         this.dgvDatosCS.SelectionChanged += new System.EventHandler(this.dgvDatosCS_SelectionChanged);
         // 
         // ColumnaIdentificador
         // 
         this.ColumnaIdentificador.Frozen = true;
         this.ColumnaIdentificador.HeaderText = "Identificador";
         this.ColumnaIdentificador.MinimumWidth = 200;
         this.ColumnaIdentificador.Name = "ColumnaIdentificador";
         this.ColumnaIdentificador.ReadOnly = true;
         this.ColumnaIdentificador.Width = 200;
         // 
         // ColumnaNombre
         // 
         this.ColumnaNombre.HeaderText = "Nombre";
         this.ColumnaNombre.MinimumWidth = 225;
         this.ColumnaNombre.Name = "ColumnaNombre";
         this.ColumnaNombre.ReadOnly = true;
         this.ColumnaNombre.Width = 225;
         // 
         // ColumnaDescripcion
         // 
         this.ColumnaDescripcion.HeaderText = "Descripción";
         this.ColumnaDescripcion.MinimumWidth = 350;
         this.ColumnaDescripcion.Name = "ColumnaDescripcion";
         this.ColumnaDescripcion.ReadOnly = true;
         this.ColumnaDescripcion.Width = 350;
         // 
         // ColumnaSeleccionada
         // 
         this.ColumnaSeleccionada.HeaderText = "Seleccionada";
         this.ColumnaSeleccionada.MinimumWidth = 85;
         this.ColumnaSeleccionada.Name = "ColumnaSeleccionada";
         this.ColumnaSeleccionada.Width = 85;
         // 
         // groupBoxFiltro
         // 
         this.groupBoxFiltro.Controls.Add(this.buttonAplicar);
         this.groupBoxFiltro.Controls.Add(this.textBoxValor);
         this.groupBoxFiltro.Controls.Add(this.labelValor);
         this.groupBoxFiltro.Controls.Add(this.cbSelectorCampo);
         this.groupBoxFiltro.Controls.Add(this.labelCampo);
         this.groupBoxFiltro.Location = new System.Drawing.Point(35, 355);
         this.groupBoxFiltro.Name = "groupBoxFiltro";
         this.groupBoxFiltro.Size = new System.Drawing.Size(904, 84);
         this.groupBoxFiltro.TabIndex = 6;
         this.groupBoxFiltro.TabStop = false;
         this.groupBoxFiltro.Text = "Filtro";
         // 
         // buttonAplicar
         // 
         this.buttonAplicar.Location = new System.Drawing.Point(823, 30);
         this.buttonAplicar.Name = "buttonAplicar";
         this.buttonAplicar.Size = new System.Drawing.Size(75, 23);
         this.buttonAplicar.TabIndex = 4;
         this.buttonAplicar.Text = "Aplicar";
         this.buttonAplicar.UseVisualStyleBackColor = true;
         this.buttonAplicar.Click += new System.EventHandler(this.buttonAplicar_Click);
         // 
         // textBoxValor
         // 
         this.textBoxValor.Location = new System.Drawing.Point(386, 30);
         this.textBoxValor.Name = "textBoxValor";
         this.textBoxValor.Size = new System.Drawing.Size(356, 23);
         this.textBoxValor.TabIndex = 3;
         // 
         // labelValor
         // 
         this.labelValor.AutoSize = true;
         this.labelValor.Location = new System.Drawing.Point(341, 30);
         this.labelValor.Name = "labelValor";
         this.labelValor.Size = new System.Drawing.Size(33, 15);
         this.labelValor.TabIndex = 2;
         this.labelValor.Text = "Valor";
         // 
         // cbSelectorCampo
         // 
         this.cbSelectorCampo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cbSelectorCampo.FormattingEnabled = true;
         this.cbSelectorCampo.Location = new System.Drawing.Point(77, 29);
         this.cbSelectorCampo.Name = "cbSelectorCampo";
         this.cbSelectorCampo.Size = new System.Drawing.Size(231, 23);
         this.cbSelectorCampo.TabIndex = 1;
         this.cbSelectorCampo.SelectedIndexChanged += new System.EventHandler(this.cbSelectorCampo_SelectedIndexChanged);
         // 
         // labelCampo
         // 
         this.labelCampo.AutoSize = true;
         this.labelCampo.Location = new System.Drawing.Point(23, 30);
         this.labelCampo.Name = "labelCampo";
         this.labelCampo.Size = new System.Drawing.Size(46, 15);
         this.labelCampo.TabIndex = 0;
         this.labelCampo.Text = "Campo";
         // 
         // buttonSeleccionarDeseleccionarFilas
         // 
         this.buttonSeleccionarDeseleccionarFilas.Location = new System.Drawing.Point(35, 303);
         this.buttonSeleccionarDeseleccionarFilas.Name = "buttonSeleccionarDeseleccionarFilas";
         this.buttonSeleccionarDeseleccionarFilas.Size = new System.Drawing.Size(189, 23);
         this.buttonSeleccionarDeseleccionarFilas.TabIndex = 1;
         this.buttonSeleccionarDeseleccionarFilas.Text = "Seleccionar/Deseleccionar filas";
         this.buttonSeleccionarDeseleccionarFilas.UseVisualStyleBackColor = true;
         this.buttonSeleccionarDeseleccionarFilas.Click += new System.EventHandler(this.buttonSeleccionarDeseleccionarFilas_Click);
         // 
         // buttonEliminarCopiasSeguridad
         // 
         this.buttonEliminarCopiasSeguridad.Location = new System.Drawing.Point(228, 303);
         this.buttonEliminarCopiasSeguridad.Name = "buttonEliminarCopiasSeguridad";
         this.buttonEliminarCopiasSeguridad.Size = new System.Drawing.Size(165, 23);
         this.buttonEliminarCopiasSeguridad.TabIndex = 2;
         this.buttonEliminarCopiasSeguridad.Text = "Eliminar copia de seguridad";
         this.buttonEliminarCopiasSeguridad.UseVisualStyleBackColor = true;
         this.buttonEliminarCopiasSeguridad.Click += new System.EventHandler(this.buttonEliminarCopiasSeguridad_Click);
         // 
         // buttonActivarDesactivarSeleccionada
         // 
         this.buttonActivarDesactivarSeleccionada.Location = new System.Drawing.Point(421, 304);
         this.buttonActivarDesactivarSeleccionada.Name = "buttonActivarDesactivarSeleccionada";
         this.buttonActivarDesactivarSeleccionada.Size = new System.Drawing.Size(199, 23);
         this.buttonActivarDesactivarSeleccionada.TabIndex = 3;
         this.buttonActivarDesactivarSeleccionada.Text = "Activar/Desactivar bit de selección";
         this.buttonActivarDesactivarSeleccionada.UseVisualStyleBackColor = true;
         this.buttonActivarDesactivarSeleccionada.Click += new System.EventHandler(this.buttonActivarDesactivarSeleccionada_Click);
         // 
         // buttonEditarCS
         // 
         this.buttonEditarCS.Location = new System.Drawing.Point(639, 304);
         this.buttonEditarCS.Name = "buttonEditarCS";
         this.buttonEditarCS.Size = new System.Drawing.Size(138, 23);
         this.buttonEditarCS.TabIndex = 4;
         this.buttonEditarCS.Text = "Editar copia seguridad";
         this.buttonEditarCS.UseVisualStyleBackColor = true;
         this.buttonEditarCS.Click += new System.EventHandler(this.buttonEditarCS_Click);
         // 
         // buttonAgregarCS
         // 
         this.buttonAgregarCS.Location = new System.Drawing.Point(783, 304);
         this.buttonAgregarCS.Name = "buttonAgregarCS";
         this.buttonAgregarCS.Size = new System.Drawing.Size(156, 23);
         this.buttonAgregarCS.TabIndex = 5;
         this.buttonAgregarCS.Text = "Nueva copia de seguridad";
         this.buttonAgregarCS.UseVisualStyleBackColor = true;
         this.buttonAgregarCS.Click += new System.EventHandler(this.buttonAgregarCS_Click);
         // 
         // buttonEjecutar
         // 
         this.buttonEjecutar.Location = new System.Drawing.Point(36, 453);
         this.buttonEjecutar.Name = "buttonEjecutar";
         this.buttonEjecutar.Size = new System.Drawing.Size(75, 23);
         this.buttonEjecutar.TabIndex = 7;
         this.buttonEjecutar.Text = "Ejecutar";
         this.buttonEjecutar.UseVisualStyleBackColor = true;
         this.buttonEjecutar.Click += new System.EventHandler(this.buttonEjecutar_Click);
         // 
         // buttonCerrar
         // 
         this.buttonCerrar.Location = new System.Drawing.Point(864, 453);
         this.buttonCerrar.Name = "buttonCerrar";
         this.buttonCerrar.Size = new System.Drawing.Size(75, 23);
         this.buttonCerrar.TabIndex = 8;
         this.buttonCerrar.Text = "Cerrar";
         this.buttonCerrar.UseVisualStyleBackColor = true;
         this.buttonCerrar.Click += new System.EventHandler(this.buttonCerrar_Click);
         // 
         // FormCopiaSeguridad
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(974, 510);
         this.Controls.Add(this.buttonCerrar);
         this.Controls.Add(this.buttonEjecutar);
         this.Controls.Add(this.buttonAgregarCS);
         this.Controls.Add(this.buttonEditarCS);
         this.Controls.Add(this.buttonActivarDesactivarSeleccionada);
         this.Controls.Add(this.buttonEliminarCopiasSeguridad);
         this.Controls.Add(this.buttonSeleccionarDeseleccionarFilas);
         this.Controls.Add(this.groupBoxFiltro);
         this.Controls.Add(this.dgvDatosCS);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.MaximizeBox = false;
         this.Name = "FormCopiaSeguridad";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "Copia de Seguridad";
         this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormCopiaSeguridad_FormClosed);
         this.Load += new System.EventHandler(this.FormCopiaSeguridad_Load);
         this.Shown += new System.EventHandler(this.FormCopiaSeguridad_Shown);
         ((System.ComponentModel.ISupportInitialize)(this.dgvDatosCS)).EndInit();
         this.groupBoxFiltro.ResumeLayout(false);
         this.groupBoxFiltro.PerformLayout();
         this.ResumeLayout(false);

      }

        #endregion

        private DataGridView dgvDatosCS;
        private GroupBox groupBoxFiltro;
        private Button buttonAplicar;
        private TextBox textBoxValor;
        private Label labelValor;
        private ComboBox cbSelectorCampo;
        private Label labelCampo;
        private Button buttonSeleccionarDeseleccionarFilas;
        private Button buttonEliminarCopiasSeguridad;
        private Button buttonActivarDesactivarSeleccionada;
        private Button buttonEditarCS;
        private Button buttonAgregarCS;
        private Button buttonEjecutar;
        private Button buttonCerrar;
        private DataGridViewTextBoxColumn ColumnaIdentificador;
        private DataGridViewTextBoxColumn ColumnaNombre;
        private DataGridViewTextBoxColumn ColumnaDescripcion;
        private DataGridViewCheckBoxColumn ColumnaSeleccionada;
    }
}