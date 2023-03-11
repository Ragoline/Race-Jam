using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] public Canvas Canvas;
    [SerializeField] private GameObject _road;
    [SerializeField] private GameObject _upperRoad;
    [SerializeField] private GameObject _obstacle;
    [SerializeField] private GameObject _bigObstacle;
    [SerializeField] private GameObject _vehicle;
    [SerializeField] private GameObject _pause;
    public static bool TimeFlows { get; private set; }
    public static GameManager Instance;
    public int Vehicle = -2;
    private Sprite[] vehicles;

    private void Awake()
    {
        var n = Resources.LoadAll<Sprite>("Images/Vehicles").Length;
        vehicles = new Sprite[n];
        vehicles = Resources.LoadAll<Sprite>("Images/Vehicles");
    }

    private void Start()
    {
        Vehicle = -2;
        Instance = this;
        TimeFlows = true;
    }

    private float second = 1f;
    void Update()
    {
        if (TimeFlows)
        {
            #region road
            _road.transform.position = new Vector2(_road.transform.position.x, _road.transform.position.y - CarController.speed);
            _upperRoad.transform.position = new Vector2(_upperRoad.transform.position.x, _upperRoad.transform.position.y - CarController.speed);
            if (_road.transform.position.y <= -800)
                _road.transform.position = new Vector2(_road.transform.position.x, _upperRoad.transform.position.y + 1600f);
            if (_upperRoad.transform.position.y <= -800)
                _upperRoad.transform.position = new Vector2(_upperRoad.transform.position.x, _road.transform.position.y + 1600f);
            #endregion

            if (second == 1f)
            {
                switch (Random.Range(0, 1000))
                {
                    case 0:
                        CreateObstacle();
                        break;

                    case 1:
                        if (Vehicle == -2)
                            CreateBigObstacle();
                        break;

                    case 2:
                        if (Vehicle == -2)
                            CreateVehicle();
                        break;
                }
            }
            if (second < 1f && second > 0f)
                second -= Time.deltaTime;
            else
                second = 1f;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    private void CreateObstacle()
    {
        int r = Random.Range(-1, 2);
        while (r == Vehicle)
            r = Random.Range(-1, 2);
        var go = Instantiate(_obstacle, Canvas.transform);
        go.transform.SetSiblingIndex(3);
        go.GetComponent<Obstacle>().Create(r, null);
        second -= Time.deltaTime;
    }

    private void CreateBigObstacle()
    {
        var go = Instantiate(_bigObstacle, Canvas.transform);
        go.transform.SetSiblingIndex(3);
        switch (Random.Range(-1, 1))
        {
            case -1:
                go.GetComponent<Obstacle>().CreateBig(-1, null);
                break;

            case 0:
                go.GetComponent<Obstacle>().CreateBig(1, null);
                break;
        }
        second -= Time.deltaTime;
    }

    private void CreateVehicle()
    {
        Vehicle = Random.Range(-1, 2);
        var go = Instantiate(_vehicle, Canvas.transform);
        go.transform.SetSiblingIndex(3);
        go.GetComponent<Vehicle>().Create(Vehicle, vehicles[Random.Range(0, vehicles.Length)]);
        second -= Time.deltaTime;
    }

    public void Pause()
    { 
        TimeFlows = !TimeFlows;
        _pause.SetActive(!_pause.activeSelf);
    }

    public void Exit()
    {
        SceneManager.LoadScene("Menu");
    }
}
