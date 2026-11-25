# Penguin Ice Run

A 3D mobile platformer made in Unity 6 (Android, landscape).

The Walrus King stole all the fish. Run and jump across the ice floes as a little penguin,
collect the fish, open the portals and defeat the Walrus King.

- **Engine:** Unity 6.6 (6000.6.0f1), URP, Input System
- **Platform:** Android, landscape
- **Controls:** on-screen joystick (move), JUMP, FIRE (snowball), PAUSE
- **Scenes:** MainMenu → Level1 → Level2 → Level3_Arena → Level3_Boss → Win
- **Enemies (Level3_Arena):** Patroller seal (Patrol → Chase → Attack), Shooter snowman (Watch → Aim → Cooldown), Charger walrus (Idle → WindUp → Charge → Rest). NavMesh + Raycast vision. Script map: `Docs/Enemy_Script_Map.png`
- **Data (Assignment 11):** balance numbers in `Assets/Data` (Player_Stats, *_Stats, Boss_Stats, Item_Values, Level_01–04). Progress and volume in `save.json` (persistentDataPath). Level select, settings and Reset progress in the main menu.
- **Sound (Assignment 12):** AudioMixer `Assets/Audio/GameMixer.mixer` (Master → Music → Tracks, Master → SFX; snapshots Normal and Paused). One `AudioManager` (DontDestroyOnLoad) plays the music with a 1 s crossfade and all sound effects with pitch 0.92–1.08. Music and SFX volume sliders in Settings (decibels, saved in `save.json`).
- **Author:** Madiyar Tokbergen — Game Development Fundamentals, Narxoz University

AI use is logged in `AI_LOG.txt` and `DECISIONS.md`.

## Sound and music: sources and licenses

All sound effects and both music tracks were made for this project by code: the Python script
`SoundTools/make_sounds.py` (numpy, simple square / triangle / saw waves and noise), written with
the help of an AI agent (Claude) and run by me. No samples or recordings from other people are used.
**License: own work, CC0 1.0 (public domain).**

| File (`Assets/Audio`) | Event in the game | Source | License |
|---|---|---|---|
| Jump.wav | penguin jumps | generated, `make_sounds.py` | CC0, own work |
| Throw.wav | penguin throws a snowball | generated, `make_sounds.py` | CC0, own work |
| Pickup.wav | fish / heart picked up | generated, `make_sounds.py` | CC0, own work |
| Hurt.wav | penguin is hit | generated, `make_sounds.py` | CC0, own work |
| WindUp.wav | enemy wind-up (red "!") — seal, snowman, walrus, boss | generated, `make_sounds.py` | CC0, own work |
| BossShoot.wav | snowman and boss shoot | generated, `make_sounds.py` | CC0, own work |
| EnemyDeath.wav | enemy dies | generated, `make_sounds.py` | CC0, own work |
| Click.wav | any UI button | generated, `make_sounds.py` | CC0, own work |
| PortalOpen.wav | portal opens | generated, `make_sounds.py` | CC0, own work |
| Win.wav | level complete / win screen | generated, `make_sounds.py` | CC0, own work |
| Lose.wav | player loses (no hearts / time up) | generated, `make_sounds.py` | CC0, own work |
| MenuMusic.wav | music: main menu and win screen | generated, `make_sounds.py` | CC0, own work |
| Music.wav | music: all levels | generated, `make_sounds.py` | CC0, own work |

To make the sounds again: `python3 SoundTools/make_sounds.py Assets/Audio`
