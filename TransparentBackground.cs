using System.Drawing;
using System.Windows.Forms;

namespace XXX
{
    public partial class Frm_TransparentBackground : Form
    {
        public Frm_TransparentBackground()
        {
            InitializeComponent();
            BackColor = Color.WhiteSmoke;       //  設定背景顏色
            TransparencyKey = Color.WhiteSmoke; //  該顏色變透明
        }
    }
}
