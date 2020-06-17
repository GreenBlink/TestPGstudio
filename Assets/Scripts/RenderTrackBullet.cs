using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTrackBullet : MonoBehaviour
{
    private Terrain terrain;

    public LineRenderer lineRenderer;
    public Transform zonaDamage;
    public SpriteRenderer spriteRendererZone;
    public int stepsLine = 100;
    
    private void Awake()
    {
        lineRenderer.positionCount = stepsLine;
    }

    public void ShowTrack(Vector3 startPosition, Vector3 targetVector)
    {
        Vector3[] points = new Vector3[stepsLine];

        for (int i = 0; i < points.Length; i++)
        {
            float time = i * .1f;

            points[i] = startPosition + targetVector * time + Physics.gravity * time * time / 2f;

            if (points[i].y < Terrain.activeTerrain.GetPosition().y)
            {
                lineRenderer.positionCount = i;
                SetPositionZonaDamage(points[i]);
                break;
            }
        }

        lineRenderer.SetPositions(points);
    }

    public bool CheakShootWeapon()
    {
        return Terrain.activeTerrain.terrainData.bounds.max.x > zonaDamage.transform.position.x && Terrain.activeTerrain.terrainData.bounds.max.z > zonaDamage.transform.position.z
             && Terrain.activeTerrain.terrainData.bounds.min.x < zonaDamage.transform.position.x && Terrain.activeTerrain.terrainData.bounds.min.z < zonaDamage.transform.position.z;
    }

    public void ShowStatusWeapon(bool isAllowed)
    {
        spriteRendererZone.color = isAllowed ? Color.green : Color.red;
        lineRenderer.endColor = isAllowed ? Color.green : Color.red;
    }

    private void SetPositionZonaDamage(Vector3 position)
    {
        zonaDamage.transform.position = new Vector3(position.x, 0.1f, position.z);
    }
}
