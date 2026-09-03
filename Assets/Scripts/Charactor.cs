using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charactor : MonoBehaviour
{

    public float MaxHP = 3;
    public GameObject HPGauge;
    float HP;
    float HPMaxWidth;

    void Start()
    {
        HP = MaxHP;
        if (HPGauge != null)
        {
            HPMaxWidth = HPGauge.GetComponent<RectTransform>().sizeDelta.x;
        }
    }

    public void Initialize()
    {
        HP = MaxHP;
    }


    public bool Hit(float damage)
    {
        HP -= damage;

        if (HP < 0)
        {
            HP = 0;
        }

        if (HPGauge != null)
        {

            HPGauge.GetComponent<RectTransform>().sizeDelta = new Vector2(HP / MaxHP * HPMaxWidth,
                HPGauge.GetComponent<RectTransform>().sizeDelta.y);
        }

        return HP > 0;
    }
}
