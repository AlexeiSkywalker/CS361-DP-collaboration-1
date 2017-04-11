using System;
using System.Windows.Forms;
using System.IO;

namespace DreamTeamL2V2
{
    public partial class Form1 : Form
    {
        // число "рабочих"
        private int n = 2;  

        // ключ для шифрования/дешифрования
        private string key = "";

        // шифр
        private string cipherType;

        // текст для шифрования
        private string plainText;

        private string fileName = "";

        private bool firstUsing = true;

        private bool textBox4Changed = false;

        public Form1()
        {
            InitializeComponent();
            Tests.runTests();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cipherType = comboBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            key = textBox2.Text;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if ((textBox3.Text == "") && (label4.Text == "Файл не выбран") || firstUsing)
            {
                MessageBox.Show("Введите текст для шифрования");
                return;
            }
            if (!int.TryParse(textBox1.Text, out n))
            {
                MessageBox.Show("Некорректное число рабочих");
                return;
            }
            if (n <= 0)
            {
                MessageBox.Show("Число рабочих должно быть больше нуля");
                return;
            }

            CipherType c;
            if (cipherType == "DES")
                c = CipherType.DES;
            else
                c = CipherType.AES;
            if (fileName != "")
            {
                FileCryptor.EncryptFile(fileName, key, c, n);
                fileName = "";
                label4.Text = "Файл не выбран";
             }
             else
                textBox4.Text = ParallelCipher.Encrypt(plainText, key, c, n);
            
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if ((textBox3.Text == "") && (label4.Text == "Файл не выбран") || firstUsing)
            {
                MessageBox.Show("Введите текст для дешифрования");
                return;
            }
            if (!int.TryParse(textBox1.Text, out n))
            {
                MessageBox.Show("Некорректное число рабочих");
                return;
            }
            if (n <= 0)
            {
                MessageBox.Show("Число рабочих должно быть больше нуля");
                return;
            }

            CipherType c;
            if (cipherType == "DES")
                c = CipherType.DES;
            else
                c = CipherType.AES;
            if (fileName != "")
            {
                FileCryptor.DecryptFile(fileName, key, c, n);
                fileName = "";
                label4.Text = "Файл не выбран";
             }
             else
                textBox4.Text = ParallelCipher.Decrypt(plainText, key, c, n);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            plainText = textBox3.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fileName = ofd.FileName;
            }
            label4.Text = fileName;
        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            if (firstUsing)
            {
                firstUsing = !firstUsing;
                textBox3.Text = "";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox4Changed)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.CreatePrompt = true;
                saveFileDialog.OverwritePrompt = true;
                saveFileDialog.FileName = "file";
                saveFileDialog.DefaultExt = "txt";
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                DialogResult result = saveFileDialog.ShowDialog();

                StreamWriter fileStream;

                if (result == DialogResult.OK)
                {
                    fileStream = new System.IO.StreamWriter(saveFileDialog.OpenFile());
                    fileStream.Write(textBox4.Text);
                    fileStream.Close();
                }
                textBox4Changed = false;
            }
            else
                MessageBox.Show("Не было сделано изменений для записи в файл");
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            textBox4Changed = true;
        }
    }
}
