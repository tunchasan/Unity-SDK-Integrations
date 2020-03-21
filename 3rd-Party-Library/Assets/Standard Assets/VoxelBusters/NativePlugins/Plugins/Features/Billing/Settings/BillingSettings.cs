using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	/// <summary>
	/// Provides an interface for accessing billing feature configuration.
	/// </summary>
	[System.Serializable]
	public partial class BillingSettings 
	{
		#region Fields

		[SerializeField] 
		[Tooltip("An array of billing products registered in Store.")]
		private 		List<BillingProduct>	m_products;
		[SerializeField]
		private 		iOSSettings				m_iOS;
		[SerializeField]
		//[InspectorButton(eInspectorButtonPosition.BOTTOM, "Clear Purchase History", "Removes all the information about the billing products purchased on Editor.", "ClearPurchaseHistory")]
		private 		AndroidSettings			m_android;

		#endregion

		#region Properties

		/// <summary>
		/// An array of billing products registered in Store. (read-only)
		/// </summary>
		public BillingProduct[] Products
		{
			get 
			{ 
				if (m_products.Count == 0)
					return null;

				return m_products.ToArray(); 
			}

			private set 
			{ 
				m_products	= new List<BillingProduct>(value); 
			}
		}

		public iOSSettings iOS
		{
			get 
			{ 
				return m_iOS; 
			}

			private set 
			{ 
				m_iOS = value; 
			}
		}

		public AndroidSettings Android
		{
			get 
			{ 
				return m_android; 
			}

			private set 
			{ 
				m_android = value; 
			}
		}

		#endregion

		#region Constructors

		public BillingSettings ()
		{
			Products	= new BillingProduct[0];
			iOS			= new iOSSettings();
			Android		= new AndroidSettings();
		}

		#endregion
	}
}