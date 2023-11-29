using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    // public float moveSpeed = 5f; 
    // public float attackRange = 1.5f; 
    public basicTileGridScript gridScript;
    // private bool isMoving;
    // public Tile currentTile;
    // Start is called before the first frame update
    void Start()
    {
        gridScript = GetComponent<basicTileGridScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called when the player interacts with a tile 
    // public void OnTileClicked(Tile clickedTile)
    // {
    //     // if (!isMoving)
    //     // {
    //         currentTile = clickedTile;
    //         StartCoroutine(MoveToTile(clickedTile));
    //     // }
    // }

    // IEnumerator MoveToTile(Tile destinationTile)
    // {
    //     // isMoving = true;
    //     Vector3 targetPosition = destinationTile.floor.transform.position;

    //     while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
    //     {
    //         transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime);
    //         yield return null;
    //     }

    //     transform.position = targetPosition;

    //     // Check for enemies in the destination tile
    //     CheckForEnemies(destinationTile);

    //     // isMoving = false;
    // }

    // void CheckForEnemies(Tile tile)
    // {
    //     if (tile.ContainsEnemy())
    //     {
    //         AttackEnemy(tile.GetEnemy());
    //     }
    // }

    void AttackEnemy(GameObject enemy)
    {
        // Attack enemy
        // int remainingHP = enemyCharacters[tile.floor.transform].hp;

        // // Assuming basic attack until enemy's HP becomes zero
        // while (remainingHP > 0){
        //     // Reduce player's remaining HP
        //     remainingHP -= playerCharacters[currentPlayer].dmg;
        // }
    }
}
