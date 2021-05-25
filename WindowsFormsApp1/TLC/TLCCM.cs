using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Data;

namespace TLC
{
    public class CM
    {
        public enum MBB { OK = 0 , OKCancel = 1 , AbortRetryIgnore = 2 , YesNoCancel = 3 , YesNo = 4 , RetryCancel = 5 }; //MessageBoxButtons
        public enum MBI { None = 0 , Error = 16 , Hand = 16 , Stop = 16 , Question = 32 , Exclamation = 48 , Warning = 48 , Information = 64 , Asterisk = 64 }; //MessageBoxIcon
        public enum MBDB { Button1 = 0 , Button2 = 256 , Button3 = 512 }; //MessageBoxDefaultButton
    }

    public class MsgBox
    {
        public static DialogResult Show ( string sMsgText , string sMsgTitle = "訊息" ,
                                          TLC.CM.MBB mBtn = TLC.CM.MBB.OK ,
                                          TLC.CM.MBI mIcon = TLC.CM.MBI.Information,
                                          TLC.CM.MBDB mBtnD = CM.MBDB.Button1)
        {
            DialogResult result = new DialogResult();
            MessageBoxButtons Btn = (MessageBoxButtons)mBtn;
            MessageBoxIcon Icon = (MessageBoxIcon)mIcon;
            MessageBoxDefaultButton BtnD = (MessageBoxDefaultButton)mBtnD;
            result = MessageBox.Show(sMsgText , sMsgTitle , Btn , Icon,BtnD);
            return result;
        }
    }
}
