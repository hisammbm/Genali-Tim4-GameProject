using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("--------Audio Sources-------")]
    [SerializeField] AudioSource musicScource;
    [SerializeField] AudioSource SFXSource;

    [Header("----------Audio Clip--------")]
    public AudioClip background;
    public AudioClip Shoot;
    public AudioClip Explosion;

    private void Start()
    {
        musicScource.clip = background;
        musicScource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }


}
