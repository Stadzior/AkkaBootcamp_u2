using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Akka.Actor;

namespace ChartApp
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            ChartArea chartArea2 = new ChartArea();
            Legend legend2 = new Legend();
            Series series2 = new Series();
            this.sysChart = new Chart();
            this.btnCpu = new Button();
            this.btnMemory = new Button();
            this.btnDisk = new Button();
            ((ISupportInitialize)(this.sysChart)).BeginInit();
            this.SuspendLayout();
            // 
            // sysChart
            // 
            chartArea2.Name = "ChartArea1";
            this.sysChart.ChartAreas.Add(chartArea2);
            this.sysChart.Dock = DockStyle.Fill;
            legend2.Name = "Legend1";
            this.sysChart.Legends.Add(legend2);
            this.sysChart.Location = new Point(0, 0);
            this.sysChart.Name = "sysChart";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.sysChart.Series.Add(series2);
            this.sysChart.Size = new Size(684, 446);
            this.sysChart.TabIndex = 0;
            this.sysChart.Text = "sysChart";
            // 
            // btnCpu
            // 
            this.btnCpu.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnCpu.Location = new Point(562, 274);
            this.btnCpu.Name = "btnCpu";
            this.btnCpu.Size = new Size(110, 41);
            this.btnCpu.TabIndex = 1;
            this.btnCpu.Text = "CPU (ON)";
            this.btnCpu.UseVisualStyleBackColor = true;
            this.btnCpu.Click += new EventHandler(this.BtnCpu_Click);
            // 
            // btnMemory
            // 
            this.btnMemory.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnMemory.Location = new Point(562, 321);
            this.btnMemory.Name = "btnMemory";
            this.btnMemory.Size = new Size(110, 41);
            this.btnMemory.TabIndex = 2;
            this.btnMemory.Text = "MEMORY (OFF)";
            this.btnMemory.UseVisualStyleBackColor = true;
            this.btnMemory.Click += new EventHandler(this.BtnMemory_Click);
            // 
            // btnDisk
            // 
            this.btnDisk.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnDisk.Location = new Point(562, 368);
            this.btnDisk.Name = "btnDisk";
            this.btnDisk.Size = new Size(110, 41);
            this.btnDisk.TabIndex = 3;
            this.btnDisk.Text = "DISK (OFF)";
            this.btnDisk.UseVisualStyleBackColor = true;
            this.btnDisk.Click += new EventHandler(this.BtnDisk_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(684, 446);
            this.Controls.Add(this.btnDisk);
            this.Controls.Add(this.btnMemory);
            this.Controls.Add(this.btnCpu);
            this.Controls.Add(this.sysChart);
            this.Name = "Main";
            this.Text = "System Metrics";
            this.FormClosing += new FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new EventHandler(this.Main_Load);
            ((ISupportInitialize)(this.sysChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Chart sysChart;
        private Button btnCpu;
        private Button btnMemory;
        private Button btnDisk;
    }
}

