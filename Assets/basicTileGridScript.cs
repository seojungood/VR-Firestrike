using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class basicTileGridScript : MonoBehaviour
{

    public int scale;
    public int width;
    public int height;

    public Transform startTransform;

    public Tile[,] tiles;

    int[,] testingData;
    int testingx = 0;
    int testingy = 0;

    // Start is called before the first frame update
    void Start()
    {
        testingData =  new int[5, 5];
        
        float tileSize = 10f * scale;

        float startW = this.transform.position.x - (width*10f*scale)/2;
        float startH = this.transform.position.z - (height*10f*scale)/2;
        tiles = new Tile[width,height];

        float w = startW + tileSize/2;
        float h = startH + tileSize / 2;


        for(int i = 0; i < width; i++)
        {
            w += tileSize;
            h = startH + tileSize / 2;
            for (int j = 0; j < height; j++)
            {
                tiles[i,j] = new Tile(scale);
                tiles[i, j].setLocation(new Vector3(w, this.transform.position.y, h));
                h += tileSize;
            }
            //startW = (width*10f*scale)/2;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //FOR TESTING PURPOSES
        /*
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            showPlayerMoves(2, 3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            showPlayerMoves(1,1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            showPlayerMoves(3,4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            showPlayerMoves(0,0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            turnOffAllLights();
        }

        */

        //more testing

        /*
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            testingData[0, 0] = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            testingData[testingx, testingy] = 0;
            if (testingx >= 4)
            {
                testingx = 0;
                testingy += 1;
            }
            else
            {
                testingx += 1;
            }
            testingData[testingx, testingy] = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            testingData[testingx, testingy] = 0;
            if (testingx >= 4)
            {
                testingx = 0;
                testingy += 1;
            }
            else
            {
                testingx += 1;
            }
            testingData[testingx, testingy] = 2;
        }
        getData(testingData);
        */
    }

    void getData(int[,] boardData)
    {
        turnOffAllLights();
        for(int i = 0; i < boardData.GetLength(0); i++)
        {
            for(int j = 0; j < boardData.GetLength(1); j++)
            {
                showMoves(i, j, boardData[i, j]);
            }
        }
    }

    void turnOffAllLights()
    {
        foreach (Tile t in tiles)
        {
            t.changeLight(false);
        }
    }

    void showMoves(int row, int col, int type)
    {
        if (type == 0)
            return;
        for (int i = row - 1; i <= row + 1; i++)
        {
            if (!(i >= 0) || !(i < width))
                continue;
            for (int j = col - 1; j <= col + 1; j++)
            {
                if (!(j >= 0) || !(j < height))
                    continue;
                //if type == 1, player space, otherwise enemy space
                tiles[i, j].playerLightColor(type == 1);
                tiles[i, j].changeLight(true);
            }
        }
    }

    void showPlayerMoves(int row, int col)
    {
        for(int i = row-1; i <= row+1; i++)
        {
            if (!(i >= 0) || !(i < width))
                continue;
            for (int j = col - 1; j <= col + 1; j++)
            {
                if (!(j >= 0) || !(j < height))
                    continue;
                tiles[i, j].changeLight(true);
            }
        }
    }

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

        //1 is player character, and 2 is enemy charater
        public void playerLightColor(bool isPlayer)
        {
            if(isPlayer)
            {
                light.color = Color.green;
            }
            else
            {
                light.color = Color.red;
            }

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
