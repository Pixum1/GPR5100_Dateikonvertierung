using System;
using System.IO;
using System.Text;
using System.Windows;
using Microsoft.Win32;

namespace GPR5100_Dateikonvertierung
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string selectedFilePath;
        const string FILE_FORMAT = "All Files|*.*";

        public MainWindow()
        {
            InitializeComponent();
        }
        private void ConvertToTextFile(string _from, string _to)
        {
            string msg = Convert.ToBase64String(File.ReadAllBytes(_from));
            File.WriteAllText(_to, msg);
        }
        private void ConvertFromTextFile(string _from, string _to)
        {
            byte[] data = Convert.FromBase64String(File.ReadAllText(_from));

            using (FileStream fs = new FileStream(_to, FileMode.OpenOrCreate))
            {
                fs.Write(data, 0, data.Length);
                fs.Close();
            }
        }
        private void ApplyFilter(string _path, SaveFileDialog _filter)
        {
            if (Path.GetExtension(_path) == ".txt")
                _filter.Filter = FILE_FORMAT;
            else
                _filter.Filter = "TXT Files|*.txt";
        }

        private void OnClick_SaveAs(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveDlg = new SaveFileDialog();

                ApplyFilter(selectedFilePath, saveDlg);

                if (saveDlg.ShowDialog() == true)
                {
                    if (!Path.HasExtension(saveDlg.FileName))
                    {
                        ShowError("You have to choose a valid format!");
                        return;
                    }
                    if (Path.GetExtension(saveDlg.FileName) == Path.GetExtension(selectedFilePath))
                    {
                        ShowError("You can't convert to the same file format!");
                        return;
                    }

                    //Convert selected file to a new text file
                    if (Path.GetExtension(saveDlg.FileName) == ".txt")
                    {
                        ConvertToTextFile(selectedFilePath, saveDlg.FileName);
                    }
                    //Convert selected file to a new format
                    else
                    {
                        ConvertFromTextFile(selectedFilePath, saveDlg.FileName);
                    }

                    MessageBox.Show($"Succesfully converted from {Path.GetExtension(selectedFilePath)} to {Path.GetExtension(saveDlg.FileName)}");
                }
            }
            catch (Exception _e)
            {
                ShowError(_e.Message);
            }
        }

        private void OnClick_SelectFile(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openDlg = new OpenFileDialog();
                openDlg.Filter = FILE_FORMAT;

                if (openDlg.ShowDialog() == true)
                {
                    selectedFilePath = openDlg.FileName;
                    Txt_SelectedPath.Text = selectedFilePath;

                    Btn_SaveAs.IsEnabled = true;
                }
            }
            catch (Exception _e)
            {
                ShowError(_e.Message);
            }
        }
        private void ShowError(string _msg)
        {
            MessageBox.Show(_msg, "ERROR!");
        }
    }
}
