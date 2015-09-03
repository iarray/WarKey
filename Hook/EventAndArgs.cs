/*
 * Created by SharpDevelop.
 * User: h
 * Date: 04/21/2013
 * Time: 16:13
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Hook
{
	public delegate bool MouseHookEventHandler(object sender, MouseHookEventArgs e);
	/// <summary>
	/// 键盘钩子事件处理，返回true，拦截原按键消息并阻止发送，返回false拦截原按键消息不阻止发送。
	/// </summary>
	public delegate bool KeyBoardHookEventHandler(object sender,KeyBoardHookEventArgs e);
    public class MouseHookEventArgs
    {
    	private MouseButtons _mousebutton;
    	private POINT _position;
    	//鼠标轮已转动的制动器数
    	private short _mouseDelta=0;
    	public MouseButtons ChangedButton
    	{
    		get
    		{
    			return _mousebutton;
    		}
    	}
    	public Int32 DeltaCount 
    	{
    		get
    		{
    			return _mouseDelta;
    		}
    	}
    	public POINT Position
    	{
    		get
    		{
    			return _position;
    		}
    	}
    	public MouseHookEventArgs(MouseButtons mouseButton,POINT position,short mouseDelta)
    	{
    		_mousebutton=mouseButton;
    		_position=position;
    		_mouseDelta=mouseDelta;
    	}
    }
    public class KeyBoardHookEventArgs
    {
    	private Keys _key;
    	private KeysState _keyState;
    	public Keys Key
    	{
    		get
    		{
    			return _key;
    		}
    	}
    	public KeysState KeyState
    	{
    		get
    		{
    			return _keyState;
    		}
    	}
    	public KeyBoardHookEventArgs(Keys key,KeysState keystate)
    	{
    		_key=key;
    		_keyState=keystate;
    	}
    }
}
