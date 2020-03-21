using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.UnityEngineUtils
{
	public interface ISaveAssetCallback 
	{
		#region Methods

		void OnBeforeSave();

		#endregion
	}
}