//
//  AddressBookBinding.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 10/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "AddressBookBinding.h"
#import "AddressBookHandler.h"

#pragma mark - Status Methods

int getAuthorizationStatus ()
{
	return (int)[[AddressBookHandler Instance] getAuthorizationStatus];
}

void requestAccess ()
{
	[[AddressBookHandler Instance] requestAccess];
}

#pragma mark - Methods

void readContacts ()
{
    [[AddressBookHandler Instance] readContacts];
}