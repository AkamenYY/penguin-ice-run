# DECISIONS.md — Penguin Ice Run

What I asked the AI agent (Claude), what it got wrong, what I changed and why.

## Week 9 — Mobile Game Kickoff (Assignment 8–9)

```
2026-11-03  Asked the agent to make the midterm code as simple as possible so I can explain it. Removed knockback, coyote time, jump buffer, auto-aim and boss walking.
2026-11-04  GitHub Desktop created the repo in my home folder, not in the project. Deleted it, ran git init in the project folder and added it again.
2026-11-04  Pause panel: the level hint text showed through the panel over "PAUSED". Added one line in PauseMenu.Pause() that hides the hint.
2026-11-04  The GDD still said FIRE has auto-aim, but we removed it. Fixed the GDD.
```

## Week 10 — Three Enemy Types (Assignment 10)

```
2026-11-11  Asked the agent for 3 enemy types with simple state machines (enum + switch): Patroller (seal), Shooter (snowman), Charger (walrus). All share one base class Enemy.
2026-11-11  The enemies never saw me: the agent's Raycast used DefaultRaycastLayers, but the penguin is on the Ignore Raycast layer. Changed it to Physics.AllLayers.
2026-11-11  The old seal hurt me on touch with no warning. New enemies have touchDamage = false; every attack has a red "!" wind-up (0.5 / 0.6 / 0.7 s) and a cooldown.
2026-11-11  The Boss had its own copy of the player reference. Moved it to the Enemy base class so it is in one place.
2026-11-11  Charger uses agent.Move() for the dash, so it cannot leave the NavMesh: no flying through walls or into the water.
2026-11-11  Playtest: ______ — __ tries, ______ — __ tries. Changed: ______ __ → __, ______ __ → __. Now: __ tries.
```

## Week 11 — Data and Saves (Assignment 11)

```
2026-11-18  Asked the agent to move every balance number into ScriptableObjects: PlayerStats, EnemyStats (one asset per enemy type), BossStats, ItemValues, LevelData. Scripts only read stats.*, serialized fields have no default numbers.
2026-11-18  Levels: enemies are no longer placed in the scenes. Each LevelData has a list of EnemySpawn (prefab, position, angle, patrol length); the same GameManager code spawns them, plus fish goal, time limit and message.
2026-11-18  Save: JsonUtility + save.json in persistentDataPath, write to save.tmp then File.Replace. No BinaryFormatter, no Dictionary (unlocked levels are a List<int>).
2026-11-18  Broken save test did nothing in the Editor: Enter Play Mode keeps static fields, so the old data stayed in memory. Added a RuntimeInitializeOnLoadMethod that clears it, so every Play reads the file again like on the phone.
2026-11-18  Found a week-10 bug: the menu seals searched for the player in the main menu (no player there) and threw NullReferenceException. Enemy.Start now checks for null.
```

## Week 12 — Sound Pass (Assignment 12)

```
2026-11-25  The old sounds had no source or license, so I asked the agent to make all sounds and both music tracks with a Python script (SoundTools/make_sounds.py). Own work, CC0; listed in README.
2026-11-25  One AudioManager with DontDestroyOnLoad plays music on two AudioSources with a 1 s crossfade; if the next scene wants the same track it does not restart it. Removed the old "Music" object from every scene.
2026-11-25  Volume: Music and SFX sliders go through Log10(v) × 20 (decibels). Slider min is 0.0001, because Log10(0) is minus infinity. Saved in save.json (new fields musicVolume and sfxVolume; old saves still load with defaults).
2026-11-25  Paused snapshot: Music volume is the exposed slider parameter, and an exposed parameter is no longer changed by snapshots. So there is a child group Tracks: the slider moves Music, the snapshot moves Tracks (−16 dB + lowpass 600 Hz). Mixer update mode = Unscaled Time, so it works while Time.timeScale = 0.
2026-11-25  Wind-up sound plays in Enemy.ShowWarning(true), so every enemy and the boss are heard before the hit. Repeated sounds get pitch 0.92–1.08 from a pool of 4 SFX sources, so one sound does not change the pitch of another.
```

