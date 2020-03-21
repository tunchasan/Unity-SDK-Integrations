using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins.Internal
{
	public interface IRateMyAppOperationHandler 
	{
		#region Methods

		void Execute (IEnumerator _routine);		

		#endregion
	}
}