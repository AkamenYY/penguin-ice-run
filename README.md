# Penguin Ice Run

A 3D mobile platformer made in Unity 6 (Android, landscape).

The Walrus King stole all the fish. Run and jump across the ice floes as a little penguin,
collect the fish, open the portals and defeat the Walrus King.

- **Engine:** Unity 6.6 (6000.6.0f1), URP, Input System
- **Platform:** Android, landscape
- **Controls:** on-screen joystick (move), JUMP, FIRE (snowball), PAUSE
- **Scenes:** MainMenu → Level1 → Level2 → Level3_Arena → Level3_Boss → Win
- **Enemies (Level3_Arena):** Patroller seal (Patrol → Chase → Attack), Shooter snowman (Watch → Aim → Cooldown), Charger walrus (Idle → WindUp → Charge → Rest). NavMesh + Raycast vision. Script map: `Docs/Enemy_Script_Map.png`
- **Author:** Madiyar Tokbergen — Game Development Fundamentals, Narxoz University

AI use is logged in `AI_LOG.txt` and `DECISIONS.md`.
