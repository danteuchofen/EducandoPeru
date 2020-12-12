using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using DG.Tweening;
using System.Text.RegularExpressions;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;

    public TextAsset jsonDialogue, jsonIndicator;

    private GameObject textContainer;
    private Text textDisplay;

    private JsonData dialogue, layer, line, indicator;
    [HideInInspector]
    public int index, indexOptions, indexIndicator;
    private int sceneID;
    [HideInInspector]
    public string extra;
    private string speaker, speakerOptions;

    [HideInInspector]
    public bool canPrint, canSelect;

    void Awake()
    {
        Instance = this;
        GetNumberFromDialog();
        textContainer = GameObject.FindGameObjectWithTag("Dialogue");
        textDisplay = textContainer.GetComponentInChildren<Text>();
    }

    void Start()
    {
        LoadDialogue();
        if (textDisplay.color.a > 0)
        {
            textDisplay.DOFade(0, 0);
        }
        canSelect = false;
        canPrint = false;
    }

    void Update()
    {
        if (/*Input.GetMouseButtonDown(1) &&*/ !canSelect && canPrint)
        {
            if (index < dialogue.Count)
            {
                PrintLine();
            }
            else
            {
                canPrint = false;

                if (jsonDialogue.name.Contains("Act"))
                {
                    if (PlayerPrefs.GetInt("actCompleted") < sceneID) GameManager.Instance.SaveProgress();
                    GameManager.Instance.ShowWinPanel();
                }
                else
                {
                    StartCoroutine(GameManager.Instance.Fading(true));
                }
            }
        }
    }

    private void LoadDialogue()
    {
        index = indexOptions = indexIndicator = 0;
        dialogue = JsonMapper.ToObject(jsonDialogue.text);
        if (jsonIndicator != null) indicator = JsonMapper.ToObject(jsonIndicator.text);

        layer = dialogue;
    }

    private void GetNumberFromDialog()
    {
        string[] digits = Regex.Split(jsonDialogue.name, @"\D+");
        foreach (string value in digits)
        {
            if (int.TryParse(value, out int number))
            {
                sceneID = number;
            }
        }
    }

    public void ShowIndicators(bool active = false)
    {
        GameManager.Instance.ShowIndicator(active, indicator[indexIndicator].ToString());
        if (active)
        {
            if (indexIndicator < indicator.Count - 1) indexIndicator++;
        }
        else
        {
            NextLine();
        }
    }

    public void PrintLine()
    {
        canPrint = false;

        line = layer[index];
        foreach (JsonData key in line.Keys)
            speaker = key.ToString();

        switch (speaker)
        {
            case "Show":
            case "Hide":
                GameManager.Instance.ShowPanels();
                index++;
                PrintLine();
                break;
            case "?":
                if (jsonIndicator != null) ShowIndicators(true);
                canSelect = true;
                index++;
                break;
            default:
                StartCoroutine(PrintingLine());
                break;
        }
    }

    public void PrintingOptions(bool correct)
    {
        canSelect = false;

        if (correct)
        {
            line = layer[index - 1][0][0][0][indexOptions];
            indexOptions++;
        }
        else
        {
            line = layer[index - 1][0][1][0][0];
            if (GameManager.Instance.firstWin) GameManager.Instance.firstWin = false;
        }

        foreach (JsonData key in line.Keys)
            speakerOptions = key.ToString();

        if (!speakerOptions.Equals("?"))
        {
            if (correct)
            {
                SoundManager.Instance.PlayAudio("bien", true);
            }
            else
            {
                SoundManager.Instance.PlayAudio("mal", true);
            }
        }
        ShowingText();
    }

    public IEnumerator PrintingLine()
    {
        Tween textTween = textDisplay.DOFade(0, 0.25f);
        yield return textTween.WaitForCompletion();

        SoundManager.Instance.PlayAudio("voz", true);
        ShowingText();
        index++;
    }

    private void ShowingText()
    {
        textDisplay.DOFade(1, 0.25f);
        if (speaker.Equals("Add"))
        {

            textDisplay.text = line[0][0][0].ToString() + extra;
        }
        else
        {
            textDisplay.text = line[0].ToString();
        }
    }

    public IEnumerator PrintOptions(bool correct)
    {
        Tween textTween = textDisplay.DOFade(0, 0.25f);
        yield return textTween.WaitForCompletion();
        PrintingOptions(correct);
    }

    public void NextLine()
    {
        canSelect = false;
        PrintLine();
    }
}
