
using System.Windows.Forms;


namespace DevEnv
{


    public partial class frmMain : Form
    {

        public static int iImageCounter = 1;


        public frmMain()
        {
            InitializeComponent();
        }


        private void btnInstall_Click(object sender, System.EventArgs e)
        {
            ExtractFileToDirectoryTest();
        }


        public static void MsgBox(object obj)
        {
            if (obj != null)
                System.Windows.Forms.MessageBox.Show(obj.ToString());
            else
                System.Windows.Forms.MessageBox.Show("obj is NULL");
        }


        private void frmMain_Load(object sender, System.EventArgs e)
        {
            string fn = System.IO.Path.Combine(ExeDir, "info.txt");
            this.txtInfo.Text = System.IO.File.ReadAllText(fn, System.Text.Encoding.UTF8);

            this.pbInstructions.Image = System.Drawing.Image.FromFile(Clone1);
            tmChangeImage.Interval = 5000;
            tmChangeImage.Start();
        }


        public string ExeDir
        {
            get
            {
                return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            }

        }


        public string Clone1
        {
            get
            {
                return System.IO.Path.Combine(ExeDir, "clone1.png");
            }
        }

        public string Clone2
        {
            get
            {
                return System.IO.Path.Combine(ExeDir, "clone2.png");
            }
        }


        public static string GetAppsDir()
        {
            string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            dir = System.IO.Path.Combine(dir, "..");
            dir = System.IO.Path.Combine(dir, "..");
            dir = System.IO.Path.GetFullPath(dir);
            dir = System.IO.Path.Combine(dir, "Apps");

            return dir;
        } // End Function GetAppsDir 


        public static string GetBaseDir()
        {
            string dir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            dir = System.IO.Path.Combine(dir, "..");
            dir = System.IO.Path.Combine(dir, "..");
            dir = System.IO.Path.GetFullPath(dir);

            dir = System.IO.Path.Combine(dir, "Programme2");

            return dir;
        } // End Function GetBaseDir 


        public void ExtractFileToDirectoryTest()
        {
            string appsDir = GetAppsDir();
            string baseDir = GetBaseDir();

            string[] filez = System.IO.Directory.GetFiles(appsDir, "*.exe");

            using (frmProgress progressReport = new frmProgress())
            {
                System.Windows.Forms.Application.DoEvents();

                int x = this.Location.X + this.Width / 2 - progressReport.Width / 2;
                int y = this.Location.Y + this.Height / 2 - progressReport.Height / 2;

                System.Drawing.Point location = new System.Drawing.Point(x, y);
                progressReport.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                progressReport.Location = location;
                System.Windows.Forms.Application.DoEvents();

                progressReport.SetValue(0);
                progressReport.StartDisplay();
                System.Windows.Forms.Application.DoEvents();
                progressReport.Show(this);
                System.Windows.Forms.Application.DoEvents();

                for (int i = 0; i < filez.Length; ++i)
                {
                    progressReport.SetValue((i + 1) * 100.0 / (filez.Length));
                    System.Windows.Forms.Application.DoEvents();

                    string fn = System.IO.Path.GetFileNameWithoutExtension(filez[i]);
                    string extractDir = System.IO.Path.Combine(baseDir, fn);
                    if (!System.IO.Directory.Exists(extractDir))
                        System.IO.Directory.CreateDirectory(extractDir);

                    System.Windows.Forms.Application.DoEvents();
                    ExtractFileToDirectory(filez[i], extractDir);
                    System.Windows.Forms.Application.DoEvents();
                } // Next i 

                progressReport.Close();
            } // End using progo

        } // End Sub ExtractFileToDirectoryTest 


        public void ExtractFileToDirectory(string zipFileName, string outputDirectory)
        {
            Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(zipFileName);
            System.IO.Directory.CreateDirectory(outputDirectory);

            foreach (Ionic.Zip.ZipEntry e in zip)
            {
                // check if you want to extract e or not
                // if (e.FileName == "TheFileToExtract")
                e.Extract(outputDirectory, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);
            } // Next e

        } // End Sub ExtractFileToDirectory 


        protected void CreateSelfExtractZipFile()
        {
            //DAL = null;
            //DAL = GetDAL();

            string safeFileLocation = ""; // GetSaveFileLocation("Data", "");
            string ZipFileToCreate = System.IO.Path.Combine(System.IO.Directory.GetParent(safeFileLocation).FullName, new System.IO.DirectoryInfo(safeFileLocation).Name + ".zip");

            if (System.IO.File.Exists(ZipFileToCreate))
            {
                MsgBox("ZIP file already exists - please remove it and rerun, or ZIP it manually");
                return;
            } // End if (System.IO.File.Exists(ZipFileToCreate))

            string DirectoryToZip = safeFileLocation;

            try
            {

                using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
                {
                    zip.Comment = "This will be embedded into a self-extracting console-based exe";
                    //zip.StatusMessageTextWriter = System.Console.Out;
                    zip.AddDirectory(DirectoryToZip); // recurses subdirectories
                    //zip.Password = "foobar";
                    //zip.Encryption = Ionic.Zip.EncryptionAlgorithm.WinZipAes256;
                    //zip.Save(ZipFileToCreate);


                    Ionic.Zip.SelfExtractorSaveOptions options = new Ionic.Zip.SelfExtractorSaveOptions();
                    options.Flavor = Ionic.Zip.SelfExtractorFlavor.ConsoleApplication;
                    //options.DefaultExtractDirectory = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), System.IO.Directory.GetParent(ZipFileToCreate).Name);
                    options.DefaultExtractDirectory = "%TEMP%";
                    options.ExtractExistingFile = Ionic.Zip.ExtractExistingFileAction.OverwriteSilently;

                    // http://dotnetzip.codeplex.com/workitem/10682
                    //options.IconFile = System.IO.Path.Combine(Application.StartupPath, "box_software.ico");
                    //options.PostExtractCommandLine = "putty.exe";
                    //options.Quiet = true;
                    //options.RemoveUnpackedFilesAfterExecute = true;

                    zip.SaveSelfExtractor(System.IO.Path.ChangeExtension(ZipFileToCreate, ".exe"), options);
                } // End Using zip
            } // End Try
            catch (System.Exception ex)
            {
                MsgBox("Exception Zipping files: " + System.Environment.NewLine + ex.Message);
            } // End Catch

        } // End Sub CreateSelfExtractZipFile


        private void tmChangeImage_Tick(object sender, System.EventArgs e)
        {
            if (iImageCounter % 2 == 0)
                this.pbInstructions.Image = System.Drawing.Image.FromFile(Clone1);
            else
                this.pbInstructions.Image = System.Drawing.Image.FromFile(Clone2);

            iImageCounter++;
        } // End Sub tmChangeImage_Tick 


    } // End Class frmMain : Form 


} // End Namespace DevEnv
