using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Food : MonoBehaviour
{
    public static Food Instance;

    [HideInInspector]
    public int counter;
    private int intChanger, strength = 15;
    private string food;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        counter = 0;
        intChanger = 0;
    }

    void Update()
    {
        int index = DialogManager.Instance.index;
        if (intChanger != index)
        {
            intChanger = index;

            switch (index)
            {
                case 7:
                    food = "Apple";
                    break;
                case 10:
                    food = "Lemon";
                    break;
                case 12:
                    food = "Butter";
                    break;
                case 14:
                    food = "Cinnamon";
                    break;
                case 16:
                    food = "Water";
                    break;
                case 18:
                    food = "Sugar";
                    break;
                default:
                    food = null;
                    break;
            }

            if (food != null)
            {
                CountFood(food);
            }
        }
    }

    public void Answer(string value)
    {
        if (DialogManager.Instance.canSelect)
        {
            if (value.Equals(food))
            {
                counter--;
                EventSystem.current.currentSelectedGameObject.SetActive(false);

                if (counter == 0)
                {
                    DialogManager.Instance.indexOptions = 0;
                    SoundManager.Instance.PlayAudio("bien");
                    DialogManager.Instance.ShowIndicators();
                }
                else
                {
                    StartCoroutine(DialogManager.Instance.PrintOptions(true));
                }
            }
            else
            {
                EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>().DOShakeAnchorPos(1, strength, strength);
                StartCoroutine(DialogManager.Instance.PrintOptions(false));
            }
        }
    }

    private void CountFood(string food)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name.Contains(food)) counter++;
        }
    }
}
