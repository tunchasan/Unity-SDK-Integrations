using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	public partial class BillingSettings 
	{
		[System.Serializable]
		public class AndroidSettings
		{
			#region Fields

			[SerializeField]
			[Tooltip("The public key provided by Google Play service for In-app Billing.")]
			private 	string		m_publicKey	= null;

			#endregion

			#region Properties

			public string PublicKey
			{
				get 
				{ 
					return m_publicKey; 
				}
			}

			#endregion
		}
	}
}