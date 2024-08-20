using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OrbitStage : BaseStage
{
    GameObject mainCircle;
    GameObject player;
    GameObject enemyParent;
    GameObject rayEnemy;
    GameObject squareEnemy;
    GameObject circleEnemy;

    int patten = 2;
    float playTime = 0;
    float screenX;
    float screenY;

    List<GameObject> rayEnemys = new List<GameObject>();
    List<GameObject> circleEnemys = new List<GameObject>();

    public OrbitStage(StageController controller) : base(controller)
    {
        mainCircle = Resources.Load("Prefabs/Orbit/TempOrbit") as GameObject;
        rayEnemy = Resources.Load("Prefabs/Orbit/RayEnemy") as GameObject;
        squareEnemy = Resources.Load("Prefabs/Orbit/SquareEnemy") as GameObject;
        circleEnemy = Resources.Load("Prefabs/Orbit/CircleEnemy") as GameObject;
        player = GameObject.Find("Player");
    }

    public override void Initialize()
    {
        base.Initialize();
        mainCircle = controller.CreateMap(mainCircle, new Vector3(0, 0, 0));
        player.transform.position = mainCircle.transform.position + new Vector3(0, -1.5f, 0);
        player.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        enemyParent = new GameObject("Enemy");
        GetCurrentPlayScreen();

        //patten = Random.Range(0, 3);

        InitPatten();
        Debug.Log("OrbitStage Initialize");
    }

    public override void Update()
    {
        base.Update();
        playTime += Time.deltaTime;
        UpdatePatten();
        //Debug.Log("OrbitStage Update");
    }

    public override void Destroy()
    {
        base.Destroy();
        Util.Destroy(enemyParent);
        player.transform.localScale = new Vector3(1f, 1f, 1f);
        Debug.Log("OrbitStage Destroy");
    }

    private void InitPatten()
    {
        rayEnemys.Clear();
        circleEnemys.Clear();
        playTime = 0;
        switch (patten)
        {
            case 0:

                Vector2 spawnDir;
                for(int x = -1; x < 2; x++)
                {
                    for(int y = -1; y < 2; y++)
                    {
                        if (y == 0 && x == 0) continue;
                        spawnDir = new Vector2(x, y).normalized;
                        Vector2 spawnPos = new Vector2(spawnDir.x * mainCircle.transform.localScale.x/2, spawnDir.y * mainCircle.transform.localScale.y/2);
                        GameObject currentEnemy = Util.CreateObjToParent(rayEnemy, spawnPos, enemyParent);
                        // 회전
                        Vector3 tmp = mainCircle.transform.position - currentEnemy.transform.position;
                        float directionPlayerRot = Mathf.Atan2(tmp.y, tmp.x) * Mathf.Rad2Deg;
                        float rot = Mathf.LerpAngle(currentEnemy.transform.eulerAngles.z, directionPlayerRot, 50);
                        currentEnemy.transform.eulerAngles = new Vector3(0, 0, rot);
                        // 크기
                        currentEnemy.transform.localScale = new Vector3(3f, screenX * 1.5f, 1);
                        rayEnemys.Add(currentEnemy);
                    }
                }
                break;
            case 1:
                break;
            case 2:
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
                PattenZero();
                break;
            case 1:
                PattenOne();
                break;
            case 2:
                PattenTwo();
                break;
            default:
                break;
        }
    }

    private void GetCurrentPlayScreen()
    {
        screenY = Camera.main.orthographicSize * 2;
        screenX = screenY / Screen.height * Screen.width;
    }

    private void PattenZero()
    {
        if(playTime > 1.5f)
        {
            playTime = 0;
            GameObject currentEnemy;
            do
            {
                currentEnemy = rayEnemys[Random.Range(0, rayEnemys.Count)];
            } while (currentEnemy.activeSelf);
            currentEnemy.SetActive(true);
        }
    }

    private void PattenOne()
    {
        // 소환되는 속도 조절
        if(playTime > 0.4f)
        {
            playTime = 0;
            int  sliceMax = 4;
            GameObject currentEnemy;
            float sliceY = mainCircle.transform.localScale.y / (sliceMax-1);
            for (int spawnDefine = 0; spawnDefine < 2; ++spawnDefine)
            {
                int defineY = Random.Range(0, sliceMax);
                Vector2 spawnPos = new Vector2(spawnDefine == 0 ? (screenX / 2 + 1) * -1 : screenX / 2 + 1, mainCircle.transform.localScale.y/2 - (sliceY * defineY));
                currentEnemy = Util.CreateObjToParent(squareEnemy, spawnPos, enemyParent);
                currentEnemy.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                currentEnemy.GetComponent<Rigidbody2D>().velocity = new Vector2(spawnDefine == 0 ? 1 : -1, 0) * 4;
            }
            
        }
    }

    private void PattenTwo()
    {
        if(playTime > 1f)
        {
            int rotDir = Random.Range(0, 4);
            GameObject currentEnemy = Util.CreateObjToParent(circleEnemy, new Vector3(0, 0, 0), enemyParent);
            currentEnemy.transform.Rotate(new Vector3(0, 0, 1) * (90 * rotDir));
            circleEnemys.Add(currentEnemy);
            currentEnemy.transform.localScale = new Vector3(screenX * 1.5f, screenX * 1.5f, 1);
            currentEnemy.SetActive(true);
            currentEnemy.transform.DOScale(mainCircle.transform.localScale, 2f);
            playTime = 0;
        }

        foreach(GameObject child in circleEnemys)
        {
            if(child.transform.localScale == mainCircle.transform.localScale)
            {
                circleEnemys.Remove(child);
                child.SetActive(false);
                break;
            }
        }
    }
}
