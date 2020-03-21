using UnityEngine;
using System.Collections;

#if USES_ADDRESS_BOOK && UNITY_EDITOR
namespace VoxelBusters.NativePlugins
{	
	using Internal;

	public partial class AddressBookEditor : AddressBook 
	{		
		#region Overriden API's

		public override eABAuthorizationStatus GetAuthorizationStatus ()
		{
			return EditorAddressBook.Instance.GetAuthorizationStatus();
		}

		protected override void RequestAccess (RequestAccessCompletion _onCompletion)
		{
			base.RequestAccess (_onCompletion);

			// Request for auth
			EditorAddressBook.Instance.RequestForAuthorization();
		}

		protected override void ReadContacts (eABAuthorizationStatus _status, ReadContactsCompletion _onCompletion)
		{
			base.ReadContacts (_status, _onCompletion);

			if (_status != eABAuthorizationStatus.AUTHORIZED)
				return;

			// Requesting for contacts info
			EditorAddressBook.Instance.ReadContacts();
		}

		#endregion
	}
}
#endif