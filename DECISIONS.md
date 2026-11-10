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
