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

    private AndroidNotifications _notifications;

    void Start()
    {
        _notifications = new AndroidNotifications();

        _notifications.ResetAll();

        _notifications.CreateNotificationChannel();

        // Half minute notification FOR TEST
        _notifications.AddNotification(Notification_Title, Notification_Text, 0.008, Notification_SmallIconID);
        // 1 HOUR
        _notifications.AddNotification(Notification_Title, Notification_Text, 1, Notification_SmallIconID);
        // 7 HOUR
        _notifications.AddNotification(Notification_Title, Notification_Text, 7, Notification_SmallIconID);
        // 1 DAY
        _notifications.AddNotification(Notification_Title, Notification_Text, 24, Notification_SmallIconID);
    }
}
