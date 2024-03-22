# Death Blow #

## Summary ##

You have been held prisoner in the dungeons underneath a coliseum. You have grown weak and frail, similar to your cellmate. You have been given an opportunity to escape. “Fight and slay your cellmate and we will give you the freedom you have dreamt of.”

Weave in and out of enemy attacks to gain the advantage in lethal combat. One hit and the fight may be lost.


## Project Resources

[Web-playable Game](https://wakamoli.itch.io/death-blow)  
[Trailor](https://youtu.be/gAFm3i8_IK8)  
[Press Kit](https://github.com/CLion2/ECS179_Project/blob/74fa19849f1579fd80a9ecba850ee66c9398d2dd/PressKit.md)  
[Proposal](https://docs.google.com/document/d/1zCoWcRq2JFVWmfBo8kEtM3IQDmox__Ku/edit?usp=sharing&ouid=107318688404861168600&rtpof=true&sd=true)  

## Gameplay Explanation ##

**In this section, explain how the game should be played. Treat this as a manual within a game. Explaining the button mappings and the most optimal gameplay strategy is encouraged.**


**Add it here if you did work that should be factored into your grade but does not fit easily into the proscribed roles! Please include links to resources and descriptions of game-related material that does not fit into roles here.**

Player movement is done using the typical **WASD** input conventions. The Camera, by default, locks onto the enemy you are currently facing but can be disabled using `Mouse3/MiddleMouseButton` or  `L`. This allows you to freely move the camera however you please

#### Expected Bugs
1. Gladiator may get stuck in the blocking animation if a corner case in the code is hit. 
2. Respawning does not reset the position of the player and enemy due to some restrictions with how position is updated.
3. Blocking animation may get flipped with the regular sword holding animation by accident.


# Main Roles #

Your goal is to relate the work of your role and sub-role in terms of the content of the course. Please look at the role sections below for specific instructions for each role.

Below is a template for you to highlight items of your work. These provide the evidence needed for your work to be evaluated. Try to have at least four such descriptions. They will be assessed on the quality of the underlying system and how they are linked to course content. 

*Short Description* - Long description of your work item that includes how it is relevant to topics discussed in class. [link to evidence in your repository](https://github.com/dr-jam/ECS189L/edit/project-description/ProjectDocumentTemplate.md)

Here is an example:  
*Procedural Terrain* - The game's background consists of procedurally generated terrain produced with Perlin noise. The game can modify this terrain at run-time via a call to its script methods. The intent is to allow the player to modify the terrain. This system is based on the component design pattern and the procedural content generation portions of the course. [The PCG terrain generation script](https://github.com/dr-jam/CameraControlExercise/blob/513b927e87fc686fe627bf7d4ff6ff841cf34e9f/Obscura/Assets/Scripts/TerrainGenerator.cs#L6).

You should replay any **bold text** with your relevant information. Liberally use the template when necessary and appropriate.

## Producer

**Describe the steps you took in your role as producer. Typical items include group scheduling mechanisms, links to meeting notes, descriptions of team logistics problems with their resolution, project organization tools (e.g., timelines, dependency/task tracking, Gantt charts, etc.), and repository management methodology.**

## User Interface - Andrei Bayani

The user interface is split into 2 parts, menus and the HUD. The menus are implemented using a CanvasGroup

**Add an entry for each platform or input style your project supports.**

## Movement/Physics and Input - Soma Matsumoto

**Describe the basics of movement and physics in your game. Is it the standard physics model? What did you change or modify? Did you make your movement scripts that do not use the physics system?**

## Animation and Visuals - Soma Matsumoto & David West

**List your assets, including their sources and licenses.**

For the Enemy:

[Prisoner & Guard](https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/swordsman-170111)

[Gladiator](https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/crusader-tank-101601)

[Basic Motions](https://assetstore.unity.com/packages/3d/animations/basic-motions-free-154271)

[RPG Animations](https://assetstore.unity.com/packages/3d/animations/free-32-rpg-animations-215058)

**Describe how your work intersects with game feel, graphic design, and world-building. Include your visual style guide if one exists.**

## Game Logic - David West

**Document the game states and game data you managed and the design patterns you used to complete your task.**
Game Logic is split into 2 different files, an Enemy Controller which spawns in enemies and an Enemy AI which is the logic for all the enemies.

### Enemy Ai
For the Ai movement and figuring out the Nav Mesh Agent + Nav Mesh Surface used this Vid as a reference: 
[Creating AI](https://www.youtube.com/watch?v=TpQbqRNCgM0)

Enemy Ai has the bulk of the code for everything that the Enemies need to do, but by doing so the file length is really long. It has conditions to check if they are in a cutscene and need to be controlled by the Nav Mesh, there is the [attacks](Examples/EnemyAttack.png) + [on trigger events for the attacks](Examples/EnemyTrigger.png), [a checker for when animations are done](Examples/EnemyAnimDone.png), even a checker for when the [enemy is dead](Examples/EnemyDead.png) so as to not continuously update that dead enemy with more information. 

### Enemy Factory (Spawner)
Like exercise 4, I used something like a Factory to instaniate the Prisoner and Gladiator as clones and then they were moved by the Scene Controller. 

![](Examples/spawning.png)

There also was an update that checked when the Prisoner fight was done so that when needed it can immediatly start spawning in the Boss and have it ready for the Colloseum Scene. 

![](Examples/checkingprisoner.png)

These would then be used in the SceneController to help move the Enemies to spots that Andre wanted them to be before the fights ever started.

### Enemy Colliders
For the Weapons, both the Gladiator and the Prisoner had a box collider on their respective weapon that they had equipped. The created box collider was a bit bigger than the actual texture so that when an attack was made, it would for sure be able to register if it was a hit or not. For the Gladiator, he had a shield that also has a box collider which can then be used to try to block attacks but only for a short period of time. The final colliders were on the bodies as they would be used to figure out if the player attacks landed on the enemies or not.

Used this vid here as a reference for colliders: [vid](https://www.youtube.com/watch?v=TpQbqRNCgM0)

### Enemy Animation System
Unity premade Animations:

[Prisoner & Guard](https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/swordsman-170111)

[Gladiator](https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/crusader-tank-101601)

[Basic Motions](https://assetstore.unity.com/packages/3d/animations/basic-motions-free-154271)

[RPG Animations](https://assetstore.unity.com/packages/3d/animations/free-32-rpg-animations-215058)

These animations were used for the Prisoner, Guard and Gladiator units


# Sub-Roles

## Audio - Andrei Bayani

**List your assets, including their sources and licenses.**

**Describe the implementation of your audio system.**

**Document the sound style.** 
All audio assets are either in the [VO](https://github.com/CLion2/ECS179_Project/tree/77ecf6053977d8a3277aea7aa620d13b9597629c/Project%20179/Assets/VO) folder or the [SFX](https://github.com/CLion2/ECS179_Project/tree/77ecf6053977d8a3277aea7aa620d13b9597629c/Project%20179/Assets/SFX) folder.

The audio controller was taken from Project 4 of ECS 179 and directly imported into

All voice lines were recorded with the help of Professor Josh McCoy and Kyle Mitchell. 

Sound effects were taken from websites like mixkit

- [Mixkit License](https://mixkit.co/license/#sfxFree)
Sword Hit, Sword Block
- [Pixabay License](https://pixabay.com/service/terms/)

Background sounds: fireplace loop, water dripping, Jail door closing
Trailer Music: Hum, Tribal Drums, Dramatic reveal


## Gameplay Testing - David West

**Add a link to the full results of your gameplay tests.**

### Play Tester + Debugger / FineTuning (David)
Became a debugger for the game as it was getting closer to the due date and was able to help fix issues that both Soma and Andrei had when looking over the code. Play tested after every push to find any issues with the current game and then fix issues or improve upon some aspects of the enemy animations. 
![](Examples/FineTuning.png)

### Initial Merger (David)
Created the initial merge here by using the old main as the final product 
![](Examples/merges.png) 

**Summarize the key findings from your gameplay tests.**

## Narrative Design - Andrei Bayani

**Document how the narrative is present in the game via assets, gameplay systems, and gameplay.** 

## Press Kit and Trailer - Andrei Bayani


Press Kit

[Trailer](https://www.youtube.com/watch?v=gAFm3i8_IK8)

The primary goal of the press kit and trailer were to demonstrate the different gameplay features in 

## Game Feel and Polish - Soma Matsumoto

**Document what you added to and how you tweaked your game to improve its game feel.**


# Further Improvements + Fixes #

### Enemies
What would be nice is to have more than one enemy per fight and have the camera switch lock on between enemies. Another thing is to have a more robust movement and attacking system, where the enemy will constantly try to strafe left and right. They would also try to time the blocking to the player attacks so the enemy would not take any damage. In addition maybe adding more scripted fights to the story as there are 2 empty cells for at least 2 more fights in the dungeon portion before the gladiator fight. Included could be more animations for the enemies with a more varied attacks, block breaking and maybe even a jump attack. Then we could also change how fast the animations play based on how much health the enemy has, with lower health enemies attacking faster and doing more damage while blocking less. Then if we want to add more for the visuals, we could add in a trail to show if they are enraged and thus would do more damage to the player.
