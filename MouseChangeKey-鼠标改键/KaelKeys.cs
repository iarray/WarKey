/*
 * Created by SharpDevelop.
 * User: h
 * Date: 2013/4/29 星期一
 * Time: 18:54
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace MouseChangeKey_鼠标改键
{
	/// <summary>
	/// Description of KaelKeys.
	/// </summary>
	[Serializable]
	public class KaelKeys
	{
		public readonly Hook.Key[] _skillKeys = new Hook.Key[10]{Hook.Key.Y,Hook.Key.V,Hook.Key.X,Hook.Key.B,Hook.Key.C,
														Hook.Key.D,Hook.Key.Z,Hook.Key.T,Hook.Key.G,Hook.Key.F};
		private Hook.Key[] _changeKeys = new Hook.Key[10]{Hook.Key.Y,Hook.Key.V,Hook.Key.X,Hook.Key.B,Hook.Key.C,
														Hook.Key.D,Hook.Key.Z,Hook.Key.T,Hook.Key.G,Hook.Key.F};
		public readonly List<Hook.Key[]> MergeKey=new List<Hook.Key[]>();
		public bool isRunin;
		public Hook.Key[] KaelChangeKeys()
		{
			return this._changeKeys;
		}
		public Hook.Key this[int Index]
		{
			set
			{
				if(value!=Hook.Key.None)
				{
					this._changeKeys[Index]=value;
				}
			}
			get
			{
				return this._changeKeys[Index];
			}
		}
		public KaelKeys()
		{
			isRunin = false;
			MergeKey.Add(new Hook.Key[3]{Hook.Key.Q,Hook.Key.Q,Hook.Key.Q});
			MergeKey.Add(new Hook.Key[3]{Hook.Key.Q,Hook.Key.Q,Hook.Key.W});
			MergeKey.Add(new Hook.Key[3]{Hook.Key.Q,Hook.Key.W,Hook.Key.W});
			MergeKey.Add(new Hook.Key[3]{Hook.Key.Q,Hook.Key.W,Hook.Key.E});
			MergeKey.Add(new Hook.Key[3]{Hook.Key.W,Hook.Key.W,Hook.Key.W});
			MergeKey.Add(new Hook.Key[3]{Hook.Key.W,Hook.Key.W,Hook.Key.E});
			MergeKey.Add(new Hook.Key[3]{Hook.Key.W,Hook.Key.E,Hook.Key.E});
			MergeKey.Add(new Hook.Key[3]{Hook.Key.E,Hook.Key.E,Hook.Key.E});
			MergeKey.Add(new Hook.Key[3]{Hook.Key.E,Hook.Key.Q,Hook.Key.Q});
			MergeKey.Add(new Hook.Key[3]{Hook.Key.E,Hook.Key.E,Hook.Key.Q});
		}
	}
}
