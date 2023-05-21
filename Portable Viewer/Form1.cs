using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Portable_Viewer {
    public partial class Form1 : Form {

        string appName = "Portable Viewer";

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {  
            
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(Form1_DragEnter);
            this.DragDrop += new DragEventHandler(Form1_DragDrop);

            string[] args = Environment.GetCommandLineArgs();
            if(args.Length > 1) {
                if(args[1] == "install") {
                    Install();
                }
                else if(args[1] == "uninstall") {
                    Uninstall();
                }
                else {
                    Open(args[1]);
                } 
            }
            else{
                CheckFolder();
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void Form1_DragDrop(object sender, DragEventArgs e) {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0) {
                Open(files[0]);
            }
        }

        private void Open(string path) {
            this.Text = "Opening...";
            try
            {
                if(!System.IO.File.Exists(path))
                    throw new Exception("File not found.");
                    
                string extension = System.IO.Path.GetExtension(path).ToLower();

                PM pm = null;
                switch (extension) {
                    case ".pgm":
                        pm = new PGM(path);
                        break; 
                    case ".pbm":
                        pm = new PBM(path);
                        break;
                    case ".ppm":
                        pm = new PPM(path);
                        break;
                    default:
                        throw new Exception("Not supported file format."); 
                }
                 

                Thread t = new Thread(() => {
                    pm.Load(); 
                    picturebox.BeginInvoke((MethodInvoker)delegate() {
                        picturebox.Image = pm.Image; 
                        this.Text = System.IO.Path.GetFileName(path) + " - " + appName;
                    });
                });

                t.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }
    
        /**
         * Bu progrma normalde Program Files\Atiksoftware\Portable Viewer klasöründe çalışır. 
         * Eğer başka bir klasörde çalışıyorsa, bu dosyayı olması gereken klasöre taşır.
         */
        private void CheckFolder(){
            string currentPath = Application.ExecutablePath;
            string currentDirectoryPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

            string roamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); 
            string homeDirectoryPath = System.IO.Path.Combine(roamingPath, "Atiksoftware\\Portable Viewer"); 
            string homePath = System.IO.Path.Combine(homeDirectoryPath, Application.ProductName + ".exe");

            if(currentDirectoryPath == homeDirectoryPath){ 
                MessageBox.Show("Drag and Drop support enabled.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // ask for do you want to install this program to Program Files\Atiksoftware\Portable Viewer and bind .pgm, .pbm, .ppm file extensions to this program.
            if(MessageBox.Show("Do you want to install this program to Program Files\\Atiksoftware\\Portable Viewer and bind .pgm, .pbm, .ppm file extensions to this program?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes){
                // run me as administrator with install parameter
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.FileName = Application.ExecutablePath;
                startInfo.Arguments = "install";
                startInfo.Verb = "runas";
                System.Diagnostics.Process.Start(startInfo);
                Application.Exit();
            } 
        }

        private void Install(){
            string currentPath = Application.ExecutablePath;
            string currentDirectoryPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

            string roamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); 
            string homeDirectoryPath = System.IO.Path.Combine(roamingPath, "Atiksoftware\\Portable Viewer"); 
            string homePath = System.IO.Path.Combine(homeDirectoryPath, Application.ProductName + ".exe");

            if(currentDirectoryPath == homeDirectoryPath){ 
                MessageBox.Show("Already installed.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if(!System.IO.Directory.Exists(homeDirectoryPath))
                System.IO.Directory.CreateDirectory(homeDirectoryPath);
                
            System.IO.File.Copy(currentPath, homePath, true);

            // register file extensions
            // .pgm, .pbm, .ppm dosyalarının iconları ve açılış programları için gerekli kayıtlar
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(".pgm");
            key.SetValue("", "PortableGrayMap");
            key = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(".pbm");
            key.SetValue("", "PortableBitmap");
            key = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(".ppm");
            key.SetValue("", "PortablePixmap");

            key = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("PortableGrayMap");
            key.SetValue("", "Portable Gray Map");
            key = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("PortableBitmap");
            key.SetValue("", "Portable Bitmap");
            key = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("PortablePixmap");
            key.SetValue("", "Portable Pixmap");

            key = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("PortableGrayMap\\shell\\open\\command");
            key.SetValue("", "\"" + homePath + "\" \"%1\"");
            key = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("PortableBitmap\\shell\\open\\command");
            key.SetValue("", "\"" + homePath + "\" \"%1\"");
            key = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("PortablePixmap\\shell\\open\\command");
            key.SetValue("", "\"" + homePath + "\" \"%1\"");

            key = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("PortableGrayMap\\DefaultIcon");
            key.SetValue("", homePath + ",0");
            key = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("PortableBitmap\\DefaultIcon");
            key.SetValue("", homePath + ",0");
            key = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("PortablePixmap\\DefaultIcon");
            key.SetValue("", homePath + ",0");

            MessageBox.Show("Installation completed.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Uninstall(){ 
            string currentPath = Application.ExecutablePath;
            string currentDirectoryPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

            string roamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); 
            string homeDirectoryPath = System.IO.Path.Combine(roamingPath, "Atiksoftware\\Portable Viewer"); 
            string homePath = System.IO.Path.Combine(homeDirectoryPath, Application.ProductName + ".exe");

            if(currentDirectoryPath != homeDirectoryPath){ 
                MessageBox.Show("Not installed.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // unregister file extensions
            Microsoft.Win32.Registry.ClassesRoot.DeleteSubKeyTree(".pgm");
            Microsoft.Win32.Registry.ClassesRoot.DeleteSubKeyTree(".pbm");
            Microsoft.Win32.Registry.ClassesRoot.DeleteSubKeyTree(".ppm");

            Microsoft.Win32.Registry.ClassesRoot.DeleteSubKeyTree("PortableGrayMap");
            Microsoft.Win32.Registry.ClassesRoot.DeleteSubKeyTree("PortableBitmap");
            Microsoft.Win32.Registry.ClassesRoot.DeleteSubKeyTree("PortablePixmap");

            System.IO.File.Delete(homePath);
            System.IO.Directory.Delete(homeDirectoryPath);

            MessageBox.Show("Uninstallation completed.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
