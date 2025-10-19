namespace projetoLoja.Presentation
{
    partial class frmClient
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
            this.btnCart = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCart
            // 
            this.btnCart.BackgroundImage = global::projetoLoja.Properties.Resources.carrinhoCompra;
            this.btnCart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnCart.Image = global::projetoLoja.Properties.Resources.carrinhoCompra;
            this.btnCart.Location = new System.Drawing.Point(809, 12);
            this.btnCart.Name = "btnCart";
            this.btnCart.Size = new System.Drawing.Size(41, 39);
            this.btnCart.TabIndex = 0;
            this.btnCart.UseVisualStyleBackColor = true;
            this.btnCart.Visible = false;
            // 
            // frmClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 450);
            this.Controls.Add(this.btnCart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmClient";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmCliente";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCart;
    }
}