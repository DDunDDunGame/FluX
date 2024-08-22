using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OrbitStage : BaseStage
{
    GameObject mainCirclePrefab;
    GameObject mainCircle;
    GameObject player;
    GameObject enemyParent;
    GameObject rayEnemy;
    GameObject squareEnemy;
    GameObject circleEnemy;

    int patten = 0;
    float playTime = 0;
    float screenX;
    float screenY;

    List<GameObject> rayEnemys = new List<GameObject>();
    List<GameObject> circleEnemys = new List<GameObject>();

    public OrbitStage(StageController controller) : base(controller)
    {
        mainCirclePrefab = Resources.Load("Prefabs/Orbit/TempOrbit") as GameObject;
        rayEnemy = Resources.Load("Prefabs/Orbit/RayEnemy") as GameObject;
        squareEnemy = Resources.Load("Prefabs/Orbit/SquareEnemy") as GameObject;
        circleEnemy = Resources.Load("Prefabs/Orbit/CircleEnemy") as GameObject;
        enemyParent = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
    }

    public override void Initialize()
    {
        base.Initialize();
        mainCircle = controller.CreateMap(mainCirclePrefab, new Vector3(0, 0, 0));
        player.transform.position = new Vector3(0, -1.5f, 0);
        GetCurrentPlayScreen();

        patten = Random.Range(0, 3);

        InitPatten();
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
        Util.DestoryObjFromParent(enemyParent);
    }

    private void InitPatten()
    {
        rayEnemys.Clear();
        circleEnemys.Clear();
        playTime = 0;
        switch (patten)
        {
            case 0:
                GameObject rayParent = Resources.Load("Prefabs/Orbit/RayEnemys") as GameObject;
                rayParent = Util.CreateObjToParent(rayParent, new Vector3(0, 0, 0), enemyParent);
                foreach(Transform child in rayParent.transform)
                {
                    rayEnemys.Add(child.gameObject);
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
                OrbitCheckEnter currentOrbit = child.transform.GetChild(0).transform.GetComponent<OrbitCheckEnter>();
                if (!currentOrbit.checkEnterPlayer)
                {
                    player.GetComponent<Player>().TakeDamage(5);
                }
                child.SetActive(false);
                break;
            }
        }
    }
}
