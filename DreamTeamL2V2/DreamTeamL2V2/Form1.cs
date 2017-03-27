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
    }
}
