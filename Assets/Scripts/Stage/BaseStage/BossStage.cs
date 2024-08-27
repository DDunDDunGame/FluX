using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossStage : BaseStage
{
    GameObject bossEnemy;
    GameObject circleEnemy;
    GameObject noCircleEnemy;
    GameObject squareEnemy;
    GameObject rayEnemy;
    GameObject player;
    GameObject enemyParent;
    GameObject mapParent;
    GameObject currentMap;

    int patten = 4;
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

    // patten 2
    int currentWay = 0;
    List<Transform> squarePoints = new List<Transform>();
    List<GameObject> squareEnemys = new List<GameObject>();
    int countSquare = 0;
    bool squareStart = false;

    // patten 3
    List<GameObject> rayEnemys = new List<GameObject>();
    int attackCount = 0;
    int rayCount = 0;

    // patten 4
    List<GameObject> circleEnemys = new List<GameObject>();
    List<Vector3> jumpPos = new List<Vector3>();
    int currentJump = 0;
    float spreadCircleDelayTime = 0.5f;

    public BossStage(StageController controller) : base(controller)
    {
        RePrefabs();
    }

    public override void Initialize()
    {
        base.Initialize();
        RePrefabs();
        GetCurrentPlayScreen();
        Reset();
        patten = Random.Range(0, 5);
        currentMap = Resources.Load("Prefabs/BossStage/Patten_Map_" + patten.ToString()) as GameObject;

        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        currentMap = Util.CreateObjToParent(currentMap, new Vector3(0, 0, 0), mapParent);

        player.transform.position = currentMap.transform.Find("PlayerStartPoint").transform.position;
        InitPatten();

        controller.Player.OnBottomHit -= PlayerDropCheck;
        controller.Player.OnBottomHit += PlayerDropCheck;
    }

    public override void Update()
    {
        base.Update();
        playTime += Time.deltaTime;
        UpdatePatten();
    }

    public override void Destroy()
    {
        base.Destroy();
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        Util.DestoryObjFromParent(enemyParent);
        Util.DestoryObjFromParent(mapParent);
        controller.Player.OnBottomHit -= PlayerDropCheck;
    }

    private void RePrefabs()
    {
        bossEnemy = Resources.Load("Prefabs/BossStage/Boss") as GameObject;
        circleEnemy = Resources.Load("Prefabs/BossStage/CircleEnemy") as GameObject;
        squareEnemy = Resources.Load("Prefabs/BossStage/SquareEnemy") as GameObject;
        noCircleEnemy = Resources.Load("Prefabs/BossStage/CircleEnemyNoEffect") as GameObject;
        player = GameObject.Find("Player");
        enemyParent = GameObject.Find("Enemy");
        mapParent = GameObject.Find("Map");
    }

    private void Reset()
    {
        bossWayPoint.Clear();
        squarePoints.Clear();
        squareEnemys.Clear();
        circleEnemys.Clear();
        rayEnemys.Clear();
        jumpPos.Clear();
        wayCount = 0;
        bossMoveTime = 0;
        moveStart = false;
        attackStart = false;
        rayStart = false;
        rayTime = 0;
        squareTime = 0;
        countSquare = 0;
        squareStart = false;
        currentWay = 0;
        attackCount = 0;
        rayCount = 0;
        currentJump = 0;
        spreadCircleDelayTime = 0.5f;
    }

    private void InitPatten()
    {
        Transform wayBox;
        switch (patten)
        {
            case 0:
                wayBox = currentMap.transform.Find("BossMovePoint");
                //squareEnemy = Util.CreateObjToParent(squareEnemy, new Vector3(0, 3, 0), enemyParent);

                foreach (Transform child in wayBox)
                {
                    bossWayPoint.Add(child);
                }

                bossEnemy = Util.CreateObjToParent(bossEnemy, bossWayPoint[0].position, enemyParent);
                playTime = 5f;
                break;
            case 1:
                rayEnemy = Resources.Load("Prefabs/BossStage/Patten_1_RayEnemyPos") as GameObject;
                Transform bossPos = currentMap.transform.Find("BossStartPoint");
                bossEnemy = Util.CreateObjToParent(bossEnemy, bossPos.position, enemyParent);
                rayEnemy = Util.CreateObjToParent(rayEnemy, new Vector3(0, 0, 0), enemyParent);

                rayImage = rayEnemy.transform.GetChild(0).gameObject;
                rayImage.transform.localScale = new Vector2(1, 0.05f);

                rayEffect = rayEnemy.transform.GetChild(1).gameObject;
                rayEffect.transform.localScale = new Vector2(1, 1);

                rayEnemy.transform.position = bossEnemy.transform.position;
                break;
            case 2:
                rayEnemy = Resources.Load("Prefabs/BossStage/Patten_2_RayEnemy") as GameObject;
                wayBox = currentMap.transform.Find("BossMovePoint");

                foreach (Transform child in wayBox)
                {
                    bossWayPoint.Add(child);
                }

                wayBox = currentMap.transform.Find("SquarePoint");
                foreach (Transform child in wayBox)
                {
                    squarePoints.Add(child);
                }

                bossEnemy = Util.CreateObjToParent(bossEnemy, bossWayPoint[0].position, enemyParent);
                rayEnemy = Util.CreateObjToParent(rayEnemy, bossEnemy.transform.position, bossEnemy);

                break;
            case 3:
                wayBox = currentMap.transform.Find("BossMovePoint");
                currentWay = 0;
                foreach (Transform child in wayBox)
                {
                    bossWayPoint.Add(child);
                }

                wayBox = currentMap.transform.Find("DropRays");
                foreach(Transform child in wayBox)
                {
                    rayEnemys.Add(child.gameObject);
                }

                bossEnemy = Util.CreateObjToParent(bossEnemy, bossWayPoint[0].position, enemyParent);
                bossEnemy.transform.localScale = new Vector3(1, 1, 1);
                moveStart = true;
                break;
            case 4:
                rayEnemy = Resources.Load("Prefabs/BossStage/Patten_1_RayEnemyPos") as GameObject;
                rayEnemy = Util.CreateObjToParent(rayEnemy, new Vector3(0, 0, 0), enemyParent);
                rayEnemy.SetActive(false);
                rayImage = rayEnemy.transform.GetChild(0).gameObject;
                rayImage.transform.localScale = new Vector2(0.05f, screenY * 1.5f);
                rayImage.transform.position = new Vector2(0, screenY/4);
                rayImage.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);

                wayBox = currentMap.transform.Find("BossMovePoint");
                foreach(Transform child in wayBox)
                {
                    bossWayPoint.Add(child);
                }
                bossEnemy = Util.CreateObjToParent(bossEnemy, bossWayPoint[0].position, enemyParent);
                bossEnemy.transform.localScale = new Vector3(2, 2, 2);
                currentWay = 1;
                bossEnemy.transform.DOMove(bossWayPoint[currentWay].position, 4).SetEase(Ease.Linear);
                attackStart = true;
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
                rayTime += Time.deltaTime;
                PattenThree();
                break;
            case 4:
                PattenFour();
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
        //GuidedMissile();
    }

    private void SpreadCircleEnemy()
    {
        for (int rad = 0; rad < 360; rad += 20)
        {
            Rigidbody2D spreadEnemy = Util.CreateObjToParent(circleEnemy, bossEnemy.transform.position, enemyParent).GetComponent<Rigidbody2D>();
            spreadEnemy.rotation = rad + 270;
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
        // 움직임
        if (8 > currentWay)
        {
            if (bossEnemy.transform.position == bossWayPoint[currentWay].transform.position)
            {
                currentWay++;
                if (8 == currentWay)
                {
                    playTime = 0;
                    return;
                }
                if (rayEnemy.activeSelf) rayEnemy.SetActive(false);
                if (currentWay == 5)
                {
                    rayEnemy.SetActive(true);
                    rayEnemy.transform.rotation = Quaternion.Euler(0, 0, 90);
                    rayEnemy.transform.DOScaleX(0.6f, 0.3f).SetEase(Ease.Linear);
                    bossEnemy.transform.DOMove(bossWayPoint[currentWay].transform.position, 2).SetEase(Ease.Linear);
                }
                else
                {
                    bossEnemy.transform.DOMove(bossWayPoint[currentWay].transform.position, 0.3f).SetEase(Ease.Linear);

                    if (currentWay == 3)
                    {
                        playTime = 0;
                        squareStart = true;
                    }
                }
            }
        }
        else if(bossWayPoint.Count > currentWay)
        {
            if(playTime > 2)
            {
                if(bossEnemy.transform.position == bossWayPoint[currentWay-1].transform.position)
                {
                    bossEnemy.transform.DOMove(bossWayPoint[currentWay].position, 0.6f).SetEase(Ease.Linear);
                    currentWay++;
                }
            }
        }
        PattenTwoSquare();
    }

    private void PattenTwoSquare()
    {
        if(squareStart && playTime > 0.2f)
        {
            if(countSquare  == 9)
            {
                squareStart = false;
                return;
            }

            GameObject currentEnemy = Util.CreateObjToParent(squareEnemy, new Vector3((screenX / 2 + 1) * -1, squarePoints[countSquare % 3].transform.position.y, 0), enemyParent);
            squareEnemys.Add(currentEnemy);
            currentEnemy.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            currentEnemy.GetComponent<Rigidbody2D>().velocity = Vector2.right * 8;
            countSquare++;
            playTime = 0;
        }
        foreach(GameObject child in squareEnemys)
        {
            child.transform.Rotate(new Vector3(0, 0, 1) * 360f * Time.deltaTime);
        }
    }

    private void PattenThree()
    {
        PattenThreeDropRay();
        if (moveStart)
        {
            currentWay++;
            if (currentWay == bossWayPoint.Count)
            {
                moveStart = false;
                return;
            }
            bossEnemy.transform.DOMove(bossWayPoint[currentWay].transform.position, 1.5f).SetEase(Ease.Linear);
            moveStart = false;
            attackStart = true;
        }

        if (attackStart && bossEnemy.transform.position == bossWayPoint[currentWay].transform.position)
        {
            if (playTime > 0.5f)
            {
                playTime = 0;
                foreach(GameObject child in squareEnemys)
                {
                    child.SetActive(false);
                }
                squareEnemys.Clear();
                GameObject currentEnemy = Util.CreateObjToParent(squareEnemy, bossEnemy.transform.position, enemyParent);
                currentEnemy.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                currentEnemy.GetComponent<Rigidbody2D>().velocity = Vector2.right * 8;
                currentEnemy.GetComponent<SpriteRenderer>().DOFade(0, 0.5f).SetEase(Ease.Linear);
                squareEnemys.Add(currentEnemy);

                currentEnemy = Util.CreateObjToParent(squareEnemy, bossEnemy.transform.position, enemyParent);
                currentEnemy.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                currentEnemy.GetComponent<Rigidbody2D>().velocity = Vector2.left * 8;
                currentEnemy.GetComponent<SpriteRenderer>().DOFade(0, 0.5f).SetEase(Ease.Linear);
                squareEnemys.Add(currentEnemy);

                attackCount++;
                if (currentWay % 2 == 0 && attackCount == 2)
                {
                    moveStart = true;
                    attackCount = 0;
                    attackStart = false;
                }
                else if(currentWay % 2 == 1 && attackCount == 4)
                {
                    moveStart = true;
                    attackCount = 0;
                    attackStart = false;
                }
            }
        }
    }

    private void PattenThreeDropRay()
    {
        if(rayCount < 3)
        {
            if(rayTime > 0.5f)
            {
                rayTime = 0;
                rayEnemys[rayCount].SetActive(true);
                rayEnemys[rayCount].transform.DOMoveY(screenY/4 - 0.5f, 1).SetEase(Ease.Linear);
                rayCount++;
            }
        }
        else
        {
            if(rayTime > 2f)
            {
                rayTime = 0;
                rayEnemys.Reverse();
                rayCount = 0;
            }
        }
    }

    private void PattenFour()
    {
        if (currentWay == 1 && bossEnemy.transform.position == bossWayPoint[currentWay].position)
        {
            currentWay++;
            attackStart = false;
            moveStart = true;
            rayEnemy.SetActive(true);
            playTime = 0;
        }

        if (attackStart)
        {
            PattenFourSpreadCircle();
        }

        if (moveStart)
        {
            if(playTime > 2f)
            {
                jumpPos.Clear();
                moveStart = false;
                playTime = 0;
                spreadCircleDelayTime = 0.2f;
                attackStart = true;
                rayEnemy.SetActive(false);
                jumpPos.Add(new Vector2(bossEnemy.transform.position.x, screenY / 4));
                jumpPos.Add(bossEnemy.transform.position);
                bossEnemy.transform.DOMove(jumpPos[currentJump], 1f).SetEase(Ease.OutCubic);
            }
            Vector2 moveDir = new Vector2(player.transform.position.x - bossEnemy.transform.position.x, 0);
            rayEnemy.transform.Translate(moveDir * 2 * Time.deltaTime);
            bossEnemy.transform.Translate(moveDir * 2 * Time.deltaTime);
        }

        if(jumpPos.Count != 0 && bossEnemy.transform.position == jumpPos[currentJump])
        {
            if(currentJump == 1)
            {
                attackStart = false;
                return;
            }
            else
            {
                currentJump++;
                bossEnemy.transform.DOMove(jumpPos[currentJump], 1f).SetEase(Ease.OutCubic);
            }
        }
    }

    private void PattenFourSpreadCircle()
    {
        if(playTime > spreadCircleDelayTime)
        {
            foreach (GameObject child in circleEnemys)
            {
                child.GetComponent<Rigidbody2D>().gravityScale = 2f;
            }
            circleEnemys.Clear();

            for(int enemyCount = 0; enemyCount < 4; ++enemyCount)
            {
                GameObject currentEnemy = Util.CreateObjToParent(noCircleEnemy, bossEnemy.transform.position, enemyParent);
                currentEnemy.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                circleEnemys.Add(currentEnemy);
            }

            int defineDir = 0;
            foreach(GameObject child in circleEnemys)
            {
                Rigidbody2D childRigid = child.GetComponent<Rigidbody2D>();
                childRigid.gravityScale = 0.5f;
                if (defineDir % 2 == 0)
                {
                    if(defineDir >= 2) childRigid.AddForce((Vector2.right*1.4f + Vector2.up).normalized * 4, ForceMode2D.Impulse);
                    else childRigid.AddForce(Vector2.right * 4, ForceMode2D.Impulse);
                }
                else
                {
                    if (defineDir >= 2) childRigid.AddForce((Vector2.left*1.4f + Vector2.up).normalized * 4, ForceMode2D.Impulse);
                    else childRigid.AddForce(Vector2.left * 4, ForceMode2D.Impulse);
                }
                defineDir++;
            }
            playTime = 0;
        }
    }

    private void GetCurrentPlayScreen()
    {
        screenY = Camera.main.orthographicSize * 2;
        screenX = screenY / Screen.height * Screen.width;
    }

    private void PlayerDropCheck()
    {
        controller.Player.transform.position = new Vector2(currentMap.transform.Find("PlayerStartPoint").transform.position.x, 4.8f);
        controller.Player.Rigid.velocity = new Vector2(controller.Player.Rigid.velocity.x, 0);
    }
}
