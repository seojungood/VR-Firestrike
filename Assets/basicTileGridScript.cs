using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicTileGridScript : MonoBehaviour
{

    public int scale;
    public int width;
    public int height;

    public Transform startTransform;

    public Tile[,] tiles;

    // Start is called before the first frame update
    void Start()
    {
        float tileSize = 10f * scale;

        float startW = this.transform.position.x - (width*10f*scale)/2;
        float startH = this.transform.position.z - (height*10f*scale)/2;
        tiles = new Tile[width,height];

        float w = startW + tileSize/2;
        float h = startH + tileSize / 2;

        //TODO -- set locations based on i,j values so that tiles are centered at startTransform
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
        public void changeLight(bool on)
        {
            if(on)
                light.intensity = scale*10;
            else
                light.intensity = 0;
        }

    }
}
