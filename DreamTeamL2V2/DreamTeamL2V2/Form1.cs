using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DreamTeamL2V2
{
    public partial class Form1 : Form
    {
        // число "рабочих"
        private int n = 2;  

        // ключ для шифрования/дешифрования
        private string key;

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
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Debug.Assert(int.TryParse(textBox1.Text, out n), "Некорректный ввод числа рабочих");
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
            if (comboBox1.Text == "")
                MessageBox.Show("Выберите шифр");
            else
            {
                Debug.Assert(((textBox3.Text != "") || (label4.Text != "Файл не выбран")), "Введите текст для дешифрования");
                Debug.Assert(textBox2.Text != "", "Введите ключ");
                Debug.Assert(int.Parse(textBox1.Text) > 0, "Число рабочих должно быть больше нуля");

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
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
                MessageBox.Show("Выберите шифр");
            else
            {
                Debug.Assert(((textBox3.Text != "") || (label4.Text != "Файл не выбран")), "Введите текст для дешифрования");
                Debug.Assert(textBox2.Text != "", "Введите ключ");
                Debug.Assert(int.Parse(textBox1.Text) > 0, "Число рабочих должно быть больше нуля");

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
