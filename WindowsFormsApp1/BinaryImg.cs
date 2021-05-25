using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class BinaryImg
    {
        public int ser_no { get; set; }
        public string cre_date { get; set; }
        public string cre_time { get; set; }
        public string usr_code { get; set; }
        public string per_code { get; set; }
        public byte[] img_data { get; set; }
        public string img_name { get; set; }
        public string img_size { get; set; }
    }
}
