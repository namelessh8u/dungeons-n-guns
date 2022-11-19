using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUnlockCage : MonoBehaviour
{
    public bool canUnlock;
    public GameObject message;

    public CharacterSelector[] charSelects;
    private CharacterSelector charToUnlock;

    public SpriteRenderer cagedSR;

    // Start is called before the first frame update
    void Start()
    {
        charToUnlock = charSelects[Random.Range(0, charSelects.Length)];

        cagedSR.sprite = charToUnlock.playerToSpawn.playerSR[0].sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (canUnlock && Input.GetKeyDown(KeyCode.E))
        {
            PlayerPrefs.SetInt(charToUnlock.playerToSpawn.name, 1);

            Instantiate(charToUnlock, transform.position, transform.rotation);
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canUnlock = true;
            message.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canUnlock = false;
            message.SetActive(false);
        }
    }

}
