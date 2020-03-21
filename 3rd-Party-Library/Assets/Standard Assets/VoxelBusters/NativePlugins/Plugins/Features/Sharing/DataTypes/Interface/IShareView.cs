using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	/// <summary>
	/// Interface implemented by the share composer object.
	/// </summary>
	public interface IShareView 
	{
		#region Fields

		/// <summary>
		/// A bool value that indicates whether this instance is ready to present composer view. (read-only)
		/// </summary>
		/// <value><c>true</c> if this instance is ready to present composer view; otherwise, <c>false</c>.</value>
		bool IsReadyToShowView
		{
			get;
		}

		#endregion
	}
}