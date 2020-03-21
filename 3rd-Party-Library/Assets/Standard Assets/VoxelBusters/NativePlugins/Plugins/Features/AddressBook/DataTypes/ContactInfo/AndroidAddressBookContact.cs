using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed class AndroidAddressBookContact : AddressBookContact
	{
//		{
//			"emailID-list": [
//			    "joey@actingclass.com"
//			    ],
//			"image-path": "/storage/emulated/0/Android/data/com.company.product/files/1.png",
//			"last-name": "Joey",
//			"phone-number-list": [
//			    "911"
//			    ],
//			"first-name": "Tribbiani"
//		}

		#region Constants

		private const string	kDisplayName		= "display-name";
		private const string	kFamilyName			= "family-name";
		private const string	kGivenName			= "given-name";
		private const string	kImagePath			= "image-path";
		private const string	kPhoneNumList		= "phone-number-list";
		private const string	kEmailList			= "email-list";
		
		#endregion

		#region Constructors

		public AndroidAddressBookContact (IDictionary _contactInfoJsontDict)
		{
			string _givenName 	= _contactInfoJsontDict.GetIfAvailable<string>(kGivenName);
			string _familyName 	= _contactInfoJsontDict.GetIfAvailable<string>(kFamilyName);
		
			// Set first name and last name
			FirstName 		= _givenName;
			LastName 		= _familyName;
			ImagePath		= _contactInfoJsontDict.GetIfAvailable<string>(kImagePath);

			// Set phone numbers
			IList 		_phoneNumJSONList	= _contactInfoJsontDict.GetIfAvailable<IList>(kPhoneNumList);
			string[] 	_newPhoneNumList	= null;
			
			if (_phoneNumJSONList != null)
			{
				int		_totalCount			= _phoneNumJSONList.Count;
				_newPhoneNumList			= new string[_totalCount];
				
				for (int _iter = 0; _iter < _totalCount; _iter++)
					_newPhoneNumList[_iter]	= (string)_phoneNumJSONList[_iter];
			}
			
			PhoneNumberList		= _newPhoneNumList;
			
			// Set email id list
			IList 		_emailIDJsonList	= _contactInfoJsontDict.GetIfAvailable<IList>(kEmailList);
			string[] 	_newEmailIDList		= null;
			
			if (_emailIDJsonList != null)
			{
				int		_totalCount			= _emailIDJsonList.Count;
				_newEmailIDList				= new string[_totalCount];
				
				for (int _iter = 0; _iter < _totalCount; _iter++)
					_newEmailIDList[_iter]	= (string)_emailIDJsonList[_iter];
			}
			
			EmailIDList			= _newEmailIDList;
		}

		#endregion
	}
}