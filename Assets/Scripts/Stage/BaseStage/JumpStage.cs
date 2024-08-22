using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using DG.Tweening;

public class JumpStage : BaseStage
{
    GameObject player;
    GameObject currentMap;
    GameObject icon;
    GameObject circleEnemyPrefab;
    GameObject dropCircleEnemyPrefab;
    GameObject sideCircleEnemyPrefab;
    GameObject rayParent;

    private int patten = 2;
    private float screenX;
    private float screenY;
    private float playTime = 1;
    private GameObject enemyParent;
    private GameObject mapParent;

    private GameObject enemyFallParent;
    private int fallFlag = 0;
    private bool fallingStart = false;
    private float fallingSpeed = 5;

    //Patten 0

    //patten 1
    GameObject moveEnemy;
    private float fallingTime = 0;

    //patten 2
    private List<GameObject> rayEnemys = new List<GameObject>();

    public JumpStage(StageController controller) : base(controller)
    {
        circleEnemyPrefab = Resources.Load("Prefabs/JumpStage/CircleEnemy") as GameObject;
        dropCircleEnemyPrefab = Resources.Load("Prefabs/JumpStage/DropCircleEnemy") as GameObject;
        sideCircleEnemyPrefab = Resources.Load("Prefabs/JumpStage/SideCircle") as GameObject;
        rayParent = Resources.Load("Prefabs/JumpStage/RayEnemys") as GameObject;
        icon = Resources.Load("Prefabs/JumpStage/JumpIcon") as GameObject;
        enemyParent = GameObject.Find("Enemy");
        mapParent = GameObject.Find("Map");
        player = GameObject.Find("Player");
    }

    public override void Initialize()
    {
        base.Initialize();
        icon = Util.CreateObjToParent(icon, new Vector3(0, 0, 0), enemyParent);
        //patten = Random.Range(0, 3);
        if (patten == 2) rayParent = Util.CreateObjToParent(rayParent, new Vector3(0, 0, 0), enemyParent);
        currentMap = Resources.Load("Prefabs/JumpStage/Patten_Map_" + patten.ToString()) as GameObject;
        currentMap = Util.CreateObjToParent(currentMap, new Vector3(0, 0, 0), mapParent);
        SetMapScaleToScreen();
        
        GameObject.Find("Player").transform.position = currentMap.transform.Find("PlayerPoint").transform.position;

        // 떨어지는 enemy parent 생성
        enemyFallParent = new GameObject("FallingEnemys");
        enemyFallParent.transform.parent = enemyParent.transform;
        enemyFallParent.transform.position = new Vector2(0, screenY / 2 + 2);

        InitPatten();
    }

    public override void Update()
    {
        base.Update();
        playTime += Time.deltaTime;

        if (patten == 0) PattenZeroFallingEnemy();
        else if (patten == 1)
        {
            moveEnemy.transform.Rotate(new Vector3(0, 0, 3) * 360f * Time.deltaTime);
            PattenOneMoveEnemy();
            PattenOneFallingEnemy();
        }
        else
        {
            fallingTime += Time.deltaTime;
            PattenTwoControlRay();
            PattenTwoFallingEnemy();
        }

        if (fallingStart)
        {
            enemyFallParent.transform.Translate(Vector3.down * fallingSpeed * Time.deltaTime);
        }
        PlayerDropCheck();
    }

    public override void Destroy()
    {
        base.Destroy();
        Util.DestoryObjFromParent(enemyParent);
        Util.DestoryObjFromParent(mapParent);
    }

    private void InitPatten()
    {
        switch (patten)
        {
            case 0:
                playTime = 0;
                break;
            case 1:
                playTime = 2;
                moveEnemy = Util.CreateObjToParent(sideCircleEnemyPrefab, new Vector2(screenX / 2 * -1 + -2, -2.75f), enemyParent);
                moveEnemy.transform.localScale = new Vector3(2, 2, 2);
                break;
            case 2:
                rayEnemys.Clear();
                foreach(Transform child in rayParent.transform)
                {
                    rayEnemys.Add(child.gameObject);
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
    }

    // patten 0
    private void PattenZeroFallingEnemy()
    {
        if (!fallingStart)
        {
            if (fallFlag % 2 == 0) Util.CreateObjToParent(dropCircleEnemyPrefab, new Vector2(screenX / 3 * -1, enemyFallParent.transform.position.y), enemyFallParent);
            else Util.CreateObjToParent(dropCircleEnemyPrefab, new Vector2(screenX / 3, enemyFallParent.transform.position.y), enemyFallParent);
            fallFlag += 1;
            fallingStart = true;
        }
        else
        {
            if (enemyFallParent.transform.position.y < (screenY / 2 + 2) * -1)
            {
                Util.DestoryObjFromParent(enemyFallParent);
                enemyFallParent.transform.position = new Vector2(0, screenY / 2 + 2);
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
        foreach (Transform child in enemyFallParent.transform)
        {
            for (int rad = 0; rad < 360; rad += 20)
            {
                Rigidbody2D spreadEnemy = Util.CreateObjToParent(circleEnemyPrefab, child.position, enemyParent).GetComponent<Rigidbody2D>();
                spreadEnemy.rotation = rad + 270;
                spreadEnemy.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                spreadEnemy.AddForce(Quaternion.Euler(0, 0, rad) * Vector2.up * 4, ForceMode2D.Impulse);
            }
        }
    }

    // patten 1, 2
    private void PattenOneFallingEnemy()
    {
        if (!fallingStart)
        {
            if (fallFlag % 2 == 0)
            {
                float spawnX = screenX / 2 * -1 + 1;
                while (spawnX < screenX / 2 - 0.5f)
                {
                    Util.CreateObjToParent(dropCircleEnemyPrefab, new Vector2(spawnX, enemyFallParent.transform.position.y), enemyFallParent);
                    spawnX += 2.3f; // 사이 간격 조정
                }
            }

            else
            {
                float spawnX = screenX / 2 * -1 + 2f;
                while (spawnX < screenX / 2 - 0.5f)
                {
                    Util.CreateObjToParent(dropCircleEnemyPrefab, new Vector2(spawnX, enemyFallParent.transform.position.y), enemyFallParent);
                    spawnX += 2.3f; // 사이 간격 조정
                }

            }
            fallFlag += 1;
            fallingStart = true;
        }
        else
        {
            if (enemyFallParent.transform.position.y < (screenY / 2 + 2) * -1)
            {
                Util.DestoryObjFromParent(enemyFallParent);
                enemyFallParent.transform.position = new Vector2(0, screenY / 2 + 2);
                fallingStart = false;
            }
        }
    }

    private void PattenOneMoveEnemy()
    {
        if(playTime > 3)
        {
            moveEnemy.transform.DOMoveX(moveEnemy.transform.position.x * -1, 2).SetEase(Ease.InQuart);
            playTime = 0;
        }
    }

    private void PattenTwoFallingEnemy()
    {
        if(fallingTime > 0.3f)
        {
            fallingTime = 0;
            int upAndDown = Random.Range(0, 2);

            int defineX = Random.Range(Mathf.RoundToInt(screenX / 2 * -1 + 1), Mathf.RoundToInt(screenX / 2 - 1));
            if (upAndDown == 0) // 위쪽
            {
                GameObject currentEnemy = Util.CreateObjToParent(dropCircleEnemyPrefab, new Vector2(defineX, screenY / 2 + 1), enemyParent);
                currentEnemy.GetComponent<Rigidbody2D>().velocity = Vector2.down * 6;
            }
            else
            {
                GameObject currentEnemy = Util.CreateObjToParent(dropCircleEnemyPrefab, new Vector2(defineX, screenY / 2 * -1 - 1), enemyParent);
                currentEnemy.transform.rotation = Quaternion.Euler(0, 0, 180);
                currentEnemy.GetComponent<Rigidbody2D>().velocity = Vector2.up * 6;
            }
        }
    }

    private void PattenTwoControlRay()
    {
        if(playTime > 2f)
        {
            playTime = 0;
            int selectRay;
            do
            {
                selectRay = Random.Range(0, rayEnemys.Count);
            } while (rayEnemys[selectRay].activeSelf);
            rayEnemys[selectRay].SetActive(true);
        }
    }

    private void PlayerDropCheck()
    {
        if(player.transform.position.y < screenY/2 * -1)
        {
            player.transform.position = currentMap.transform.Find("PlayerPoint").transform.position;
        }
    }
}
