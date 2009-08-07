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
			WeifenLuo.WinFormsUI.Docking.DockPanelSkin dockPanelSkin8 = new WeifenLuo.WinFormsUI.Docking.DockPanelSkin();
			WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin autoHideStripSkin8 = new WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient22 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient50 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin dockPaneStripSkin8 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient dockPaneStripGradient8 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient51 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient23 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient52 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient8 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient53 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient54 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient24 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient55 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient56 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
			System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
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
			this.m_helpContentsMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.m_helpIndexMenu = new System.Windows.Forms.ToolStripMenuItem();
			toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.m_mainMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStripSeparator1
			// 
			toolStripSeparator1.Name = "toolStripSeparator1";
			toolStripSeparator1.Size = new System.Drawing.Size(120, 6);
			// 
			// DockPanel
			// 
			this.DockPanel.ActiveAutoHideContent = null;
			this.DockPanel.BackColor = System.Drawing.SystemColors.ControlDark;
			this.DockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DockPanel.DockBackColor = System.Drawing.SystemColors.ControlDark;
			this.DockPanel.DockLeftPortion = 200;
			this.DockPanel.DockRightPortion = 200;
			this.DockPanel.Location = new System.Drawing.Point(0, 24);
			this.DockPanel.Name = "DockPanel";
			this.DockPanel.ShowDocumentIcon = true;
			this.DockPanel.Size = new System.Drawing.Size(887, 567);
			dockPanelGradient22.EndColor = System.Drawing.SystemColors.ControlLight;
			dockPanelGradient22.StartColor = System.Drawing.SystemColors.ControlLight;
			autoHideStripSkin8.DockStripGradient = dockPanelGradient22;
			tabGradient50.EndColor = System.Drawing.SystemColors.Control;
			tabGradient50.StartColor = System.Drawing.SystemColors.Control;
			tabGradient50.TextColor = System.Drawing.SystemColors.ControlDarkDark;
			autoHideStripSkin8.TabGradient = tabGradient50;
			dockPanelSkin8.AutoHideStripSkin = autoHideStripSkin8;
			tabGradient51.EndColor = System.Drawing.SystemColors.ControlLightLight;
			tabGradient51.StartColor = System.Drawing.SystemColors.ControlLightLight;
			tabGradient51.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripGradient8.ActiveTabGradient = tabGradient51;
			dockPanelGradient23.EndColor = System.Drawing.SystemColors.Control;
			dockPanelGradient23.StartColor = System.Drawing.SystemColors.Control;
			dockPaneStripGradient8.DockStripGradient = dockPanelGradient23;
			tabGradient52.EndColor = System.Drawing.SystemColors.ControlLight;
			tabGradient52.StartColor = System.Drawing.SystemColors.ControlLight;
			tabGradient52.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripGradient8.InactiveTabGradient = tabGradient52;
			dockPaneStripSkin8.DocumentGradient = dockPaneStripGradient8;
			tabGradient53.EndColor = System.Drawing.SystemColors.ActiveCaption;
			tabGradient53.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			tabGradient53.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
			tabGradient53.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
			dockPaneStripToolWindowGradient8.ActiveCaptionGradient = tabGradient53;
			tabGradient54.EndColor = System.Drawing.SystemColors.Control;
			tabGradient54.StartColor = System.Drawing.SystemColors.Control;
			tabGradient54.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripToolWindowGradient8.ActiveTabGradient = tabGradient54;
			dockPanelGradient24.EndColor = System.Drawing.SystemColors.ControlLight;
			dockPanelGradient24.StartColor = System.Drawing.SystemColors.ControlLight;
			dockPaneStripToolWindowGradient8.DockStripGradient = dockPanelGradient24;
			tabGradient55.EndColor = System.Drawing.SystemColors.GradientInactiveCaption;
			tabGradient55.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			tabGradient55.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
			tabGradient55.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripToolWindowGradient8.InactiveCaptionGradient = tabGradient55;
			tabGradient56.EndColor = System.Drawing.Color.Transparent;
			tabGradient56.StartColor = System.Drawing.Color.Transparent;
			tabGradient56.TextColor = System.Drawing.SystemColors.ControlDarkDark;
			dockPaneStripToolWindowGradient8.InactiveTabGradient = tabGradient56;
			dockPaneStripSkin8.ToolWindowGradient = dockPaneStripToolWindowGradient8;
			dockPanelSkin8.DockPaneStripSkin = dockPaneStripSkin8;
			this.DockPanel.Skin = dockPanelSkin8;
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
			this.m_fileOpenMenu.Image = ((System.Drawing.Image) (resources.GetObject("m_fileOpenMenu.Image")));
			this.m_fileOpenMenu.Name = "m_fileOpenMenu";
			this.m_fileOpenMenu.Size = new System.Drawing.Size(152, 22);
			this.m_fileOpenMenu.Text = "&Open...";
			this.m_fileOpenMenu.Click += new System.EventHandler(this.m_fileOpenMenu_Click);
			// 
			// m_fileSaveMenu
			// 
			this.m_fileSaveMenu.Enabled = false;
			this.m_fileSaveMenu.Name = "m_fileSaveMenu";
			this.m_fileSaveMenu.Size = new System.Drawing.Size(123, 22);
			this.m_fileSaveMenu.Text = "Save &As...";
			// 
			// m_fileExitMenu
			// 
			this.m_fileExitMenu.Name = "m_fileExitMenu";
			this.m_fileExitMenu.Size = new System.Drawing.Size(123, 22);
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
			this.m_profilerRunMenu.Image = ((System.Drawing.Image) (resources.GetObject("m_profilerRunMenu.Image")));
			this.m_profilerRunMenu.Name = "m_profilerRunMenu";
			this.m_profilerRunMenu.Size = new System.Drawing.Size(152, 22);
			this.m_profilerRunMenu.Text = "&Run...";
			this.m_profilerRunMenu.Click += new System.EventHandler(this.m_profilerRunMenu_Click);
			// 
			// m_profilerConnectMenu
			// 
			this.m_profilerConnectMenu.Image = ((System.Drawing.Image) (resources.GetObject("m_profilerConnectMenu.Image")));
			this.m_profilerConnectMenu.Name = "m_profilerConnectMenu";
			this.m_profilerConnectMenu.Size = new System.Drawing.Size(152, 22);
			this.m_profilerConnectMenu.Text = "&Connect...";
			this.m_profilerConnectMenu.Click += new System.EventHandler(this.m_profilerConnectMenu_Click);
			// 
			// m_helpMenu
			// 
			this.m_helpMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_helpContentsMenu,
            this.m_helpIndexMenu,
            toolStripMenuItem1,
            this.m_helpAboutMenu});
			this.m_helpMenu.Name = "m_helpMenu";
			this.m_helpMenu.Size = new System.Drawing.Size(44, 20);
			this.m_helpMenu.Text = "&Help";
			// 
			// m_helpAboutMenu
			// 
			this.m_helpAboutMenu.Enabled = false;
			this.m_helpAboutMenu.Name = "m_helpAboutMenu";
			this.m_helpAboutMenu.Size = new System.Drawing.Size(152, 22);
			this.m_helpAboutMenu.Text = "&About";
			// 
			// m_openDialog
			// 
			this.m_openDialog.DefaultExt = "*.sdf";
			this.m_openDialog.Filter = "Profile Runs (*.sdf)|*.sdf";
			// 
			// m_helpContentsMenu
			// 
			this.m_helpContentsMenu.Image = ((System.Drawing.Image) (resources.GetObject("m_helpContentsMenu.Image")));
			this.m_helpContentsMenu.Name = "m_helpContentsMenu";
			this.m_helpContentsMenu.Size = new System.Drawing.Size(152, 22);
			this.m_helpContentsMenu.Text = "Contents";
			this.m_helpContentsMenu.Click += new System.EventHandler(this.m_helpContentsMenu_Click);
			// 
			// m_helpIndexMenu
			// 
			this.m_helpIndexMenu.Name = "m_helpIndexMenu";
			this.m_helpIndexMenu.Size = new System.Drawing.Size(152, 22);
			this.m_helpIndexMenu.Text = "Index";
			this.m_helpIndexMenu.Click += new System.EventHandler(this.m_helpIndexMenu_Click);
			// 
			// toolStripMenuItem1
			// 
			toolStripMenuItem1.Name = "toolStripMenuItem1";
			toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlDark;
			this.ClientSize = new System.Drawing.Size(887, 591);
			this.Controls.Add(this.DockPanel);
			this.Controls.Add(this.m_mainMenu);
			this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
			this.IsMdiContainer = true;
			this.MainMenuStrip = this.m_mainMenu;
			this.Name = "MainWindow";
			this.Text = "SlimTune UI";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
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
		private System.Windows.Forms.ToolStripMenuItem m_helpContentsMenu;
		private System.Windows.Forms.ToolStripMenuItem m_helpIndexMenu;
	}
}

