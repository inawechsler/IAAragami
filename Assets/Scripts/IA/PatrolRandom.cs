using System.Collections.Generic;
using UnityEngine;

public class PatrolRandom : MonoBehaviour
{
    public List<PatrolRoute> rarityRoutes = new List<PatrolRoute>();
    public PatrolRoulette patrolRoulette;
    public Dictionary<RarityEnum, float> routes = new Dictionary<RarityEnum, float>();

    void Awake()
    {

        patrolRoulette = GetComponent<PatrolRoulette>();
        rarityRoutes = patrolRoulette.patrolRoutesList;

        for (int i = 0; i < rarityRoutes.Count ; i++)
        {
            routes[rarityRoutes[i].rarity] = rarityRoutes[i].weight;
        }

    }

    public List<PatrolPoint> SetRoutes()
    {
        RarityEnum rarity = MyRandom.Roulette(routes);
        PatrolRoute patrolRoute = patrolRoulette.patrolRoutes[rarity]; //Obtengo el patrolRoute del diccionario de routes con la key obtenida (RarityEnum)
        return patrolRoute.patrolPoints;
    }

}
