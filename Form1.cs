using System.Diagnostics;

namespace FileManager
{
    public partial class Form1 : Form
    {
        private string _filePath = "C:";
        private bool _isFile = false;
        private string _currentlySelectedItemName = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            filePathTextBox.Text = _filePath;
            LoadFilesAndDirectories();
        }

        public void LoadFilesAndDirectories()
        {
            try
            {
                //check is whether the directory is File
                FileAttributes fileAttr;
                if (_isFile)
                {
                    string tempFilePath = _filePath + "/" + _currentlySelectedItemName;
                    FileInfo fileDetails = new(tempFilePath);
                    fileNameLabel.Text = fileDetails.Name;
                    fileTypeLabel.Text = fileDetails.Extension;
                    fileAttr = File.GetAttributes(tempFilePath);
                    Process.Start(tempFilePath);
                }
                else
                {
                    fileAttr = File.GetAttributes(_filePath);

                }

                if ((fileAttr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    var fileList = new DirectoryInfo(_filePath);
                    FileInfo[] files = fileList.GetFiles(); // Get all files
                    DirectoryInfo[] dirs = fileList.GetDirectories(); // Get all directories

                    //We clear the listview so that the program will update list's content and won't show redundacies
                    listView1.Items.Clear();

                    foreach (var t in files)
                    {
                        string fileExtension = t.Extension.ToUpper();
                        switch (fileExtension)
                        {
                            case ".MP3":
                            case ".MP2":
                                listView1.Items.Add(t.Name, 5);
                                break;
                            case ".EXE":
                            case ".COM":
                                listView1.Items.Add(t.Name, 7);
                                break;

                            case ".MP4":
                            case ".AVI":
                            case ".MKV":
                                listView1.Items.Add(t.Name, 6);
                                break;
                            case ".PDF":
                                listView1.Items.Add(t.Name, 4);
                                break;
                            case ".DOC":
                            case ".DOCX":
                                listView1.Items.Add(t.Name, 3);
                                break;
                            case ".PNG":
                            case ".JPG":
                            case ".JPEG":
                                listView1.Items.Add(t.Name, 9);
                                break;

                            default:
                                listView1.Items.Add(t.Name, 8);
                                break;
                        }
                    }

                    foreach (var t in dirs)
                    {
                        listView1.Items.Add(t.Name, 10);
                    }
                }
                else
                {
                    fileNameLabel.Text = this._currentlySelectedItemName;
                }
            }
            catch (Exception e)
            {

            }
        }

        public void LoadButtonAction()
        {
            RemoveBackSlash();
            _filePath = filePathTextBox.Text;
            LoadFilesAndDirectories();
            _isFile = false;
        }

        //This function is called to remove any unnecessary backslashes at the end of the path to avoid errors
        public void RemoveBackSlash()
        {
            string path = filePathTextBox.Text;
            if (path.LastIndexOf("/") == path.Length - 1)
            {
                if (path.Length > 0) filePathTextBox.Text = path.Substring(0, path.Length - 1);
            }
        }

        //This function is returns back to the previous directory of the current directory by removing the text in filePathTextBox
        public void GoBack()
        {
            try
            {
                RemoveBackSlash();
                string path = filePathTextBox.Text;
                path = path.Substring(0, path.LastIndexOf("/"));
                this._isFile = false;
                filePathTextBox.Text = path;
                RemoveBackSlash();
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            LoadButtonAction();
        }

        //This Function first: applies the current directory path to the listView1; second: determines whether the current selected object is whether file or directory
        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            _currentlySelectedItemName = e.Item.Text;

            FileAttributes fileAttr = File.GetAttributes(_filePath + "/" + _currentlySelectedItemName);
            if ((fileAttr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                _isFile = false;
                filePathTextBox.Text = _filePath + "/" + _currentlySelectedItemName;
            }
            else
            {
                _isFile = true;
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            LoadButtonAction();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            GoBack();
            LoadButtonAction();
        }
    }
}