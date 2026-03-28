using Unity.Cinemachine;
using UnityEngine;
public class CustomCamera : MonoBehaviour {
	public static CustomCamera Instance;

	[SerializeField] private Transform targetTransform;
	[SerializeField] private CinemachineCamera cinemachineCamera;
	[SerializeField] private CinemachineImpulseSource impulseSource;
	[SerializeField] private float zoomSpeed = 2f;
	[SerializeField] private float floatOffset = 3f;

	private Player player;
	private float maxOrthographicSize = 15f;
	private float minOrthographicSize = 5f;
	Vector3 offset = new Vector3(0, 0, -10);
	Vector3 targetPosition;

	private void Awake() {
		Instance = this;
	}
	void Start() => player = FindAnyObjectByType<Player>();

	void LateUpdate() {
		if (player == null) return;

		Transform currentAsteroid = player.transform.parent;

		if (currentAsteroid == null || !currentAsteroid.gameObject.activeInHierarchy) {
			targetPosition = player.transform.position + offset;
			cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(cinemachineCamera.Lens.OrthographicSize,
				minOrthographicSize, zoomSpeed * Time.deltaTime);
			targetTransform.position = targetPosition;
			return;
		}

		Transform nextAsteroid = FindNextAsteroid();

		if (!player.isGrounded) {
			targetPosition = player.transform.position + offset;
		} else if (nextAsteroid != null) {
			targetPosition = (currentAsteroid.position + nextAsteroid.position) / 2f + offset;
			float targetOrthographicSize = CalculateRequiredOrthographicSize(currentAsteroid, nextAsteroid);
			cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(cinemachineCamera.Lens.OrthographicSize,
				Mathf.Clamp(targetOrthographicSize, minOrthographicSize, maxOrthographicSize),
				zoomSpeed * Time.deltaTime);
		} else {
			targetPosition = currentAsteroid.position + offset;
			cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(cinemachineCamera.Lens.OrthographicSize,
				minOrthographicSize, zoomSpeed * Time.deltaTime);
		}

		targetTransform.position = targetPosition;
	}

	private float CalculateRequiredOrthographicSize(Transform currentAsteroid, Transform nextAsteroid) {
		float distance = Vector3.Distance(currentAsteroid.position, nextAsteroid.position);
		float aspectRatio = (float)Screen.width / Screen.height;
		return distance / (2f * aspectRatio) + floatOffset;
	}

	private Transform FindNextAsteroid() {
		var asteroids = SpawnManager.Instance.asteroidTransforms;
		if (asteroids.Count == 0) return null;

		Transform closest = null;
		float closestDist = Mathf.Infinity;
		Transform current = player.transform.parent;

		if (current == null) return null;

		foreach (Transform asteroid in asteroids) {
			if (asteroid == current) continue;
			float dist = Vector3.Distance(current.position, asteroid.position);
			if (dist < closestDist) {
				closestDist = dist;
				closest = asteroid;
			}
		}
		return closest;
	}

	public void SetDamping(float damping) {
		cinemachineCamera.gameObject.GetComponent<CinemachinePositionComposer>().Damping = new Vector3(damping, damping, damping);
	}

	public void CameraShake() {
		impulseSource.GenerateImpulseWithForce(1f);
	}
}