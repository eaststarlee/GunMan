using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10;
    public float damage = 1;
    Vector2 direction;
    public Vector2 Direction
    {
        set
        {
            direction = value.normalized; // 방향의 벡터값 연산용
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)  // colider 2개의 출돌 이벤트, tag설정 사전필요
    {
        if (collision.tag == "Wall" || collision.tag == "Enemy")
        {
            gameObject.SetActive(false);  // 벽에 닿을시 오브젝트 풀링에 다시 돌아감
        }
        
    }
}
