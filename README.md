# Project DH2323 Animaiton Track

Project repo for DH2323 Animation track

Project participants: [Frida Andersson](mailto:friande@kth.se) | [Anders Wallenthin](mailto:wallenth@kth.se)

## Concept

Building upon Lab 3 (Animation track) we aimed to create a more complete game experience with; 1. pathfinding, 2. active agents (NPCs having a goal and acting towards it), 3.Communicating states of agents and game objects to user.

## Starting point

Wanting to create something that was both fun to develop and interact with while also being educational for us when working on the project. Having Lab 3 (Animation track) as a base we wanted to change the theme from obvious war associations to something more lighthearted, without necessarily losing the core mechanics of the game. With this we came up with the idea of clearing out bugs from a garden with bug spray. The bugs (NPC agents) should seek out a flower to eat when they spawn, and if they eaten a flower they should seek out a new one.

## Design

### Visual Style

Once again taking inspiration from Lab 3, we choose to keep the semi top-down view. Finding and designing all elements from scratch, we took on a make it work approach meaning that many assets remain as placeholders (eg the player is a capsule and the obstacles are cubes). We found suitable assets for the bugs and flowers from the Unity Asset Store.


## Implementation

### Movement

The player moves around using WASD which is and aims with the mouse, shooting with left click. Aim is placed on a plane to ensure that the interaction with the bugs is intuitive. Movement 

### Mechanics

The flowers and obstacles spawn at the beginning of the game, and are static in the game world, where they are fitted to a navigation grid used for pathfinding. The bugs then spawn during the game at random locations with some restrictions regarding distance to flowers and not inside obstacles. Using the navigation grid and A\* pathfinding the bugs then seek out the nearest flower, eating it upon arrival. If the player is close enough to a bug they can shoot it with the bug spray, destroying it after enough hits.

## Testing

## Deployment

## Documentation
