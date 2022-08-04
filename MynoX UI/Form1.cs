using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using KrnlAPI;

namespace MynoX
{
    public partial class MynoX : Form
    {
        KrnlApi MainAPI = new KrnlApi();
        public MynoX()
        {
            InitializeComponent();
            MainAPI.Initialize();
        }

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]

            public static extern bool ReleaseCapture();

        public bool NamedPipeExist(string pipe)
        {
            return Directory.GetFiles(@"\\\\.\\pipe\\").Contains($@"\\\\.\\pipe\\{pipe}");
        }

        private void MynoX_Load(object sender, EventArgs e)
        {

        }

        private void MinimizeButton_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void TitleLabel_Click(object sender, EventArgs e)
        {

        }

        private void TitleLabel_Click_1(object sender, EventArgs e)
        {

        }

        Point lastPoint;
        private void MynoPanel_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void MynoPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            codeinput.Clear();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (Stream s = File.Open(saveFileDialog1.FileName, FileMode.CreateNew))
                using (StreamWriter sw = new StreamWriter(s))
                {
                    sw.Write(codeinput.Text);
                }
            }
        }

        private void OpenFileButton_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                openFileDialog1.Title = "Open";
                codeinput.Text = File.ReadAllText(openFileDialog1.FileName);
            }
        }

        private void Execute_Click(object sender, EventArgs e)
        {
            if (MainAPI.IsInjected())
            {
                MainAPI.Execute(codeinput.Text);
            }
            else
            {
                MessageBox.Show($"Inject {Functions.exploitdllname} before using this!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        private void AttachButton_Click(object sender, EventArgs e)
        {
            //Functions.Inject();
            MainAPI.Inject();
        }

        private void scriptlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            codeinput.Text = File.ReadAllText($"./Scripts/{ScriptList.SelectedItem}");
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            ScriptList.Items.Clear();
            Functions.PopulateListBox(ScriptList, "./Scripts", "*.txt");
            Functions.PopulateListBox(ScriptList, "./Scripts", "*.lua");
        }
        private void StatusTimer_Tick(object sender, EventArgs e)
        {
            if (MainAPI.IsInjected())
            {
                AttachButton.Text = "Attached";
                AttachButton.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                AttachButton.Text = "Attach";
                AttachButton.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void MynoPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
