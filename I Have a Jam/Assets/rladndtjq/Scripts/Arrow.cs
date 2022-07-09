using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
enum BulletType
{
    Normal,
    Granade,
    Plague
}
public class Arrow : MonoBehaviour
{
    [SerializeField] Vector2 Offset;
    public static Arrow Instance { get; private set; }

    public float BulletDmg = 10;
    public int granadeBulletAmount;
    public int Hp;
    [HideInInspector] public float currentHp;
     public int shootAmount;
    [HideInInspector] public int shootBack = 0;
    [HideInInspector] public bool canShoot = false;
    [HideInInspector] public int shootCount = 0;
    [SerializeField] TextMesh hpText;

    LineRenderer lineRenderer;
    GameObject circle;
    SpriteRenderer spriteRenderer;
    Vector2 mouseDir;
    Vector2 spawnPosition;
    float dir;
    void Start()
    {
        Instance = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        lineRenderer = transform.Find("Line").GetComponent<LineRenderer>();
        circle = transform.Find("Circle").gameObject;
        currentHp = Hp;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, Mathf.Infinity, LayerMask.GetMask("Wall") | LayerMask.GetMask("Enemy"));
        Debug.DrawLine(transform.position, hit.point, Color.red);
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, hit.point);
        lineRenderer.material.mainTextureOffset = new Vector2(Time.time * Offset.x, Time.time * Offset.y);
        circle.transform.position = hit.point;

        hpText.text = currentHp.ToString();
        if(shootBack == shootAmount)
        {
            GameManager.instance.isShootEnd = true;
            shootBack = 0;
            shootCount = 0;
            Debug.Log("shootEnd");
        }
        

        if(canShoot)
        {
            if(Input.GetMouseButton(0) && GameManager.instance.isSelect == false)
            {
                spriteRenderer.enabled = true;
                lineRenderer.enabled = true;
                circle.GetComponent<SpriteRenderer>().enabled = true;
                mouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                dir = Mathf.Atan2(mouseDir.x, mouseDir.y) * -Mathf.Rad2Deg + 90;
                transform.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(dir, 20, 160));
            }

            if(Input.GetMouseButtonUp(0) && GameManager.instance.isSelect == false)
            {
                spawnPosition = transform.position;
                StartCoroutine(Shoot());
                spriteRenderer.enabled = false;
                lineRenderer.enabled = false;
                circle.GetComponent<SpriteRenderer>().enabled = false;
                canShoot = false; 
            }
        }
    }

    private IEnumerator Shoot()
    {
        for(int i = 0; i < shootAmount; i++)
        {
            var bullet = ObjectPool.GetObject(ObjectPool.instance.prefebs[0].gameObject, null);
            bullet.GetComponent<Bullet>().NormalDamage = BulletDmg;

            if(shootCount < granadeBulletAmount)
                bullet.GetComponent<Bullet>().bulletType = (int)BulletType.Granade;   

            bullet.transform.position = new Vector3(spawnPosition.x,spawnPosition.y + 0.5f, 0);
            shootCount++;
            
            yield return new WaitForSeconds(0.1f);
        }
    }
    
   
    
}
