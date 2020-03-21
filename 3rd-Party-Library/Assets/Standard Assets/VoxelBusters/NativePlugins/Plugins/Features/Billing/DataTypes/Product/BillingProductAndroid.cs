using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;
using System.Globalization;

namespace VoxelBusters.NativePlugins.Internal
{
	/*
		"productId":"54units",
		"type":"inapp",
		"price":"Rs. 60.00",
		"price_amount_micros":60000000,
		"price_currency_code":"INR",
		"title":"Units_54",
		"description":"54 units"
	*/
	
	public sealed class BillingProductAndroid : BillingProductMutable 
	{
		#region Constants

		private const string	kProductIdentifier		= "product-identifier";
		private const string	kLocalisedPrice			= "localised-price";
		private const string	kPriceAmount			= "price-amount-micros";
		private const string	kPriceCurrencyCode		= "currency-code";
		private const string	kPriceCurrencySymbol	= "currency-symbol";
		private const string	kName					= "name";
		private const string	kDescription			= "description";

		#endregion

		#region Constructors
		
		public BillingProductAndroid (IDictionary _productJsonDict)
		{
			m_productIdentifiers		= new PlatformValue[]
			{
				PlatformValue.Android(_productJsonDict[kProductIdentifier] as string)
			};
			Name				= _productJsonDict[kName] as string;
			Description			= _productJsonDict[kDescription] as string;
			Price				= _productJsonDict.GetIfAvailable<long>(kPriceAmount)/1000000.0f;//As the value is in microns
			LocalizedPrice		= _productJsonDict.GetIfAvailable<string>(kLocalisedPrice);
			CurrencyCode		= _productJsonDict.GetIfAvailable<string>(kPriceCurrencyCode);
			CurrencySymbol		= _productJsonDict.GetIfAvailable<string>(kPriceCurrencySymbol);
		}

		#endregion

		#region Static Methods

		public static IDictionary CreateJSONObject (BillingProduct _product)
		{
			IDictionary _productJsonDict			= new Dictionary<string, string>();
			_productJsonDict[kProductIdentifier]	= _product.ProductIdentifier;
			_productJsonDict[kName]					= _product.Name;
			_productJsonDict[kDescription]			= _product.Description;
			_productJsonDict[kPriceAmount]			= (_product.Price * 1000000).ToString();
			_productJsonDict[kLocalisedPrice]		= _product.LocalizedPrice;
			_productJsonDict[kPriceCurrencyCode]	= _product.CurrencyCode;
			_productJsonDict[kPriceCurrencySymbol]	= _product.CurrencySymbol;

			return _productJsonDict;
		}

		#endregion
	}
}