using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed class iOSWebViewMessage : WebViewMessage
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

		private const string 	kURLKey			= "url";
		private const string 	kHostKey		= "host";
		private const string 	kArgumentsKey	= "arguments";
		private const string 	kURLSchemeKey	= "url-scheme";

		#endregion

		#region Constructor

//		{
//			"tag": "tag",
//			"message-data": {
//				"host": "move",
//				"arguments": {
//					"cmd": "showAlert",
//					"var": "myVar"
//				},
//				"url-scheme": "unity"
//			}
//		}

		public iOSWebViewMessage (IDictionary _schemeDataJsonDict)
		{
			string		_URL		= _schemeDataJsonDict.GetIfAvailable<string>(kURLKey);
			string 		_schemeName	= _schemeDataJsonDict.GetIfAvailable<string>(kURLSchemeKey);
			string 		_host		= _schemeDataJsonDict.GetIfAvailable<string>(kHostKey);
			IDictionary _args		= _schemeDataJsonDict.GetIfAvailable<IDictionary>(kArgumentsKey);

			// Set properties
			this.URL				= _URL;
			this.Scheme				= _schemeName;
			this.Host				= _host;
			this.Arguments			= new Dictionary<string, string>();

			if (_args != null)
			{
				foreach (string _key in _args.Keys)
				{
					string _keyStr		= _key.ToString();
					string _valueStr	= _args[_key].ToString();

					// Add key and value
					Arguments[_keyStr]	= _valueStr;
				}
			}
		}

		#endregion
	}
}