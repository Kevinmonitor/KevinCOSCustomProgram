using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

	public static SoundManager Instance { get; private set; }

    public Camera _camera;

    [Header("SOURCES")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("AUDIO")]
    public AudioClip music;
    public AudioClip enemyFire;
    public AudioClip playerFire;
    public AudioClip playerDamage;
    public AudioClip enemyBoom;

	private void Awake()
	{
		
		if (Instance != null && Instance != this) 
		{ 
			Destroy(this); 
		} 
		else 
		{ 
			Instance = this; 
		} 

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartMusic(){
        musicSource.clip = music;
        musicSource.Play();
    }

    public void StopMusic(){
        musicSource.Stop();
    }

    public void PlaySFX(AudioClip clip, float volumeScale = 1.0f){
        SFXSource.PlayOneShot(clip, volumeScale);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
