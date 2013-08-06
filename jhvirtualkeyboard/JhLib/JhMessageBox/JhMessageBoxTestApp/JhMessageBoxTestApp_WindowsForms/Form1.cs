using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using JhLib;


namespace JhMessageBoxTestApp_WindowsForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            _myInterlocutionFacility = JhMessageBox.GetNewInstance("DesignForge", "DemoApp-WindowsForms");
            InitializeComponent();
        }

        private void OnClick_btnNotifyUser(object sender, EventArgs e)
        {
            //_myInterlocutionFacility.NotifyUser("Oh you!");
            _myInterlocutionFacility.NotifyUser(summaryText: "SummaryText",
                                                detailText: "DetailText1234567890");
        }

        private IInterlocution _myInterlocutionFacility;
    }
}
