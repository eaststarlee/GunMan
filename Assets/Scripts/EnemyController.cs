using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    enum State
    {
        Spawning,
        Moving,
        Dying
    }

    public float speed = 2;

    public Material FlashMaterial;
    public Material DefaultMaterial;
    
    public AudioClip hitsound;
    public AudioClip deadsound;


    GameObject target;
    State state;

    void Start()
    {

    }

    public void Spawn(GameObject target)
    {
        this.target = target;   
        state = State.Spawning; 
        GetComponent<Charactor>().Initialize();
        GetComponent<Animator>().SetTrigger("Spawn");
        Invoke("StartMoving", 1);
        GetComponent<Collider2D>().enabled = false;

    }

    void StartMoving()
    {
        GetComponent<Collider2D>().enabled = true;
        state = State.Moving;
    }

    private void FixedUpdate()
    {
        if (state == State.Spawning)
        {
            // 적이 스폰 후 이동 상태로 변경
            Vector2 direction = target.transform.position - transform.position;
            transform.Translate(direction.normalized * speed * Time.fixedDeltaTime);

            if (direction.x < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }

            // Spawning 상태를 끝내고 Moving 상태로 변경
            if (Vector2.Distance(transform.position, target.transform.position) < 0.1f) // 플레이어에 가까워지면
            {
                state = State.Moving;
            }
        }
        else if (state == State.Moving)
        {
            // 이제 적이 플레이어를 따라옴
            Vector2 direction = target.transform.position - transform.position;
            transform.Translate(direction.normalized * speed * Time.fixedDeltaTime);

            if (direction.x < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            float d = collision.gameObject.GetComponent<Bullet>().damage;

            if (GetComponent<Charactor>().Hit(d))
            {
                // 살아있을때(맞음)
                Flash();
                GetComponent<AudioSource>().PlayOneShot(hitsound);
            }
            else
            {
                // 죽었을때
                Die();
                GetComponent<AudioSource>().PlayOneShot(deadsound);
            }
        }
    }


    void Flash()
    {
        GetComponent<SpriteRenderer>().material = FlashMaterial;
        Invoke("AfterFlash", 0.5f);
    }

    void AfterFlash()
    {
        GetComponent<SpriteRenderer>().material = DefaultMaterial;
    }




    void Die()
    {
        state = State.Dying;
        GetComponent<Animator>().SetTrigger("Die");
        StartCoroutine(AfterDyingCoroutine());  // 코루틴을 호출하여 일정 시간 후 게임 오브젝트를 삭제
    }

    // 코루틴을 사용하여 일정 시간 후에 게임 오브젝트를 삭제
    IEnumerator AfterDyingCoroutine()
    {
        yield return new WaitForSeconds(1.5f);  // 1.5초 후에
        gameObject.SetActive(false);
    }
}
