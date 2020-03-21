#if UNITY_ANDROID && USES_ADDRESS_BOOK
using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;
using System.Collections.Generic;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class AddressBookAndroid : AddressBook 
	{
		#region Parsing constants

		//Access status flags
		private const string	kContactsListKey	= "contacts-list";
		private const string	kAuthStatusKey		= "auth-status";
		private const string	kAccessAuthorized	= "authorized";
		private const string	kAccessDenied		= "denied";
		private const string	kAccessRestricted	= "restricted";
		
		#endregion

		#region Parsing Methods
		protected override void ParseReadContactsResponseData (IDictionary _dataDict, out eABAuthorizationStatus _authStatus, out AddressBookContact[] _contactsList)
		{
			List<object> 	_contactsJSONList		= _dataDict.GetIfAvailable<List<object>>(kContactsListKey);
			
			if (_contactsJSONList != null)
			{
				int						_count				= _contactsJSONList.Count;
				AddressBookContact[]	_newContactsList	= new AndroidAddressBookContact[_count];
				
				for (int _iter = 0; _iter < _count; _iter++)
					_newContactsList[_iter]		= new AndroidAddressBookContact((IDictionary)_contactsJSONList[_iter]);
				
				// Set properties
				_authStatus		= eABAuthorizationStatus.AUTHORIZED;
				_contactsList	= _newContactsList;		
			}
			else
			{
				// Set properties
				_authStatus		= GetAuthorizationStatus(_dataDict.GetIfAvailable<string>(kAuthStatusKey));
				_contactsList	= null;	
			}
		}

		private eABAuthorizationStatus GetAuthorizationStatus (string _statusStr)
		{
			eABAuthorizationStatus _authStatus;

			if(kAccessAuthorized.Equals(_statusStr))
			{
				_authStatus = eABAuthorizationStatus.AUTHORIZED;
			}
			else if(kAccessDenied.Equals(_statusStr))
			{
				_authStatus = eABAuthorizationStatus.DENIED;
			}
			else if(kAccessRestricted.Equals(_statusStr))
			{
				_authStatus = eABAuthorizationStatus.RESTRICTED;
			}
			else
			{
				_authStatus = eABAuthorizationStatus.DENIED;
				DebugUtility.Logger.LogError(Constants.kDebugTag, "[AddressBook] Wrong parse status " + _statusStr + " " + "Cross check keys with native. Sending DENIED status by default");
			}

			return _authStatus;
		}

		#endregion
	}
}
#endif