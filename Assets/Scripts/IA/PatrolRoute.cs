using System.Collections.Generic;
using UnityEngine;

public class PatrolRoute : MonoBehaviour
{
    public List<PatrolPoint> patrolPoints = new List<PatrolPoint>();

    public RarityEnum rarity;

    public int weight;
}
