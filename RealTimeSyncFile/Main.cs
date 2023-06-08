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

namespace RealTimeSyncFile
{
	public partial class Main : Form
	{
		public Main()
		{
			InitializeComponent();
		}

		List<string> files = new List<string>();
		//临时文件
		string infopath = Directory.GetCurrentDirectory() + "\\RealTimeSynFileTemp.txt";

		/// <summary>
		/// 计时器 分析当前文件时间
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timer1_Tick(object sender, EventArgs e)
		{
			listBox1.SelectedIndex = listBox1.Items.Count - 1;
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.fileway = textBox1.Text;
			Properties.Settings.Default.Save();
		}

		/// <summary>
		///  窗体启动实例
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_Load(object sender, EventArgs e)
		{
			textBox1.Text = Properties.Settings.Default.fileway;


			if (!File.Exists(infopath))
			{
				File.AppendAllText(infopath, "");
			}
			else
			{
				File.Delete(Directory.GetCurrentDirectory() + "\\RealTimeSynFileTemp.txt");
			}
		}

		/// <summary>
		/// 获取网盘文件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void button1_Click(object sender, EventArgs e)
		{
			files.Clear();
			listBox1.Items.Add("开始获取文件");
			string info = "";
			string path = textBox1.Text;
			//获取网盘的所有路径，并且将信息写入到日志内
			DateTime KS = DateTime.Now;
			await Task.Run(() =>
			{
				files.AddRange(Directory.GetFiles(textBox1.Text, "*", SearchOption.AllDirectories));
				for (int i = 0; i < files.Count; i++)
				{
					info = File.GetLastWriteTime(files[i]).ToString();
					info += "=" + files[i].ToString().Replace(path, "") + "\n";
					File.AppendAllText(infopath, info);
				}
			});
			DateTime JS = DateTime.Now;
			TimeSpan T = JS - KS;
			listBox1.Items.Add("文件目录获取成功,耗时 " + T);
			listBox1.Items.Add("文件数量 " + files.Count);
		}
	}
}
