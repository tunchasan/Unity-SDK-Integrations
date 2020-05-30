
using Library.PushNotification;
using UnityEngine;

public class NotificationExample : MonoBehaviour
{
    // Notification Title
    private const string Notification_Title = "Race Tower";

    // Notification Text
    private const string Notification_Text = "Hey! We missed you";

    // Notification Icon
    private const string Notification_SmallIconID = "icon_01";

    void Start()
    {
        #if UNITY_ANROID

        AndroidNotifications.ResetAll();

        AndroidNotifications.CreateNotificationChannel();

        // Half minute notification FOR TEST
        AndroidNotifications.AddNotification(Notification_Title, Notification_Text, 0.008, Notification_SmallIconID);
        // 1 HOUR
        AndroidNotifications.AddNotification(Notification_Title, Notification_Text, 1, Notification_SmallIconID);
        // 7 HOUR
        AndroidNotifications.AddNotification(Notification_Title, Notification_Text, 7, Notification_SmallIconID);
        // 1 DAY
        AndroidNotifications.AddNotification(Notification_Title, Notification_Text, 24, Notification_SmallIconID);
  
        #endif

        #if UNITY_IOS

        // Remove all delivered notification from IOSNotificationCenter
        IOSNotification.RemoveAllDeliveredNotifications();

        // Remove all ScheduledNotifications
        IOSNotification.RemoveAllScheduledNotifications();

        IOSNotification.ScheduleNotification("Hey!", "Come and Play", "We missed you", new System.TimeSpan(0,1,0), "N_MINUTE");

        #endif
    }
}

