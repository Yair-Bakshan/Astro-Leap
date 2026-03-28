using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
	public static SpawnManager Instance;
	public List<Transform> asteroidTransforms = new List<Transform>();
	[HideInInspector] public Transform transformAsteroid;

	[SerializeField] private GameObject asteroidPrefab;
	[SerializeField] private int maxAsteroidCount = 1;

	[SerializeField] float[] clampYOffsets = new float[2];



	private void Awake() {
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Destroy(gameObject);
	}

	void Update() {
		if (asteroidTransforms.Count < maxAsteroidCount) SpawnAsteroid();
	}

	private void SpawnAsteroid() {
		Vector2 spawnPos = Vector2.zero;
		if (asteroidTransforms.Count > 0)
			spawnPos = asteroidTransforms[asteroidTransforms.Count - 1].position;
		else
			spawnPos = transformAsteroid.position;


		float[] yOffsets = new float[2];
		yOffsets[0] = Random.Range(clampYOffsets[0], clampYOffsets[1]);
		yOffsets[1] = Random.Range(-clampYOffsets[1], -clampYOffsets[0]);
		int randomNumY = Random.Range(0, 2);

		float xOffset = Random.Range(-8.5f, 8.5f);

		spawnPos += new Vector2(xOffset, yOffsets[randomNumY]);

		GameObject asteroid = Instantiate(asteroidPrefab, spawnPos, Quaternion.identity);
		asteroidTransforms.Add(asteroid.transform);
	}
}