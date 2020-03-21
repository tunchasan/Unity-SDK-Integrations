#if USES_ADDRESS_BOOK && UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public class EditorAddressBook : SharedScriptableObject<EditorAddressBook>
	{
		#region Constants
		
		private 	const 		string		kRequestAccessFinishedEvent			= "ABRequestAccessFinished";
		private 	const 		string		kReadContactsFinishedEvent			= "ABReadContactsFinished";

		#endregion

		#region Fields

		[SerializeField]
		private				eABAuthorizationStatus		m_authorizationStatus;
		[SerializeField]
		private 			AddressBookContact[] 		m_contactsList			= new AddressBookContact[0];

		#endregion

		#region Auth Methods

		public eABAuthorizationStatus GetAuthorizationStatus ()
		{
			return m_authorizationStatus;
		}
		
		public void RequestForAuthorization ()
		{
			string 				_message		= string.Format("{0} would like to access your contacts.", UnityEditor.PlayerSettings.productName);	
			string[]			_buttons		= new string[2] { 
				"Ok", 
				"Dont allow" 
			};
			
			NPBinding.UI.ShowAlertDialogWithMultipleButtons(string.Empty, _message, _buttons, (string _pressedBtn)=>{

				string			_error			= null;

				if (_pressedBtn.Equals("Ok"))
				{
					m_authorizationStatus		= eABAuthorizationStatus.AUTHORIZED;
				}
				else
				{
					_error						= "The operation could not be completed because user denied access to AddressBook.";
					m_authorizationStatus		= eABAuthorizationStatus.DENIED;
				}

				NPBinding.AddressBook.InvokeMethod(kRequestAccessFinishedEvent, new object[] { 
					m_authorizationStatus, 
					_error 
				}, new Type[] { 
					typeof(eABAuthorizationStatus), 
					typeof(string)
				});
			});
		}

		#endregion

		#region Read Contacts Methods

		public void ReadContacts ()
		{
			eABAuthorizationStatus 	_authStatus		= GetAuthorizationStatus();

			if (_authStatus == eABAuthorizationStatus.AUTHORIZED)
			{
				int 					_totalContacts		= m_contactsList.Length;
				AddressBookContact[]	_contactsListCopy	= new AddressBookContact[_totalContacts];
				
				for (int _iter = 0; _iter < _totalContacts; _iter++)
					_contactsListCopy[_iter]				= new EditorAddressBookContact(m_contactsList[_iter]);
				
				// Callback is sent to binding event listener
				SendReadContactsFinishedEvent(eABAuthorizationStatus.AUTHORIZED, _contactsListCopy);
			}
			else
			{
				SendReadContactsFinishedEvent(_authStatus, null);
				return;
			}
		}

		private void SendReadContactsFinishedEvent (eABAuthorizationStatus _authStatus, AddressBookContact[] _contactsList)
		{
			NPBinding.AddressBook.InvokeMethod(kReadContactsFinishedEvent, new object[] { 
				_authStatus, 
				_contactsList 
			}, new Type[] { 
				typeof(eABAuthorizationStatus), 
				typeof(AddressBookContact[])
			});
		}

		#endregion
	}
}
#endif