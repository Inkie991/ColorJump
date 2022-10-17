using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager: MonoBehaviour, IGameManager
{
    [SerializeField] bool generateLevel = true;
    [SerializeField] private float loadGameDelayTime = 1f;
    private const string LEVEL_OBJECT_ROOT_NAME = "level";
    private const float DISTANCE_BETWEEN_PLATFORMS = 1.5f;
    private Vector3 SPHERE_START_POSITION = new Vector3(0, 17.85f, -8);
    
    public ManagerStatus Status { get; private set; }
    public int Level { get; set; }
    public int Progress { get; set; } = 50; // TODO: Remove initial value

    public void Startup()
    {
        Debug.Log("Level manager starting...");

        Invoke("LoadGame", loadGameDelayTime);

        Status = ManagerStatus.Started;
    }

    void LoadGame()
    {
        if (generateLevel)
        {
            LoadTestingScene();
            // TODO: FIXME
            Invoke("Generate", 0.1f);
        }
        else
        {
            LoadTestingScene();
        }
    }

    void LoadEmptyScene()
    {
        SceneManager.LoadScene("Empty");
    }
    
    void LoadTestingScene()
    {
        SceneManager.LoadScene("Testing");
    }

    public void Generate()
    {
        Debug.Log("Generating level " + Level);
        var testLevel = new PlatformSpec[]
        {
            new PlatformSpec("Platform1", 180, new List<SectorSpec>()
            {
                new SectorSpec("Sector2.0", 90, GamingColors.Red)
            }, new List<WallSpec>()
            {
               new WallSpec("Wall", 180, GamingColors.Red) 
            }),
            new ColorChangerSpec(GamingColors.Green),
            new PlatformSpec("Platform1", 180),
            new ColorChangerSpec(GamingColors.Blue),
            new PlatformSpec("Platform1", 270),
            new PlatformSpec("Platform1", 360),
        };
        var columnPrefab = Resources.Load<GameObject>("Prefabs/Column");
        var spherePrefab = Resources.Load<GameObject>("Prefabs/Sphere");
        // delete level if exists
        var level = GameObject.FindWithTag("LevelRoot");
        if (level != null)
        {
            DestroyImmediate(level);
        }

        level = new GameObject(LEVEL_OBJECT_ROOT_NAME);
        level.tag = "LevelRoot";
        
        var sphere = instantiateChild(level, spherePrefab, SPHERE_START_POSITION);
        // add column
        var column = instantiateChild(level, columnPrefab);
        column.name = "Column";
        column.transform.localScale = new Vector3(1f, 10f, 1f);
        // add platforms
        float y = 0;
        foreach (PlatformSpec spec in testLevel)
        {
            var prefab = Resources.Load<GameObject>("Prefabs/" + spec.Name);
            var platform = instantiateChild(column, prefab, new Vector3(0, y, 0));
            platform.transform.Rotate(Vector3.up, spec.Angle);

            if (spec is ColorChangerSpec)
            {
                var color = new GamingColor();
                color.color = (spec as ColorChangerSpec).Color;
                platform.GetComponent<ColorChanger>().SetColor(color);
            }
            foreach (SectorSpec sectorSpec in spec.Sectors)
            {
                var sectorPrefab = Resources.Load<GameObject>("Prefabs/" + sectorSpec.Name);
                var sector = instantiateChild(column, sectorPrefab, new Vector3(0, y, 0));
                sector.transform.Rotate(Vector3.up, spec.Angle);
                sector.GetComponent<ColorableObject>().GamingColor.color = sectorSpec.Color;
            }

            foreach (WallSpec wallSpec in spec.Walls)
            {
                
                var wallPrefab = Resources.Load<GameObject>("Prefabs/" + wallSpec.Name);
                var wall = instantiateChild(column, wallPrefab, new Vector3(0, y, 0));
                wall.transform.Rotate(Vector3.up, spec.Angle);
                // TODO: add script to wall
            }
            y -= DISTANCE_BETWEEN_PLATFORMS;
            
        }
        
        // add final platform
        var finalPlatformPrefab = Resources.Load<GameObject>("Prefabs/Platform");
        var finalPlatform = instantiateChild(column, finalPlatformPrefab, new Vector3(0, y, 0));

        column.transform.localScale = new Vector3(10, 100, 10);
        sphere.transform.localScale = new Vector3(10, 10, 10);

        var cameraMotion = GameObject.FindWithTag("MainCamera").GetComponent<CameraMotion>();
        cameraMotion.ResetY();
        cameraMotion.Sphere = sphere.GetComponent<Bounceable>();
        
        // TODO: add light
        // TODO: fix scaling
        // TODO: add sectors generation
    }
    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void NextLevel()
    {
        Level++;
        Managers.Saves.Save();

        Reload();
        if (generateLevel)
        {
            // TODO: FIXME
            Invoke("Generate", 0.1f);
        }
    }

    private GameObject instantiateChild(GameObject parent, Object prefab, Vector3 position)
    {
        var child = (GameObject) Instantiate(prefab);
        child.transform.parent = parent.transform;
        child.transform.position = position;
        return child;
    }

    private GameObject instantiateChild(GameObject parent, Object prefab)
    {
        return instantiateChild(parent, prefab, Vector3.zero);
    }
}

struct SectorSpec
{
    public string Name;
    public float Angle;
    public GamingColors Color;

    public SectorSpec(string name, float angle, GamingColors color)
    {
        Name = name;
        Angle = angle;
        Color = color;
    }
}

struct WallSpec
{
    public string Name;
    public float Angle;
    public GamingColors Color;

    public WallSpec(string name, float angle, GamingColors color)
    {
        Name = name;
        Angle = angle;
        Color = color;
    }
}

class PlatformSpec
{
    public string Name;
    public float Angle;
    public List<SectorSpec> Sectors;
    public List<WallSpec> Walls;

    public PlatformSpec(string name, float angle, List<SectorSpec> sectors, List<WallSpec> walls)
    {
        Name = name;
        Angle = angle;
        Sectors = sectors;
        Walls = walls;
    }

    public PlatformSpec(string suffix, float angle) : this(suffix, angle, new List<SectorSpec>(), new List<WallSpec>())
    {
    }
}

class ColorChangerSpec : PlatformSpec
{
    public GamingColors Color;

    public ColorChangerSpec(GamingColors color) : base("ColorChanger", 0f, new List<SectorSpec>(), new List<WallSpec>())
    {
        Color = color;
    }
}