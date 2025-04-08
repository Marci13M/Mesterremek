namespace CareCompass
{
    partial class Doctor_Statistics
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.dTP_Vege = new System.Windows.Forms.DateTimePicker();
            this.dTP_Kezdo = new System.Windows.Forms.DateTimePicker();
            this.tLP_StatisticsIdoszak = new System.Windows.Forms.TableLayoutPanel();
            this.Pn_DoctorStatisticsIdo = new System.Windows.Forms.Panel();
            this.Lb_VegDatum = new System.Windows.Forms.Label();
            this.Lb_KezdetDatum = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tLP_StatisztikaCharts = new System.Windows.Forms.TableLayoutPanel();
            this.tLP_BalCharts = new System.Windows.Forms.TableLayoutPanel();
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart3 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.Btn_OK = new CareCompass.Customs.CCButton();
            this.Btn_Egyedi = new CareCompass.Customs.CCButton();
            this.Btn_Ma = new CareCompass.Customs.CCButton();
            this.Btn_Utolso7Nap = new CareCompass.Customs.CCButton();
            this.Btn_Utolso30Nap = new CareCompass.Customs.CCButton();
            this.Btn_ElozoHonap = new CareCompass.Customs.CCButton();
            this.tLP_StatisticsIdoszak.SuspendLayout();
            this.Pn_DoctorStatisticsIdo.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tLP_StatisztikaCharts.SuspendLayout();
            this.tLP_BalCharts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart3)).BeginInit();
            this.SuspendLayout();
            // 
            // dTP_Vege
            // 
            this.dTP_Vege.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dTP_Vege.CustomFormat = "yyyy MMM dd";
            this.dTP_Vege.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dTP_Vege.Location = new System.Drawing.Point(227, 16);
            this.dTP_Vege.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dTP_Vege.Name = "dTP_Vege";
            this.dTP_Vege.Size = new System.Drawing.Size(132, 22);
            this.dTP_Vege.TabIndex = 2;
            this.dTP_Vege.ValueChanged += new System.EventHandler(this.dTP_Vege_ValueChanged);
            // 
            // dTP_Kezdo
            // 
            this.dTP_Kezdo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dTP_Kezdo.CustomFormat = "yyyy MMM dd";
            this.dTP_Kezdo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dTP_Kezdo.Location = new System.Drawing.Point(40, 17);
            this.dTP_Kezdo.Margin = new System.Windows.Forms.Padding(0);
            this.dTP_Kezdo.Name = "dTP_Kezdo";
            this.dTP_Kezdo.Size = new System.Drawing.Size(132, 22);
            this.dTP_Kezdo.TabIndex = 1;
            this.dTP_Kezdo.Value = new System.DateTime(2025, 2, 17, 0, 0, 0, 0);
            this.dTP_Kezdo.ValueChanged += new System.EventHandler(this.dTP_Kezdo_ValueChanged);
            // 
            // tLP_StatisticsIdoszak
            // 
            this.tLP_StatisticsIdoszak.ColumnCount = 3;
            this.tLP_StatisticsIdoszak.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400F));
            this.tLP_StatisticsIdoszak.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 67F));
            this.tLP_StatisticsIdoszak.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tLP_StatisticsIdoszak.Controls.Add(this.Btn_OK, 0, 0);
            this.tLP_StatisticsIdoszak.Controls.Add(this.Pn_DoctorStatisticsIdo, 0, 0);
            this.tLP_StatisticsIdoszak.Controls.Add(this.tableLayoutPanel2, 2, 0);
            this.tLP_StatisticsIdoszak.Dock = System.Windows.Forms.DockStyle.Top;
            this.tLP_StatisticsIdoszak.Location = new System.Drawing.Point(0, 0);
            this.tLP_StatisticsIdoszak.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tLP_StatisticsIdoszak.Name = "tLP_StatisticsIdoszak";
            this.tLP_StatisticsIdoszak.RowCount = 1;
            this.tLP_StatisticsIdoszak.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tLP_StatisticsIdoszak.Size = new System.Drawing.Size(1120, 62);
            this.tLP_StatisticsIdoszak.TabIndex = 3;
            // 
            // Pn_DoctorStatisticsIdo
            // 
            this.Pn_DoctorStatisticsIdo.Controls.Add(this.Lb_VegDatum);
            this.Pn_DoctorStatisticsIdo.Controls.Add(this.Lb_KezdetDatum);
            this.Pn_DoctorStatisticsIdo.Controls.Add(this.dTP_Kezdo);
            this.Pn_DoctorStatisticsIdo.Controls.Add(this.dTP_Vege);
            this.Pn_DoctorStatisticsIdo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pn_DoctorStatisticsIdo.Location = new System.Drawing.Point(0, 0);
            this.Pn_DoctorStatisticsIdo.Margin = new System.Windows.Forms.Padding(0);
            this.Pn_DoctorStatisticsIdo.Name = "Pn_DoctorStatisticsIdo";
            this.Pn_DoctorStatisticsIdo.Size = new System.Drawing.Size(400, 62);
            this.Pn_DoctorStatisticsIdo.TabIndex = 0;
            // 
            // Lb_VegDatum
            // 
            this.Lb_VegDatum.AutoSize = true;
            this.Lb_VegDatum.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_VegDatum.ForeColor = System.Drawing.Color.White;
            this.Lb_VegDatum.Location = new System.Drawing.Point(227, 15);
            this.Lb_VegDatum.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lb_VegDatum.MinimumSize = new System.Drawing.Size(133, 23);
            this.Lb_VegDatum.Name = "Lb_VegDatum";
            this.Lb_VegDatum.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.Lb_VegDatum.Size = new System.Drawing.Size(133, 23);
            this.Lb_VegDatum.TabIndex = 4;
            this.Lb_VegDatum.Text = "label2";
            this.Lb_VegDatum.Click += new System.EventHandler(this.Lb_VegDatum_Click);
            // 
            // Lb_KezdetDatum
            // 
            this.Lb_KezdetDatum.AutoSize = true;
            this.Lb_KezdetDatum.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_KezdetDatum.ForeColor = System.Drawing.Color.White;
            this.Lb_KezdetDatum.Location = new System.Drawing.Point(40, 16);
            this.Lb_KezdetDatum.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lb_KezdetDatum.MinimumSize = new System.Drawing.Size(133, 23);
            this.Lb_KezdetDatum.Name = "Lb_KezdetDatum";
            this.Lb_KezdetDatum.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.Lb_KezdetDatum.Size = new System.Drawing.Size(133, 23);
            this.Lb_KezdetDatum.TabIndex = 3;
            this.Lb_KezdetDatum.Text = "label1";
            this.Lb_KezdetDatum.Click += new System.EventHandler(this.Lb_KezdetDatum_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.Controls.Add(this.Btn_Egyedi, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.Btn_Ma, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.Btn_Utolso7Nap, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.Btn_Utolso30Nap, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.Btn_ElozoHonap, 4, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(467, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(653, 62);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // tLP_StatisztikaCharts
            // 
            this.tLP_StatisztikaCharts.ColumnCount = 1;
            this.tLP_StatisztikaCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tLP_StatisztikaCharts.Controls.Add(this.tLP_BalCharts, 0, 1);
            this.tLP_StatisztikaCharts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tLP_StatisztikaCharts.Location = new System.Drawing.Point(0, 62);
            this.tLP_StatisztikaCharts.Margin = new System.Windows.Forms.Padding(0);
            this.tLP_StatisztikaCharts.Name = "tLP_StatisztikaCharts";
            this.tLP_StatisztikaCharts.RowCount = 2;
            this.tLP_StatisztikaCharts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tLP_StatisztikaCharts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tLP_StatisztikaCharts.Size = new System.Drawing.Size(1120, 624);
            this.tLP_StatisztikaCharts.TabIndex = 5;
            // 
            // tLP_BalCharts
            // 
            this.tLP_BalCharts.ColumnCount = 1;
            this.tLP_BalCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tLP_BalCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tLP_BalCharts.Controls.Add(this.chart2, 0, 0);
            this.tLP_BalCharts.Controls.Add(this.chart3, 0, 1);
            this.tLP_BalCharts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tLP_BalCharts.Location = new System.Drawing.Point(0, 49);
            this.tLP_BalCharts.Margin = new System.Windows.Forms.Padding(0);
            this.tLP_BalCharts.Name = "tLP_BalCharts";
            this.tLP_BalCharts.RowCount = 2;
            this.tLP_BalCharts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tLP_BalCharts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tLP_BalCharts.Size = new System.Drawing.Size(1120, 575);
            this.tLP_BalCharts.TabIndex = 0;
            // 
            // chart2
            // 
            chartArea1.AxisX.IsMarginVisible = false;
            chartArea1.AxisX.TitleFont = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            chartArea1.AxisY.Interval = 1D;
            chartArea1.AxisY.TitleFont = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            chartArea1.Name = "ChartArea1";
            this.chart2.ChartAreas.Add(chartArea1);
            this.chart2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart2.Location = new System.Drawing.Point(0, 0);
            this.chart2.Margin = new System.Windows.Forms.Padding(0);
            this.chart2.Name = "chart2";
            series1.BorderWidth = 6;
            series1.ChartArea = "ChartArea1";
            series1.Name = "Series1";
            series1.YValuesPerPoint = 4;
            this.chart2.Series.Add(series1);
            this.chart2.Size = new System.Drawing.Size(1120, 287);
            this.chart2.TabIndex = 0;
            this.chart2.Text = "chart2";
            title1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            title1.ForeColor = System.Drawing.Color.IndianRed;
            title1.Name = "Foglalások szám";
            this.chart2.Titles.Add(title1);
            // 
            // chart3
            // 
            chartArea2.AxisX.IsMarginVisible = false;
            chartArea2.AxisX.TitleFont = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            chartArea2.AxisY.TitleFont = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            chartArea2.Name = "ChartArea1";
            this.chart3.ChartAreas.Add(chartArea2);
            this.chart3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart3.Location = new System.Drawing.Point(0, 287);
            this.chart3.Margin = new System.Windows.Forms.Padding(0);
            this.chart3.Name = "chart3";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.SplineArea;
            series2.MarkerBorderWidth = 3;
            series2.MarkerColor = System.Drawing.Color.Red;
            series2.MarkerSize = 7;
            series2.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series2.Name = "Series1";
            this.chart3.Series.Add(series2);
            this.chart3.Size = new System.Drawing.Size(1120, 288);
            this.chart3.TabIndex = 1;
            this.chart3.Text = "chart3";
            title2.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            title2.Name = "Bevétel";
            this.chart3.Titles.Add(title2);
            // 
            // Btn_OK
            // 
            this.Btn_OK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.Btn_OK.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.Btn_OK.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.Btn_OK.BorderRadius = 5;
            this.Btn_OK.BorderSize = 0;
            this.Btn_OK.Dock = System.Windows.Forms.DockStyle.Left;
            this.Btn_OK.FlatAppearance.BorderSize = 0;
            this.Btn_OK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_OK.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Btn_OK.ForeColor = System.Drawing.Color.White;
            this.Btn_OK.Location = new System.Drawing.Point(404, 4);
            this.Btn_OK.Margin = new System.Windows.Forms.Padding(4);
            this.Btn_OK.Name = "Btn_OK";
            this.Btn_OK.Size = new System.Drawing.Size(59, 54);
            this.Btn_OK.TabIndex = 5;
            this.Btn_OK.Text = "OK";
            this.Btn_OK.TextColor = System.Drawing.Color.White;
            this.Btn_OK.UseVisualStyleBackColor = false;
            this.Btn_OK.Click += new System.EventHandler(this.Btn_OK_Click);
            // 
            // Btn_Egyedi
            // 
            this.Btn_Egyedi.BackColor = System.Drawing.Color.Transparent;
            this.Btn_Egyedi.BackgroundColor = System.Drawing.Color.Transparent;
            this.Btn_Egyedi.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(204)))), ((int)(((byte)(188)))));
            this.Btn_Egyedi.BorderRadius = 5;
            this.Btn_Egyedi.BorderSize = 0;
            this.Btn_Egyedi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Btn_Egyedi.FlatAppearance.BorderSize = 0;
            this.Btn_Egyedi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Egyedi.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Btn_Egyedi.ForeColor = System.Drawing.Color.White;
            this.Btn_Egyedi.Location = new System.Drawing.Point(4, 4);
            this.Btn_Egyedi.Margin = new System.Windows.Forms.Padding(4);
            this.Btn_Egyedi.Name = "Btn_Egyedi";
            this.Btn_Egyedi.Size = new System.Drawing.Size(122, 54);
            this.Btn_Egyedi.TabIndex = 0;
            this.Btn_Egyedi.Text = "Egyedi";
            this.Btn_Egyedi.TextColor = System.Drawing.Color.White;
            this.Btn_Egyedi.UseVisualStyleBackColor = false;
            this.Btn_Egyedi.Click += new System.EventHandler(this.Btn_Egyedi_Click);
            // 
            // Btn_Ma
            // 
            this.Btn_Ma.BackColor = System.Drawing.Color.Transparent;
            this.Btn_Ma.BackgroundColor = System.Drawing.Color.Transparent;
            this.Btn_Ma.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(204)))), ((int)(((byte)(188)))));
            this.Btn_Ma.BorderRadius = 5;
            this.Btn_Ma.BorderSize = 0;
            this.Btn_Ma.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Btn_Ma.FlatAppearance.BorderSize = 0;
            this.Btn_Ma.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Ma.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Btn_Ma.ForeColor = System.Drawing.Color.White;
            this.Btn_Ma.Location = new System.Drawing.Point(134, 4);
            this.Btn_Ma.Margin = new System.Windows.Forms.Padding(4);
            this.Btn_Ma.Name = "Btn_Ma";
            this.Btn_Ma.Size = new System.Drawing.Size(122, 54);
            this.Btn_Ma.TabIndex = 1;
            this.Btn_Ma.Text = "Ma";
            this.Btn_Ma.TextColor = System.Drawing.Color.White;
            this.Btn_Ma.UseVisualStyleBackColor = false;
            this.Btn_Ma.Click += new System.EventHandler(this.Btn_Ma_Click);
            // 
            // Btn_Utolso7Nap
            // 
            this.Btn_Utolso7Nap.BackColor = System.Drawing.Color.Transparent;
            this.Btn_Utolso7Nap.BackgroundColor = System.Drawing.Color.Transparent;
            this.Btn_Utolso7Nap.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(204)))), ((int)(((byte)(188)))));
            this.Btn_Utolso7Nap.BorderRadius = 5;
            this.Btn_Utolso7Nap.BorderSize = 0;
            this.Btn_Utolso7Nap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Btn_Utolso7Nap.FlatAppearance.BorderSize = 0;
            this.Btn_Utolso7Nap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Utolso7Nap.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Btn_Utolso7Nap.ForeColor = System.Drawing.Color.White;
            this.Btn_Utolso7Nap.Location = new System.Drawing.Point(264, 4);
            this.Btn_Utolso7Nap.Margin = new System.Windows.Forms.Padding(4);
            this.Btn_Utolso7Nap.Name = "Btn_Utolso7Nap";
            this.Btn_Utolso7Nap.Size = new System.Drawing.Size(122, 54);
            this.Btn_Utolso7Nap.TabIndex = 2;
            this.Btn_Utolso7Nap.Text = "Utolsó 7 nap";
            this.Btn_Utolso7Nap.TextColor = System.Drawing.Color.White;
            this.Btn_Utolso7Nap.UseVisualStyleBackColor = false;
            this.Btn_Utolso7Nap.Click += new System.EventHandler(this.Btn_Utolso7Nap_Click);
            // 
            // Btn_Utolso30Nap
            // 
            this.Btn_Utolso30Nap.BackColor = System.Drawing.Color.Transparent;
            this.Btn_Utolso30Nap.BackgroundColor = System.Drawing.Color.Transparent;
            this.Btn_Utolso30Nap.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(204)))), ((int)(((byte)(188)))));
            this.Btn_Utolso30Nap.BorderRadius = 5;
            this.Btn_Utolso30Nap.BorderSize = 0;
            this.Btn_Utolso30Nap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Btn_Utolso30Nap.FlatAppearance.BorderSize = 0;
            this.Btn_Utolso30Nap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Utolso30Nap.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.Btn_Utolso30Nap.ForeColor = System.Drawing.Color.White;
            this.Btn_Utolso30Nap.Location = new System.Drawing.Point(394, 4);
            this.Btn_Utolso30Nap.Margin = new System.Windows.Forms.Padding(4);
            this.Btn_Utolso30Nap.Name = "Btn_Utolso30Nap";
            this.Btn_Utolso30Nap.Size = new System.Drawing.Size(122, 54);
            this.Btn_Utolso30Nap.TabIndex = 3;
            this.Btn_Utolso30Nap.Text = "Utolsó 30 nap";
            this.Btn_Utolso30Nap.TextColor = System.Drawing.Color.White;
            this.Btn_Utolso30Nap.UseVisualStyleBackColor = false;
            this.Btn_Utolso30Nap.Click += new System.EventHandler(this.Btn_Utolso30Nap_Click);
            // 
            // Btn_ElozoHonap
            // 
            this.Btn_ElozoHonap.BackColor = System.Drawing.Color.Transparent;
            this.Btn_ElozoHonap.BackgroundColor = System.Drawing.Color.Transparent;
            this.Btn_ElozoHonap.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(204)))), ((int)(((byte)(188)))));
            this.Btn_ElozoHonap.BorderRadius = 5;
            this.Btn_ElozoHonap.BorderSize = 0;
            this.Btn_ElozoHonap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Btn_ElozoHonap.FlatAppearance.BorderSize = 0;
            this.Btn_ElozoHonap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_ElozoHonap.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.Btn_ElozoHonap.ForeColor = System.Drawing.Color.White;
            this.Btn_ElozoHonap.Location = new System.Drawing.Point(524, 4);
            this.Btn_ElozoHonap.Margin = new System.Windows.Forms.Padding(4);
            this.Btn_ElozoHonap.Name = "Btn_ElozoHonap";
            this.Btn_ElozoHonap.Size = new System.Drawing.Size(125, 54);
            this.Btn_ElozoHonap.TabIndex = 4;
            this.Btn_ElozoHonap.Text = "Előző hónap";
            this.Btn_ElozoHonap.TextColor = System.Drawing.Color.White;
            this.Btn_ElozoHonap.UseVisualStyleBackColor = false;
            this.Btn_ElozoHonap.Click += new System.EventHandler(this.Btn_ElozoHonap_Click);
            // 
            // Doctor_Statistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(57)))), ((int)(((byte)(92)))));
            this.ClientSize = new System.Drawing.Size(1120, 686);
            this.Controls.Add(this.tLP_StatisztikaCharts);
            this.Controls.Add(this.tLP_StatisticsIdoszak);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximumSize = new System.Drawing.Size(5120, 2658);
            this.Name = "Doctor_Statistics";
            this.Text = "Doctor_Statistics";
            this.Load += new System.EventHandler(this.Doctor_Statistics_Load);
            this.tLP_StatisticsIdoszak.ResumeLayout(false);
            this.Pn_DoctorStatisticsIdo.ResumeLayout(false);
            this.Pn_DoctorStatisticsIdo.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tLP_StatisztikaCharts.ResumeLayout(false);
            this.tLP_BalCharts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DateTimePicker dTP_Kezdo;
        private System.Windows.Forms.DateTimePicker dTP_Vege;
        private System.Windows.Forms.TableLayoutPanel tLP_StatisticsIdoszak;
        private System.Windows.Forms.Panel Pn_DoctorStatisticsIdo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Customs.CCButton Btn_Egyedi;
        private Customs.CCButton Btn_Ma;
        private Customs.CCButton Btn_Utolso7Nap;
        private Customs.CCButton Btn_Utolso30Nap;
        private Customs.CCButton Btn_ElozoHonap;
        private Customs.CCButton Btn_OK;
        private System.Windows.Forms.TableLayoutPanel tLP_StatisztikaCharts;
        private System.Windows.Forms.TableLayoutPanel tLP_BalCharts;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart3;
        private System.Windows.Forms.Label Lb_VegDatum;
        private System.Windows.Forms.Label Lb_KezdetDatum;
    }
}