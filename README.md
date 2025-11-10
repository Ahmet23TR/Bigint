# Bigint — Unity Multiplayer Project

A Unity project that demonstrates multiplayer functionality using Photon (PUN / Fusion) and contains gameplay scripts for a simple networked sample. The repository includes Photon SDK code and demo scenes, plus local game logic in `Assets/Scripts`.

## Key points
- Engine: Unity (open the project folder with Unity Editor)
- Networking: Photon (PUN / Fusion packages included under `Assets/Photon`)
- Project scripts (game logic) live in `Assets/Scripts`

## How to run
1. Open this folder in the Unity Editor.
2. Make sure the required Unity packages are installed (see [Packages/packages-lock.json](Packages/packages-lock.json)).
3. Configure Photon AppId / settings (see editor helper: [`PhotonEditor`](Assets/Photon/PhotonUnityNetworking/Code/Editor/PhotonEditor.cs)).
4. Open a demo scene or your scene and press Play in the Editor.

## Main gameplay scripts
- [`GameManager`](Assets/Scripts/GameManager.cs) — central game flow and session management
- [`PlayerSpawner`](Assets/Scripts/PlayerSpawner.cs) — spawns players on the network
- [`PlayerMovement`](Assets/Scripts/PlayerMovement.cs) — player input & movement
- [`ThirdPersonCamera`](Assets/Scripts/ThirdPersonCamera.cs) — follow camera for players
- [`PlayerNickname`](Assets/Scripts/PlayerNickname.cs) — handles player nickname logic
- [`ShowMyNickname`](Assets/Scripts/ShowMyNickname.cs) — UI display of local nickname
- [`PlayerColor`](Assets/Scripts/PlayerColor.cs) — player color selection/sync
- [`Health`](Assets/Scripts/Health.cs) — health and damage
- [`RaycastAttack`](Assets/Scripts/RaycastAttack.cs) — simple attack implementation
- [`FaceToward`](Assets/Scripts/FaceToward.cs) — rotate to face targets
- [`GravityController`](Assets/Scripts/GravityController.cs) — gravity/physics helpers
- [`PhotonChatManager`](Assets/Scripts/PhotonChatManager.cs) — chat integration (Photon Chat)
- [`PlayerSpawner`](Assets/Scripts/PlayerSpawner.cs) — (listed again for emphasis: spawns and manages player objects)
- [`StartSceneMenu`](Assets/Scripts/StartSceneMenu.cs) — start menu UI
- [`ThirdPersonCamera`](Assets/Scripts/ThirdPersonCamera.cs) — camera control
- [`ShowMyNickname`](Assets/Scripts/ShowMyNickname.cs) — local name display

(Each file above is in `Assets/Scripts`; click the links to open the source.)

## Useful project files
- Photon editor & setup helper: [`Assets/Photon/PhotonUnityNetworking/Code/Editor/PhotonEditor.cs`](Assets/Photon/PhotonUnityNetworking/Code/Editor/PhotonEditor.cs)
- IL weaving / Fusion codegen: [`Assets/Photon/Fusion/CodeGen/Fusion.CodeGen.cs`](Assets/Photon/Fusion/CodeGen/Fusion.CodeGen.cs)
- Packages lock: [Packages/packages-lock.json](Packages/packages-lock.json)
- Demo procedural world (noise generator): [`Assets/Photon/PhotonUnityNetworking/Demos/DemoProcedural/Scripts/Noise.cs`](Assets/Photon/PhotonUnityNetworking/Demos/DemoProcedural/Scripts/Noise.cs)

## Notes
- This project includes third-party assets and Photon demo code. Check respective license files in `Assets` (e.g. Photon lib change logs and license files).
- If you plan to build or run multiplayer online, obtain and apply your Photon AppId via the Photon setup UI (see `PhotonEditor`).

## Contributing
- Edit or extend scripts under `Assets/Scripts`.
- Follow existing code style and ensure networked behavior is synchronized correctly.
