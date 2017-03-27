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
        private int n;  

        // ключ для шифрования/дешифрования
        private string key;

        // шифр
        private string cipherType;

        // текст для шифрования
        private string plainText;   

        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Debug.Assert(int.TryParse(textBox1.Text, out n));
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
            MessageBox.Show("Вызов шифрования");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Вызов дешифрования");
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            plainText = textBox3.Text;
        }


    }
}
