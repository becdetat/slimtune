/*
* Copyright (c) 2007-2009 SlimDX Group
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/
namespace SlimTuneUI
{
	partial class MainWindow
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
			System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
			WeifenLuo.WinFormsUI.Docking.DockPanelSkin dockPanelSkin4 = new WeifenLuo.WinFormsUI.Docking.DockPanelSkin();
			WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin autoHideStripSkin4 = new WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient10 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient22 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin dockPaneStripSkin4 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient dockPaneStripGradient4 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient23 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient11 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient24 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient4 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient25 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient26 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient12 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient27 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient28 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			this.DockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
			this.m_mainMenu = new System.Windows.Forms.MenuStrip();
			this.m_fileMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.m_fileOpenMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.m_fileSaveMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.m_fileExitMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.profilerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.m_profilerRunMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.m_profilerConnectMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.m_helpMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.m_helpAboutMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.m_openDialog = new System.Windows.Forms.OpenFileDialog();
			toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.m_mainMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStripSeparator1
			// 
			toolStripSeparator1.Name = "toolStripSeparator1";
			toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
			// 
			// DockPanel
			// 
			this.DockPanel.ActiveAutoHideContent = null;
			this.DockPanel.BackColor = System.Drawing.SystemColors.ControlDark;
			this.DockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DockPanel.DockBackColor = System.Drawing.SystemColors.ControlDark;
			this.DockPanel.Location = new System.Drawing.Point(0, 24);
			this.DockPanel.Name = "DockPanel";
			this.DockPanel.Size = new System.Drawing.Size(887, 567);
			dockPanelGradient10.EndColor = System.Drawing.SystemColors.ControlLight;
			dockPanelGradient10.StartColor = System.Drawing.SystemColors.ControlLight;
			autoHideStripSkin4.DockStripGradient = dockPanelGradient10;
			tabGradient22.EndColor = System.Drawing.SystemColors.Control;
			tabGradient22.StartColor = System.Drawing.SystemColors.Control;
			tabGradient22.TextColor = System.Drawing.SystemColors.ControlDarkDark;
			autoHideStripSkin4.TabGradient = tabGradient22;
			dockPanelSkin4.AutoHideStripSkin = autoHideStripSkin4;
			tabGradient23.EndColor = System.Drawing.SystemColors.ControlLightLight;
			tabGradient23.StartColor = System.Drawing.SystemColors.ControlLightLight;
			tabGradient23.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripGradient4.ActiveTabGradient = tabGradient23;
			dockPanelGradient11.EndColor = System.Drawing.SystemColors.Control;
			dockPanelGradient11.StartColor = System.Drawing.SystemColors.Control;
			dockPaneStripGradient4.DockStripGradient = dockPanelGradient11;
			tabGradient24.EndColor = System.Drawing.SystemColors.ControlLight;
			tabGradient24.StartColor = System.Drawing.SystemColors.ControlLight;
			tabGradient24.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripGradient4.InactiveTabGradient = tabGradient24;
			dockPaneStripSkin4.DocumentGradient = dockPaneStripGradient4;
			tabGradient25.EndColor = System.Drawing.SystemColors.ActiveCaption;
			tabGradient25.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			tabGradient25.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
			tabGradient25.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
			dockPaneStripToolWindowGradient4.ActiveCaptionGradient = tabGradient25;
			tabGradient26.EndColor = System.Drawing.SystemColors.Control;
			tabGradient26.StartColor = System.Drawing.SystemColors.Control;
			tabGradient26.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripToolWindowGradient4.ActiveTabGradient = tabGradient26;
			dockPanelGradient12.EndColor = System.Drawing.SystemColors.ControlLight;
			dockPanelGradient12.StartColor = System.Drawing.SystemColors.ControlLight;
			dockPaneStripToolWindowGradient4.DockStripGradient = dockPanelGradient12;
			tabGradient27.EndColor = System.Drawing.SystemColors.GradientInactiveCaption;
			tabGradient27.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			tabGradient27.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
			tabGradient27.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripToolWindowGradient4.InactiveCaptionGradient = tabGradient27;
			tabGradient28.EndColor = System.Drawing.Color.Transparent;
			tabGradient28.StartColor = System.Drawing.Color.Transparent;
			tabGradient28.TextColor = System.Drawing.SystemColors.ControlDarkDark;
			dockPaneStripToolWindowGradient4.InactiveTabGradient = tabGradient28;
			dockPaneStripSkin4.ToolWindowGradient = dockPaneStripToolWindowGradient4;
			dockPanelSkin4.DockPaneStripSkin = dockPaneStripSkin4;
			this.DockPanel.Skin = dockPanelSkin4;
			this.DockPanel.TabIndex = 0;
			// 
			// m_mainMenu
			// 
			this.m_mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_fileMenu,
            this.profilerToolStripMenuItem,
            this.m_helpMenu});
			this.m_mainMenu.Location = new System.Drawing.Point(0, 0);
			this.m_mainMenu.Name = "m_mainMenu";
			this.m_mainMenu.Size = new System.Drawing.Size(887, 24);
			this.m_mainMenu.TabIndex = 3;
			this.m_mainMenu.Text = "Main Menu";
			// 
			// m_fileMenu
			// 
			this.m_fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_fileOpenMenu,
            this.m_fileSaveMenu,
            toolStripSeparator1,
            this.m_fileExitMenu});
			this.m_fileMenu.Name = "m_fileMenu";
			this.m_fileMenu.Size = new System.Drawing.Size(37, 20);
			this.m_fileMenu.Text = "&File";
			// 
			// m_fileOpenMenu
			// 
			this.m_fileOpenMenu.Name = "m_fileOpenMenu";
			this.m_fileOpenMenu.Size = new System.Drawing.Size(152, 22);
			this.m_fileOpenMenu.Text = "&Open...";
			this.m_fileOpenMenu.Click += new System.EventHandler(this.m_fileOpenMenu_Click);
			// 
			// m_fileSaveMenu
			// 
			this.m_fileSaveMenu.Enabled = false;
			this.m_fileSaveMenu.Name = "m_fileSaveMenu";
			this.m_fileSaveMenu.Size = new System.Drawing.Size(152, 22);
			this.m_fileSaveMenu.Text = "Save &As...";
			// 
			// m_fileExitMenu
			// 
			this.m_fileExitMenu.Name = "m_fileExitMenu";
			this.m_fileExitMenu.Size = new System.Drawing.Size(152, 22);
			this.m_fileExitMenu.Text = "E&xit";
			this.m_fileExitMenu.Click += new System.EventHandler(this.m_fileExitMenu_Click);
			// 
			// profilerToolStripMenuItem
			// 
			this.profilerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_profilerRunMenu,
            this.m_profilerConnectMenu});
			this.profilerToolStripMenuItem.Name = "profilerToolStripMenuItem";
			this.profilerToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
			this.profilerToolStripMenuItem.Text = "&Profiler";
			// 
			// m_profilerRunMenu
			// 
			this.m_profilerRunMenu.Name = "m_profilerRunMenu";
			this.m_profilerRunMenu.Size = new System.Drawing.Size(152, 22);
			this.m_profilerRunMenu.Text = "&Run...";
			this.m_profilerRunMenu.Click += new System.EventHandler(this.m_profilerRunMenu_Click);
			// 
			// m_profilerConnectMenu
			// 
			this.m_profilerConnectMenu.Name = "m_profilerConnectMenu";
			this.m_profilerConnectMenu.Size = new System.Drawing.Size(152, 22);
			this.m_profilerConnectMenu.Text = "&Connect...";
			this.m_profilerConnectMenu.Click += new System.EventHandler(this.m_profilerConnectMenu_Click);
			// 
			// m_helpMenu
			// 
			this.m_helpMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_helpAboutMenu});
			this.m_helpMenu.Name = "m_helpMenu";
			this.m_helpMenu.Size = new System.Drawing.Size(44, 20);
			this.m_helpMenu.Text = "&Help";
			// 
			// m_helpAboutMenu
			// 
			this.m_helpAboutMenu.Name = "m_helpAboutMenu";
			this.m_helpAboutMenu.Size = new System.Drawing.Size(152, 22);
			this.m_helpAboutMenu.Text = "&About";
			// 
			// m_openDialog
			// 
			this.m_openDialog.DefaultExt = "*.sdf";
			this.m_openDialog.Filter = "Profile Runs (*.sdf)|*.sdf";
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlDark;
			this.ClientSize = new System.Drawing.Size(887, 591);
			this.Controls.Add(this.DockPanel);
			this.Controls.Add(this.m_mainMenu);
			this.IsMdiContainer = true;
			this.MainMenuStrip = this.m_mainMenu;
			this.Name = "MainWindow";
			this.Text = "SlimTune UI";
			this.m_mainMenu.ResumeLayout(false);
			this.m_mainMenu.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip m_mainMenu;
		private System.Windows.Forms.ToolStripMenuItem m_fileMenu;
		private System.Windows.Forms.ToolStripMenuItem m_fileSaveMenu;
		private System.Windows.Forms.ToolStripMenuItem m_fileExitMenu;
		private System.Windows.Forms.ToolStripMenuItem m_fileOpenMenu;
		private System.Windows.Forms.ToolStripMenuItem profilerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem m_profilerRunMenu;
		private System.Windows.Forms.ToolStripMenuItem m_profilerConnectMenu;
		private System.Windows.Forms.ToolStripMenuItem m_helpMenu;
		private System.Windows.Forms.ToolStripMenuItem m_helpAboutMenu;
		public WeifenLuo.WinFormsUI.Docking.DockPanel DockPanel;
		private System.Windows.Forms.OpenFileDialog m_openDialog;
	}
}

