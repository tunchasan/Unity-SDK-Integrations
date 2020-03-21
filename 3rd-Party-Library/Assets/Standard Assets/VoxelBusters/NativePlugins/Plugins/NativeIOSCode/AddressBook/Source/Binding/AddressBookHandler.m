//
//  AddressBookHandler.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 10/12/14.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "AddressBookHandler.h"
#import "NSData+UIImage.h"

@implementation AddressBookHandler

#define kLastName							@"last-name"
#define kImagePath							@"image-path"
#define kFirstName							@"first-name"
#define kPhoneNumList						@"phone-number-list"
#define kEmailIDList						@"emailID-list"

#define	kContactList						@"contacts-list"
#define	kAuthStatus							@"auth-status"
#define kError								@"error"

#define kABReadContactsFinished				"ABReadContactsFinished"
#define kABRequestAccessFinished	 		"ABRequestAccessFinished"

#pragma mark - Auth Methods

- (ContactsAuthorizationStatus)getAuthorizationStatus
{
#ifdef __IPHONE_9_0
	if (SYSTEM_VERSION_GREATER_THAN_OR_EQUAL_TO(@"9.0"))
		return [CNContactStore authorizationStatusForEntityType:CNEntityTypeContacts];
#endif
	
	return ABAddressBookGetAuthorizationStatus();
}

- (void)requestAccess
{
#ifdef __IPHONE_9_0
	if (SYSTEM_VERSION_GREATER_THAN_OR_EQUAL_TO(@"9.0"))
	{
		CNAuthorizationStatus authStatus 	= [CNContactStore authorizationStatusForEntityType:CNEntityTypeContacts];
		
		if (authStatus == CNAuthorizationStatusNotDetermined)
		{
			CNContactStore *contactStore	= [[[CNContactStore alloc] init] autorelease];
			
			[contactStore requestAccessForEntityType:CNEntityTypeContacts completionHandler:^(BOOL granted, NSError *__nullable error) {
				
				// Invoke handler
				[self onRequesAccessFinished:[self getAuthorizationStatus] error:(NSError *)error];
			}];
		}
		else
		{
			// Invoke handler
			[self onRequesAccessFinished:authStatus error:nil];
		}
	}
	else
#endif
	{
		if (ABAddressBookRequestAccessWithCompletion != NULL)
		{
			ABAuthorizationStatus authorisationStatus	= ABAddressBookGetAuthorizationStatus();
			
			// Request access permission from user
			if (authorisationStatus == kABAuthorizationStatusNotDetermined)
			{
				CFErrorRef *error               = nil;
				ABAddressBookRef addressBook    = ABAddressBookCreateWithOptions(nil, error);

				ABAddressBookRequestAccessWithCompletion(addressBook, ^(bool granted, CFErrorRef error) {
					
					// Invoke handler
					[self onRequesAccessFinished:[self getAuthorizationStatus] error:(NSError *)error];
				});
				
				CFRelease(addressBook);
			}
			else
			{
				// Invoke handler
				[self onRequesAccessFinished:authorisationStatus error:nil];
			}
		}
		else
		{
			// Invoke handler
			[self onRequesAccessFinished:kABAuthorizationStatusAuthorized error:nil];
		}
	}
}

- (void)onRequesAccessFinished:(ContactsAuthorizationStatus)authorisationStatus error:(NSError *)error
{
	// Notify Unity
	NSMutableDictionary *dataDict	= [NSMutableDictionary dictionary];
	
	[dataDict setObject:[NSNumber numberWithInt:authorisationStatus] forKey:kAuthStatus];
	
	if (error)
	{
		[dataDict setObject:[error description] forKey:kError];
	}
	
	NotifyEventListener(kABRequestAccessFinished, ToJsonCString(dataDict));
}

#pragma mark - Read Contacts Methods

- (void)readContacts
{
	NSCharacterSet *phoneNumExcludedCharacterSet	= [[NSCharacterSet characterSetWithCharactersInString:@"0123456789+"] invertedSet];
	
#ifdef __IPHONE_9_0
	if (SYSTEM_VERSION_GREATER_THAN_OR_EQUAL_TO(@"9.0"))
	{
		CNContactStore *contactStore		= [[[CNContactStore alloc] init] autorelease];
		NSMutableArray *contactsList		= [NSMutableArray array];
		BOOL finished						= NO;

		// Set fetch request properties
		CNContactFetchRequest *fetchRequest	= [[CNContactFetchRequest alloc] initWithKeysToFetch:@[CNContactGivenNameKey,
																								   CNContactFamilyNameKey,
																								   CNContactImageDataKey,
																								   CNContactPhoneNumbersKey,
																								   CNContactEmailAddressesKey]];
		[fetchRequest setUnifyResults:YES];
		[fetchRequest setSortOrder:CNContactSortOrderGivenName];
	
		do
		{
			
			finished = [contactStore enumerateContactsWithFetchRequest:fetchRequest
																 error:nil
															usingBlock:^(CNContact * _Nonnull contact, BOOL * _Nonnull stop) {
																
																// Add object
																if (contact != NULL)
																	[contactsList addObject:contact];
															}];
		} while (!finished);
		
		// Once we have all the contacts info convert it to json format
		NSMutableArray *contactsJSONList    = [NSMutableArray array];

		for (CNContact *eachContact in contactsList)
		{
			NSMutableDictionary *contactInfoDict    = [NSMutableDictionary dictionary];
			
			if (eachContact.givenName != NULL)
				contactInfoDict[kFirstName]	= eachContact.givenName;
			
			if (eachContact.familyName != NULL)
				contactInfoDict[kLastName]	= eachContact.familyName;
			
			if (eachContact.imageData != NULL)
				contactInfoDict[kImagePath]	= [eachContact.imageData saveImage];
			
			if (eachContact.phoneNumbers != NULL)
			{
				NSMutableArray *phoneNumList	= [NSMutableArray array];
				
				for (CNLabeledValue<CNPhoneNumber*> *phoneNumber in eachContact.phoneNumbers)
				{
					NSString *rawFormatNumber	= [[phoneNumber value] stringValue];
					NSString *formattedNumber	= [[rawFormatNumber componentsSeparatedByCharactersInSet:phoneNumExcludedCharacterSet] componentsJoinedByString:@""];
					
					[phoneNumList addObject:formattedNumber];
				}
				
				contactInfoDict[kPhoneNumList]	= phoneNumList;
			}
			
			if (eachContact.emailAddresses != NULL)
			{
				NSMutableArray *emailIDList		= [NSMutableArray array];
				
				for (CNLabeledValue<NSString*> *emailID in eachContact.emailAddresses)
					[emailIDList addObject:[emailID value]];
				
				contactInfoDict[kEmailIDList]	= emailIDList;
			}
			
			// Add this info to json list
			[contactsJSONList addObject:contactInfoDict];
		}
		
		// Invoke handler
		[self onReadContactsFinished:contactsJSONList error:nil];
	}
	else
#endif
	{
		CFErrorRef *error              	 	= nil;
		ABAddressBookRef addressBook   	 	= ABAddressBookCreateWithOptions(nil, error);
		CFArrayRef allPeople            	= ABAddressBookCopyArrayOfAllPeopleInSourceWithSortOrdering(addressBook, nil, kABPersonFirstNameProperty);
		CFIndex totalContacts           	= CFArrayGetCount(allPeople);
		NSMutableArray *contactsJSONList    = [NSMutableArray arrayWithCapacity:totalContacts];
		
		for (int iter = 0; iter < totalContacts; iter++)
		{
			ABRecordRef person                      = CFArrayGetValueAtIndex(allPeople, iter);
			NSMutableDictionary *eachContactData    = [NSMutableDictionary dictionary];
			
			// Get name info
			NSString *firstName 			= (NSString *)CFBridgingRelease(ABRecordCopyValue(person, kABPersonFirstNameProperty));
			NSString *lastName  			= (NSString *)CFBridgingRelease(ABRecordCopyValue(person, kABPersonLastNameProperty));
			
			if (firstName)
			{
				eachContactData[kFirstName]	= firstName;
			}
			
			if (lastName)
			{
				eachContactData[kLastName]	= lastName;
			}
			
			// Get image
			NSData *imageData				= (NSData *)CFBridgingRelease(ABPersonCopyImageData(person));
			
			if (imageData)
			{
				// Add image path to the contact info dictionary
				eachContactData[kImagePath]	= [imageData saveImage];
			}
			
			// Get phone numbers
			ABMultiValueRef phoneNumbersRef 		= CFBridgingRelease(ABRecordCopyValue(person, kABPersonPhoneProperty));
			CFIndex phoneNumberCount        		= ABMultiValueGetCount(phoneNumbersRef);
			NSMutableArray *phoneNumberList			= [NSMutableArray array];

			for (CFIndex pIter = 0; pIter < phoneNumberCount; pIter++)
			{
				NSString *curNumber			= (NSString *)CFBridgingRelease(ABMultiValueCopyValueAtIndex(phoneNumbersRef, pIter));
				
				if (curNumber)
				{
					NSString *formattedNum 	= [[curNumber componentsSeparatedByCharactersInSet:phoneNumExcludedCharacterSet] componentsJoinedByString:@""];
					
					// Add phone no
					[phoneNumberList addObject:formattedNum];
				}
			}
			
			eachContactData[kPhoneNumList]  = phoneNumberList;
			
			// Get email address
			ABMultiValueRef emailIDsRef   	= CFBridgingRelease(ABRecordCopyValue(person, kABPersonEmailProperty));
			CFIndex emaildIdCount			= ABMultiValueGetCount(emailIDsRef);
			NSMutableArray *emailIDList		= [NSMutableArray array];
			
			for (CFIndex i = 0; i < emaildIdCount; i++)
			{
				NSString *curEmail     	 	= (NSString *)CFBridgingRelease(ABMultiValueCopyValueAtIndex(emailIDsRef, i));
				
				if (curEmail)
				{
					// Add email id
					[emailIDList addObject:curEmail];
				}
			}
			
			eachContactData[kEmailIDList]	= emailIDList;
			
			// Add contact info to the list
			[contactsJSONList addObject:eachContactData];
		}
		
		// Invoke handler
		[self onReadContactsFinished:contactsJSONList error:nil];
		
		// Release
		CFRelease(allPeople);
		CFRelease(addressBook);
	}
}

- (void)onReadContactsFinished:(NSMutableArray *)contactsJSONList error:(NSError *)error
{
	// Notify unity
	NSMutableDictionary *dataDict	= [NSMutableDictionary dictionary];
	[dataDict setObject:[NSNumber numberWithLong:[self getAuthorizationStatus]] forKey:kAuthStatus];

	if (contactsJSONList)
		[dataDict setObject:contactsJSONList forKey:kContactList];
	
	if (error)
		[dataDict setObject:[error description] forKey:kError];
	
	NotifyEventListener(kABReadContactsFinished, ToJsonCString(dataDict));
}

@end