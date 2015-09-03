using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Hook;

namespace MouseChangeKey_鼠标改键
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //获取窗口句柄传递键盘消息
        [DllImport("USER32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int iParam);

        //声明托盘图标
        private System.Windows.Forms.NotifyIcon notifyIcon = null;
        System.Windows.Forms.ContextMenuStrip contextMenu = new System.Windows.Forms.ContextMenuStrip();
        System.Windows.Forms.ToolStripMenuItem item = new System.Windows.Forms.ToolStripMenuItem();
        public MainWindow()
        {
            InitializeComponent();
        }
        private SaveKeys _myKeys = new SaveKeys();
        private void Window_LButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void tbXButton1_KeyDown(object sender, KeyEventArgs e)
        {
            _myKeys.XButton1 = (Hook.Key)Enum.Parse(typeof(Hook.Key), e.Key.ToString());
            e.Handled = true;
           tbXButton1.Text = e.Key.ToString();
        }
        MouseHook mouseHook;
        KeyBoardHook keyBoardHook;
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
        	#region 设置托盘图标
	    	 this.ShowInTaskbar = false;
            //设置托盘的各个属性
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.Text = "Warcraft3 Key";
            notifyIcon.Icon = Properties.Resources.miniico;
            notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_MouseClick);
            notifyIcon.Visible = true;
            item.Text="退出";
            item.Click+= CloseClick;
            contextMenu.Items.Add(item);
            this.notifyIcon.ContextMenuStrip = contextMenu;
            #endregion
            mouseHook = new MouseHook();
            keyBoardHook = new KeyBoardHook();
            mouseHook.Flag=HookFlag.IsPaused;
            keyBoardHook.Flag=HookFlag.IsPaused;
            keyBoardHook.KeyDown += keyBoardHook_KeyDown;
            mouseHook.MouseEvent += MouseButtonEventProc;
            if(File.Exists(@"config.cfg"))
            {
	            using(FileStream fs = new FileStream(@"config.cfg", FileMode.Open))
			    {
			      BinaryFormatter formatter = new BinaryFormatter();
			      _myKeys = (SaveKeys)formatter.Deserialize(fs);//在这里大家要注意咯,他的返回值是object
			    }
            }
            InitializeTextBox();
            
        }
		
        private void notifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (this.Visibility == Visibility.Visible)
                {
                    this.Visibility = Visibility.Hidden;
                }
                else
                {
                    this.Visibility = Visibility.Visible;
                    this.Activate();
                }
            }
        }
        
        private bool keyBoardHook_KeyDown(object sender, KeyBoardHookEventArgs e)
        {
            int index = Array.IndexOf(_myKeys._leftkeys, (Hook.Key)e.Key);
            IntPtr war3 = FindWindow(null, "Warcraft III");
            if(war3==IntPtr.Zero)return false;
            if (index != -1)
            {
                SendMessage(war3, (uint)WM_KEYBOARD.WM_KEYDOWN, (int)_myKeys._rightkeys[index], 0);
                SendMessage(war3, (uint)WM_KEYBOARD.WM_KEYUP, (int)_myKeys._rightkeys[index], 0);
                return true;
            }
            else if((index=Array.IndexOf(_myKeys._leftResKeys,(Hook.Key)e.Key))!=-1)
            {
                SendMessage(war3, (uint)WM_KEYBOARD.WM_KEYDOWN, (int)_myKeys._rightResKeys[index], 0);
                SendMessage(war3, (uint)WM_KEYBOARD.WM_KEYUP, (int)_myKeys._rightResKeys[index], 0);
                return true;
            }
            else if((index=Array.IndexOf(_myKeys._myKaelKeys.KaelChangeKeys(),(Hook.Key)e.Key))!=-1)
            {
            	SendMessage(war3, (uint)WM_KEYBOARD.WM_KEYDOWN, (int)_myKeys._rightResKeys[index], 0);
                SendMessage(war3, (uint)WM_KEYBOARD.WM_KEYUP, (int)_myKeys._rightResKeys[index], 0);
                return true;
            }
            else if(e.Key==Hook.Keys.Enter)
            {
        		keyBoardHook.Flag=HookFlag.IsPaused;
        		return false;
            }
           return false;
        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (mouseHook != null)
            {
                mouseHook.Dispose();
            }
            FileStream fs=new FileStream(@"config.cfg",FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
      		formatter.Serialize(fs, _myKeys);
      		fs.Close();
        }
         IntPtr HWND_BROADCAST = new IntPtr(0xffff); 
        private bool MouseButtonEventProc(object sender, MouseHookEventArgs e)
        {
        	if(e.ChangedButton==MouseButtons.Right)
        	{
        		keyBoardHook.Flag=HookFlag.IsRunning;
        	}
        	if (e.ChangedButton == MouseButtons.XButton1)
            {
        		IntPtr war3 = FindWindow(null, "Warcraft III");
                SendMessage(war3, (uint)WM_KEYBOARD.WM_KEYDOWN, (int)_myKeys.XButton1, 0);
                SendMessage(war3, (uint)WM_KEYBOARD.WM_KEYUP, (int)_myKeys.XButton1, 0);
                return true;
            }
            else if (e.ChangedButton == MouseButtons.XButton2)
            {
                IntPtr war3 = FindWindow(null, "Warcraft III");
                SendMessage(war3, (uint)WM_KEYBOARD.WM_KEYDOWN, (int)_myKeys.XButton2, 0);
                SendMessage(war3, (uint)WM_KEYBOARD.WM_KEYUP, (int)_myKeys.XButton2, 0);
                return true;
            }
            else if (e.ChangedButton ==MouseButtons.Middle)
            {
                IntPtr war3 = FindWindow(null, "Warcraft III");
                SendMessage(war3, (uint)WM_KEYBOARD.WM_KEYDOWN, (int)_myKeys.MiddleButton, 0);
                SendMessage(war3, (uint)WM_KEYBOARD.WM_KEYUP, (int)_myKeys.MiddleButton, 0);
                return true;
            }
            return false;
        }

        private void tbXButton2_KeyDown(object sender, KeyEventArgs e)
        {
            _myKeys.XButton2 = (Hook.Key)Enum.Parse(typeof(Hook.Key), e.Key.ToString());
            e.Handled = true;
            tbXButton2.Text = e.Key.ToString();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {

			this.Visibility= Visibility.Hidden;
			if (mouseHook != null && keyBoardHook != null)
            {
                this.mouseHook.Flag = HookFlag.IsRunning;
                this.keyBoardHook.Flag = HookFlag.IsRunning;
            }
        }
		
        private void CloseClick(object sender,EventArgs e)
        {
            mouseHook.Dispose();
            if (mouseHook != null)
            {
                mouseHook = null;
            }
            notifyIcon.Visible=false;
            this.Close();
        }
        private void MiddleButton_KeyDown(object sender, KeyEventArgs e)
        {
            _myKeys.MiddleButton = (Hook.Key)Enum.Parse(typeof(Hook.Key), e.Key.ToString());
            e.Handled = true;
            tbMiddleButton.Text = e.Key.ToString();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            e.Handled = true;
            textBox.Text = e.Key.ToString();
            int index = textBox.Name[textBox.Name.Length - 1]-48;
            if (index>=0&&index<10)
            {
                _myKeys._rightkeys[index] = (Hook.Key)Enum.Parse(typeof(Hook.Key), e.Key.ToString());
            }
        }

        private void LeftTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            e.Handled = true;
            textBox.Text = e.Key.ToString();
            int index = textBox.Name[textBox.Name.Length - 1]-48;
            if (index>=0&&index<10)
            {
                _myKeys._leftkeys[index] = (Hook.Key)Enum.Parse(typeof(Hook.Key), e.Key.ToString());
            }
        }

        private void Window_Activated_1(object sender, EventArgs e)
        {
            if (mouseHook != null && keyBoardHook != null)
            {
                this.mouseHook.Flag = HookFlag.IsPaused;
                this.keyBoardHook.Flag = HookFlag.IsPaused;
            }
        }

        private void Window_Deactivated_1(object sender, EventArgs e)
        {
            if (mouseHook != null && keyBoardHook != null)
            {
                this.mouseHook.Flag = HookFlag.IsRunning;
                this.keyBoardHook.Flag = HookFlag.IsRunning;
            }
        }
        
		private void LeftResTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			TextBox textBox = (TextBox)sender;
            e.Handled = true;
            textBox.Text = e.Key.ToString();
            int index = textBox.Name[textBox.Name.Length - 1]-48;
            if (index>=0&&index<10)
            {
                _myKeys._leftResKeys[index] = (Hook.Key)Enum.Parse(typeof(Hook.Key), e.Key.ToString());
            }
		}
		
		private void LTextBox_TextChanged(object sender,TextChangedEventArgs e)
		{
			TextBox textBox = (TextBox)sender;
            int index = textBox.Name[textBox.Name.Length - 1]-48;
            if(textBox.Text=="")
            {
            	_myKeys._leftkeys[index]=Hook.Key.None;
            }
		}
		
		private void RTextBox_TextChanged(object sender,TextChangedEventArgs e)
		{
			TextBox textBox = (TextBox)sender;
            int index = textBox.Name[textBox.Name.Length - 1]-48;
            if(textBox.Text=="")
            {
            	_myKeys._rightkeys[index]=Hook.Key.None;
            }
		}
		
		private void LresTextBox_TextChanged(object sender,TextChangedEventArgs e)
		{
			TextBox textBox = (TextBox)sender;
            int index = textBox.Name[textBox.Name.Length - 1]-48;
            if(textBox.Text=="")
            {
            	_myKeys._leftResKeys[index]=Hook.Key.None;
            }
		}
		
		private void RresTextBox_TextChanged(object sender,TextChangedEventArgs e)
		{
			TextBox textBox = (TextBox)sender;
            int index = textBox.Name[textBox.Name.Length - 1]-48;
            if(textBox.Text=="")
            {
            	_myKeys._rightResKeys[index]=Hook.Key.None;
            }
		}
		
		private void KaelKeysTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			TextBox textBox = (TextBox)sender;
            e.Handled = true;
            textBox.Text = e.Key.ToString();
            int index = textBox.Name[textBox.Name.Length - 1]-48;
            if (index>=0&&index<10)
            {
            	_myKeys._myKaelKeys[index] = (Hook.Key)Enum.Parse(typeof(Hook.Key), e.Key.ToString());
            }
		}
			
		private void KaelKeysTextBox_TextChanged(object sender,TextChangedEventArgs e)
		{
			TextBox textBox = (TextBox)sender;
            int index = textBox.Name[textBox.Name.Length - 1]-48;
            if(textBox.Text=="")
            {
            	textBox.Text=_myKeys._myKaelKeys._skillKeys[index].ToString();
            	_myKeys._myKaelKeys[index]=_myKeys._myKaelKeys._skillKeys[index];
            }
		}
		private void InitializeTextBox()
		{
			#region 自定义键控件加载
			for(int i=0,top=30;i<5;i++,top+=30)
			{
				TextBox tb=new TextBox();
				tb.Name="ltb"+i.ToString();
				tb.Height=20;
				tb.Width=50;
				tb.VerticalAlignment=VerticalAlignment.Top;
				tb.HorizontalAlignment=HorizontalAlignment.Left;
				tb.Margin= new Thickness(17,top,0,0);
				tb.KeyDown+= LeftTextBox_KeyDown;
				tb.TextChanged+= LTextBox_TextChanged;
				if(_myKeys._leftkeys[i]!=Hook.Key.None)
					tb.Text=_myKeys._leftkeys[i].ToString();
				gridCustomKey.Children.Add(tb);
				
				
				TextBox tb1=new TextBox();
				tb1.Name="rtb"+i.ToString();
				tb1.Height=20;
				tb1.Width=50;
				tb1.VerticalAlignment=VerticalAlignment.Top;
				tb1.HorizontalAlignment=HorizontalAlignment.Left;
				tb1.Margin= new Thickness(75,top,0,0);
				tb1.KeyDown+= TextBox_KeyDown;
				tb1.TextChanged+=RTextBox_TextChanged;
				if(_myKeys._rightkeys[i]!=Hook.Key.None)
					tb1.Text=_myKeys._rightkeys[i].ToString();
				gridCustomKey.Children.Add(tb1);
				
				
				TextBox tb2=new TextBox();
				tb2.Name="ltb"+(i+5).ToString();
				tb2.Height=20;
				tb2.Width=50;
				tb2.VerticalAlignment=VerticalAlignment.Top;
				tb2.HorizontalAlignment=HorizontalAlignment.Left;
				tb2.Margin= new Thickness(140,top,0,0);
				tb2.KeyDown+= LeftTextBox_KeyDown;
				tb2.TextChanged+= LTextBox_TextChanged;
				if(_myKeys._leftkeys[i+5]!=Hook.Key.None)
					tb2.Text=_myKeys._leftkeys[i+5].ToString();
				gridCustomKey.Children.Add(tb2);
				
				
				TextBox tb3=new TextBox();
				tb3.Name="rtb"+(i+5).ToString();
				tb3.Height=20;
				tb3.Width=50;
				tb3.VerticalAlignment=VerticalAlignment.Top;
				tb3.HorizontalAlignment=HorizontalAlignment.Left;
				tb3.Margin= new Thickness(200,top,0,0);
				tb3.KeyDown+= TextBox_KeyDown;
				tb3.TextChanged+=RTextBox_TextChanged;
				if(_myKeys._rightkeys[i+5]!=Hook.Key.None)
					tb3.Text=_myKeys._rightkeys[i+5].ToString();
				gridCustomKey.Children.Add(tb3);
			}
			#endregion
			#region 物品栏改键控件加载
			for(int j=0,top=10;j<3;j++,top+=30)
			{
				TextBox tb=new TextBox();
				tb.Name="lrs"+(2*j).ToString();
				tb.Margin=new Thickness(17,top,0,0);
				tb.VerticalAlignment=VerticalAlignment.Top;
				tb.HorizontalAlignment=HorizontalAlignment.Left;
				tb.Height=20;
				tb.Width=50;
				if(_myKeys._leftResKeys[2*j]!=Hook.Key.None)
				{
					tb.Text=_myKeys._leftResKeys[2*j].ToString();
				}
				tb.KeyDown+=LeftResTextBox_KeyDown;
				tb.TextChanged+=LresTextBox_TextChanged;
				gridResKey.Children.Add(tb);
				
				
				TextBox tb1=new TextBox();
				tb1.Name="lrs"+(2*j+1).ToString();
				tb1.Margin=new Thickness(150,top,0,0);
				tb1.VerticalAlignment=VerticalAlignment.Top;
				tb1.HorizontalAlignment=HorizontalAlignment.Left;
				tb1.Height=20;
				tb1.Width=50;
				tb1.KeyDown+=LeftResTextBox_KeyDown;
				tb1.TextChanged+=RresTextBox_TextChanged;
				gridResKey.Children.Add(tb1);
			}
			#endregion
			#region 加载鼠标改键
			if(_myKeys.XButton1!=Hook.Key.None)
				tbXButton1.Text=_myKeys.XButton1.ToString();
			if(_myKeys.XButton2!=Hook.Key.None)
				tbXButton2.Text=_myKeys.XButton2.ToString();
			if(_myKeys.MiddleButton!= Hook.Key.None)
				tbMiddleButton.Text=_myKeys.MiddleButton.ToString();
			#endregion
			#region 卡尔改键加载
			for(int i=0,top=80;i<5;i++,top+=30)
			{
				TextBox tb1=new TextBox();
				tb1.Name="ktb"+i.ToString();
				tb1.Height=20;
				tb1.Width=50;
				tb1.VerticalAlignment=VerticalAlignment.Top;
				tb1.HorizontalAlignment=HorizontalAlignment.Left;
				tb1.Margin= new Thickness(75,top,0,0);
				tb1.KeyDown+= KaelKeysTextBox_KeyDown;
				tb1.TextChanged+=KaelKeysTextBox_TextChanged;
				if(_myKeys._myKaelKeys[i]!=Hook.Key.None)
					tb1.Text=_myKeys._myKaelKeys[i].ToString();
				gridKaelKey.Children.Add(tb1);
				
				TextBox tb3=new TextBox();
				tb3.Name="ktb"+(i+5).ToString();
				tb3.Height=20;
				tb3.Width=50;
				tb3.VerticalAlignment=VerticalAlignment.Top;
				tb3.HorizontalAlignment=HorizontalAlignment.Left;
				tb3.Margin= new Thickness(200,top,0,0);
				tb3.KeyDown+= KaelKeysTextBox_KeyDown;
				tb3.TextChanged+=KaelKeysTextBox_TextChanged;
				if(_myKeys._myKaelKeys[i+5]!=Hook.Key.None)
					tb3.Text=_myKeys._myKaelKeys[i+5].ToString();
				gridKaelKey.Children.Add(tb3);
			}
			#endregion
		}
		
    }
}
