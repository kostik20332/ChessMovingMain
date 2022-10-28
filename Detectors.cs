using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detectors : MonoBehaviour
{
    public bool chessOnCell = false;
    public GameObject squareIndicatorGreenPrefab;
    public GameObject squareIndicatorRedPrefab;
    private GameObject squareIndicator;
    private Moving chessScript;
    private Collider chessCollider;
    public bool allowedToMove = false;
    public bool allowedToDestroy = false;
    private bool redIndicatorsExist = false;
    public bool isBlack;

    public void SquareIndicatorsOn()
    {
        if (!chessOnCell)
        {
            squareIndicator = Instantiate(squareIndicatorGreenPrefab, new Vector3(this.transform.position.x, 0.41f, this.transform.position.z), Quaternion.Euler(90, 0, 0));
            ChessMovingMain.chessOnTheWay = false;
            allowedToMove = true;
        }
        else
        {
            chessScript = chessCollider.gameObject.GetComponent<Moving>();
            if (chessScript.isBlack == ChessMovingMain.isWhiteMove)
            {
                squareIndicator = Instantiate(squareIndicatorRedPrefab, new Vector3(this.transform.position.x, 0.41f, this.transform.position.z), Quaternion.Euler(90, 0, 0));
            }
            allowedToMove = false;
            ChessMovingMain.chessOnTheWay = true;
        }
    }

    public void PawnSquareIndicatorsOn()
    {
        if (!chessOnCell)
        {
            squareIndicator = Instantiate(squareIndicatorGreenPrefab, new Vector3(this.transform.position.x, 0.41f, this.transform.position.z), Quaternion.Euler(90, 0, 0));
            ChessMovingMain.chessOnTheWay = false;
            allowedToMove = true;
        }
        else
        {
            allowedToMove = false;
            ChessMovingMain.chessOnTheWay = true;
        }
    }

    public void PawnRedSquareIndicatorsOn()
    {
        if (chessOnCell)
        {
            redIndicatorsExist = true;
            allowedToDestroy = true;
            squareIndicator = Instantiate(squareIndicatorRedPrefab, new Vector3(this.transform.position.x, 0.41f, this.transform.position.z), Quaternion.Euler(90, 0, 0));
        }
    }

    public void SquareIndicatorsOff()
    {
        allowedToMove = false;
        Destroy(squareIndicator);
        allowedToDestroy = false;
    }

    public void ChessOnTheWayCheck()
    {
        if (!chessOnCell)
        {
            ChessMovingMain.chessOnTheWay = false;
        }
        else
        {
            ChessMovingMain.chessOnTheWay = true;
        }
    }

    public void chessDeleting()
    {
        GameObject.Destroy(chessCollider.gameObject);
    }

    public bool isChessColourBlack()
    {
        if(chessCollider != null)
        {
            chessScript = chessCollider.gameObject.GetComponent<Moving>();
            if (chessScript.isBlack)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    void Update()
    {
        if (!ChessMovingMain.visibleSquares)
        {
            allowedToMove = false;
            Destroy(squareIndicator);
        }
        if (redIndicatorsExist)
        {
            if (ChessMovingMain.pawnRedIndicatorsDelete)
            {
                Destroy(squareIndicator);
                redIndicatorsExist = false;
            }
        }
    }

    void OnTriggerEnter(Collider oth)
    {
        chessCollider = oth;
        chessOnCell = true;
    }

    void OnTriggerExit(Collider oth)
    {
        chessCollider = null;
        chessOnCell = false;
    }

}
