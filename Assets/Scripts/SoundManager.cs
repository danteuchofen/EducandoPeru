using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private AudioClip sound;
    private AudioSource audioSource;
    private float duration;
    private string audioName;
    private int counter;

    void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        counter = 1;
    }

    public void PlayAudio(string type, bool checkCan = false)
    {
        switch (type)
        {
            case "voz":
                audioName = counter.ToString();
                counter++;
                break;
            case "bien":
            case "mal":
                audioName = type;
                break;
        }
        sound = Resources.Load<AudioClip>("Audios/voz" + audioName);

        if (sound == null)
        {
            duration = 3;
        }
        else
        {
            duration = sound.length;
            audioSource.PlayOneShot(sound);
        }

        if (checkCan)
        {
            if (type.Equals("voz"))
            {
                StartCoroutine(CheckFinishedAudio(1));
            }
            else
            {
                StartCoroutine(CheckFinishedAudio(2));
            }
        }
    }

    public IEnumerator CheckFinishedAudio(int typeOfCan)
    {
        yield return new WaitForSeconds(duration);

        switch (typeOfCan)
        {
            case 1:
                DialogManager.Instance.canPrint = true;
                break;
            case 2:
                DialogManager.Instance.canSelect = true;
                break;
        }
    }
}
