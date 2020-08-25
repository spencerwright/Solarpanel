using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace parser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Setting up names for buttons and paths for files
            textBox1.Text = @"C:\Users\Spencer Wright\Desktop\Lrad20200810.txt";
            button1.Text = "Parse";
            textBox2.Text = @"C:\Users\Spencer Wright\Desktop\Test.txt";
            textBox3.Text = "VBatt:";
            button2.Text = "4 hour shift";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (File.Exists(textBox2.Text) == true)
            {
                // Cheching to see if the file exist 
                DialogResult button = MessageBox.Show("Do you want to overwrite text file?", "Warning", MessageBoxButtons.OKCancel);
                if (button != DialogResult.OK)
                {
                    return;
                }
            }
            Console.WriteLine("Parse started");
            ReadFile(textBox1.Text);
            Console.WriteLine("Parse Complete");
            MessageBox.Show("Parse Complete", "Done", MessageBoxButtons.OKCancel);
        }
        public void ReadFile(string fileName)
        {
            string data = "";
            List<string> one = new List<string>();
            try
            {
                string[] lines = System.IO.File.ReadAllLines(fileName);
                //Itterates through each line in the text file
                foreach (string line in lines)
                {
                    if( line.Contains("PowerModule: Status:") == false)
                    {
                        // This line does not contain Powermodule: then continue to the next line
                        continue;
                    }

                    string[] words = line.Split(' ');
                    // Itterates through each word in the line 
                    for (int index = 0; index < words.Length; index++)
                    {
                        string word = words[index];                        
                        if (word == "State:")
                        {
                            // Checks to see if word is State: to then find ceratin information in the line       
                            bool found = false;
                            int i = index + 1;
                            // Itterates through each word in the line from the text file 
                            while (i< words.Length)
                            {
                                string element = words[i];
                                if (element.Contains("Solar_Charging"))
                                {
                                    // Checks to see if Solar_Charging is in the line to then append green
                                    index = i;
                                    data += "green ";
                                    found = true;
                                    break;
                                }
                                else if (word == "VBatt:")
                                {
                                    // Checks for VBatt: to then stop the loop because Solar_Charging will never be after VBatt:
                                    index = i - 1;
                                    break;
                                }
                                i++;                                
                            }
                            if (found == false)
                            {
                                // If Solar_Charging is not found in the line then red is appended to the data string
                                data += "red ";
                            }
      
                        }
                        else if (word =="VBatt:" )
                        {
                            // Checks to see if word is VBatt: to then grab the next element
                            string value = words[index + 1];
                            data += value;
                            data = data.Substring(1,data.Length-2);
                            one.Add(data);
                            data = "";
                        }
                        
                        else if (index == 0)
                        {
                            // Chech the first element of the line to get the date and format it
                            string date = words[index];
                            string[] value = date.Split(',');
                            DateTime original = DateTime.Parse(value[0]);
                            date = original.ToString("MM/dd/yyyy H:mm");
                            data += " ";
                            data += date + " ";
                            string time = words[index + 1];
                            data += time + " ";
                        }
                    }
                }
            }
            
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            WriteToText(one);
        }

        public void WriteToText(List<string> value)
        {
            // When called willt take the list of strings and write to the specified text file 
            string filename = textBox2.Text;
            try
            {
                System.IO.File.AppendAllLines(filename, value);
                
            }
            
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (File.Exists(textBox2.Text) == true)
            {
                // Cheching to see if the file exist 
                DialogResult button = MessageBox.Show("Do you want to overwrite text file?", "Warning", MessageBoxButtons.OKCancel);
                if (button != DialogResult.OK)
                {
                    return;
                }
            }
            Console.WriteLine("Parse started");
            TimeFile(textBox1.Text);
            Console.WriteLine("Parse Complete");
            MessageBox.Show("Parse Complete", "Done", MessageBoxButtons.OKCancel);
        }
        public void TimeFile(string fileName)
        {
            // Rewrites the time in the text file to be four hours behind
            List<string> one = new List<string>();
            try
            {
                string[] lines = System.IO.File.ReadAllLines(fileName);
                // Itterates through each line in the text file to find the time and format it the correct way
                foreach (string line in lines)
                {
                    string[] words = line.Split(' ');
                    string setup = words[0] +" " +words[1];
                    DateTime original = DateTime.Parse(setup);
                    DateTime update = original.Add(new TimeSpan(-4, 0, 0));
                    string data = update.ToString("MM/dd/yyyy HH:mm");
                    string power = " "+words[2]+" "+words[3]+" "+words[4];
                    data += power;
                    one.Add(data);                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            WriteToText(one);
        }
    }
}
    
    