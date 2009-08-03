namespace SlimTuneUI
{
	partial class ChartVisualizer
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
			if(disposing && (components != null))
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
			System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
			this.m_chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.m_backgroundWorker = new System.ComponentModel.BackgroundWorker();
			this.m_refreshTimer = new System.Windows.Forms.Timer(this.components);
			this.m_backButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize) (this.m_chart)).BeginInit();
			this.SuspendLayout();
			// 
			// m_chart
			// 
			this.m_chart.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_chart.BackColor = System.Drawing.Color.Transparent;
			this.m_chart.BackImageTransparentColor = System.Drawing.Color.Transparent;
			this.m_chart.BackSecondaryColor = System.Drawing.Color.Transparent;
			this.m_chart.BorderlineColor = System.Drawing.Color.Transparent;
			chartArea1.AlignmentOrientation = System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal;
			chartArea1.BackColor = System.Drawing.Color.Transparent;
			chartArea1.Name = "CalleesArea";
			chartArea2.AlignmentOrientation = System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal;
			chartArea2.BackColor = System.Drawing.Color.Transparent;
			chartArea2.Name = "CallersArea";
			this.m_chart.ChartAreas.Add(chartArea1);
			this.m_chart.ChartAreas.Add(chartArea2);
			legend1.BackColor = System.Drawing.Color.Transparent;
			legend1.DockedToChartArea = "CalleesArea";
			legend1.IsDockedInsideChartArea = false;
			legend1.IsTextAutoFit = false;
			legend1.LegendStyle = System.Windows.Forms.DataVisualization.Charting.LegendStyle.Column;
			legend1.Name = "CalleesLegend";
			legend1.TableStyle = System.Windows.Forms.DataVisualization.Charting.LegendTableStyle.Tall;
			legend2.BackColor = System.Drawing.Color.Transparent;
			legend2.DockedToChartArea = "CallersArea";
			legend2.IsDockedInsideChartArea = false;
			legend2.IsTextAutoFit = false;
			legend2.LegendStyle = System.Windows.Forms.DataVisualization.Charting.LegendStyle.Column;
			legend2.Name = "CallersLegend";
			legend2.TableStyle = System.Windows.Forms.DataVisualization.Charting.LegendTableStyle.Tall;
			this.m_chart.Legends.Add(legend1);
			this.m_chart.Legends.Add(legend2);
			this.m_chart.Location = new System.Drawing.Point(0, 29);
			this.m_chart.Name = "m_chart";
			this.m_chart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Pastel;
			series1.ChartArea = "CalleesArea";
			series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
			series1.IsValueShownAsLabel = true;
			series1.LabelFormat = "P2";
			series1.Legend = "CalleesLegend";
			series1.Name = "CalleesSeries";
			series1.SmartLabelStyle.AllowOutsidePlotArea = System.Windows.Forms.DataVisualization.Charting.LabelOutsidePlotAreaStyle.No;
			series2.ChartArea = "CallersArea";
			series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
			series2.IsValueShownAsLabel = true;
			series2.LabelFormat = "P2";
			series2.Legend = "CallersLegend";
			series2.Name = "CallersSeries";
			series2.SmartLabelStyle.AllowOutsidePlotArea = System.Windows.Forms.DataVisualization.Charting.LabelOutsidePlotAreaStyle.No;
			this.m_chart.Series.Add(series1);
			this.m_chart.Series.Add(series2);
			this.m_chart.Size = new System.Drawing.Size(707, 521);
			this.m_chart.TabIndex = 0;
			title1.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
			title1.Name = "Callees Title";
			title1.Text = "Callees";
			title2.DockedToChartArea = "CallersArea";
			title2.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			title2.IsDockedInsideChartArea = false;
			title2.Name = "Callers Title";
			title2.Text = "Callers";
			this.m_chart.Titles.Add(title1);
			this.m_chart.Titles.Add(title2);
			this.m_chart.MouseClick += new System.Windows.Forms.MouseEventHandler(this.m_chart_MouseClick);
			this.m_chart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.m_chart_MouseMove);
			// 
			// m_refreshTimer
			// 
			this.m_refreshTimer.Interval = 3000;
			this.m_refreshTimer.Tick += new System.EventHandler(this.m_refreshTimer_Tick);
			// 
			// m_backButton
			// 
			this.m_backButton.Location = new System.Drawing.Point(0, 0);
			this.m_backButton.Name = "m_backButton";
			this.m_backButton.Size = new System.Drawing.Size(75, 23);
			this.m_backButton.TabIndex = 1;
			this.m_backButton.Text = "Back";
			this.m_backButton.UseVisualStyleBackColor = true;
			this.m_backButton.Click += new System.EventHandler(this.m_backButton_Click);
			// 
			// ChartVisualizer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(707, 550);
			this.Controls.Add(this.m_backButton);
			this.Controls.Add(this.m_chart);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.Name = "ChartVisualizer";
			this.Text = "ChartVisualizer";
			((System.ComponentModel.ISupportInitialize) (this.m_chart)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataVisualization.Charting.Chart m_chart;
		private System.ComponentModel.BackgroundWorker m_backgroundWorker;
		private System.Windows.Forms.Timer m_refreshTimer;
		private System.Windows.Forms.Button m_backButton;
	}
}