using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MouseChangeKey_鼠标改键
{
	[Serializable]
    class SaveKeys
    {
    	public KaelKeys _myKaelKeys;
        public Hook.Key XButton1;
        public Hook.Key XButton2;
        public Hook.Key MiddleButton;
        public Hook.Key[] _leftkeys = new Hook.Key[10];
        public Hook.Key[] _rightkeys = new Hook.Key[10];
        public Hook.Key[] _leftResKeys = new Hook.Key[6];
        public Hook.Key[] _rightResKeys = new Hook.Key[6] { Hook.Key.NumPad7, Hook.Key.NumPad8, Hook.Key.NumPad4, Hook.Key.NumPad5, Hook.Key.NumPad1, Hook.Key.NumPad2 };
        public SaveKeys()
        {
        	_myKaelKeys=new KaelKeys();
        }
    }
}
