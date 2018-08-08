using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DebugTools
{
    public partial class FormCustomConsole : Form
    {
        delegate void SetTextCallback(string text);
        public static bool isActive = false;
        public static FormCustomConsole FormCustomConsolePtr = null;

        private const int texboxMaxChars = 600000; // If texbox has this number of chars, delete oldest ones
        private const int texboxDeleteOffset = 1000; // It will delete this number of chars + how much it needs to display whole event


        public FormCustomConsole()
        {
            InitializeComponent();
            isActive = true;
            FormCustomConsolePtr = this; // Static pointer to last opened instance
        }

        public static void Write(string text) // Add text to console from every object thru static method
        {
            if (isActive) // If active, update text
            {
                FormCustomConsolePtr.UpdateText(text);
            }
        }

        public static void WriteLine(string text) // Add text to console from every object thru static method
        {
            if (isActive) // If active, update text
            {
                FormCustomConsolePtr.UpdateTextLn(text);
            }
        }

        public static void WriteLineWithConsole(string text) // Add text to console from every object thru static method
        {
            if (isActive) // If active, update text
            {
                FormCustomConsolePtr.UpdateTextLn(text);
            }
            Console.WriteLine(text);
        }

        public static void WriteWithConsole(string text) // Add text to console from every object thru static method
        {
            if (isActive) // If active, update text
            {
                FormCustomConsolePtr.UpdateText(text);
            }
            Console.Write(text);
        }

        private void FormCustomConsole_FormClosed(object sender, FormClosedEventArgs e)
        {
            isActive = false;
        }

        private void UpdateText(string text) // This method must not be static, and it ensures that all threads can manipulate it
        {
            if (this.textBoxConsole.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(Write);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.textBoxConsole.Text += text;
            }
        }

        private void UpdateTextLn(string text) // This method must not be static, and it ensures that all threads can manipulate it
        {
            if (this.textBoxConsole.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(WriteLine);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.textBoxConsole.Text += text + "\r\n";
            }
        }

        private void textBoxConsole_TextChanged(object sender, EventArgs e)
        {
            int ptr;
            if (textBoxConsole.Text.Length > texboxMaxChars)
            {
                textBoxConsole.Text = textBoxConsole.Text.Substring(texboxDeleteOffset);
                ptr = textBoxConsole.Text.IndexOf(Environment.NewLine); // Make a clean cut, from newline
                textBoxConsole.Text = textBoxConsole.Text.Substring(ptr + 1);
            }
            // Make sure we are showing last line in textbox
            textBoxConsole.SelectionStart = textBoxConsole.Text.Length;
            textBoxConsole.ScrollToCaret();

        }

        private void buttonDiag_Click(object sender, EventArgs e)
        {
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBoxConsole.Clear();
        }
    }
}
