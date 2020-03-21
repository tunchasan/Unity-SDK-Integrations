using UnityEngine;
using System.Collections;

namespace VoxelBusters.Utility
{
	public class InspectorButtonInfo 
	{
		#region Properties
		
		public string Name 
		{ 
			get; 
			private set; 
		}
		
		public string ToolTip 
		{ 
			get; 
			private set; 
		}
		
		public string InvokeMethod 
		{ 
			get; 
			private set; 
		}
		
		#endregion
		
		#region Constructors

		private InspectorButtonInfo ()
		{}
		
		public InspectorButtonInfo (string _buttonName, string _toolTip, string _invokeMethod)
		{
			this.Name			= _buttonName;
			this.ToolTip		= _toolTip;
			this.InvokeMethod	= _invokeMethod;
		}
		
		#endregion
	}
}