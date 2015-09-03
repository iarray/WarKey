using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;

namespace Hook
{
    public abstract class GlobalHook
    {
        //安装钩子
        [DllImport("user32.dll", CharSet =CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
        //卸载钩子
        [DllImport("user32.dll", CharSet =CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(IntPtr idHook);
        //调用下一个钩子
        [DllImport("user32.dll", CharSet =CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(IntPtr idHook, int nCode,Int32 wParam, IntPtr lParam);

        //定义钩子句柄
        public IntPtr _hookHandle ;
    	protected HookFlag _flag;
    	public HookFlag Flag
    	{
            set
            {
                _flag = value;
            }
    		get
    		{
    			return _flag;
    		}
    	}
    	protected abstract bool InstallHook();
    }
    public class MouseHook : GlobalHook,IDisposable
    {
        /// <summary>
        /// 底层鼠标钩子
        /// </summary>
        private const int WH_MOUSE_LL = 14;
    	private HookProc _mouseHookProc;
    	public event MouseHookEventHandler MouseEvent;
    	/// <summary>
    	/// 安装钩子
    	/// </summary>
    	/// <returns></returns>
    	protected override bool InstallHook()
    	{
    		IntPtr pInstance = Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().ManifestModule);
    		if(_hookHandle==IntPtr.Zero)
    		{
    			_mouseHookProc=new HookProc(MouseHookProc);
    			_hookHandle=SetWindowsHookEx(WH_MOUSE_LL,_mouseHookProc,pInstance,0);
    		}
    		if(_hookHandle!=IntPtr.Zero)
    		{
    			_flag=HookFlag.IsRunning;
    			return true;
    		}
    		else
    		{
    			_flag=HookFlag.IsStop;
    			return false;
    		}
    	}
    	/// <summary>
    	/// 构造函数安装钩子
    	/// </summary>
    	public MouseHook()
    	{
    		_hookHandle = IntPtr.Zero;
    		InstallHook();
    	}
    	/// <summary>
    	/// 鼠标钩子处理方法
    	/// </summary>
    	/// <param name="nCode">指定是否需要处理该消息,消息类型WH_MOUSE_LL</param>
    	/// <param name="wParam">该消息的附加消息 </param>
    	/// <param name="lParam">该消息的附加消息 </param>
    	/// <returns></returns>
    	private int MouseHookProc(int nCode,Int32 wParam,IntPtr lParam)
    	{
    		if(Flag==HookFlag.IsRunning&&(nCode>=0)&&(MouseEvent!=null))
    		{
    			//将数据从非托管内存块封送到新分配的指定类型的托管对象。
    			MouseHookStruct mouseHookStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));
    			MouseButtons mousebutton=MouseButtons.None;
    			short mouseDelta = 0;
    			switch(wParam)
    			{
					case (int)WM_MOUSE.WM_LBUTTONDOWN:
						mousebutton=MouseButtons.Left;
						break;
					case(int)WM_MOUSE.WM_RBUTTONDOWN:
						mousebutton=MouseButtons.Right;
						break;
					case(int)WM_MOUSE.WM_MBUTTONDOWN:
						mousebutton=MouseButtons.Middle;
						break;
					case (int)WM_MOUSE.WM_MOUSEWHEEL:
                        mouseDelta = (short)((mouseHookStruct.MouseData >> 16) & 0xffff);
						break;
					case(int)WM_MOUSE.WM_XBUTTONDOWN:
						if(mouseHookStruct.MouseData==(uint)WM_MOUSE.MD_XBUTTON1)
						{
							mousebutton=MouseButtons.XButton1;
						}
						else if(mouseHookStruct.MouseData==(uint)WM_MOUSE.MD_XBUTTON2)
						{
							mousebutton=MouseButtons.XButton2;
						}
						break;
    			}
    			MouseHookEventArgs e = new MouseHookEventArgs(mousebutton,mouseHookStruct.Point,mouseDelta);
    			if(MouseEvent(this,e))
    			{
    				return 1;
    			}
    		}
            return CallNextHookEx(this._hookHandle, nCode, wParam, lParam);
    	}
    	/// <summary>
    	/// 卸载钩子
    	/// </summary>
    	/// <returns></returns>
    	private bool UnInstallHook()
    	{
    		bool result=true;
    		if(_hookHandle!=IntPtr.Zero)
    		{
    			result = UnhookWindowsHookEx(_hookHandle)&&result;
    		}
    		if(_hookHandle==IntPtr.Zero)
    		{
    			_flag=HookFlag.IsStop;
    			return result;
    		}
    		else
    		{
    			return result;
    		}
    	}
    	/// <summary>
    	/// 析构函数卸载钩子
    	/// </summary>
    	~MouseHook()
    	{
    		Dispose();
    	}
    	/// <summary>
    	/// 销毁鼠标钩子
    	/// </summary>
    	public void Dispose()
    	{
    		UnInstallHook();
    		_mouseHookProc=null;
    		MouseEvent=null;
    	}
    }
    public class KeyBoardHook:GlobalHook,IDisposable
    {
    	/// <summary>
        /// 底层键盘钩子
        /// </summary>
        private const int WH_KEYBOARD_LL = 13;
        private HookProc _keyBoardHookProc;
    	public event KeyBoardHookEventHandler KeyDown;
//    	public event KeyBoardHookEventHandler KeyPress;
    	public event KeyBoardHookEventHandler KeyUp;

    	/// <summary>
    	/// 安装键盘钩子
    	/// </summary>
    	/// <returns></returns>
    	protected override bool InstallHook()
    	{
    		IntPtr pInstance = Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().ManifestModule);
    		if(_hookHandle==IntPtr.Zero)
    		{
    			_keyBoardHookProc=new HookProc(KeyBoardHookProc);
    			_hookHandle=SetWindowsHookEx(WH_KEYBOARD_LL,_keyBoardHookProc,pInstance,0);
    		}
    		if(_hookHandle!=IntPtr.Zero)
    		{
    			_flag=HookFlag.IsRunning;
    			return true;
    		}
    		else
    		{
    			_flag=HookFlag.IsStop;
    			return false;
    		}
    	}
    	/// <summary>
    	/// 构造函数安装钩子
    	/// </summary>
    	public KeyBoardHook()
    	{
    		_hookHandle=IntPtr.Zero;
    		InstallHook();
    	}
    	/// <summary>
    	/// 键盘钩子处理方法
    	/// </summary>
    	/// <param name="nCode">消息类型，这里为键盘消息</param>
    	/// <param name="wParam">消息附加参数</param>
    	/// <param name="lParam">消息附加参数</param>
    	/// <returns></returns>
    	public int KeyBoardHookProc(int nCode,Int32 wParam,IntPtr lParam)
    	{
    		if(Flag == HookFlag.IsRunning && (nCode)>=0&&(KeyDown!=null||KeyUp!=null))
    		{
    			KeyboardHookStruct _keyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
    			if(KeyDown!=null&&((int)WM_KEYBOARD.WM_SYSKEYDOWN==wParam||(int)WM_KEYBOARD.WM_KEYDOWN==wParam))
				{
					Keys keyCode= (Keys)_keyboardHookStruct.VKCode;
					KeyBoardHookEventArgs e = new KeyBoardHookEventArgs(keyCode, KeysState.IsDown);
					if(KeyDown(this,e))
					{
						return 1;
					}
				}
				if(KeyUp!=null&&((int)WM_KEYBOARD.WM_KEYUP==wParam||(int)WM_KEYBOARD.WM_SYSKEYUP==wParam))
				{
					Keys keyCode= (Keys)_keyboardHookStruct.VKCode;
					KeyBoardHookEventArgs e = new KeyBoardHookEventArgs(keyCode, KeysState.IsUp);
					if(KeyUp(this,e))
					{
						return 1;
					}
				}
    			return CallNextHookEx(_hookHandle, nCode, wParam, lParam);
    		}
    		return 0;
    		
    	}
    	/// <summary>
    	/// 卸载钩子
    	/// </summary>
    	/// <returns></returns>
    	private bool UnInstallHook()
    	{
    		bool result=true;
    		if(_hookHandle!=IntPtr.Zero)
    		{
    			result = UnhookWindowsHookEx(_hookHandle)&&result;
    		}
    		if(_hookHandle==IntPtr.Zero)
    		{
    			_flag=HookFlag.IsStop;
    			return result;
    		}
    		else
    		{
    			return result;
    		}
    	}
    	/// <summary>
    	/// 析构函数卸载键盘钩子
    	/// </summary>
    	~KeyBoardHook()
    	{
    		Dispose();
    	}
    	
    	public void Dispose()
    	{
    		UnInstallHook();
    		_keyBoardHookProc=null;
    		KeyDown=null;
    		KeyUp=null;
    	}
    }
}
