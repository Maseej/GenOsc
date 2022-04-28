using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using ZedGraph;


namespace GenOsc_GUI
{
    public partial class Form1 : Form
    {
        // Starting time in milliseconds
       
        private SerialPort myport;
       
        

        public Form1()
        {
            InitializeComponent();
        }

        public delegate void AddDataDelegate(String myString);
        public AddDataDelegate myDelegate;

        private void Form1_Load(object sender, EventArgs e)
        {
            this.myDelegate = new AddDataDelegate(AddDataMethod);

           


        }
        
        public void connectbutton_Click(object sender, EventArgs e)
        {
            myport = new SerialPort();
            myport.PortName = comporttextbox.Text;
            myport.BaudRate = Convert.ToInt32(baudratetextbox.Text);
            if (myport.IsOpen) return;
            myport.DtrEnable = true;
            myport.RtsEnable = true;
            myport.Open();

            connectbutton.Enabled = false;
            disconnectbutton.Enabled = true;
            comporttextbox.Enabled = false;
            baudratetextbox.Enabled = false;

            myport.DataReceived += new SerialDataReceivedEventHandler(SerialPortDataReceived);
            
        }
       



   

        private void zedGraphControl1_Load(object sender, EventArgs e)
        {
           
        }


        
       

        
      
        ///codes para makapagsend ng data sa richtextbox
        ///
        public void AddDataMethod(String myString)
        {
            richtextbox.AppendText(myString);
            richtextbox.ScrollToCaret(); //Para magscrolldown ng kusa.

        }




        public void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
      {
           
            //Display sensor data
            SerialPort serialconnection = (SerialPort)sender;
            string indata = serialconnection.ReadLine();
            
           richtextbox.Invoke(this.myDelegate, new Object[] { indata + "\n" });

            

        }

      
        private void disconnectbutton_Click(object sender, EventArgs e)
        {
            if (myport.IsOpen == false) return;
            myport.Close();
            connectbutton.Enabled = true;
            disconnectbutton.Enabled = false;
            comporttextbox.Enabled = true;
            baudratetextbox.Enabled = true;

                        


        }

        private void exitbutton_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }



       
    }
}
