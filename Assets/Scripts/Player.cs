using UnityEngine;
using System.Collections.Generic;
public class Player : MonoBehaviour {
	[SerializeField] private float alignmentSpeed = 2.5f, baseJumpForce = 5f;
	[SerializeField] private GameObject particleExplosion;
	[SerializeField] private float maxFallTimer;
	private Rigidbody2D rb;
	private Animator anim;
	private Transform currentAsteroid;
	public bool isGrounded;
	private float fallTimer;
	private bool gameOverTriggered = false;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
	}

	void Update() {
		anim.SetBool("isGrounded", isGrounded);
		if (isGrounded && Input.GetMouseButtonDown(0))
			Jump();
		AlignToAsteroid();

		if (!isGrounded) {
			fallTimer += Time.deltaTime;
			if (fallTimer >= maxFallTimer && !gameOverTriggered)
			{
				gameOverTriggered = true;
				GameManager.Instance.GameOver();
			}
		} else {
			fallTimer = 0f;
		}
	}

	public void Jump() {
		anim.SetTrigger("Jump");
		CustomCamera.Instance.CameraShake();
		FindAnyObjectByType<AudioManager>().Play("Jump");
		GameObject particle = Instantiate(particleExplosion, currentAsteroid.position, Quaternion.identity);
		transform.SetParent(null);
		Destroy(currentAsteroid.gameObject);
		Destroy(particle, 5f);
		FindAnyObjectByType<AudioManager>().Play("Explosion");

		Vector2 jumpDirection = transform.up;
		rb.bodyType = RigidbodyType2D.Dynamic;
		rb.linearVelocity = jumpDirection * baseJumpForce;
		isGrounded = false;

		MaxVelocity(baseJumpForce);
	}

	private void AlignToAsteroid() {
		if (!isGrounded || currentAsteroid == null) return;
		Vector3 direction = (transform.position - currentAsteroid.position).normalized;
		if (Vector3.Angle(transform.up, direction) < 0.5f) {
			return;
		}
		Quaternion targetRotation = Quaternion.FromToRotation(transform.up, direction) * transform.rotation;
		transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * alignmentSpeed);
	}

	private void MaxVelocity(float maxVelocity) {
		float velocity = Mathf.Min(maxVelocity,rb.linearVelocity.y);
		rb.linearVelocityY = velocity;
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (!collision.transform.CompareTag("Asteroid"))
			return;
		currentAsteroid = collision.transform;
		isGrounded = true;
		rb.linearVelocity = Vector2.zero;
		rb.bodyType = RigidbodyType2D.Kinematic;
		transform.SetParent(currentAsteroid);
		currentAsteroid.GetComponentInParent<Asteroid>().SetRotating(true,
		GameManager.Instance.SpeedIncrease(currentAsteroid.GetComponentInParent<Asteroid>().rotationSpeed));
		GameManager.Instance.AddScore();
		SpawnManager.Instance.transformAsteroid = currentAsteroid;
		SpawnManager.Instance.asteroidTransforms.Remove(currentAsteroid);
	}
}