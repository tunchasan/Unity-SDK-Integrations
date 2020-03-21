using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed class iOSAddressBookContact : AddressBookContact 
	{
//		{
//			"emailID-list": [
//			    "joey@actingclass.com"
//			    ],
//			"image-path": "file://localhost/var/mobile/Applications/ED14DC3D-37C4-44D2-B750-CC595DE48D78/Documents/207D078C-5AE6-4B7F-BEA2-EE1045B27C82.jpg",
//			"last-name": "Joey",
//			"phone-number-list": [
//			    "911"
//			    ],
//			"first-name": "Tribbiani"
//		}

		#region Constants

		private const string 		kLastName			= "last-name";
		private const string 		kImagePath			= "image-path";		
		private const string 		kFirstName			= "first-name";
		private const string 		kPhoneNumList		= "phone-number-list";
		private const string 		kEmailIDList		= "emailID-list";
		
		#endregion

		#region Constructors

		public iOSAddressBookContact (IDictionary _contactInfoDict)
		{
			// Set first name and last name
			FirstName		= _contactInfoDict.GetIfAvailable<string>(kFirstName);
			LastName		= _contactInfoDict.GetIfAvailable<string>(kLastName);
			ImagePath		= _contactInfoDict.GetIfAvailable<string>(kImagePath);

			// Set phone numbers
			IList 		_phoneNumJSONList	= _contactInfoDict.GetIfAvailable<IList>(kPhoneNumList);
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
			IList 		_emailIDJsonList	= _contactInfoDict.GetIfAvailable<IList>(kEmailIDList);
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