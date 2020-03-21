using UnityEngine;
using System.Collections;

namespace VoxelBusters.Utility
{
	public partial class InspectorButtonAttribute : PropertyAttribute 
	{
		#region Properties

		public InspectorButtonInfo[] Buttons
		{
			get;
			private set;
		}
		
		public eInspectorButtonPosition Position 
		{ 
			get; 
			private set; 
		}
		
		#endregion
		
		#region Constructor

		private	InspectorButtonAttribute ()
		{
			this.Buttons	= new InspectorButtonInfo[0];
			this.Position	= eInspectorButtonPosition.BOTTOM;
		}
		
		public InspectorButtonAttribute (eInspectorButtonPosition _position, string _buttonName, string _toolTip, string _invokeMethod)
		{
			this.Buttons	= new InspectorButtonInfo[] {
				new InspectorButtonInfo(_buttonName, _toolTip, _invokeMethod)
			};
			this.Position	= _position;
		}

		public InspectorButtonAttribute (eInspectorButtonPosition _position, params string[] _buttonInfoList)
		{
			int						_count		= _buttonInfoList.Length;
			InspectorButtonInfo[] 	_buttons	= new InspectorButtonInfo[_count];

			for (int _iter = 0; _iter < _count; _iter++)
			{
				string[]	_buttonComponents	= _buttonInfoList[_iter].Split(';');

				_buttons[_iter]					= new InspectorButtonInfo(_buttonComponents[0], _buttonComponents[1], _buttonComponents[2]);
			}

			// Set properties
			this.Buttons	= _buttons;
			this.Position	= _position;
		}

		#endregion
	}
}