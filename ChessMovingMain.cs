using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessMovingMain : MonoBehaviour
{
    private Camera _camera;
    private GameObject pawn;
    private GameObject rook;
    private GameObject horse;
    private GameObject elephant;
    private GameObject queen;
    private GameObject king;
    private GameObject cellIndicator;
    public GameObject pawnPrefab;
    public GameObject rookPrefab;
    public GameObject horsePrefab;
    public GameObject elephantPrefab;
    public GameObject queenPrefab;
    public GameObject kingPrefab;
    private GameObject detector;
    private GameObject detector2;//для первого хода пешки
    public GameObject squareIndicator;
    public int pos;
    public int i;
    public int indicator = 0;
    public int indicator2 = 0;
    public int indicator3 = 0;
    public int chessOnTheWayCounter = 0;
    public static int chessColour;
    private int directionChanging = -1;
    private int posAllowed;
    private int posAllowedFirstMotion;
    private int value;
    private int value2;
    private int detectorNumber;
    private int posOfChessOnTheWay;
    static public int squareCounter;
    public int squareCounterPubl;
    private float indicatorPosition;
    private float startingPosPawn;
    private float startingPosRook;
    private float startingPosHorse;
    private float startingPosElephant;
    private string posString;
    private string posString2;
    private string posString3;
    public bool pawnSelected;
    public bool chessSelected;
    public bool rookSelected;
    public bool horseSelected;
    public bool elephantSelected;
    public bool queenSelected;
    public bool kingSelected;
    public bool chessOnCell2;
    static public bool chessOnTheWay = false;
    static public bool chessOnCellBreaker;
    static public bool visibleSquarePawn = true;
    static public bool visibleSquares = true;
    private bool thisAllowedToMove;
    public static bool isWhiteMove = true;
    public bool square;
    public static bool pawnRedIndicatorsDelete = false;
    private Moving script;
    private Detectors script2;
    public Material[] materialArray;

    void Awake()
    {
        _camera = GetComponent<Camera>();
        for (int i = 1; i > -2; i -= 2)
        {
            chessColour = i;
            //спавн пешек
            for (int k = 0; k < 8; k++)
            {
                startingPosPawn = -9.4f + 2.67f * k;
                Instantiate(pawnPrefab, new Vector3(startingPosPawn, 1.25f, -6.94f * i), Quaternion.identity);
            }
            //спавн ладьей
            for (int z = 0; z < 2; z++)
            {
                startingPosRook = -9.4f + 18.8f * z;
                Instantiate(rookPrefab, new Vector3(startingPosRook, 1.25f, -9.5f * i), Quaternion.identity);
            }
            //спавн коней
            for (int z = 0; z < 2; z++)
            {
                startingPosHorse = -6.73f + 13.46f * z;
                Instantiate(horsePrefab, new Vector3(startingPosHorse, 1.18f, -9.5f * i), Quaternion.Euler(0, 270 * i, 0));
            }
            //спавн слонов
            for (int z = 0; z < 2; z++)
            {
                startingPosElephant = -4f + 8f * z;
                Instantiate(elephantPrefab, new Vector3(startingPosElephant, 1.18f, -9.5f * i), Quaternion.identity);
            }
            //спавн короля и королевы
            Instantiate(queenPrefab, new Vector3(-1.3f, 1.18f, -9.5f * i), Quaternion.identity);
            Instantiate(kingPrefab, new Vector3(1.3f, 3.1f, -9.5f * i), Quaternion.identity);
        }
    }

    void PawnIndicatorsSpawn(int pawnPosition)
    {
        chessOnTheWay = false;
        for (int z = 1; z < 3; z++)
        {
            if (script.isBlack)
            {
                directionChanging = 1;
            }
            if (!chessOnTheWay)
            {
                if (z == 2)
                {
                    posAllowedFirstMotion = pawnPosition - z * directionChanging;
                    posString2 = posAllowedFirstMotion.ToString();
                    detector = GameObject.FindGameObjectWithTag($"{posString2}");
                    script2 = detector.GetComponent<Detectors>();//получаем доступ к скрипту, потом желательно оптимизироать эту штуку
                    script2.PawnSquareIndicatorsOn();
                }
                else
                {
                    posAllowed = pawnPosition - z * directionChanging;
                    posString = posAllowed.ToString();
                    detector = GameObject.FindGameObjectWithTag($"{posString}");
                    script2 = detector.GetComponent<Detectors>();//получаем доступ к скрипту, потом желательно оптимизироать эту штуку
                    script2.PawnSquareIndicatorsOn();
                }
            }
            else
            {
                posOfChessOnTheWay = posAllowed;
                chessOnTheWay = false;
                break;
            }
            directionChanging = -1;
        }
    }

    void PawnMovingIndicatorsSpawn(int pawnPosition)
    {
        if (script.isBlack)
        {
            directionChanging = 1;
        }
        chessOnTheWay = false;
        posAllowed = pawnPosition - 1 * directionChanging;
        posString = posAllowed.ToString();
        detector = GameObject.FindGameObjectWithTag($"{posString}");
        script2 = detector.GetComponent<Detectors>();//получаем доступ к скрипту, потом желательно оптимизироать эту штуку
        script2.PawnSquareIndicatorsOn();
        directionChanging = -1;
    }

    void PawnRedIndicatorsSpawn(int pawnPosition)
    {
        pawnRedIndicatorsDelete = false;//отключение переменной с помощью которой удаляем красные индикаторы пешек
        chessOnTheWay = false;
        for (int z = 0, x = 1; z < 2; z++, x -= 2)
        {
            if (script.isBlack)
            {
                directionChanging = 1;
            }
            posAllowed = pawnPosition - (11 - 2 * z) * directionChanging * x;
            if (posAllowed > 10 && posAllowed < 89)
            {
                if (z == 0)
                {
                    posString2 = posAllowed.ToString();
                    detector = GameObject.FindGameObjectWithTag($"{posString2}");
                }
                else
                {
                    posString3 = posAllowed.ToString();
                    detector = GameObject.FindGameObjectWithTag($"{posString3}");
                }
                script2 = detector.GetComponent<Detectors>();//получаем доступ к скрипту, потом желательно оптимизироать эту штуку
                chessOnCell2 = script2.chessOnCell;
                if (chessOnCell2)
                {
                    if (isWhiteMove == script2.isChessColourBlack())
                    {
                        script2.PawnRedSquareIndicatorsOn();
                    }
                }
                else
                {
                    chessOnTheWay = false;
                    break;
                }
            }
        }
        directionChanging = -1;
    }

    void RookIndicatorsSpawn()
    {
        //создание индикаторов
        chessOnTheWay = false;
        visibleSquares = true;
        value = pos + 1;
        value2 = pos - pos % 10 + 9;
        for (int i = value; i < value2; i++)
        {
            if (!chessOnTheWay)
            {
                posAllowed = i;
                posString = posAllowed.ToString();
                detector = GameObject.FindGameObjectWithTag($"{posString}");
                script2 = detector.GetComponent<Detectors>();//получаем доступ к скрипту, потом желательно оптимизироать эту штуку
                script2.SquareIndicatorsOn();
            }
            else
            {
                posOfChessOnTheWay = posAllowed;
                chessOnTheWay = false;
                break;
            }
        }
        value = pos - 1;
        value2 = pos - pos % 10;
        for (int i = value; i > value2; i--)
        {
            if (!chessOnTheWay)
            {
                posAllowed = i;
                posString = posAllowed.ToString();
                detector = GameObject.FindGameObjectWithTag($"{posString}");
                script2 = detector.GetComponent<Detectors>();//получаем доступ к скрипту, потом желательно оптимизироать эту штуку
                script2.SquareIndicatorsOn();
            }
            else
            {
                posOfChessOnTheWay = posAllowed;
                chessOnTheWay = false;
                break;
            }
        }
        //создание индикаторов
        chessOnTheWay = false;
        value = pos + 10;
        while (value < 90)
        {
            if (!chessOnTheWay)
            {
                posAllowed = value;
                if (posAllowed != pos)
                {
                    posString = posAllowed.ToString();
                    detector = GameObject.FindGameObjectWithTag($"{posString}");
                    script2 = detector.GetComponent<Detectors>();//получаем доступ к скрипту, потом желательно оптимизироать эту штуку
                    script2.SquareIndicatorsOn();
                }
                value += 10;
            }
            else
            {
                posOfChessOnTheWay = posAllowed;
                chessOnTheWay = false;
                break;
            }
        }
        //создание индикаторов
        chessOnTheWay = false;
        value = pos - 10;
        while (value > 10)
        {
            if (!chessOnTheWay)
            {
                posAllowed = value;
                if (posAllowed != pos)
                {
                    posString = posAllowed.ToString();
                    detector = GameObject.FindGameObjectWithTag($"{posString}");
                    script2 = detector.GetComponent<Detectors>();//получаем доступ к скрипту, потом желательно оптимизироать эту штуку
                    script2.SquareIndicatorsOn();
                }
                value -= 10;
            }
            else
            {
                posOfChessOnTheWay = posAllowed;
                chessOnTheWay = false;
                break;
            }
        }
    }

    void AnyChessIndicatorsDelete()
    {
        for (int i = 1; i < 9; i++)
        {
            for (int j = 1; j < 9; j++)
            {
                detectorNumber = 10 * i + j;
                posString = detectorNumber.ToString();
                detector = GameObject.FindGameObjectWithTag($"{posString}");
                script2 = detector.GetComponent<Detectors>();//получаем доступ к скрипту, потом желательно оптимизироать эту штуку
                script2.SquareIndicatorsOff();
            }
        }
    }

    void HorseIndicators()
    {
        posString = value.ToString();
        detector = GameObject.FindGameObjectWithTag($"{posString}");
        if (detector != null)
        {
            script2 = detector.GetComponent<Detectors>();//получаем доступ к скрипту, потом желательно оптимизироать эту штуку
            script2.SquareIndicatorsOn();
        }
    }
    void HorseIndicatorsSpawn()
    {
        //создание индикаторов
        visibleSquares = true;
        value = pos + 12;
        value2 = pos % 10;
        if (value > 10 && value < 89 && value2 < 7)
        {
            HorseIndicators();
        }
        value = pos - 12;
        value2 = pos % 10;
        if (value > 10 && value < 89 && value2 > 2)
        {
            HorseIndicators();
        }
        value = pos - 8;
        value2 = pos % 10;
        if (value > 10 && value < 89 && value2 < 7)
        {
            HorseIndicators();
        }
        value = pos + 8;
        value2 = pos % 10;
        if (value > 10 && value < 89 && value2 > 2)
        {
            HorseIndicators();
        }
        value = pos + 21;
        value2 = pos % 10;
        if (value > 10 && value < 89 && value2 < 8)
        {
            HorseIndicators();
        }
        value = pos - 19;
        value2 = pos % 10;
        if (value > 10 && value < 89 && value2 < 8)
        {
            HorseIndicators();
        }
        value = pos - 21;
        value2 = pos % 10;
        if (value > 10 && value < 89 && value2 > 1)
        {
            HorseIndicators();
        }
        value = pos + 19;
        value2 = pos % 10;
        if (value > 10 && value < 89 && value2 > 1)
        {
            HorseIndicators();
        }
    }
    void ElephantIndicatorsSpawn()
    {
        visibleSquares = true;
        chessOnTheWay = false;
        value = pos + 11;
        for (int v = value; v < 89; v += 11)
        {
            value2 = v % 10;
            if (!chessOnTheWay && value2 < 9)
            {
                posString = v.ToString();
                detector = GameObject.FindGameObjectWithTag($"{posString}");
                script2 = detector.GetComponent<Detectors>();//получаем доступ к скрипту, потом желательно оптимизироать эту штуку
                script2.SquareIndicatorsOn();
            }
            else
            {
                posOfChessOnTheWay = posAllowed;
                chessOnTheWay = false;
                break;
            }
        }
        chessOnTheWay = false;
        value = pos + 9;
        for (int v = value; v < 89; v += 9)
        {
            value2 = v % 10;
            if (!chessOnTheWay && value2 > 0)
            {
                posString = v.ToString();
                detector = GameObject.FindGameObjectWithTag($"{posString}");
                script2 = detector.GetComponent<Detectors>();//получаем доступ к скрипту, потом желательно оптимизироать эту штуку
                script2.SquareIndicatorsOn();
            }
            else
            {
                posOfChessOnTheWay = posAllowed;
                chessOnTheWay = false;
                break;
            }
        }
        chessOnTheWay = false;
        value = pos - 9;
        for (int v = value; v > 10; v -= 9)
        {
            value2 = v % 10;
            if (!chessOnTheWay && value2 < 9)
            {
                posString = v.ToString();
                detector = GameObject.FindGameObjectWithTag($"{posString}");
                script2 = detector.GetComponent<Detectors>();//получаем доступ к скрипту, потом желательно оптимизироать эту штуку
                script2.SquareIndicatorsOn();
            }
            else
            {
                posOfChessOnTheWay = posAllowed;
                chessOnTheWay = false;
                break;
            }
        }
        chessOnTheWay = false;
        value = pos - 11;
        for (int v = value; v > 10; v -= 11)
        {
            value2 = v % 10;
            if (!chessOnTheWay && value2 > 0)
            {
                posString = v.ToString();
                detector = GameObject.FindGameObjectWithTag($"{posString}");
                script2 = detector.GetComponent<Detectors>();//получаем доступ к скрипту, потом желательно оптимизироать эту штуку
                script2.SquareIndicatorsOn();
            }
            else
            {
                posOfChessOnTheWay = posAllowed;
                chessOnTheWay = false;
                break;
            }
        }
    }
    void QueenIndicatorsSpawn()
    {
        visibleSquares = true;
        chessOnTheWay = false;
        value = pos + 11;
        for (int v = value; v < 89; v += 11)
        {
            value2 = v % 10;
            if (!chessOnTheWay && value2 < 9)
            {
                posString = v.ToString();
                detector = GameObject.FindGameObjectWithTag($"{posString}");
                script2 = detector.GetComponent<Detectors>();//получаем доступ к скрипту, потом желательно оптимизироать эту штуку
                script2.SquareIndicatorsOn();
            }
            else
            {
                posOfChessOnTheWay = posAllowed;
                chessOnTheWay = false;
                break;
            }
        }
        chessOnTheWay = false;
        value = pos + 9;
        for (int v = value; v < 89; v += 9)
        {
            value2 = v % 10;
            if (!chessOnTheWay && value2 > 0)
            {
                posString = v.ToString();
                detector = GameObject.FindGameObjectWithTag($"{posString}");
                script2 = detector.GetComponent<Detectors>();//получаем доступ к скрипту, потом желательно оптимизироать эту штуку
                script2.SquareIndicatorsOn();
            }
            else
            {
                posOfChessOnTheWay = posAllowed;
                chessOnTheWay = false;
                break;
            }
        }
        chessOnTheWay = false;
        value = pos - 9;
        for (int v = value; v > 10; v -= 9)
        {
            value2 = v % 10;
            if (!chessOnTheWay && value2 < 9)
            {
                posString = v.ToString();
                detector = GameObject.FindGameObjectWithTag($"{posString}");
                script2 = detector.GetComponent<Detectors>();//получаем доступ к скрипту, потом желательно оптимизироать эту штуку
                script2.SquareIndicatorsOn();
            }
            else
            {
                posOfChessOnTheWay = posAllowed;
                chessOnTheWay = false;
                break;
            }
        }
        chessOnTheWay = false;
        value = pos - 11;
        for (int v = value; v > 10; v -= 11)
        {
            value2 = v % 10;
            if (!chessOnTheWay && value2 > 0)
            {
                posString = v.ToString();
                detector = GameObject.FindGameObjectWithTag($"{posString}");
                script2 = detector.GetComponent<Detectors>();//получаем доступ к скрипту, потом желательно оптимизироать эту штуку
                script2.SquareIndicatorsOn();
            }
            else
            {
                posOfChessOnTheWay = posAllowed;
                chessOnTheWay = false;
                break;
            }
        }
        chessOnTheWay = false;
        value = pos + 1;
        value2 = pos - pos % 10 + 9;
        for (int i = value; i < value2; i++)
        {
            if (!chessOnTheWay)
            {
                posAllowed = i;
                posString = posAllowed.ToString();
                detector = GameObject.FindGameObjectWithTag($"{posString}");
                script2 = detector.GetComponent<Detectors>();//получаем доступ к скрипту, потом желательно оптимизироать эту штуку
                script2.SquareIndicatorsOn();
            }
            else
            {
                posOfChessOnTheWay = posAllowed;
                chessOnTheWay = false;
                break;
            }
        }
        value = pos - 1;
        value2 = pos - pos % 10;
        for (int i = value; i > value2; i--)
        {
            if (!chessOnTheWay)
            {
                posAllowed = i;
                posString = posAllowed.ToString();
                detector = GameObject.FindGameObjectWithTag($"{posString}");
                script2 = detector.GetComponent<Detectors>();//получаем доступ к скрипту, потом желательно оптимизироать эту штуку
                script2.SquareIndicatorsOn();
            }
            else
            {
                posOfChessOnTheWay = posAllowed;
                chessOnTheWay = false;
                break;
            }
        }
        //создание индикаторов
        chessOnTheWay = false;
        value = pos + 10;
        while (value < 90)
        {
            if (!chessOnTheWay)
            {
                posAllowed = value;
                if (posAllowed != pos)
                {
                    posString = posAllowed.ToString();
                    detector = GameObject.FindGameObjectWithTag($"{posString}");
                    script2 = detector.GetComponent<Detectors>();//получаем доступ к скрипту, потом желательно оптимизироать эту штуку
                    script2.SquareIndicatorsOn();
                }
                value += 10;
            }
            else
            {
                posOfChessOnTheWay = posAllowed;
                chessOnTheWay = false;
                break;
            }
        }
        //создание индикаторов
        chessOnTheWay = false;
        value = pos - 10;
        while (value > 10)
        {
            if (!chessOnTheWay)
            {
                posAllowed = value;
                if (posAllowed != pos)
                {
                    posString = posAllowed.ToString();
                    detector = GameObject.FindGameObjectWithTag($"{posString}");
                    script2 = detector.GetComponent<Detectors>();//получаем доступ к скрипту, потом желательно оптимизироать эту штуку
                    script2.SquareIndicatorsOn();
                }
                value -= 10;
            }
            else
            {
                posOfChessOnTheWay = posAllowed;
                chessOnTheWay = false;
                break;
            }
        }
    }
    void KingIndicatorsSpawn()
    {
        visibleSquares = true;
        for (int i = -1; i < 2; i += 2)
        {
            value = pos + i;
            value2 = value % 10;
            if (value > 10 && value < 89 && value2 < 9 && value2 > 0)
            {
                posString = value.ToString();
                detector = GameObject.FindGameObjectWithTag($"{posString}");
                if (detector != null)
                {
                    script2 = detector.GetComponent<Detectors>();//получаем доступ к скрипту, потом желательно оптимизироать эту штуку
                    script2.SquareIndicatorsOn();
                }
            }
        }
        for (int i = -10; i < 11; i += 20)
        {
            value = pos + i;
            if (value > 10 && value < 89)
            {
                posString = value.ToString();
                detector = GameObject.FindGameObjectWithTag($"{posString}");
                if (detector != null)
                {
                    script2 = detector.GetComponent<Detectors>();//получаем доступ к скрипту, потом желательно оптимизироать эту штуку
                    script2.SquareIndicatorsOn();
                }
            }
        }
        for (int i = -11; i < 12; i += 22)
        {
            value = pos + i;
            value2 = value % 10;
            if (value > 10 && value < 89 && value2 < 9 && value2 > 0)
            {
                posString = value.ToString();
                detector = GameObject.FindGameObjectWithTag($"{posString}");
                if (detector != null)
                {
                    script2 = detector.GetComponent<Detectors>();//получаем доступ к скрипту, потом желательно оптимизироать эту штуку
                    script2.SquareIndicatorsOn();
                }
            }
        }
        for (int i = -9; i < 10; i += 18)
        {
            value = pos + i;
            value2 = value % 10;
            if (value > 10 && value < 89 && value2 < 9 && value2 > 0)
            {
                posString = value.ToString();
                detector = GameObject.FindGameObjectWithTag($"{posString}");
                if (detector != null)
                {
                    script2 = detector.GetComponent<Detectors>();//получаем доступ к скрипту, потом желательно оптимизироать эту штуку
                    script2.SquareIndicatorsOn();
                }
            }
        }
    }

    int longWalkChessMoving(GameObject hitObject, GameObject chess)
    {
        posString = posAllowed.ToString();
        if (hitObject.tag == posString)//проверяем разрешён ли ход на нажатую клетку
        {
            cellIndicator = hitObject;
            script2 = cellIndicator.GetComponent<Detectors>();//получаем доступ к скрипту
            chessOnCell2 = script2.chessOnCell;
            thisAllowedToMove = script2.allowedToMove;
            if (!chessOnCell2 && thisAllowedToMove)//проверяем свободна ли от других фигур клетка в которую хотим сделать ход
            {
                isWhiteMove = !isWhiteMove;//смена цвета фигур которым разрешён ход
                script.Relocate(hitObject.transform.position.x, chess.transform.position.y, hitObject.transform.position.z);//перемещаем объект по координатам объекта на который нажали
                rookSelected = false;
                elephantSelected = false;
                queenSelected = false;
                chessSelected = false;
                posAllowed = 0;
                return 1;
            }
            else if (chessOnCell2 && chessOnTheWayCounter < 2)
            {
                if (isWhiteMove == script2.isChessColourBlack())
                {
                    script2.chessDeleting();
                    isWhiteMove = !isWhiteMove;//смена цвета фигур которым разрешён ход
                    script.Relocate(hitObject.transform.position.x, chess.transform.position.y, hitObject.transform.position.z);//перемещаем объект по координатам объекта на который нажали
                    rookSelected = false;
                    elephantSelected = false;
                    queenSelected = false;
                    chessSelected = false;
                    return 1;
                }
            }
            else
            {
                chessOnTheWay = false;
                return 1;
            }
        }
        else
        {
            cellIndicator = GameObject.FindGameObjectWithTag($"{posString}");
            script2 = cellIndicator.GetComponent<Detectors>();//получаем доступ к скрипту
            if (script2 != null)
            {
                chessOnCell2 = script2.chessOnCell;
                if (chessOnCell2)
                {
                    return 1;
                }
            }
        }
        return 0;
    }

    void HorseMoving(GameObject hitObject)
    {
        posString = value.ToString();
        if (hitObject.tag == posString)//проверяем разрешён ли ход на нажатую клетку по правилам
        {
            cellIndicator = hitObject;
            script2 = cellIndicator.GetComponent<Detectors>();//получаем доступ к скрипту
            chessOnCell2 = script2.chessOnCell;
            thisAllowedToMove = script2.allowedToMove;
            if (!chessOnCell2 && thisAllowedToMove)//проверяем свободна ли от других фигур клетка в которую хотим сделать ход
            {
                isWhiteMove = !isWhiteMove;//смена цвета фигур которым разрешён ход
                script.Relocate(hitObject.transform.position.x, horse.transform.position.y, hitObject.transform.position.z);//перемещаем объект по координатам объекта на который нажали
                horseSelected = false;
                chessSelected = false;
            }
            else if (chessOnCell2)
            {
                if (isWhiteMove == script2.isChessColourBlack())
                {
                    script2.chessDeleting();
                    isWhiteMove = !isWhiteMove;//смена цвета фигур которым разрешён ход
                    script.Relocate(hitObject.transform.position.x, horse.transform.position.y, hitObject.transform.position.z);//перемещаем объект по координатам объекта на который нажали
                    horseSelected = false;
                    chessSelected = false;
                }
            }
        }
    }

    void KingMoving(GameObject hitObject)
    {
        posString = value.ToString();
        if (hitObject.tag == posString)//проверяем разрешён ли ход на нажатую клетку по правилам
        {
            cellIndicator = hitObject;
            script2 = cellIndicator.GetComponent<Detectors>();//получаем доступ к скрипту
            chessOnCell2 = script2.chessOnCell;
            thisAllowedToMove = script2.allowedToMove;
            if (thisAllowedToMove)//проверяем свободна ли от других фигур клетка в которую хотим сделать ход
            {
                isWhiteMove = !isWhiteMove;//смена цвета фигур которым разрешён ход
                script.Relocate(hitObject.transform.position.x, king.transform.position.y, hitObject.transform.position.z);//перемещаем объект по координатам объекта на который нажали
                kingSelected = false;
                chessSelected = false;
            }
            else if (chessOnCell2)
            {
                if (isWhiteMove == script2.isChessColourBlack())
                {
                    script2.chessDeleting();
                    isWhiteMove = !isWhiteMove;//смена цвета фигур которым разрешён ход
                    script.Relocate(hitObject.transform.position.x, king.transform.position.y, hitObject.transform.position.z);//перемещаем объект по координатам объекта на который нажали
                    kingSelected = false;
                    chessSelected = false;
                }
            }
        }
    }

    void ChessActivation(GameObject hitObject)
    {
        if (hitObject.tag == "pawnMoving" || hitObject.tag == "pawn")
        {
            visibleSquares = true;
            pawn = hitObject;//помещаем в переменную пешки выбранную пешку
            script = pawn.GetComponent<Moving>();//получаем доступ к скрипту
            pos = script.posOfChess;//получаем информацию о позиции выбранной пешки
            if (!script.isBlack == isWhiteMove)
            {
                pawnSelected = true;//помечаем, что выбрали пешку
                chessSelected = true;
                pawn.GetComponent<Renderer>().material = materialArray[1];
                PawnRedIndicatorsSpawn(pos);
                if (hitObject.tag == "pawn")
                {
                    PawnIndicatorsSpawn(pos);
                }
                else if (hitObject.tag == "pawnMoving")
                {
                    PawnMovingIndicatorsSpawn(pos);
                }
            }
        }
        else if (hitObject.tag == "rook")
        {
            rook = hitObject;//помещаем в переменную ладьи выбранную ладью
            script = rook.GetComponent<Moving>();//получаем доступ к скрипту
            pos = script.posOfChess;//получаем информацию о позиции выбранной ладьи
            if (!script.isBlack == isWhiteMove)
            {
                rookSelected = true;//помечаем, что уже выбрали ладью
                chessSelected = true;
                rook.GetComponent<Renderer>().material = materialArray[1];
                RookIndicatorsSpawn();
            }
        }
        else if (hitObject.tag == "horse")
        {
            horse = hitObject;//помещаем в переменную ладьи выбранную ладью
            script = horse.GetComponent<Moving>();//получаем доступ к скрипту
            pos = script.posOfChess;//получаем информацию о позиции выбранной ладьи
            if (!script.isBlack == isWhiteMove)
            {
                horseSelected = true;//помечаем, что уже выбрали ладью
                chessSelected = true;
                horse.GetComponent<Renderer>().material = materialArray[1];
                HorseIndicatorsSpawn();
            }
        }
        else if (hitObject.tag == "elephant")
        {
            elephant = hitObject;//помещаем в переменную ладьи выбранную ладью
            script = elephant.GetComponent<Moving>();//получаем доступ к скрипту
            pos = script.posOfChess;//получаем информацию о позиции выбранной ладьи
            if (!script.isBlack == isWhiteMove)
            {
                elephantSelected = true;//помечаем, что уже выбрали ладью
                chessSelected = true;
                elephant.GetComponent<Renderer>().material = materialArray[1];
                ElephantIndicatorsSpawn();
            }
        }
        else if (hitObject.tag == "queen")
        {
            queen = hitObject;//помещаем в переменную ладьи выбранную 
            script = queen.GetComponent<Moving>();//получаем доступ к скрипту
            pos = script.posOfChess;//получаем информацию о позиции выбранной ладьи
            if (!script.isBlack == isWhiteMove)
            {
                queenSelected = true;//помечаем, что уже выбрали ладью
                chessSelected = true;
                queen.GetComponent<Renderer>().material = materialArray[1];
                QueenIndicatorsSpawn();
            }
        }
        else if (hitObject.tag == "king")
        {
            king = hitObject;//помещаем в переменную ладьи выбранную 
            script = king.GetComponent<Moving>();//получаем доступ к скрипту
            pos = script.posOfChess;//получаем информацию о позиции выбранной ладьи
            if (!script.isBlack == isWhiteMove)
            {
                kingSelected = true;//помечаем, что уже выбрали ладью
                chessSelected = true;
                king.GetComponent<Renderer>().material = materialArray[1];
                KingIndicatorsSpawn();
            }
        }
    }

    void Update()
    {
        squareCounterPubl = squareCounter;
        if (chessSelected == false)
        {
            if (Input.GetMouseButtonDown(0))//ждём нажатия на экран
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);//создаём луч в точку, куда тыкаем пальцем
                RaycastHit hit;//создаём переменную для информации о луче
                if (Physics.Raycast(ray, out hit))//метод возвращает true или false и передаёт информацию о луче в переменную hit
                {
                    GameObject hitObject = hit.transform.gameObject;//помещаем в переменную hitObject объект в который попали
                    if (hitObject.tag == "pawnMoving" || hitObject.tag == "pawn")
                    {
                        visibleSquares = true;
                        pawn = hitObject;//помещаем в переменную пешки выбранную пешку
                        script = pawn.GetComponent<Moving>();//получаем доступ к скрипту
                        pos = script.posOfChess;//получаем информацию о позиции выбранной пешки
                        if (!script.isBlack == isWhiteMove)
                        {
                            pawnSelected = true;//помечаем, что выбрали пешку
                            chessSelected = true;
                            pawn.GetComponent<Renderer>().material = materialArray[1];
                            PawnRedIndicatorsSpawn(pos);
                            if (hitObject.tag == "pawn")
                            {
                                PawnIndicatorsSpawn(pos);
                            }
                            else if (hitObject.tag == "pawnMoving")
                            {
                                PawnMovingIndicatorsSpawn(pos);
                            }
                        }
                    }
                    else if (hitObject.tag == "rook")
                    {
                        rook = hitObject;//помещаем в переменную ладьи выбранную ладью
                        script = rook.GetComponent<Moving>();//получаем доступ к скрипту
                        pos = script.posOfChess;//получаем информацию о позиции выбранной ладьи
                        if (!script.isBlack == isWhiteMove)
                        {
                            rookSelected = true;//помечаем, что уже выбрали ладью
                            chessSelected = true;
                            rook.GetComponent<Renderer>().material = materialArray[1];
                            RookIndicatorsSpawn();
                        }
                    }
                    else if (hitObject.tag == "horse")
                    {
                        horse = hitObject;//помещаем в переменную ладьи выбранную ладью
                        script = horse.GetComponent<Moving>();//получаем доступ к скрипту
                        pos = script.posOfChess;//получаем информацию о позиции выбранной ладьи
                        if (!script.isBlack == isWhiteMove)
                        {
                            horseSelected = true;//помечаем, что уже выбрали ладью
                            chessSelected = true;
                            horse.GetComponent<Renderer>().material = materialArray[1];
                            HorseIndicatorsSpawn();
                        }
                    }
                    else if (hitObject.tag == "elephant")
                    {
                        elephant = hitObject;//помещаем в переменную ладьи выбранную ладью
                        script = elephant.GetComponent<Moving>();//получаем доступ к скрипту
                        pos = script.posOfChess;//получаем информацию о позиции выбранной ладьи
                        if (!script.isBlack == isWhiteMove)
                        {
                            elephantSelected = true;//помечаем, что уже выбрали ладью
                            chessSelected = true;
                            elephant.GetComponent<Renderer>().material = materialArray[1];
                            ElephantIndicatorsSpawn();
                        }
                    }
                    else if (hitObject.tag == "queen")
                    {
                        queen = hitObject;//помещаем в переменную ладьи выбранную 
                        script = queen.GetComponent<Moving>();//получаем доступ к скрипту
                        pos = script.posOfChess;//получаем информацию о позиции выбранной ладьи
                        if (!script.isBlack == isWhiteMove)
                        {
                            queenSelected = true;//помечаем, что уже выбрали ладью
                            chessSelected = true;
                            queen.GetComponent<Renderer>().material = materialArray[1];
                            QueenIndicatorsSpawn();
                        }
                    }
                    else if (hitObject.tag == "king")
                    {
                        king = hitObject;//помещаем в переменную ладьи выбранную 
                        script = king.GetComponent<Moving>();//получаем доступ к скрипту
                        pos = script.posOfChess;//получаем информацию о позиции выбранной ладьи
                        if (!script.isBlack == isWhiteMove)
                        {
                            kingSelected = true;//помечаем, что уже выбрали ладью
                            chessSelected = true;
                            king.GetComponent<Renderer>().material = materialArray[1];
                            KingIndicatorsSpawn();
                        }
                    }
                }
            }
        }
        else if (pawnSelected == true && chessSelected == true)//действия если выбрали пешку
        {
            if (Input.GetMouseButtonDown(0))//ждём нажатия на экран
            {
                if (script.isBlack)
                {
                    pawn.GetComponent<Renderer>().material = materialArray[2];
                }
                else
                {
                    pawn.GetComponent<Renderer>().material = materialArray[0];
                }
                visibleSquarePawn = false;
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);//создаём луч в точку, куда тыкаем пальцем
                RaycastHit hit;//создаём переменную для информации о луче
                if (Physics.Raycast(ray, out hit))//метод возвращает true или false и передаёт информацию о луче в переменную hit
                {
                    GameObject hitObject = hit.transform.gameObject;//помещаем в переменную hitObject объект в который попали
                    if (hitObject.tag == posString || hitObject.tag == posString2 || hitObject.tag == posString3)//проверяем разрешён ли ход на нажатую клетку
                    {
                        cellIndicator = hitObject;
                        script2 = cellIndicator.GetComponent<Detectors>();//получаем доступ к скрипту
                        chessOnCell2 = script2.chessOnCell;
                        thisAllowedToMove = script2.allowedToMove;
                        if (!chessOnCell2 && thisAllowedToMove)//проверяем свободна ли от других фигур клетка в которую хотим сделать ход
                        {
                            script.Relocate(hitObject.transform.position.x, pawn.transform.position.y, hitObject.transform.position.z);//перемещаем объект по координатам объекта на который нажали
                            pawnSelected = false;
                            chessSelected = false;
                            isWhiteMove = !isWhiteMove;//смена цвета фигур которым разрешён ход
                            pawn.tag = "pawnMoving";
                            posAllowed = 0;
                            squareCounter += 2;
                        }
                        else if (chessOnCell2)
                        {
                            if (script2.allowedToDestroy)
                            {
                                script2.chessDeleting();
                                isWhiteMove = !isWhiteMove;//смена цвета фигур которым разрешён ход
                                script.Relocate(hitObject.transform.position.x, pawn.transform.position.y, hitObject.transform.position.z);//перемещаем объект по координатам объекта на который нажали
                                pawnSelected = false;
                                chessSelected = false;
                                pawnRedIndicatorsDelete = true;
                            }
                        }
                        else
                        {
                            squareCounter++;
                        }
                    }
                    else
                    {
                        pawnSelected = false;//помечаем, что уже сняли выделение с пешки
                        chessSelected = false;
                        squareCounter++;
                        ChessActivation(hitObject);
                    }
                }
            }
        }
        else if (rookSelected == true && chessSelected == true)//действия если выбрали ладью
        {
            if (Input.GetMouseButtonDown(0))//ждём нажатия на экран
            {
                if (script.isBlack)
                {
                    rook.GetComponent<Renderer>().material = materialArray[2];
                }
                else
                {
                    rook.GetComponent<Renderer>().material = materialArray[0];
                }
                visibleSquares = false;
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);//создаём луч в точку, куда тыкаем пальцем
                RaycastHit hit;//создаём переменную для информации о луче
                if (Physics.Raycast(ray, out hit))//метод возвращает true или false и передаёт информацию о луче в переменную hit
                {
                    GameObject hitObject = hit.transform.gameObject;//помещаем в переменную hitObject объект в который попали
                    chessOnTheWay = false;
                    //проверяем ход по вертикали
                    value2 = pos - pos % 10;
                    for (value = pos - 1; value > value2; value--)
                    {
                        posAllowed = value;
                        if (longWalkChessMoving(hitObject, rook) == 1)
                        {
                            break;
                        }
                    }
                    value2 = pos - pos % 10 + 9;
                    for (value = pos + 1; value < value2; value++)
                    {
                        posAllowed = value;
                        if (longWalkChessMoving(hitObject, rook) == 1)
                        {
                            break;
                        }
                    }
                    //проверяем ход по горизонтали в одну сторону
                    value = pos + 10;
                    while (value < 90)
                    {
                        posAllowed = value;
                        if (longWalkChessMoving(hitObject, rook) == 1)
                        {
                            break;
                        }
                        value += 10;
                    }
                    //проверяем ход по горизонтали в другую сторону
                    value = pos - 10;
                    while (value > 10)
                    {
                        posAllowed = value;
                        if (longWalkChessMoving(hitObject, rook) == 1)
                        {
                            break;
                        }
                        value -= 10;
                    }
                    //если нажали на другую ладью                   
                    AnyChessIndicatorsDelete();
                    rookSelected = false;
                    chessSelected = false;
                    ChessActivation(hitObject);
                }
            }
        }
        else if (horseSelected == true && chessSelected == true)//действия если выбрали ладью
        {
            if (Input.GetMouseButtonDown(0))//ждём нажатия на экран
            {
                if (script.isBlack)
                {
                    horse.GetComponent<Renderer>().material = materialArray[2];
                }
                else
                {
                    horse.GetComponent<Renderer>().material = materialArray[0];
                }
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);//создаём луч в точку, куда тыкаем пальцем
                RaycastHit hit;//создаём переменную для информации о луче
                if (Physics.Raycast(ray, out hit))//метод возвращает true или false и передаёт информацию о луче в переменную hit
                {
                    GameObject hitObject = hit.transform.gameObject;//помещаем в переменную hitObject объект в который попали
                    chessOnTheWay = false;
                    //проверяем ходы
                    value = pos - 12;
                    value2 = pos % 10;
                    if (value > 10 && value < 89 && value2 > 2)
                    {
                        HorseMoving(hitObject);
                    }
                    value = pos + 12;
                    value2 = pos % 10;
                    if (value > 10 && value < 89 && value2 < 7)
                    {
                        HorseMoving(hitObject);
                    }
                    value = pos - 8;
                    value2 = pos % 10;
                    if (value > 10 && value < 89 && value2 < 7)
                    {
                        HorseMoving(hitObject);
                    }
                    value = pos + 8;
                    value2 = pos % 10;
                    if (value > 10 && value < 89 && value2 > 2)
                    {
                        HorseMoving(hitObject);
                    }

                    value = pos - 21;
                    value2 = pos % 10;
                    if (value > 10 && value < 89 && value2 > 1)
                    {
                        HorseMoving(hitObject);
                    }
                    value = pos + 21;
                    value2 = pos % 10;
                    if (value > 10 && value < 89 && value2 < 8)
                    {
                        HorseMoving(hitObject);
                    }
                    value = pos + 19;
                    value2 = pos % 10;
                    if (value > 10 && value < 89 && value2 < 8)
                    {
                        HorseMoving(hitObject);
                    }
                    value = pos - 19;
                    value2 = pos % 10;
                    if (value > 10 && value < 89 && value2 > 1)
                    {
                        HorseMoving(hitObject);
                    }
                    AnyChessIndicatorsDelete();//удаляем все индикаторы
                    horseSelected = false;
                    chessSelected = false;
                    ChessActivation(hitObject);
                }
            }
        }
        else if (elephantSelected == true && chessSelected == true)//действия если выбрали ладью
        {
            if (Input.GetMouseButtonDown(0))//ждём нажатия на экран
            {
                if (script.isBlack)
                {
                    elephant.GetComponent<Renderer>().material = materialArray[2];
                }
                else
                {
                    elephant.GetComponent<Renderer>().material = materialArray[0];
                }
                visibleSquares = false;
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);//создаём луч в точку, куда тыкаем пальцем
                RaycastHit hit;//создаём переменную для информации о луче
                if (Physics.Raycast(ray, out hit))//метод возвращает true или false и передаёт информацию о луче в переменную hit
                {
                    GameObject hitObject = hit.transform.gameObject;//помещаем в переменную hitObject объект в который попали
                    //проверяем можно ли походить на выбранную клетку
                    chessOnTheWay = false;
                    chessOnTheWayCounter = 0;
                    value = pos - 11;
                    for (int v = value; v > 10; v -= 11)
                    {
                        posAllowed = v;
                        posString = posAllowed.ToString();
                        value2 = v % 10;
                        if (value2 > 0)
                        {
                            if (longWalkChessMoving(hitObject, elephant) == 1)
                            {
                                indicator3 = 1;
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    chessOnTheWayCounter = 0;
                    value = pos + 11;
                    for (int v = value; v < 89; v += 11)
                    {
                        posAllowed = v;
                        posString = posAllowed.ToString();
                        value2 = v % 10;
                        if (value2 < 9)
                        {
                            if (longWalkChessMoving(hitObject, elephant) == 1)
                            {
                                indicator3 = 1;
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    chessOnTheWayCounter = 0;
                    value = pos + 9;
                    for (int v = value; v < 89; v += 9)
                    {
                        posAllowed = v;
                        posString = posAllowed.ToString();
                        value2 = v % 10;
                        if (value2 > 0)
                        {
                            if (longWalkChessMoving(hitObject, elephant) == 1)
                            {
                                indicator3 = 1;
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    chessOnTheWayCounter = 0;
                    value = pos - 9;
                    for (int v = value; v > 10; v -= 9)
                    {
                        posAllowed = v;
                        posString = posAllowed.ToString();
                        value2 = v % 10;
                        if (value2 < 9)
                        {
                            if (longWalkChessMoving(hitObject, elephant) == 1)
                            {
                                indicator3 = 1;
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    AnyChessIndicatorsDelete();
                    elephantSelected = false;
                    chessSelected = false;
                    ChessActivation(hitObject);
                }
            }
        }
        else if (queenSelected == true && chessSelected == true)//действия если выбрали королеву
        {
            if (Input.GetMouseButtonDown(0))//ждём нажатия на экран
            {
                if (script.isBlack)
                {
                    queen.GetComponent<Renderer>().material = materialArray[2];
                }
                else
                {
                    queen.GetComponent<Renderer>().material = materialArray[0];
                }
                visibleSquares = false;
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);//создаём луч в точку, куда тыкаем пальцем
                RaycastHit hit;//создаём переменную для информации о луче
                if (Physics.Raycast(ray, out hit))//метод возвращает true или false и передаёт информацию о луче в переменную hit
                {
                    GameObject hitObject = hit.transform.gameObject;//помещаем в переменную hitObject объект в который попали
                    //проверяем можно ли походить на выбранную клетку
                    chessOnTheWay = false;
                    //проверяем на ход слона
                    value = pos - 11;
                    for (int v = value; v > 10; v -= 11)
                    {
                        posAllowed = v;
                        posString = posAllowed.ToString();
                        value2 = v % 10;
                        if (value2 > 0)
                        {
                            if (longWalkChessMoving(hitObject, queen) == 1)
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    value = pos + 11;
                    for (int v = value; v < 89; v += 11)
                    {
                        posAllowed = v;
                        posString = posAllowed.ToString();
                        value2 = v % 10;
                        if (value2 < 9)
                        {
                            if (longWalkChessMoving(hitObject, queen) == 1)
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    value = pos + 9;
                    for (int v = value; v < 89; v += 9)
                    {
                        posAllowed = v;
                        posString = posAllowed.ToString();
                        value2 = v % 10;
                        if (value2 > 0)
                        {
                            if (longWalkChessMoving(hitObject, queen) == 1)
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    value = pos - 9;
                    for (int v = value; v > 10; v -= 9)
                    {
                        posAllowed = v;
                        posString = posAllowed.ToString();
                        value2 = v % 10;
                        if (value2 < 9)
                        {
                            if (longWalkChessMoving(hitObject, queen) == 1)
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    //проверяем на ход ладьи

                    value2 = pos - pos % 10;
                    for (value = pos - 1; value > value2; value--)
                    {
                        posAllowed = value;
                        posString = posAllowed.ToString();
                        if (longWalkChessMoving(hitObject, queen) == 1)
                        {
                            break;
                        }
                    }
                    value2 = pos - pos % 10 + 9;
                    for (value = pos + 1; value < value2; value++)
                    {
                        posAllowed = value;
                        posString = posAllowed.ToString();
                        if (longWalkChessMoving(hitObject, queen) == 1)
                        {
                            break;
                        }
                    }
                    //проверяем ход по горизонтали в одну сторону
                    value = pos + 10;
                    while (value < 90)
                    {
                        posAllowed = value;
                        posString = posAllowed.ToString();
                        if (longWalkChessMoving(hitObject, queen) == 1)
                        {
                            break;
                        }
                        value += 10;
                    }
                    //проверяем ход по горизонтали в другую сторону
                    value = pos - 10;
                    while (value > 10)
                    {
                        posAllowed = value;
                        posString = posAllowed.ToString();
                        if (longWalkChessMoving(hitObject, queen) == 1)
                        {
                            break;
                        }
                        value -= 10;
                    }
                    AnyChessIndicatorsDelete();
                    //если нажали на другого слона
                    queenSelected = false;
                    chessSelected = false;
                    ChessActivation(hitObject);
                }
            }
        }
        else if (kingSelected == true && chessSelected == true)//действия если выбрали короля
        {
            if (Input.GetMouseButtonDown(0))//ждём нажатия на экран
            {
                if (script.isBlack)
                {
                    king.GetComponent<Renderer>().material = materialArray[2];
                }
                else
                {
                    king.GetComponent<Renderer>().material = materialArray[0];
                }
                visibleSquares = false;
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);//создаём луч в точку, куда тыкаем пальцем
                RaycastHit hit;//создаём переменную для информации о луче
                if (Physics.Raycast(ray, out hit))//метод возвращает true или false и передаёт информацию о луче в переменную hit
                {
                    GameObject hitObject = hit.transform.gameObject;//помещаем в переменную hitObject объект в который попали
                    //проверяем можно ли ходить на выбранную клетку
                    for (int i = -1; i < 2; i += 2)
                    {
                        value = pos + i;
                        value2 = value % 10;
                        if (value > 10 && value < 89 && value2 < 9 && value2 > 0)
                        {
                            KingMoving(hitObject);
                        }
                    }
                    for (int i = -10; i < 11; i += 20)
                    {
                        value = pos + i;
                        if (value > 10 && value < 89)
                        {
                            KingMoving(hitObject);
                        }
                    }
                    for (int i = -11; i < 12; i += 22)
                    {
                        value = pos + i;
                        value2 = value % 10;
                        if (value > 10 && value < 89 && value2 < 9 && value2 > 0)
                        {
                            KingMoving(hitObject);
                        }
                    }
                    for (int i = -9; i < 10; i += 18)
                    {
                        value = pos + i;
                        value2 = value % 10;
                        if (value > 10 && value < 89 && value2 < 9 && value2 > 0)
                        {
                            KingMoving(hitObject);
                        }
                    }

                    AnyChessIndicatorsDelete();
                    kingSelected = false;
                    chessSelected = false;
                    ChessActivation(hitObject);
                }
            }
        }
    }
}
