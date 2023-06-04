using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public static SpawnManager instance;
    private Transform defaultPoint;
    private Vector3 spawnLocation;
    private bool setPoint;

    private void Awake()
    {
        if (SpawnManager.instance == null)
            SpawnManager.instance = this;
        else
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
    }

    public void SetSpawn(Vector3 pos)
    {
        setPoint = true;
        spawnLocation = pos;
    }

    public void SpawnPlayer()
    {
        Loader.Scene scene = Loader.CurrentScene;

        switch (scene)
        {
            case Loader.Scene.DesertedIsland:
                SpawnAtStart();
                break;
            case Loader.Scene.Harbor:

                if (setPoint)
                {
                    SpawnAtSetLocation();
                    setPoint = false;
                }
                else
                {
                    SpawnAtStart();
                }
                break;
            case Loader.Scene.StartMenu:
                break;
            case Loader.Scene.LoadingScene:
                break;
            case Loader.Scene.Tavern:
                SpawnAtStart();
                break;
            default:
                break;
        }
    }

    public void SpawnAtStart()
    {
        defaultPoint = GameObject.Find("DefaultSpawnPoint").transform;
        PlayerController.instance.SetPos(defaultPoint.position);

    }

    void SpawnAtSetLocation()
    {
        PlayerController.instance.SetPos(spawnLocation);
    }


}
