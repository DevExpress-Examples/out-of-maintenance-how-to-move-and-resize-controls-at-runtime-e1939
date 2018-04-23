using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;
using DevExpress.XtraGrid;

namespace WindowsApplication1
{
    public partial class Form1 : Form
    {
     
                private DataTable CreateTable(int RowCount)
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add("Name", typeof(string));
            tbl.Columns.Add("ID", typeof(int));
            tbl.Columns.Add("Number", typeof(int));
            tbl.Columns.Add("Date", typeof(DateTime));
            for (int i = 0; i < RowCount; i++)
                tbl.Rows.Add(new object[] { String.Format("Name{0}", i), i, 3 - i, DateTime.Now.AddDays(i) });
            return tbl;
        }
        
        

        public Form1()
        {
            InitializeComponent();
            gridControl1.DataSource = CreateTable(20);
        }

        private void checkButton1_CheckedChanged(object sender, EventArgs e)
        {
            customizator1.IsCustomization = checkButton1.Checked;
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkButton1.Checked = false;
            switch (radioGroup1.SelectedIndex)
            {
                case 0: customizator1.SelectedControl = gridControl1;break;
                case 1: customizator1.SelectedControl = radioGroup1;break;
            }
        }
    }
}