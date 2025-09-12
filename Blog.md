# DH2323 Project Blog

## Wednesday 25/5 
### Goal before: 
Each person bring 2-3 more thought out ideas or articles on what the project could entail, for base of the discussion on narrowing down the scope of the project

### Todays accomplishments
Today we started narrowing down what the project could entail. Both of us brought ideas and research on simple games and game designs that we could work from. Where the main thing that we decided on was a game that isn't as violence coded as the tanks from lab 2 and 3, and that we wanted to implement a more intricate pathfinding, such as A*. We also put up requirements for the MVP, goals that are the minimum for us to call it a successful project. 

## Wednesday 15/6 
### Todays accomplishments
The main focus today was setting up the environment and  figuring how Unity Cloud works. This was successful, even though Unity Cloud was a bit more frustrating than we imagined. After a bit of effort to get Unity Cloud to work, we focused on getting the camera focused on the player and imported packages with models for the flowers and bugs that hopefully can be used during this project. The bug package that we found though, are in 2D, which is a problem that we will have to figure out during the project. Either by finding a new package with 3D bugs, which wasn't found this time, or making sure that the bugs, even though technically in 2D, are facing the camera. 

## Monday 7/7
### Goals of today
Frida will focus on spawning flowers in the game.
Anders will focus on the raycast from the spray bottle that is connected to the player. 

### Todays accomplishments
We have today successfully implemented the functionality of spawning flowers randomly in the game. One problem that we will have to solve down the line is implementing so that the flowers can't spawn on top of each other, which is a possibility in the current state. This will probably be solved by implementing a radius of how close the flowers can be to another flower. We also managed to implement the same functionality for bugs, so that they can spawn at random positions. 

We have also managed to implement so that the spray bottle, an artifact connected to the player, can deal damage to the bugs, by using a raycast from the bottle and an enemy layer on the bugs. There is no visual representation of the damage taking place for now, but if you spray a bug enough times it will disappear. 

ANDERS SKRIV OM RAYCASTENS OVERSHOOT HÄR!

We today also fixed the problem where the bugs models are 2D in a 3D environment, by fixing their position to always be rotated against the camera. 

LÄGG IN BILD PÅ 2D --> 3D HÄR!

## Thursday 10/10
### Goals of today
Frida will research visual representation of health bars and start setting up a system for that. If time allows, Frida will also 