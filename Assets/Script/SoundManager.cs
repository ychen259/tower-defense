using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : singleTon<SoundManager> {
    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private AudioSource sfxSource;

    [SerializeField]
    private Slider musicSlider;
    
    [SerializeField]
    private Slider sfxSlider;

    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    // Use this for initialization
    public void Start () {
        AudioClip[] clips = Resources.LoadAll<AudioClip>("audio") as AudioClip[];

        foreach(AudioClip clip in clips)
        {
            /*clip.name == name of file*/
            audioClips.Add(clip.name, clip);
        }

        LoadVolume();

       // musicSlider.onValueChanged.AddListener(delegate { UpdateMusic(); }); /*change musicSlider, then call update music function*/
        //sfxSlider.onValueChanged.AddListener(delegate { UpdateSFX(); });
    }

    public void PlaySFX(string name)
    {
        sfxSource.PlayOneShot(audioClips[name]);
    }

    public void UpdateMusic()
    {
        musicSource.volume = musicSlider.value / 100f;

        /*store sfxslider.value SFX*/
        PlayerPrefs.SetFloat("Music", musicSlider.value);
        PlayerPrefs.Save();
    }

    public void UpdateSFX()
    {
        sfxSource.volume = sfxSlider.value / 100f;

        PlayerPrefs.SetInt("SFX", (int)sfxSlider.value);
        PlayerPrefs.Save();
    }

    /*set the default value*/
    public void LoadVolume()
    {
        /*Unity3D中值是通过键名来读取的，当值不存在时，返回默认值*/
        /*在游戏关闭再重启时候，会保留这个值*/

        /*musicSlider.value from 0 to 100*/
        sfxSlider.value = PlayerPrefs.GetInt("SFX", 50);
        musicSlider.value = PlayerPrefs.GetFloat("Music", 50f);
    }
}
