using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	public partial class BillingSettings
	{
		[System.Serializable]
		public class iOSSettings
		{
			#region Fields

			[SerializeField]
			[Tooltip("If enabled, payment receipts are validated before sending events. It's an optional measure used to avoid unauthorized puchases.")]
			private 		bool		m_supportsReceiptValidation		= true;
//			[SerializeField]
//			[Tooltip("If enabled, an additional copy of build information is maintained by the plugin. It's an optional measure used to avoid unauthorized puchases.")]
//			private			bool 		m_makeCopyOfBuildInfo 			= false;
			[SerializeField]
			[Tooltip("Custom server URL used for receipt validation. By default, Apple server is used.")]
			private 		string		m_validateUsingServerURL = null;

			#endregion

			#region Properties

			internal bool SupportsReceiptValidation
			{
				get
				{
					return m_supportsReceiptValidation;
				}
			}

			internal bool MakeCopyOfBuildInfo
			{
				get
				{
					return false;
				}
			}

			internal string ValidateUsingServerURL
			{
				get
				{
					return m_validateUsingServerURL;
				}
			}

			#endregion
		}
	}
}
