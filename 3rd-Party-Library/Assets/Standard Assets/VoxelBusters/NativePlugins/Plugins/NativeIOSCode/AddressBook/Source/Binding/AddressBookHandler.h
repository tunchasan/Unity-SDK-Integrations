//
//  AddressBookHandler.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 10/12/14.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>
#ifdef __IPHONE_9_0
	#import <Contacts/Contacts.h>
#endif
#import <AddressBook/AddressBook.h>
#import "HandlerBase.h"

enum ContactsAuthorizationStatus
{
	ContactsAuthorizationStatusNotDetermined = 0,
	ContactsAuthorizationStatusRestricted,
	ContactsAuthorizationStatusDenied,
	ContactsAuthorizationStatusAuthorized
};
typedef enum ContactsAuthorizationStatus ContactsAuthorizationStatus;

@interface AddressBookHandler : HandlerBase

// Auth methods
- (ContactsAuthorizationStatus)getAuthorizationStatus;
- (void)requestAccess;

// Methods
- (void)readContacts;

@end
