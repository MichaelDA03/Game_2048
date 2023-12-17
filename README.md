# 2048
A 2048 game app with C#

# Base logic of the game:

- In the beginning, we place 2 numbers (90% of 2 and 10% of 4) randomly on the grid.
- Everytime we move, the numbers slide in the direction of our movement. If two of the same numbers collide after a move, they merge by and add themselves.
- After every move, a new number spawns on the grid (it has 90% chances of being a 2 and 10% chances of being a 4).
- When the user gets to 2048, he iwins the game.

# Grid:

The game uses a 4 by 4 grid.

# Tiles:

We use tiles that we place in every case of the grid in order to put the numbers.

# Movements

The user uses the direction arrows in order to move ingame.

# Score

The score is calculated by adding the merged tiles score to the base score.
