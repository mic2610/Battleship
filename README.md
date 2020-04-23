Battleship

=======================================
Battleship has been designed and developed as a single player game, using .NET Core 3.1 as a Web API to effectively accomodate the following functionality:

# Create a board
# Add a battleship to the board
# Take an “attack” at a given position, and report back whether the attack resulted in a hit or a miss.

# Technical features:
* Unit tests against the above units of work are in the "~/Tests/" solution folder
* The player board and opponent board have their state tracked through the use of server caching using "IMemoryCache" dependency injected into the API, player board and opponent board are cached for 10 minutes
* Various assumptions and their conditions have been included within the solution, but the whole game has not been created due to time constraints, such as a battleship cannot be added to an existing battleship
* Additional "reset" has been added as a POST request to "~/battleship/reset"


# Create a board:
* GET Request to "~/battleship"

Creates a player and opponent board and caches them for 10 minutes, opponent board has battleships populated for it through code. Player id and opponent id are also generated through code.

JSON boday response:
```
{
    "playerBoard": [
        "- - - - - - - - - - ",
        "- - - - - - - - - - ",
        "- - - - - - - - - - ",
        "- - - - - - - - - - ",
        "- - - - - - - - - - ",
        "- - - - - - - - - - ",
        "- - - - - - - - - - ",
        "- - - - - - - - - - ",
        "- - - - - - - - - - ",
        "- - - - - - - - - - "
    ],
    "opponentBoard": [
        "B B B - - - - - - - ",
        "- - - - - - - - - - ",
        "- - B - - - - - - - ",
        "- - B - - - - - - - ",
        "- - B - - - - - - - ",
        "- - B - - - - - - - ",
        "- - - - - - - - - - ",
        "- - - - - - - - - - ",
        "- - B B B B B - - - ",
        "- - - - - - - - - - "
    ],
    "results": [
        "Battleship created starting at position of row: 1, column: 1",
        "Battleship created starting at position of row: 9, column: 3",
        "Battleship created starting at position of row: 3, column: 3"
    ],
    "playerId": 1,
    "opponentId": 2
}
```

# Add a battleship to the board:
* POST Request to "~/battleship/add"
* Modifies and adds a battleship to the single player's board, works from left to right for horizontal battleships and top to bottom for vertical battleships
* Can only be a 10x10 board
* "row" and "column" are index + 1 to make it easier for consuming code to add battleships
* "alignment" can either be "Horizontal" or "Vertical"
* "playerId" must always be posted through as 1 and "opponentId" must always be posted through as 2 to get the boards from cache

POST "~/battleship/add" Request JSON:
```
{
  "row": 1,
  "column": 1,
  "shipSize": 5,
  "alignment": "Horizontal",
  "playerId": 1,
  "opponentId": 2
}
```

POST ~/battleship/add Response JSON:
```
{
    "playerBoard": [
        "B B B B B - - - - - ",
        "- - - - - - - - - - ",
        "- - - - - - - - - - ",
        "- - - - - - - - - - ",
        "- - - - - - - - - - ",
        "- - - - - - - - - - ",
        "- - - - - - - - - - ",
        "- - - - - - - - - - ",
        "- - - - - - - - - - ",
        "- - - - - - - - - - "
    ],
    "opponentBoard": [
        "B B B - - - - - - - ",
        "- - - - - - - - - - ",
        "- - B - - - - - - - ",
        "- - B - - - - - - - ",
        "- - B - - - - - - - ",
        "- - B - - - - - - - ",
        "- - - - - - - - - - ",
        "- - - - - - - - - - ",
        "- - B B B B B - - - ",
        "- - - - - - - - - - "
    ],
    "results": [
        "Battleship created starting at position of row: 1, column: 1"
    ],
    "playerId": 1,
    "opponentId": 2
}
```

# Take an “attack” at a given position, and report back whether the attack resulted in a hit or a miss.
* POST Request to "~/battleship/attackOpponent"
* Attacks the opponent's board
* "row" and "column" are index + 1 to make it easier for consuming code to attack battleships
* Returns a message indicating whether there has been a hit or miss and even if a battleship has been destroyed on the opponent's board, these are returned in the "results" array but also marked on the board
* "playerId" must always be posted through as 1 and "opponentId" must always be posted through as 2 to get the boards from cache

POST "~/battleship/attackOpponent" Request JSON:
```
{
  "row": 1,
  "column": 3,
  "playerId": 1,
  "opponentId": 2
}
```

POST ~/battleship/attackOpponent Response JSON:
```
{
    "playerBoard": [
        "B B B B B - - - - - ",
        "- - - - - - - - - - ",
        "- - - - - - - - - - ",
        "- - - - - - - - - - ",
        "- - - - - - - - - - ",
        "- - - - - - - - - - ",
        "- - - - - - - - - - ",
        "- - - - - - - - - - ",
        "- - - - - - - - - - ",
        "- - - - - - - - - - "
    ],
    "opponentBoard": [
        "H H H - - - - - - - ",
        "- - - - - - - - - - ",
        "- - B - - - - - - - ",
        "- - B - - - - - - - ",
        "- - B - - - - - - - ",
        "- - B - - - - - - - ",
        "- - - - - - - - - - ",
        "- - - - - - - - - - ",
        "- - B B B B B - - - ",
        "- - - - - - - - - - "
    ],
    "results": [
        "You have destroyed a battleship starting at position of row: 1, column: 3"
    ],
    "playerId": "9d75107e-759f-4181-951a-ea13ab0f01e0",
    "opponentId": "c68c5818-7e44-478a-a6a2-d4b19c76c5db"
}
```

# Reset
* POST Request to "~/battleship/reset"
* Removes the boards from cache

POST "~/battleship/reset" Request JSON:
```
{
  "playerId": 1,
  "opponentId": 2
}
```

POST "~/battleship/reset" Respone JSON:
```
{
  "Player board: 1 and opponent board: 2 have been reset"
}
```