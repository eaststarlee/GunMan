using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 3;   // public으로 해두면 Unity Inspector에 멤버변수speed 보임
    public GameObject bulletPrefab;

    public Material FlashMaterial;
    public Material DefaultMaterial;

    public AudioClip shotsound;
    public AudioClip Hitsound;
    public AudioClip deadsound;
    Vector3 move;

    void Start()
    {
        
    }

    void Update()
    {
        // ##캐릭터 이동
        move = Vector3.zero; // 이거 없으면 미끄러지듯이 계속 이동 

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        //Input.GetKey  = 어떤키가 눌려있는 상태일떄 참 reuturn
        {
            move += new Vector3(-1, 0, 0);
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            move += new Vector3(1, 0, 0);
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            move += new Vector3(0, 1, 0);
        }

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            move += new Vector3(0, -1, 0);
        }

        // ##대각선 이동시 빨라지는걸 방지
        move = move.normalized;

        if (move.x < 0)
        {
            GetComponent<SpriteRenderer>(). flipX = true;
        }
        if (move.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        // ##애니메이터 move / stop 트리거
        if (move.magnitude > 0)
        {
            GetComponent<Animator>().SetTrigger("Move");
        }
        else
        {
            GetComponent<Animator>().SetTrigger("Stop");
        }

        // ##마우스 입력시 총알발사
        if (Input.GetMouseButtonDown(0))
        {
            shoot();
        }


    }

    void shoot()  // 총알 발사 함수
    {
        GetComponent<AudioSource>().PlayOneShot(shotsound);

        Vector3 worldposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldposition.z = 0;
        worldposition -= (transform.position + new Vector3(0, -0.5f, 0));

        GameObject newBullet = GetComponent<ObjectPool>().Get();  //오브젝트 풀링에서 가져옴
        if (newBullet != null)
        {
            newBullet.transform.position = transform.position + new Vector3(0, -0.5f, 0);
            newBullet.GetComponent<Bullet>().Direction = worldposition;
        }


    }


    private void FixedUpdate()  // update이지만 물리충돌이 일어날경우 함수
    {
        transform.Translate(move * speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)  // 둘다 isTrigger가 아닐떄
    {
        if (collision.gameObject.tag == "Enemy")
        {
           if( GetComponent<Charactor>().Hit(1))
            {
                //살아있다 (맞을때)
                Flash();
                GetComponent<AudioSource>().PlayOneShot(Hitsound);
            }
            else
            {
                //죽었당 ㅠ
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
        GetComponent<Animator>().SetTrigger("Die");
        Invoke("AfterDying", 0.875f);
    }

    void AfterDying()
    {
        SceneManager.LoadScene("GameOverScene");
    }

}
