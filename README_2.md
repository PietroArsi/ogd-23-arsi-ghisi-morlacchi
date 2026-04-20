# Catnipped!

A cooperative multiplayer 3D game built in Unity. Two to four players run an underground catnip operation, harvesting, cooking, and processing catnip while defending against P.U.P.S. (Paws United for Protecting Society) raids.

Three students built it for the **Online Game Design** university course.

---

## Gameplay

Players have five minutes to maximize catnip output through a multi-step pipeline:

1. **Harvest** raw catnip from the field
2. **Cook** it in furnaces (5–10 seconds)
3. **Cut** the cooked batch on cutting tables (5 seconds)
4. **Store** the finished product

Enemy dogs and pawlice officers raid throughout the match. Five evidence points ends the game. Output volume determines the final score: one to three "crunchies" (stars).

Players can also catch mice for bonus points and build defensive traps and walls through a construction menu.

---

## Tech stack

| Layer | Technology |
|---|---|
| Engine | Unity 2021.3.22f1 (LTS) |
| Language | C# |
| Networking | Unity Netcode for GameObjects 1.2.0 |
| Matchmaking | Unity Relay + Lobby Services |
| Rendering | Universal Render Pipeline (URP) |
| Camera | Cinemachine 2.8.9 |
| UI | TextMesh Pro, UGUI |
| Navigation | Unity NavMesh |

---

## Architecture

### Server-authoritative state machine

A state machine in `GameManagerStates.cs` drives the game lifecycle:

```
WaitingOtherPlayers → CountdownToStart (4s) → GamePlaying (300s) → GameEnd
```

The host drives all transitions. `NetworkVariable<State>` propagates each change to clients via `OnValueChanged` callbacks.

### Priority-based interaction

`PickAndPlace.cs` resolves interactions by sorting overlapping colliders by the priority score from `SpawnableObjParent.GetPriority()`. When a player stands near multiple interactables, the highest-priority target wins without asking the player to choose.

### Client-server split

- The host runs game state, enemy spawning, and win/loss logic via `[ServerRpc]`
- The host broadcasts state changes via `[ClientRpc]` for UI, sound, and object visibility
- Each `PlayerNetwork` exposes a `LocalInstance` reference populated only on the owning client, so input handling stays local

### Session management

`ConnectionManager.cs` (singleton) maintains a `NetworkList<PlayerData>` visible to all clients. It handles color assignment, name generation, Unity Authentication, and clean disconnects for both host and client, mid-lobby and mid-game.

### Enemy AI

Dogs spawn every 30 seconds and navigate via NavMesh agents (`NetworkNavMeshAgent.cs`). Players can catch roaming mice for bonus points.

---

## Systems

- Cooking and cutting pipelines: timed, multi-stage resource processing with network-synchronized state
- Evidence system: tracks pawlice suspicion; five points ends the game
- Achievement system: persistent unlocks stored across sessions via `SaveManager`
- Construction menu: runtime placement of traps and walls
- Scoring: crunchies rating computed from final production volume
- Audio: separate managers for BGM, player SFX, and UI feedback

---

## Scenes

| Scene | Purpose |
|---|---|
| MainMenu | Title screen |
| LobbyManagement | Create or browse lobbies |
| CharacterSelectionScreen | Pick character before match |
| LevelSelection | Choose map |
| NetworkTestLevel | Main gameplay level |

---

## Controls

| Input | Action |
|---|---|
| WASD | Move |
| Mouse | Camera |
| Left Click | Interact / Scratch |
| E | Cut catnip |
| Q | Open construction menu |
| M | Meow |

---

## Team

Three students built Catnipped! over one academic semester:

- **Pietro Arsi** — UI systems, gameplay mechanics, scoring
- **Luca Ghisi** — networking foundation, enemy systems
- **Giovanni Morlacchi** — additional gameplay contributions

About 80 commits, with feature branches and regular merges.

---

## Running the project

1. Install **Unity 2021.3.22f1** via Unity Hub
2. Clone the repository
3. Open the project folder in Unity Hub
4. Open `Assets/Scenes/MainMenu.unity`
5. Press Play, or build via **File → Build Settings**

Multiplayer requires an internet connection. Unity Relay handles NAT traversal; you don't need a dedicated server.
