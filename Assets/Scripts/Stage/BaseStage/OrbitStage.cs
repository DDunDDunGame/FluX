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

    int patten = 0;
    float playTime = 0;
    float screenX;
    float screenY;

    List<GameObject> rayEnemys = new List<GameObject>();

    public OrbitStage(StageController controller) : base(controller)
    {
        mainCircle = Resources.Load("Prefabs/Orbit/TempOrbit") as GameObject;
        rayEnemy = Resources.Load("Prefabs/Orbit/RayEnemy") as GameObject;
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
                break;
            case 2:
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

    }

    private void PattenTwo()
    {

    }
}
