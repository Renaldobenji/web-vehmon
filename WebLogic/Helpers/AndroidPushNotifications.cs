using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PushSharp;
using PushSharp.Android;
using PushSharp.Core;
using WebLogic.Contracts;

namespace WebLogic.Helpers
{
    public class AndroidPushNotifications
    {
        private PushBroker push;
        public AndroidPushNotifications()
        {
            //wire up events
            //create the puchbroker object
            push = new PushBroker();
            //Wire up the events for all the services that the broker registers
            push.OnNotificationSent += NotificationSent;
            push.OnChannelException += ChannelException;
            push.OnServiceException += ServiceException;
            push.OnNotificationFailed += NotificationFailed;
            push.OnDeviceSubscriptionExpired += DeviceSubscriptionExpired;
            push.OnDeviceSubscriptionChanged += DeviceSubscriptionChanged;
            push.OnChannelCreated += ChannelCreated;
            push.OnChannelDestroyed += ChannelDestroyed;
        }

        ~AndroidPushNotifications()
        {
            push.OnNotificationSent -= NotificationSent;
            push.OnChannelException -= ChannelException;
            push.OnServiceException -= ServiceException;
            push.OnNotificationFailed -= NotificationFailed;
            push.OnDeviceSubscriptionExpired -= DeviceSubscriptionExpired;
            push.OnDeviceSubscriptionChanged -= DeviceSubscriptionChanged;
            push.OnChannelCreated -= ChannelCreated;
            push.OnChannelDestroyed -= ChannelDestroyed;
        }

        public void PushNotification(String deviceID, VehmonNotification JsonPayload)
        {
            String jsonData = JsonConvert.SerializeObject(JsonPayload);

            push.RegisterGcmService(new GcmPushChannelSettings("AIzaSyAj9222KhG5qX667aK7DhLA2bo18-bvPmQ"));
            push.QueueNotification(new GcmNotification().ForDeviceRegistrationId(deviceID).WithJson(jsonData));
        }

        //Currently it will raise only for android devices
        static void DeviceSubscriptionChanged(object sender,
        string oldSubscriptionId, string newSubscriptionId, INotification notification)
        {
            //Do something here
            String renad = "";
        }

        //this even raised when a notification is successfully sent
        static void NotificationSent(object sender, INotification notification)
        {
            //Do something here
            String renad = "";
        }

        //this is raised when a notification is failed due to some reason
        static void NotificationFailed(object sender,
        INotification notification, Exception notificationFailureException)
        {
            //Do something here
            String renad = "";
        }

        //this is fired when there is exception is raised by the channel
        static void ChannelException
            (object sender, IPushChannel channel, Exception exception)
        {
            //Do something here
            String renad = "";
        }

        //this is fired when there is exception is raised by the service
        static void ServiceException(object sender, Exception exception)
        {
            //Do something here
            String renad = "";
        }

        //this is raised when the particular device subscription is expired
        static void DeviceSubscriptionExpired(object sender,
        string expiredDeviceSubscriptionId,
            DateTime timestamp, INotification notification)
        {
            //Do something here
            String renad = "";
        }

        //this is raised when the channel is destroyed
        static void ChannelDestroyed(object sender)
        {
            //Do something here
            String renad = "";
        }

        //this is raised when the channel is created
        static void ChannelCreated(object sender, IPushChannel pushChannel)
        {
            //Do something here
            String renad = "";
        }
    }
}
