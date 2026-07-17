# Black Sheep

A visual novel game with a card game mechanic that unravels the game's narrative. 

## Overview

The player is meant to witness the mind of a nameless protagonist that has their own beliefs, morals and experiences. The player, as they play through the game, should feel as if they're watching through someone else's eyes as well as get glimpses into their inner voice. Progress is driven through dialogue interactions and through interactions within the main mechanic of this game: the card game. The card game is meant to be a remix of Russian Roulette with the addition of cards that the player can choose from on who to shoot. Depending on who the player shoots, the story will diverge into separate branches, letting the player peek more into the nameless person's mentality and despair.

## Built With

- Unity 6000.068f1 (URP)

**Non-Default Packages:**
- DOTween (HOTween v2)
- Custom NUnit
- Performance testing API
- Unity Version Control
- JetBrains Rider Editor
- Mono Cecil
- 2D Aseprite Importer

Do note: Originally, the repo was made using an older version of Unity (6000.0.43f1) but then was changed to using the newer versions as seen through my Git branches swapping between versions

## Project Structure

- `Assets/Scripts/` — all C# scripts, organized by system (Inventory, Crafting, Cluebook, Moon Puzzles, Storytelling, Environment, UI, Player)
- `\Scripts\Systems\Events\StopPlayerInput.cs` — central file containing every event used by the EventBus with comments documenting each event's publishers, subscribers, and purpose
- `Assets/Prefabs` — reusable game objects (UI elements, managers, etc.)
- `Assets/Resources` — Scriptable objects that are meant to presist across scenes and not reset to their default values. They're referenced by multiple scripts during runtime.
- `Assets\Scripts\ScriptableObjects` — dialogue data, card type data, and other shared data assets shared across multiple scripts
- `Assets/Utilities/` — containing the input system mapping for the game

## Architecture

The game is built around an event-driven architecture using a central `EventBus` (`EventBus.cs`). Scripts communicate by publishing and subscribing to events rather than calling each other directly, which keeps systems loosely coupled. Scripts only call each other directly when they're attached to the same parent game object. `IEvent` is within `EventBus.cs`.

- All events are defined as classes implementing `IEvent` inside `StopPlayerInput.cs`
- Each event in `StopPlayerInput.cs` is documented with its publishers, subscribers, and purpose
- Scripts that depend on other scripts document those relationships in their XML `<remarks>` blocks, including `<see cref>` references for quick navigation between related scripts

**Singletons:**
Some manager-style scripts inherit from a generic Singleton<T> base class (`Singleton.cs`), ensuring only one instance of that script exists in a scene and that it's globally accessible via ScriptName.Instance.
`FinishGame.cs`, `EventBus.cs`, `AudioManager.cs`, `Sounds.cs`, `Dealer.cs` all inherit from Singleton<T>
Singleton<T> handles creating an instance automatically if one doesn't exist, destroying duplicate instances, and persisting across scene loads (unless _destroyOnLoad is set to true, as `EventBus.cs` does)

**Input System:**
The player's inputs across the game are managed though `InputController.cs`
`InputController.cs` manages all the input needed for the game in addition to sending events to scripts to respond to certain inputs
`InputController.cs` also manages the disabling/enabling of inputs for the game based on conditions. 
`InputController.cs` is a monobehaviour as it only needs to exist in one scene, which is the `Game` scene (Assets -> Scenes -> GameScenes)
`InputController.cs` is within a folder called Events (Assets -> Scripts -> Events)

## Key Systems (Assets -> Scripts)

**Core** — Contains `EventBus.cs` and `Singleton.cs` 

**Managers** — Contains all the managers responsible for audio, UI canvas changing, animation, and monitoring the game state.

**Card Game** — Contains all the scripts relating to the card game:
`Dealer.cs` determines the randomized choosing of card types whenever cards are given to the player, `GamblingTable.cs` monitors the card game's state on whether to move onto a certain canvas 
and advance to the next round, `SingleCard.cs` is the construction of each player card that'll receive card types and can be movable.

**Dialogue** — Contains all the scripts relating to the dialogue system:
`DialogueBox.cs` controls the progression of dialogue, `DialogueButton.cs` prompts dialogue branching, `FewMessageButton.cs` prompts a set number of independant dialogue to be said, 
`SkipDialogueButton.cs` skips an entire dialogue sequence.

**Events** — Contains `InputController.cs` and the event script, `StopPlayerInput.cs`

**UI** — Contains all the independant UI scripts as well as UI buttons 

## Setup

- Clone the repository
- Open the project in Unity (see `ProjectSettings` for the exact version used)
- Open Assets -> Scenes -> GameScenes -> StartScene
- Select scene named `StartScene` and press play 

Do note: You cannot directly play the scene where all the gameplay takes place in. The actual gameplay scene is called `Game` under GameScenes folder.
This is because there's no AudioManager or game object consisting of all the playable sound effects for `AudioManager.cs` to play from existing in the `Game` scene. 
AudioManager and the game object containing all the playable sound effects are inside `StartScene` and will be persistent across scenes. 
`StartScene` is a scene that ONLY contains the start menu.

## Known Issues

- `StopPlayerInput.cs` (which contains all event definitions) should be renamed to `Events.cs` to better reflect its contents

## Notes

- Anything within Testing folder (Assets -> Scripts -> Testing) is not used in the actual game but were used during development in test scenes 
