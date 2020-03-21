/*
 * Copyright (C) 2012-2015 Soomla Inc. - All Rights Reserved
 *
 *   Unauthorized copying of this file, via any medium is strictly prohibited
 *   Proprietary and confidential
 *
 *   Written by Refael Dakar <refael@soom.la>
 */

#import <Foundation/Foundation.h>

//highway events
#define HIGHWAY_EVENT_GROW_INSIGHTS_INITIALIZED     @"insights_init"
#define HIGHWAY_EVENT_INSIGHTS_REFRESH_STARTED      @"insights_retrieve_started"
#define HIGHWAY_EVENT_INSIGHTS_REFRESH_FINISHED     @"insights_retrieve_finished"
#define HIGHWAY_EVENT_INSIGHTS_REFRESH_FAILED       @"insights_retrieve_failed"

//component events
#define EVENT_GROW_INSIGHTS_INITIALIZED             @"grow_insights_init_inside"
#define EVENT_INSIGHTS_REFRESH_STARTED              @"insights_retrieve_started_inside"
#define EVENT_INSIGHTS_REFRESH_FINISHED             @"insights_retrieve_finished_inside"
#define EVENT_INSIGHTS_REFRESH_FAILED               @"insights_retrieve_failed_inside"

@interface InsightsEventHandling : NSObject

+ (void)observeAllEventsWithObserver:(id)observer withSelector:(SEL)selector;

+(void)postInsightsInitializedEvent;

+(void)postInsightsRefreshStartedEvent;

+(void)postInsightsRefreshFailedEvent;

+(void)postInsightsRefreshFinishedEvent;

@end
