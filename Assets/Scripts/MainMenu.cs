using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text energyText;

    [SerializeField] private Button playButton;

    [SerializeField] private AndroidNotificationHandler androidNotificationHandler;
    [SerializeField] private iOSNotificationHandler iOSNotificationHandler;

    [SerializeField] private int maxEnergy;
    [SerializeField] private int energyRechargeDuration;

    private int energy;

    private const string EnergyKey = "Energy";
    private const string EnergyReadyKey = "EnergyReady";

    private void Start()
    {
        OnApplicationFocus(true);
    }

    //OnApplicationFocus  affects minimize and maximize. Start only affects close/open app. Put these two together to failsafe.
    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus) return;

        CancelInvoke();//Cancel all invokes. Specify to Cancel specific invokes.

        //calls value from Score System
        int highScore = PlayerPrefs.GetInt(ScoreSystem.HighScoreKey, 0);

        highScoreText.text = $"High Score: {highScore} "; //can also do "High Score" + highScore.ToString();

        energy = PlayerPrefs.GetInt(EnergyKey, maxEnergy);

        if(energy == 0)
        {
            string energyReadyString = PlayerPrefs.GetString(EnergyReadyKey, string.Empty);

            if(energyReadyString == string.Empty ) 
            {
                return;
            }
            //saves energy in strings because DateTime doesnt do integers? Doesnt have a method for it.
            DateTime energyReady = DateTime.Parse(energyReadyString);

            //energyReady is ready if DateTime.Now is higher/greater than it.
            if(DateTime.Now > energyReady)
            {
                energy = maxEnergy;
                PlayerPrefs.SetInt(EnergyKey, energy);
            }
            else
            {
                //if energy is not ready, do these things.
                playButton.interactable = false;
                Invoke(nameof(EnergyRecharged), (energyReady - DateTime.Now).Seconds);
            }
        }

        energyText.text = $"Play ({energy})";
    }

    private void EnergyRecharged()
    {
        playButton.interactable = true;

        energy = maxEnergy;
        PlayerPrefs.SetInt(EnergyKey, energy);

        energyText.text = $"Play ({energy})";

    }
    public void Play()
    {
        if (energy < 1) return;
        
        energy--;

        PlayerPrefs.SetInt(EnergyKey, energy);

        if(energy == 0)
        {
            //UI Display
            DateTime energyReady = DateTime.Now.AddMinutes(energyRechargeDuration);
            PlayerPrefs.SetString(EnergyReadyKey,energyReady.ToString());

            //only compiles if Android is used
#if UNITY_ANDROID
            androidNotificationHandler.ScheduleNotification(energyReady);
#elif UNITY_IOS
            iosNotificationHandler.ScheduleNotificaton(energyRechargeDuration);
#endif
        }


        SceneManager.LoadScene(1);
    }
}
