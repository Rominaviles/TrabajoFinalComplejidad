using tpfinal;
using System.Runtime.InteropServices;



namespace tpfinal
{
    public partial class Inicio : Form
    {
        bool OK = false;
        public Inicio()
        {

            
            InitializeComponent();

            pathDataSet.Text = System.IO.Directory.GetCurrentDirectory(); //Utils.get_patron();

        }


        private void btnclose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
     
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Utils.get_patron();
                openFileDialog.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pathDataSet.Text = openFileDialog.FileName;
                    Utils.set_patron(pathDataSet.Text);
                    OK = true;
                }
                else
                {
                    Utils.init_patron();
                    pathDataSet.Text = Utils.get_patron();
                }
            }

           // MessageBox.Show("Titilos", "File Content at path: " + pathDataSet.Text, MessageBoxButtons.OK);
        }
        private void btnNo_Click(object sender, EventArgs e)
        {
            if (OK)
            {
                this.Hide();
                Form2 progreso = new Form2();
                progreso.Show();
            }
            else
            {
                MessageBox.Show("Seleccione un archivo con extensi�n .csv!");
            }

           

        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();


        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);


        private void barra_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

    }
}