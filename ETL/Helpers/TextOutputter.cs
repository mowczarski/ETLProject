using System;
using System.IO;
using System.Text;
using System.Windows.Controls;

namespace ETL.Helpers
{
    public class TextOutputterr : TextWriter
    {
        TextBox textBox = null;

        public TextOutputterr(TextBox output)
        {
            textBox = output;
        }

        public override void Write(char value)
        {
            base.Write(value);
            textBox.Dispatcher.BeginInvoke(new Action(() =>
            {
                textBox.AppendText(value.ToString());
            }));
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}
