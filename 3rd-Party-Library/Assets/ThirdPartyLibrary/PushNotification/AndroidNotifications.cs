using Unity.Notifications.Android;
using UnityEngine;

namespace Library.PushNotification
{
    public class AndroidNotifications
    {
        private AndroidNotificationChannel channel;

        private const string channelName = "N_CENTER";

        private const string channelId = "CHANNEL_1";

        // Create new notification channel
        public void CreateNotificationChannel()
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
        public void AddNotification(string titleText, string messageText, double DelayTimeAsHours, string iconId)
        {
            Debug.Log("Creating Notification");

            var notification = new AndroidNotification()
            {
                Title = titleText,

                Text = messageText,

                FireTime = System.DateTime.Now.AddHours(DelayTimeAsHours),

                SmallIcon = iconId
            };

            AndroidNotificationCenter.SendNotification(notification, channel.Id);
        }

        // Reset all notifications and it's channel
        public void ResetAll()
        {
            Debug.Log("Reset Notifications....");

            AndroidNotificationCenter.CancelAllNotifications();

            AndroidNotificationCenter.DeleteNotificationChannel(channelId);
        }
    }
}
