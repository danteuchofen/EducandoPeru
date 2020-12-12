using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Answers : MonoBehaviour
{
    private int strength = 15;
    private string answer;
    public int mixChildOrderLevel = 0;

    void Start()
    {
        switch (mixChildOrderLevel)
        {
            case 1:
                GameManager.Instance.MixChildOrder(gameObject);
                break;
            case 2:
                foreach (Transform child in gameObject.transform)
                {
                    GameManager.Instance.MixChildOrder(child.gameObject);
                }
                GameManager.Instance.MixChildOrder(gameObject);
                break;
        }
    }

    public void AnswerString(string correct)
    {
        if (DialogManager.Instance.canSelect)
        {
            answer = GameObject.FindGameObjectWithTag("InputField").GetComponentInChildren<Text>().text;
            if (answer.Equals(correct))
            {
                SoundManager.Instance.PlayAudio("bien");
                DialogManager.Instance.ShowIndicators();
            }
            else
            {
                StartCoroutine(DialogManager.Instance.PrintOptions(false));
            }
        }
    }

    public void AnswerBool(bool correct)
    {
        if (DialogManager.Instance.canSelect)
        {
            if (correct)
            {
                SoundManager.Instance.PlayAudio("bien");
                DialogManager.Instance.ShowIndicators();
            }
            else
            {
                EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>().DOShakeAnchorPos(1, strength, strength);
                StartCoroutine(DialogManager.Instance.PrintOptions(false));
            }
        }
    }

    private void FindChildWithTag(GameObject parent, string _tag)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Transform child = parent.transform.GetChild(i);
            if (child.tag == _tag)
            {
                answer = child.gameObject.GetComponentInChildren<Text>().text;
            }
        }
    }
}
