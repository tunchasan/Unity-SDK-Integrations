namespace Library.PushNotification
{
    #if UNITY_ANDROID

    using Unity.Notifications.Android;
    using UnityEngine;

    public class AndroidNotifications
    {
        private static AndroidNotificationChannel channel;

        private const string channelName = "N_CENTER";

        private const string channelId = "CHANNEL_1";

        // Create new notification channel
        public static void CreateNotificationChannel()
        {
            Debug.Log("Channel Creating with " + channelId);

            channel = new AndroidNotificationChannel()
            {
                Id = channelId,

                Name = channelName,

                Importance = Importance.High,

                Description = "Generic notifications"
            };

            AndroidNotificationCenter.RegisterNotificationChannel(channel);
        }

        // Add new notification to channel and execute
        public static void AddNotification(string titleText, string messageText, double DelayTimeAsHours, string iconId)
        {
            Debug.Log("Creating Notification");

            var notification = new AndroidNotification()
            {
                Title = titleText,

                Text = messageText,

                FireTime = System.DateTime.Now.AddHours(DelayTimeAsHours),

                LargeIcon = iconId
            };

            AndroidNotificationCenter.SendNotification(notification, channel.Id);
        }

        // Reset all notifications and it's channel
        public static void ResetAll()
        {
            Debug.Log("Reset Notifications....");

            AndroidNotificationCenter.CancelAllNotifications();

            AndroidNotificationCenter.DeleteNotificationChannel(channelId);
        }
    }

    #endif

    #if UNITY_IOS

    using System;
    using Unity.Notifications.iOS;

    public class IOSNotification
    {
        // Create and Schedule Notification
        public static void ScheduleNotification(string titleText, string messageText, string subTitle, TimeSpan timeInterval, string notificationID)
        {
            var timeTrigger = new iOSNotificationTimeIntervalTrigger()
            {
                TimeInterval = timeInterval,
                Repeats = false
            };

            var notification = new iOSNotification()
            {
                // You can optionally specify a custom identifier which can later be 
                // used to cancel the notification, if you don't set one, a unique 
                // string will be generated automatically.
                Identifier = notificationID,
                Title = titleText,
                Body = messageText,
                Subtitle = subTitle,
                ShowInForeground = true,
                ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
                CategoryIdentifier = "category_a",
                ThreadIdentifier = "thread1",
                Trigger = timeTrigger,
            };

            notification.ShowInForeground = true;

            notification.ForegroundPresentationOption = (PresentationOption.Sound | PresentationOption.Alert);

            //Schedule Notification
            iOSNotificationCenter.ScheduleNotification(notification);
        }

        // Cancel the notification if it doesn’t trigger:
        public static void RemoveScheduledNotification(string notificationID)
        {
            iOSNotificationCenter.RemoveScheduledNotification(notificationID);
        }

        // Cancel all notifications that don't trigger yet
        public static void RemoveAllScheduledNotifications()
        {
            iOSNotificationCenter.RemoveAllScheduledNotifications();
        }

        // Removes the notification from the Notification Center if it was already shown to the user
        public static void RemoveDeliveredNotification(string notificationID)
        {
            iOSNotificationCenter.RemoveDeliveredNotification(notificationID);
        }

        // Removes all notifications from the Notification Center if it was already shown to the user
        public static void RemoveAllDeliveredNotifications()
        {
            iOSNotificationCenter.RemoveAllDeliveredNotifications();
        }
    }

    #endif
}

