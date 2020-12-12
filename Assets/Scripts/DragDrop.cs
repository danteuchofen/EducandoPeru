using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    public static DragDrop Instance;

    public int count = 0;
    public bool mixChildOrder = false;
    public int dropActivity = 0;
    public Sprite verticalBook;

    private Canvas canvas;
    private GameObject containerPanel, gameObjectsPanel;
    private List<GameObject> _gameObjects = new List<GameObject>();

    private RectTransform selectedGameObject;
    private Vector2 startPos;
    private int ascendingCounter = 1;

    void Awake()
    {
        Instance = this;

        canvas = FindObjectOfType<Canvas>();
        containerPanel = transform.GetChild(0).gameObject;
        gameObjectsPanel = transform.GetChild(1).gameObject;
    }

    void Start()
    {
        for (int i = 0; i < gameObjectsPanel.transform.childCount; i++)
        {
            _gameObjects.Add(gameObjectsPanel.transform.GetChild(i).gameObject);
        }

        if (mixChildOrder)
        {
            GameManager.Instance.MixChildOrder(gameObjectsPanel);
        }
    }

    void Update()
    {
        if (mixChildOrder && gameObjectsPanel.transform.childCount == 0)
        {
            DialogManager.Instance.ShowIndicators();
            mixChildOrder = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_gameObjects.Contains(eventData.pointerPress) && DialogManager.Instance.canSelect)
        {
            selectedGameObject = EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>();
            startPos = selectedGameObject.anchoredPosition;
            selectedGameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        else
        {
            selectedGameObject = null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (selectedGameObject != null)
        {
            selectedGameObject.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (selectedGameObject != null)
        {
            selectedGameObject.anchoredPosition = startPos;
            selectedGameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
            selectedGameObject = null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (selectedGameObject != null)
        {
            switch (dropActivity)
            {
                case 0:
                    if (!_gameObjects.Contains(eventData.pointerCurrentRaycast.gameObject))
                    {
                        selectedGameObject.transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                    }
                    else
                    {
                        selectedGameObject.transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent);
                    }
                    break;
                case 1:
                    if (!_gameObjects.Contains(eventData.pointerCurrentRaycast.gameObject))
                    {
                        if (selectedGameObject.GetComponent<Image>().color.Equals(eventData.pointerCurrentRaycast.gameObject.GetComponent<Image>().color))
                        {
                            selectedGameObject.transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                            selectedGameObject.GetComponent<Image>().raycastTarget = false;
                            SoundManager.Instance.PlayAudio("bien");
                        }
                        else
                        {
                            StartCoroutine(DialogManager.Instance.PrintOptions(false));
                            selectedGameObject.anchoredPosition = startPos;
                        }
                    }
                    else
                    {
                        selectedGameObject.transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent);
                    }
                    break;
                case 2:
                    if (!_gameObjects.Contains(eventData.pointerCurrentRaycast.gameObject))
                    {
                        if (selectedGameObject.name.Contains(ascendingCounter.ToString()))
                        {
                            selectedGameObject.GetComponent<Image>().sprite = verticalBook;
                            selectedGameObject.transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                            selectedGameObject.GetComponent<Image>().raycastTarget = false;
                            SoundManager.Instance.PlayAudio("bien");
                            ascendingCounter++;
                        }
                        else
                        {
                            StartCoroutine(DialogManager.Instance.PrintOptions(false));
                            selectedGameObject.anchoredPosition = startPos;
                        }
                    }
                    else
                    {
                        selectedGameObject.transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent);
                    }
                    break;
            }
        }
    }

    public bool CheckRecipient()
    {
        bool resultFlag = true;

        for (int i = 0; i < containerPanel.transform.childCount; i++)
        {
            if (!containerPanel.transform.GetChild(i).childCount.Equals(count))
            {
                resultFlag = false;
                break;
            }
        }
        return resultFlag;
    }
}
