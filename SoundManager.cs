using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip _point, _swipe, _button;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start() {
        _point = Resources.Load<AudioClip>("point");
        _swipe = Resources.Load<AudioClip>("swipe");
        _button = Resources.Load<AudioClip>("button");
        audioSrc = GetComponent<AudioSource>();
    }

    public static void PlaySound(string clip) {
        if (PlayerPrefs.GetInt("sfx" , 1) == 1){
        switch (clip) {
            case "point":
                audioSrc.PlayOneShot(_point);
                break;
            case "swipe":
                audioSrc.PlayOneShot(_swipe);
                break;
            case "button":
                audioSrc.PlayOneShot(_button);
                break;
        }
        }
    }
}
