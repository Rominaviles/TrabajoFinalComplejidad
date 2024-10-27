using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace tpfinal
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            

        }
        

       /* protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Estrategia estrategia = new Estrategia();
            progressBar1.Maximum = Utils.lineCount;
            progressBar1.Step = 1;
            using (TextFieldParser parser = new TextFieldParser(@Utils.get_patron()))
            {

                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");
                string[] columns = parser.ReadFields();
                string[] fields = parser.ReadFields();
                string nombre = Utils.RemoveSpecialCharacters(fields[0]);
                string tiempo = Utils.RemoveSpecialCharacters(fields[1]);
                string prioridad = Utils.RemoveSpecialCharacters(fields[2]);
                while (!parser.EndOfData)
                {
                    fields = parser.ReadFields();
                    nombre = Utils.RemoveSpecialCharacters(fields[0]);
                    tiempo = Utils.RemoveSpecialCharacters(fields[1]);
                    prioridad = Utils.RemoveSpecialCharacters(fields[2]);
                    Backend.datos.Add(new Proceso(nombre, Int32.Parse(tiempo), Int32.Parse(prioridad)));
                    progressBar1.PerformStep();
                }
            }
            
            Form1 buscador = new Form1();
            buscador.Show();
            this.Close();
        }*/

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Estrategia estrategia = new Estrategia();
            progressBar1.Maximum = Utils.lineCount;
            progressBar1.Step = 1;
            using (var stream = File.OpenRead(Utils.get_patron()))
            using (var reader = new StreamReader(stream))
            {

                var data = Utils.Parse(reader, ';', '"');

                foreach (var line in data)
                {
                    if (line.Count >=3)
                    {
                        string nombre = Utils.RemoveSpecialCharacters(line[0]);
                        string tiempo = Utils.RemoveSpecialCharacters(line[1]);
                        string prioridad = Utils.RemoveSpecialCharacters(line[2]);
                        Backend.datos.Add(new Proceso(nombre, Int32.Parse(tiempo), Int32.Parse(prioridad)));
                        progressBar1.PerformStep();                    
                    }

                }
            }

            Form1 buscador = new Form1();
            buscador.Show();
            this.Close();
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
