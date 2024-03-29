using UnityEngine;


public class Item : MonoBehaviour
{
    public ItemID Id { get; private set; }
    public ItemData itemData;
    private IItem Iitem;
    private string m_ActieisItemName;

    public string Name { get; private set; }
    public ItemType itemType { get; private set; }
    public float AttakAdd { get; private set; }
    public float AttakMulti { get; private set; }
    public float AttakSpeedAdd { get; private set; }
    public float AttakSpeedMulti { get; private set; }
    public float Speed { get; private set; }
    public float Range { get; private set; }
    public int Cost { get; private set; }
    public string Sprite { get; private set; }
    public string AcquireSound { get; private set; }
    public string UseSound { get; private set; }

    private Rigidbody2D rb;
    private CircleCollider2D collider2D;
    private Collider2D _collider2D;
    private SpriteRenderer spriteRenderer;
    private AudioClip clip;

    const int m_isBoxType = 4000;
    private Vector3 initialPosition;
    public void Init(ItemID id, Vector3 po)
    {
        if ((int)id > m_isBoxType)
        {
            return;
        }
        rb = gameObject.AddComponent<Rigidbody2D>();
        collider2D = gameObject.AddComponent<CircleCollider2D>();
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

        rb.mass = 3;
        rb.drag = 999999;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        gameObject.tag = "Item";

        Id = id;
        itemData = Managers.Data.Item[Id];

        this.Name = itemData.Name;
        this.itemType = (ItemType)itemData.ItemType;
        this.AttakAdd = itemData.AttakAdd;
        this.AttakMulti = itemData.AttakMulti;
        this.AttakSpeedAdd = itemData.AttakSpeedAdd;
        this.AttakSpeedMulti = itemData.AttakSpeedMulti;
        this.Speed = itemData.Speed;
        this.Range = itemData.Range;
        this.Cost = itemData.Cost;
        this.Sprite = itemData.Sprite;
        this.AcquireSound = itemData.AcquireSound;
        this.UseSound = itemData.UseSound;

        transform.position = po + Vector3.up;
        initialPosition = po + Vector3.up;
        transform.localScale = new Vector2(2f, 2f);
        collider2D.enabled = true;
        collider2D.radius = 0.05f;
        spriteRenderer.sprite = Managers.Resource.LoadSprite(Sprite);
        spriteRenderer.sortingOrder = 4;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !gameObject.CompareTag("Box")) 
        {

            Debug.Log("아이템획득");
            Managers.Sound.ChangeGetItemSound(AcquireSound);
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            

            
            if (this.itemType == ItemType.Passive)
            {
                playerStats.UpdateStats(AttakAdd, AttakMulti, AttakSpeedAdd, AttakSpeedMulti, Speed, Range);

            }
            else if (this.itemType == ItemType.Active)
            {
                m_ActieisItemName = string.Concat("Item_", Name);
                Managers.Item.AddComponent(m_ActieisItemName, transform);
                Managers.UI.GetActive(Sprite);
            }
        
            else if (this.itemType == ItemType.Consumer)
            {
                GetConsumerItem((int)Id, playerStats);
            }
        }
    }

    private void GetConsumerItem(int itemId, PlayerStats playerStats)
    {
        switch (itemId)
        {
            case 3001:
                playerStats.GetKey();
                break;
            case 3002:
                playerStats.GetBomb();
                break;

            case 3003:
            case 3004:
                int healAmount = (itemId == 3003) ? 1 : 2;
                if (playerStats.hp < 8)
                    playerStats.GetHp(healAmount);
                break;
        }
        Managers.UI.GetConsumer();
    }
    //게임오브젝트가 활성화 되면 그 위아래로 왔다 갔다 할 메소드(미완)
    #region
    public float ShakeY = 0.2f;
    public float ShakeDuration = 0.8f;
    private void Update()
    {
        if (itemType != 0)
        {
            Shake();
        }
    }

    private void Shake()
    {
        float newY = Mathf.Abs(Mathf.Sin(Time.time * Mathf.PI / ShakeDuration)) * ShakeY;

        var newPosition = initialPosition;
        newPosition.y += newY;

        transform.position = newPosition;
    }
    #endregion
}
