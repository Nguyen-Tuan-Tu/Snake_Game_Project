using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    // Kéo thả Object BackgroundMusic vào ô này trong Inspector
    public AudioSource bgmSource; 

    private void Awake()
    {
        Instance = this;
    }
    public void MuteBGM() {
        if (bgmSource != null) bgmSource.Pause();
    }

    public void UnmuteBGM() {
        if (bgmSource != null) bgmSource.UnPause();
    }
}
