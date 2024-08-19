using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossStage : BaseStage
{
    GameObject bossEnemy;
    GameObject circleEnemy;
    GameObject squareEnemy;
    GameObject rayEnemy;
    GameObject player;
    GameObject enemyParent;
    GameObject mapParent;
    List<GameObject> maps = new List<GameObject>();
    GameObject currentMap;

    int patten = 1;
    float screenX = 0;
    float screenY = 0;
    float playTime = 0;

    // patten 0
    List<Transform> bossWayPoint = new List<Transform>();
    int wayCount = 0;
    float bossMoveTime = 0;
    Vector2 guideDir;
    bool moveStart = false;

    // patten 1
    GameObject rayImage;
    GameObject rayEffect;
    bool attackStart = false;
    bool rayStart = false;
    float rayTime = 0;
    float squareTime = 0;

    public BossStage(StageController controller) : base(controller)
    {
        bossEnemy = Resources.Load("Prefabs/BossStage/Boss") as GameObject;
        circleEnemy = Resources.Load("Prefabs/BossStage/CircleEnemy") as GameObject;
        squareEnemy = Resources.Load("Prefabs/BossStage/SquareEnemy") as GameObject;
        rayEnemy = Resources.Load("Prefabs/BossStage/RayEnemyPos") as GameObject;
        player = GameObject.Find("Player");
    }

    public override void Initialize()
    {
        base.Initialize();
        GetCurrentPlayScreen();
        player = GameObject.Find("Player");
        //patten = Random.Range(0, 5);
        enemyParent = GameObject.Find("Enemy");
        mapParent = GameObject.Find("Map");
        maps.Add(Resources.Load("Prefabs/BossStage/Patten_Map_0") as GameObject);
        maps.Add(Resources.Load("Prefabs/BossStage/Patten_Map_1") as GameObject);

        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        currentMap = Util.CreateObjToParent(maps[patten], new Vector3(0, 0, 0), mapParent);

        player.transform.position = currentMap.transform.Find("PlayerStartPoint").transform.position;
        InitPatten();
        Debug.Log("BossStage Initialize");
    }

    public override void Update()
    {
        base.Update();
        playTime += Time.deltaTime;
        UpdatePatten();
        Debug.Log("BossStage Update");
    }

    public override void Destroy()
    {
        base.Destroy();
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        Debug.Log("BossStage Destroy");
    }

    private void InitPatten()
    {
        switch (patten)
        {
            case 0:
                Transform wayBox = currentMap.transform.Find("BossMovePoint");
                squareEnemy = Util.CreateObjToParent(squareEnemy, new Vector3(0, 3, 0), enemyParent);

                foreach (Transform child in wayBox)
                {
                    bossWayPoint.Add(child);
                }

                bossEnemy = Util.CreateObjToParent(bossEnemy, bossWayPoint[0].position, enemyParent);
                playTime = 5f;
                break;
            case 1:
                Transform bossPos = currentMap.transform.Find("BossStartPoint");
                bossEnemy = Util.CreateObjToParent(bossEnemy, bossPos.position, enemyParent);
                rayEnemy = Util.CreateObjToParent(rayEnemy, new Vector3(0, 0, 0), enemyParent);

                rayImage = rayEnemy.transform.GetChild(0).gameObject;
                rayImage.transform.localScale = new Vector2(screenX, 0.05f);
                rayImage.transform.position = new Vector2(screenX / 2 * -1 + 1, 0);
                rayImage.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);

                rayEffect = rayEnemy.transform.GetChild(1).gameObject;
                rayEffect.transform.localScale = new Vector2(screenX, 2f);
                rayEffect.transform.position = new Vector2(screenX / 2 * -1 + 1, 0);

                rayEnemy.transform.position = bossEnemy.transform.position;
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            default:
                break;
        }
    }

    private void UpdatePatten()
    {
        switch (patten)
        {
            case 0:
                if (moveStart) bossMoveTime += Time.deltaTime;
                PattenZero();
                break;
            case 1:
                if (attackStart) rayTime += Time.deltaTime;
                squareTime += Time.deltaTime;
                PattenOne();
                break;
            case 2:
                PattenTwo();
                break;
            case 3:
                break;
            case 4:
                break;
            default:
                break;
        }
    }

    private void PattenZero()
    {
        if(playTime > 5f)
        {
            playTime = 0;
            moveStart = true;
            bossMoveTime = 1f;
        }
        if (moveStart)
        {
            if(bossMoveTime > 0.6f)
            {
                if (wayCount > 0 && wayCount < 4) SpreadCircleEnemy();
                wayCount++;
                if (wayCount >= bossWayPoint.Count)
                {
                    moveStart = false;
                    bossWayPoint.Reverse();
                    wayCount = 0;
                }

                bossEnemy.transform.DOMove(bossWayPoint[wayCount].position, 0.6f).SetEase(Ease.Linear);
                bossMoveTime = 0;
            }
        }
        GuidedMissile();
    }

    private void SpreadCircleEnemy()
    {
        for (int rad = 0; rad < 360; rad += 20)
        {
            Rigidbody2D spreadEnemy = Util.CreateObjToParent(circleEnemy, bossEnemy.transform.position, enemyParent).GetComponent<Rigidbody2D>();
            spreadEnemy.rotation = rad;
            spreadEnemy.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            spreadEnemy.AddForce(Quaternion.Euler(0, 0, rad) * Vector2.up * 4, ForceMode2D.Impulse);
        }
    }

    private void GuidedMissile()
    {
        if (player == null) return;
        guideDir = (player.transform.position - squareEnemy.transform.position).normalized;
        squareEnemy.transform.right = Vector3.Slerp(squareEnemy.transform.right.normalized, guideDir, Time.deltaTime * 2);
        squareEnemy.transform.Translate(squareEnemy.transform.right.normalized * Time.deltaTime * 10);
    }

    private void PattenOne()
    {
        PattenOneSquare();
        if (playTime > 2f)
        {
            rayImage.SetActive(false);
            attackStart = true;
        }

        if (attackStart)
        {
            if(rayTime > 1 && !rayStart)
            {
                rayEffect.SetActive(true);
                rayStart = true;
            }

            if(rayStart && !rayEffect.activeSelf)
            {
                attackStart = false;
                rayStart = false;
                rayTime = 0;
                playTime = 0;
                rayImage.SetActive(true);
            }
        }
        else
        {
            Vector2 moveDir = new Vector2(0, player.transform.position.y - bossEnemy.transform.position.y);
            rayEnemy.transform.Translate(moveDir * Time.deltaTime * 2);
            bossEnemy.transform.Translate(moveDir * Time.deltaTime * 2);
        }
    }

    private void PattenOneSquare()
    {
        if (squareTime > 0.3f)
        {
            squareTime = 0;
            float defineY = Random.Range(Mathf.RoundToInt(screenY / 2 * -1 + 1), Mathf.RoundToInt(screenY / 2 - 1));
            GameObject currentEnemy = Util.CreateObjToParent(squareEnemy, new Vector2(screenX / 2 + 1, defineY), enemyParent);
            currentEnemy.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            currentEnemy.GetComponent<Rigidbody2D>().velocity = Vector3.left * 8;
        }
    }

    private void PattenTwo()
    {

    }

    private void GetCurrentPlayScreen()
    {
        screenY = Camera.main.orthographicSize * 2;
        screenX = screenY / Screen.height * Screen.width;
    }
}
