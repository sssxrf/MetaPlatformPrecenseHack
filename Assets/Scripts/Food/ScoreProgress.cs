using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreProgress : MonoBehaviour
{
    [SerializeField] private Transform top;
    [SerializeField] private Transform bottom;
    [SerializeField] private GameObject inside;
    [SerializeField] private Material lightupMaterial;
    [SerializeField] private GameObject star;
    
    // Start is called before the first frame update
    void Start()
    {
        inside.transform.position = bottom.position;
    }

    // Update is called once per frame
    void Update()
    {
        // check progress of the score to update location of inside part
        float totalProgressDistance = top.transform.position.y - bottom.transform.position.y;
        float currentProgressPercentage = (float)MRSceneManager.Instance._score/  (float)MRSceneManager.Instance.maxScore;
        float currentProgress = totalProgressDistance * currentProgressPercentage;
        Debug.Log(" Current Progress Percentage " + currentProgressPercentage );
        inside.transform.position = new Vector3(inside.transform.position.x,  bottom.transform.position.y + currentProgress,inside.transform.position.z);
        if (currentProgressPercentage >= 1f)
        {
            star.GetComponent<MeshRenderer>().material = lightupMaterial;
        }
    }
}
