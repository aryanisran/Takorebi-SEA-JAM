using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OldEli : MonoBehaviour
{
    PlayerController player;
    public bool isBuilding, prevFrame, canBuild;
    public GameObject guidePrefab, platformPrefab, flashbangPrefab;
    GameObject guide, platform;
    public int flashCount;

    public int prevCheckpoint;

    public Transform[] checkPoints;

    float buildCD = 0f;
    [SerializeField] GameObject bombCDUI;
    [SerializeField] Slider buildCDUI;

    //[SerializeField] int platformSpawned;

    // Start is called before the first frame update
    void Start()
    {
        canBuild = true;
        player = GetComponent<PlayerController>();
        player.moveSpeed = 2.5f;
    }

    // Update is called once per frame
    void Update()
    {
        #region Build Platform
        //Make the guide when we press X
        if (Input.GetKeyDown(KeyCode.Z) && !isBuilding && canBuild)
        {
            isBuilding = true;
            prevFrame = true;
            //Round the x and y of the platform to the nearest 0.5 so it's in line with the grid
            float roundedX = Mathf.Round((transform.position.x) * 2) / 2;
            float roundedY = Mathf.Round((transform.position.y) * 2) / 2;
            guide = Instantiate(guidePrefab, new Vector3(roundedX, roundedY, 1.5f), Quaternion.identity);
        }
        if (isBuilding)
        {
            //Move the guide when pressing arrow buttons
            Vector3 inputDir;
            inputDir = Vector3.zero;
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                inputDir += Vector3.right;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                inputDir += Vector3.left;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                inputDir += Vector3.up;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                inputDir += Vector3.down;
            }
            if (guide.transform.position.x - transform.position.x + inputDir.x * 0.25f < 1.5f && guide.transform.position.y - transform.position.y + inputDir.y * 0.25f < 1.5f)
            {
                guide.transform.position += inputDir * 0.25f;
            }
            //Finalize building when we press X again
            if (Input.GetKeyDown(KeyCode.Z) && !prevFrame)
            {
                canBuild = false;
                Destroy(Instantiate(platformPrefab, guide.transform.position, Quaternion.identity), 5f);
                StartCoroutine(WaitPlatformCo());
                //Store a temporary reference to the guide so we don't delete the guide stored in our global variable, just in case it gives us null refernce issues
                buildCD = 5f;
                GameObject temp = guide;
                guide = null;
                Destroy(temp);
                //platformSpawned++; 
                isBuilding = false;
            }
            //This is to make sure we don't finalize building on the same frame we first pressed X
            if (prevFrame && !Input.GetKey(KeyCode.X))
            {
                prevFrame = false;
            }
        }
        #endregion
        Flashbang();

        if (flashCount <= 0)
        {
            bombCDUI.SetActive(true);
        }
        else { bombCDUI.SetActive(false); }

        buildCDUI.value = buildCD;
        if (buildCD >= 0)
        {
            buildCD = buildCD - Time.deltaTime;
        }
        else
        {
            buildCD = 0f;
        }
    }


    IEnumerator WaitPlatformCo()
    {
        yield return new WaitForSeconds(5f);
        canBuild = true;
    }

    void Flashbang()
    {
        if (Input.GetKeyDown(KeyCode.X) && flashCount > 0)
        {
            AudioManager.instance.Play("Flashbang");
            Instantiate(flashbangPrefab, transform.position + Vector3.back * 0.2f, Quaternion.identity);
            flashCount--;
        }
    }

    public void PickupFlash()
    {
        flashCount++;
        //Might have other ui stuff to add here later
    }

    public void PassCheckpoint(int _i)
    {
        prevCheckpoint = _i;
        PlayerPrefs.SetInt("Checkpoint", prevCheckpoint);
    }
}
