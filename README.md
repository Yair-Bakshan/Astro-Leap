# 🚀 Astro Leap

A fast-paced arcade Unity game where you leap between rotating asteroids, build combos, and try not to fall into the void. This project explores physics-based movement, dynamic camera systems, and responsive arcade controls.

[**🎮 Play Now (Itch.io)**](https://devhex.itch.io/astro-leap)

![Gameplay](https://github.com/Yair-Bakshan/Astro-Leap/blob/main/gameplay.gif)

---

## 🧠 About

**Astro Leap** started as a small Unity project in 2025 to explore physics, camera systems, and responsive controls. It was polished so that it will feel *good enough to play at all.*

---

## ✨ Features

* 🎯 **Skill-Based Gameplay:** Precision jumping between rotating asteroids.
* 🔥 **Combo System:** Rewards perfect timing to skyrocket your score.
* 🎥 **Dynamic Camera:** Built with **Cinemachine** for auto-zoom, centering, and screen shake.
* 💥 **Polished FX:** Particle-driven asteroid destruction and "Shatter" states.
* 🔊 **Audio System:** Custom manager with pitch variation and reactive soundscapes.
* 🧾 **Animated UI:** Features a typewriter effect and counting score animations on the death screen.

---

## 🎮 Controls

* **Left Click / Tap** → Jump to the next asteroid.
* **Menu Navigation** → Use the **Play** button to start your run.

---

## 🛠️ Technical Setup

### Requirements
* **Unity 6.0 LTS** or later.
* **TextMeshPro** & **Cinemachine** (Install via Package Manager).
* **LeanTween** (Add to project for UI/Object animations).

### Quick Start
1.  **Clone the Repository:**
    ```bash
    git clone https://github.com/Yair-Bakshan/Astro-Leap.git
    ```
2.  **Configure Scenes:** Ensure `Menu` and `Game` are added to your **Build Settings**.
3.  **Wire Audio:** Assign clips to the `AudioManager` matching these keys: `MainTheme`, `Type`, `Jump`, `Explosion`, `Lose`, `Shatter`.
4.  **Hit Play:** Open the `Menu` scene and start leaping!

---

## 📂 Project Structure (`Assets/Scripts`)

| Script | Responsibility |
| :--- | :--- |
| **Player.cs** | Handles jump physics, asteroid alignment, and "fall" logic. |
| **GameManager.cs** | Manages scores, high scores (PlayerPrefs), and death UI. |
| **SpawnManager.cs** | Procedural asteroid generation and object tracking. |
| **Asteroid.cs** | Controls rotation speeds and shatter state triggers. |
| **CustomCamera.cs** | Cinemachine controller for zoom and impulse (shake) logic. |
| **AudioManager.cs** | Global system for pitch-perfect SFX and music. |

---

## 📝 Notes
* **Audio Initialization:** `AudioManager.Start()` initializes sources with `pitch = 0f`. Sound is triggered dynamically during gameplay.
* **Prefabs:** Ensure `asteroidPrefab` and `particleExplosion` are assigned in the `SpawnManager` and `Player` inspectors.

## 🤝 Contributing
Contributions are welcome! Please keep changes focused, follow the existing formatting standards, and add unit tests where applicable.

## 📄 License
This project is licensed under the [MIT License](LICENSE).