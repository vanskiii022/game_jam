using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapManager : Singleton<MapManager>
{
    public string curMapName = "";
    public GameObject[] curMaps = new GameObject[2];
    public bool isASide = true;
    public Camera[] cameras = new Camera[2];
    public Vector3 bSidePos = new Vector3(0, 1000, 0);
    public EnemyAI[] enemyAIs;

    private void SwitchEnemies(bool toASide)
    {
        foreach (EnemyAI e in enemyAIs)
        {
            e.SwitchSide(toASide);
        }
    }

    private Vector3 GetTeleportPos(bool toASide, Vector3 currentPosition, out bool canTeleport)
    {
        Debug.Log(currentPosition);
        Vector3 rayStart = currentPosition + Vector3.up * 50f;
        Vector3 rayDirection = Vector3.down;
        RaycastHit hit;
        int layerMask = LayerMask.GetMask("Default");
        float dy = 0f;
        if (Physics.Raycast(rayStart, rayDirection, out hit, 100f + Mathf.Epsilon, layerMask))
        {
            Debug.Log("1 Ray hit an object on the Default layer at y position: " + hit.point.y);
            //Debug.DrawLine(rayStart, hit.point, Color.red, duration: 5f);
            dy = Mathf.Max(0f, currentPosition.y - hit.point.y);
        }
        else
        {
            Debug.Log("1 Ray did not hit any object on the Default layer.");
            //Debug.DrawRay(rayStart, rayDirection * (100f + Mathf.Epsilon), Color.blue, duration: 5f);
        }



        currentPosition = currentPosition + bSidePos * (toASide ? -1 : 1);
        Debug.Log(currentPosition);
        Vector3 tpPos = new Vector3(currentPosition.x, 0, currentPosition.z);
        canTeleport = false;
        rayStart = currentPosition + Vector3.up * 50f;
        rayDirection = Vector3.down;
        if (Physics.Raycast(rayStart, rayDirection, out hit, 100f + Mathf.Epsilon, layerMask))
        {
            Debug.Log("2 Ray hit an object on the Default layer at y position: " + hit.point.y);
            //Debug.DrawLine(rayStart, hit.point, Color.red, duration: 5f);
            float hitYPosition = hit.point.y;
            tpPos = new Vector3(currentPosition.x, hitYPosition + dy, currentPosition.z);
            canTeleport = true;
        }
        else
        {
            Debug.Log("2 Ray did not hit any object on the Default layer.");
            //Debug.DrawRay(rayStart, rayDirection * (100f + Mathf.Epsilon), Color.blue, duration: 5f);
        }

        return tpPos;
    }

    public void SwitchSide()
    {
        bool oldIsASide = isASide; 
        isASide = !isASide;
        var player = PlayerManager.Instance.player;
        bool canTp = false;
        Vector3 tpPos = GetTeleportPos(isASide, player.transform.position, out canTp);
        if(canTp)
        {
            player.Teleport(tpPos);
            cameras[0].gameObject.SetActive(isASide);
            cameras[1].gameObject.SetActive(!isASide);
            SwitchEnemies(isASide);
        }
        else
        {
            isASide = oldIsASide;
        }
    }

    private void GetEnemies()
    {
        Transform enemiesParent = curMaps[1].transform.Find("Enemies");

        // 如果找到了这个子物体
        if (enemiesParent != null)
        {
            // 获取 "Enemies" 下所有具有 EnemyAI 组件的物体
            enemyAIs = enemiesParent.GetComponentsInChildren<EnemyAI>(true);
        }
    }

    public void ChangeMap(string mapName)
    {
        if (curMaps[0]) Destroy(curMaps[0]);
        if (curMaps[1]) Destroy(curMaps[1]);
        curMapName = mapName;
        GameObject mapA = Resources.Load<GameObject>("Prefabs/Maps/" + curMapName + "_A");
        GameObject mapB = Resources.Load<GameObject>("Prefabs/Maps/" + curMapName + "_B");
        curMaps[0] = Instantiate(mapA);
        curMaps[1] = Instantiate(mapB);
        curMaps[1].transform.position = bSidePos;
        GetEnemies();
        SwitchEnemies(true);
        cameras[0].gameObject.SetActive(isASide);
        cameras[1].gameObject.SetActive(!isASide);
        var player = PlayerManager.Instance;
        player.Respawn();
    }

    public void GetSpawnPoint(out Vector3 pos, out Quaternion rot)
    {
        Transform spawnPt = curMaps[isASide ? 0 : 1].transform.Find("SpawnPoint");
        pos = spawnPt.position + Vector3.up * 0.42f;
        rot = spawnPt.rotation;
    }

    public Vector3 GetPlayerRelativePos()
    {
        return PlayerManager.Instance.transform.position - bSidePos * (isASide? 0 : 1);
    }

    private void Update()
    {
        if (PlayerManager.Instance != null)
        {
            StarterAssetsInputs input = PlayerManager.Instance.input;
            if (input != null && input.switchSide)
            {
                input.switchSide = false;
                if (!PlayerManager.Instance.player.IsDead)
                {
                    SwitchSide();
                }
            }
        }
    }

    private void LateUpdate()
    {
        cameras[0].GetComponent<PlayerFollowCamera>().OnLateUpdate();
        cameras[1].GetComponent<PlayerFollowCamera>().OnLateUpdate();
    }
}