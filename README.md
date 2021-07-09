#About
A Tool that aims to streamline the process of working with multiple scenes at a single time in a project within unity both in the editor and in-game.

# Purpose
I wanted to build a tool inside unity that would allow me to work with several scenes at one time and unload and load them faster instead of having to manually drag and drop scenes into the inspector from the project. The Game Level Toolkit aims to streamline the process of working with multiple scenes at a single time in a project within unity both in the editor and in-game.

See a Live Demonstration - https://www.youtube.com/watch?v=d2CKQlZtT-s&ab_channel=ThomasMcLaughlin
# Game Levels
Game Levels are scriptable objects within unity that contain a set of scenes which are assigned to it. Game Levels can also contain dependencies to other game levels which means the level requires other levels to be loaded for it to work, the game will automatically handle the loading of these levels and their dependencies. In the Editor the user can load a game level with or without its dependencies. 


# Key Features
**Multi-Scene Editing:**  Load Multiple Game Levels at any one time. When loading a level you can opt to load other levels that the target level is dependent on (or choose not). Choose to keep any open scenes opened while loading another level, or choose to load the level on its own.

**Multi-Scene Creation:**  You can create a Game Level which contains as many scenes as the user desires. The user can also add new scenes to the level in the future. They can remove certain scenes from a level or delete the level as a whole. The user can also duplicate a scene from a level to another level they can also duplicate a level to create a new level. The user can integrate their own scenes created through the traditional unity means into create game levels too, or they can choose to decouple scenes from a game level to act as an independent unity scene.

**Dependencies:**  Game Levels can be assigned dependencies which are a list of other game levels that the target level requires to be able to run correctly. The user can choose to load a scene with these dependencies in the editor or they can exclude them, but when running a level in game the client will automatically handle adding all level dependencies.

**Categories:**  Game Levels can be assigned a category to act as a filter for the user. This is helpful for large teams which may be working with a large number of levels and instead of having to look through an extensive list they can reduce the size of the list by using a category filter..

# Other Notes
UI was put together using Odin Inspector Elements for the most part to speed up the development process. 
