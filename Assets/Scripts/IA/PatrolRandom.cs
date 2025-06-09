using System.Collections.Generic;
using UnityEngine;

public class PatrolRandom : MonoBehaviour
{
    public List<PatrolRoute> rarityRoutes = new List<PatrolRoute>();
    public PatrolRoulette patrolRoulette;
    public Dictionary<RarityEnum, float> routes = new Dictionary<RarityEnum, float>();

    private RarityEnum _lastPathUsed;
    private bool _lastPathFailed = false;

    private Dictionary<RarityEnum, float> defaultWeights = new Dictionary<RarityEnum, float>();

    void Awake()
    {
        patrolRoulette = GetComponent<PatrolRoulette>();
        rarityRoutes = patrolRoulette.patrolRoutesList;

        for (int i = 0; i < rarityRoutes.Count; i++)
        {
            routes[rarityRoutes[i].rarity] = rarityRoutes[i].weight;
            defaultWeights[rarityRoutes[i].rarity] = rarityRoutes[i].weight;
        }
    }

    //public List<PatrolPoint> SetRoutes()
    //{
    //    RarityEnum rarity = MyRandom.Roulette(routes);
    //    PatrolRoute patrolRoute = patrolRoulette.patrolRoutes[rarity]; //Obtengo el patrolRoute del diccionario de routes con la key obtenida (RarityEnum)
    //    return patrolRoute.patrolPoints;
    //}
    //public List<PatrolPoint> SetRoutes()
    //{
    //    Dictionary<RarityEnum, float> dynamicWeights = new Dictionary<RarityEnum, float>(defaultWeights);

    //    if (_lastPathFailed)
    //    {
    //        float boostTotal = 20f; // Cuánto boost total se reparte
    //        int otherPaths = defaultWeights.Count - 1;
    //        float boostPerPath = boostTotal / otherPaths;

    //        foreach (var key in defaultWeights.Keys)
    //        {
    //            if (!key.Equals(_lastPathUsed))
    //                dynamicWeights[key] += boostPerPath;
    //        }

    //        _lastPathFailed = false;
    //    }

    //    RarityEnum chosen = MyRandom.Roulette(dynamicWeights);
    //    _lastPathUsed = chosen;
    //    return patrolRoulette.patrolRoutes[chosen].patrolPoints;
    //}

    public List<PatrolPoint> SetRoutes()
    {
        Dictionary<RarityEnum, float> dynamicWeights = new Dictionary<RarityEnum, float>(defaultWeights);

        float boostTotal = 20f; // Cuánto boost total se reparte
        float boostPerPath =  boostTotal;

        foreach (var key in defaultWeights.Keys)
        {
            if(!key.Equals(_lastPathUsed))
                dynamicWeights[key] += boostPerPath;
        }

        if (gameObject.CompareTag("Melee"))
        {
            Debug.Log("=== Pesos actuales del Roulette ===");
  
            foreach (var kvp in dynamicWeights)
            {
                Debug.Log($"{kvp.Key}: {kvp.Value}");
            }
        }
        

        RarityEnum chosen = MyRandom.Roulette(dynamicWeights);
        _lastPathUsed = chosen;
        if (gameObject.CompareTag("Melee"))
            Debug.Log($"Elegido: {chosen}");
        

        return patrolRoulette.patrolRoutes[chosen].patrolPoints;
    }

    public void MarkLastPathCompleted(out List<PatrolPoint> waypoints)
    {
        waypoints = SetRoutes();
    } 

}
