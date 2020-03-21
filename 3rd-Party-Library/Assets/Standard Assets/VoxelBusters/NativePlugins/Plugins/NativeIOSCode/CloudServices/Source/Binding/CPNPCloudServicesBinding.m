//
//  CPNPCloudServicesBinding.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 18/01/16.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "CPNPCloudServicesBinding.h"
#import "CPNPCloudServicesHandler.h"

#pragma mark - Initialise

void cpnpCloudServicesInitialise ()
{
	[CPNPCloudServicesHandler Instance];
}

#pragma mark - Setting values

void cpnpCloudServicesSetBool (const char *key, bool value)
{
	[[NSUbiquitousKeyValueStore defaultStore] setBool:value
											   forKey:ConvertToNSString(key)];
}

void cpnpCloudServicesSetLong (const char *key, long value)
{
	[[NSUbiquitousKeyValueStore defaultStore] setLongLong:value
												   forKey:ConvertToNSString(key)];
}

void cpnpCloudServicesSetDouble (const char *key, double value)
{
	[[NSUbiquitousKeyValueStore defaultStore] setDouble:value
												 forKey:ConvertToNSString(key)];
}

void cpnpCloudServicesSetString (const char *key, const char *value)
{
	[[NSUbiquitousKeyValueStore defaultStore] setString:ConvertToNSString(value)
												 forKey:ConvertToNSString(key)];
}

void cpnpCloudServicesSetList (const char *key, const char *value)
{
	[[NSUbiquitousKeyValueStore defaultStore] setArray:FromJson(value)
												forKey:ConvertToNSString(key)];
}

void cpnpCloudServicesSetDictionary (const char *key, const char *value)
{
	[[NSUbiquitousKeyValueStore defaultStore] setDictionary:FromJson(value)
													 forKey:ConvertToNSString(key)];
}

#pragma mark -  Gettings values

bool cpnpCloudServicesGetBool (const char *key)
{
	return [[NSUbiquitousKeyValueStore defaultStore] boolForKey:ConvertToNSString(key)];
}

long cpnpCloudServicesGetLong (const char *key)
{
	return [[NSUbiquitousKeyValueStore defaultStore] longLongForKey:ConvertToNSString(key)];
}

double cpnpCloudServicesGetDouble (const char *key)
{
	return [[NSUbiquitousKeyValueStore defaultStore] doubleForKey:ConvertToNSString(key)];
}

char* cpnpCloudServicesGetString (const char *key)
{
	NSString *value	= [[NSUbiquitousKeyValueStore defaultStore] stringForKey:ConvertToNSString(key)];
	
	return value ? CStringCopy([value UTF8String]) : NULL;
}

char* cpnpCloudServicesGetList (const char *key)
{
	NSArray	*value	= [[NSUbiquitousKeyValueStore defaultStore] arrayForKey:ConvertToNSString(key)];
	
	return value ? CStringCopy(ToJsonCString(value)) : NULL;
}

char* cpnpCloudServicesGetDictionary (const char *key)
{
	NSDictionary *value	= [[NSUbiquitousKeyValueStore defaultStore] dictionaryForKey:ConvertToNSString(key)];
	
	return value ? CStringCopy(ToJsonCString(value)) : NULL;
}

#pragma mark - Synchronise

bool cpnpCloudServicesSynchronise ()
{
	return [[NSUbiquitousKeyValueStore defaultStore] synchronize];
}

#pragma mark - Removing values

void cpnpCloudServicesRemoveKey (const char *key)
{
	[[NSUbiquitousKeyValueStore defaultStore] removeObjectForKey:ConvertToNSString(key)];
}

void cpnpCloudServicesRemoveAllKeys ()
{
    NSDictionary * dict = [[NSUbiquitousKeyValueStore defaultStore] dictionaryRepresentation];
    for (id key in dict)
    {
        cpnpCloudServicesRemoveKey([key UTF8String]);
    }
}
