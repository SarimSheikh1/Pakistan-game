# Pakistan Street Simulator 🇵🇰

A realistic 3D open-world driving and life simulator inspired by Pakistani cities (Lahore, Karachi, Faisalabad). Built with **Unity** and **C#**, powered by the **Universal Render Pipeline (URP)** and designed for online multiplayer via **Netcode for GameObjects**.

---

## 🎮 Features
- **Third-Person Movement & Camera**: Smooth character navigation with sprint, jump, ground detection, and full 360-degree orbital camera.
- **Pakistani Urban Environment**: Bazaars, local shops, footpaths, roadside khokhas, and authentic urban architecture.
- **Vehicles System (In Progress)**: Realistic motorbikes (70cc/125cc), rickshaws, and cars with physics-based driving and enter/exit mechanics.
- **Online Multiplayer**: Host/Client lobby and synchronization powered by Unity Netcode for GameObjects.
- **Missions & Economy**: Delivery challenges, local jobs, and currency progression.

---

## 🛠️ Tech Stack
- **Engine**: Unity (URP - Universal Render Pipeline)
- **Language**: C#
- **Multiplayer**: Netcode for GameObjects (NGO) & Unity Transport (UTP)
- **Target Platform**: PC / Windows

---

## 📂 Project Structure
```text
Assets/
├── Scenes/               # City prototypes and gameplay scenes
├── Scripts/
│   ├── Player/           # Player movement, animation, and interaction
│   ├── Camera/           # Third-person orbital follow camera
│   ├── Vehicles/         # Motorbike and car physics
│   ├── Multiplayer/      # Network sync and lobby systems
│   ├── Missions/         # Delivery and quest logic
│   └── UI/               # Menus, HUD, and minimap
├── Prefabs/              # Reusable game objects
├── Materials/            # PBR materials and shaders
├── Textures/             # Road, sidewalk, and building textures
└── Audio/                # Ambient city sounds and local sound effects
```

---

## 🚀 Getting Started
1. Clone the repository:
   ```bash
   git clone https://github.com/SarimSheikh1/Pakistan-game.git
   ```
2. Open **Unity Hub** and select **Add project from disk**.
3. Choose the project directory and open with **Unity 6 LTS** or **Unity 2022.3 LTS**.
4. Open scene: `Assets/Scenes/Street_Prototype.unity`.
5. Press **Play** to test movement and camera!
