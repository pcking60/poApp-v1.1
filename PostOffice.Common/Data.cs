using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostOffice.Common
{
    class Data
    {
        public void GetData()
        {
            SqlConnection con = new SqlConnection("Data Source = .; Initial Catalog = domain; Integrated Security = True");
            con.Open();
            SqlCommand cmd = new SqlCommand("Select * from Item", con);

        }
    }
}
