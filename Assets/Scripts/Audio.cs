using UnityEngine;

[System.Serializable]
public class Sound {
	public string name;
	public float volume; 
	public float minPitch,maxPitch;
	public AudioClip clip;
	[HideInInspector] public AudioSource source;
}
