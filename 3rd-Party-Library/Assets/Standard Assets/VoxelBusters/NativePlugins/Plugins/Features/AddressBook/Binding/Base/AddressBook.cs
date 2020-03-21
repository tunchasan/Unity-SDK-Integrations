#if USES_ADDRESS_BOOK 
using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	/// <summary>
	/// Provides cross-platform interface to access the user’s contact information.
	/// </summary>
	// <description> 
	/// <para>
	/// Since most apps only read contact information without making changes, <see cref="AddressBook"/> provides readonly access to contact's information.
	/// </para> 
	/// <para>
	/// In iOS platform, users can grant or deny access to contact data on a per-application basis. 
	/// And the user is prompted only the first time access is requested; any subsequent <see cref="AddressBook"/> calls use the existing permissions.
	/// </para>
	///	</description> 
	public partial class AddressBook : MonoBehaviour 
	{
		#region Auth Methods

		/// <summary>
		/// Returns the current authorization status to access the contact data.
		/// </summary>
		/// <returns>The current authorization status to access the contact data.</returns>
		public virtual eABAuthorizationStatus GetAuthorizationStatus ()
		{
			return eABAuthorizationStatus.NOT_DETERMINED;
		}

		protected virtual void RequestAccess (RequestAccessCompletion _onCompletion)
		{
			// Cache callback
			RequestAccessFinishedEvent		= _onCompletion;
		}

		#endregion

		#region Read Methods

		/// <summary>
		/// Retrieves all the contact information saved in address book database.
		/// </summary>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		/// <example>
		/// The following code example demonstrates how to retrieves contacts information.
		/// <code>
		/// using UnityEngine;
		/// using System.Collections;
		/// using VoxelBusters.NativePlugins;
		/// 
		/// public class ExampleClass : MonoBehaviour 
		/// {		
		/// 	public void ReadContacts ()
		/// 	{
		/// 		NPBinding.AddressBook.ReadContacts(OnReadContactsFinished);	
		/// 	}
		/// 
		/// 	private void OnReadContactsFinished (eABAuthorizationStatus _authorizationStatus, AddressBookContact[] _contactList)
		/// 	{
		/// 		if (_authorizationStatus == eABAuthorizationStatus.AUTHORIZED)
		/// 		{
		/// 			// Insert your code to handle contact info
		/// 		}
		/// 		else
		/// 		{
		/// 			// Something went wrong
		/// 		}
		/// 	}
		/// }
		/// </code>
		/// </example>
		public void ReadContacts (ReadContactsCompletion _onCompletion)
		{
			eABAuthorizationStatus	_authStatus	= GetAuthorizationStatus();

			if (_authStatus == eABAuthorizationStatus.NOT_DETERMINED)
			{
				RequestAccess((eABAuthorizationStatus _newAuthStatus, string _error)=>{

					ReadContacts(_newAuthStatus, _onCompletion);
				});
			}
			else
			{
				ReadContacts(_authStatus, _onCompletion);
			}
		}

		protected virtual void ReadContacts (eABAuthorizationStatus _status, ReadContactsCompletion _onCompletion)
		{
			// Cache callback
			ReadContactsFinishedEvent	= _onCompletion;

			if (_status != eABAuthorizationStatus.AUTHORIZED)
			{
				ABReadContactsFinished(_status, null);
				return;
			}
		}

		#endregion
	}
}
#endif