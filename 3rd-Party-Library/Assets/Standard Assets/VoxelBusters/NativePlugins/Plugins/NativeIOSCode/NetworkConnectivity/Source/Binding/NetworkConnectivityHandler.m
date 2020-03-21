//
//  NetworkConnectivityHandler.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 06/12/14.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NetworkConnectivityHandler.h"

@interface NetworkConnectivityHandler ()

@property (nonatomic, retain)   Reachability	*hostReachability;
@property (nonatomic, assign)   bool			isHostReachable;

@end

@implementation NetworkConnectivityHandler

#define kNetworkChanged		"ConnectivityChanged"

@synthesize hostReachability;

@synthesize isConnected;
@synthesize isHostReachable;

#pragma mark - init and dealloc

- (id)init
{
	self = [super init];
    
    if (self)
    {
		// Default flag values
		self.isConnected			= NO;
        self.isHostReachable    	= NO;
		
		// Register for notification
		[[NSNotificationCenter defaultCenter] addObserver:self
												 selector:@selector(reachabilityChanged:)
													 name:kReachabilityChangedNotification
												   object:nil];
    }
    
    return self;
}


- (void)dealloc
{
    // Remove as observer
    [[NSNotificationCenter defaultCenter] removeObserver:self
                                                    name:kReachabilityChangedNotification
                                                  object:nil];
	
    // Release
	[self.hostReachability stopNotifier];
	self.hostReachability		= NULL;
	
    [super dealloc];
}

#pragma mark - Methods

- (void)setHostAddress:(NSString *)hostAddress
{
	// Old reachablity handler will be released
	if (self.hostReachability != NULL)
	{
		[self.hostReachability stopNotifier];
		self.hostReachability	= NULL;
	}
	
	struct sockaddr_in6 addr;
	memset(&addr, 0, sizeof(struct sockaddr_in6));
	addr.sin6_len = sizeof(struct sockaddr_in6);
	addr.sin6_family = AF_INET6;
	inet_pton(AF_INET6, [hostAddress UTF8String], &addr.sin6_addr);
	
	self.hostReachability   	= [Reachability reachabilityWithAddress:(const struct sockaddr *)&addr];
	[self.hostReachability startNotifier];
	[self updateReachablityStatus:self.hostReachability];
}


#pragma mark - Notification

- (void)reachabilityChanged:(NSNotification *)note
{
	Reachability* curReach = [note object];
    
    if ([curReach isKindOfClass:[Reachability class]])
    {
        [self updateReachablityStatus:curReach];
    }
}

- (void)updateReachablityStatus:(Reachability *)reachability
{
	if (reachability == self.hostReachability)
	{
		BOOL connectionRequired		= [reachability connectionRequired];
		BOOL reachable				= [self checkIfReachable:reachability];
		self.isHostReachable		= reachable && !connectionRequired;
	}
	
	// Check new status
    BOOL newConnectivityStatus		= self.isHostReachable;
	
	// Send message to Unity
    if (self.isConnected)
    {
        if (!newConnectivityStatus)
        {
            NSLog(@"[NetworkConnectivityHandler] is not reachable");
            NotifyEventListener(kNetworkChanged, "false");
        }
    }
    else
    {
        if (newConnectivityStatus)
        {
            NSLog(@"[NetworkConnectivityHandler] is reachable");
            NotifyEventListener(kNetworkChanged, "true");
        }
    }
	
	// Cache the new connectivity status
	self.isConnected		= newConnectivityStatus;
}

- (bool)checkIfReachable:(Reachability *)reachability
{
    NetworkStatus status = [reachability currentReachabilityStatus];
    
	return (status != NotReachable);
}

@end