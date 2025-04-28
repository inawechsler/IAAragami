using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PatrolRoulette : MonoBehaviour
{
    [SerializeField] public List<PatrolRoute> patrolRoutesList = new();
    public Dictionary<RarityEnum, PatrolRoute> patrolRoutes = new Dictionary<RarityEnum, PatrolRoute>();

    private void Awake()
    {

        foreach (var patrolPoint in patrolRoutesList)
        {
           
            if (!patrolRoutes.ContainsKey(patrolPoint.rarity))
            {
                patrolRoutes[patrolPoint.rarity] = patrolPoint;
            }
            patrolRoutes[patrolPoint.rarity] = patrolPoint;

            Debug.Log(patrolRoutes[patrolPoint.rarity].name + ", " + patrolRoutes[patrolPoint.rarity].weight);

        }
    }



}
