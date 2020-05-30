using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.iOS;
using UnityEngine;

public class IOSNotification : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = new TimeSpan(0, 0, 15),
            Repeats = false
        };

        var notification = new iOSNotification()
        {
            // You can optionally specify a custom identifier which can later be 
            // used to cancel the notification, if you don't set one, a unique 
            // string will be generated automatically.
            Identifier = "_notification_01",
            Title = "Title",
            Body = "Scheduled at: " + DateTime.Now.ToShortDateString() + " triggered in 5 seconds",
            Subtitle = "This is a subtitle, something, something important...",
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = timeTrigger,
        };
notification.ShowInForeground = true;

// In this case you need to specify its 'ForegroundPresentationOption'
notification.ForegroundPresentationOption = (PresentationOption.Sound |                                                     PresentationOption.Alert);

        iOSNotificationCenter.ScheduleNotification(notification);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
