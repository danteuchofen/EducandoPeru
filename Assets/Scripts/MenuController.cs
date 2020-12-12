using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MenuController : MonoBehaviour
{
    public GameObject buttonsPanel;

    private int activitiesCompleted;
    private float tweenSpeed = 1;
    private GameObject fadePanel;

    void Awake()
    {
        fadePanel = GameObject.FindGameObjectWithTag("Fade");
    }

    void Start()
    {
        StartCoroutine(Fading(false));

        activitiesCompleted = PlayerPrefs.GetInt("actCompleted");
        for (int i = buttonsPanel.transform.childCount - 1; i > activitiesCompleted * 2; i--)
        {
            buttonsPanel.transform.GetChild(i).GetComponent<Button>().interactable = false;
            buttonsPanel.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().DOFade(0.5f, 0);
            buttonsPanel.transform.GetChild(i).transform.GetChild(1).GetComponentInChildren<Text>().text = "¿?";
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            RestartActCompleted();
        }
        print(activitiesCompleted);
    }

    void RestartActCompleted()
    {
        PlayerPrefs.SetInt("actCompleted", 0);
        PlayerPrefs.Save();
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(Fading(true, sceneName));
    }

    public IEnumerator Fading(bool active, string sceneName = "")
    {

        Tween _tween;
        Image fadeImage = fadePanel.GetComponent<Image>();
        if (active)
        {
            fadePanel.SetActive(true);
            _tween = fadeImage.DOFade(1, tweenSpeed);
            yield return _tween.WaitForCompletion();

            SceneManager.LoadScene(sceneName);
        }
        else
        {
            if (fadeImage.color.a < 1) fadeImage.DOFade(1, 0);

            _tween = fadeImage.DOFade(0, tweenSpeed);
            yield return _tween.WaitForCompletion();
            fadePanel.SetActive(false);
        }
    }
}
