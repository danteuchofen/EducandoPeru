using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChallengeManager : MonoBehaviour
{
    public static ChallengeManager Instance;
    public Text streakDisplay;

    [HideInInspector]
    public int highStreak, currentStreak;
    private string streakName;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        streakName = SceneManager.GetActiveScene().name + "Streak";
        highStreak = PlayerPrefs.GetInt(streakName);
        streakDisplay.text = "Racha actual: " + currentStreak + "\n" + "Racha más alta: " + highStreak;
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetButtonDown("Jump"))
        {
            RestartActCompleted();
        }
#endif
    }

    public void UploadStreak(int _currentStreak)
    {
        currentStreak = _currentStreak;
        if (currentStreak > highStreak)
        {
            print("Ok");
            PlayerPrefs.SetInt(streakName, _currentStreak);
            PlayerPrefs.Save();
            highStreak = _currentStreak;
        }
        streakDisplay.text = "Racha actual: " + currentStreak + "\n" + "Racha más alta: " + highStreak;
    }

    void RestartActCompleted()
    {
        PlayerPrefs.SetInt(streakName, 0);
        PlayerPrefs.Save();
    }
}
