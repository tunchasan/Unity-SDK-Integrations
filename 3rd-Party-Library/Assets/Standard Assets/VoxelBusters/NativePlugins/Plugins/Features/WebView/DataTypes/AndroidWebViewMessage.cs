using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed class AndroidWebViewMessage : WebViewMessage  
	{
//		{
//			"host": "move",
//			"arguments": {
//				"cmd": "showAlert",
//				"var": "myVar"
//			},
//			"url-scheme": "unity"
//		}

		#region Constants

		private const string 	kURL		= "url";
		private const string 	kHost		= "host";
		private const string 	kArguments	= "arguments";
		private const string 	kURLScheme	= "url-scheme";

		#endregion

		#region Constructor

		public AndroidWebViewMessage (IDictionary _schemeDataJsonDict)
		{
			string _url			= _schemeDataJsonDict.GetIfAvailable<string>(kURL);
			string _scheme		= _schemeDataJsonDict.GetIfAvailable<string>(kURLScheme);
			string _host		= _schemeDataJsonDict.GetIfAvailable<string>(kHost);
			IDictionary _args	= _schemeDataJsonDict[kArguments] as IDictionary;

			// Assign value
			URL					= _url;
			Scheme				= _scheme;
			Host				= _host;
			Arguments			= new Dictionary<string, string>();

			foreach (object _key in _args.Keys)
			{
				string _keyStr		= _key.ToString();
				string _valueStr	= _args[_key].ToString();

				// Add key and value
				Arguments[_keyStr]	= _valueStr;
			}
		}

		#endregion
	}
}