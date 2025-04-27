using UnityEngine;
using System.Collections;
using System;

public class RangeModel : AIModel
{
    public bool isTimeToDropMine = false;
    public Mine minePrefab;
    private Coroutine mineDropCor;
    protected override void Awake()
    {
        base.Awake();
        onAttack += DropMine;  
    }

    public void DropMine()
    {
        if (minePrefab != null)
        {
            Mine mine = Instantiate(minePrefab, Position.position, Quaternion.identity);
            mine.transform.forward = Position.forward;
            mine.gameObject.SetActive(true);
        }
    }

    public void ManageMine()
    {
        if (mineDropCor != null)
        {
            StopCoroutine(mineDropCor);
        }
        mineDropCor = StartCoroutine(ManageMineDropBool());
    }

    private IEnumerator ManageMineDropBool()
    {
        isTimeToDropMine = false;
        yield return new WaitForSeconds(1f);
        isTimeToDropMine = true;
    }


}
