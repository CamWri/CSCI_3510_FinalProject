# Depth Trigger

**Team Members:**  
- Camden Wright  
- Koi McManis  
- Kyron Barrow  
- Matthew Graves  

---

## Objective  

**Winning Conditions:**  
Depth Trigger is a survival-focused game inspired by horde shooters and roguelites like Call Of Duty Zombies. Players aim to:  
- Survive as many rounds as possible  
- Progress to deeper floors of the dungeon  
- Defeat more zombies than in previous runs to achieve a higher score  

**Losing Conditions:**  
- The player dies when health reaches 0  
- There are no other failure conditions; gameplay is entirely focused on survival and progression  

---

## Controls  

| Action         | Key/Button         |  
|----------------|--------------------|  
| Move           | WASD               |  
| Jump           | Space              |  
| Fire Weapon    | Left Mouse Click   |  
| Crouch         | C                  |  
| Sprint         | Left Shift         |  
| Interact       | F (doors, weapons) |  

---

## Installation / Run Notes  

1. Open the project in Unity 6000.2.7f2
2. Start the game by opening the MainMenu scene and clicking the “Play Main” button  
3. Alternatively, for testing, you can open the ProceduralGeneration scene:  
   - To switch weapons, select the PCController GameObject in the hierarchy, go to the Gun Swap MonoBehaviour script, and set the current gun type  
   - To test weapon rarity, expand CameraHolder → MainCamera, select a coresponding weapon GameObject (SMG, Pistol, etc.), and adjust its rarity in the Gun MonoBehaviour script  
   - Example: To start with a Legendary SMG, set the SMG weapon’s rarity to Legendary and swap to the SMG type through the PCController  


