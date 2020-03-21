using UnityEngine;
using System.Collections;

namespace VoxelBusters.Utility
{
	public class PopupAttribute : PropertyAttribute 
	{
		#region Fields

		public	string[]	options;

		#endregion

		#region Constructors

		public PopupAttribute (params string[] _options)
		{
			options	= _options;
		}

		#endregion
	}
}