# Thomas and his friends

## Context
This is the 2nd module of 42 school Unity Piscine : 7 modules made to learn Unity fundamentals.  
Here we learn more about scripts, 3D Physics, tags, layers.  
A real work of level design was done.  

## Game Overview
This is a 3D platformer where 3 characters (cubes) have to cooperate to reach the end of the level togeteher.  
The player can switch the controlled character using Alpha1, 2 or 3. The camera is centered automatically on the controlled character.  
- Movement: AD or left/right arrows
- Jump: Spacebar (no wall jump)
- Reset level: R or backspace

Characters :  
- Claire : big blue cube, slow, little jump
- John : yellow stick, high speed and jump
- Thomnas : little red cube, medium speed and jump

It a character falls or hit by a deadly object, the level restarts.

To win a level, the 3 characters must be in their dedicated end area. When it is done, we go to next level.
There are 5 stages, each level introducing a new mechanic and increasing difficulty.

## Game mechanics
- Obstacles
- Platforms : horizontal / vertical / diagonale sliding
  - White : all characters can go on it
  - Specific color : only the same color character can go on it, others fall through.
- Teleporters
- Buttons : 2 types
  - Colored button : open same color doors
  - Purple button : change all platforms color with character color
- Turrets : target one character and shoot deadly bullets on him
---

## Technical Details
A scene for each level in `Assets/Scenes/Stage(1->5)`.


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



