using System.Collections.Generic;
using UnityEngine;

public class PatrolRandom : MonoBehaviour
{
    public List<PatrolRoute> rarityRoutes = new List<PatrolRoute>();
    public PatrolRoulette patrolRoulette;
    public Dictionary<RarityEnum, float> routes = new Dictionary<RarityEnum, float>();

    private RarityEnum _lastPathUsed;

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

    public List<PatrolPoint> SetRoutes()
    {
        Dictionary<RarityEnum, float> dynamicWeights = new Dictionary<RarityEnum, float>(defaultWeights);

        float boostTotal = 20f;
        float boostPerPath =  boostTotal;

        foreach (var key in defaultWeights.Keys)
        {
            if(!key.Equals(_lastPathUsed))
                dynamicWeights[key] += boostPerPath;
        }
        
        RarityEnum chosen = MyRandom.Roulette(dynamicWeights);
        _lastPathUsed = chosen;

        return patrolRoulette.patrolRoutes[chosen].patrolPoints;
    }

    public void MarkLastPathCompleted(out List<PatrolPoint> waypoints)
    {
        waypoints = SetRoutes();
    } 

}
