using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NextstageTest : MonoBehaviour
{
    [SerializeField] private GameObject child1;
    [SerializeField] private GameObject child2;
    [SerializeField] private GameObject child3;
    [SerializeField] private BoxCollider2D _boxCollider;

    void Start()
    {
        transform.position += new Vector3(0, 2.5f, 0);
        child1 = transform.Find("BasementDoorOpen").gameObject;
        child2 = transform.Find("BasementDoorGetIn").gameObject;
        child3 = transform.Find("BasementDoorGetIn2").gameObject;

        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SetActive(false);
            child2.SetActive(true);
            child1.SetActive(false);
            child3.SetActive(true);
            StartCoroutine(NextScene());
        }
    }

    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(1.1f);
        Managers.Sound.ChangeBGM("ClearSound");
        SceneManager.LoadScene(2);

    }

}
