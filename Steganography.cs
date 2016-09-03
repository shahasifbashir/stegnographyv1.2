using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.IO;
using System.Speech.Synthesis;
using System.Threading;
namespace Steganography
{
    public partial class Steganography : Form
    {
        /// <summary>
        /// this creates an object for speech systhisizer 
        /// </summary>
        private static SpeechSynthesizer synth = new SpeechSynthesizer();
        /// <summary>
        /// the are the threads will be created to run the speech in parallel with text
        /// the multiple threads are created for different messages 
        /// these  make use of  lambada expresions to take an argument 
        /// the system has a bug 
        /// the threads can not be used multiple times
        /// </summary>
        Thread t = new Thread((x)=>mesg(x));
        public Steganography()
        {
            InitializeComponent();
            t.Start("welcome to the stegnography version one point two");
  
        }

        Bitmap bmp = null;
       

        string extractedText = string.Empty;

        private void hideButton_Click(object sender, EventArgs e)
        {
            bmp = (Bitmap)imagePictureBox.Image;

            string text = dataTextBox.Text;

            if (text.Equals(""))
            {
                try
                {
                    Thread t1 = new Thread((x) => mesg(x));
                    t1.Start("The text you want to hide can not be empty");
            
                }
                finally
                {
                    MessageBox.Show("The text you want to hide can't be empty", "Warning");
                }
                return;
            }

            if (encryptCheckBox.Checked)
            {
                if (passwordTextBox.Text.Length < 5)
                {
                    try
                    {
                        Thread t2 = new Thread((x) => mesg(x));
                        t2.Start("Please enter a password with at least six characters ");
                    }
                    finally
                    {
                        MessageBox.Show("Please enter a password with at least 6 characters", "Warning");
                    }
                    return;
                }
                else
                {
                    text = Crypto.EncryptStringAES(text, passwordTextBox.Text);
                }
            }

            bmp = SteganographyHelper.embedText(text, bmp);
            try
            {
                Thread t3 = new Thread((x) => mesg(x));
                t3.Start("Your text was hidden in the image successfully! do not forget to save the image man");
            }
            finally
            {
                MessageBox.Show("Your text was hidden in the image successfully!", "Done");
            }
            notesLabel.Text = "Notes: don't forget to save your new image.";
            notesLabel.ForeColor = Color.OrangeRed;
        }

        private void extractButton_Click(object sender, EventArgs e)
        {
            bmp = (Bitmap)imagePictureBox.Image;

            string extractedText = SteganographyHelper.extractText(bmp);

            if (encryptCheckBox.Checked)
            {
                try
                {
                    extractedText = Crypto.DecryptStringAES(extractedText, passwordTextBox.Text);
                }
                catch
                {
                    try
                    {
                        Thread t4 = new Thread((x) => mesg(x));
                        t4.Start("wrong password please enter a valid password");
                    }
                    finally
                    {
                        MessageBox.Show("Wrong password", "Error");
                      
                    }
                    return;
                }
            }

            dataTextBox.Text = extractedText;
        }

        private void imageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open_dialog = new OpenFileDialog();
            open_dialog.Filter = "Image Files (*.jpeg; *.png; *.bmp)|*.jpg; *.png; *.bmp";

            if (open_dialog.ShowDialog() == DialogResult.OK)
            {
                imagePictureBox.Image = Image.FromFile(open_dialog.FileName);
            }
        }

        private void imageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save_dialog = new SaveFileDialog();
            save_dialog.Filter = "Png Image|*.png|Bitmap Image|*.bmp";

            if (save_dialog.ShowDialog() == DialogResult.OK)
            {
                switch (save_dialog.FilterIndex)
                {
                    case 0:
                        {
                            bmp.Save(save_dialog.FileName, ImageFormat.Png);
                        }break;
                    case 1:
                        {
                            bmp.Save(save_dialog.FileName, ImageFormat.Bmp);
                        } break;
                }

                notesLabel.Text = "Notes:";
                notesLabel.ForeColor = Color.Black;
            }
        }

        private void textToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save_dialog = new SaveFileDialog();
            save_dialog.Filter = "Text Files|*.txt";

            if (save_dialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(save_dialog.FileName, dataTextBox.Text);
            }
        }

        private void textToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open_dialog = new OpenFileDialog();
            open_dialog.Filter = "Text Files|*.txt";

            if (open_dialog.ShowDialog() == DialogResult.OK)
            {
                dataTextBox.Text = File.ReadAllText(open_dialog.FileName);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread t5 = new Thread((x) => mesg(x));
            t5.Start("by shah aaasif bashir");
            MessageBox.Show("By: Shah Asif Bashir", "About");
            //synth.Speak("by shah asif bashir");
        }
        /// <summary>
        /// this is the function which is used to speak things out 
        /// </summary>
        /// <param name="message"></param>
        private static void mesg(object message)
        {
            string m = Convert.ToString(message);
            synth.Speak(m);
                    
        }

    }

}