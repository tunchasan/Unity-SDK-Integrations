using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	/// <summary>
	/// Represents an object that holds information about a product registered in Store.
	/// </summary>
	[System.Serializable]
	public class BillingProduct
	{
		#region Fields

		[SerializeField]
		[Tooltip("The name of the product. This information is used for simulating feature behaviour in Editor.")]
		private 	string			m_name;
		[SerializeField]
		[Tooltip("The description of the product. This information is used for simulating feature behaviour in Editor.")]
		private 	string			m_description;
		[SerializeField]
		[Tooltip("The flag determines the product type. If enabled, product is identified as consumable.")]
		private 	bool			m_isConsumable;
		[SerializeField]
		[HideInInspector]
		private 	float			m_price;
		[Tooltip("Array of string values, where each value represents a product in a specific platform.")]
		[SerializeField]
		[FormerlySerializedAs("m_productIDs")]
		protected	PlatformValue[]	m_productIdentifiers;
		[SerializeField]
		[Tooltip("Optional extra data to let store to send back in purchase response. Used for security purposes and supported by Google's Android Platform only currently.")]
		private 	string 			m_developerPayload;

		[SerializeField]
		[HideInInspector]
		private 	string			m_iosProductId;
		[SerializeField]
		[HideInInspector]
		private 	string 			m_androidProductId;

		#endregion

		#region Properties

		/// <summary>
		/// The name of the product. (read-only)
		/// </summary>
		public string Name 
		{ 
			get	
			{ 
				return  m_name; 
			}
			protected set	
			{ 
				m_name	= value; 
			}
		}

		/// <summary>
		/// A description of the product. (read-only)
		/// </summary>
		public string Description 
		{ 
			get	
			{ 
				return  m_description; 
			} 
			protected set	
			{ 
				m_description	= value; 
			} 
		}

		/// <summary>
		/// A bool value used to identify product type. Is it Consumable/Non-Consumable product? (read-only)
		/// </summary>
		public bool IsConsumable
		{ 
			get	
			{ 
				return  m_isConsumable; 
			}
			protected set 
			{ 
				m_isConsumable	= value; 
			} 
		}

		/// <summary>
		/// The cost of the product in the local currency. (read-only)
		/// </summary>
		public float Price 
		{ 
			get	
			{ 
				return  m_price; 
			}
			protected set 
			{ 
				m_price	= value; 
			}
		}

		/// <summary>
		/// The cost of the product prefixed with local currency symbol. (read-only)
		/// </summary>
		public string LocalizedPrice 
		{ 
			get; 
			protected set; 
		}

		/// <summary>
		/// The string used as a local currency code. (read-only)
		/// </summary>
		public string CurrencyCode
		{
			get;
			protected set;
		}

		/// <summary>
		/// The string used as a local currency symbol. (read-only)
		/// </summary>
		public string CurrencySymbol
		{
			get;
			protected set;
		}

		/// <summary>
		/// The string that identifies the product to the Store. (read-only)
		/// </summary>
		public string ProductIdentifier
		{
			get 
			{
				PlatformValue _platformIdentifier	= PlatformValueHelper.GetCurrentPlatformValue(_array: m_productIdentifiers);
				if (_platformIdentifier == null)
					return null;

				return _platformIdentifier.Value;
			}
		}

		/// <summary>
		/// This is used to specify any additional arguments that you want Store to send back along with the purchase information.
		/// Note : This is currently supported by Google's Android Platform only.
		/// </summary>
		public string DeveloperPayload
		{
			get
			{
				return m_developerPayload;
			}
			set
			{
				m_developerPayload = value;
			}
		}

		#endregion

		#region Constructors

		protected BillingProduct ()
		{}

		protected BillingProduct (BillingProduct _product)
		{
			this.Name				= _product.Name;
			this.Description		= _product.Description;
			this.IsConsumable		= _product.IsConsumable;
			this.Price				= _product.Price;
			this.m_productIdentifiers		= _product.m_productIdentifiers;
			this.LocalizedPrice		= _product.LocalizedPrice;
			this.CurrencyCode		= _product.CurrencyCode;
			this.CurrencySymbol		= _product.CurrencySymbol;
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// Creates a new billing product with given information.
		/// </summary>
		/// <param name="_name">The name of the product.</param>
		/// <param name="_isConsumable">The type of the billing product. Is it Consumable/Non-Consumable product?</param>
		/// <param name="_platformIDs">An array of platform specific product identifiers.</param>
		/// <example>
		/// The following code example shows how to dynamically create billing product.
		/// <code>
		/// BillingProduct _newProduct	= BillingProduct.Create("name", true, Platform.Android("android-product-id"), Platform.IOS("ios-product-id"));
		/// </code>
		/// </example>
		public static BillingProduct Create (string _name, bool _isConsumable, params PlatformValue[] _productIDs)
		{
			BillingProduct	_newProduct	= new BillingProduct();
			_newProduct.Name			= _name;
			_newProduct.IsConsumable	= _isConsumable;
			_newProduct.m_productIdentifiers	= _productIDs;

			return _newProduct;
		}

		#endregion

		#region Public Methods

		public override string ToString ()
		{
			return string.Format ("[BillingProduct: Name={0}, ProductIdentifier={1}, IsConsumable={2}, LocalizedPrice={3}, CurrencyCode={4}, CurrencySymbol={5}]", 
			                      Name, ProductIdentifier, IsConsumable, LocalizedPrice, CurrencyCode, CurrencySymbol);
		}

		#endregion

		#region Private Methods

		internal void RebuildObject ()
		{
			if (!string.IsNullOrEmpty(m_iosProductId) || !string.IsNullOrEmpty(m_androidProductId))
			{
				// Copy product identifier information
				m_productIdentifiers	= new PlatformValue[]
				{
					PlatformValue.IOS(m_iosProductId),
					PlatformValue.Android(m_androidProductId)
				};

				// Reset deprecated attribute value
				m_iosProductId		= null;
				m_androidProductId	= null;
			}
		}

		#endregion
	}
}