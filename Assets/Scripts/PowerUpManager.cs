using Entities;
using System.Collections;
using System.Collections.Generic;
using Tiles;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public GameObject ConnectionPrefab;
    public BoardGenerator BG;
    private List<Connection>[,] Connections;
    private Vector2Int PlayerPos;
    public Transform PlayerTransform;
    public List<Tile> ProxyTiles;

    private class Connection
    {
        public Vector2Int ConnectedTile;
        public GameObject ConnectionGO;
    }


    private void Awake()
    {
        Instance = this;

        Connections = new List<Connection>[BoardGenerator.boardSize, BoardGenerator.boardSize];
        for (int x = 0; x < BoardGenerator.boardSize; x++)
        {
            for (int y = 0; y < BoardGenerator.boardSize; y++)
            {
                Connections[x, y] = new List<Connection>();
            }
        }
    }

    private void Start()
    {
        if (Game.Instance.CurrentPlayer == null)
        {
            Debug.Log("Player nie istnieje, jestesmy w dupie");
        }
        else
        {
            PlayerTransform = Game.Instance.CurrentPlayer.GetComponent<Player>().transform;
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.K))
        {
            BG.board[3, 3].TakeDamage(100);
        }

        CheckPlayerPosition();
        CheckForConnection();
    }

    public void DestroyTile(Vector2Int Position)
    {
        List<Vector2Int> TilesToUpdate = new List<Vector2Int>();
        foreach (var connection in Connections[Position.x, Position.y])
        {
            TilesToUpdate.Add(connection.ConnectedTile);
            Destroy(connection.ConnectionGO);
            Connections[connection.ConnectedTile.x, connection.ConnectedTile.y]
                .Remove(FindConnection(connection.ConnectedTile, Position));
        }

        Connections[Position.x, Position.y].Clear();
        foreach (var tile in TilesToUpdate)
        {
            UpdateTiles(tile);
        }

        UpdateTiles(Position);
    }

    public void MakeConnection(Vector2Int Pos1, Vector2Int Pos2)
    {
        Connection con = new Connection();
        con.ConnectedTile = new Vector2Int(Pos2.x, Pos2.y);
        con.ConnectionGO = CreateEdge(Pos1, Pos2);
        Connections[Pos1.x, Pos1.y].Add(con);

        Connection con2 = new Connection();
        con2.ConnectionGO = con.ConnectionGO;
        con2.ConnectedTile = new Vector2Int(Pos1.x, Pos1.y);
        Connections[Pos2.x, Pos2.y].Add(con2);

        UpdateTiles(Pos1);
    }

    private GameObject CreateEdge(Vector2Int Pos1, Vector2Int Pos2)
    {
        GameObject newCon = Instantiate(ConnectionPrefab, transform);
        newCon.transform.position = (BoardGenerator.GetRealPosition(Pos1) + BoardGenerator.GetRealPosition(Pos2)) / 2f;
        if (Pos1.x == Pos2.x)
        {
            newCon.transform.Rotate(Vector3.up, 90f);
        }

        return newCon;
    }

    Connection FindConnection(Vector2Int Pos1, Vector2Int Pos2)
    {
        foreach (Connection con in Connections[Pos1.x, Pos1.y])
        {
            if (con.ConnectedTile == Pos2)
                return con;
        }

        return null;
    }

    public void UpdateTiles(Vector2Int Pos)
    {
        bool[,] Updated = new bool[BoardGenerator.boardSize, BoardGenerator.boardSize];
        for (int x = 0; x < BoardGenerator.boardSize; x++)
        {
            for (int y = 0; y < BoardGenerator.boardSize; y++)
            {
                Updated[x, y] = false;
            }
        }

        List<Tile> Tiles = new List<Tile>();
        bool PowerGenerator = false;

        Queue<Vector2Int> Q = new Queue<Vector2Int>();
        Q.Enqueue(Pos);
        while (Q.Count > 0)
        {
            Tiles.Add(BG.board[Q.Peek().x, Q.Peek().y]);
            if (BG.board[Q.Peek().x, Q.Peek().y].PowerGenerator == true)
                PowerGenerator = true;
            Updated[Q.Peek().x, Q.Peek().y] = true;
            foreach (var con in Connections[Q.Peek().x, Q.Peek().y])
            {
                if (Updated[con.ConnectedTile.x, con.ConnectedTile.y] == true)
                {
                    Debug.Log(con.ConnectedTile.x + " " + con.ConnectedTile.y);
                    continue;
                }

                Q.Enqueue(con.ConnectedTile);
            }

            Q.Dequeue();
        }

        foreach (var tile in Tiles)
        {
            if (PowerGenerator)
            {
                tile.PowerOn();
            }
            else
            {
                tile.PowerOff();
            }
        }

        UpdatePowerUps();
    }

    private void UpdatePowerUps()
    {
        //TODO: tutaj potrzeba funkcji "ClearStats()" ktora zresetuje gracza do stanu poczatkowego
        PlayerTransform.GetComponent<Player>().ResetPowerUps();

        bool[,] Updated = new bool[BoardGenerator.boardSize, BoardGenerator.boardSize];
        for (int x = 0; x < BoardGenerator.boardSize; x++)
        {
            for (int y = 0; y < BoardGenerator.boardSize; y++)
            {
                Updated[x, y] = false;
            }
        }

        foreach (var tile in ProxyTiles)
        {
            tile.Highlight(false);
        }

        ProxyTiles.Clear();
        bool PowerGenerator = false;

        Queue<Vector2Int> Q = new Queue<Vector2Int>();
        Q.Enqueue(PlayerPos);
        while (Q.Count > 0)
        {
            ProxyTiles.Add(BG.board[Q.Peek().x, Q.Peek().y]);
            if (BG.board[Q.Peek().x, Q.Peek().y].PowerGenerator == true)
                PowerGenerator = true;
            Updated[Q.Peek().x, Q.Peek().y] = true;
            Debug.Log(Q.Peek().x + " " + Q.Peek().y);
            foreach (var con in Connections[Q.Peek().x, Q.Peek().y])
            {
                if (Updated[con.ConnectedTile.x, con.ConnectedTile.y] == true)
                {
                    Debug.Log(con.ConnectedTile.x + " " + con.ConnectedTile.y);
                    continue;
                }

                Q.Enqueue(con.ConnectedTile);
            }

            Q.Dequeue();
        }

        if (PowerGenerator)
        {
            foreach (var tile in ProxyTiles)
            {
                tile.ApplyPowerUP();
                tile.Highlight(true);
            }
        }
    }

    private void CheckPlayerPosition()
    {
        Vector2Int newPlayerPos = new Vector2Int((int) ((PlayerTransform.position.x + 3f) / 6f),
            (int) ((PlayerTransform.position.z + 3f) / 6f));
        if (newPlayerPos != PlayerPos)
        {
            PlayerPos = new Vector2Int(newPlayerPos.x, newPlayerPos.y);
            UpdatePowerUps();
        }

        PlayerPos = new Vector2Int(newPlayerPos.x, newPlayerPos.y);
    }

    private void CheckForConnection()
    {
        if (Game.Instance.points < 100)
        {
            return;
        }
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit cameraRayHit;

        if (Physics.Raycast(cameraRay, out cameraRayHit))
        {
            if (cameraRayHit.transform.tag == "ConButton")
            {
                ConnectionButton ConButton = cameraRayHit.transform.gameObject.GetComponent<ConnectionButton>();
                if (Input.GetMouseButtonDown(1))
                {
                    Game.Instance.points -= 100;

                    if (FindConnection(ConButton.pos1, ConButton.pos2) == null)
                    {
                        MakeConnection(ConButton.pos1, ConButton.pos2);
                    }
                }
                else
                {
                    ConButton.HighLight();
                }
                
            }
        }
    }

    /// <summary>
    ///     Main instance getter
    /// </summary>
    public static PowerUpManager Instance { get; private set; }

}