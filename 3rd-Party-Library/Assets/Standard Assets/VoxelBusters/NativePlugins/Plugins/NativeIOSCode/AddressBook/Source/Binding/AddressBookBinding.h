//
//  AddressBookBinding.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 10/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>

// Status methods
UIKIT_EXTERN int getAuthorizationStatus ();
UIKIT_EXTERN void requestAccess ();

// Methods
UIKIT_EXTERN void readContacts ();