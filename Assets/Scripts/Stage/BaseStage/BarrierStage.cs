using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BarrierStage : BaseStage
{
    GameObject barrierPrefab;
    GameObject barrier;
    GameObject barrierRange;
    GameObject player;
    GameObject circleEnemy;
    GameObject rayEnemy;
    GameObject redRayEnemy;
    GameObject enemyParent;

    float screenX;
    float screenY;
    int patten = 1;

    float playTime = 0;
    bool isBarrier = false;
    List<GameObject> redRays = new List<GameObject>();

    public BarrierStage(StageController controller) : base(controller)
    {
        barrierPrefab = Resources.Load("Prefabs/BarrierStage/Barrier") as GameObject;
        circleEnemy = Resources.Load("Prefabs/BarrierStage/CircleEnemy") as GameObject;
        rayEnemy = Resources.Load("Prefabs/BarrierStage/RayEnemy") as GameObject;
        redRayEnemy = Resources.Load("Prefabs/BarrierStage/RedRayEnemy") as GameObject;
        barrierRange = Resources.Load("Prefabs/BarrierStage/BarrierRange") as GameObject;
        player = controller.Player.gameObject;
        enemyParent = GameObject.Find("Enemy");
    }

    public override void Initialize()
    {
        base.Initialize();
        player.transform.position = new Vector3(0, 0, 0);
        foreach(Transform child in player.transform)
        {
            if (child.name.Contains("Barrier"))
            {
                barrier = child.gameObject;
                child.gameObject.SetActive(true);
                isBarrier = true;
            }
        }
        if (!isBarrier) barrier = Util.CreateObjToParent(barrier, new Vector3(0, 1.25f, 0), player);
        barrierRange = Util.CreateObjToParent(barrierRange, new Vector3(0, 0, 0), enemyParent);
        barrier.SetActive(true);
        barrier.transform.SetPositionAndRotation(new Vector3(0, 1.25f, 0), Quaternion.identity);
        InitPatten();
        patten = 1;
        //patten = Random.Range(0, 3);
        GetCurrentPlayScreen();
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
        barrier.SetActive(false);
        Util.DestoryObjFromParent(enemyParent);
    }

    private void GetCurrentPlayScreen()
    {
        screenY = Camera.main.orthographicSize * 2;
        screenX = screenY / Screen.height * Screen.width;
    }

    private void InitPatten()
    {
        switch (patten)
        {
            case 0:
                playTime = 3;
                redRays.Clear();
                break;
            case 1:
                break;
            case 2:
                playTime = 3;
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

    // 랜덤 레이저 지이이이이잉이이이잉
    private void PattenZero()
    {
        if(playTime > 4f)
        {
            // 4방향
            playTime = 0;
            Vector2 spawnDir = new Vector2(0, 0);
            int getAxis = 0;
            do
            {
                spawnDir = new Vector2(0, 0);
                getAxis = Random.Range(0, 2);
                spawnDir[getAxis] = Random.Range(-1, 2);
            } while (spawnDir[getAxis] == 0);
            Vector2 spawnPos = new Vector2(spawnDir.x * screenX/2, spawnDir.y * screenY/2);
            GameObject currentEnemy = Util.CreateObjToParent(redRayEnemy, spawnPos, enemyParent);
            if (getAxis == 1) currentEnemy.transform.localRotation = Quaternion.Euler(0, 0, 90);
            currentEnemy.GetComponent<Rigidbody2D>().velocity = spawnDir.normalized * -8;
            redRays.Add(currentEnemy);
            // 8방향
            //playTime = 0;
            //Vector2 spawnDir;
            //do
            //{
            //    spawnDir = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)).normalized;
            //}while(spawnDir.x == 0 && spawnDir.y == 0);
            //Vector2 spawnPos = new Vector2(spawnDir.x * screenX / 2 + 2, spawnDir.y * screenX / 2 + 2);
            //GameObject currentEnemy = Util.CreateObjToParent(redRayEnemy, spawnPos, enemyParent);
            //// 회전
            //Vector3 tmp = player.transform.position - currentEnemy.transform.position;
            //float directionPlayerRot = Mathf.Atan2(tmp.y, tmp.x) * Mathf.Rad2Deg;
            //float rot = Mathf.LerpAngle(currentEnemy.transform.eulerAngles.z, directionPlayerRot, 50);
            //currentEnemy.transform.eulerAngles = new Vector3(0, 0, rot);
            //// 크기
            //currentEnemy.transform.localScale = new Vector3(0.15f, screenX, 1);
            //currentEnemy.GetComponent<Rigidbody2D>().velocity = spawnDir * -8;
        }
        CheckRayVelocity();

       void CheckRayVelocity()
       {
            foreach(GameObject child in redRays)
            {
                float checkX = Mathf.Abs(child.transform.position.x) - player.transform.localScale.x;
                float checkY = Mathf.Abs(child.transform.position.y) - player.transform.localScale.y;
                Rigidbody2D currentRigid = child.GetComponent<Rigidbody2D>();
                if (checkX > 0 || checkY > 0)
                {
                    currentRigid.velocity = currentRigid.velocity.normalized * 8;
                }
                else
                {
                    currentRigid.velocity = currentRigid.velocity.normalized * 1;
                }
            }     
       }
    }

    // 랜덤 총알 날라오기
    private void PattenOne()
    {
        if(playTime > 1f)
        {
            playTime = 0;
            Vector2 spawnDir;
            do
            {
                spawnDir = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)).normalized;
            } while (spawnDir.x == 0 && spawnDir.y == 0);
            Vector2 spawnPos = new Vector2(spawnDir.x * screenX/2, spawnDir.y * screenX/2);
            GameObject currentEnemy = Util.CreateObjToParent(circleEnemy, spawnPos, enemyParent);
            Vector2 dir = (player.transform.position - currentEnemy.transform.position).normalized * -1;
            currentEnemy.transform.right = Vector3.Slerp(currentEnemy.transform.right.normalized, dir, 360);
            currentEnemy.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            currentEnemy.GetComponent<Rigidbody2D>().velocity = spawnDir * -8;
        }
    }

    // 이.거.절.대.못.막.습.니.다.
    private void PattenTwo()
    {
        if (playTime > 2f)
        {
            // 4방향 레이저 발사
            playTime = 0;
            Vector2 spawnDir = new Vector2(0, 0);
            int getAxis = 0;
            do
            {
                spawnDir = new Vector2(0, 0);
                getAxis = Random.Range(0, 2);
                spawnDir[getAxis] = Random.Range(-1, 2);
            } while (spawnDir[getAxis] == 0);
            Vector2 spawnPos = new Vector2(spawnDir.x * screenX, spawnDir.y * screenY);
            GameObject currentEnemy = Util.CreateObjToParent(rayEnemy, spawnPos, enemyParent);
            currentEnemy.transform.right = Vector3.Slerp(currentEnemy.transform.right, spawnDir * -1, 360);
            currentEnemy.GetComponent<Rigidbody2D>().velocity = spawnDir.normalized * -8;

            // ALL 랜덤 레이저 발사
            //playTime = 0;
            //Vector2 spawnDir = new Vector2(0, 0);
            //do
            //{
            //    spawnDir = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)).normalized;
            //} while (spawnDir.x == 0 && spawnDir.y == 0);
            //Vector2 spawnPos = new Vector2(spawnDir.x * screenX, spawnDir.y * screenY);
            //GameObject currentEnemy = Util.CreateObjToParent(rayEnemy, spawnPos, enemyParent);
            //if (getAxis == 1) currentEnemy.GetComponent<Rigidbody2D>().rotation = 90;
            //currentEnemy.transform.localScale = new Vector3(screenX / 2, 0.5f, 1);
            //currentEnemy.GetComponent<Rigidbody2D>().velocity = spawnDir.normalized * -8;
        }
    }
}
