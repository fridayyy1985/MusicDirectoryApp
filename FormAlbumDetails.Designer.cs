namespace MusicDirectoryApp.Forms
{
    partial class FormAlbumDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAlbumDetails));
            this.panelMain = new System.Windows.Forms.Panel();
            this.groupBoxTracks = new System.Windows.Forms.GroupBox();
            this.panelTrackButtons = new System.Windows.Forms.Panel();
            this.btnAddTrack = new System.Windows.Forms.Button();
            this.btnEditTrack = new System.Windows.Forms.Button();
            this.btnDeleteTrack = new System.Windows.Forms.Button();
            this.dataGridViewTracks = new System.Windows.Forms.DataGridView();
            this.groupBoxAlbumInfo = new System.Windows.Forms.GroupBox();
            this.numTracks = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.numRating = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtLabel = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtGenre = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numYear = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.txtArtist = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxAlbumCover = new System.Windows.Forms.GroupBox();
            this.lblImagePath = new System.Windows.Forms.Label();
            this.pictureBoxAlbum = new System.Windows.Forms.PictureBox();
            this.btnSelectImage = new System.Windows.Forms.Button();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.panelMain.SuspendLayout();
            this.groupBoxTracks.SuspendLayout();
            this.panelTrackButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTracks)).BeginInit();
            this.groupBoxAlbumInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTracks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRating)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numYear)).BeginInit();
            this.groupBoxAlbumCover.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAlbum)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.groupBoxTracks);
            this.panelMain.Controls.Add(this.groupBoxAlbumInfo);
            this.panelMain.Controls.Add(this.groupBoxAlbumCover);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(10);
            this.panelMain.Size = new System.Drawing.Size(800, 550);
            this.panelMain.TabIndex = 0;
            // 
            // groupBoxTracks
            // 
            this.groupBoxTracks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxTracks.Controls.Add(this.panelTrackButtons);
            this.groupBoxTracks.Controls.Add(this.dataGridViewTracks);
            this.groupBoxTracks.Location = new System.Drawing.Point(330, 220);
            this.groupBoxTracks.Name = "groupBoxTracks";
            this.groupBoxTracks.Size = new System.Drawing.Size(460, 320);
            this.groupBoxTracks.TabIndex = 2;
            this.groupBoxTracks.TabStop = false;
            this.groupBoxTracks.Text = "Список треков";
            // 
            // panelTrackButtons
            // 
            this.panelTrackButtons.Controls.Add(this.btnAddTrack);
            this.panelTrackButtons.Controls.Add(this.btnEditTrack);
            this.panelTrackButtons.Controls.Add(this.btnDeleteTrack);
            this.panelTrackButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTrackButtons.Location = new System.Drawing.Point(3, 16);
            this.panelTrackButtons.Name = "panelTrackButtons";
            this.panelTrackButtons.Size = new System.Drawing.Size(454, 46);
            this.panelTrackButtons.TabIndex = 1;
            // 
            // btnAddTrack
            // 
            this.btnAddTrack.Location = new System.Drawing.Point(10, 8);
            this.btnAddTrack.Name = "btnAddTrack";
            this.btnAddTrack.Size = new System.Drawing.Size(100, 25);
            this.btnAddTrack.TabIndex = 0;
            this.btnAddTrack.Text = "Добавить трек";
            this.btnAddTrack.UseVisualStyleBackColor = true;
            this.btnAddTrack.Click += new System.EventHandler(this.btnAddTrack_Click);
            // 
            // btnEditTrack
            // 
            this.btnEditTrack.Location = new System.Drawing.Point(120, 8);
            this.btnEditTrack.Name = "btnEditTrack";
            this.btnEditTrack.Size = new System.Drawing.Size(100, 25);
            this.btnEditTrack.TabIndex = 1;
            this.btnEditTrack.Text = "Редактировать";
            this.btnEditTrack.UseVisualStyleBackColor = true;
            this.btnEditTrack.Click += new System.EventHandler(this.btnEditTrack_Click);
            // 
            // btnDeleteTrack
            // 
            this.btnDeleteTrack.Location = new System.Drawing.Point(230, 8);
            this.btnDeleteTrack.Name = "btnDeleteTrack";
            this.btnDeleteTrack.Size = new System.Drawing.Size(100, 25);
            this.btnDeleteTrack.TabIndex = 2;
            this.btnDeleteTrack.Text = "Удалить";
            this.btnDeleteTrack.UseVisualStyleBackColor = true;
            this.btnDeleteTrack.Click += new System.EventHandler(this.btnDeleteTrack_Click);
            // 
            // dataGridViewTracks
            // 
            this.dataGridViewTracks.AllowUserToAddRows = false;
            this.dataGridViewTracks.AllowUserToDeleteRows = false;
            this.dataGridViewTracks.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridViewTracks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTracks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTracks.Location = new System.Drawing.Point(3, 63);
            this.dataGridViewTracks.Name = "dataGridViewTracks";
            this.dataGridViewTracks.ReadOnly = true;
            this.dataGridViewTracks.RowHeadersVisible = false;
            this.dataGridViewTracks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTracks.Size = new System.Drawing.Size(454, 255);
            this.dataGridViewTracks.TabIndex = 0;
            // 
            // groupBoxAlbumInfo
            // 
            this.groupBoxAlbumInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxAlbumInfo.Controls.Add(this.numTracks);
            this.groupBoxAlbumInfo.Controls.Add(this.label8);
            this.groupBoxAlbumInfo.Controls.Add(this.numRating);
            this.groupBoxAlbumInfo.Controls.Add(this.label7);
            this.groupBoxAlbumInfo.Controls.Add(this.txtDescription);
            this.groupBoxAlbumInfo.Controls.Add(this.label6);
            this.groupBoxAlbumInfo.Controls.Add(this.txtLabel);
            this.groupBoxAlbumInfo.Controls.Add(this.label5);
            this.groupBoxAlbumInfo.Controls.Add(this.txtGenre);
            this.groupBoxAlbumInfo.Controls.Add(this.label4);
            this.groupBoxAlbumInfo.Controls.Add(this.numYear);
            this.groupBoxAlbumInfo.Controls.Add(this.label3);
            this.groupBoxAlbumInfo.Controls.Add(this.txtArtist);
            this.groupBoxAlbumInfo.Controls.Add(this.label2);
            this.groupBoxAlbumInfo.Controls.Add(this.txtTitle);
            this.groupBoxAlbumInfo.Controls.Add(this.label1);
            this.groupBoxAlbumInfo.Location = new System.Drawing.Point(330, 10);
            this.groupBoxAlbumInfo.Name = "groupBoxAlbumInfo";
            this.groupBoxAlbumInfo.Size = new System.Drawing.Size(460, 200);
            this.groupBoxAlbumInfo.TabIndex = 1;
            this.groupBoxAlbumInfo.TabStop = false;
            this.groupBoxAlbumInfo.Text = "Информация об альбоме";
            // 
            // numTracks
            // 
            this.numTracks.Location = new System.Drawing.Point(370, 165);
            this.numTracks.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numTracks.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTracks.Name = "numTracks";
            this.numTracks.Size = new System.Drawing.Size(80, 20);
            this.numTracks.TabIndex = 15;
            this.numTracks.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(257, 167);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(107, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Количество треков:";
            // 
            // numRating
            // 
            this.numRating.DecimalPlaces = 1;
            this.numRating.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numRating.Location = new System.Drawing.Point(170, 165);
            this.numRating.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numRating.Name = "numRating";
            this.numRating.Size = new System.Drawing.Size(80, 20);
            this.numRating.TabIndex = 13;
            this.numRating.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(90, 167);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Рейтинг:";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(90, 90);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.Size = new System.Drawing.Size(360, 70);
            this.txtDescription.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 93);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Описание:";
            // 
            // txtLabel
            // 
            this.txtLabel.Location = new System.Drawing.Point(90, 65);
            this.txtLabel.Name = "txtLabel";
            this.txtLabel.Size = new System.Drawing.Size(160, 20);
            this.txtLabel.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Лейбл:";
            // 
            // txtGenre
            // 
            this.txtGenre.Location = new System.Drawing.Point(320, 65);
            this.txtGenre.Name = "txtGenre";
            this.txtGenre.Size = new System.Drawing.Size(130, 20);
            this.txtGenre.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(266, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Жанр:";
            // 
            // numYear
            // 
            this.numYear.Location = new System.Drawing.Point(390, 40);
            this.numYear.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.numYear.Minimum = new decimal(new int[] {
            1900,
            0,
            0,
            0});
            this.numYear.Name = "numYear";
            this.numYear.Size = new System.Drawing.Size(60, 20);
            this.numYear.TabIndex = 5;
            this.numYear.Value = new decimal(new int[] {
            2024,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(356, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Год:";
            // 
            // txtArtist
            // 
            this.txtArtist.Location = new System.Drawing.Point(90, 40);
            this.txtArtist.Name = "txtArtist";
            this.txtArtist.Size = new System.Drawing.Size(260, 20);
            this.txtArtist.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Исполнитель:";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(90, 15);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(360, 20);
            this.txtTitle.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Название:";
            // 
            // groupBoxAlbumCover
            // 
            this.groupBoxAlbumCover.Controls.Add(this.lblImagePath);
            this.groupBoxAlbumCover.Controls.Add(this.pictureBoxAlbum);
            this.groupBoxAlbumCover.Controls.Add(this.btnSelectImage);
            this.groupBoxAlbumCover.Location = new System.Drawing.Point(10, 10);
            this.groupBoxAlbumCover.Name = "groupBoxAlbumCover";
            this.groupBoxAlbumCover.Size = new System.Drawing.Size(310, 530);
            this.groupBoxAlbumCover.TabIndex = 0;
            this.groupBoxAlbumCover.TabStop = false;
            this.groupBoxAlbumCover.Text = "Обложка альбома";
            // 
            // lblImagePath
            // 
            this.lblImagePath.AutoEllipsis = true;
            this.lblImagePath.Location = new System.Drawing.Point(10, 510);
            this.lblImagePath.Name = "lblImagePath";
            this.lblImagePath.Size = new System.Drawing.Size(290, 15);
            this.lblImagePath.TabIndex = 2;
            this.lblImagePath.Text = "Путь не выбран";
            // 
            // pictureBoxAlbum
            // 
            this.pictureBoxAlbum.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pictureBoxAlbum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxAlbum.Location = new System.Drawing.Point(10, 20);
            this.pictureBoxAlbum.Name = "pictureBoxAlbum";
            this.pictureBoxAlbum.Size = new System.Drawing.Size(290, 420);
            this.pictureBoxAlbum.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxAlbum.TabIndex = 0;
            this.pictureBoxAlbum.TabStop = false;
            // 
            // btnSelectImage
            // 
            this.btnSelectImage.Location = new System.Drawing.Point(10, 450);
            this.btnSelectImage.Name = "btnSelectImage";
            this.btnSelectImage.Size = new System.Drawing.Size(290, 50);
            this.btnSelectImage.TabIndex = 1;
            this.btnSelectImage.Text = "Выбрать обложку...";
            this.btnSelectImage.UseVisualStyleBackColor = true;
            this.btnSelectImage.Click += new System.EventHandler(this.btnSelectImage_Click);
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.btnCancel);
            this.panelButtons.Controls.Add(this.btnSave);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(0, 550);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(800, 50);
            this.panelButtons.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(690, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(580, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 30);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // FormAlbumDetails
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelButtons);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(816, 638);
            this.Name = "FormAlbumDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Детали альбома";
            this.panelMain.ResumeLayout(false);
            this.groupBoxTracks.ResumeLayout(false);
            this.panelTrackButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTracks)).EndInit();
            this.groupBoxAlbumInfo.ResumeLayout(false);
            this.groupBoxAlbumInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTracks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRating)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numYear)).EndInit();
            this.groupBoxAlbumCover.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAlbum)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.GroupBox groupBoxAlbumCover;
        private System.Windows.Forms.PictureBox pictureBoxAlbum;
        private System.Windows.Forms.Button btnSelectImage;
        private System.Windows.Forms.GroupBox groupBoxAlbumInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.TextBox txtArtist;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numYear;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtGenre;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numRating;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numTracks;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBoxTracks;
        private System.Windows.Forms.Panel panelTrackButtons;
        private System.Windows.Forms.Button btnAddTrack;
        private System.Windows.Forms.Button btnEditTrack;
        private System.Windows.Forms.Button btnDeleteTrack;
        private System.Windows.Forms.DataGridView dataGridViewTracks;
        private System.Windows.Forms.Label lblImagePath;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
    }
}