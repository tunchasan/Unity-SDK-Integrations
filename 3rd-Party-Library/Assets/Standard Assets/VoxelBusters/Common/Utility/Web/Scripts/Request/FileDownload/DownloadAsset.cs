using UnityEngine;
using System.Collections;

namespace VoxelBusters.Utility
{
	public class DownloadAsset : Request 
	{
		#region Delegates

		public delegate void Completion (WWW _www, string _error);

		#endregion

		#region Properties

		public Completion OnCompletion
		{
			get;
			set;
		}

		#endregion

		#region Constructors
		
		public DownloadAsset (URL _URL, bool _isAsynchronous) : base(_URL, _isAsynchronous)
		{
			WWWObject	= new WWW(_URL.URLString);
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
//			Debug.Log("[DownloadAsset] Did finish downloading, Error=" + WWWObject.error);

			if (string.IsNullOrEmpty(WWWObject.error))
			{
				if (OnCompletion != null)
					OnCompletion(WWWObject, null);
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
