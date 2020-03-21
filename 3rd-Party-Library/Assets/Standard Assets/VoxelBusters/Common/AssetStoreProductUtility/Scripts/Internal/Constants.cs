using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("Assembly-CSharp-Editor")]
[assembly:InternalsVisibleTo("Assembly-CSharp-Editor-firstpass")]

namespace VoxelBusters.UASUtils.Internal
{
	internal partial class Constants : MonoBehaviour 
	{
		// Related to company 
		internal const 	string 		kCopyrightsText					= "Copyright © 2018 Voxel Busters Interactive LLP. All rights reserved.";

		// Related to update window
		internal const 	string 		kButtonSkipVersion				= "Skip";
		internal const 	string 		kButtonDownloadFromAssetStore	= "Go To Asset Store";
		internal const 	string 		kButtonDownloadFromOurServer	= "Download";
	}
}