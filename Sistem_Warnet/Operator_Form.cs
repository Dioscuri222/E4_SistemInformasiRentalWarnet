using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sistem_Warnet
{
    public partial class Operator_Form : Form
    {
        private string connectionString = "Data Source=FASYALTP\\FASYALTP;Initial Catalog=DBWarnet;Integrated Security=True";
        private SqlConnection conn;
        public Operator_Form()
        {
            InitializeComponent();
        }

        private void Staff_Form_Load(object sender, EventArgs e)
        {

        }
    }
}
