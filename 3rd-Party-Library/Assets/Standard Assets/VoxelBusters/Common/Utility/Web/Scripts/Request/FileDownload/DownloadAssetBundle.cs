using UnityEngine;
using System.Collections;

namespace VoxelBusters.Utility
{
	public class DownloadAssetBundle : Request 
	{
		#region Delegates

		public delegate void Completion (AssetBundle _assetBundle, string _error);

		#endregion

		#region Properties

		public Completion OnCompletion
		{
			get;
			set;
		}

		#endregion

		#region Constructors

		public DownloadAssetBundle (URL _URL, int _version, bool _isAsynchronous) : base(_URL, _isAsynchronous)
		{
			WWWObject	= WWW.LoadFromCacheOrDownload(_URL.URLString, _version);
		}

		#endregion

		#region Handling Response

		protected override void DidFailStartRequestWithError (string _error)
		{
			if (OnCompletion != null)
				OnCompletion(null, _error);
		}

		protected override void OnFetchingResponse ()
		{			
//			Debug.Log("[DownloadAsset] Did finish downloading asset bundle. Error= " + WWWObject.error);

			if (string.IsNullOrEmpty(WWWObject.error))
			{
				if (OnCompletion != null)
					OnCompletion(WWWObject.assetBundle, null);
			}
			else
			{
				if (OnCompletion != null)
					OnCompletion(null, WWWObject.error);
			}
		}

		#endregion
	}
}