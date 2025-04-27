using UnityEngine;
using System.Collections;
using System;
using Unity.VisualScripting;

public class RangeModel : AIModel
{
    public bool isTimeToDropMine = false;
    [SerializeField] public Mine minePrefab;
    private Coroutine mineDropCor;
    private int mineCount;
    private int autoExplosionMine = 4;
    protected override void Awake()
    {
        base.Awake();
        onAttack += DropMine;
        onSightAcheived += ManageMine;
        lostSightDuration = 10f;
    }

    public void DropMine()
    {
        if (minePrefab != null)
        {
            var mine = Instantiate(minePrefab.gameObject, Position.position, Quaternion.identity);
            mine.transform.forward = Position.forward;
            mine.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Mine prefab is not assigned in the inspector.");
        }
    }
    private void Update()
    {
        Debug.Log(isTimeToDropMine);
    }
    public void ManageMine()
    {
        mineDropCor = StartCoroutine(ManageMineDropBool());
    }
    private IEnumerator ManageMineDropBool()
    {
        isTimeToDropMine = false;
        yield return new WaitForSeconds(2f);
        mineCount++;
        isTimeToDropMine = true;
        if (mineCount == autoExplosionMine)
        {
            Destroy(gameObject);
        }
        yield return new WaitForSeconds(.3f);
        isTimeToDropMine = false;
        ManageMine();
    }


}
