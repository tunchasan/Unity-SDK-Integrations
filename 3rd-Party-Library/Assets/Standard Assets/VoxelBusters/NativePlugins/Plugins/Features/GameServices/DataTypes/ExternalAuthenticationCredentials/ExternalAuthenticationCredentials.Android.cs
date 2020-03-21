using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	public partial class ExternalAuthenticationCredentials
	{
		public class Android
		{
			#region Constants
			private		const	string 		kServerAuthCodeKey	= "server-auth-code";
			#endregion

			public string ServerAuthCode
			{
				get; 
				set;
			}

			public void Load(IDictionary _jsonDict)
			{
				string _authCodeEncoded = _jsonDict.GetIfAvailable<string>(kServerAuthCodeKey);
				ServerAuthCode = _authCodeEncoded.FromBase64();
			}
		}
	}
}