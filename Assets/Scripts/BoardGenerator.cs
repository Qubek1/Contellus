using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Tiles;


public enum PlateTypeName { Normal, SpeedBoost, AttackBoost, AttackSpeedBoost, MultiShoot, Regen, Power}
[System.Serializable]
public class PlateType
{
    public PlateTypeName name;
    public GameObject platePrefab;
    public int quantity;
    public Color color;
}
public class BoardGenerator : MonoBehaviour
{
    public static int boardSize = 15;
    public Tile[,] board = new Tile[boardSize, boardSize];
    [SerializeField]
    public PlateType[] PlateTypes;
    public Color PowerOffColor;
    public Color PowerOnColor;
    public Color HighlightColor;
    public Gradient DamagedTileGradient;
    public GameObject ConnectionButtonGO;
    public PowerUpManager PUM;
    
    void Start()
    {
        GenerateBoard();
        GenerateButtons();
    }

    void Update()
    {

    }
    void GenerateBoard()
    {
        int plates = boardSize * boardSize;
        var values = System.Enum.GetValues(typeof(PlateTypeName));
        for (int k = 0; k < values.Length; k++)
        {
            int quantity = PlateTypes.First(type => type.name == (PlateTypeName)values.GetValue(k)).quantity;
            GameObject prafab = PlateTypes.First(type => type.name == (PlateTypeName)values.GetValue(k)).platePrefab;
            for (int l = 0; l < quantity; l++)
            {
                int spot = Random.Range(0, plates);
                plates--;
                int counter = 0;
                bool stop = false;
                for (int i = 0; i < boardSize; i++)
                {
                    for (int j = 0; j < boardSize; j++)
                    {
                        if (board[i, j] != null) continue;
                        if (counter == spot)
                        {
                            board[i, j] = prafab.GetComponent<Tile>();
                            board[i, j].PowerOnColor = PlateTypes.First(type => type.name == (PlateTypeName)values.GetValue(k)).color;
                            board[i, j].PowerOffColor = PowerOffColor;
                            board[i, j].PowerOnColor2 = PowerOnColor;
                            board[i, j].ProxyHighlight = HighlightColor;
                            board[i, j].DamageGradient = DamagedTileGradient;
                            stop = true;
                            break;
                        }
                        counter++;
                    }
                    if (stop) break;
                }
            }
        }
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                if (board[i, j] == null) continue;
                GameObject plate = Instantiate(board[i, j].gameObject,transform) as GameObject;
                board[i, j] = plate.GetComponent<Tile>();
                board[i, j].Position = new Vector2Int(i, j);
                board[i, j].PUM = PUM;
                plate.GetComponent<Tile>().PowerOff();
                plate.transform.position = new Vector3(i*6, 0, j*6);
            }
        }
    }

    private void GenerateButtons()
    {
        for (int x = 0; x < boardSize; x++)
        {
            for (int y = 0; y < boardSize-1; y++)
            {
                Vector3 Pos = GetRealPosition(new Vector2Int(x, y));
                Pos += new Vector3(0, 0, 3);
                GameObject GO = Instantiate(ConnectionButtonGO, transform) as GameObject;
                GO.transform.position = Pos;
                ConnectionButton ConButton = GO.GetComponent<ConnectionButton>();
                ConButton.pos1 = new Vector2Int(x, y);
                ConButton.pos2 = new Vector2Int(x, y+1);
            }
        }
        for (int x = 0; x < boardSize - 1; x++)
        {
            for (int y = 0; y < boardSize; y++)
            {
                Vector3 Pos = GetRealPosition(new Vector2Int(x, y));
                Pos += new Vector3(3, 0, 0);
                GameObject GO = Instantiate(ConnectionButtonGO, transform);
                GO.transform.position = Pos;
                ConnectionButton ConButton = GO.GetComponent<ConnectionButton>();
                ConButton.pos1 = new Vector2Int(x, y);
                ConButton.pos2 = new Vector2Int(x+1, y);
            }
        }
    }

    public static Vector3 GetRealPosition(Vector2Int Pos)
    {
        return new Vector3(Pos.x * 6f, 0, Pos.y * 6f);
    }
}
