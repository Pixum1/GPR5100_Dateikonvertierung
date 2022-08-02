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
        string convertedFileContent;

        public MainWindow()
        {
            InitializeComponent();
        }

        private string GetFileContent()
        {
            try
            {
                using (StreamReader reader = new StreamReader(selectedFilePath))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception _e)
            {
                ShowError(_e.Message);
                return _e.Message;
            }
        }
        private void WriteContentToFile(string _content, string _path)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(File.OpenWrite(_path), Encoding.Default))
                {
                    writer.WriteLine(_content);
                }
            }
            catch (Exception _e)
            {
                ShowError(_e.Message);
            }
        }

        private byte[] ConvertStringToByteArray(string _string)
        {
            return Encoding.Default.GetBytes(_string);

            //UTF32Encoding encoder = new UTF32Encoding();
            //return encoder.GetBytes(_string);
        }
        private string ConvertByteArrayToString(byte[] _bytes)
        {
            return Encoding.Default.GetString(_bytes);

            //UTF32Encoding encoder = new UTF32Encoding();
            //return encoder.GetString(_bytes);
        }

        private void OnClick_Convert(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(selectedFilePath)) return;

                convertedFileContent = ConvertByteArrayToString(ConvertStringToByteArray(GetFileContent()));

                MessageBox.Show("Succesfully converted");
            }
            catch (Exception _e)
            {
                ShowError(_e.Message);
            }
        }

        private void OnClick_SaveAs(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.SaveFileDialog saveDlg = new Microsoft.Win32.SaveFileDialog();
                saveDlg.Filter = "TXT Files (.txt)|*.txt|PNG Files (.png)|*.png";
                saveDlg.ShowDialog();

                WriteContentToFile(convertedFileContent, saveDlg.FileName);
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
                Microsoft.Win32.OpenFileDialog openDlg = new Microsoft.Win32.OpenFileDialog();
                openDlg.Filter = "TXT Files (.txt)|*.txt|PNG Files (.png)|*.png";
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
