#if USES_ADDRESS_BOOK 
using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class AddressBook : MonoBehaviour 
	{
		#region Delegates

		protected delegate void RequestAccessCompletion	(eABAuthorizationStatus _authorizationStatus, string _error);

		/// <summary>
		/// Delegate that will be called after contacts information stored in address book database are retrieved.
		/// </summary>
		/// <param name="_authorizationStatus">The current authorization status to access the contact data.</param>
		/// <param name="_contactList">An array of <see cref="AddressBookContact"/> objects, that holds contacts information stored in address book database.</param>
		public delegate void ReadContactsCompletion (eABAuthorizationStatus _authorizationStatus, AddressBookContact[] _contactList);

		#endregion

		#region Events

		protected RequestAccessCompletion RequestAccessFinishedEvent;
		protected ReadContactsCompletion ReadContactsFinishedEvent;

		#endregion

		#region Auth Methods

		protected virtual void ABRequestAccessFinished (string _dataStr)
		{}

		protected void ABRequestAccessFinished (eABAuthorizationStatus _authStatus, string _error)
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, string.Format("[AddressBook] Request access finished. AuthStatus={0}, Error={1}.", _authStatus, _error.GetPrintableString()));
	
			if (RequestAccessFinishedEvent != null)
				RequestAccessFinishedEvent(_authStatus, _error);
		}

		#endregion

		#region Read Contacts Methods

		private void ABReadContactsFinished (string _data)
		{	
			eABAuthorizationStatus	_authStatus;
			AddressBookContact[]	_contactsList;
			IDictionary				_dataDict		= JSONUtility.FromJSON(_data) as IDictionary;

			// Parse response
			ParseReadContactsResponseData(_dataDict, out _authStatus, out _contactsList);

			// Invoke handler
			ABReadContactsFinished(_authStatus, _contactsList);
		}

		private void ABReadContactsFinished (eABAuthorizationStatus _authStatus, AddressBookContact[] _contactsList)
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, string.Format("[AddressBook] Read contacts finished. Status= {0}.", _authStatus));
			
			if (ReadContactsFinishedEvent != null)
				ReadContactsFinishedEvent(_authStatus, _contactsList);
		}

		#endregion 

		#region Parsing Methods

		protected virtual void ParseReadContactsResponseData (IDictionary _dataDict, out eABAuthorizationStatus _authStatus, out AddressBookContact[] _contactsList)
		{
			_contactsList	= null;
			_authStatus		= eABAuthorizationStatus.DENIED;
		}

		#endregion
	}
}
#endif