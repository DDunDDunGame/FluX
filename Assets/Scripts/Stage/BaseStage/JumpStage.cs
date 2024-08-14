using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using DG.Tweening;
using System.Runtime.InteropServices.WindowsRuntime;

public class JumpStage : BaseStage
{
    List<GameObject> tiles = new List<GameObject>();
    GameObject currentMap;
    GameObject squareEnemyPrefab;
    GameObject circleEnemyPrefab;
    GameObject rayEnemyPrefab;

    private int patten;
    private float screenX;
    private float screenY;
    private float playTime = 1;
    private GameObject enemyPrent;

    private GameObject enemyFallPrent;
    private int fallFlag = 0;
    private bool fallingStart = false;
    private float fallingSpeed = 5;

    //Patten 0

    //patten 1
    GameObject moveEnemy;
    private float moveEnemyTime = 3;

    //patten 2
    private float rayEnemyTime = 4;
    private int rayFlag = 0;
    private bool rayStart = false;
    private List<SpriteRenderer> rayEnemySrs = new List<SpriteRenderer>();

    public JumpStage(StageController controller) : base(controller)
    {
        tiles.Add(Resources.Load("Prefabs/JumpStage/Patten_Map_0") as GameObject);
        tiles.Add(Resources.Load("Prefabs/JumpStage/Patten_Map_1_2") as GameObject);
        squareEnemyPrefab = Resources.Load("Prefabs/JumpStage/SquareEnemy") as GameObject;
        circleEnemyPrefab = Resources.Load("Prefabs/JumpStage/CircleEnemy") as GameObject;
        rayEnemyPrefab = Resources.Load("Prefabs/JumpStage/RayEnemy") as GameObject;
        enemyPrent = GameObject.Find("Enemy");
    }

    public override void Initialize()
    {
        base.Initialize();
        patten = Random.Range(0, 3);
        currentMap = Util.MapCreate(tiles[patten == 0 ? 0 : 1], new Vector3(0, 0, 0));
        SetMapScaleToScreen();
        
        GameObject.Find("Player").transform.position = currentMap.transform.Find("PlayerPoint").transform.position;

        // 떨어지는 enemy parent 생성
        enemyFallPrent = new GameObject("FallingEnemys");
        enemyFallPrent.transform.parent = enemyPrent.transform;
        enemyFallPrent.transform.position = new Vector2(0, screenY / 2 + 2);

        InitPatten();
        Debug.Log("JumpStage Initialize");
    }

    public override void Update()
    {
        base.Update();
        playTime += Time.deltaTime;

        if (patten == 0) PattenZeroFallingEnemy();
        else if (patten == 1)
        {
            PattenOneMoveEnemy();
            PattenOneTwoFallingEnemy();
        }
        else
        {
            PattenTwoControlRay();
            PattenOneTwoFallingEnemy();
        }

        if (fallingStart)
        {
            enemyFallPrent.transform.Translate(Vector3.down * fallingSpeed * Time.deltaTime);
        }

        Debug.Log("JumpStage Update");
    }

    public override void Destroy()
    {
        base.Destroy();
        Util.MapDestroy();
        Debug.Log("JumpStage Destroy");
    }

    private void InitPatten()
    {
        switch (patten)
        {
            case 0:
                playTime = 0;
                break;
            case 1:
                moveEnemy = Util.CreateObjToParent(squareEnemyPrefab, new Vector2(screenX / 2 * -1 + -2, -2.75f), enemyPrent);
                moveEnemy.transform.localScale = new Vector3(2, 2, 2);
                break;
            case 2:
                rayEnemySrs.Add(Util.CreateObjToParent(rayEnemyPrefab, new Vector3(0, -2.75f, 0), enemyPrent).GetComponent<SpriteRenderer>());
                rayEnemySrs.Add(Util.CreateObjToParent(rayEnemyPrefab, new Vector3(0, -0.75f, 0), enemyPrent).GetComponent<SpriteRenderer>());
                foreach (SpriteRenderer child in rayEnemySrs)
                {
                    child.transform.localScale = new Vector2(Mathf.Ceil(screenX / child.sprite.bounds.size.x), 2);
                    child.color = new Color32(255, 0, 0, 0);
                }
                playTime = 0;
                break;
            default:
                Debug.LogError("해당 패턴은 존재하지 않습니다. JumpStage.cs");
                break;
        }
    }

    // 화면 비율에 맞게 타일 크기 조정
    private void SetMapScaleToScreen()
    {
        screenY = Camera.main.orthographicSize * 2;
        screenX = screenY / Screen.height * Screen.width;
        if (patten != 0)
        {
            GameObject tile = currentMap.transform.GetChild(0).gameObject;
            float spriteX = tile.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
            tile.transform.localScale = new Vector2(Mathf.Ceil(screenX / spriteX), tile.transform.localScale.y);
        }
    }

    // patten 0
    private void PattenZeroFallingEnemy()
    {
        if (!fallingStart)
        {
            if (fallFlag % 2 == 0) Util.CreateObjToParent(circleEnemyPrefab, new Vector2(screenX / 3 * -1, enemyFallPrent.transform.position.y), enemyFallPrent);
            else Util.CreateObjToParent(circleEnemyPrefab, new Vector2(screenX / 3, enemyFallPrent.transform.position.y), enemyFallPrent);
            fallFlag += 1;
            fallingStart = true;
        }
        else
        {
            if (enemyFallPrent.transform.position.y < (screenY / 2 + 2) * -1)
            {
                Util.DestoryObjFromParent(enemyFallPrent);
                enemyFallPrent.transform.position = new Vector2(0, screenY / 2 + 2);
                fallingStart = false;
                playTime = 0;
            }
            // 2초마다 실행
            if (playTime > 1.0f)
            {
                SpreadCircleEnemy();
                playTime = 0;
            }
        }
    }

    private void SpreadCircleEnemy() // 적 퍼짐 관리
    {
        foreach (Transform child in enemyFallPrent.transform)
        {
            for (int rad = 0; rad < 360; rad += 20)
            {
                Rigidbody2D spreadEnemy = Util.CreateObjToParent(circleEnemyPrefab, child.position, enemyPrent).GetComponent<Rigidbody2D>();
                spreadEnemy.rotation = rad;
                spreadEnemy.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                spreadEnemy.AddForce(Quaternion.Euler(0, 0, rad) * Vector2.up * 4, ForceMode2D.Impulse);
            }
        }
    }

    // patten 1, 2
    private void PattenOneTwoFallingEnemy()
    {
        if (!fallingStart)
        {
            if (fallFlag % 2 == 0)
            {
                float spawnX = screenX / 2 * -1 + 1;
                while (spawnX < screenX / 2 - 0.5f)
                {
                    Util.CreateObjToParent(squareEnemyPrefab, new Vector2(spawnX, enemyFallPrent.transform.position.y), enemyFallPrent);
                    spawnX += 2.3f; // 사이 간격 조정
                }
            }

            else
            {
                float spawnX = screenX / 2 * -1 + 2f;
                while (spawnX < screenX / 2 - 0.5f)
                {
                    Util.CreateObjToParent(squareEnemyPrefab, new Vector2(spawnX, enemyFallPrent.transform.position.y), enemyFallPrent);
                    spawnX += 2.3f; // 사이 간격 조정
                }

            }
            fallFlag += 1;
            fallingStart = true;
        }
        else
        {
            if (enemyFallPrent.transform.position.y < (screenY / 2 + 2) * -1)
            {
                Util.DestoryObjFromParent(enemyFallPrent);
                enemyFallPrent.transform.position = new Vector2(0, screenY / 2 + 2);
                fallingStart = false;
            }
        }
    }

    private void PattenOneMoveEnemy()
    {
        if(playTime > moveEnemyTime + 2)
        {
            moveEnemy.transform.DOMoveX(moveEnemy.transform.position.x * -1, moveEnemyTime).SetEase(Ease.InQuart);
            playTime = 0;
        }
    }

    private void PattenTwoControlRay()
    {
        SpriteRenderer actionRay;
        if (!rayStart)
        {
            actionRay = rayEnemySrs[rayFlag % 2];
            actionRay.color = new Color32(255, 0, 0, 0);
            actionRay.transform.localScale = new Vector3(actionRay.transform.localScale.x, 2, 0);
            actionRay.DOFade(1, rayEnemyTime).SetEase(Ease.Linear);
            rayStart = true;
        }
        else
        {
            if(playTime > rayEnemyTime + 1)
            {
                rayFlag += 1;
                playTime = 0;
                rayStart = false;

                actionRay = rayEnemySrs[(rayFlag - 1) % 2];
                actionRay.color = new Color32(255, 255, 255, 255);
                actionRay.transform.localScale = new Vector3(actionRay.transform.localScale.x, 3, 0);
                actionRay.transform.DOScaleY(0.1f, 1).SetEase(Ease.Linear);
                actionRay.DOFade(0, 1).SetEase(Ease.Linear);
            }
        }
    }
}
