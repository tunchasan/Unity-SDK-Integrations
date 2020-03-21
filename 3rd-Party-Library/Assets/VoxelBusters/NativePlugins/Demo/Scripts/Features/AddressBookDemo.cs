using UnityEngine;
using System.Collections;
using VoxelBusters.NativePlugins;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins.Demo
{
	using Internal;

#if !USES_ADDRESS_BOOK
	public class AddressBookDemo : NPDisabledFeatureDemo 
#else
	public class AddressBookDemo : NPDemoBase
#endif
	{
		#region Properties

#pragma warning disable
		// UI Settings
		private 	float 					m_eachColumnWidth;
		private 	float 					m_eachRowHeight 				= 150f;
		private 	int 					m_maxContactsToRender 			= 50;

		// Data holders
		private 	AddressBookContact[] 	m_contactsInfo = null;
		private 	Texture[] 				m_contactPictures;

		// Misc
		private 	GUIScrollView 			m_contactsScrollView;
#pragma warning restore

		#endregion

#if !USES_ADDRESS_BOOK
	}
#else
		#region Unity Methods

		protected override void Start()
		{
			base.Start();

			// Initialise
			m_contactsScrollView = gameObject.AddComponent<GUIScrollView>();

			// Set additional info texts
			AddExtraInfoTexts(
				"You can add dummy contact information to Editor AddressBook to simulate this feature in Unity Editor. It is accessible from Menu (Window->Voxel Busters->Native Plugins).");
		}	

		#endregion

		#region GUI Methods
		
		protected override void DisplayFeatureFunctionalities ()
		{		
			base.DisplayFeatureFunctionalities();
			
			GUILayout.Label("Authorization Methods", kSubTitleStyle);
			
			if (GUILayout.Button("Get Authorization Status"))
			{
				AddNewResult("Authorization Status = " + GetAuthorizationStatus());
			}
			
			GUILayout.Label("Contact Methods", kSubTitleStyle);
			
			if (GUILayout.Button("Read Contacts"))
			{
				ReadContacts();
			}
			
			DrawContactsInfoList();	
		}

		private void DrawContactsInfoList ()
		{
			if (m_contactsInfo == null || m_contactsInfo.Length == 0)
				return;

			m_eachColumnWidth = (GetWindowWidth() - GetWindowWidth()*0.1f)/5;
			GUILayoutOption _entryWidthOption 		= GUILayout.Width(m_eachColumnWidth);
			GUILayoutOption _entryHeightOption 		= GUILayout.Height(m_eachRowHeight);
			GUILayoutOption _entryHalfHeightOption 	= GUILayout.Height(m_eachRowHeight/2);
			
			GUILayout.BeginHorizontal();
			{
				GUILayout.Box("Picture"		, kSubTitleStyle, _entryWidthOption, _entryHalfHeightOption);
				GUILayout.Box("First Name"	, kSubTitleStyle, _entryWidthOption, _entryHalfHeightOption);
				GUILayout.Box("Last Name"	, kSubTitleStyle, _entryWidthOption, _entryHalfHeightOption);
				GUILayout.Box("Phone #'s"	, kSubTitleStyle, _entryWidthOption, _entryHalfHeightOption);
				GUILayout.Box("Email ID's"	, kSubTitleStyle, _entryWidthOption, _entryHalfHeightOption);
			}					
			GUILayout.EndHorizontal();
			
			m_contactsScrollView.BeginScrollView();
			{
				for(int _i = 0; _i < m_contactsInfo.Length ; _i++)
				{
					if (_i > m_maxContactsToRender) //This is just to limit drawing
					{
						break;
					}
					
					AddressBookContact _eachContact = m_contactsInfo[_i];
					GUILayout.BeginHorizontal();
					{							
						GUILayout.Label(m_contactPictures[_i]					, _entryWidthOption, _entryHeightOption);
						GUILayout.Label(_eachContact.FirstName					, _entryWidthOption, _entryHeightOption);
						GUILayout.Label(_eachContact.LastName					, _entryWidthOption, _entryHeightOption);
						
						int _oldFontSize = UISkin.label.fontSize;
						UISkin.label.fontSize = (int)(_oldFontSize * 0.5);
						
						GUILayout.Label(_eachContact.PhoneNumberList.ToJSON()	, _entryWidthOption, _entryHeightOption);
						GUILayout.Label(_eachContact.EmailIDList.ToJSON()		, _entryWidthOption, _entryHeightOption);
						
						UISkin.label.fontSize = _oldFontSize;
					}
					GUILayout.EndHorizontal();
					
				}
			}
			m_contactsScrollView.EndScrollView();
		}
		
		#endregion

		#region API Methods
		
		private eABAuthorizationStatus GetAuthorizationStatus ()
		{
			return NPBinding.AddressBook.GetAuthorizationStatus();
		}

		private void ReadContacts()
		{
			AddNewResult("Started reading contacts in background. Please wait...");
			NPBinding.AddressBook.ReadContacts(OnReceivingContacts);			
		}

		private void LoadContactsImageAtIndex (int _index)
		{
			AddressBookContact 	_contactInfo	= m_contactsInfo[_index];

			_contactInfo.GetImageAsync((Texture2D _texture, string _error)=>{
				if (!string.IsNullOrEmpty(_error))
				{
					DebugUtility.Logger.LogError(Constants.kDebugTag, "[AddressBook] Contact Picture download failed " + _error);
					m_contactPictures[_index] = null;
				}
				else
				{
					m_contactPictures[_index] = _texture;
				}
			});
		}
		
		#endregion
	
		#region API Callback Methods

		private void OnReceivingContacts (eABAuthorizationStatus _authorizationStatus, AddressBookContact[] _contactList)
		{
			AddNewResult(string.Format("Read contacts request finished. Authorization Status = {0}.", _authorizationStatus));

			if (_contactList != null)
			{
				AppendResult(string.Format("Total no of contacts info fetched is {0}.", _contactList.Length));

				// Cache received contacts info
				m_contactsInfo				= _contactList;

				// Start loading images
				int		_totalContacts		= _contactList.Length;
				m_contactPictures 			= new Texture[_totalContacts];
					
				for (int _iter = 0; _iter < _totalContacts; _iter++)
					LoadContactsImageAtIndex(_iter);
			}
		}

		private void AddNewContactFinished (bool _status, string _error)
		{
			AddNewResult(string.Format("Add new contact information request finished. Status = {0}. Error = {1}.", _status, _error.GetPrintableString()));
		}

		#endregion
	}
#endif
}