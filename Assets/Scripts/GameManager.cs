using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public static GameManager Instance;

	[Header("Death UI")]
	[SerializeField] private List<string> deathTexts = new List<string>();
	[SerializeField] private GameObject scoreStats, restart, menu, astronout;
	[SerializeField] private TextMeshProUGUI deathText;
	[SerializeField] private TextMeshProUGUI deathScoreText;
	[SerializeField] private TextMeshProUGUI deathBestText;
	[SerializeField] private float typingSpeed;
	[SerializeField] private float scoreSpeed;

	[Header("Score UI")]
	[SerializeField] private TextMeshProUGUI scoreText;
	[SerializeField] private TextMeshProUGUI comboText;
	[SerializeField] private int maxCombo;
	[SerializeField] private float maxComboTime;

	[Header("Audio")]
	[SerializeField] private AudioManager audioManager;


	private int score;
	private int best;
	private int combo = 1;
	private float comboTimer;

	private void Awake() {
		best = LoadPrefs();
		if (Instance == null) {
			Instance = this;
		} else {
			Destroy(gameObject);
			return;
		}
		UpdateText();
	}

	private void Update() {
		if (combo > 1) {
			comboTimer -= Time.deltaTime;

			if (comboTimer <= 0) {
				comboTimer = maxComboTime;
				combo = 1;
				UpdateText();
				PlayPopAnimation(comboText);
			}
		}
	}
	#region UI
	public float SpeedIncrease(float rotationVal) {
		return rotationVal + rotationVal / 50;
	}

	public void AddScore() {
		comboTimer = maxComboTime;
		score += combo;
		if (best < score) {
			best = score;
		}
		combo = Mathf.Min(maxCombo, combo + 1);
		PlayPopAnimation(scoreText);
		PlayPopAnimation(comboText);
		UpdateText();
	}

	private void UpdateText() {
		scoreText.text = score.ToString();
		comboText.text = "x" + combo.ToString();
	}

	private void PlayPopAnimation(TMP_Text textElement) {
		Vector3 defaultScale = Vector3.one;

		LeanTween.cancel(textElement.gameObject);
		textElement.transform.localScale = defaultScale;

		LeanTween.scale(textElement.gameObject, defaultScale * 1.3f, 0.2f)
			.setEase(LeanTweenType.easeOutBack)
			.setIgnoreTimeScale(true) // 👈 Add this
			.setOnComplete(() => {
				LeanTween.scale(textElement.gameObject, defaultScale, 0.2f)
					.setIgnoreTimeScale(true); // 👈 Also needed for the return animation
			});
	}

	public void GameOver() {
        audioManager.Stop("MainTheme");

        SavePrefs();
		FindAnyObjectByType<AudioManager>().Play("Lose");
		string text = deathTexts[Random.Range(0, deathTexts.Count)];
		Player player = FindAnyObjectByType<Player>();
		Destroy(player.gameObject);

		Time.timeScale = 0;

		GameObject gameOverCanvas = deathText.transform.parent.gameObject;
		gameOverCanvas.SetActive(true);
		comboText.transform.parent.parent.gameObject.SetActive(false);

		// Move UI elements from off-screen down into position
		MoveFromTop(scoreStats);
		MoveFromTop(restart);
		MoveFromTop(menu);
		MoveFromTop(astronout);
		MoveFromTop(deathText.gameObject);

		// 👉 Wait until UI moves finish, then animate score
		StartCoroutine(DelayedScoreAndTextAnimation(text));
	}

	private void MoveFromTop(GameObject obj) {
		RectTransform rect = obj.GetComponent<RectTransform>();
		float startY = Screen.height * 1.2f; // Start off-screen
		float endY = rect.anchoredPosition.y; // Target position

		rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, startY); // Start position

		// Animate the movement with a smooth transition (no bounce)
		LeanTween.moveY(rect, endY, 0.7f).setEase(LeanTweenType.easeInOutQuad).setIgnoreTimeScale(true);
	}


	public void Restart() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		Time.timeScale = 1f;
	}
	public void Menu() {
        SceneManager.LoadScene("Menu");
		Time.timeScale = 1f;
    }
    private IEnumerator TypeText(string text) {
		deathText.text = ""; // Clear the text before starting the typing animation
		
		audioManager.Play("Type");
		foreach (char letter in text) {
			deathText.text += letter; // Add one letter at a time
			yield return new WaitForSecondsRealtime(typingSpeed); // Use WaitForSecondsRealtime instead of WaitForSeconds
		}
		audioManager.Stop("Type");
	}
	private IEnumerator DelayedScoreAndTextAnimation(string deathMessage) {
		yield return new WaitForSecondsRealtime(0.8f); // Wait for UI to finish moving

		// Start typing the death message at the same time
		StartCoroutine(TypeText(deathMessage));

		// Animate score first
		yield return StartCoroutine(AnimateScore(deathScoreText));

		// After score finishes, show and animate best
		deathBestText.text = best.ToString();
		PlayPopAnimation(deathBestText);
	}

	IEnumerator AnimateScore(TextMeshProUGUI text) {
		int curScore = 0;
		while (curScore < score) {
			curScore++;
			text.text = curScore.ToString();
			yield return new WaitForSecondsRealtime(scoreSpeed);
		}
	}
	#endregion
	#region Save/Load
	public void SavePrefs() {
		PlayerPrefs.SetInt("Best", best);
		PlayerPrefs.Save();
	}

	public int LoadPrefs() {
		int best = PlayerPrefs.GetInt("Best");
		return best;
	}
	#endregion
}