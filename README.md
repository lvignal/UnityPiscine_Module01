# Thomas and his friends

## Context
This is the 2nd module of 42 school Unity Piscine : 7 modules made to learn Unity fundamentals.  
Here we learn more about scripts, 3D Physics, tags, layers.  
Includes a strong focus on level design.  

## Game Overview
This is a 3D platformer where 3 characters (cubes) have to cooperate to reach the end of the level together.  
The player can switch the controlled character (camera centered automatically on it).  

### Controls
- Select the controlled character: Alpha1/2/3
- Movement: AD or left/right arrows
- Jump: Spacebar (no wall jump)
- Reset level: R or backspace

### Characters
- Claire : large blue cube, slow, low jump
- John : tall yellow cube, fast, high jump
- Thomas : small red cube, medium speed and jump

### Objective
To win a level, the 3 characters must be in their dedicated end area. When it is done, we go to the next level.
If a character falls or is hit by a deadly object, the level restarts.
There are 5 stages, each level introducing a new mechanic and increasing difficulty.

## Game Mechanics
- Obstacles
- Platforms : horizontal / vertical / diagonal movement
  - White : all characters can go on it
  - Specific color : only the same color character can go on it, others fall through.
- Teleporters
- Buttons : 2 types
  - Colored button : open same color doors
  - Purple button : change all platforms color with character color
- Turrets : target one character and shoot deadly bullets at him
- Traps 
---

## Technical Details
### Architecture
- A scene for each level : `Assets/Scenes/Stage(1->5)`.
- `GameManager` singleton : manages levels
- `LevelManager` in each scene : centralizes inputs, controls camera, check and trigger level end
- `PlayerController`:  shared script with character-specific parameters (speed, jump force). Important work on physics (ground detection, slide down walls, jump fall)
- One script by mechanic (`ButtonView, PlatformController, TeleportationEnter, TurretController`)
- `ButtonsManager`: handles connections between buttons and walls and platforms
- Organized scene tree with containers : Decor / Platforms / Teleporters...

### Unity Features Used
- Tags & Layers (character-specific interactions)
- Physics system (collisions, triggers, raycast, gravity)
- Scene management (multiple levels)
- Prefabs (reusable objects: platforms, turrets, buttons, traps)

### Level Design
- Progressive difficulty across 5 stages
- Introduction of new mechanics per level
- Use of color-based mechanics to enforce cooperation


## Preview

### Stage 1  
<img width="1267" height="229" alt="image" src="https://github.com/user-attachments/assets/8faa5c56-a14d-43ec-a2d8-817eab590e2a" />

### Stage 2  
<img width="1456" height="267" alt="image" src="https://github.com/user-attachments/assets/86c6d9d5-3f4f-47fb-bb43-d656a500b2d3" />

### Stage 3  
<img width="1486" height="250" alt="image" src="https://github.com/user-attachments/assets/ebd79a8d-e983-4136-b6bf-1c91c6dcc1b6" />

### Stage 4  
<img width="1594" height="211" alt="image" src="https://github.com/user-attachments/assets/1b551848-07a4-4202-aad3-4a9a9f2ab501" />

### Stage 5  
<img width="1598" height="190" alt="image" src="https://github.com/user-attachments/assets/11d1c260-382d-4aca-9847-1bd337c19ed6" />



