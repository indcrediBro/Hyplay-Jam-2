﻿using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

public class MapManager : MonoBehaviour
{
    public MapConfig config;
    public MapView view;

    public Map CurrentMap { get; private set; }

    private void Start()
    {
        if (PlayerPrefs.HasKey("Map") && GameDataReferences.Instance.continueGame)
        {
            ContinueMap();
        }
        else
        {
            GenerateNewMap();
        }
    }

    public void ContinueMap()
    {
        string mapJson = PlayerPrefs.GetString("Map");
        Map map = JsonConvert.DeserializeObject<Map>(mapJson);
        // using this instead of .Contains()
        if (map.path.Any(p => p.Equals(map.GetBossNode().point)))
        {
            // payer has already reached the boss, generate a new map
            GenerateNewMap();
        }
        else
        {
            CurrentMap = map;
            // player has not reached the boss yet, load the current map
            view.ShowMap(map);
        }
    }

    public void GenerateNewMap()
    {
        Map map = MapGenerator.GetMap(config);
        CurrentMap = map;
        Debug.Log(map.ToJson());
        view.ShowMap(map);


    }

    public void SaveMap()
    {
        if (CurrentMap == null) return;

        string json = JsonConvert.SerializeObject(CurrentMap, Formatting.Indented,
            new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        PlayerPrefs.SetString("Map", json);
        PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        SaveMap();
    }
}
