using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_IOS
using Unity.Notifications.iOS;
#endif
using UnityEngine;

public class iOSNotificationHandler : MonoBehaviour
{
#if UNITY_IOS

    //only compiles if iOS is used
    private const string ChannelId = "notification_channel";
    public void ScheduleNotification(int minutes)
    {
        //all notification needs a channel
        iOSNotification notification = new iOSNotification
        {
            Title = "Energy Recharged",
            Subtitle = "Your energy has been recharged",
            Body = "Your energy has been recharged, come back to play again!",
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = new iOSNotificationTimeIntervalTrigger
            {
                TimeInterval = new System.TimeSpan(0, minutes, 0),
                Repeats = false
            }
        };

        iOSNotificationCenter.ScheduleNotification(notification);


    }
#endif

}
