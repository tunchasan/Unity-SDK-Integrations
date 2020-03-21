//
//  CPNPCloudServicesBinding.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 18/01/16.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>

// Initialise
UIKIT_EXTERN void cpnpCloudServicesInitialise ();

// Setting values
UIKIT_EXTERN void cpnpCloudServicesSetBool (const char *key, bool value);
UIKIT_EXTERN void cpnpCloudServicesSetLong (const char *key, long value);
UIKIT_EXTERN void cpnpCloudServicesSetDouble (const char *key, double value);
UIKIT_EXTERN void cpnpCloudServicesSetString (const char *key, const char *value);
UIKIT_EXTERN void cpnpCloudServicesSetList (const char *key, const char *value);
UIKIT_EXTERN void cpnpCloudServicesSetDictionary (const char *key, const char *value);

// Gettings values
UIKIT_EXTERN bool cpnpCloudServicesGetBool (const char *key);
UIKIT_EXTERN long cpnpCloudServicesGetLong (const char *key);
UIKIT_EXTERN double cpnpCloudServicesGetDouble (const char *key);
UIKIT_EXTERN char* cpnpCloudServicesGetString (const char *key);
UIKIT_EXTERN char* cpnpCloudServicesGetList (const char *key);
UIKIT_EXTERN char* cpnpCloudServicesGetDictionary (const char *key);

// Synchronise
UIKIT_EXTERN bool cpnpCloudServicesSynchronise ();

// Removing values
UIKIT_EXTERN void cpnpCloudServicesRemoveKey (const char *key);
UIKIT_EXTERN void cpnpCloudServicesRemoveAllKeys ();
