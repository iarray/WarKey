/*
 * Created by SharpDevelop.
 * User: h
 * Date: 2013/4/20 星期六
 * Time: 19:45
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace Hook
{
    #region 枚举定义
	//钩子当前状态
	public enum HookFlag
	{
		//运行中
		IsRunning,
        //暂停
        IsPaused,
		//停止
		IsStop
	}
	/// <summary>
	/// 鼠标按键消息
	/// </summary>
    public enum WM_MOUSE : int
    {
        /// <summary>
        /// 鼠标开始
        /// </summary>
        WM_MOUSEFIRST = 0x200,

        /// <summary>
        /// 鼠标移动
        /// </summary>
        WM_MOUSEMOVE = 0x200,

        /// <summary>
        /// 左键按下
        /// </summary>
        WM_LBUTTONDOWN = 0x201,

        /// <summary>
        /// 左键释放
        /// </summary>
        WM_LBUTTONUP = 0x202,

        /// <summary>
        /// 左键双击
        /// </summary>
        WM_LBUTTONDBLCLK = 0x203,

        /// <summary>
        /// 右键按下
        /// </summary>
        WM_RBUTTONDOWN = 0x204,

        /// <summary>
        /// 右键释放
        /// </summary>
        WM_RBUTTONUP = 0x205,

        /// <summary>
        /// 右键双击
        /// </summary>
        WM_RBUTTONDBLCLK = 0x206,

        /// <summary>
        /// 中键按下
        /// </summary>
        WM_MBUTTONDOWN = 0x207,

        /// <summary>
        /// 中键释放
        /// </summary>
        WM_MBUTTONUP = 0x208,

        /// <summary>
        /// 中键双击
        /// </summary>
        WM_MBUTTONDBLCLK = 0x209,

        /// <summary>
        /// 滚轮滚动
        /// </summary>
        /// <remarks>WINNT4.0以上才支持此消息</remarks>
        WM_MOUSEWHEEL = 0x020A,
        /// <summary>
        /// 自定义按键释放
        /// </summary>
       	WM_XBUTTONUP = 0x020C,
       	/// <summary>
       	/// 自定义按键按下
       	/// </summary>
       	WM_XBUTTONDOWN = 0x020B,
       	/// <summary>
       	/// 自定义键一
       	/// </summary>
       	MD_XBUTTON1=0x10000,
       	/// <summary>
       	/// 自定义键二
       	/// </summary>
       	MD_XBUTTON2=0x20000
    }
    /// <summary>
    /// 键盘按键消息
    /// </summary>
	public enum WM_KEYBOARD : int
    {
        /// <summary>
        /// 非系统按键按下
        /// </summary>
        WM_KEYDOWN = 0x100,

        /// <summary>
        /// 非系统按键释放
        /// </summary>
        WM_KEYUP = 0x101,

        /// <summary>
        /// 系统按键按下
        /// </summary>
        WM_SYSKEYDOWN = 0x104,

        /// <summary>
        /// 系统按键释放
        /// </summary>
        WM_SYSKEYUP = 0x105
    }    
	/// <summary>
	/// 鼠标键值
	/// </summary>
	public enum MouseButtons
	{
		None,
		Left,
		Right,
		Middle,
		XButton1,
		XButton2
	}
	/// <summary>
	/// 键盘按键码
	/// </summary>
	public enum Keys
	{
		KeyCode = 65535,
		Modifiers = -65536,
		None = 0,
		LButton = 1,
		RButton = 2,
		Cancel = 3,
		MButton = 4,
		XButton1 = 5,
		XButton2 = 6,
		Back = 8,
		Tab = 9,
		LineFeed = 10,
		Clear = 12,
		Return = 13,
		Enter = 13,
		ShiftKey = 16,
		ControlKey = 17,
		Menu = 18,
		Pause = 19,
		Capital = 20,
		CapsLock = 20,
		KanaMode = 21,
		HanguelMode = 21,
		HangulMode = 21,
		JunjaMode = 23,
		FinalMode = 24,
		HanjaMode = 25,
		KanjiMode = 25,
		Escape = 27,
		IMEConvert = 28,
		IMENonconvert = 29,
		IMEAccept = 30,
		IMEAceept = 30,
		IMEModeChange = 31,
		Space = 32,
		Prior = 33,
		PageUp = 33,
		Next = 34,
		PageDown = 34,
		End = 35,
		Home = 36,
		Left = 37,
		Up = 38,
		Right = 39,
		Down = 40,
		Select = 41,
		Print = 42,
		Execute = 43,
		Snapshot = 44,
		PrintScreen = 44,
		Insert = 45,
		Delete = 46,
		Help = 47,
		D0 = 48,
		D1 = 49,
		D2 = 50,
		D3 = 51,
		D4 = 52,
		D5 = 53,
		D6 = 54,
		D7 = 55,
		D8 = 56,
		D9 = 57,
		A = 65,
		B = 66,
		C = 67,
		D = 68,
		E = 69,
		F = 70,
		G = 71,
		H = 72,
		I = 73,
		J = 74,
		K = 75,
		L = 76,
		M = 77,
		N = 78,
		O = 79,
		P = 80,
		Q = 81,
		R = 82,
		S = 83,
		T = 84,
		U = 85,
		V = 86,
		W = 87,
		X = 88,
		Y = 89,
		Z = 90,
		LWin = 91,
		RWin = 92,
		Apps = 93,
		Sleep = 95,
		NumPad0 = 96,
		NumPad1 = 97,
		NumPad2 = 98,
		NumPad3 = 99,
		NumPad4 = 100,
		NumPad5 = 101,
		NumPad6 = 102,
		NumPad7 = 103,
		NumPad8 = 104,
		NumPad9 = 105,
		Multiply = 106,
		Add = 107,
		Separator = 108,
		Subtract = 109,
		Decimal = 110,
		Divide = 111,
		F1 = 112,
		F2 = 113,
		F3 = 114,
		F4 = 115,
		F5 = 116,
		F6 = 117,
		F7 = 118,
		F8 = 119,
		F9 = 120,
		F10 = 121,
		F11 = 122,
		F12 = 123,
		F13 = 124,
		F14 = 125,
		F15 = 126,
		F16 = 127,
		F17 = 128,
		F18 = 129,
		F19 = 130,
		F20 = 131,
		F21 = 132,
		F22 = 133,
		F23 = 134,
		F24 = 135,
		NumLock = 144,
		Scroll = 145,
		LShiftKey = 160,
		RShiftKey = 161,
		LControlKey = 162,
		RControlKey = 163,
		LMenu = 164,
		RMenu = 165,
		BrowserBack = 166,
		BrowserForward = 167,
		BrowserRefresh = 168,
		BrowserStop = 169,
		BrowserSearch = 170,
		BrowserFavorites = 171,
		BrowserHome = 172,
		VolumeMute = 173,
		VolumeDown = 174,
		VolumeUp = 175,
		MediaNextTrack = 176,
		MediaPreviousTrack = 177,
		MediaStop = 178,
		MediaPlayPause = 179,
		LaunchMail = 180,
		SelectMedia = 181,
		LaunchApplication1 = 182,
		LaunchApplication2 = 183,
		OemSemicolon = 186,
		Oem1 = 186,
		Oemplus = 187,
		Oemcomma = 188,
		OemMinus = 189,
		OemPeriod = 190,
		OemQuestion = 191,
		Oem2 = 191,
		Oemtilde = 192,
		Oem3 = 192,
		OemOpenBrackets = 219,
		Oem4 = 219,
		OemPipe = 220,
		Oem5 = 220,
		OemCloseBrackets = 221,
		Oem6 = 221,
		OemQuotes = 222,
		Oem7 = 222,
		Oem8 = 223,
		OemBackslash = 226,
		Oem102 = 226,
		ProcessKey = 229,
		Packet = 231,
		Attn = 246,
		Crsel = 247,
		Exsel = 248,
		EraseEof = 249,
		Play = 250,
		Zoom = 251,
		NoName = 252,
		Pa1 = 253,
		OemClear = 254,
		Shift = 65536,
		Control = 131072,
		Alt = 262144
	}
    public enum Key
    {
        A = 65,
        AbntC1 = 0x93,
        AbntC2 = 0x94,
        Add = 107,
        Apps = 93,
        Attn = 0xa3,
        B = 66,
        Back = 8,
        BrowserBack = 166,
        BrowserFavorites = 171,
        BrowserForward = 167,
        BrowserHome = 172,
        BrowserRefresh = 168,
        BrowserSearch = 170,
        BrowserStop = 169,
        C = 67,
        Cancel = 1,
        Capital = 8,
        CapsLock = 8,
        Clear = 5,
        CrSel = 247,
        D = 68,
        D0 = 48,
        D1 = 49,
        D2 = 50,
        D3 = 51,
        D4 = 52,
        D5 = 53,
        D6 = 54,
        D7 = 55,
        D8 = 56,
        D9 = 57,
        DbeAlphanumeric = 0x9d,
        DbeCodeInput = 0xa7,
        DbeDbcsChar = 0xa1,
        DbeDetermineString = 0xa9,
        DbeEnterDialogConversionMode = 170,
        DbeEnterImeConfigureMode = 0xa5,
        DbeEnterWordRegisterMode = 0xa4,
        DbeFlushString = 0xa6,
        DbeHiragana = 0x9f,
        DbeKatakana = 0x9e,
        DbeNoCodeInput = 0xa8,
        DbeNoRoman = 0xa3,
        DbeRoman = 0xa2,
        DbeSbcsChar = 160,
        DeadCharProcessed = 0xac,
        Decimal = 110,
        Delete = 46,
        Divide = 111,
        Down = 40,
        E = 69,
        End = 35,
        Enter = 6,
        EraseEof = 249,
        Escape = 13,
        Execute = 43,
        ExSel = 248,
        F = 70,
        F1 = 112,
        F10 = 121,
        F11 = 122,
        F12 = 123,
        F13 = 124,
        F14 = 125,
        F15 = 126,
        F16 = 127,
        F17 = 128,
        F18 = 129,
        F19 = 130,
        F2 = 113,
        F20 = 131,
        F21 = 132,
        F22 = 133,
        F23 = 134,
        F24 = 135,
        F3 = 114,
        F4 = 115,
        F5 = 116,
        F6 = 117,
        F7 = 118,
        F8 = 119,
        F9 = 120,
        FinalMode = 24,
        G = 71,
        H = 72,
        HangulMode = 9,
        HanjaMode = 12,
        Help = 47,
        Home = 36,
        I = 73,
        ImeAccept = 30,
        ImeConvert = 28,
        ImeModeChange = 31,
        ImeNonConvert = 29,
        ImeProcessed = 0x9b,
        Insert = 45,
        J = 74,
        JunjaMode = 23,
        K = 75,
        KanaMode = 21,
        KanjiMode = 25,
        L = 76,
        LaunchApplication1 = 182,
        LaunchApplication2 = 183,
        LaunchMail = 180,
        Left = 37,
        LeftAlt = 262144,
        LeftCtrl = 162,
        LeftShift = 160,
        LineFeed = 10,
        LWin = 164,
        M = 77,
        MediaNextTrack = 176,
        MediaPlayPause = 179,
        MediaPreviousTrack = 177,
        MediaStop = 178,
        Multiply = 106,
        N = 78,
        Next = 34,
        NoName = 252,
        None = 0,
        NumLock = 144,
        NumPad0 = 96,
        NumPad1 = 97,
        NumPad2 = 98,
        NumPad3 = 99,
        NumPad4 = 100,
        NumPad5 = 101,
        NumPad6 = 102,
        NumPad7 = 103,
        NumPad8 = 104,
        NumPad9 = 105,
        O = 79,
        Oem1 = 186,
        Oem102 = 226,
        Oem2 = 191,
        Oem3 = 192,
        Oem4 = 219,
        Oem5 = 220,
        Oem6 = 221,
        Oem7 = 222,
        Oem8 = 223,
        OemAttn = 0x9d,
        OemAuto = 160,
        OemBackslash = 226,
        OemBackTab = 0xa2,
        OemClear = 254,
        OemCloseBrackets = 221,
        OemComma = 188,
        OemCopy = 0x9f,
        OemEnlw = 0xa1,
        OemFinish = 0x9e,
        OemMinus = 189,
        OemOpenBrackets = 0x95,
        OemPeriod = 190,
        OemPipe = 220,
        OemPlus = 187,
        OemQuestion = 191,
        OemQuotes = 222,
        OemSemicolon = 186,
        OemTilde = 192,
        P = 80,
        Pa1 = 253,
        PageDown = 34,
        PageUp = 33,
        Pause = 19,
        Play = 250,
        Print = 42,
        PrintScreen = 44,
        Prior = 33,
        Q = 81,
        R = 82,
        Return = 13,
        Right = 39,
        RightAlt = 0x79,
        RightCtrl = 163,
        RightShift = 161,
        RWin = 165,
        S = 83,
        Scroll = 145,
        Select = 41,
        SelectMedia = 181,
        Separator = 108,
        Sleep = 95,
        Snapshot = 44,
        Space = 32,
        Subtract = 109,
        System = 0x9c,
        T = 84,
        Tab = 9,
        U = 85,
        Up = 38,
        V = 86,
        VolumeDown = 174,
        VolumeMute = 173,
        VolumeUp = 175,
        W = 87,
        X = 88,
        Y = 89,
        Z = 90,
        Zoom = 251
    }
	public enum KeysState
	{
		Normal,
		IsDown,
		IsUp
	}
	#endregion
	
	#region 结构体定义
	/// <summary>
	/// 坐标结构体
	/// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
    }
	
    /// <summary>
    /// 鼠标钩子事件结构定义
    /// </summary>
    /// <remarks>详细说明请参考MSDN中关于 MSLLHOOKSTRUCT 的说明</remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct MouseHookStruct
    {
        /// <summary>
        /// Specifies a POINT structure that contains the x- and y-coordinates of the cursor, in screen coordinates.
        /// </summary>
        public POINT Point;

        public UInt32 MouseData;
        public UInt32 Flags;
        public UInt32 Time;
        public UInt32 ExtraInfo;
    }
    /// <summary>
    /// 键盘钩子事件结构定义
    /// </summary>
    /// <remarks>详细说明请参考MSDN中关于 KBDLLHOOKSTRUCT 的说明</remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyboardHookStruct
    {
        /// <summary>
        /// Specifies a virtual-key code. The code must be a value in the range 1 to 254. 
        /// </summary>
        public UInt32 VKCode;

        /// <summary>
        /// Specifies a hardware scan code for the key.
        /// </summary>
        public UInt32 ScanCode;

        /// <summary>
        /// Specifies the extended-key flag, event-injected flag, context code, 
        /// and transition-state flag. This member is specified as follows. 
        /// An application can use the following values to test the keystroke flags. 
        /// </summary>
        public UInt32 Flags;

        /// <summary>
        /// Specifies the time stamp for this message. 
        /// </summary>
        public UInt32 Time;

        /// <summary>
        /// Specifies extra information associated with the message. 
        /// </summary>
        public UInt32 ExtraInfo;
    }
    #endregion
    
    #region 委托声明
	/// <summary>
    /// 钩子委托声明
    /// </summary>
    /// <param name="nCode">消息类型</param>
    /// <param name="wParam">消息参数</param>
    /// <param name="lParam">消息附加参数</param>
    /// <returns></returns>
    public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);
    #endregion
}
