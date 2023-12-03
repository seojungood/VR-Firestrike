using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameLogic : MonoBehaviour
{
    public struct CharacterStats{
        public int x;
        public int y;
        public int hp;
        public int dmg;
        public float hitChance;
        public float critChance;
        // Method to get default CharacterStats 
        public static CharacterStats GetDefault()
        {
            return new CharacterStats
            {
                x = 0,
                y = 0,
                hp = 20, // default hp value
                dmg = 5, // default damage value
                hitChance = 70, // default hit chance - 70% for easier calc
                critChance = 30 // default crit chance - 30%
            };
        }
    }

    public Dictionary<Transform, CharacterStats> playerCharacters;
    public Dictionary<Transform, CharacterStats> enemyCharacters;

    // Enemy chosen for current enemy's turn that will make an AI move
    public Transform currentEnemy;
    public Transform currentPlayer;

    // Location [x, y]
    public int[] currentEnemyLoc;
    public int[] currentPlayerLoc;

    
    public int[,] grid = new int[5, 5];

    // True when it is player's turn
    bool playerturn;

    // indicates when player/enemy's turn is over
    bool turnOver = false;

    // Start is called before the first frame update
    void Start(){
        // Initialize Grid with characters
        InitializeGrid();

        // Can start move
        turnOver = true;

        // Player always starts
        playerturn = true;

    }

    // Update is called once per frame
    void Update()
    {
        if(turnOver){
            if(playerturn){
                // player turn moves 

                // Use Keyboard arrow keys for now
                // Move up
                if (Input.GetKeyDown(KeyCode.UpArrow) && currentPlayerLoc[1] < 4) {
                    if(currentPlayerLoc[0] == currentEnemyLoc[0] && currentPlayerLoc[1] + 1 == currentEnemyLoc[1]){
                        Attack(currentPlayerLoc, currentEnemyLoc, true);
                    } else{
                        currentPlayerLoc[1]++;
                        Debug.Log("Moved Up");
                    } 
                } // Move down
                else if (Input.GetKeyDown(KeyCode.DownArrow) && currentPlayerLoc[1] > 0) {
                    if (currentPlayerLoc[0] == currentEnemyLoc[0] && currentPlayerLoc[1] - 1 == currentEnemyLoc[1]){
                        Attack(currentPlayerLoc, currentEnemyLoc, true);
                    }
                    else{
                        currentPlayerLoc[1]--;
                        Debug.Log("Moved Down");
                    }
                } // Move left
                else if (Input.GetKeyDown(KeyCode.LeftArrow) && currentPlayerLoc[0] > 0){
                    if (currentPlayerLoc[0] - 1 == currentEnemyLoc[0] && currentPlayerLoc[1] == currentEnemyLoc[1]){
                        Attack(currentPlayerLoc, currentEnemyLoc, true);
                    }
                    else{
                        currentPlayerLoc[0]--;
                        Debug.Log("Moved Left");
                    }
                } // Move right
                else if (Input.GetKeyDown(KeyCode.RightArrow) && currentPlayerLoc[0] < 4) {
                    if (currentPlayerLoc[0] + 1 == currentEnemyLoc[0] && currentPlayerLoc[1] == currentEnemyLoc[1]){
                        Attack(currentPlayerLoc, currentEnemyLoc, true);
                    }
                    else{
                        currentPlayerLoc[0]++;
                        Debug.Log("Moved Right");
                    }
                }

                // Send grid

                turnOver = true;
            }else{
                turnOver = false;
                // If player's turn ends and it is AI turn...
                ChooseRandomEnemy();
                FindTargetAndMove();
            }
        }
    }

    // Intialize grid at start
    void InitializeGrid(){
        // Initialize grid with 0 (empty) values
        for (int i = 0; i < 5; i++){
            for (int j = 0; j < 5; j++){
                grid[i, j] = 0;
            }
        }

        // Place players and enemies in specific positions
        grid[0, 2] = 1; // Player at position (0,0)
        grid[4, 3] = 2; // Enemy at position (4,3)

        currentPlayerLoc = new int[] {0,0};
        currentEnemyLoc = new int[] {4,3};

        // playerCharacters.Add()
        // enemyCharacters.Add()

    }

    // Chooses a random enemy to base closest character...etc
        // Randomized selection of enemy to lower difficulty
    void ChooseRandomEnemy()
    {
        // Choose a random enemy character as the target for this turn
        if (enemyCharacters.Count > 0){
            List<Transform> enemyKeys = new List<Transform>(enemyCharacters.Keys); 
            currentEnemy = enemyKeys[UnityEngine.Random.Range(0, enemyKeys.Count)];
        }
    }

    // Enemy finds its closest target and moves to them
    void FindTargetAndMove(){
       // Null check  
        if (playerCharacters.Count == 0)
            return;

        // Find the closest location of closest player
        int[] targetLoc = GetClosestPlayer();
        
        // Check if the player is adjacent to the target and attack if true
        if ((Mathf.Abs(targetLoc[0] - currentEnemyLoc[0]) == 1 && targetLoc[1] == currentEnemyLoc[1]) ||
            (Mathf.Abs(targetLoc[1] - currentEnemyLoc[1]) == 1 && targetLoc[0] == currentEnemyLoc[0])){

            Attack(currentEnemyLoc, targetLoc, false);
        } // If player is not adjacent to enemy, make a move 
        else if (targetLoc[0] != -1 && targetLoc[1] != -1){
            // Get the path to the target tile and move
            if(targetLoc[0] < currentEnemyLoc[0]){
                // move left
                currentEnemyLoc[0]--;
            } else if(targetLoc[0] > currentEnemyLoc[0]){
                // move right
                currentEnemyLoc[0]++;
            }else if(targetLoc[1] < currentEnemyLoc[1]){
                // move up
                currentEnemyLoc[1]--;
            }else{
                // move down
                currentEnemyLoc[1]++;
            }

            // Check if the player is adjacent to the target and attack if true
            if ((Mathf.Abs(targetLoc[0] - currentEnemyLoc[0]) == 1 && targetLoc[1] == currentEnemyLoc[1]) ||
                (Mathf.Abs(targetLoc[1] - currentEnemyLoc[1]) == 1 && targetLoc[0] == currentEnemyLoc[0])){
                Attack(currentEnemyLoc, targetLoc, false);
            }
        
        }

        //!!!! SEND THIS INFO TO VIEW...

        // Enemy's turn is over
        turnOver = true;
    }

// Gets closest player to currentEnemy, returns coordinates with int[], [0] = x, [1] = y
int[] GetClosestPlayer(){
    float shortestDistance = Mathf.Infinity;
    int[] closestPosition = new int[]{-1, -1}; // default null

    for (int x = 0; x < 5; x++){
        for (int y = 0; y < 5; y++){
            if (grid[x, y] == 1){ // Check if the current cell has a player
            
                float distanceToPlayer = Mathf.Sqrt(Mathf.Pow(x - currentEnemyLoc[0], 2) + Mathf.Pow(y - currentEnemyLoc[1], 2));
                
                if (distanceToPlayer < shortestDistance){
                    shortestDistance = distanceToPlayer;
                    closestPosition[0] = x;
                    closestPosition[1] = y;
                }
            }
        }
    }
    return closestPosition;
}

// Calculates the amount of turns it takes to defeat a player character based on hp and dmg
int CalculateTurnsToDefeat(int[] playerLoc){
    int totalTurns = 0;

    // Retrieve playerStates from the grid using location
    CharacterStats playerStats = default(CharacterStats);
    foreach (var character in playerCharacters.Values){
        if (character.x == playerLoc[0] && character.y == playerLoc[1]){
            playerStats = character;
        }
    }

    int remainingHP = playerStats.hp;

    // Assuming basic attack until enemy's HP becomes zero
    while (remainingHP > 0){
        // Reduce player's remaining HP
        remainingHP -= enemyCharacters[currentEnemy].dmg;

        if (remainingHP > 0){
            // Player still alive, increment turns
            totalTurns++;
        }
    }
    return totalTurns;
}
    
    // Method that attacks
    void Attack(int[] attackerLoc, int[] targetLoc, bool attackByPlayer){
        CharacterStats playerStats = default(CharacterStats);
        CharacterStats enemyStats = default(CharacterStats);

        System.Random r  = new System.Random();

        if(attackByPlayer){
            foreach (var characterStats in playerCharacters.Values){
                if (characterStats.x == attackerLoc[0] && characterStats.y == attackerLoc[1]){
                    playerStats = characterStats;
                }
            }   
            foreach (var characterStats in enemyCharacters.Values){
                if (characterStats.x == targetLoc[0] && characterStats.y == targetLoc[1]){
                    enemyStats = characterStats;
                }
            }   
            
            // If hit
            if(r.Next(0, 100) <= playerStats.hitChance){
                // if Crit
                if(r.Next(0, 100) <= playerStats.critChance){
                    enemyStats.hp -= playerStats.dmg * 2;
                } // didn't crit
                else{
                    enemyStats.hp -= playerStats.dmg;
                }
            }

            if (enemyStats.hp <= 0) {
            // Enemy is defeated, remove them from dictionary
                enemyCharacters.Remove(currentEnemy);
                grid[targetLoc[0],targetLoc[1]] = 0;
             }

        } else{
            foreach (var characterStats in playerCharacters.Values){
                if (characterStats.x == targetLoc[0] && characterStats.y == targetLoc[1]){
                    playerStats = characterStats;
                }
            }   
            foreach (var characterStats in enemyCharacters.Values){
                if (characterStats.x == attackerLoc[0] && characterStats.y == attackerLoc[1]){
                    enemyStats = characterStats;
                }
            }   

            // If hit
            if(r.Next(0, 100) <= enemyStats.hitChance){
                // if Crit
                if(r.Next(0, 100) <= enemyStats.critChance){
                    playerStats.hp -= enemyStats.dmg * 2;
                } // didn't crit
                else{
                    playerStats.hp -= enemyStats.dmg;
                }
            }

            if(playerStats.hp <= 0){
                playerCharacters.Remove(currentPlayer);
                grid[targetLoc[0],targetLoc[1]] = 0;
            }
        }

        
    }


    // method to send everything to grid

}
