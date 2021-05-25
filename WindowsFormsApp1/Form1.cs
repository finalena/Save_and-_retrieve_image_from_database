using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            GetData();


        }

        private void GetData()
        {
            byte[] aryByte = null;
            using (FileStream fs = new FileStream(@"C:\Users\USER\Desktop\sign.jpg", FileMode.Open))
            {
                aryByte = new byte[(int)fs.Length];
                fs.Read(aryByte, 0, aryByte.Length);
            }

            //編碼成Base64，寫入文字檔
            string strImg = Convert.ToBase64String(aryByte);
            using (StreamWriter sw = new StreamWriter(@"C: \Users\USER\Desktop\test.txt"))
            {
                sw.Write(strImg);
            }

            string strImgRead = string.Empty;
            //讀字串
            using (StreamReader sr = new StreamReader(@"C: \Users\USER\Desktop\test.txt"))
            {
                strImgRead = sr.ReadToEnd();
            }

            //轉回Base64，建立新圖檔
            using (FileStream fs = new FileStream(@"C:\Users\USER\Desktop\NewSign.jpg", FileMode.Create))
            {
                byte[] aryWrite = Convert.FromBase64String(strImgRead);
                fs.Write(aryWrite, 0, aryWrite.Length);
            }
        }
    }
}
