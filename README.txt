PENGUIN ICE RUN — Game Development Fundamentals, Midterm Project
Student: Madiyar Tokbergen
Path: B — 3D third-person mobile platformer

ENGINE
  Unity 6.6 (6000.6.0f1), Universal Render Pipeline 17.6, Input System 1.20
  Target platform: Android (APK), landscape. Keyboard works in parallel for the editor.

STORY
  The Walrus King stole all the fish. The penguin travels across the ice floes,
  collects the fish, opens the portals and defeats the Walrus King.

GAME STRUCTURE (Build Settings order)
  0 MainMenu     — title, story, PLAY / QUIT
  1 Level1       — tutorial: platforms at different heights, a ferry ice floe (Animation Clip),
                   ice spikes; collect 5 of 6 fish to open the portal
  2 Level2       — harder & longer: patrolling seals, bobbing floes, falling icicles with red
                   warning marks, a heart (medkit); collect 8 of 10 fish to open the portal
  3 Level3_Boss  — boss arena: the Walrus King (12 HP, HP bar), ice-ball fan attack with "!"
                   warning, icicle rain with red columns, pauses to fight back, second phase
                   below 50% HP (more and wider attacks), ice pillars as cover, 2 hearts
  4 Win          — "YOU WIN!", fish counter, MENU button
  Death (HP = 0 or falling into the water) restarts the current level.

CONTROLS
  Phone:    on-screen joystick (move), JUMP button, FIRE button (snowball, auto-aim)
  Keyboard: WASD / arrows — move, Space — jump, X — fire

WHERE EACH COURSE TOPIC IS
  A4 Physics   — PenguinController (Rigidbody velocity, SphereCast ground check), colliders,
                 trigger pickups (Fish, HeartPickup, Portal), collision damage (Enemy, Hazard)
  A5 Animation — Penguin Animator: Idle / Run / Jump (+ turns toward movement);
                 animated seal, boss, fish and hearts; moving floes via Animation Clips
                 (FloeFerry_*, FloeBob) + MovingPlatform
  A6 Scripting — Thrower (cooldown), Boss (Instantiate spawn patterns, coroutines, phases),
                 Health (invincibility blinking coroutine, GetComponent), inheritance:
                 Enemy -> Seal, Boss; Projectile -> Snowball, BossShot, Icicle;
                 GameManager (SceneManager.LoadScene), sounds
  A7 Input     — Input Actions asset (Assets/Input/PlayerControls.inputactions), PlayerInput
                 (Send Messages), On-Screen Stick + On-Screen Buttons anchored to the corners

SCRIPTS (Assets/Scripts)
  GameManager, PenguinController, Health, CameraFollow, Thrower, Fish, HeartPickup, Hazard,
  Enemy, Seal, Boss, Projectile, Snowball, BossShot, Icicle, Spawner, MovingPlatform, Portal,
  MenuUI, MenuCamera
  Editor tools (not part of the game): Assets/Editor/MidtermBuilder.cs (builds the scenes,
  prefabs, materials and animations — Tools > Build Midterm Game), ApkBuilder.cs (Tools > Build APK)

ASSETS, SOURCES AND LICENSES
  3D models (penguin, seals, Walrus King, fish, hearts, ice floes, spikes, icebergs, portal):
      made by me in Unity from primitives and simple procedural low-poly meshes — own work.
  Materials, skyboxes (Unity procedural skybox), particles (snow, confetti, sparkles):
      made in Unity — own work.
  UI icons (heart, fish): drawn for this project — own work.
  Font: Unity built-in LegacyRuntime (Arial).
  Sounds (Assets/Audio: Jump, Pickup, Hurt, Throw, BossShoot, Win, Music):
      SOURCE: ______________________   LICENSE: ______________________
  Unity packages: Input System, URP, uGUI (Unity Companion License).

AI USAGE
  See AI_LOG.txt.
