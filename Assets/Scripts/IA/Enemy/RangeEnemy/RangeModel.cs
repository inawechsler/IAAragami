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

    public void DropMine()//Método que se ejecuta en onAttack recibido
    {
        if (minePrefab != null)
        {
            var mine = Instantiate(minePrefab.gameObject, SelfPosition.position, Quaternion.identity);
            mine.transform.forward = SelfPosition.forward;
            mine.gameObject.SetActive(true);
        }
    }

    public void ManageMine()//Corrutina que maneja el dropeo de la mina
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
