using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAI : MonoBehaviour
{
    public struct CharacterStats
    {
        public int hp;
        public int dmg;
        public float hitChance;
        public float critChance;
    }

    public Dictionary<Transform, CharacterStats> playerCharacters;
    public Dictionary<Transform, CharacterStats> enemyCharacters;

    private Transform currentEnemy;
    
    public basicTileGridScript gridScript;

    // Start is called before the first frame update
    void Start()
    {
        gridScript = GetComponent<basicTileGridScript>();
    }

    // Update is called once per frame
    void Update()
    {
        // If player's turn ends and it is AI turn...
        ChooseRandomEnemy();
        FindTargetAndMove();
    }

    // Chooses a random enemy to base closest character...etc
        // Randomized selection of enemy to lower difficulty
    void ChooseRandomEnemy()
    {
        // Choose a random enemy character as the target for this turn
        if (enemyCharacters.Count > 0)
        {
            List<Transform> enemyKeys = new List<Transform>(enemyCharacters.Keys);
            int randomIndex = Random.Range(0, enemyKeys.Count);
            currentEnemy = enemyKeys[randomIndex];
        }
    }

    // 
    void FindTargetAndMove(){
       // Null check  
        if (playerCharacters.Count == 0 || gridScript == null)
            return;

        Tile closestPlayer = GetClosestPlayer();

        // !!!
        if (closestPlayer != null){
            Vector3 targetPosition = closestPlayer.floor.transform.position;

            // Find the closest tile to the target position
            Tile targetTile = GetClosestPlayer();

            if (targetTile != null){
                // Get the path to the target tile
                // List<Tile> path = Pathfinding.FindPath(transform.position, targetTile);

                // StartCoroutine(FollowPath(path));
            }
        }
    }
// Method to convert basicTileGridScript.Tile to enemyAI.Tile
Tile ConvertToEnemyAITile(basicTileGridScript.Tile basicTile)
{
    // Create a new enemyAI.Tile instance using data from basicTile
    Tile enemyAITile = new Tile(basicTile.scale); // Assuming 'scale' is a common property

    // Other properties assignment as needed
    enemyAITile.floor = basicTile.floor;
    // Assign other properties accordingly...

    return enemyAITile;
}

// Now use this method in your GetClosestPlayer method
Tile GetClosestPlayer()
{
    float shortestDistance = Mathf.Infinity;
    int leastTurnsToDefeat = int.MaxValue;
    Tile closestTile = null;

    foreach (basicTileGridScript.Tile basicTile in gridScript.tiles)
    {
        // Convert basicTile to enemyAI.Tile
        Tile tile = ConvertToEnemyAITile(basicTile);

        float distanceToTile = Vector3.Distance(tile.floor.transform.position, currentEnemy.transform.position);

        if (distanceToTile < shortestDistance)
        {
            shortestDistance = distanceToTile;
            closestTile = tile;
        }
        else if (Mathf.Approximately(distanceToTile, shortestDistance))
        {
            // Calculate turns to defeat for equidistant player characters
            int turnsToDefeat = CalculateTurnsToDefeat(tile);
            
            if (turnsToDefeat < leastTurnsToDefeat)
            {
                leastTurnsToDefeat = turnsToDefeat;
                closestTile = tile;
            }
        }
    }

    return closestTile;
}
// Calculates the amount of turns it takes to defeat a player character based on hp and dmg
int CalculateTurnsToDefeat(Tile tile)
{
    int totalTurns = 0;
    int remainingHP = playerCharacters[tile.floor.transform].hp;

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

    //
    System.Collections.IEnumerator FollowPath(List<Tile> path)
    {
        foreach (Tile tile in path){
        Vector3 targetPosition = tile.floor.transform.position;

        // Move towards the next tile in the path
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            // May need to look at this
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime);
            yield return null;
        }

        // Move to the next tile position
        transform.position = targetPosition;

        // Break the loop to move only one tile at a time
        break;
    }
    }

    // Had to import Tile this way... or create an entire differnet script for Tile
    public class Tile
    {
        public GameObject floor;
        public GameObject lightObject;
        public Light light;
        public int scale;

        public Tile(int scale)
        {
            floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            lightObject = new GameObject("light");
            light = lightObject.AddComponent<Light>();
            light.color = Color.red;
            this.scale = scale;
            //set up scale for floor piece
            floor.transform.localScale = new Vector3(scale*10f, scale*0.1f, scale*10f);

        }
        
        public void setLocation(Vector3 newLoc)
        {
            floor.transform.position = newLoc;
            light.transform.position = newLoc + new Vector3(0,1,0);
        }
        public void changeLight(bool on)
        {
            if(on)
                light.intensity = scale*10;
            else
                light.intensity = 0;
        }

    }

}
