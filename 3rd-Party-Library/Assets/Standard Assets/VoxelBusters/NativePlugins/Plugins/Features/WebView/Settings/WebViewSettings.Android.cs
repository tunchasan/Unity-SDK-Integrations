using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	public partial class WebViewSettings
	{
		[System.Serializable]
		internal class AndroidSettings
		{
			#region Fields
			
			[SerializeField]
			[Tooltip ("Set this to make use of full performance if you have a full screen activity. But, the events you pass from webview to Unity will be queued and fired once you close the webview.")]		
			private 	bool 		m_useNewActivityForFullScreenWebView 	= false;
			
			#endregion
			
			#region Properties
			
			internal bool UseNewActivityForFullScreenWebView
			{
				get 
				{ 
					return m_useNewActivityForFullScreenWebView;
				}
			}

			#endregion
		}
	}
}