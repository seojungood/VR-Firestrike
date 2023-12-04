using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class basicTileGridWithLogic : MonoBehaviour
{

    public int scale;
    public int width;
    public int height;

    public Transform startTransform;

    public Tile[,] tiles;

    int[,] gameData;


    public Character p1;
    public Character p2;

    public Character e1;
    public Character e2;


    public GameObject player1;
    public GameObject player2;

    public GameObject enemy1;

    public GameObject enemy2;


    public int baseHealth;

    public int baseAttack;

    public TextMeshProUGUI scoreboard;

    int x = 0;
    int y = 0;


    bool playerTurn = true;
    int playerCharacter = 0;
    //w a s d
    char direction = ' ';


    bool isAttack = false;

    //public Random r;// = new Random();

    // Start is called before the first frame update
    void Start()
    {
        //r = new Random();

        p1 = new Character(baseHealth, player1);
        p2 = new Character(baseHealth, player2);

        e1 = new Character(baseHealth, enemy1);
        e2 = new Character(baseHealth, enemy2);

        scoreboard.text = "Game In Progress...";
        scoreboard.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        gameData = new int[width, height];


        float tileSize = 10f * scale;

        float startW = this.transform.position.x - (width * 10f * scale) / 2;
        float startH = this.transform.position.z - (height * 10f * scale) / 2;
        tiles = new Tile[width, height];

        float w = startW + tileSize / 2;
        float h = startH + tileSize / 2;


        for (int i = 0; i < width; i++)
        {
            w += tileSize;
            h = startH + tileSize / 2;
            for (int j = 0; j < height; j++)
            {
                tiles[i, j] = new Tile(scale);
                tiles[i, j].setLocation(new Vector3(w, this.transform.position.y, h));
                h += tileSize;
            }
            //startW = (width*10f*scale)/2;
        }

        turnOffAllLights();
        initCharacterLoc();
    }

    void initCharacterLoc()
    {
        setCharacterLoc(p1, tiles[0, 0]);
        setCharacterLoc(p2, tiles[0, height - 1]);
        p1.x = 0;
        p1.y = 0;

        p2.x = 0;
        p2.y = height - 1;

        gameData[0, 0] = 1;
        gameData[0, height - 1] = 1;

        setCharacterLoc(e1, tiles[width - 1, 0]);
        setCharacterLoc(e2, tiles[width - 1, height - 1]);


        e1.x = width - 1;
        e1.y = 0;

        e2.x = width - 1;
        e2.y = height - 1;

        gameData[width - 1, 0] = 2;
        gameData[width - 1, height - 1] = 2;

        player1.transform.Rotate(0, 90, 0);
        player2.transform.Rotate(0, 90, 0);

        enemy1.transform.Rotate(0, -90, 0);
        enemy2.transform.Rotate(0, -90, 0);
    }

    void setCharacterLoc(Character g, Tile t)
    {
        g.movement.Walk(new Vector3(t.getX(), g.o.transform.position.y, t.getZ()));
        // g.transform.position = new Vector3(t.getX(), g.transform.position.y, t.getZ());
    }



    // Update is called once per frame
    void Update()
    {
        if (playerTurn)
        {
            playerStuff();
        }
        else
        {
            Character e = getEnemy();
            if (e != null)
            {
                Character p = getClosestPlayer(e);
                if (p == null)
                    return;

                moveEnemyToPlayer(e, p);

                if (p1.hp <= 0 && p2.hp <= 0)
                {
                    scoreboard.text = "You Lost!";
                    scoreboard.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                }
                else
                    playerTurn = true;
            }
        }
    }

    public void moveEnemyToPlayer(Character e, Character p)
    {
        if (e.x > p.x)
        {
            moveEnemy(e, e.x - 1, e.y);
        }
        else if (e.x < p.x)
        {
            moveEnemy(e, e.x + 1, e.y);

        }
        else if (e.y > p.y)
        {
            moveEnemy(e, e.x, e.y - 1);

        }
        else
        {
            moveEnemy(e, e.x, e.y + 1);

        }
    }

    public void moveEnemy(Character e, int x, int y)
    {
        if (!attackPlayer(x, y))
        {
            setCharacterLoc(e, tiles[x, y]);
            e.x = x;
            e.y = y;
            //Debug.Log("move to " + x + " " + y);
        }
        //Debug.Log("attackkkkkkk");

    }

    //attacks player and returns true if possible, false otherwise
    public bool attackPlayer(int x, int y)
    {
        if (p1.x == x && p1.y == y && p1.hp > 0)
        {
            Attack(x, y);
            return true;
        }
        else if (p2.x == x && p2.y == y && p2.hp > 0)
        {
            Attack(x, y);
            return true;
        }
        return false;
    }



    public Character getClosestPlayer(Character e)
    {
        if (p1.hp > 0 && p2.hp > 0)
        {
            float d = Vector3.Distance(e.o.transform.position, player1.transform.position);
            if (d < Vector3.Distance(e.o.transform.position, player2.transform.position))
                return p1;
            return p2;
        }
        else if (p1.hp > 0)
            return p1;
        else if (p2.hp > 0)
            return p2;
        else
        {
            scoreboard.text = "You Lost!";
            scoreboard.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
        return null;

    }


    public Character getEnemy()
    {
        if (e1.hp > 0 && e2.hp > 0)
        {
            int r = Random.Range(0, 1);
            if (r == 0)
                return e1;
            else
                return e2;
        }
        if (e1.hp > 0)
            return e1;
        if (e2.hp > 0)
            return e2;
        scoreboard.text = "You Won!";
        scoreboard.color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
        return null;

    }


    public void playerStuff()
    {
        getPlayerInfo();


        if (playerCharacter == 1)
        {
            tiles[p1.x, p1.y].changeColor(Color.magenta);
            tiles[p1.x, p1.y].changeLight(true);
            tiles[p2.x, p2.y].changeLight(false);
            x = p1.x;
            y = p1.y;
        }
        else if (playerCharacter == 2)
        {
            tiles[p2.x, p2.y].changeColor(Color.magenta);
            tiles[p1.x, p1.y].changeLight(false);
            tiles[p2.x, p2.y].changeLight(true);
            x = p2.x;
            y = p2.y;
        }

        lights();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (playerCharacter != 1 && playerCharacter != 2)
                return;
            if (direction == ' ')
                return;

            if (direction == 'w')
            {
                if (playerCharacter == 1)
                {
                    move(p1, x + 1, y);

                }
                else
                {
                    move(p2, x + 1, y);
                }


            }
            else if (direction == 'a')
            {
                if (playerCharacter == 1)
                {
                    move(p1, x, y + 1);

                }
                else
                {
                    move(p2, x, y + 1);
                }
            }
            else if (direction == 's')
            {
                if (playerCharacter == 1)
                {
                    move(p1, x - 1, y);

                }
                else
                {
                    move(p2, x - 1, y);
                }
            }
            else if (direction == 'd')
            {
                if (playerCharacter == 1)
                {
                    move(p1, x, y - 1);

                }
                else
                {
                    move(p2, x, y - 1);
                }
            }
            turnOffAllLights();

            playerCharacter = 0;
            direction = ' ';

            playerTurn = false;

            if (e1.hp <= 0 && e2.hp <= 0)
            {
                scoreboard.text = "You Won!";
                scoreboard.color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
            }

        }
    }

    public void move(Character o, int x, int y)
    {
        if (!isEnemyHere(x, y))
        {
            setCharacterLoc(o, tiles[x, y]);
            if (playerCharacter == 1)
            {
                p1.x = x;
                p1.y = y;
            }
            else
            {
                p2.x = x;
                p2.y = y;
            }
        }
        else
        {
            Attack(x, y);
        }
    }


    public void lights()
    {
        if (direction == 'w')
        {
            if (x + 1 <= width - 1)
            {
                tiles[x + 1, y].changeColor(Color.green);
                tiles[x + 1, y].changeLight(true);
            }

        }
        else if (direction == 'a')
        {
            if (y + 1 <= height - 1)
            {
                tiles[x, y + 1].changeColor(Color.green);
                tiles[x, y + 1].changeLight(true);
            }
        }
        else if (direction == 's')
        {
            if (x - 1 >= 0)
            {
                tiles[x - 1, y].changeColor(Color.green);
                tiles[x - 1, y].changeLight(true);
            }
        }
        else if (direction == 'd')
        {
            if (y - 1 >= 0)
            {
                tiles[x, y - 1].changeColor(Color.green);
                tiles[x, y - 1].changeLight(true);


            }
        }
    }

    public bool isEnemyHere(int x, int y)
    {
        if (e1.x == x && e1.y == y)
            return true;
        else if (e2.x == x && e2.y == y)
            return true;
        return false;
    }


    public void getPlayerInfo()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            turnOffAllLights();
            playerCharacter = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            turnOffAllLights();

            playerCharacter = 2;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            turnOffAllLights();
            direction = 'w';
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            turnOffAllLights();
            direction = 's';

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            turnOffAllLights();
            direction = 'a';

        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            turnOffAllLights();
            direction = 'd';

        }
    }


    public void Attack(int x, int y)
    {
        if (p1.x == x && p1.y == y)
        {
            p1.hp = p1.hp - baseAttack;
            if (p1.hp <= 0)
            {
                player1.SetActive(false);
            }
        }
        else if (p2.x == x && p2.y == y)
        {
            p2.hp = p2.hp - baseAttack;
            if (p2.hp <= 0)
            {
                player2.SetActive(false);
            }
        }
        else if (e1.x == x && e1.y == y)
        {
            e1.hp = e1.hp - baseAttack;
            if (e1.hp <= 0)
            {
                enemy1.SetActive(false);
            }
        }
        else if (e2.x == x && e2.y == y)
        {
            e2.hp = e2.hp - baseAttack;
            if (e2.hp <= 0)
            {
                enemy2.SetActive(false);
            }
        }
    }

    public void getData(int[,] boardData)
    {
        turnOffAllLights();
        for (int i = 0; i < boardData.GetLength(0); i++)
        {
            for (int j = 0; j < boardData.GetLength(1); j++)
            {
                showMoves(i, j, boardData[i, j]);
            }
        }
    }

    public void turnOffAllLights()
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
        for (int i = row - 1; i <= row + 1; i++)
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


    public class Character
    {
        public int x;
        public int y;
        public int hp;
        public GameObject o;

        public MovementScript movement;

        public Character()
        {
            x = y = 0;
            hp = 0;
        }

        public Character(int bHP, GameObject startO)
        {
            x = y = 0;
            o = startO;
            hp = bHP;

            movement = startO.GetComponent<MovementScript>();
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
            floor.transform.localScale = new Vector3(scale * 10f, scale * 0.1f, scale * 10f);

        }

        public void setLocation(Vector3 newLoc)
        {
            floor.transform.position = newLoc;
            light.transform.position = newLoc + new Vector3(0, 1, 0);
        }

        public float getX()
        {
            return floor.transform.position.x;
        }

        public float getZ()
        {
            return floor.transform.position.z;
        }

        //1 is player character, and 2 is enemy charater
        public void playerLightColor(bool isPlayer)
        {
            if (isPlayer)
            {
                light.color = Color.green;
            }
            else
            {
                light.color = Color.red;
            }

        }

        public void changeColor(Color c)
        {
            light.color = c;
        }

        public void changeLight(bool on)
        {
            if (on)
                light.intensity = scale * 10;
            else
                light.intensity = 0;
        }

    }
}
