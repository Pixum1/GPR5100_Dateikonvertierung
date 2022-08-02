using System;
using System.IO;
using System.Text;
using System.Windows;

namespace GPR5100_Dateikonvertierung
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string selectedFilePath;
        string fileFormat = "All Files|*.*";

        public MainWindow()
        {
            InitializeComponent();
        }
        private void OnClick_SaveAs(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.SaveFileDialog saveDlg = new Microsoft.Win32.SaveFileDialog();

                if (Path.GetExtension(selectedFilePath) == ".txt")
                    saveDlg.Filter = fileFormat;
                else
                    saveDlg.Filter = "TXT Files|*.txt";

                saveDlg.ShowDialog();

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

                if (Path.GetExtension(saveDlg.FileName) == ".txt")
                {
                    string msg = Convert.ToBase64String(File.ReadAllBytes(selectedFilePath));
                    File.WriteAllText(saveDlg.FileName, msg);
                }
                else
                {
                    byte[] data = Convert.FromBase64String(File.ReadAllText(selectedFilePath));

                    using (FileStream fs = new FileStream(saveDlg.FileName, FileMode.OpenOrCreate))
                    {
                        fs.Write(data, 0, data.Length);
                        fs.Close();
                    }
                }

                MessageBox.Show($"Succesfully converted from {Path.GetExtension(selectedFilePath)} to {Path.GetExtension(saveDlg.FileName)}");
            }
            catch (Exception _e)
            {
                ShowError(_e.Message);
            }
        }

        byte[] GetStringData(string _string)
        {
            return Convert.FromBase64String(_string);
        }

        private void OnClick_SelectFile(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog openDlg = new Microsoft.Win32.OpenFileDialog();
                openDlg.Filter = fileFormat;
                openDlg.ShowDialog();

                //if (!(System.IO.Path.GetExtension(openDlg.FileName).ToLower() == ".txt")) return;

                selectedFilePath = openDlg.FileName;
                Txt_SelectedPath.Text = selectedFilePath;
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
