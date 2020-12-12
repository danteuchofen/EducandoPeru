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
        if (Input.GetButtonDown("Jump"))
        {
            RestartActCompleted();
        }
    }

    public void UploadStreak(int _currentStreak)
    {
        currentStreak = _currentStreak;
        if (currentStreak > highStreak)
        {
            PlayerPrefs.SetInt(streakName, highStreak);
            highStreak = _currentStreak;
        }
        streakDisplay.text = "Racha actual: " + currentStreak + "\n" + "Racha más alta: " + highStreak;
        PlayerPrefs.Save();
    }

    void RestartActCompleted()
    {
        PlayerPrefs.SetInt(streakName, 0);
        PlayerPrefs.Save();
    }
}
