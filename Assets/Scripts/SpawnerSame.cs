using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpawnerSame : MonoBehaviour
{
    public static SpawnerSame Instance;

    public GameObject emptyImage;
    public Transform inputFieldPanel;
    public int maxPrefabs = 1;
    public List<Sprite> sprites = new List<Sprite>();

    private Image image;
    private string[] signs = { "<", ">", "=" };
    private Text signContainer;
    private int signIndex = -1, currentStreak = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CreatePrefabs();
    }

    void Update()
    {

    }

    public void CreatePrefabs()
    {
        foreach (Transform child in transform)
        {
            int prefabIndex = Random.Range(0, sprites.Count),
                numberPrefabs = Random.Range(1, maxPrefabs + 1);

            for (int i = 0; i < numberPrefabs; i++)
            {
                emptyImage.GetComponent<Image>().sprite = sprites[prefabIndex];
                Instantiate(emptyImage, child);
            }
        }
    }

    public void CheckPrefab()
    {
        if (DialogManager.Instance.canSelect)
        {
            int number1 = int.Parse(inputFieldPanel.GetChild(0).GetComponentInChildren<Text>().text),
                number2 = int.Parse(inputFieldPanel.GetChild(2).GetComponentInChildren<Text>().text);

            if (number1 == transform.GetChild(0).childCount && number2 == transform.GetChild(1).childCount)
            {
                switch (inputFieldPanel.GetChild(1).GetComponentInChildren<Text>().text)
                {
                    case "<":
                        if (number1 < number2) RightAnswer();
                        else WrongAnswer();
                        break;
                    case ">":
                        if (number1 > number2) RightAnswer();
                        else WrongAnswer();
                        break;
                    case "=":
                        if (number1 == number2) RightAnswer();
                        else WrongAnswer();
                        break;
                }
            }
            else
            {
                WrongAnswer();
            }
            DialogManager.Instance.NextLine();
        }
    }

    public void ChangeSign()
    {
        if (DialogManager.Instance.canSelect)
        {
            if (signIndex < 0)
            {
                signContainer = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>();

                for (int i = 0; i < signs.Length; i++)
                {
                    if (signContainer.text.Equals(signs[i]))
                    {
                        signIndex = i + 1;
                        break;
                    }
                }
            }
            if (signIndex >= signs.Length)
            {
                signIndex = 0;
            }

            signContainer.text = signs[signIndex++];
        }
    }

    private void RightAnswer()
    {
        currentStreak++;
        DeletePrefabs();

        DialogManager.Instance.index -= 1;
        SoundManager.Instance.PlayAudio("bien");
    }

    private void WrongAnswer()
    {
        ChallengeManager.Instance.UploadStreak(currentStreak);
        currentStreak = 0;
        SoundManager.Instance.PlayAudio("mal");
    }

    public void DeletePrefabs()
    {
        foreach (Transform parent in transform)
        {
            foreach (Transform child in parent)
            {
                Destroy(child.gameObject);
            }
        }
        StartCoroutine(ResetPrefabs());
    }

    private IEnumerator ResetPrefabs()
    {
        yield return new WaitForEndOfFrame();
        CreatePrefabs();
    }
}
