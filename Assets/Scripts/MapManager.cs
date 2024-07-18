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

    public void SwitchSide()
    {
        isASide = !isASide;
        var player = PlayerManager.Instance.player;
        player.Teleport(player.transform.position + bSidePos * (isASide ? -1 : 1));
        cameras[0].gameObject.SetActive(isASide);
        cameras[1].gameObject.SetActive(!isASide);
        SwitchEnemies(isASide);
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
        var player = PlayerManager.Instance.player;
        Transform spawnPt = curMaps[0].transform.Find("SpawnPoint");
        player.Teleport(spawnPt.position + Vector3.up * 0.42f, spawnPt.rotation);
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
                SwitchSide();
            }
        }
    }

    private void LateUpdate()
    {
        cameras[0].GetComponent<PlayerFollowCamera>().OnLateUpdate();
        cameras[1].GetComponent<PlayerFollowCamera>().OnLateUpdate();
    }
}