using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BarrierStage : BaseStage
{
    GameObject barrier;
    BarrierEffect barrierEffect;
    GameObject barrierRange;
    GameObject player;
    GameObject circleEnemy;
    GameObject rayEnemy;
    GameObject redRayEnemy;
    GameObject enemyParent;
    GameObject rayPos;
    GameObject visualLineObj;
    LineRender visualLine;

    float screenX;
    float screenY;
    int patten = 0;

    bool init = false;
    float playTime = 0;
    bool isBarrier = false;
    List<GameObject> redRays = new List<GameObject>();

    public BarrierStage(StageController controller) : base(controller)
    {
        RePrefabs();
        player = controller.Player.gameObject;
    }

    public override void Initialize()
    {
        base.Initialize();
        RePrefabs();
        barrierRange = Resources.Load("Prefabs/BarrierStage/BarrierRange") as GameObject;
        visualLine = Util.CreateObjToParent(visualLineObj, new Vector3(0, 0, 0), enemyParent).GetComponent<LineRender>();
        player.transform.position = new Vector3(0, 0, 0);
        foreach (Transform child in player.transform)
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
        barrierEffect = barrier.GetComponent<BarrierEffect>();
        patten = Random.Range(0, 2);
        barrier.transform.SetPositionAndRotation(new Vector3(0, 1.25f, 0), Quaternion.identity);
        InitPatten();
        GetCurrentPlayScreen();
    }

    public override void Update()
    {
        if (init)
        {
            controller.Player.Rigid.velocity = Vector2.zero;
            player.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
            init = false;
        }
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

    private void RePrefabs()
    {
        barrier = Resources.Load("Prefabs/BarrierStage/Barrier") as GameObject;
        circleEnemy = Resources.Load("Prefabs/BarrierStage/CircleEnemy") as GameObject;
        rayEnemy = Resources.Load("Prefabs/BarrierStage/RayEnemy") as GameObject;
        redRayEnemy = Resources.Load("Prefabs/BarrierStage/RedRayEnemy") as GameObject;
        barrierRange = Resources.Load("Prefabs/BarrierStage/BarrierRange") as GameObject;
        visualLineObj = Resources.Load("Prefabs/BarrierStage/VisualLine") as GameObject;
        rayPos = Resources.Load("Prefabs/BarrierStage/RayStartPos") as GameObject;
        enemyParent = GameObject.Find("Enemy");
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
        if (playTime > 4f)
        {
            // 4방향
            playTime = 0;
            int spawnDir = Random.Range(0, 4);
            GameObject currentEnemy = null;
            switch (spawnDir)
            {
                case 0: // 위 -> 아래
                    currentEnemy = Util.CreateObjToParent(rayPos, new Vector3(screenX / 2 * -1, screenY / 2, 0), enemyParent);
                    currentEnemy.GetComponent<Rigidbody2D>().velocity = Vector2.down * 6;
                    break;
                case 1: // 아래 -> 위
                    currentEnemy = Util.CreateObjToParent(rayPos, new Vector3(screenX / 2, screenY / 2 * -1, 0), enemyParent);
                    currentEnemy.GetComponent<Rigidbody2D>().velocity = Vector2.up * 6;
                    break;
                case 2: // 오른 -> 왼
                    currentEnemy = Util.CreateObjToParent(rayPos, new Vector3(screenX / 2, screenY / 2, 0), enemyParent);
                    currentEnemy.GetComponent<Rigidbody2D>().velocity = Vector2.right * -6;
                    break;
                case 3: // 왼 -> 오른
                    currentEnemy = Util.CreateObjToParent(rayPos, new Vector3(screenX / 2 * -1, screenY / 2 * -1, 0), enemyParent);
                    currentEnemy.GetComponent<Rigidbody2D>().velocity = Vector2.right * 6;
                    break;
            }
            redRays.Add(currentEnemy);
        }
        CheckRayVelocity();

        void CheckRayVelocity()
        {
            foreach (GameObject child in redRays)
            {
                float checkX = Mathf.Abs(child.transform.position.x) - (player.transform.localScale.x);
                float checkY = Mathf.Abs(child.transform.position.y) - (player.transform.localScale.y);
                Rigidbody2D currentRigid = child.GetComponent<Rigidbody2D>();
                if (checkX > 0 && checkY > 0)
                {
                    currentRigid.velocity = currentRigid.velocity.normalized * 6;
                }
                else
                {
                    currentRigid.velocity = currentRigid.velocity.normalized * 1;
                }

                if (currentRigid.velocity.normalized == Vector2.up)
                {
                    RaycastHit2D hitBarrier = Physics2D.Raycast(child.transform.position, Vector2.right * -1, Mathf.Infinity, LayerMask.GetMask("Barrier"));
                    RaycastHit2D hitPlayer = Physics2D.Raycast(child.transform.position, Vector2.right * -1, Mathf.Infinity, LayerMask.GetMask("Player", "Barrier"));
                    if (hitPlayer.collider != null && hitPlayer.collider.gameObject == player)
                    {
                        hitPlayer.transform.GetComponent<Player>().TakeDamage(5);
                        visualLine.Play(child.transform.position, hitPlayer.point);
                        barrierEffect.RayBarrierEffect.Stop();
                    }
                    else if (hitBarrier)
                    {
                        visualLine.Play(child.transform.position, hitBarrier.point);
                        barrierEffect.RayBarrierEffect.Play();
                    }
                    else
                    {
                        visualLine.Play(child.transform.position, new Vector2(child.transform.position.x * -1, child.transform.position.y));
                        barrierEffect.RayBarrierEffect.Stop();
                    }
                }
                else if (currentRigid.velocity.normalized == Vector2.down)
                {
                    RaycastHit2D hitBarrier = Physics2D.Raycast(child.transform.position, Vector2.right, Mathf.Infinity, LayerMask.GetMask("Barrier"));
                    RaycastHit2D hitPlayer = Physics2D.Raycast(child.transform.position, Vector2.right, Mathf.Infinity, LayerMask.GetMask("Player", "Barrier"));
                    if (hitPlayer.collider != null && hitPlayer.collider.gameObject == player)
                    {
                        hitPlayer.transform.GetComponent<Player>().TakeDamage(5);
                        visualLine.Play(child.transform.position, hitPlayer.point);
                        barrierEffect.RayBarrierEffect.Stop();
                    }
                    else if (hitBarrier)
                    {
                        visualLine.Play(child.transform.position, hitBarrier.point);
                        barrierEffect.RayBarrierEffect.Play();
                    }
                    else
                    {
                        visualLine.Play(child.transform.position, new Vector2(child.transform.position.x * -1, child.transform.position.y));
                        barrierEffect.RayBarrierEffect.Stop();
                    }
                }
                else if (currentRigid.velocity.normalized == Vector2.right)
                {
                    RaycastHit2D hitBarrier = Physics2D.Raycast(child.transform.position, Vector2.up, Mathf.Infinity, LayerMask.GetMask("Barrier"));
                    RaycastHit2D hitPlayer = Physics2D.Raycast(child.transform.position, Vector2.up, Mathf.Infinity, LayerMask.GetMask("Player", "Barrier"));
                    if (hitPlayer.collider != null && hitPlayer.collider.gameObject == player)
                    {
                        hitPlayer.transform.GetComponent<Player>().TakeDamage(5);
                        visualLine.Play(child.transform.position, hitPlayer.point);
                        barrierEffect.RayBarrierEffect.Stop();
                    }
                    else if (hitBarrier)
                    {
                        visualLine.Play(child.transform.position, hitBarrier.point);
                        barrierEffect.RayBarrierEffect.Play();
                    }
                    else
                    {
                        visualLine.Play(child.transform.position, new Vector2(child.transform.position.x, child.transform.position.y * -1));
                        barrierEffect.RayBarrierEffect.Stop();
                    }
                }
                else
                {
                    RaycastHit2D hitBarrier = Physics2D.Raycast(child.transform.position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Barrier"));
                    RaycastHit2D hitPlayer = Physics2D.Raycast(child.transform.position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Player", "Barrier"));
                    if (hitPlayer.collider != null && hitPlayer.collider.gameObject == player)
                    {
                        hitPlayer.transform.GetComponent<Player>().TakeDamage(5);
                        visualLine.Play(child.transform.position, hitPlayer.point);
                        barrierEffect.RayBarrierEffect.Stop();
                    }
                    else if (hitBarrier)
                    {
                        visualLine.Play(child.transform.position, hitBarrier.point);
                        barrierEffect.RayBarrierEffect.Play();
                    }
                    else
                    {
                        visualLine.Play(child.transform.position, new Vector2(child.transform.position.x, child.transform.position.y * -1));
                        barrierEffect.RayBarrierEffect.Stop();
                    }
                }
            }
        }
    }

    // 랜덤 총알 날라오기
    private void PattenOne()
    {
        if (playTime > 1f)
        {
            playTime = 0;
            Vector2 spawnDir;
            do
            {
                spawnDir = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)).normalized;
            } while (spawnDir.x == 0 && spawnDir.y == 0);
            Vector2 spawnPos = new Vector2(spawnDir.x * screenX / 2, spawnDir.y * screenX / 2);
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
