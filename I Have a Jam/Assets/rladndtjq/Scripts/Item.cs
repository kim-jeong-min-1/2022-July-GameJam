using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ItemType
{
    heal,
    bullet
}

public class Item : MonoBehaviour
{
    public int item;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.CompareTag("bullet"))
        {
            if(item == 1)
            {
                Arrow.Instance.shootAmount++;
                Arrow.Instance.shootCount++;
                
            }
            else
            {
                Arrow.Instance.currentHp += 20;
                if(Arrow.Instance.currentHp > Arrow.Instance.Hp)
                {
                    Arrow.Instance.currentHp = Arrow.Instance.Hp;
                }
            }

            Destroy(this.gameObject);
        }
    }
}
