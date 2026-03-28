using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {

	public float rotationSpeed = 50f;

	[SerializeField] private SpriteRenderer shatterRenderer;
	[SerializeField] private GameObject particleExplosion;
	[SerializeField] private List<Sprite> shatterSprites = new List<Sprite>();
	[SerializeField] private float minshatterTime, maxshatterTime;

	private bool isRotating;
	private float shatterTime;
	private float curShatterTime;
	private int shatterCount;

	private void Start() {
		shatterTime = Random.Range(minshatterTime, maxshatterTime);
		curShatterTime = shatterTime;
	}

	void Update() {
		if (isRotating) curShatterTime -= Time.deltaTime;
		if (isRotating) transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
		if(curShatterTime <= 0) {
			if (shatterCount < shatterSprites.Count) {
				shatterRenderer.sprite = shatterSprites[shatterCount];
				curShatterTime = shatterTime;
				FindAnyObjectByType<AudioManager>().Play("Shatter");
			} else {
				FindAnyObjectByType<Player>().Jump();
			}
			shatterCount++;
		}
	}

	public void SetRotating(bool rotate, float rotationSpeed) {
		isRotating = rotate;
		this.rotationSpeed = rotationSpeed;
	}
}