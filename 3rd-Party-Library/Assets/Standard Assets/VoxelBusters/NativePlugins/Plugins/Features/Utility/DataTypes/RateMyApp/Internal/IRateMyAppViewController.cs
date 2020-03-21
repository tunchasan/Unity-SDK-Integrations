using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins.Internal
{
	#region Delegates

	public delegate void ShowDialogResultDelegate(string _buttonName);

	#endregion

	public interface IRateMyAppViewController 
	{
		#region Methods

		void ShowDialog (string _title, string _message, string[] _buttons, ShowDialogResultDelegate _onCompletion);

		#endregion
	}
}