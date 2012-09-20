namespace delayclose
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Net;
    using System.Runtime.ExceptionServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows.Forms;
    using System.Security;

    public class Form1 : Form
    {

        #region Fields
        private bool abort;
        //private List<string> adjectives;
        //private List<string> adverbs;
        private int attempts;
        private readonly string BaseCaptchaURL;
        private string board;
        private readonly List<string> boardlist;
        private Button buttonAbort;
        private Button buttonClearCap;
        private Button buttonDownload;
        private Button buttonGO;
        private Button buttonLoadProxies;
        private Button buttonPic;
        private Button buttonPost;
        private Button buttonPurge;
        private Button buttonRemove;
        private Button buttonSaveProxies;
        private readonly string CaptchaImagesDirectory;
        private readonly List<CCaptcha> captchas;
        private int currenttable;
        private CheckBox checkBox50;
        private CheckBox checkBoxCheck;
        private CheckBox checkBoxDot;
        private CheckBox checkBoxImage;
        private CheckBox checkBoxQuote;
        private CheckBox checkBoxRandImg;
        private CheckBox checkBoxSage;
        private IContainer components;
        private Bitmap DelayBitmap;
        private readonly string DelayImagesDirectory;
        private bool image_limit;
        private string[] images;
        //private List<string> imurge;
        private readonly ImageCodecInfo jpg;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label7;
        private Label labelAttempts;
        private Label labelCaptchaCount;
        private Label labelCaptchaError;
        private Label labelName;
        private Label labelPosts;
        private Label labelProxyCount;
        private Label labelTimer;
        //private List<string> nouns;
        private OpenFileDialog openFileDialogPicture;
        private OpenFileDialog openFileDialogProxy;
        private readonly EncoderParameters par;
        private uint pic_rand;
        private List<CPic> pics;
        private PictureBox pictureBox;
        private PictureBox pictureBoxCaptcha;
        private PictureBox pictureBoxKeion;
        private PictureBox pictureBoxLogo;
        private List<string> postlist;
        private int posts;
        private int[] proxycounter;
        private List<CProxy>[] proxytable;
        private string SaveCaptchasFile;
        private SaveFileDialog saveFileDialog1;
        private TextBox status;
        private TextBox textBoxCaptcha;
        private TextBox textBoxMessage;
        private TextBox textBoxName;
        private TextBox textBoxQuote;
        private TextBox textBoxRemove;
        private TextBox textBoxThread;
        private TextBox textBoxTimer;
        private string thread;
        private ToolTip toolTip1;
        private CheckBox checkBoxNew;
        private List<CProxy> triedproxies;
        //private List<string> verbs;
        [SuppressUnmanagedCodeSecurity, DllImport("MSVCR100.dll", CallingConvention=CallingConvention.Cdecl, SetLastError=true)]
        internal static extern void exit(int i);

        [SuppressUnmanagedCodeSecurity, DllImport("MSVCR100.dll", CallingConvention=CallingConvention.Cdecl, SetLastError=true)]
        internal static extern int rand();
 
        [SuppressUnmanagedCodeSecurity, DllImport("MSVCR100.dll", CallingConvention=CallingConvention.Cdecl, SetLastError=true)]
        internal static extern void srand(uint i);


        #endregion

        #region Contructors/Destructors

        public Form1()
        {
            string item = null;
            StreamReader reader = null;
            CCaptcha captcha = null;
            string[] strArray = null;
            string str3 = null;
            StreamReader reader2 = null;
            StreamReader reader3 = null;
            ImageCodecInfo info = null;
            ImageCodecInfo[] imageEncoders = null;
            try
            {
                InitializeComponent();
                ComponentResourceManager manager = new ComponentResourceManager(typeof(Form1));
                Bitmap bitmap = (Bitmap)manager.GetObject("Default");
                DelayBitmap = bitmap;
                pictureBox.Image = bitmap;
                int abc = new Random().Next(5);
                pictureBoxLogo.Image = (Bitmap)manager.GetObject("Logo");
                switch (abc + 1)
                {
                    case 1:
                        pictureBoxKeion.Image = (Bitmap)manager.GetObject("mio");
                        goto Label_01BD;

                    case 2:
                        pictureBoxKeion.Image = (Bitmap)manager.GetObject("mugi");
                        goto Label_01BD;

                    case 3:
                        pictureBoxKeion.Image = (Bitmap)manager.GetObject("ritsu");
                        goto Label_01BD;

                    case 4:
                        pictureBoxKeion.Image = (Bitmap)manager.GetObject("azumeow");
                        goto Label_01BD;

                    case 5:
                        pictureBoxKeion.Image = (Bitmap)manager.GetObject("yui");
                        goto Label_01BD;

                    default:
                        goto Label_01BD;
                }
                if (DateTime.Today.Day == 0x1b)
                {
                    pictureBoxKeion.Image = (Bitmap)manager.GetObject("yui");
                }
            Label_01BD:
                status.AppendText("Initializing JPEG image codec... ");
                try
                {
                    bool flag3 = false;
                    imageEncoders = ImageCodecInfo.GetImageEncoders();
                    int index = 0;
                    while (true)
                    {
                        if (index >= imageEncoders.Length)
                        {
                            break;
                        }
                        info = imageEncoders[index];
                        if (info.MimeType == "image/jpeg")
                        {
                            jpg = info;
                            flag3 = true;
                            goto Label_0230;
                        }
                        index++;
                    }
                    if (!flag3)
                    {
                        throw new ArgumentException();
                    }
                }
                catch (Exception)
                {
                    status.AppendText("failed, the program may behave unexpectedly. \r\n");
                }
            Label_0230:
                status.AppendText("success. \r\n");
                try
                {
                    string[] files = Directory.GetFiles(Application.StartupPath + @"\Images");
                    images = files;
                    status.AppendText(files.Length + " Images loaded.\r\n");
                }
                catch (Exception)
                {
                    MessageBox.Show("Failed to load \"Images\" Folder, make sure that it exists and is filled with images.", "Error 32873", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    exit(1);
                }
                boardlist = new List<string>();
                postlist = new List<string>();
                try
                {
                    reader3 = new StreamReader(Application.StartupPath + @"\tripcode.txt");
                    textBoxName.Text = reader3.ReadLine();
                    reader3.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Failed to load the tripcode, make sure there is a tripcode.txt in the same folder as the applicaion.", "Error 32873", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    exit(1);
                }
                try
                {
                    reader = new StreamReader(Application.StartupPath + @"\boardlist.txt");
                    while (true)
                    {
                        item = reader.ReadLine();
                        if (item == null)
                        {
                            break;
                        }
                        try
                        {
                            bool flag2 = false;
                            List<string>.Enumerator enumerator2 = boardlist.GetEnumerator();
                            while (enumerator2.MoveNext())
                            {
                                if (enumerator2.Current == item)
                                {
                                    flag2 = true;
                                }
                            }
                            if (!flag2)
                            {
                                boardlist.Add(item);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                    reader.Close();
                    reader = new StreamReader(Application.StartupPath + @"\postlist.txt");
                    while (true)
                    {
                        item = reader.ReadLine();
                        if (item == null)
                        {
                            break;
                        }
                        try
                        {
                            bool flag = false;
                            List<string>.Enumerator enumerator = postlist.GetEnumerator();
                            while (enumerator.MoveNext())
                            {
                                if (enumerator.Current == item)
                                {
                                    flag = true;
                                }
                            }
                            if (!flag)
                            {
                                postlist.Add(item);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                    reader.Close();
                    status.AppendText(boardlist.Count + " boards total loaded.\r\n");
                    status.AppendText(postlist.Count + " posts total loaded.\r\n");
                }
                catch (Exception)
                {
                    MessageBox.Show("Failed to load the lists, make sure there is a boardlist.txt and postlist.txt in the same folder as the applicaion.", "Error 32873", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    exit(1);
                }
                par = new EncoderParameters();
                par.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 10L);
                SaveCaptchasFile = Application.CommonAppDataPath + @"\saved_captchas.txt";
                string path = (Path.GetTempPath() + @"delayclose\") + DateTime.Now.Ticks.ToString("x") + @"\";
                CaptchaImagesDirectory = path;
                DelayImagesDirectory = path;
                Directory.CreateDirectory(path);
                BaseCaptchaURL = "http://www.google.com/recaptcha/api/noscript?k=6Ldp2bsSAAAAAAJ5uyx_lx34lJeEpTLVkP5k04qc";
                captchas = new List<CCaptcha>();
                pics = new List<CPic>();
                triedproxies = new List<CProxy>();
                try
                {
                    reader2 = new StreamReader(SaveCaptchasFile);
                    while (true)
                    {
                        str3 = reader2.ReadLine();
                        if (str3 == null)
                        {
                            break;
                        }
                        try
                        {
                            char[] separator = new char[] { ' ' };
                            strArray = str3.Split(separator);
                            captcha = new CCaptcha(strArray[0], long.Parse(strArray[1]));
                            if (captcha.isValid2())
                            {
                                captchas.Add(captcha);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                    reader2.Close();
                }
                catch (Exception)
                {
                }
                try
                {
                    new FileInfo(SaveCaptchasFile).Delete();
                }
                catch (Exception)
                {
                }
                labelCaptchaCount.Text = captchas.Count.ToString();
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += LoadCaptchaPics;
                worker.ProgressChanged += LoadCaptchaPicsProgressChanged;
                worker.WorkerReportsProgress = true;
                worker.RunWorkerAsync();
                try
                {
                    status.AppendText("Initializing proxy table with " + 5 + " rows... ");
                    proxytable = new List<CProxy>[5];
                    proxycounter = new int[5];
                    for (int i = 0; i < 5; i++)
                    {
                        proxycounter[i] = 0;
                        proxytable[i] = new List<CProxy>();
                    }
                }
                catch (Exception)
                {
                    exit(1);
                }
                status.AppendText(" done\r\n");
                labelProxyCount.Text = "0";
                labelPosts.Text = "0";
                labelAttempts.Text = "0";
                status.AppendText("Instructions:\r\n");
                status.AppendText("1) Enter the target thread URL at the top\r\n");
                status.AppendText("2) Load in text file(s) with proxies\r\n");
                status.AppendText("3) Fill out captchas.  (PROTIP: only type the important word.)  ");
                status.AppendText("4) Enter desired delay between posts (0 = post as fast as possible)\r\n");
                status.AppendText("5) Push butans.\r\n");
                ServicePointManager.Expect100Continue = false;
                ServicePointManager.SetTcpKeepAlive(true, 0x3a98, 0x3e8);
            }
            catch
            {
                base.Dispose(true);
            }
        }

        private void Forme1()
        {
            IContainer components = this.components;
            if (components != null)
            {
                components.Dispose();
            }
            try
            {
                PictureBox pictureBoxCaptcha = this.pictureBoxCaptcha;
                IDisposable disposable = pictureBoxCaptcha;
                if (pictureBoxCaptcha != null)
                {
                    pictureBoxCaptcha.Dispose();
                }
                Directory.Delete(CaptchaImagesDirectory, true);
            }
            catch (Exception)
            {
            }
            try
            {
                StreamWriter writer = new StreamWriter(SaveCaptchasFile, true);
                List<CCaptcha>.Enumerator enumerator = captchas.GetEnumerator();
                while (true)
                {
                    if (!enumerator.MoveNext())
                    {
                        break;
                    }
                    CCaptcha current = enumerator.Current;
                    writer.WriteLine(current.val + " " + current.expiretime.Ticks);
                }
                writer.Close();
            }
            catch (Exception)
            {
            }
        }


        #endregion
    
        #region Events

        private void buttonAbort_Click(object sender, EventArgs e)
        {
            abort = true;
            DisplayButtons(true);
            status.AppendText("\r\n**Stopped**\r\n\r\n");
        }

        private void buttonClearCap_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Clear Captchas", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                captchas.Clear();
                labelCaptchaCount.Text = "0";
                status.AppendText("Captchas cleared... \r\n");
            }
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            DisplayButtons(false);
            BackgroundWorker worker = new BackgroundWorker {
                WorkerReportsProgress = true
            };
            worker.DoWork += bwLeechPosts_DoWork;
            worker.ProgressChanged += bwSpamPost_ProgressChanged;
            worker.RunWorkerCompleted += bwLeechPosts_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private void buttonGO_Click(object sender, EventArgs e)
        {
            if (ParseURL() == -1)
            {
                status.AppendText("Invalid thread, fix and restart.\r\n");
            }
            else if (proxies_count() == 0)
            {
                status.AppendText("Add some proxies\r\n");
            }
            else
            {
                int num3;
                try
                {
                    num3 = Convert.ToInt32(textBoxTimer.Text);
                    if (num3 < 0)
                    {
                        throw new ArgumentException();
                    }
                }
                catch (Exception)
                {
                    status.AppendText("Enter a non-negative number into the timer.\n");
                    return;
                }
                PruneCaptchas();
                DisplayButtons(false);
                abort = false;
                image_limit = false;
                posts = 0;
                attempts = 0;
                labelAttempts.Text = "0";
                labelPosts.Text = "0";
                if (num3 == 0)
                {
                    int index = 0;
                    do
                    {
                        int num2;
                        if (proxytable[index].Count != 0)
                        {
                            num2 = index + 1;
                            status.AppendText("Starting thread " + num2 + "\n");
                            BackgroundWorker worker2 = new BackgroundWorker {
                                WorkerReportsProgress = true
                            };
                            worker2.DoWork += bwSpamPost_DoWork;
                            worker2.ProgressChanged += bwSpamPost_ProgressChanged;
                            worker2.RunWorkerCompleted += bwSpamPost_RunWorkerCompleted;
                            worker2.RunWorkerAsync(index);
                        }
                        else
                        {
                            num2 = index + 1;
                            status.AppendText("Row " + num2 + " has no proxies\n");
                        }
                        index = num2;
                    }
                    while (index < 5);
                }
                else
                {
                    BackgroundWorker worker = new BackgroundWorker {
                        WorkerReportsProgress = true
                    };
                    worker.DoWork += bwTimedSpamPost_DoWork;
                    worker.ProgressChanged += bwSpamPost_ProgressChanged;
                    worker.RunWorkerCompleted += bwSpamPost_RunWorkerCompleted;
                    worker.RunWorkerAsync(num3);
                }
            }
        }

        private void buttonLoadProxies_Click(object sender, EventArgs e)
        {
            if (openFileDialogProxy.ShowDialog() != DialogResult.Cancel)
            {
                string fileName = openFileDialogProxy.FileName;
                status.AppendText("Loading " + Path.GetFileName(fileName) + "...");
                int num = 0;
                try
                {
                    string str;
                    StreamReader reader = new StreamReader(fileName);
                Label_004F:
                    str = reader.ReadLine();
                    if (str != null)
                    {
                        try
                        {
                            char[] separator = new char[] { ':' };
                            string[] strArray = str.Split((string[]) null, StringSplitOptions.RemoveEmptyEntries)[0].Split(separator);
                            CProxy item = new CProxy();
                            if (strArray.Length == 2)
                            {
                                item.ip = strArray[0];
                                item.port = int.Parse(strArray[1]);
                                bool flag = false;
                                List<CProxy>.Enumerator enumerator = triedproxies.GetEnumerator();
                                while (enumerator.MoveNext())
                                {
                                    CProxy current = enumerator.Current;
                                    if ((item.ip == current.ip) && (item.port == current.port))
                                    {
                                        flag = true;
                                    }
                                }
                                if (!flag)
                                {
                                    triedproxies.Add(item);
                                    BackgroundWorker worker = new BackgroundWorker {
                                        WorkerReportsProgress = true
                                    };
                                    worker.DoWork += testProxy_DoWork;
                                    worker.ProgressChanged += testProxy_ProgressChanged;
                                    worker.RunWorkerAsync(item);
                                    num++;
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                        goto Label_004F;
                    }
                    reader.Close();
                }
                catch (Exception)
                {
                }
                status.AppendText(num + " new proxies found\r\n");
            }
        }

        private void buttonPic_Click(object sender, EventArgs e)
        {
            if (openFileDialogPicture.ShowDialog() != DialogResult.Cancel)
            {
                string fileName = openFileDialogPicture.FileName;
                status.AppendText("Loading " + Path.GetFileName(fileName) + "...");
                try
                {
                    DelayBitmap.Dispose();
                    Bitmap bitmap = new Bitmap(openFileDialogPicture.FileName);
                    DelayBitmap = bitmap;
                    pictureBox.Image = bitmap;
                }
                catch (Exception)
                {
                    status.AppendText(" failed.\r\n");
                    checkBoxImage.Checked = false;
                    checkBoxImage.Enabled = false;
                    return;
                }
                checkBoxImage.Enabled = true;
                status.AppendText(" success.\r\n");
            }
        }

        private void buttonPost_Click(object sender, EventArgs e)
        {
            if (ParseURL() == -1)
            {
                status.AppendText("Invalid thread/board, fix and restart.\r\n");
            }
            else if (proxies_count() == 0)
            {
                status.AppendText("Add some proxies\r\n");
            }
            else if (captchas.Count == 0)
            {
                status.AppendText("No captchas.\r\n");
            }
            else
            {
                PruneCaptchas();
                checkBoxDot.Checked = false;
                checkBoxRandImg.Checked = false;
                DisplayButtons(false);
                abort = false;
                image_limit = false;
                labelAttempts.Text = "0";
                labelPosts.Text = "0";
                bool flag = false;
                List<CProxy>[] proxytable = this.proxytable;
                int index = 0;
                if (0 < proxytable.Length)
                {
                    do
                    {
                        List<CProxy> list = proxytable[index];
                        int num = 0;
                        if (0 < list.Count)
                        {
                            do
                            {
                                if (DateTime.Now > list[num].tm)
                                {
                                    CProxy local1 = list[num];
                                    flag = true;
                                    break;
                                }
                                num++;
                            }
                            while (num < list.Count);
                        }
                        index++;
                    }
                    while (index < proxytable.Length);
                    if (flag)
                    {
                        BackgroundWorker worker = new BackgroundWorker {
                            WorkerReportsProgress = true
                        };
                        worker.DoWork += bwPost_DoWork;
                        worker.ProgressChanged += bwSpamPost_ProgressChanged;
                        worker.RunWorkerCompleted += bwPost_RunWorkerCompleted;
                        worker.RunWorkerAsync();
                        return;
                    }
                }
                status.AppendText("No proxies available.\r\n");
            }
        }

        private void buttonPurge_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Clear Proxies", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                List<CProxy>[] proxytable = this.proxytable;
                int index = 0;
                if (0 < proxytable.Length)
                {
                    do
                    {
                        proxytable[index].Clear();
                        index++;
                    }
                    while (index < proxytable.Length);
                }
                triedproxies.Clear();
                labelProxyCount.Text = proxies_count().ToString();
                status.AppendText("Proxy list cleared... \r\n");
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (textBoxRemove.Text == "")
            {
                status.AppendText("Enter a proxy to remove\r\n");
            }
            else
            {
                CProxy proxy = new CProxy();
                string text = textBoxRemove.Text;
                char[] separator = new char[] { ':' };
                string[] strArray = text.Split((string[]) null, StringSplitOptions.RemoveEmptyEntries)[0].Split(separator);
                if (strArray.Length != 2)
                {
                    status.AppendText("Bad proxy format\r\n");
                }
                else
                {
                    proxy.ip = strArray[0];
                    int num3 = int.Parse(strArray[1]);
                    proxy.port = num3;
                    bool flag = false;
                    List<CProxy>[] proxytable = this.proxytable;
                    int index = 0;
                    if (0 < proxytable.Length)
                    {
                        do
                        {
                            List<CProxy> list = proxytable[index];
                            int num = 0;
                            if (0 < list.Count)
                            {
                                string ip = proxy.ip;
                                do
                                {
                                    if ((list[num].ip == ip) && (list[num].port == num3))
                                    {
                                        list.RemoveAt(num);
                                        labelProxyCount.Text = proxies_count().ToString();
                                        flag = true;
                                        break;
                                    }
                                    num++;
                                }
                                while (num < list.Count);
                            }
                            index++;
                        }
                        while (index < proxytable.Length);
                        if (flag)
                        {
                            status.AppendText(text + " has been removed\r\n");
                            return;
                        }
                    }
                    status.AppendText("No such proxy\r\n");
                }
            }
        }

        private void buttonSaveProxies_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() != DialogResult.Cancel)
            {
                string fileName = saveFileDialog1.FileName;
                status.AppendText("Saving good proxies to " + Path.GetFileName(fileName) + "...");
                try
                {
                    StreamWriter writer = new StreamWriter(fileName, false);
                    List<CProxy>[] proxytable = this.proxytable;
                    int index = 0;
                    while (true)
                    {
                        if (index >= proxytable.Length)
                        {
                            break;
                        }
                        List<CProxy>.Enumerator enumerator = proxytable[index].GetEnumerator();
                        while (true)
                        {
                            if (!enumerator.MoveNext())
                            {
                                break;
                            }
                            CProxy current = enumerator.Current;
                            writer.WriteLine(current.ip + ":" + current.port);
                        }
                        index++;
                    }
                    writer.Close();
                    status.AppendText("Done.\r\nSaved to " + fileName + Environment.NewLine);
                }
                catch (Exception)
                {
                    status.AppendText("Failed.\r\nFile error.\r\n");
                }
            }
        }

        private void textBoxCaptcha_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == '\r'))
            {
                e.Handled = true;
                if (pics.Count > 0)
                {
                    string fn = pics[0].fn;
                    string text = textBoxCaptcha.Text;
                    bool flag = pics[0].isValid();
                    DateTime expiretime = pics[0].expiretime;
                    pics.RemoveAt(0);
                    if (pics.Count > 0)
                    {
                        do
                        {
                            if (pics[0].isValid())
                            {
                                break;
                            }
                            pics.RemoveAt(0);
                        }
                        while (pics.Count > 0);
                    }
                    if (pics.Count > 0)
                    {
                        pictureBoxCaptcha.LoadAsync(CaptchaImagesDirectory + pics[0].fn + ".jpg");
                    }
                    if (flag)
                    {
                        string s = ((BaseCaptchaURL + "&recaptcha_challenge_field=" + fn) + "&recaptcha_response_field=" + text).Replace(' ', '+');
                        BackgroundWorker worker2 = new BackgroundWorker();
                        worker2.RunWorkerCompleted += SubmitCaptchaAsyncCompleted;
                        worker2.DoWork += SubmitCaptchaAsyncDoWork;
                        worker2.RunWorkerAsync(new CSubmitCaptchaData(s, expiretime));
                    }
                    else
                    {
                        pics.Clear();
                    }
                }
                textBoxCaptcha.Text = "";
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += LoadCaptchaPics;
                worker.ProgressChanged += LoadCaptchaPicsProgressChanged;
                worker.WorkerReportsProgress = true;
                worker.RunWorkerAsync();
            }
        }
        #endregion

        #region BackGroundWorkers
        private void bwLeechPosts_DoWork(object sender, DoWorkEventArgs e)
        {
            string str3;
            BackgroundWorker worker = (BackgroundWorker) sender;
            string input = "";
            List<string>.Enumerator enumerator = boardlist.GetEnumerator();
        Label_001B:
            if (enumerator.MoveNext())
            {
                string current = enumerator.Current;
                status.AppendText("Scraping /" + current + "/...\r\n");
                try
                {
                    WebClient client = new WebClient();
                    int num = 0;
                    while (true)
                    {
                        if (num > 9)
                        {
                            break;
                        }
                        int num3 = num;
                        input = input + client.DownloadString(("http://boards.4chan.org/" + current) + "/" + num3);
                        status.AppendText("\t Page " + num + " ...\r\n");
                        num++;
                    }
                    if (input.IndexOf("action=\"https://sys.4chan.org/") == -1)
                    {
                        throw new ArgumentException();
                    }
                    goto Label_001B;
                }
                catch (Exception)
                {
                    worker.ReportProgress(0, "Download of Posts failed.\r\n");
                    return;
                }
            }
            worker.ReportProgress(0, "Finished Scraping.\r\n");
            Regex regex = new Regex("<blockquote.*?>.*?<.blockquote>");
            string str = "";
            foreach (Match match in regex.Matches(input))
            {
                str = str + (match.Value + Environment.NewLine);
            }
            str = str.Replace("&quot;", "\"").Replace("/&amp;", "&").Replace("&#44;", ",").Replace("&lt;", "<").Replace("&gt;", ">").Replace("<blockquote></blockquote>\r\n", "");
            str = new Regex(@">>\d+").Replace(str, "");
            str = new Regex("^(.){0,35}\r\n").Replace(str, "");
            worker.ReportProgress(0, "Reloading post list... ");
            StreamWriter writer = new StreamWriter("postlist.txt", false);
            writer.Write(str);
            writer.Close();
            postlist.Clear();
            StreamReader reader = new StreamReader(Application.StartupPath + @"\postlist.txt");
        Label_0242:
            str3 = reader.ReadLine();
            if (str3 != null)
            {
                try
                {
                    bool flag = false;
                    List<string>.Enumerator enumerator2 = postlist.GetEnumerator();
                    while (enumerator2.MoveNext())
                    {
                        if (enumerator2.Current == str3)
                        {
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        postlist.Add(str3);
                    }
                    goto Label_0242;
                }
                catch (Exception)
                {
                    goto Label_0242;
                }
            }
            reader.Close();
        }

        private void bwLeechPosts_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DisplayButtons(true);
            status.AppendText(postlist.Count + " posts total loaded.\r\n");
        }

        private void bwPost_DoWork(object sender, DoWorkEventArgs e)
        {
            CProxy userState = null;
            BackgroundWorker worker = (BackgroundWorker) sender;
            CPostInfo pi = new CPostInfo();
            srand((uint) ((int) DateTime.Now.Ticks));
        Label_0024:
            if (proxies_count() == 0)
            {
                worker.ReportProgress(0, "No more proxies, stopping\r\n");
                abort = true;
                return;
            }
            if (abort)
            {
                return;
            }
            bool flag = false;
            List<CProxy>[] proxytable = this.proxytable;
            int index = 0;
            if (0 >= proxytable.Length)
            {
                goto Label_0659;
            }
            do
            {
                List<CProxy> list = proxytable[index];
                int num = 0;
                if (0 < list.Count)
                {
                    do
                    {
                        if (DateTime.Now > list[num].tm)
                        {
                            userState = list[num];
                            flag = true;
                            break;
                        }
                        num++;
                    }
                    while (num < list.Count);
                }
                index++;
            }
            while (index < proxytable.Length);
            if (!flag)
            {
                goto Label_0659;
            }
            srand((uint) ((int) DateTime.Now.Ticks));
            pi.prx = userState;
            if (captchas.Count == 0)
            {
                worker.ReportProgress(1);
                return;
            }
            worker.ReportProgress(0, (userState.ip + ":") + userState.port + " posting\r\n");
            pi.submiturl = "https://sys.4chan.org/" + board + "/post";
            NameValueCollection  values = new NameValueCollection();
            pi.formdata = values;
            string text = textBoxMessage.Text;
            values.Add("com", text);
            if (checkBoxNew.Checked)
            {
                values.Add("resto", "new");
            }
            else { values.Add("resto", thread);}
            values.Add("name", textBoxName.Text);
            if (checkBoxSage.Checked)
            {
                values.Add("email", "sage");
            }
            else
            {
                values.Add("email", "");
            }
            values.Add("sub", "");
            values.Add("recaptcha_response_field", "manual_challenge");
            values.Add("pwd", (rand() * rand()).ToString("x"));
            values.Add("mode", "regist");
            if (checkBoxImage.Checked)
            {
                new MemoryStream();
                par.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 12L);
                MemoryStream stream = GeneratePicture();
                pi.pic = stream;
                pictureBox.Image = DelayBitmap;
                stream.Seek(0L, SeekOrigin.Begin);
            }
            else
            {
                pi.pic = null;
            }
            switch (doPost(pi))
            {
                case proxy_status.STATUS_BAND:
                    worker.ReportProgress(0, (userState.ip + ":") + userState.port + " B&\r\n");
                    worker.ReportProgress(0x63, userState);
                    break;

                case proxy_status.STATUS_OK:
                {
                    int num3;
                    worker.ReportProgress(0, (userState.ip + ":") + userState.port + " post success!\r\n");
                    worker.ReportProgress(2);
                    worker.ReportProgress(4);
                    int failures = userState.Failures;
                    if (failures <= 0)
                    {
                        num3 = 0;
                    }
                    else
                    {
                        num3 = failures - 1;
                    }
                    userState.Failures = num3;
                    return;
                }
                case proxy_status.STATUS_FLOOD:
                {
                    worker.ReportProgress(0, (userState.ip + ":") + userState.port+ " tripped flood detection\r\n");
                    DateTime time3 = DateTime.Now.AddSeconds(120.0);
                    userState.tm = time3;
       
                    goto Label_05C4;
                }
                case proxy_status.STATUS_DUPE:
                    worker.ReportProgress(0, (userState.ip + ":") + userState.port + " duplicate?\r\n");
                    break;

                case proxy_status.STATUS_CAP:
                    worker.ReportProgress(0, (userState.ip + ":") + userState.port + " bad captcha?\r\n");
                    break;

                case proxy_status.STATUS_LIMIT:
                    worker.ReportProgress(0, (userState.ip + ":") + userState.port + " image limit\r\n");
                    worker.ReportProgress(3);
                    break;

                case proxy_status.STATUS_INVALID:
                    worker.ReportProgress(0, (userState.ip + ":") + userState.port+ " invalid thread specified\r\n");
                    worker.ReportProgress(3);
                    break;

                case proxy_status.STATUS_NOBOARD:
                    worker.ReportProgress(0, (userState.ip + ":") + userState.port + " board doesn't exist\r\n");
                    break;

                case proxy_status.STATUS_ABORT:
                    worker.ReportProgress(0, (userState.ip + ":") + userState.port+ " operation aborted\r\n");
                    break;

                case proxy_status.STATUS_CONFAIL:
                    worker.ReportProgress(0, (userState.ip + ":") + userState.port+ " connection failed mid transfer");
                    worker.ReportProgress(5, userState);
                    break;

                case proxy_status.STATUS_RCVFAIL:
                    worker.ReportProgress(0, (userState.ip + ":") + userState.port+ " failed to receive response from 4chan");
                    worker.ReportProgress(5, userState);
                    break;

                default:
                    worker.ReportProgress(0, (userState.ip + ":") + userState.port+ " encountered unknown failure");
                    worker.ReportProgress(5, userState);
                    break;
            }
            if (checkBoxSage.Checked )
            {
                DateTime time2 = DateTime.Now.AddSeconds(60.0);
                userState.tm = time2;
            }
            else if (checkBoxNew.Checked)
            {
                DateTime time = DateTime.Now.AddSeconds(120.0);
                userState.tm = time;
            }
            else
            {
                DateTime time3 = DateTime.Now.AddSeconds(30.0);
                userState.tm = time3;
 
            
            }
        Label_05C4:
            Thread.Sleep(0x3e8);
            goto Label_0024;
        Label_0659:
            worker.ReportProgress(0, "No proxies available.\r\n");
        }

        private void bwPost_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            abort = true;
            labelCaptchaCount.Text = captchas.Count.ToString();
            DisplayButtons(true);
        }

        private void bwSpamPost_DoWork(object sender, DoWorkEventArgs e)
        {
            CProxy proxy;
            int num5;
            BackgroundWorker worker = (BackgroundWorker) sender;
            CPostInfo pi = new CPostInfo();
            int argument = (int) e.Argument;
        Label_001B:
            if (proxytable[argument].Count == 0)
            {
                worker.ReportProgress(0, ("No more proxies in table" + argument + ", thread ") + argument + " stopping\r\n");
                return;
            }
            if (abort)
            {
                return;
            }
            int num2 = this.proxycounter[argument];
            if (DateTime.Now <= proxytable[argument][num2].tm)
            {
                while (true)
                {
                    int[] numArray2 = this.proxycounter;
                    numArray2[argument]++;
                    if (this.proxycounter[argument] >= proxytable[argument].Count)
                    {
                        break;
                    }
                    num2 = this.proxycounter[argument];
                    if (DateTime.Now > proxytable[argument][num2].tm)
                    {
                        goto Label_00E9;
                    }
                }
                this.proxycounter[argument] = 0;
                Thread.Sleep(0x1388);
                goto Label_001B;
            }
        Label_00E9:
            proxy = proxytable[argument][num2];
        int[] proxycounter = this.proxycounter;
            int num6 = proxycounter[argument];
            if (num6 >= (proxytable[argument].Count - 1))
            {
                num5 = 0;
            }
            else
            {
                num5 = num6 + 1;
            }
            proxycounter[argument] = num5;
            srand((uint) ((int) DateTime.Now.Ticks));
            pi.prx = proxy;
            if (captchas.Count == 0)
            {
                worker.ReportProgress(1);
            }
            else
            {
                worker.ReportProgress(0, (proxy.ip + ":") + proxy.port + " posting\r\n");
                pi.submiturl = "https://sys.4chan.org/" + board + "/post";
                NameValueCollection  values = new NameValueCollection();
                pi.formdata = values;
                string str = GenerateText();
                values.Add("com", str);
                if (checkBoxNew.Checked)
                { values.Add("resto", "new"); }
                else
                {
                    values.Add("resto", thread);
                }
                values.Add("name", "");
                if (checkBoxSage.Checked)
                {
                    values.Add("email", "sage");
                }
                else
                {
                    values.Add("email", "");
                }
                values.Add("sub", "");
                values.Add("recaptcha_response_field", "manual_challenge");
                values.Add("pwd", (rand() * rand()).ToString("x"));
                values.Add("mode", "regist");
                if (checkBoxImage.Checked)
                {
                    MemoryStream stream = GeneratePicture();
                    pi.pic = stream;
                    pictureBox.Image = DelayBitmap;
                    stream.Seek(0L, SeekOrigin.Begin);
                }
                else
                {
                    pi.pic = null;
                }
                if (checkBox50.Checked && ((rand() % 3) == 0))
                {
                    pi.pic = null;
                }
                attempts++;
                switch (doPost(pi))
                {
                    case proxy_status.STATUS_BAND:
                        worker.ReportProgress(0, (proxy.ip + ":") + proxy.port + " B&\r\n");
                        worker.ReportProgress(0x63, proxy);
                        break;

                    case proxy_status.STATUS_OK:
                    {
                        int num3;
                        posts++;
                        worker.ReportProgress(0, (proxy.ip + ":") + proxy.port + " post success!\r\n");
                        worker.ReportProgress(2);
                        worker.ReportProgress(4);
                        int failures = proxy.Failures;
                        if (failures <= 0)
                        {
                            num3 = 0;
                        }
                        else
                        {
                            num3 = failures - 1;
                        }
                        proxy.Failures = num3;
                        break;
                    }
                    case proxy_status.STATUS_FLOOD:
                    {
                        worker.ReportProgress(0, (proxy.ip + ":") + proxy.port + " tripped flood detection\r\n");
                        DateTime time5 = DateTime.Now.AddSeconds(120.0);
                        proxy.tm = time5;
                        goto Label_001B;
                    }
                    case proxy_status.STATUS_DUPE:
                        worker.ReportProgress(0, (proxy.ip + ":") + proxy.port + " duplicate?\r\n");
                        break;

                    case proxy_status.STATUS_CAP:
                        worker.ReportProgress(0, (proxy.ip + ":") + proxy.port + " bad captcha?\r\n");
                        break;

                    case proxy_status.STATUS_LIMIT:
                        worker.ReportProgress(0, (proxy.ip + ":") + proxy.port + " image limit\r\n");
                        worker.ReportProgress(3);
                        break;

                    case proxy_status.STATUS_INVALID:
                        worker.ReportProgress(0, (proxy.ip + ":") + proxy.port+ " invalid thread specified\r\n");
                        worker.ReportProgress(3);
                        break;

                    case proxy_status.STATUS_NOBOARD:
                        worker.ReportProgress(0, (proxy.ip + ":") + proxy.port + " board doesn't exist\r\n");
                        break;

                    case proxy_status.STATUS_ABORT:
                        worker.ReportProgress(0, (proxy.ip + ":") + proxy.port + " operation aborted\r\n");
                        break;

                    case proxy_status.STATUS_CONFAIL:
                        worker.ReportProgress(0, (proxy.ip + ":") + proxy.port + " connection failed mid transfer");
                        worker.ReportProgress(5, proxy);
                        break;

                    case proxy_status.STATUS_RCVFAIL:
                        worker.ReportProgress(0, (proxy.ip + ":") + proxy.port + " failed to receive response from 4chan");
                        worker.ReportProgress(5, proxy);
                        break;

                    default:
                        worker.ReportProgress(0, (proxy.ip + ":") + proxy.port + " encountered unknown failure");
                        worker.ReportProgress(5, proxy);
                        break;
                }
                if (thread == "")
                {
                    DateTime time4 = DateTime.Now.AddSeconds(300.0);
                    proxy.tm = time4;
                }
                else if (checkBoxSage.Checked)
                {
                    DateTime time3 = DateTime.Now.AddSeconds(60.0);
                    proxy.tm = time3;
                }
                else
                {
                    DateTime time2 = DateTime.Now.AddSeconds(30.0);
                    proxy.tm = time2;
                }
                goto Label_001B;
            }
        }

        private void bwSpamPost_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            bool flag = false;
            if (e.ProgressPercentage == 0)
            {
                status.AppendText((string) e.UserState);
            }
            if (e.ProgressPercentage == 1)
            {
                status.AppendText("No more captchas, stopping.\r\n");
                abort = true;
                DisplayButtons(true);
            }
            if (e.ProgressPercentage == 2)
            {
                labelCaptchaCount.Text = captchas.Count.ToString();
            }
            if (e.ProgressPercentage == 3)
            {
                status.AppendText("Image limit or 404, stopping.\r\n");
                DisplayButtons(true);
                abort = true;
            }
            if (e.ProgressPercentage == 4)
            {
                labelAttempts.Text = attempts.ToString();
                labelPosts.Text = posts.ToString();
            }
            if (e.ProgressPercentage == 5)
            {
                CProxy userState = (CProxy) e.UserState;
                int num3 = userState.Failures + 1;
                userState.Failures = num3;
                if (num3 >= 2)
                {
                    status.AppendText(", proxy sucks, not using it anymore.\r\n");
                    flag = true;
                }
                else
                {
                    status.AppendText(Environment.NewLine);
                }
            }
            if ((e.ProgressPercentage == 0x63) || flag)
            {
                CProxy proxy = (CProxy) e.UserState;
                List<CProxy>[] proxytable = this.proxytable;
                int index = 0;
                if (0 < proxytable.Length)
                {
                    do
                    {
                        List<CProxy> list = proxytable[index];
                        int num = 0;
                        if (0 < list.Count)
                        {
                            do
                            {
                                if ((list[num].ip == proxy.ip) && (list[num].port == proxy.port))
                                {
                                    list.RemoveAt(num);
                                    labelProxyCount.Text = proxies_count().ToString();
                                    break;
                                }
                                num++;
                            }
                            while (num < list.Count);
                        }
                        index++;
                    }
                    while (index < proxytable.Length);
                }
            }
        }

        private void bwSpamPost_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            labelCaptchaCount.Text = captchas.Count.ToString();
        }

        private void bwTimedSpamPost_DoWork(object sender, DoWorkEventArgs e)
        {
            CProxy userState = null;
            proxy_status _status;
            BackgroundWorker worker = (BackgroundWorker) sender;
            CPostInfo pi = new CPostInfo();
            int argument = (int) e.Argument;
        Label_001E:
            if (proxies_count() == 0)
            {
                worker.ReportProgress(0, "No more proxies, stopping\r\n");
                abort = true;
                return;
            }
            if (abort)
            {
                return;
            }
            bool flag = false;
            List<CProxy>[] proxytable = this.proxytable;
            int index = 0;
            if (0 < proxytable.Length)
            {
                do
                {
                    List<CProxy> list = proxytable[index];
                    int num = 0;
                    if (0 < list.Count)
                    {
                        do
                        {
                            if (DateTime.Now > list[num].tm)
                            {
                                userState = list[num];
                                flag = true;
                                break;
                            }
                            num++;
                        }
                        while (num < list.Count);
                    }
                    index++;
                }
                while (index < proxytable.Length);
                if (flag)
                {
                    srand((uint) ((int) DateTime.Now.Ticks));
                    pi.prx = userState;
                    if (captchas.Count == 0)
                    {
                        worker.ReportProgress(1);
                        return;
                    }
                    worker.ReportProgress(0, (userState.ip + ":") + userState.port + " posting\r\n");
                    pi.submiturl = "https://sys.4chan.org/" + board + "/post";
                    NameValueCollection  values = new NameValueCollection();
                    pi.formdata = values;
                    string str = GenerateText();
                    values.Add("com", str);
                    if (checkBoxNew.Checked)
                    {
                        values.Add("resto", "new");
                    }
                    else
                    {
                        values.Add("resto", thread);
                    }
                    values.Add("name", "");
                    if (checkBoxSage.Checked)
                    {
                        values.Add("email", "sage");
                    }
                    else
                    {
                        values.Add("email", "");
                    }
                    values.Add("sub", "");
                    values.Add("recaptcha_response_field", "manual_challenge");
                    values.Add("pwd", (rand() * rand()).ToString("x"));
                    values.Add("mode", "regist");
                    if (checkBoxImage.Checked)
                    {
                        MemoryStream stream = GeneratePicture();
                        pi.pic = stream;
                        pictureBox.Image = DelayBitmap;
                        stream.Seek(0L, SeekOrigin.Begin);
                    }
                    else
                    {
                        pi.pic = null;
                    }
                    if (checkBox50.Checked && ((rand() % 2) == 0))
                    {
                        pi.pic = null;
                    }
                    attempts++;
                    _status = doPost(pi);
                    if (checkBoxCheck.Checked)
                    {
                        userState.Failures = 0;
                    }
                    switch (_status)
                    {
                        case proxy_status.STATUS_BAND:
                            worker.ReportProgress(0, (userState.ip + ":") + userState.port + " B&\r\n");
                            worker.ReportProgress(0x63, userState);
                            goto Label_060D;

                        case proxy_status.STATUS_OK:
                        {
                            int num3;
                            posts++;
                            worker.ReportProgress(0, (userState.ip + ":") + userState.port + " post success!\r\n");
                            worker.ReportProgress(2);
                            worker.ReportProgress(4);
                            int failures = userState.Failures;
                            if (failures <= 0)
                            {
                                num3 = 0;
                            }
                            else
                            {
                                num3 = failures - 1;
                            }
                            userState.Failures = num3;
                            goto Label_060D;
                        }
                        case proxy_status.STATUS_FLOOD:
                        {
                            worker.ReportProgress(0, (userState.ip + ":") + userState.port + " tripped flood detection\r\n");
                            DateTime time4 = DateTime.Now.AddSeconds(120.0);
                            userState.tm = time4;
                            goto Label_001E;
                        }
                        case proxy_status.STATUS_DUPE:
                            worker.ReportProgress(0, (userState.ip + ":") + userState.port + " duplicate?\r\n");
                            goto Label_060D;

                        case proxy_status.STATUS_CAP:
                            worker.ReportProgress(0, (userState.ip + ":") + userState.port + " bad captcha?\r\n");
                            goto Label_060D;

                        case proxy_status.STATUS_LIMIT:
                            worker.ReportProgress(0, (userState.ip + ":") + userState.port + " image limit\r\n");
                            worker.ReportProgress(3);
                            goto Label_060D;

                        case proxy_status.STATUS_INVALID:
                            worker.ReportProgress(0, (userState.ip + ":") + userState.port + " invalid thread specified\r\n");
                            worker.ReportProgress(3);
                            goto Label_060D;

                        case proxy_status.STATUS_NOBOARD:
                            worker.ReportProgress(0, (userState.ip + ":") + userState.port + " board doesn't exist\r\n");
                            goto Label_060D;

                        case proxy_status.STATUS_ABORT:
                            worker.ReportProgress(0, (userState.ip + ":") + userState.port + " operation aborted\r\n");
                            goto Label_060D;

                        case proxy_status.STATUS_CONFAIL:
                            worker.ReportProgress(0, (userState.ip + ":") + userState.port + " connection failed mid transfer");
                            worker.ReportProgress(5, userState);
                            goto Label_060D;

                        case proxy_status.STATUS_RCVFAIL:
                            worker.ReportProgress(0, (userState.ip + ":") + userState.port + " failed to receive response from 4chan");
                            worker.ReportProgress(5, userState);
                            goto Label_060D;
                    }
                    worker.ReportProgress(0, (userState.ip + ":") + userState.port + " encountered unknown failure");
                    worker.ReportProgress(5, userState);
                    goto Label_060D;
                }
            }
            worker.ReportProgress(0, "No proxies available, waiting 20 seconds...\r\n");
            Thread.Sleep(0x4e20);
            goto Label_001E;
        Label_060D:
            if (thread == "")
            {
                DateTime time3 = DateTime.Now.AddSeconds(300.0);
                userState.tm = time3;
            }
            else if (checkBoxSage.Checked)
            {
                DateTime time2 = DateTime.Now.AddSeconds(60.0);
                userState.tm = time2;
            }
            else
            {
                DateTime time = DateTime.Now.AddSeconds(30.0);
                userState.tm = time;
            }
            if (_status == proxy_status.STATUS_OK)
            {
                worker.ReportProgress(0, "Sleeping for " + argument + " seconds.\r\n");
                Thread.Sleep((int) (argument * 0x3e8));
            }
            goto Label_001E;
        }

        private void LoadCaptchaPics(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            WebClient client = new WebClient();
            int num2 = 0;
            while (true)
            {
                if ((pics.Count >= 5) || (num2 >= 3))
                {
                    break;
                }
                try
                {
                    string str2 = client.DownloadString(BaseCaptchaURL);
                    uint index = (uint)str2.IndexOf("src=\"image?c=");
                    uint num3 = (uint)str2.IndexOf("\"", (int)(((int)index) + 5));
                    string s = str2.Substring(((int)index) + 13, ((int)(num3 - index)) - 13);
                    string address = "http://www.google.com/recaptcha/api/image?c=" + s;
                    string fileName = CaptchaImagesDirectory + s + ".jpg";
                    client.DownloadFile(address, fileName);
                    pics.Add(new CPic(s));
                    worker.ReportProgress(0);
                    if (pics.Count == 1)
                    {
                        pictureBoxCaptcha.LoadAsync(CaptchaImagesDirectory + pics[0].fn + ".jpg");
                    }
                }
                catch (Exception)
                {
                    worker.ReportProgress(1);
                    num2++;
                }
            }
        }

        private void LoadCaptchaPicsProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (e.ProgressPercentage)
            {
                case 0:
                    pictureBoxCaptcha.Visible = true;
                    labelCaptchaError.Visible = false;
                    break;

                case 1:
                    pictureBoxCaptcha.Visible = false;
                    labelCaptchaError.Visible = true;
                    break;
            }
        }

        private void SubmitCaptchaAsyncCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            labelCaptchaCount.Text = captchas.Count.ToString();
        }

        private void SubmitCaptchaAsyncDoWork(object sender, DoWorkEventArgs e)
        {
            CSubmitCaptchaData argument = (CSubmitCaptchaData)e.Argument;
            string url = argument.url;
            DateTime tm = argument.tm;
            string str = "";
            try
            {
                str = new WebClient().DownloadString(url);
            }
            catch (Exception)
            {
                return;
            }
            if (str.IndexOf("Your answer was correct.") != -1 || str.IndexOf("Réponse correcte") != -1)//Add || and your local language here.
            {
                uint index = (uint)str.IndexOf("cols=\"100\">");
                uint num2 = (uint)str.IndexOf("<", (int)(((int)index) + 1));
                captchas.Add(new CCaptcha(str.Substring(((int)index) + 11, ((int)(num2 - index)) - 11), tm));
            }
        }

        private void testProxy_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            CProxy argument = (CProxy)e.Argument;

            WebClient client = new WebClient
            {
                Proxy = new WebProxy(argument.ip, argument.port)
            };
            
            try
            {
                string str = client.DownloadString("https://4chan.org/banned");
                if (str.IndexOf("You are banned") != -1)
                { throw new ArgumentException(); }
                else
                {
                    DateTime now = DateTime.Now;
                    argument.tm = now;
                    argument.Failures = 0;
                    proxytable[currenttable].Add(argument);
                    worker.ReportProgress(0, ((argument.ip + ":") + argument.port + " works, stored in row ") + (currenttable + 1) + Environment.NewLine);
                    worker.ReportProgress(1);
                    /*There's should be something here but I can't figure shit out.*/
                    /*Oh wait !*/
                    if (currenttable < 4)
                    {
                        currenttable++;
                    }
                    else
                    {
                        currenttable =0;
                    }
                }
            }
            catch (Exception)
            {
                worker.ReportProgress(0, (argument.ip + ":") + argument.port + " doesn't work\r\n");
            }
        }

        private void testProxy_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                status.AppendText((string)e.UserState);
            }
            if (e.ProgressPercentage == 1)
            {
                labelProxyCount.Text = proxies_count().ToString();
            }
        }
        #endregion

        #region Some Methods

        private proxy_status doPost(CPostInfo pi)
        {
            int num4;
            proxy_status _status2;
            string str = "";
            new MemoryStream();
            CProxy prx = pi.prx;
            new WebClient { Proxy = new WebProxy(prx.ip, prx.port) };
            srand((uint)((int)DateTime.Now.Ticks));
            int num6 = rand() % 3;
            rand();
            switch (num6)
            {
                case 0:
                    num4 = (rand() % 0x63) + 50;
                    break;

                case 1:
                    num4 = (rand() % 0x3e7) + 100;
                    break;

                default:
                    num4 = (rand() % 0x3e7) << 1;
                    break;
            }
            DateTime time = new DateTime(0x7b2, 1, 1, 0, 0, 0);
            TimeSpan span = (TimeSpan)(DateTime.UtcNow - time);
            long num3 = Convert.ToInt64(span.TotalMilliseconds);
            int num2 = 0;
            if (0 < (rand() % 6))
            {
                long num9 = num4 * 0xf4240;
                do
                {
                    num3 -= num9;
                    num2++;
                }
                while (num2 < (rand() % 6));
            }
            string str2 = (num3 - rand()).ToString();
            if (checkBoxDot.Checked)
            {
                str2 = str2 + ".png";
            }
            else
            {
                str2 = str2 + ".jpg";
            }
            byte[] buffer2 = new byte[0x200];
            int num8 = rand() * rand();
            int num7 = rand() * num8;
            int num5 = rand() * num7;
            string str4 = num5.ToString("x");
            byte[] bytes = Encoding.ASCII.GetBytes("\r\n--" + str4 + Environment.NewLine);
            string format = "\r\n--" + str4 + "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}";
            string str10 = "Content-Disposition: form-data; name=\"upfile\"; filename=\"{0}\"\r\nContent-Type: application/octet-stream\r\nContent-Transfer-Encoding: binary\r\n\r\n";
            ServicePointManager.Expect100Continue = false;
            try
            {
                int num;
                prx = pi.prx;
                WebProxy proxy2 = new WebProxy(prx.ip, prx.port)
                {
                    UseDefaultCredentials = true
                };
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(pi.submiturl);
                request.ContentType = "multipart/form-data; boundary=" + num5.ToString("x");
                request.UserAgent = "Mozilla/5.0 (Windows NT 5.0; WOW64; rv:6.0) Gecko/20100101 Firefox/6.0";
                request.Method = "POST";
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Proxy = proxy2;
                request.Timeout = 0x3a98;
                request.ReadWriteTimeout = 0x3a98;
                Stream requestStream = request.GetRequestStream();
                IEnumerator enumerator = pi.formdata.Keys.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        string current = (string)enumerator.Current;
                        string str9 = string.Format(format, current, pi.formdata[current]);
                        byte[] buffer5 = Encoding.UTF8.GetBytes(str9);
                        requestStream.Write(buffer5, 0, buffer5.Length);
                    }
                }
                finally
                {
                    IEnumerator enumerator2 = enumerator;
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
                requestStream.Write(bytes, 0, bytes.Length);
                if (abort)
                {
                    return proxy_status.STATUS_ABORT;
                }
                string val = "";
                if (checkBoxCheck.Checked)
                {
                    val = (rand() * rand()).ToString();
                }
                else
                {
                    val = captchas[0].val;
                    captchas.RemoveAt(0);
                }
                string s = string.Format(format, "recaptcha_challenge_field", val);
                byte[] buffer = Encoding.UTF8.GetBytes(s);
                requestStream.Write(buffer, 0, buffer.Length);
                requestStream.Write(bytes, 0, bytes.Length);
                if (pi.pic != null)
                {
                    string str7 = string.Format(str10, str2);
                    byte[] buffer3 = Encoding.UTF8.GetBytes(str7);
                    requestStream.Write(buffer3, 0, buffer3.Length);
                    while (true)
                    {
                        num = pi.pic.Read(buffer2, 0, buffer2.Length);
                        if (num == 0)
                        {
                            break;
                        }
                        if (abort)
                        {
                            return proxy_status.STATUS_ABORT;
                        }
                        requestStream.Write(buffer2, 0, num);
                    }
                }
                requestStream.Flush();
                bytes = Encoding.ASCII.GetBytes("\r\n--" + str4 + "--\r\n");
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                while (true)
                {
                    num = responseStream.Read(buffer2, 0, buffer2.Length);
                    if (num <= 0)
                    {
                        break;
                    }
                    str = str + Encoding.ASCII.GetString(buffer2, 0, num);
                }
                responseStream.Close();
                response.Close();
                if (str.IndexOf("Post successful") != -1)
                {
                    return proxy_status.STATUS_OK;
                }
                if (str.IndexOf("Flood") != -1)
                {
                    return proxy_status.STATUS_FLOOD;
                }
                if (str.IndexOf("Duplicate") != -1)
                {
                    return proxy_status.STATUS_DUPE;
                }
                if (str.IndexOf("verification") != -1)
                {
                    return proxy_status.STATUS_CAP;
                }
                if (str.IndexOf("board doesn") != -1)
                {
                    return proxy_status.STATUS_NOBOARD;
                }
                if (str.IndexOf("ISP") != -1)
                {
                    return proxy_status.STATUS_BAND;
                }
                if (str.IndexOf("Max limit of") != -1)
                {
                    return proxy_status.STATUS_LIMIT;
                }
                if (str.IndexOf("Thread specified") != -1)
                {
                    return proxy_status.STATUS_INVALID;
                }
                goto Label_054F;
            }
            catch (WebException exception)
            {
                if (exception.Response != null)
                {
                    if (((HttpWebResponse)exception.Response).StatusCode == HttpStatusCode.Forbidden)
                    {
                        return proxy_status.STATUS_BAND;
                    }
                    goto Label_054F;
                }
                if (exception.Status == WebExceptionStatus.ReceiveFailure)
                {
                    return proxy_status.STATUS_RCVFAIL;
                }
                if (exception.Status != WebExceptionStatus.Success)
                {
                    return proxy_status.STATUS_CONFAIL;
                }
                _status2 = proxy_status.STATUS_UNKNOWNWEB;
            }
            catch (Exception)
            {
                goto Label_054F;
            }
            return _status2;
        Label_054F:
            return proxy_status.STATUS_UNKNOWN;
        }

        private MemoryStream GeneratePicture()
        {
            Bitmap bitmap;
            MemoryStream stream = new MemoryStream();
            if (checkBoxRandImg.Checked)
            {
                DelayBitmap.Dispose();
                int length = images.Length;
                string filename = images[rand() % length];
                DelayBitmap = new Bitmap(filename);
            }
            if (checkBoxDot.Checked)
            {
                int height = (rand() % 100) + 100;
                bitmap = new Bitmap((rand() % 100) + 100, height);
            }
            else
            {
                double num5;
                int width;
                if (DelayBitmap.Width > DelayBitmap.Height)
                {
                    width = DelayBitmap.Width;
                }
                else
                {
                    width = DelayBitmap.Height;
                }
                double num3 = 125.0 / ((double)width);
                if (num3 < 1.0)
                {
                    num5 = 1.0;
                }
                else
                {
                    num5 = num3 + 1.0;
                }
                double num4 = (((rand() * (num5 - num3)) * 3.0517578125E-05) + num3) + 0.1;
                bitmap = new Bitmap(DelayBitmap, (int)(DelayBitmap.Width * num4), (int)(DelayBitmap.Height * num4));
                if ((bitmap.Width >= 300) && (bitmap.Height >= 300))
                {
                    if ((bitmap.Width > 900) || (bitmap.Height > 900))
                    {
                        bitmap = new Bitmap(bitmap, bitmap.Width / 2, bitmap.Height / 2);
                    }
                }
                else
                {
                    bitmap = new Bitmap(bitmap, bitmap.Width << 1, bitmap.Height << 1);
                }
            }
            if (checkBoxDot.Checked)
            {
                Color color = Color.FromArgb(0xff, rand() % 0xff, rand() % 0xff, rand() % 0xff);
                int x = 0;
                if (0 < bitmap.Width)
                {
                    do
                    {
                        int y = 0;
                        if (0 < bitmap.Height)
                        {
                            do
                            {
                                bitmap.SetPixel(x, y, color);
                                y++;
                            }
                            while (y < bitmap.Height);
                        }
                        x++;
                    }
                    while (x < bitmap.Width);
                }
                bitmap.Save(stream, ImageFormat.Png);
            }
            else
            {
                par.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)((rand() % 20) + 0x41));
                bitmap.Save(stream, jpg, par);
            }
            bitmap.Dispose();
            return stream;
        }

        private string GenerateText()
        {
            string input;
            if (checkBoxQuote.Checked)
                input = ">>" + textBoxQuote.Text.Substring(0, thread.Length - 2) + (((rand() % 0x59) + 10));
            else
                input = "";
            input = (input + "\n" + postlist[rand() % postlist.Count]).Replace("<br />", Environment.NewLine);
            return new Regex("<[^>]*>").Replace(input, "").Replace("Comment too long. Click here to view the full text.", "");
        }

        private int ParseURL()
        {
            try
            {
                string text = textBoxThread.Text;
                if (text.Substring(0, 7) == "http://")
                {
                    text = text.Substring(7);
                }
                if (text.Substring(0, 0x10) != "boards.4chan.org")
                {
                    throw new ArgumentException();
                }
                text = text.Substring(0x11);
                int index = text.IndexOf('/');
                if (index == -1)
                {
                    board = text.Substring(0, text.Length);
                    thread = "";
                    return 0;
                }
                board = text.Substring(0, index);
                if ((text.Length > 1) && (text.Length < 5))
                {
                    thread = "";
                    return 0;
                }
                text = text.Substring(index + 5);
                index = 0;
                while (true)
                {
                    if (((index >= text.Length) || (text[index] > '9')) || (text[index] < '0'))
                    {
                        break;
                    }
                    index++;
                }
                thread = text.Substring(0, index);
            }
            catch (Exception)
            {
                thread = "";
                board = "";
                return -1;
            }
            return 0;
        }

        private int proxies_count()
        {
            int num2 = 0;
            List<CProxy>[] proxytable = this.proxytable;
            int index = 0;
            if (0 < proxytable.Length)
            {
                do
                {
                    num2 = proxytable[index].Count + num2;
                    index++;
                }
                while (index < proxytable.Length);
            }
            return num2;
        }

        private void PruneCaptchas()
        {
            for (int i = captchas.Count - 1; i >= 0; i--)
            {
                if (!captchas[i].isValid())
                {
                    captchas.RemoveAt(i);
                }
            }
            labelCaptchaCount.Text = captchas.Count.ToString();
        }

        #endregion

        #region Classes

        private class CCaptcha
        {
            public DateTime expiretime;
            public string val;

            public CCaptcha(string s, DateTime t)
            {
                expiretime = t;
                val = s;
            }

            public CCaptcha(string s, long t)
            {
                expiretime = new DateTime(t);
                val = s;
            }

            [return: MarshalAs(UnmanagedType.U1)]
            public bool isValid()
            {
                bool num;
                if ((DateTime.Now < expiretime) && (DateTime.Now.AddDays(1.0) > expiretime))
                {
                    num = true;
                }
                else
                {
                    num = false;
                }
                return num;
            }

            [return: MarshalAs(UnmanagedType.U1)]
            public bool isValid2()
            {
                bool num;
                if ((DateTime.Now.AddHours(1.0) < expiretime) && (DateTime.Now.AddDays(1.0) > expiretime))
                {
                    num = true;
                }
                else
                {
                    num = false;
                }
                return num;
            }
        }

        private class CPic
        {
            public DateTime expiretime;
            public string fn;

            public CPic(string s)
            {
                DateTime time = DateTime.Now.AddMinutes(295.0);
                expiretime = time;
                fn = s;
            }

            [return: MarshalAs(UnmanagedType.U1)]
            public bool isValid()
            {
                bool num;
                if ((DateTime.Now < expiretime) && (DateTime.Now.AddDays(1.0) > expiretime))
                {
                    num = true;
                }
                else
                {
                    num = false;
                }
                return num;
            }
        }

        private class CPostInfo : IDisposable
        {
            public NameValueCollection formdata;
            public MemoryStream pic;
            public Form1.CProxy prx;
            public string submiturl;
            private bool disposed;
            private IntPtr handle;
            private readonly Component component = new Component();


            void IDisposable.Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);


            }
            [System.Runtime.InteropServices.DllImport("Kernel32")]
            private extern static Boolean CloseHandle(IntPtr handle);
            protected virtual void Dispose(bool disposing)
            {
                if (!disposed)
                {

                    if (disposing)
                    {
                        component.Dispose();
                    }

                    CloseHandle(handle);
                    handle = IntPtr.Zero;


                    disposed = true;

                }
            }
        }

        private class CProxy
        {
            public int Failures;
            public string ip;
            public int port;
            public DateTime tm;
        }

        private class CSubmitCaptchaData
        {
            public DateTime tm;
            public string url;

            public CSubmitCaptchaData(string s, DateTime t)
            {
                url = s;
                tm = t;
            }
        }

        private enum proxy_status
        {
            STATUS_OK,
            STATUS_BAND,
            STATUS_FLOOD,
            STATUS_CAP,
            STATUS_DUPE,
            STATUS_LIMIT,
            STATUS_INVALID,
            STATUS_NOBOARD,
            STATUS_CONFAIL,
            STATUS_RCVFAIL,
            STATUS_UNKNOWNWEB,
            STATUS_UNKNOWN,
            STATUS_ABORT
        }
        #endregion

        #region Auto-Generated Methods

        private void DisplayButtons([MarshalAs(UnmanagedType.U1)] bool display)
        {
            buttonGO.Enabled = display;
            byte num = Convert.ToByte(!display);
            buttonAbort.Enabled = Convert.ToBoolean(num);
            textBoxThread.Enabled = display;
            textBoxQuote.Enabled = display;
            textBoxMessage.Enabled = display;
            checkBoxSage.Enabled = display;
            checkBoxRandImg.Enabled = display;
            buttonPic.Enabled = display;
            checkBoxImage.Enabled = display;
            buttonPost.Enabled = display;
            textBoxName.Enabled = display;
            textBoxTimer.Enabled = display;
            checkBoxCheck.Enabled = display;
            checkBoxDot.Enabled = display;
            checkBoxQuote.Enabled = display;
            checkBox50.Enabled = display;
        }
        [HandleProcessCorruptedStateExceptions]
        protected override void Dispose([MarshalAs(UnmanagedType.U1)] bool flag1)
        {
            if (flag1)
            {
                try
                {
                    Forme1();
                    return;
                }
                finally
                {
                    base.Dispose(true);
                }
            }
            base.Dispose(false);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBoxCaptcha = new System.Windows.Forms.PictureBox();
            this.labelCaptchaCount = new System.Windows.Forms.Label();
            this.textBoxCaptcha = new System.Windows.Forms.TextBox();
            this.status = new System.Windows.Forms.TextBox();
            this.labelProxyCount = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxThread = new System.Windows.Forms.TextBox();
            this.buttonLoadProxies = new System.Windows.Forms.Button();
            this.openFileDialogProxy = new System.Windows.Forms.OpenFileDialog();
            this.buttonGO = new System.Windows.Forms.Button();
            this.labelCaptchaError = new System.Windows.Forms.Label();
            this.buttonAbort = new System.Windows.Forms.Button();
            this.labelAttempts = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelPosts = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.buttonSaveProxies = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.buttonPost = new System.Windows.Forms.Button();
            this.buttonPurge = new System.Windows.Forms.Button();
            this.buttonDownload = new System.Windows.Forms.Button();
            this.buttonClearCap = new System.Windows.Forms.Button();
            this.openFileDialogPicture = new System.Windows.Forms.OpenFileDialog();
            this.checkBoxImage = new System.Windows.Forms.CheckBox();
            this.checkBoxCheck = new System.Windows.Forms.CheckBox();
            this.checkBoxSage = new System.Windows.Forms.CheckBox();
            this.checkBoxRandImg = new System.Windows.Forms.CheckBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.buttonPic = new System.Windows.Forms.Button();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.pictureBoxKeion = new System.Windows.Forms.PictureBox();
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.textBoxTimer = new System.Windows.Forms.TextBox();
            this.labelTimer = new System.Windows.Forms.Label();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.textBoxRemove = new System.Windows.Forms.TextBox();
            this.checkBoxDot = new System.Windows.Forms.CheckBox();
            this.checkBoxQuote = new System.Windows.Forms.CheckBox();
            this.textBoxQuote = new System.Windows.Forms.TextBox();
            this.checkBox50 = new System.Windows.Forms.CheckBox();
            this.checkBoxNew = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCaptcha)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxKeion)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(356, 425);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Captchas:";
            // 
            // pictureBoxCaptcha
            // 
            this.pictureBoxCaptcha.Location = new System.Drawing.Point(10, 425);
            this.pictureBoxCaptcha.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxCaptcha.Name = "pictureBoxCaptcha";
            this.pictureBoxCaptcha.Size = new System.Drawing.Size(300, 57);
            this.pictureBoxCaptcha.TabIndex = 7;
            this.pictureBoxCaptcha.TabStop = false;
            // 
            // labelCaptchaCount
            // 
            this.labelCaptchaCount.AutoSize = true;
            this.labelCaptchaCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelCaptchaCount.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelCaptchaCount.Location = new System.Drawing.Point(428, 425);
            this.labelCaptchaCount.Name = "labelCaptchaCount";
            this.labelCaptchaCount.Size = new System.Drawing.Size(25, 13);
            this.labelCaptchaCount.TabIndex = 11;
            this.labelCaptchaCount.Text = "000";
            this.toolTip1.SetToolTip(this.labelCaptchaCount, "Available captchas.");
            // 
            // textBoxCaptcha
            // 
            this.textBoxCaptcha.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxCaptcha.Location = new System.Drawing.Point(10, 485);
            this.textBoxCaptcha.Name = "textBoxCaptcha";
            this.textBoxCaptcha.Size = new System.Drawing.Size(300, 20);
            this.textBoxCaptcha.TabIndex = 10;
            this.textBoxCaptcha.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxCaptcha_KeyPress);
            // 
            // status
            // 
            this.status.BackColor = System.Drawing.Color.White;
            this.status.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.status.Location = new System.Drawing.Point(9, 104);
            this.status.Multiline = true;
            this.status.Name = "status";
            this.status.ReadOnly = true;
            this.status.Size = new System.Drawing.Size(300, 174);
            this.status.TabIndex = 12;
            // 
            // labelProxyCount
            // 
            this.labelProxyCount.AutoSize = true;
            this.labelProxyCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelProxyCount.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelProxyCount.Location = new System.Drawing.Point(428, 443);
            this.labelProxyCount.Name = "labelProxyCount";
            this.labelProxyCount.Size = new System.Drawing.Size(25, 13);
            this.labelProxyCount.TabIndex = 14;
            this.labelProxyCount.Text = "000";
            this.toolTip1.SetToolTip(this.labelProxyCount, "Available good proxies.");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(356, 443);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Proxies:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.Location = new System.Drawing.Point(12, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Target Thread";
            // 
            // textBoxThread
            // 
            this.textBoxThread.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxThread.Location = new System.Drawing.Point(109, 78);
            this.textBoxThread.Name = "textBoxThread";
            this.textBoxThread.Size = new System.Drawing.Size(200, 20);
            this.textBoxThread.TabIndex = 16;
            this.toolTip1.SetToolTip(this.textBoxThread, "Enter the full URL, not just the thread number.");
            // 
            // buttonLoadProxies
            // 
            this.buttonLoadProxies.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.buttonLoadProxies.Location = new System.Drawing.Point(321, 3);
            this.buttonLoadProxies.Name = "buttonLoadProxies";
            this.buttonLoadProxies.Size = new System.Drawing.Size(157, 24);
            this.buttonLoadProxies.TabIndex = 17;
            this.buttonLoadProxies.Text = "Load Proxies";
            this.toolTip1.SetToolTip(this.buttonLoadProxies, "Load a text file with proxies.");
            this.buttonLoadProxies.UseVisualStyleBackColor = true;
            this.buttonLoadProxies.Click += new System.EventHandler(this.buttonLoadProxies_Click);
            // 
            // openFileDialogProxy
            // 
            this.openFileDialogProxy.Filter = "Text Files|*.txt";
            this.openFileDialogProxy.Title = "Select text file with proxy list";
            // 
            // buttonGO
            // 
            this.buttonGO.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonGO.Font = new System.Drawing.Font("Impact", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonGO.Location = new System.Drawing.Point(321, 177);
            this.buttonGO.Name = "buttonGO";
            this.buttonGO.Size = new System.Drawing.Size(156, 68);
            this.buttonGO.TabIndex = 19;
            this.buttonGO.Text = "SPAM";
            this.buttonGO.UseVisualStyleBackColor = true;
            this.buttonGO.Click += new System.EventHandler(this.buttonGO_Click);
            // 
            // labelCaptchaError
            // 
            this.labelCaptchaError.AutoSize = true;
            this.labelCaptchaError.Location = new System.Drawing.Point(13, 448);
            this.labelCaptchaError.Name = "labelCaptchaError";
            this.labelCaptchaError.Size = new System.Drawing.Size(286, 13);
            this.labelCaptchaError.TabIndex = 20;
            this.labelCaptchaError.Text = "Connection problems.  Press enter in the box below to retry.";
            this.labelCaptchaError.Visible = false;
            // 
            // buttonAbort
            // 
            this.buttonAbort.Enabled = false;
            this.buttonAbort.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.buttonAbort.Location = new System.Drawing.Point(321, 251);
            this.buttonAbort.Name = "buttonAbort";
            this.buttonAbort.Size = new System.Drawing.Size(156, 27);
            this.buttonAbort.TabIndex = 21;
            this.buttonAbort.Text = "Abort";
            this.buttonAbort.UseVisualStyleBackColor = true;
            this.buttonAbort.Click += new System.EventHandler(this.buttonAbort_Click);
            // 
            // labelAttempts
            // 
            this.labelAttempts.AutoSize = true;
            this.labelAttempts.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelAttempts.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelAttempts.Location = new System.Drawing.Point(428, 486);
            this.labelAttempts.Name = "labelAttempts";
            this.labelAttempts.Size = new System.Drawing.Size(25, 13);
            this.labelAttempts.TabIndex = 25;
            this.labelAttempts.Text = "000";
            this.toolTip1.SetToolTip(this.labelAttempts, "Attempted posts.");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label5.Location = new System.Drawing.Point(356, 486);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "Attempts:";
            // 
            // labelPosts
            // 
            this.labelPosts.AutoSize = true;
            this.labelPosts.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelPosts.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelPosts.Location = new System.Drawing.Point(428, 468);
            this.labelPosts.Name = "labelPosts";
            this.labelPosts.Size = new System.Drawing.Size(25, 13);
            this.labelPosts.TabIndex = 23;
            this.labelPosts.Text = "000";
            this.toolTip1.SetToolTip(this.labelPosts, "Succesful posts.");
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label7.Location = new System.Drawing.Point(356, 468);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Posts:";
            // 
            // buttonSaveProxies
            // 
            this.buttonSaveProxies.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.buttonSaveProxies.Location = new System.Drawing.Point(321, 28);
            this.buttonSaveProxies.Name = "buttonSaveProxies";
            this.buttonSaveProxies.Size = new System.Drawing.Size(157, 23);
            this.buttonSaveProxies.TabIndex = 26;
            this.buttonSaveProxies.Text = "Save Good Proxies";
            this.toolTip1.SetToolTip(this.buttonSaveProxies, "Save a list of the good proxies that have been loaded so far.");
            this.buttonSaveProxies.UseVisualStyleBackColor = true;
            this.buttonSaveProxies.Click += new System.EventHandler(this.buttonSaveProxies_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Text Files|*.txt";
            // 
            // buttonPost
            // 
            this.buttonPost.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonPost.Location = new System.Drawing.Point(251, 357);
            this.buttonPost.Name = "buttonPost";
            this.buttonPost.Size = new System.Drawing.Size(69, 59);
            this.buttonPost.TabIndex = 42;
            this.buttonPost.Text = "Single post";
            this.toolTip1.SetToolTip(this.buttonPost, "Make a single post using a random proxy. (always uses given message)");
            this.buttonPost.UseVisualStyleBackColor = true;
            this.buttonPost.Click += new System.EventHandler(this.buttonPost_Click);
            // 
            // buttonPurge
            // 
            this.buttonPurge.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.buttonPurge.Location = new System.Drawing.Point(321, 76);
            this.buttonPurge.Name = "buttonPurge";
            this.buttonPurge.Size = new System.Drawing.Size(157, 23);
            this.buttonPurge.TabIndex = 44;
            this.buttonPurge.Text = "Clear Proxies";
            this.toolTip1.SetToolTip(this.buttonPurge, "Purge the current proxy list so you can load a new one.");
            this.buttonPurge.UseVisualStyleBackColor = true;
            this.buttonPurge.Click += new System.EventHandler(this.buttonPurge_Click);
            // 
            // buttonDownload
            // 
            this.buttonDownload.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDownload.Location = new System.Drawing.Point(321, 104);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.Size = new System.Drawing.Size(157, 67);
            this.buttonDownload.TabIndex = 54;
            this.buttonDownload.Text = "Download Posts";
            this.toolTip1.SetToolTip(this.buttonDownload, "Fetches the latest posts from the boardlist and reloads the post list.");
            this.buttonDownload.UseVisualStyleBackColor = true;
            this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);
            // 
            // buttonClearCap
            // 
            this.buttonClearCap.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.buttonClearCap.Location = new System.Drawing.Point(321, 52);
            this.buttonClearCap.Name = "buttonClearCap";
            this.buttonClearCap.Size = new System.Drawing.Size(157, 23);
            this.buttonClearCap.TabIndex = 58;
            this.buttonClearCap.Text = "Clear Captchas";
            this.toolTip1.SetToolTip(this.buttonClearCap, "Remove All Captchas");
            this.buttonClearCap.UseVisualStyleBackColor = true;
            this.buttonClearCap.Click += new System.EventHandler(this.buttonClearCap_Click);
            // 
            // openFileDialogPicture
            // 
            this.openFileDialogPicture.Filter = "Image Files(*.BMP;*.JPG;*.PNG;*.GIF)|*.BMP;*.JPG;*.PNG;*.GIF";
            this.openFileDialogPicture.Title = "Select image";
            // 
            // checkBoxImage
            // 
            this.checkBoxImage.AutoSize = true;
            this.checkBoxImage.Checked = true;
            this.checkBoxImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxImage.Location = new System.Drawing.Point(216, 286);
            this.checkBoxImage.Name = "checkBoxImage";
            this.checkBoxImage.Size = new System.Drawing.Size(61, 17);
            this.checkBoxImage.TabIndex = 41;
            this.checkBoxImage.Text = "Image?";
            this.checkBoxImage.UseVisualStyleBackColor = true;
            // 
            // checkBoxCheck
            // 
            this.checkBoxCheck.AutoSize = true;
            this.checkBoxCheck.Location = new System.Drawing.Point(146, 286);
            this.checkBoxCheck.Name = "checkBoxCheck";
            this.checkBoxCheck.Size = new System.Drawing.Size(63, 17);
            this.checkBoxCheck.TabIndex = 40;
            this.checkBoxCheck.Text = "Check?";
            this.checkBoxCheck.UseVisualStyleBackColor = true;
            // 
            // checkBoxSage
            // 
            this.checkBoxSage.AutoSize = true;
            this.checkBoxSage.Location = new System.Drawing.Point(9, 286);
            this.checkBoxSage.Name = "checkBoxSage";
            this.checkBoxSage.Size = new System.Drawing.Size(65, 17);
            this.checkBoxSage.TabIndex = 39;
            this.checkBoxSage.Text = "NSage?";
            this.checkBoxSage.UseVisualStyleBackColor = true;
            // 
            // checkBoxRandImg
            // 
            this.checkBoxRandImg.AutoSize = true;
            this.checkBoxRandImg.Checked = true;
            this.checkBoxRandImg.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxRandImg.Location = new System.Drawing.Point(74, 286);
            this.checkBoxRandImg.Name = "checkBoxRandImg";
            this.checkBoxRandImg.Size = new System.Drawing.Size(72, 17);
            this.checkBoxRandImg.TabIndex = 38;
            this.checkBoxRandImg.Text = "Rnd Img?";
            this.checkBoxRandImg.UseVisualStyleBackColor = true;
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(331, 357);
            this.pictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(71, 59);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 37;
            this.pictureBox.TabStop = false;
            // 
            // buttonPic
            // 
            this.buttonPic.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonPic.Location = new System.Drawing.Point(407, 357);
            this.buttonPic.Name = "buttonPic";
            this.buttonPic.Size = new System.Drawing.Size(74, 59);
            this.buttonPic.TabIndex = 36;
            this.buttonPic.Text = "Choose picture";
            this.buttonPic.UseVisualStyleBackColor = true;
            this.buttonPic.Click += new System.EventHandler(this.buttonPic_Click);
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.BackColor = System.Drawing.Color.White;
            this.textBoxMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxMessage.Location = new System.Drawing.Point(10, 357);
            this.textBoxMessage.Multiline = true;
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.Size = new System.Drawing.Size(233, 59);
            this.textBoxMessage.TabIndex = 35;
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.InitialImage = null;
            this.pictureBoxLogo.Location = new System.Drawing.Point(161, 3);
            this.pictureBoxLogo.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(115, 66);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLogo.TabIndex = 43;
            this.pictureBoxLogo.TabStop = false;
            // 
            // pictureBoxKeion
            // 
            this.pictureBoxKeion.InitialImage = null;
            this.pictureBoxKeion.Location = new System.Drawing.Point(21, 3);
            this.pictureBoxKeion.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxKeion.Name = "pictureBoxKeion";
            this.pictureBoxKeion.Size = new System.Drawing.Size(57, 66);
            this.pictureBoxKeion.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxKeion.TabIndex = 45;
            this.pictureBoxKeion.TabStop = false;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelName.Location = new System.Drawing.Point(7, 331);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(35, 13);
            this.labelName.TabIndex = 46;
            this.labelName.Text = "Name";
            // 
            // textBoxName
            // 
            this.textBoxName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxName.Location = new System.Drawing.Point(48, 328);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(98, 20);
            this.textBoxName.TabIndex = 47;
            // 
            // textBoxTimer
            // 
            this.textBoxTimer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxTimer.Location = new System.Drawing.Point(233, 328);
            this.textBoxTimer.MaxLength = 2;
            this.textBoxTimer.Name = "textBoxTimer";
            this.textBoxTimer.Size = new System.Drawing.Size(19, 20);
            this.textBoxTimer.TabIndex = 48;
            this.textBoxTimer.Text = "5";
            // 
            // labelTimer
            // 
            this.labelTimer.AutoSize = true;
            this.labelTimer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelTimer.Location = new System.Drawing.Point(152, 331);
            this.labelTimer.Name = "labelTimer";
            this.labelTimer.Size = new System.Drawing.Size(79, 13);
            this.labelTimer.TabIndex = 49;
            this.labelTimer.Text = "Post timer (sec)";
            // 
            // buttonRemove
            // 
            this.buttonRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonRemove.Location = new System.Drawing.Point(376, 326);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(97, 22);
            this.buttonRemove.TabIndex = 51;
            this.buttonRemove.Text = "Remove proxy";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // textBoxRemove
            // 
            this.textBoxRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxRemove.Location = new System.Drawing.Point(272, 327);
            this.textBoxRemove.Name = "textBoxRemove";
            this.textBoxRemove.Size = new System.Drawing.Size(98, 20);
            this.textBoxRemove.TabIndex = 52;
            // 
            // checkBoxDot
            // 
            this.checkBoxDot.AutoSize = true;
            this.checkBoxDot.Location = new System.Drawing.Point(285, 286);
            this.checkBoxDot.Name = "checkBoxDot";
            this.checkBoxDot.Size = new System.Drawing.Size(74, 17);
            this.checkBoxDot.TabIndex = 53;
            this.checkBoxDot.Text = "Dotspam?";
            this.checkBoxDot.UseVisualStyleBackColor = true;
            // 
            // checkBoxQuote
            // 
            this.checkBoxQuote.AutoSize = true;
            this.checkBoxQuote.Location = new System.Drawing.Point(419, 286);
            this.checkBoxQuote.Name = "checkBoxQuote";
            this.checkBoxQuote.Size = new System.Drawing.Size(61, 17);
            this.checkBoxQuote.TabIndex = 55;
            this.checkBoxQuote.Text = "Quote?";
            this.checkBoxQuote.UseVisualStyleBackColor = true;
            this.checkBoxQuote.Visible = false;
            // 
            // textBoxQuote
            // 
            this.textBoxQuote.Location = new System.Drawing.Point(420, 284);
            this.textBoxQuote.Name = "textBoxQuote";
            this.textBoxQuote.Size = new System.Drawing.Size(60, 20);
            this.textBoxQuote.TabIndex = 56;
            this.textBoxQuote.Visible = false;
            // 
            // checkBox50
            // 
            this.checkBox50.AutoSize = true;
            this.checkBox50.Location = new System.Drawing.Point(361, 286);
            this.checkBox50.Name = "checkBox50";
            this.checkBox50.Size = new System.Drawing.Size(55, 17);
            this.checkBox50.TabIndex = 57;
            this.checkBox50.Text = "50/50";
            this.checkBox50.UseVisualStyleBackColor = true;
            // 
            // checkBoxNew
            // 
            this.checkBoxNew.AutoSize = true;
            this.checkBoxNew.Location = new System.Drawing.Point(9, 305);
            this.checkBoxNew.Name = "checkBoxNew";
            this.checkBoxNew.Size = new System.Drawing.Size(85, 17);
            this.checkBoxNew.TabIndex = 59;
            this.checkBoxNew.Text = "New Thread";
            this.checkBoxNew.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(484, 521);
            this.Controls.Add(this.checkBoxNew);
            this.Controls.Add(this.checkBoxRandImg);
            this.Controls.Add(this.buttonClearCap);
            this.Controls.Add(this.checkBox50);
            this.Controls.Add(this.checkBoxQuote);
            this.Controls.Add(this.buttonDownload);
            this.Controls.Add(this.checkBoxDot);
            this.Controls.Add(this.textBoxRemove);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.labelTimer);
            this.Controls.Add(this.textBoxTimer);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.pictureBoxKeion);
            this.Controls.Add(this.buttonPurge);
            this.Controls.Add(this.pictureBoxLogo);
            this.Controls.Add(this.buttonPost);
            this.Controls.Add(this.checkBoxImage);
            this.Controls.Add(this.checkBoxCheck);
            this.Controls.Add(this.checkBoxSage);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.buttonPic);
            this.Controls.Add(this.textBoxMessage);
            this.Controls.Add(this.buttonSaveProxies);
            this.Controls.Add(this.labelAttempts);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.labelPosts);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.buttonAbort);
            this.Controls.Add(this.labelCaptchaError);
            this.Controls.Add(this.buttonGO);
            this.Controls.Add(this.buttonLoadProxies);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxThread);
            this.Controls.Add(this.labelProxyCount);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.status);
            this.Controls.Add(this.textBoxCaptcha);
            this.Controls.Add(this.labelCaptchaCount);
            this.Controls.Add(this.pictureBoxCaptcha);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxQuote);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(500, 560);
            this.MinimumSize = new System.Drawing.Size(500, 560);
            this.Name = "Form1";
            this.Text = "DelayClose";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCaptcha)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxKeion)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

    }
}

