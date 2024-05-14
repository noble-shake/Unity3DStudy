using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControl : MonoBehaviour
{
    [SerializeField] GameObject fabHitHole;
    [SerializeField] Transform trsTarget;
    Camera camMain;
    [SerializeField] LineRenderer lineRenderer;
    float distance;
    Vector3 hitPosition;
    RaycastHit hit;
    short shootCounter; // 2byte -30000 ~ 30000 (also, Sorting Layer Range)

    [SerializeField] List<Light> listLight; // 0 : hitLight, 1 : ShootLight

    // Start is called before the first frame update
    void Start()
    {
        camMain = Camera.main;
        // lineRenderer = GetComponent<LineRenderer>();
        distance = Vector3.Distance(camMain.transform.position, trsTarget.position);
        clearEffect();
    }

    // Update is called once per frame
    void Update()
    {
        gunPoint();
        shoot();
    }

    private void gunPoint() {
        if (Physics.Raycast(camMain.transform.position, camMain.transform.forward, out hit, distance, LayerMask.GetMask("Ground")))
        {
            
            transform.LookAt(hitPosition);
        }
    }

    private void shoot()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            lineRenderer.SetPosition(0, lineRenderer.transform.position);
            // Z-Fighting Problem, need to edit Z-value 
            // Hole Rotation not adjusted.
            // Instantiate(fabHitHole, hitPosition, Quaternion.identity);

            Debug.Log($"point : {hit.point}");
            Debug.Log($"normal : {hit.normal}");
            // Instantiate(fabHitHole, hit.point + hit.normal * 0.001f, Quaternion.);

            // Yet, Because of Renderging Priority. need to sorting Type Change in fab script..
            //GameObject go = Instantiate(fabHitHole, hit.point + hit.normal * 0.001f, Quaternion.identity);
            //hitHole goSc = go.GetComponent<hitHole>();
            //goSc.SetSorting(shootCounter++);
            //if (shootCounter >= short.MaxValue)
            //{
            //    shootCounter = 0;
            //}

            // Hole Fab Rotating Problem exist,
            GameObject go = Instantiate(fabHitHole, hit.point + hit.normal * 0.001f, Quaternion.FromToRotation(Vector3.forward, hit.normal));
            // hit pointed rotation.
            hitHole goSc = go.GetComponent<hitHole>();
            goSc.SetSorting(shootCounter++);
            if (shootCounter >= short.MaxValue)
            {
                shootCounter = 0;
            }

            listLight[0].transform.position = hit.point + hit.normal * 0.01f;
            listLight[0].enabled = true;
            listLight[1].enabled = true;

            lineRenderer.SetPosition(1, listLight[0].transform.position);
            lineRenderer.enabled = true;

            Invoke("clearEffect", 0.1f);
        }
    }



    private void clearEffect() {
        listLight[0].enabled = false;
        listLight[1].enabled = false;
    }
}
