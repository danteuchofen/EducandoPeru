using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject activityPanels;
    private List<GameObject> panels = new List<GameObject>();
    private GameObject buttonPanel, secButtonPanel, textCointainer, character, indicatorPanel, fadePanel;
    [HideInInspector]
    public int panelIndex = 0, indicatorIndex = 0;
    private int loopPanelIndex = 0, loopPanelNumber = 0, previousPanelIndex = 0;
    private float tweenSpeed = 1;
    private Tween panelTween;

    public int activityIndex;

    void Awake()
    {
        Instance = this;

        textCointainer = GameObject.FindGameObjectWithTag("Dialogue");
        character = GameObject.FindGameObjectWithTag("Character");
        indicatorPanel = GameObject.FindGameObjectWithTag("Indicator");
        buttonPanel = GameObject.FindGameObjectWithTag("Button");
        secButtonPanel = GameObject.FindGameObjectWithTag("SecButton");
        fadePanel = GameObject.FindGameObjectWithTag("Fade");

        for (int i = 0; i < activityPanels.transform.childCount; i++)
        {
            panels.Add(activityPanels.transform.GetChild(i).gameObject);
        }
    }

    void Start()
    {
        for (int i = 0; i < activityPanels.transform.childCount; i++)
        {
            panels[i].SetActive(false);
        }

        textCointainer.gameObject.SetActive(false);
        character.gameObject.SetActive(false);
        if (indicatorPanel != null) indicatorPanel.gameObject.SetActive(false);
        ActiveButton(buttonPanel, true, StartDialogue, "EMPEZAR");
        secButtonPanel.gameObject.SetActive(false);
        StartCoroutine(Fading(false));
    }

    void Update()
    {

    }

    public void ShowPanels()
    {
        panelIndex++;
        switch (activityIndex)
        {
            case 1:
                switch (panelIndex)
                {
                    case 1:
                        ActiveImage(panels[0], true);
                        break;
                    case 2:
                        ActiveImage(panels[0], false);
                        break;
                    case 3:
                        ActiveImage(panels[1], true);
                        ActiveButton(secButtonPanel, true);
                        break;
                    case 4:
                        ActiveImage(panels[1], false);
                        ActiveButton(secButtonPanel, false);
                        break;
                    case 5:
                        ActiveImage(panels[2], true);
                        break;
                    case 6:
                        ActiveImage(panels[2], false);
                        break;
                    case 7:
                        ActiveImage(panels[3], true);
                        break;
                    case 8:
                        ActiveImage(panels[3], false);
                        break;
                    case 9:
                        ActiveImage(panels[4], true);
                        ActiveButton(buttonPanel, true, ShowPreviousPanel, "MOSTRAR");
                        break;
                    case 10:
                        ActiveImage(panels[4], false);
                        ActiveButton(buttonPanel, false, ShowPreviousPanel);
                        break;
                    case 11:
                        ActiveImage(panels[5], true);
                        loopPanelIndex = 5;
                        loopPanelNumber = 2;
                        ActiveButton(buttonPanel, true, ContinueDialogue, "COCINAR");
                        break;
                    case 12:
                        ActiveButton(buttonPanel, true, CheckDragAndDrop, "LISTO");
                        break;
                    case 13:
                        ActiveImage(panels[7], false);
                        ActiveButton(buttonPanel, false, CheckDragAndDrop);
                        break;
                }
                break;
            case 2:
                switch (panelIndex)
                {
                    case 1:
                        ActiveImage(panels[0], true);
                        break;
                    case 2:
                        ActiveImage(panels[1], true);
                        ActiveButton(buttonPanel, true, Spawner.Instance.CheckPrefab, "COMPROBAR");
                        break;
                    case 3:
                        ActiveImage(panels[1], false);
                        ActiveButton(buttonPanel, false, Spawner.Instance.CheckPrefab);
                        break;
                    case 4:
                        ActiveImage(panels[0], false);
                        break;
                    case 5:
                        ActiveImage(panels[2], true);
                        break;
                }
                break;
            case 3:
                switch (panelIndex)
                {
                    case 1:
                        ActiveImage(panels[0], true);
                        break;
                    case 2:
                        ActiveImage(panels[1], true);
                        break;
                    case 3:
                        ActiveImage(panels[0], false);
                        ActiveImage(panels[1], false);
                        break;
                    case 4:
                        ActiveImage(panels[2], true);
                        break;
                    case 5:
                        ActiveImage(panels[2], false);
                        break;
                    case 6:
                        ActiveImage(panels[3], true);
                        ActiveButton(secButtonPanel, true, PrintNextLine, "CONTINUAR");
                        break;
                    case 7:
                        ActiveImage(panels[3], false);
                        ActiveButton(secButtonPanel, false);
                        break;
                }
                break;
            case 4:
                switch (panelIndex)
                {
                    case 1:
                        ActiveImage(panels[0], true);
                        break;
                    case 2:
                        ActiveImage(panels[1], true);
                        ActiveButton(buttonPanel, true, SpawnerSame.Instance.CheckPrefab, "COMPROBAR");
                        break;
                    case 3:
                        ActiveImage(panels[1], false);
                        ActiveButton(buttonPanel, false, SpawnerSame.Instance.CheckPrefab);
                        break;
                    case 4:
                        ActiveImage(panels[0], false);
                        break;
                    case 5:
                        ActiveImage(panels[2], true);
                        break;
                }
                break;
        }
    }

    public void ShowIndicator(bool active, string text)
    {
        if (active) indicatorPanel.GetComponentInChildren<Text>().text = text;
        ScaleImage(indicatorPanel, active);
    }

    private void ActiveImage(GameObject _gameObject, bool active)
    {
        Image image = _gameObject.transform.GetChild(0).GetComponent<Image>();
        Vector2 imagePos;

        if (active)
        {
            _gameObject.gameObject.SetActive(true);
            imagePos = _gameObject.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition;
            image.rectTransform.DOAnchorPos(imagePos, tweenSpeed);
        }
        else
        {
            imagePos = _gameObject.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition;
            panelTween = image.rectTransform.DOAnchorPos(imagePos, tweenSpeed);
            StartCoroutine(WaitForDotween(_gameObject));
        }
    }

    private void ScaleImage(GameObject _gameObject, bool active)
    {
        Image image = _gameObject.transform.GetChild(0).GetComponent<Image>();
        Vector2 imageSize;

        if (active)
        {
            imageSize = _gameObject.transform.GetChild(2).GetComponent<RectTransform>().localScale;
            _gameObject.gameObject.SetActive(true);
            if (imageSize != Vector2.zero) image.rectTransform.DOScale(Vector2.zero, 0);
            image.rectTransform.DOScale(imageSize, tweenSpeed);
        }
        else
        {
            imageSize = _gameObject.transform.GetChild(1).GetComponent<RectTransform>().localScale;
            panelTween = image.rectTransform.DOScale(imageSize, tweenSpeed);
            StartCoroutine(WaitForDotween(_gameObject));
        }
    }

    private void ActiveButton(GameObject _gameObject, bool active, UnityAction listener = null, string _text = "")
    {
        Transform _button = _gameObject.transform.GetChild(0);
        Image image = _button.GetComponent<Image>();
        Vector2 imagePos;

        if (active)
        {
            image.rectTransform.anchoredPosition = _gameObject.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition;

            if (!_text.Equals("")) _button.GetComponentInChildren<Text>().text = _text;
            if (listener != null) _button.GetComponent<Button>().onClick.AddListener(listener);

            _gameObject.gameObject.SetActive(true);
            imagePos = _gameObject.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition;
            image.rectTransform.DOAnchorPos(imagePos, tweenSpeed);
        }
        else
        {
            imagePos = _gameObject.transform.GetChild(3).GetComponent<RectTransform>().anchoredPosition;
            panelTween = image.rectTransform.DOJumpAnchorPos(imagePos, 50, 5, tweenSpeed);
            StartCoroutine(WaitForDotween(_gameObject));

            if (listener != null) _button.GetComponent<Button>().onClick.RemoveListener(listener);
        }
    }

    public IEnumerator Fading(bool active)
    {

        Tween _tween;
        Image fadeImage = fadePanel.GetComponent<Image>();
        if (active)
        {
            fadePanel.SetActive(true);
            _tween = fadeImage.DOFade(1, tweenSpeed);
            yield return _tween.WaitForCompletion();
            SceneManager.LoadScene("Menu");
        }
        else
        {
            if (fadeImage.color.a < 1) fadeImage.DOFade(1, 0);

            _tween = fadeImage.DOFade(0, tweenSpeed);
            yield return _tween.WaitForCompletion();
            fadePanel.SetActive(false);
        }
    }

    public void CompleteAct()
    {
        int actCompleted = PlayerPrefs.GetInt("actCompleted");
        actCompleted++;
        PlayerPrefs.SetInt("actCompleted", actCompleted);
        PlayerPrefs.Save();
    }

    private IEnumerator WaitForDotween(GameObject _gameObject)
    {
        yield return panelTween.WaitForCompletion();
        _gameObject.gameObject.SetActive(false);
    }

    public void MixChildOrder(GameObject parent)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            parent.transform.GetChild(i).SetSiblingIndex(Random.Range(0, parent.transform.childCount));
        }
    }

    private void ShowPreviousPanel()
    {
        if (previousPanelIndex == 0)
        {
            for (int i = 0; i < panels.Count; i++)
            {
                if (panels[i].activeInHierarchy)
                {
                    previousPanelIndex = i - 1;
                    break;
                }
            }
        }

        if (!panelTween.IsActive())
        {
            if (DialogManager.Instance.canSelect)
            {
                DialogManager.Instance.canSelect = false;
                panels[previousPanelIndex].GetComponent<RectTransform>().SetAsLastSibling();
                ActiveImage(panels[previousPanelIndex], true);
            }
            else
            {
                DialogManager.Instance.canSelect = true;
                ActiveImage(panels[previousPanelIndex], false);
                panelTween.OnComplete(() =>
                {
                    panels[previousPanelIndex].GetComponent<RectTransform>().SetSiblingIndex(previousPanelIndex);
                });
            }
        }
    }

    public void StartDialogue()
    {
        ActiveButton(buttonPanel, false, StartDialogue, "EMPEZAR");
        ActiveImage(textCointainer, true);
        ActiveImage(character, true);

        DialogManager.Instance.NextLine();
    }

    public void ContinueDialogue()
    {
        if (DialogManager.Instance.canSelect)
        {
            Image fadeImage = fadePanel.GetComponent<Image>();

            if (fadeImage.color.a > 0)
            {
                fadeImage.DOFade(0, 0);
            }
            fadePanel.SetActive(true);
            fadeImage.DOFade(1, tweenSpeed);
            StartCoroutine(ContinuingDialogue());
        }
    }

    private IEnumerator ContinuingDialogue()
    {
        if (loopPanelNumber < loopPanelIndex) loopPanelNumber += loopPanelIndex;

        yield return new WaitForSeconds(2);
        panels[loopPanelIndex++].SetActive(false);
        panels[loopPanelIndex].SetActive(true);

        Tween _tween = fadePanel.GetComponent<Image>().DOFade(0, tweenSpeed);
        yield return _tween.WaitForCompletion();
        fadePanel.SetActive(false);

        if (loopPanelIndex < loopPanelNumber)
        {
            buttonPanel.transform.GetChild(0).GetComponentInChildren<Text>().text = "CORTAR";
        }
        else
        {
            ShowIndicator(false, "");
            ActiveButton(buttonPanel, false, ContinueDialogue, "");
        }

        DialogManager.Instance.NextLine();
    }

    private void CheckDragAndDrop()
    {
        if (DialogManager.Instance.canSelect)
        {
            if (DragDrop.Instance.CheckRecipient())
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

    public void PrintNextLine()
    {
        if (indicatorPanel != null) DialogManager.Instance.ShowIndicators();
        if (DialogManager.Instance.canSelect) DialogManager.Instance.NextLine();
    }

    public void Repeat()
    {
        if (DialogManager.Instance.canSelect)
        {
            panelIndex = 0;
            ActiveImage(panels[2], false);

            DialogManager.Instance.index = 4;
            DialogManager.Instance.NextLine();
            if (Spawner.Instance != null) Spawner.Instance.DeletePrefabs();
            if (SpawnerSame.Instance != null) SpawnerSame.Instance.DeletePrefabs();
        }
    }
}
