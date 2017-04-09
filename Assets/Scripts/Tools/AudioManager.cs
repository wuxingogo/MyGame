using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wuxingogo.Runtime;

public class AudioManager : XMonoBehaviour {
	public AudioClip[] bgmClips = null;
	public AudioSource audioSource = null;
	// Use this for initialization
	public static AudioManager Instance = null;
	void Awake()
	{
		Instance = this;
	}
	[X]
	public void PlayNextBgm()
	{
		int index = Random.Range (0, bgmClips.Length);
		if (bgmClips.Length > 0) {
			AudioClip ac = bgmClips[index];
			audioSource.clip = ac;
			audioSource.Play ();
		}
	}

	void Update()
	{
		if (!audioSource.isPlaying)
			PlayNextBgm ();
	}
}
