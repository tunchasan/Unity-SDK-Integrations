using UnityEngine;
using System.Collections;

#if USES_ADDRESS_BOOK && UNITY_IOS
using System.Runtime.InteropServices;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{	
	using Internal;

	public partial class AddressBookIOS : AddressBook 
	{		
		#region Native Methods

		[DllImport("__Internal")]
		private static extern int getAuthorizationStatus();
		
		[DllImport("__Internal")]
		private static extern void requestAccess ();

		[DllImport("__Internal")]
		private static extern void readContacts ();

		#endregion
		
		#region Overriden API's

		public override eABAuthorizationStatus GetAuthorizationStatus ()
		{
			iOSABAuthorizationStatus _iOSAuthStatus	= (iOSABAuthorizationStatus)getAuthorizationStatus();

			return ConvertFromNativeAuthorizationStatus(_iOSAuthStatus);
		}

		protected override void RequestAccess (RequestAccessCompletion _onCompletion)
		{
			base.RequestAccess (_onCompletion);

			// Native method call
			requestAccess();
		}

		protected override void ReadContacts (eABAuthorizationStatus _status, ReadContactsCompletion _onCompletion)
		{
			base.ReadContacts (_status, _onCompletion);

			if (_status != eABAuthorizationStatus.AUTHORIZED)
				return;

			// Native method call
			readContacts();
		}

		#endregion
	}
}
#endif