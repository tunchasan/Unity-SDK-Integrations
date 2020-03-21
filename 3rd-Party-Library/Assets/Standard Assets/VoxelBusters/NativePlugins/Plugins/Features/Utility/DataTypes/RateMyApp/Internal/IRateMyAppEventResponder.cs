using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins.Internal
{
	public interface IRateMyAppEventResponder
	{
		#region Methods

		void OnRemindMeLater ();
		void OnRate ();
		void OnDontShow ();

		#endregion
	}
}