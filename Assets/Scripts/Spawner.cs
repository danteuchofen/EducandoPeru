using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance;

    public int minNumberPrefabs = 5;
    public List<GameObject> gameObjects = new List<GameObject>();

    private int counter, answer, currentStreak = 0;
    private Text inputField;
    private string prefabName;

    void Awake()
    {
        Instance = this;

        inputField = GameObject.FindGameObjectWithTag("InputField").GetComponentInChildren<Text>();
    }

    void Start()
    {
        CreatePrefabs();
    }

    void Update()
    {
        
    }

    private void CreatePrefabs()
    {
        counter = 0;
        int numberPrefabs = Random.Range(minNumberPrefabs, gameObjects.Count * 2);

        for (int i = 0; i < numberPrefabs; i++)
        {
            int prefab = Random.Range(0, gameObjects.Count);

            Instantiate(gameObjects[prefab], transform);
        }
        SelectPrefab();
    }

    private void SelectPrefab()
    {
        bool resultFlag = false;

        while (!resultFlag)
        {
            int prefab = Random.Range(0, gameObjects.Count);
            prefabName = gameObjects[prefab].name;

            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name.Contains(prefabName))
                {
                    counter++;
                }
            }
            if (counter != 0)
            {
                resultFlag = true;
            }
        }
        DialogManager.Instance.extra = prefabName;
    }

    public void CheckPrefab()
    {
        if (DialogManager.Instance.canSelect)
        {
            answer = int.Parse(inputField.text);
            if (counter == answer)
            {
                currentStreak++;
                DeletePrefabs();

                DialogManager.Instance.index -= 2;
                SoundManager.Instance.PlayAudio("bien");
            }
            else
            {
                ChallengeManager.Instance.UploadStreak(currentStreak);
                currentStreak = 0;
                SoundManager.Instance.PlayAudio("mal");
            }
            DialogManager.Instance.NextLine();
        }
    }

    public void DeletePrefabs()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        StartCoroutine(ResetPrefabs());
    }

    private IEnumerator ResetPrefabs()
    {
        yield return new WaitForEndOfFrame();
        CreatePrefabs();
    }
}
