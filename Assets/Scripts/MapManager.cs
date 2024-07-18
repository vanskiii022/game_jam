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


    public void SwitchSide()
    {
        isASide = !isASide;
        var player = PlayerManager.Instance.player;
        player.Teleport(player.transform.position + bSidePos * (isASide ? -1 : 1));
        cameras[0].gameObject.SetActive(isASide);
        cameras[1].gameObject.SetActive(!isASide);
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
        cameras[0].gameObject.SetActive(isASide);
        cameras[1].gameObject.SetActive(!isASide);
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
}