

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO.Ports;
using System.Management;
using System.Windows.Forms;
using ZedGraph;

namespace WindowsFormsApplication4
{
  public class Form1 : Form
  {
    private string datafromcom;
    public double xmajorstep = 1.0;
    public double xmax = 10.0;
    public double subtrahend = 10.0;
    public double divisor = 1000.0;
    private int tickStart = 0;
    private IContainer components = (IContainer) null;
    private ZedGraphControl z1;
    private SerialPort serialPort;
    private RichTextBox richtextbox;
    private Button connectbutton;
    private Button disconnectbutton;
    private Panel panel1;
    private Panel panel6;
    private Panel panel4;
    private Panel panel2;
    private CheckBox sawtoothcheckbox;
    private CheckBox rectanglecheckbox;
    private CheckBox trianglecheckbox;
    private Button resetbutton;
    private Panel panel3;
    private PictureBox pictureBox1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label3;
    private PictureBox pictureBox5;
    private CheckBox sinecheckbox;
    private PictureBox pictureBox2;
    private System.Windows.Forms.Label label2;
    private Panel panel5;
    private System.Windows.Forms.Label label4;
    private GroupBox groupBox1;
    private TrackBar dutycycletrackbar;
    private ToolTip Sasabihinmopagtinuroka;
    private TrackBar frequencytrackbar;
    private System.Windows.Forms.Label dutycyclelabel;
    private System.Windows.Forms.Label frequencylabel;
    private PictureBox pictureBox7;
    private CheckBox horizontalcheckbutton;
    private CheckBox verticalcheckbox;
    private CheckBox halfsecond;
    private Timer timer1;
    private CheckBox tenthsecond;
    private PictureBox pictureBox4;
    private CheckBox meleeseconds;
    private GroupBox groupBox2;
    private GroupBox groupBox3;

    public Form1() => this.InitializeComponent();

    public void Form1_Load(object sender, EventArgs e)
    {
      this.creategraph(this.z1);
      this.tickStart = Environment.TickCount;
    }

    public void creategraph(ZedGraphControl z1)
    {
      GraphPane graphPane = z1.GraphPane;
      z1.IsShowHScrollBar = true;
      z1.IsShowVScrollBar = true;
      z1.IsAutoScrollRange = true;
      z1.IsEnableHZoom = false;
      z1.IsEnableVZoom = false;
      z1.IsSynchronizeYAxes = true;
      z1.IsSynchronizeXAxes = true;
      z1.GraphPane.IsBoundedRanges = true;
      z1.IsEnableHPan = true;
      z1.IsEnableVPan = true;
      z1.IsShowPointValues = false;
      graphPane.Title.FontSpec.FontColor = Color.Black;
      graphPane.Title.Text = "  ";
      graphPane.XAxis.Title.Text = "";
      graphPane.YAxis.Title.Text = "";
      RollingPointPairList points = new RollingPointPairList(20000);
      LineItem lineItem = graphPane.AddCurve((string) null, (IPointList) points, Color.Black, SymbolType.None);
      graphPane.XAxis.Scale.Min = 0.0;
      graphPane.XAxis.Scale.Max = this.xmax;
      graphPane.XAxis.Scale.MinorStep = 0.1;
      graphPane.XAxis.Scale.MajorStep = this.xmajorstep;
      graphPane.XAxis.Scale.IsVisible = false;
      graphPane.XAxis.Type = AxisType.Linear;
      graphPane.YAxis.Scale.Min = -10.0;
      graphPane.YAxis.Scale.Max = 10.0;
      graphPane.YAxis.Scale.MinorStep = 0.1;
      graphPane.YAxis.Scale.MajorStep = 1.0;
      graphPane.XAxis.MajorGrid.IsVisible = true;
      graphPane.YAxis.MajorGrid.IsVisible = true;
      graphPane.XAxis.MajorGrid.Color = Color.Black;
      graphPane.YAxis.MajorGrid.Color = Color.Black;
      graphPane.Fill.Color = Color.DarkGray;
      lineItem.Line.Width = 2f;
      lineItem.Symbol.Size = 8f;
      lineItem.Symbol.Fill = new Fill(Color.White);
      graphPane.Chart.Fill = new Fill(Color.White, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), -45f);
      graphPane.Margin.Top = 1f;
      graphPane.Margin.Left = 30f;
      graphPane.Margin.Bottom = 35f;
      this.dutycycletrackbar.Enabled = false;
      this.frequencytrackbar.Enabled = false;
      this.sinecheckbox.Enabled = false;
      this.rectanglecheckbox.Enabled = false;
      this.trianglecheckbox.Enabled = false;
      this.sawtoothcheckbox.Enabled = false;
      z1.AxisChange();
      z1.GraphPane.AxisChange();
    }

    public void connectbutton_Click(object sender, EventArgs e)
    {
      try
      {
        foreach (Form1.COMPortInfo comPortInfo in Form1.COMPortInfo.GetCOMPortsInfo())
          this.serialPort.PortName = comPortInfo.Name;
        this.serialPort.BaudRate = 9600;
        if (this.serialPort.IsOpen)
          return;
        this.serialPort.Open();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        int num = (int) MessageBox.Show("Please plug the USB cable again.");
        Application.Restart();
      }
      this.frequencytrackbar.Enabled = true;
      this.dutycycletrackbar.Enabled = true;
      this.serialPort.DtrEnable = true;
      this.serialPort.RtsEnable = true;
      this.sinecheckbox.Enabled = true;
      this.rectanglecheckbox.Enabled = true;
      this.trianglecheckbox.Enabled = true;
      this.sawtoothcheckbox.Enabled = true;
      this.connectbutton.Enabled = false;
      this.disconnectbutton.Enabled = true;
      this.z1.IsShowPointValues = false;
    }

    public void disconnectbutton_Click(object sender, EventArgs e)
    {
      if (!this.serialPort.IsOpen)
        return;
      this.serialPort.Close();
      this.connectbutton.Enabled = true;
      this.disconnectbutton.Enabled = false;
      this.sinecheckbox.Enabled = false;
      this.rectanglecheckbox.Enabled = false;
      this.trianglecheckbox.Enabled = false;
      this.sawtoothcheckbox.Enabled = false;
      this.sinecheckbox.Checked = false;
      this.rectanglecheckbox.Checked = false;
      this.trianglecheckbox.Checked = false;
      this.sawtoothcheckbox.Checked = false;
      this.frequencytrackbar.Enabled = false;
      this.dutycycletrackbar.Enabled = false;
      this.z1.IsShowPointValues = true;
    }

    public void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
      try
      {
        while (this.serialPort.BytesToRead > 0)
        {
          this.datafromcom = this.serialPort.ReadLine();
          if (this.datafromcom.Trim() != "")
          {
            float y = float.Parse(this.datafromcom);
            if (this.z1.GraphPane.CurveList.Count <= 0 || !(this.z1.GraphPane.CurveList[0] is LineItem curve) || !(curve.Points is IPointListEdit points))
              break;
            double x = (double) (Environment.TickCount - this.tickStart) / this.divisor;
            points.Add(x, (double) y);
            Scale scale = this.z1.GraphPane.XAxis.Scale;
            if (x > scale.Max - scale.MajorStep)
            {
              scale.Max = x + scale.MajorStep;
              scale.Min = scale.Max - this.subtrahend;
            }
            this.z1.AxisChange();
            this.z1.Invalidate();
            this.BeginInvoke((Delegate) new Form1.SetTextCallback(this.SetText), (object) this.datafromcom);
          }
        }
      }
      catch (Exception ex)
      {
      }
    }

    public void SetText(string text)
    {
      this.richtextbox.Text = text;
      this.richtextbox.ScrollToCaret();
    }

    public void checkBox1_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.sinecheckbox.Checked)
        return;
      int num = 0;
      do
      {
        this.serialPort.Write("0");
        this.serialPort.Write(",");
        ++num;
      }
      while (num != 100);
      this.rectanglecheckbox.Checked = false;
      this.trianglecheckbox.Checked = false;
      this.sawtoothcheckbox.Checked = false;
      this.sinecheckbox.Enabled = false;
      this.rectanglecheckbox.Enabled = true;
      this.trianglecheckbox.Enabled = true;
      this.sawtoothcheckbox.Enabled = true;
    }

    public void checkBox2_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.rectanglecheckbox.Checked)
        return;
      int num = 0;
      do
      {
        this.serialPort.Write("1");
        this.serialPort.Write(",");
        ++num;
      }
      while (num != 100);
      this.sinecheckbox.Checked = false;
      this.trianglecheckbox.Checked = false;
      this.sawtoothcheckbox.Checked = false;
      this.rectanglecheckbox.Enabled = false;
      this.sinecheckbox.Enabled = true;
      this.trianglecheckbox.Enabled = true;
      this.sawtoothcheckbox.Enabled = true;
    }

    public void checkBox3_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.trianglecheckbox.Checked)
        return;
      int num = 0;
      do
      {
        this.serialPort.Write("2");
        this.serialPort.Write(",");
        ++num;
      }
      while (num != 100);
      this.sinecheckbox.Checked = false;
      this.rectanglecheckbox.Checked = false;
      this.sawtoothcheckbox.Checked = false;
      this.trianglecheckbox.Enabled = false;
      this.sinecheckbox.Enabled = true;
      this.rectanglecheckbox.Enabled = true;
      this.sawtoothcheckbox.Enabled = true;
    }

    public void checkBox4_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.sawtoothcheckbox.Checked)
        return;
      int num = 0;
      do
      {
        this.serialPort.Write("3");
        this.serialPort.Write(",");
        ++num;
      }
      while (num != 100);
      this.sinecheckbox.Checked = false;
      this.trianglecheckbox.Checked = false;
      this.rectanglecheckbox.Checked = false;
      this.sawtoothcheckbox.Enabled = false;
      this.sinecheckbox.Enabled = true;
      this.rectanglecheckbox.Enabled = true;
      this.trianglecheckbox.Enabled = true;
    }

    public void resetbutton_Click(object sender, EventArgs e) => Application.Restart();

    public void dutycyclelabel_Click(object sender, EventArgs e)
    {
    }

    public void dutycycletrackbar_Scroll(object sender, EventArgs e)
    {
      int num = 113;
      this.dutycyclelabel.Text = (Convert.ToInt32(this.dutycycletrackbar.Value) - num).ToString() + "%";
      this.serialPort.Write(this.dutycycletrackbar.Value.ToString());
      this.serialPort.Write(",");
    }

    public void frequencytrackbar_Scroll(object sender, EventArgs e)
    {
      if (this.frequencytrackbar.Value >= 4 && this.frequencytrackbar.Value <= 12)
        this.frequencylabel.Text = (((double) this.frequencytrackbar.Value - 3.0) / 10.0).ToString() + "Hz";
      else if (this.frequencytrackbar.Value >= 13 && this.frequencytrackbar.Value <= 112)
        this.frequencylabel.Text = ((double) this.frequencytrackbar.Value - 12.0).ToString() + "Hz";
      this.serialPort.Write(this.frequencytrackbar.Value.ToString());
      this.serialPort.Write(",");
    }

    public void verticalcheckbox_CheckedChanged(object sender, EventArgs e)
    {
      if (this.verticalcheckbox.Checked)
        this.z1.IsEnableVZoom = true;
      else
        this.z1.IsEnableVZoom = false;
    }

    public void horizontalcheckbutton_CheckedChanged(object sender, EventArgs e)
    {
      if (this.horizontalcheckbutton.Checked)
        this.z1.IsEnableHZoom = true;
      else
        this.z1.IsEnableHZoom = false;
    }

    public void millisecond_CheckedChanged(object sender, EventArgs e)
    {
      if (this.halfsecond.Checked)
      {
        this.divisor = 500.0;
        this.subtrahend = 10.0;
        this.halfsecond.Enabled = false;
        this.tenthsecond.Enabled = false;
        this.meleeseconds.Enabled = false;
      }
      else
      {
        this.divisor = 1000.0;
        this.subtrahend = 10.0;
      }
    }

    public void timer1_Tick(object sender, EventArgs e)
    {
    }

    private void tenthsecond_CheckedChanged(object sender, EventArgs e)
    {
      if (this.tenthsecond.Checked)
      {
        this.divisor = 100.0;
        this.subtrahend = 10.0;
        this.tenthsecond.Enabled = false;
        this.halfsecond.Enabled = false;
        this.meleeseconds.Enabled = false;
      }
      else
      {
        this.divisor = 1000.0;
        this.subtrahend = 10.0;
      }
    }

    private void meleeseconds_CheckedChanged(object sender, EventArgs e)
    {
      if (this.meleeseconds.Checked)
      {
        this.divisor = 1.0;
        this.subtrahend = 10.0;
        this.tenthsecond.Enabled = false;
        this.halfsecond.Enabled = false;
        this.meleeseconds.Enabled = false;
      }
      else
      {
        this.divisor = 1000.0;
        this.subtrahend = 10.0;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form1));
      this.z1 = new ZedGraphControl();
      this.serialPort = new SerialPort(this.components);
      this.richtextbox = new RichTextBox();
      this.connectbutton = new Button();
      this.disconnectbutton = new Button();
      this.panel1 = new Panel();
      this.label3 = new System.Windows.Forms.Label();
      this.panel6 = new Panel();
      this.pictureBox1 = new PictureBox();
      this.label1 = new System.Windows.Forms.Label();
      this.panel3 = new Panel();
      this.groupBox3 = new GroupBox();
      this.dutycycletrackbar = new TrackBar();
      this.dutycyclelabel = new System.Windows.Forms.Label();
      this.resetbutton = new Button();
      this.groupBox2 = new GroupBox();
      this.frequencylabel = new System.Windows.Forms.Label();
      this.frequencytrackbar = new TrackBar();
      this.groupBox1 = new GroupBox();
      this.sawtoothcheckbox = new CheckBox();
      this.sinecheckbox = new CheckBox();
      this.pictureBox2 = new PictureBox();
      this.rectanglecheckbox = new CheckBox();
      this.trianglecheckbox = new CheckBox();
      this.panel5 = new Panel();
      this.label4 = new System.Windows.Forms.Label();
      this.panel4 = new Panel();
      this.panel2 = new Panel();
      this.pictureBox5 = new PictureBox();
      this.label2 = new System.Windows.Forms.Label();
      this.Sasabihinmopagtinuroka = new ToolTip(this.components);
      this.halfsecond = new CheckBox();
      this.tenthsecond = new CheckBox();
      this.meleeseconds = new CheckBox();
      this.pictureBox7 = new PictureBox();
      this.horizontalcheckbutton = new CheckBox();
      this.verticalcheckbox = new CheckBox();
      this.timer1 = new Timer(this.components);
      this.pictureBox4 = new PictureBox();
      this.panel1.SuspendLayout();
      this.panel6.SuspendLayout();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.panel3.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.dutycycletrackbar.BeginInit();
      this.groupBox2.SuspendLayout();
      this.frequencytrackbar.BeginInit();
      this.groupBox1.SuspendLayout();
      ((ISupportInitialize) this.pictureBox2).BeginInit();
      this.panel5.SuspendLayout();
      this.panel4.SuspendLayout();
      this.panel2.SuspendLayout();
      ((ISupportInitialize) this.pictureBox5).BeginInit();
      ((ISupportInitialize) this.pictureBox7).BeginInit();
      ((ISupportInitialize) this.pictureBox4).BeginInit();
      this.SuspendLayout();
      this.z1.Cursor = Cursors.Cross;
      this.z1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.z1.ForeColor = Color.FromArgb(41, 53, 65);
      this.z1.Location = new Point(287, 77);
      this.z1.Margin = new Padding(40, 1, 5, 40);
      this.z1.Name = "z1";
      this.z1.ScrollGrace = 0.0;
      this.z1.ScrollMaxX = 0.0;
      this.z1.ScrollMaxY = 0.0;
      this.z1.ScrollMaxY2 = 0.0;
      this.z1.ScrollMinX = 0.0;
      this.z1.ScrollMinY = 0.0;
      this.z1.ScrollMinY2 = 0.0;
      this.z1.Size = new Size(865, 563);
      this.z1.TabIndex = 0;
      this.serialPort.DtrEnable = true;
      this.serialPort.RtsEnable = true;
      this.serialPort.DataReceived += new SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
      this.richtextbox.BackColor = SystemColors.ScrollBar;
      this.richtextbox.BorderStyle = BorderStyle.FixedSingle;
      this.richtextbox.Font = new Font("Century Gothic", 14f);
      this.richtextbox.ForeColor = Color.Black;
      this.richtextbox.HideSelection = false;
      this.richtextbox.Location = new Point(1058, 122);
      this.richtextbox.Name = "richtextbox";
      this.richtextbox.ScrollBars = RichTextBoxScrollBars.None;
      this.richtextbox.Size = new Size(62, 26);
      this.richtextbox.TabIndex = 15;
      this.richtextbox.Text = "";
      this.Sasabihinmopagtinuroka.SetToolTip((Control) this.richtextbox, "Current Value");
      this.connectbutton.BackColor = Color.FromArgb(50, 50, 50);
      this.connectbutton.BackgroundImage = (Image) componentResourceManager.GetObject("connectbutton.BackgroundImage");
      this.connectbutton.BackgroundImageLayout = ImageLayout.Zoom;
      this.connectbutton.FlatStyle = FlatStyle.Flat;
      this.connectbutton.Font = new Font("Bookman Old Style", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.connectbutton.ForeColor = Color.FromArgb(19, 29, 37);
      this.connectbutton.Location = new Point(20, 442);
      this.connectbutton.Name = "connectbutton";
      this.connectbutton.Size = new Size(71, 66);
      this.connectbutton.TabIndex = 1;
      this.connectbutton.Text = " ";
      this.Sasabihinmopagtinuroka.SetToolTip((Control) this.connectbutton, "Connect");
      this.connectbutton.UseVisualStyleBackColor = false;
      this.connectbutton.Click += new EventHandler(this.connectbutton_Click);
      this.disconnectbutton.BackColor = Color.FromArgb(50, 50, 50);
      this.disconnectbutton.BackgroundImage = (Image) componentResourceManager.GetObject("disconnectbutton.BackgroundImage");
      this.disconnectbutton.BackgroundImageLayout = ImageLayout.Zoom;
      this.disconnectbutton.Enabled = false;
      this.disconnectbutton.FlatStyle = FlatStyle.Flat;
      this.disconnectbutton.Font = new Font("Bookman Old Style", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.disconnectbutton.ForeColor = Color.FromArgb(19, 29, 37);
      this.disconnectbutton.Location = new Point(109, 442);
      this.disconnectbutton.Name = "disconnectbutton";
      this.disconnectbutton.Size = new Size(71, 66);
      this.disconnectbutton.TabIndex = 13;
      this.disconnectbutton.Text = " ";
      this.Sasabihinmopagtinuroka.SetToolTip((Control) this.disconnectbutton, "Disconnect");
      this.disconnectbutton.UseVisualStyleBackColor = false;
      this.disconnectbutton.Click += new EventHandler(this.disconnectbutton_Click);
      this.panel1.BackColor = Color.FromArgb(0, 49, 69);
      this.panel1.BackgroundImageLayout = ImageLayout.Stretch;
      this.panel1.Controls.Add((Control) this.label3);
      this.panel1.Controls.Add((Control) this.panel6);
      this.panel1.Controls.Add((Control) this.panel3);
      this.panel1.Dock = DockStyle.Left;
      this.panel1.ForeColor = SystemColors.Window;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(288, 640);
      this.panel1.TabIndex = 36;
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Century Gothic", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label3.ForeColor = SystemColors.ButtonHighlight;
      this.label3.Location = new Point(15, 87);
      this.label3.Name = "label3";
      this.label3.Size = new Size(148, 24);
      this.label3.TabIndex = 37;
      this.label3.Text = "Configuration";
      this.panel6.BackColor = Color.FromArgb(226, 126, 0);
      this.panel6.BackgroundImageLayout = ImageLayout.Stretch;
      this.panel6.Controls.Add((Control) this.pictureBox1);
      this.panel6.Controls.Add((Control) this.label1);
      this.panel6.Location = new Point(0, 0);
      this.panel6.Name = "panel6";
      this.panel6.Size = new Size(288, 77);
      this.panel6.TabIndex = 35;
      this.pictureBox1.BackColor = Color.FromArgb(226, 126, 0);
      this.pictureBox1.Image = (Image) componentResourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.InitialImage = (Image) componentResourceManager.GetObject("pictureBox1.InitialImage");
      this.pictureBox1.Location = new Point(18, 19);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(93, 42);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
      this.pictureBox1.TabIndex = 1;
      this.pictureBox1.TabStop = false;
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Rockwell", 26.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.ForeColor = SystemColors.ControlLightLight;
      this.label1.Location = new Point(102, 18);
      this.label1.Name = "label1";
      this.label1.Size = new Size(159, 43);
      this.label1.TabIndex = 0;
      this.label1.Text = "GenOsc";
      this.panel3.BackColor = Color.FromArgb(29, 35, 41);
      this.panel3.BackgroundImageLayout = ImageLayout.Stretch;
      this.panel3.Controls.Add((Control) this.groupBox3);
      this.panel3.Controls.Add((Control) this.disconnectbutton);
      this.panel3.Controls.Add((Control) this.resetbutton);
      this.panel3.Controls.Add((Control) this.groupBox2);
      this.panel3.Controls.Add((Control) this.groupBox1);
      this.panel3.Controls.Add((Control) this.panel5);
      this.panel3.Controls.Add((Control) this.connectbutton);
      this.panel3.ForeColor = Color.FromArgb(29, 35, 41);
      this.panel3.Location = new Point(0, 118);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(288, 523);
      this.panel3.TabIndex = 36;
      this.groupBox3.Controls.Add((Control) this.dutycycletrackbar);
      this.groupBox3.Controls.Add((Control) this.dutycyclelabel);
      this.groupBox3.Font = new Font("Century Gothic", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.groupBox3.ForeColor = Color.White;
      this.groupBox3.Location = new Point(9, 294);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new Size(272, 83);
      this.groupBox3.TabIndex = 45;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Duty Cycle";
      this.dutycycletrackbar.BackColor = Color.FromArgb(50, 50, 50);
      this.dutycycletrackbar.Cursor = Cursors.NoMoveHoriz;
      this.dutycycletrackbar.LargeChange = 1;
      this.dutycycletrackbar.Location = new Point(6, 27);
      this.dutycycletrackbar.Maximum = 213;
      this.dutycycletrackbar.Minimum = 113;
      this.dutycycletrackbar.Name = "dutycycletrackbar";
      this.dutycycletrackbar.Size = new Size(173, 45);
      this.dutycycletrackbar.TabIndex = 41;
      this.dutycycletrackbar.TickStyle = TickStyle.None;
      this.Sasabihinmopagtinuroka.SetToolTip((Control) this.dutycycletrackbar, "Toggle your desired Duty Cycle");
      this.dutycycletrackbar.Value = 163;
      this.dutycycletrackbar.Scroll += new EventHandler(this.dutycycletrackbar_Scroll);
      this.dutycyclelabel.AutoSize = true;
      this.dutycyclelabel.BackColor = Color.FromArgb(50, 50, 50);
      this.dutycyclelabel.BorderStyle = BorderStyle.FixedSingle;
      this.dutycyclelabel.FlatStyle = FlatStyle.System;
      this.dutycyclelabel.Font = new Font("Century Gothic", 18f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dutycyclelabel.ForeColor = Color.White;
      this.dutycyclelabel.Location = new Point(186, 35);
      this.dutycyclelabel.Name = "dutycyclelabel";
      this.dutycyclelabel.Size = new Size(60, 32);
      this.dutycyclelabel.TabIndex = 42;
      this.dutycyclelabel.Text = "50%";
      this.Sasabihinmopagtinuroka.SetToolTip((Control) this.dutycyclelabel, "Duty cycle value percentage");
      this.dutycyclelabel.Click += new EventHandler(this.dutycyclelabel_Click);
      this.resetbutton.Anchor = AnchorStyles.None;
      this.resetbutton.BackColor = Color.FromArgb(50, 50, 50);
      this.resetbutton.BackgroundImage = (Image) componentResourceManager.GetObject("resetbutton.BackgroundImage");
      this.resetbutton.BackgroundImageLayout = ImageLayout.Zoom;
      this.resetbutton.FlatStyle = FlatStyle.Flat;
      this.resetbutton.Font = new Font("Bookman Old Style", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.resetbutton.ForeColor = Color.FromArgb(19, 29, 37);
      this.resetbutton.Location = new Point(197, 441);
      this.resetbutton.Name = "resetbutton";
      this.resetbutton.Size = new Size(71, 67);
      this.resetbutton.TabIndex = 35;
      this.resetbutton.Text = " ";
      this.Sasabihinmopagtinuroka.SetToolTip((Control) this.resetbutton, "Restart the Application");
      this.resetbutton.UseVisualStyleBackColor = false;
      this.resetbutton.Click += new EventHandler(this.resetbutton_Click);
      this.groupBox2.Controls.Add((Control) this.frequencylabel);
      this.groupBox2.Controls.Add((Control) this.frequencytrackbar);
      this.groupBox2.Font = new Font("Century Gothic", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.groupBox2.ForeColor = Color.White;
      this.groupBox2.Location = new Point(9, 195);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(272, 83);
      this.groupBox2.TabIndex = 41;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Frequency";
      this.frequencylabel.AutoSize = true;
      this.frequencylabel.BackColor = Color.FromArgb(50, 50, 50);
      this.frequencylabel.BorderStyle = BorderStyle.FixedSingle;
      this.frequencylabel.FlatStyle = FlatStyle.System;
      this.frequencylabel.Font = new Font("Century Gothic", 18f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.frequencylabel.ForeColor = Color.White;
      this.frequencylabel.Location = new Point(185, 31);
      this.frequencylabel.Name = "frequencylabel";
      this.frequencylabel.Size = new Size(74, 32);
      this.frequencylabel.TabIndex = 44;
      this.frequencylabel.Text = "0.2Hz";
      this.Sasabihinmopagtinuroka.SetToolTip((Control) this.frequencylabel, "Frequency value in Hz");
      this.frequencytrackbar.BackColor = Color.FromArgb(50, 50, 50);
      this.frequencytrackbar.Cursor = Cursors.NoMoveHoriz;
      this.frequencytrackbar.Location = new Point(6, 26);
      this.frequencytrackbar.Maximum = 112;
      this.frequencytrackbar.Minimum = 4;
      this.frequencytrackbar.Name = "frequencytrackbar";
      this.frequencytrackbar.Size = new Size(173, 45);
      this.frequencytrackbar.TabIndex = 42;
      this.frequencytrackbar.TickStyle = TickStyle.None;
      this.Sasabihinmopagtinuroka.SetToolTip((Control) this.frequencytrackbar, "Toggle your desired Frequency");
      this.frequencytrackbar.Value = 5;
      this.frequencytrackbar.Scroll += new EventHandler(this.frequencytrackbar_Scroll);
      this.groupBox1.Controls.Add((Control) this.sawtoothcheckbox);
      this.groupBox1.Controls.Add((Control) this.sinecheckbox);
      this.groupBox1.Controls.Add((Control) this.pictureBox2);
      this.groupBox1.Controls.Add((Control) this.rectanglecheckbox);
      this.groupBox1.Controls.Add((Control) this.trianglecheckbox);
      this.groupBox1.Font = new Font("Century Gothic", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.groupBox1.ForeColor = Color.White;
      this.groupBox1.Location = new Point(9, 4);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(272, 180);
      this.groupBox1.TabIndex = 40;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Wave Selection";
      this.sawtoothcheckbox.Appearance = Appearance.Button;
      this.sawtoothcheckbox.AutoSize = true;
      this.sawtoothcheckbox.BackColor = Color.FromArgb(50, 50, 50);
      this.sawtoothcheckbox.FlatStyle = FlatStyle.Popup;
      this.sawtoothcheckbox.Font = new Font("Montserrat", 9.749999f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.sawtoothcheckbox.ForeColor = Color.White;
      this.sawtoothcheckbox.Location = new Point(7, 137);
      this.sawtoothcheckbox.Name = "sawtoothcheckbox";
      this.sawtoothcheckbox.Size = new Size(80, 26);
      this.sawtoothcheckbox.TabIndex = 31;
      this.sawtoothcheckbox.Text = "Sawtooth  ";
      this.Sasabihinmopagtinuroka.SetToolTip((Control) this.sawtoothcheckbox, "Produce a Sawtooth Wave");
      this.sawtoothcheckbox.UseVisualStyleBackColor = false;
      this.sawtoothcheckbox.CheckedChanged += new EventHandler(this.checkBox4_CheckedChanged);
      this.sinecheckbox.Appearance = Appearance.Button;
      this.sinecheckbox.AutoSize = true;
      this.sinecheckbox.BackColor = Color.FromArgb(50, 50, 50);
      this.sinecheckbox.BackgroundImageLayout = ImageLayout.Stretch;
      this.sinecheckbox.FlatStyle = FlatStyle.Popup;
      this.sinecheckbox.Font = new Font("Montserrat", 9.749999f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.sinecheckbox.ForeColor = Color.White;
      this.sinecheckbox.Location = new Point(8, 35);
      this.sinecheckbox.Name = "sinecheckbox";
      this.sinecheckbox.Size = new Size(80, 26);
      this.sinecheckbox.TabIndex = 28;
      this.sinecheckbox.Text = "Sine         ";
      this.Sasabihinmopagtinuroka.SetToolTip((Control) this.sinecheckbox, "Produce Sine Wave");
      this.sinecheckbox.UseVisualStyleBackColor = false;
      this.sinecheckbox.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
      this.pictureBox2.BackColor = Color.FromArgb(41, 53, 65);
      this.pictureBox2.Image = (Image) componentResourceManager.GetObject("pictureBox2.Image");
      this.pictureBox2.Location = new Point(95, 27);
      this.pictureBox2.Name = "pictureBox2";
      this.pictureBox2.Size = new Size(163, 141);
      this.pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
      this.pictureBox2.TabIndex = 39;
      this.pictureBox2.TabStop = false;
      this.rectanglecheckbox.Appearance = Appearance.Button;
      this.rectanglecheckbox.AutoSize = true;
      this.rectanglecheckbox.BackColor = Color.FromArgb(50, 50, 50);
      this.rectanglecheckbox.FlatStyle = FlatStyle.Popup;
      this.rectanglecheckbox.Font = new Font("Montserrat", 9.749999f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.rectanglecheckbox.ForeColor = Color.White;
      this.rectanglecheckbox.Location = new Point(7, 69);
      this.rectanglecheckbox.Name = "rectanglecheckbox";
      this.rectanglecheckbox.Size = new Size(80, 26);
      this.rectanglecheckbox.TabIndex = 29;
      this.rectanglecheckbox.Text = "Rectangle ";
      this.Sasabihinmopagtinuroka.SetToolTip((Control) this.rectanglecheckbox, "produce Rectangular Wave");
      this.rectanglecheckbox.UseVisualStyleBackColor = false;
      this.rectanglecheckbox.CheckedChanged += new EventHandler(this.checkBox2_CheckedChanged);
      this.trianglecheckbox.Appearance = Appearance.Button;
      this.trianglecheckbox.AutoSize = true;
      this.trianglecheckbox.BackColor = Color.FromArgb(50, 50, 50);
      this.trianglecheckbox.FlatStyle = FlatStyle.Popup;
      this.trianglecheckbox.Font = new Font("Montserrat", 9.749999f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.trianglecheckbox.ForeColor = Color.White;
      this.trianglecheckbox.Location = new Point(7, 102);
      this.trianglecheckbox.Name = "trianglecheckbox";
      this.trianglecheckbox.Size = new Size(82, 26);
      this.trianglecheckbox.TabIndex = 30;
      this.trianglecheckbox.Text = "Triangular  ";
      this.Sasabihinmopagtinuroka.SetToolTip((Control) this.trianglecheckbox, "Produce Triangular Wave");
      this.trianglecheckbox.UseVisualStyleBackColor = false;
      this.trianglecheckbox.CheckedChanged += new EventHandler(this.checkBox3_CheckedChanged);
      this.panel5.BackColor = Color.FromArgb(0, 49, 69);
      this.panel5.BackgroundImageLayout = ImageLayout.Stretch;
      this.panel5.Controls.Add((Control) this.label4);
      this.panel5.Location = new Point(-3, 395);
      this.panel5.Name = "panel5";
      this.panel5.Size = new Size(296, 37);
      this.panel5.TabIndex = 38;
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Century Gothic", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label4.ForeColor = SystemColors.ButtonHighlight;
      this.label4.Location = new Point(20, 6);
      this.label4.Name = "label4";
      this.label4.Size = new Size(94, 24);
      this.label4.TabIndex = 38;
      this.label4.Text = "Controls";
      this.panel4.BackColor = Color.WhiteSmoke;
      this.panel4.Controls.Add((Control) this.panel2);
      this.panel4.Dock = DockStyle.Top;
      this.panel4.Location = new Point(288, 0);
      this.panel4.Name = "panel4";
      this.panel4.Size = new Size(865, 77);
      this.panel4.TabIndex = 38;
      this.panel2.BackColor = Color.WhiteSmoke;
      this.panel2.Controls.Add((Control) this.pictureBox5);
      this.panel2.Controls.Add((Control) this.label2);
      this.panel2.Location = new Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(874, 77);
      this.panel2.TabIndex = 39;
      this.pictureBox5.Image = (Image) componentResourceManager.GetObject("pictureBox5.Image");
      this.pictureBox5.Location = new Point(15, 7);
      this.pictureBox5.Name = "pictureBox5";
      this.pictureBox5.Size = new Size(110, 65);
      this.pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
      this.pictureBox5.TabIndex = 19;
      this.pictureBox5.TabStop = false;
      this.Sasabihinmopagtinuroka.SetToolTip((Control) this.pictureBox5, "\"A mind is a terrible thing to waste\"");
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Century Gothic", 24.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.ForeColor = Color.DimGray;
      this.label2.Location = new Point(135, 25);
      this.label2.Name = "label2";
      this.label2.Size = new Size(245, 40);
      this.label2.TabIndex = 0;
      this.label2.Text = "Graph Display";
      this.Sasabihinmopagtinuroka.AutomaticDelay = 250;
      this.Sasabihinmopagtinuroka.AutoPopDelay = 5000;
      this.Sasabihinmopagtinuroka.BackColor = Color.FromArgb(50, 50, 50);
      this.Sasabihinmopagtinuroka.ForeColor = SystemColors.HighlightText;
      this.Sasabihinmopagtinuroka.InitialDelay = 250;
      this.Sasabihinmopagtinuroka.IsBalloon = true;
      this.Sasabihinmopagtinuroka.OwnerDraw = true;
      this.Sasabihinmopagtinuroka.ReshowDelay = 50;
      this.Sasabihinmopagtinuroka.ShowAlways = true;
      this.halfsecond.AutoSize = true;
      this.halfsecond.BackColor = Color.DimGray;
      this.halfsecond.BackgroundImage = (Image) componentResourceManager.GetObject("halfsecond.BackgroundImage");
      this.halfsecond.FlatAppearance.BorderColor = Color.White;
      this.halfsecond.FlatStyle = FlatStyle.Flat;
      this.halfsecond.Font = new Font("Century", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.halfsecond.ForeColor = SystemColors.ActiveCaptionText;
      this.halfsecond.Location = new Point(908, 577);
      this.halfsecond.Name = "halfsecond";
      this.halfsecond.Size = new Size(103, 20);
      this.halfsecond.TabIndex = 47;
      this.halfsecond.Text = "0.5 Sec / Div.";
      this.Sasabihinmopagtinuroka.SetToolTip((Control) this.halfsecond, "Please restart the app if you want again to modify the time / division");
      this.halfsecond.UseVisualStyleBackColor = false;
      this.halfsecond.CheckedChanged += new EventHandler(this.millisecond_CheckedChanged);
      this.tenthsecond.AutoSize = true;
      this.tenthsecond.BackColor = Color.DimGray;
      this.tenthsecond.BackgroundImage = (Image) componentResourceManager.GetObject("tenthsecond.BackgroundImage");
      this.tenthsecond.FlatAppearance.BorderColor = Color.White;
      this.tenthsecond.FlatStyle = FlatStyle.Flat;
      this.tenthsecond.Font = new Font("Century", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.tenthsecond.ForeColor = SystemColors.ActiveCaptionText;
      this.tenthsecond.Location = new Point(908, 599);
      this.tenthsecond.Name = "tenthsecond";
      this.tenthsecond.Size = new Size(103, 20);
      this.tenthsecond.TabIndex = 48;
      this.tenthsecond.Text = "0.1 Sec / Div.";
      this.Sasabihinmopagtinuroka.SetToolTip((Control) this.tenthsecond, "Please restart the app if you want again to modify the time / division");
      this.tenthsecond.UseVisualStyleBackColor = false;
      this.tenthsecond.CheckedChanged += new EventHandler(this.tenthsecond_CheckedChanged);
      this.meleeseconds.AutoSize = true;
      this.meleeseconds.BackColor = Color.DimGray;
      this.meleeseconds.BackgroundImage = (Image) componentResourceManager.GetObject("meleeseconds.BackgroundImage");
      this.meleeseconds.FlatAppearance.BorderColor = Color.White;
      this.meleeseconds.FlatStyle = FlatStyle.Flat;
      this.meleeseconds.Font = new Font("Century", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.meleeseconds.ForeColor = SystemColors.ActiveCaptionText;
      this.meleeseconds.Location = new Point(1019, 599);
      this.meleeseconds.Name = "meleeseconds";
      this.meleeseconds.Size = new Size(104, 20);
      this.meleeseconds.TabIndex = 49;
      this.meleeseconds.Text = "1 mSec / Div.";
      this.Sasabihinmopagtinuroka.SetToolTip((Control) this.meleeseconds, "Please restart the app if you want again to modify the time / division");
      this.meleeseconds.UseVisualStyleBackColor = false;
      this.meleeseconds.CheckedChanged += new EventHandler(this.meleeseconds_CheckedChanged);
      this.pictureBox7.BackColor = Color.Transparent;
      this.pictureBox7.BackgroundImage = (Image) componentResourceManager.GetObject("pictureBox7.BackgroundImage");
      this.pictureBox7.BackgroundImageLayout = ImageLayout.Zoom;
      this.pictureBox7.Location = new Point(297, 316);
      this.pictureBox7.Name = "pictureBox7";
      this.pictureBox7.Size = new Size(36, 38);
      this.pictureBox7.TabIndex = 43;
      this.pictureBox7.TabStop = false;
      this.horizontalcheckbutton.AutoSize = true;
      this.horizontalcheckbutton.BackColor = Color.DimGray;
      this.horizontalcheckbutton.BackgroundImage = (Image) componentResourceManager.GetObject("horizontalcheckbutton.BackgroundImage");
      this.horizontalcheckbutton.FlatAppearance.BorderColor = Color.White;
      this.horizontalcheckbutton.FlatAppearance.BorderSize = 3;
      this.horizontalcheckbutton.FlatStyle = FlatStyle.Flat;
      this.horizontalcheckbutton.Font = new Font("Century", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.horizontalcheckbutton.ForeColor = SystemColors.ActiveCaptionText;
      this.horizontalcheckbutton.Location = new Point(309, 599);
      this.horizontalcheckbutton.Name = "horizontalcheckbutton";
      this.horizontalcheckbutton.Size = new Size(123, 20);
      this.horizontalcheckbutton.TabIndex = 45;
      this.horizontalcheckbutton.Text = "Horizontal Zoom";
      this.horizontalcheckbutton.UseVisualStyleBackColor = false;
      this.horizontalcheckbutton.CheckedChanged += new EventHandler(this.horizontalcheckbutton_CheckedChanged);
      this.verticalcheckbox.AutoSize = true;
      this.verticalcheckbox.BackColor = Color.DimGray;
      this.verticalcheckbox.BackgroundImage = (Image) componentResourceManager.GetObject("verticalcheckbox.BackgroundImage");
      this.verticalcheckbox.FlatAppearance.BorderColor = Color.White;
      this.verticalcheckbox.FlatStyle = FlatStyle.Flat;
      this.verticalcheckbox.Font = new Font("Century", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.verticalcheckbox.ForeColor = SystemColors.ActiveCaptionText;
      this.verticalcheckbox.Location = new Point(309, 578);
      this.verticalcheckbox.Name = "verticalcheckbox";
      this.verticalcheckbox.Size = new Size(107, 20);
      this.verticalcheckbox.TabIndex = 46;
      this.verticalcheckbox.Text = "Vertical Zoom";
      this.verticalcheckbox.UseVisualStyleBackColor = false;
      this.verticalcheckbox.CheckedChanged += new EventHandler(this.verticalcheckbox_CheckedChanged);
      this.timer1.Tick += new EventHandler(this.timer1_Tick);
      this.pictureBox4.BackColor = Color.Transparent;
      this.pictureBox4.BackgroundImage = (Image) componentResourceManager.GetObject("pictureBox4.BackgroundImage");
      this.pictureBox4.BackgroundImageLayout = ImageLayout.Zoom;
      this.pictureBox4.Location = new Point(687, 569);
      this.pictureBox4.Name = "pictureBox4";
      this.pictureBox4.Size = new Size(80, 52);
      this.pictureBox4.TabIndex = 44;
      this.pictureBox4.TabStop = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.Silver;
      this.BackgroundImageLayout = ImageLayout.Stretch;
      this.ClientSize = new Size(1153, 640);
      this.Controls.Add((Control) this.meleeseconds);
      this.Controls.Add((Control) this.tenthsecond);
      this.Controls.Add((Control) this.halfsecond);
      this.Controls.Add((Control) this.verticalcheckbox);
      this.Controls.Add((Control) this.horizontalcheckbutton);
      this.Controls.Add((Control) this.pictureBox4);
      this.Controls.Add((Control) this.pictureBox7);
      this.Controls.Add((Control) this.panel4);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.richtextbox);
      this.Controls.Add((Control) this.z1);
      this.ForeColor = SystemColors.ButtonHighlight;
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (Form1);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Load += new EventHandler(this.Form1_Load);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.panel6.ResumeLayout(false);
      this.panel6.PerformLayout();
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.panel3.ResumeLayout(false);
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.dutycycletrackbar.EndInit();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.frequencytrackbar.EndInit();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      ((ISupportInitialize) this.pictureBox2).EndInit();
      this.panel5.ResumeLayout(false);
      this.panel5.PerformLayout();
      this.panel4.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      ((ISupportInitialize) this.pictureBox5).EndInit();
      ((ISupportInitialize) this.pictureBox7).EndInit();
      ((ISupportInitialize) this.pictureBox4).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private delegate void SetTextCallback(string text);

    internal class ProcessConnection
    {
      public static ConnectionOptions ProcessConnectionOptions() => new ConnectionOptions()
      {
        Impersonation = ImpersonationLevel.Impersonate,
        Authentication = AuthenticationLevel.Default,
        EnablePrivileges = true
      };

      public static ManagementScope ConnectionScope(
        string machineName,
        ConnectionOptions options,
        string path)
      {
        ManagementScope managementScope = new ManagementScope();
        managementScope.Path = new ManagementPath("\\\\" + machineName + path);
        managementScope.Options = options;
        managementScope.Connect();
        return managementScope;
      }
    }

    public class COMPortInfo
    {
      public string Name { get; set; }

      public string Description { get; set; }

      public static List<Form1.COMPortInfo> GetCOMPortsInfo()
      {
        List<Form1.COMPortInfo> comPortsInfo = new List<Form1.COMPortInfo>();
        ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(Form1.ProcessConnection.ConnectionScope(Environment.MachineName, Form1.ProcessConnection.ProcessConnectionOptions(), "\\root\\CIMV2"), new ObjectQuery("SELECT * FROM Win32_PnPEntity WHERE ConfigManagerErrorCode = 0"));
        using (managementObjectSearcher)
        {
          foreach (ManagementObject managementObject in managementObjectSearcher.Get())
          {
            if (managementObject != null)
            {
              object obj = managementObject["Caption"];
              if (obj != null)
              {
                string str = obj.ToString();
                if (str.Contains("Prolific"))
                  comPortsInfo.Add(new Form1.COMPortInfo()
                  {
                    Name = str.Substring(str.LastIndexOf("(COM")).Replace("(", string.Empty).Replace(")", string.Empty),
                    Description = str
                  });
              }
            }
          }
        }
        return comPortsInfo;
      }
    }
  }
}
