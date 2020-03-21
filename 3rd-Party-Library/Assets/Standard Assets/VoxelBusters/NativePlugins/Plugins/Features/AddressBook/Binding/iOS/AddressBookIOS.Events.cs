using UnityEngine;
using System.Collections;

#if USES_ADDRESS_BOOK && UNITY_IOS
using System;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class AddressBookIOS : AddressBook 
	{
		private enum iOSABAuthorizationStatus
		{
			kABAuthorizationStatusNotDetermined = 0,
			kABAuthorizationStatusRestricted,
			kABAuthorizationStatusDenied,
			kABAuthorizationStatusAuthorized
		};

		#region Constants

		private		const		string 		kContactsListKey	= "contacts-list";
		private		const		string 		kAuthStatusKey		= "auth-status";
		private		const		string 		kErrorKey			= "error";

		#endregion

		#region Methods

		protected override void ABRequestAccessFinished (string _dataStr)
		{
			IDictionary		_dataDict			= (IDictionary)JSONUtility.FromJSON(_dataStr);
			string			_error				=  _dataDict.GetIfAvailable<string>(kErrorKey);
			eABAuthorizationStatus _authStatus	= ConvertFromNativeAuthorizationStatus(_dataDict.GetIfAvailable<iOSABAuthorizationStatus>(kAuthStatusKey));

			// Invoke handler
			ABRequestAccessFinished(_authStatus, _error);
		}

		protected override void ParseReadContactsResponseData (IDictionary _dataDict, out eABAuthorizationStatus _authStatus, out AddressBookContact[] _contactsList)
		{
			IList 			_contactsJSONList	= _dataDict.GetIfAvailable<IList>(kContactsListKey);

			if (_contactsJSONList != null)
			{
				int						_count				= _contactsJSONList.Count;
				AddressBookContact[]	_newContactsList	= new iOSAddressBookContact[_count];

				for (int _iter = 0; _iter < _count; _iter++)
					_newContactsList[_iter]					= new iOSAddressBookContact((IDictionary)_contactsJSONList[_iter]);

				// Set properties
				_authStatus		= eABAuthorizationStatus.AUTHORIZED;
				_contactsList	= _newContactsList;		
			}
			else
			{
				// Set properties
				_authStatus		= ConvertFromNativeAuthorizationStatus(_dataDict.GetIfAvailable<iOSABAuthorizationStatus>(kAuthStatusKey));
				_contactsList	= null;	
			}
		}

		#endregion

		#region Misc. Methods

		private eABAuthorizationStatus ConvertFromNativeAuthorizationStatus (iOSABAuthorizationStatus _iOSAuthStatus)
		{
			switch (_iOSAuthStatus)
			{
			case iOSABAuthorizationStatus.kABAuthorizationStatusNotDetermined:
				return eABAuthorizationStatus.NOT_DETERMINED;

			case iOSABAuthorizationStatus.kABAuthorizationStatusRestricted:
				return eABAuthorizationStatus.RESTRICTED;

			case iOSABAuthorizationStatus.kABAuthorizationStatusDenied:
				return eABAuthorizationStatus.DENIED;

			case iOSABAuthorizationStatus.kABAuthorizationStatusAuthorized:
				return eABAuthorizationStatus.AUTHORIZED;

			default:
				throw new Exception("[AddressBook] Unsupported status.");
			}
		}

		#endregion
	}
}
#endif