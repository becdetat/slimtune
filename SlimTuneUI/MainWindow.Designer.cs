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
			System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
			WeifenLuo.WinFormsUI.Docking.DockPanelSkin dockPanelSkin1 = new WeifenLuo.WinFormsUI.Docking.DockPanelSkin();
			WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin autoHideStripSkin1 = new WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient1 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin dockPaneStripSkin1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient dockPaneStripGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient2 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient2 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient3 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient4 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient5 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient3 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient6 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient7 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
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
			this.m_helpContentsMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.m_helpIndexMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.m_helpAboutMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.m_openDialog = new System.Windows.Forms.OpenFileDialog();
			this.m_viewMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.m_viewConnectionsMenu = new System.Windows.Forms.ToolStripMenuItem();
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
			// toolStripMenuItem1
			// 
			toolStripMenuItem1.Name = "toolStripMenuItem1";
			toolStripMenuItem1.Size = new System.Drawing.Size(119, 6);
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
			dockPanelGradient1.EndColor = System.Drawing.SystemColors.ControlLight;
			dockPanelGradient1.StartColor = System.Drawing.SystemColors.ControlLight;
			autoHideStripSkin1.DockStripGradient = dockPanelGradient1;
			tabGradient1.EndColor = System.Drawing.SystemColors.Control;
			tabGradient1.StartColor = System.Drawing.SystemColors.Control;
			tabGradient1.TextColor = System.Drawing.SystemColors.ControlDarkDark;
			autoHideStripSkin1.TabGradient = tabGradient1;
			dockPanelSkin1.AutoHideStripSkin = autoHideStripSkin1;
			tabGradient2.EndColor = System.Drawing.SystemColors.ControlLightLight;
			tabGradient2.StartColor = System.Drawing.SystemColors.ControlLightLight;
			tabGradient2.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripGradient1.ActiveTabGradient = tabGradient2;
			dockPanelGradient2.EndColor = System.Drawing.SystemColors.Control;
			dockPanelGradient2.StartColor = System.Drawing.SystemColors.Control;
			dockPaneStripGradient1.DockStripGradient = dockPanelGradient2;
			tabGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
			tabGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
			tabGradient3.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripGradient1.InactiveTabGradient = tabGradient3;
			dockPaneStripSkin1.DocumentGradient = dockPaneStripGradient1;
			tabGradient4.EndColor = System.Drawing.SystemColors.ActiveCaption;
			tabGradient4.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			tabGradient4.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
			tabGradient4.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
			dockPaneStripToolWindowGradient1.ActiveCaptionGradient = tabGradient4;
			tabGradient5.EndColor = System.Drawing.SystemColors.Control;
			tabGradient5.StartColor = System.Drawing.SystemColors.Control;
			tabGradient5.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripToolWindowGradient1.ActiveTabGradient = tabGradient5;
			dockPanelGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
			dockPanelGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
			dockPaneStripToolWindowGradient1.DockStripGradient = dockPanelGradient3;
			tabGradient6.EndColor = System.Drawing.SystemColors.GradientInactiveCaption;
			tabGradient6.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			tabGradient6.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
			tabGradient6.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripToolWindowGradient1.InactiveCaptionGradient = tabGradient6;
			tabGradient7.EndColor = System.Drawing.Color.Transparent;
			tabGradient7.StartColor = System.Drawing.Color.Transparent;
			tabGradient7.TextColor = System.Drawing.SystemColors.ControlDarkDark;
			dockPaneStripToolWindowGradient1.InactiveTabGradient = tabGradient7;
			dockPaneStripSkin1.ToolWindowGradient = dockPaneStripToolWindowGradient1;
			dockPanelSkin1.DockPaneStripSkin = dockPaneStripSkin1;
			this.DockPanel.Skin = dockPanelSkin1;
			this.DockPanel.TabIndex = 0;
			// 
			// m_mainMenu
			// 
			this.m_mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_fileMenu,
            this.m_viewMenu,
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
			this.m_fileOpenMenu.Size = new System.Drawing.Size(123, 22);
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
			this.m_profilerRunMenu.Size = new System.Drawing.Size(128, 22);
			this.m_profilerRunMenu.Text = "&Run...";
			this.m_profilerRunMenu.Click += new System.EventHandler(this.m_profilerRunMenu_Click);
			// 
			// m_profilerConnectMenu
			// 
			this.m_profilerConnectMenu.Image = ((System.Drawing.Image) (resources.GetObject("m_profilerConnectMenu.Image")));
			this.m_profilerConnectMenu.Name = "m_profilerConnectMenu";
			this.m_profilerConnectMenu.Size = new System.Drawing.Size(128, 22);
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
			// m_helpContentsMenu
			// 
			this.m_helpContentsMenu.Image = ((System.Drawing.Image) (resources.GetObject("m_helpContentsMenu.Image")));
			this.m_helpContentsMenu.Name = "m_helpContentsMenu";
			this.m_helpContentsMenu.Size = new System.Drawing.Size(122, 22);
			this.m_helpContentsMenu.Text = "Contents";
			this.m_helpContentsMenu.Click += new System.EventHandler(this.m_helpContentsMenu_Click);
			// 
			// m_helpIndexMenu
			// 
			this.m_helpIndexMenu.Name = "m_helpIndexMenu";
			this.m_helpIndexMenu.Size = new System.Drawing.Size(122, 22);
			this.m_helpIndexMenu.Text = "Index";
			this.m_helpIndexMenu.Click += new System.EventHandler(this.m_helpIndexMenu_Click);
			// 
			// m_helpAboutMenu
			// 
			this.m_helpAboutMenu.Enabled = false;
			this.m_helpAboutMenu.Name = "m_helpAboutMenu";
			this.m_helpAboutMenu.Size = new System.Drawing.Size(122, 22);
			this.m_helpAboutMenu.Text = "&About";
			// 
			// m_openDialog
			// 
			this.m_openDialog.DefaultExt = "*.sdf";
			this.m_openDialog.Filter = "Profile Runs (*.sdf)|*.sdf";
			// 
			// m_viewMenu
			// 
			this.m_viewMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_viewConnectionsMenu});
			this.m_viewMenu.Name = "m_viewMenu";
			this.m_viewMenu.Size = new System.Drawing.Size(44, 20);
			this.m_viewMenu.Text = "&View";
			// 
			// m_viewConnectionsMenu
			// 
			this.m_viewConnectionsMenu.Name = "m_viewConnectionsMenu";
			this.m_viewConnectionsMenu.Size = new System.Drawing.Size(152, 22);
			this.m_viewConnectionsMenu.Text = "&Connections";
			this.m_viewConnectionsMenu.Click += new System.EventHandler(this.m_viewConnectionsMenu_Click);
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
		private System.Windows.Forms.ToolStripMenuItem m_viewMenu;
		private System.Windows.Forms.ToolStripMenuItem m_viewConnectionsMenu;
	}
}

