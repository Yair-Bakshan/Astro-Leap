using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour {
	[SerializeField] private List<ParticleSystem> particles = new List<ParticleSystem>();

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start() {
		if (particles != null) {
			foreach (ParticleSystem p in particles) {
				p.Play();
			}
		}
	}

	// Update is called once per frame
	void Update() {
	}
}
