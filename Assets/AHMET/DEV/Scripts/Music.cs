using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour
{

    public AudioSource audio;
    public AudioClip[] myMusic = new AudioClip[3];
    private int last;
    private int current;
    // Use this for initialization
    void Start()
    {
        playRandomMusic();
    }

    // Update is called once per frame
    void Update()
    {
        if (!audio.isPlaying)
            playRandomMusic();
    }
    void playRandomMusic()
    {
        current = Random.Range(0, myMusic.Length);
        if (current == last)
        {
            playRandomMusic();
        }
        else
        {
            audio.clip = myMusic[current] as AudioClip;
            last = current;
            audio.Play();
        }
        
    }
}