using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    [SerializeField]private AudioSource BoosDeadMusic;
    [SerializeField]private AudioSource WinMusic;
    private AudioSource BossComeHeare;
    void Awake()
    {
        instance = this;
        BossComeHeare=this.GetComponent<AudioSource>();
        BossComeHeare.volume=0.3f;
    }
    public void BossDeadMusic()
    {
        BoosDeadMusic.Play();
        StartCoroutine(volumeControl());
    }
    public void Win(){
        WinMusic.Play();
    }
IEnumerator volumeControl(){
    yield return new WaitForSeconds(1);
    BossComeHeare.volume=0.1f;
} 
}
