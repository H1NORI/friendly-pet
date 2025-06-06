﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceObjectsOnPlane : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }

    /// <summary>
    /// Invoked whenever an object is placed in on a plane.
    /// </summary>
    public static event Action onPlacedObject;

    ARRaycastManager m_RaycastManager;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    [SerializeField]
    int m_MaxNumberOfObjectsToPlace = 1;

    int m_NumberOfPlacedObjects = 0;

    [SerializeField]
    bool m_CanReposition = true;

    public bool canReposition
    {
        get => m_CanReposition;
        set => m_CanReposition = value;
    }

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }

    void Start()
    {
        if (spawnedObject != null)
        {
            spawnedObject.SetActive(false);
        }
    }

    void Update()
    {
        Vector2 inputPosition = default;
        bool isInputTriggered = false;

        // Check for touch input (mobile)
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            inputPosition = Input.GetTouch(0).position;
            isInputTriggered = true;
        }// Check for mouse input (PC)
        else if (Input.GetMouseButtonDown(0))
        {
            inputPosition = Input.mousePosition;
            isInputTriggered = true;
        }

        if (isInputTriggered)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (m_RaycastManager.Raycast(inputPosition, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = s_Hits[0].pose;

                if (m_NumberOfPlacedObjects < m_MaxNumberOfObjectsToPlace)
                {
                    spawnedObject = m_PlacedPrefab;
                    spawnedObject.SetActive(true);
                    spawnedObject.transform.position = hitPose.position;

                    Vector3 targetScale = m_PlacedPrefab.transform.localScale;
                    spawnedObject.transform.localScale = Vector3.zero;
                    FaceObjectToCamera(spawnedObject.transform);
                    StartCoroutine(AnimateScaleIn(spawnedObject.transform, targetScale));

                    m_NumberOfPlacedObjects++;
                }
                else if (m_CanReposition)
                {
                    spawnedObject.transform.position = hitPose.position;
                    Vector3 targetScale = m_PlacedPrefab.transform.localScale;
                    spawnedObject.transform.localScale = Vector3.zero;
                    FaceObjectToCamera(spawnedObject.transform);
                    StartCoroutine(AnimateScaleIn(spawnedObject.transform, targetScale));
                }

                onPlacedObject?.Invoke();
            }
        }

    }

    void FaceObjectToCamera(Transform objTransform)
    {
        var cameraTransform = Camera.main.transform;
        Vector3 directionToCamera = cameraTransform.position - objTransform.position;
        directionToCamera.y = 0;
        if (directionToCamera.sqrMagnitude > 0.001f)
        {
            objTransform.rotation = Quaternion.LookRotation(directionToCamera);
        }
    }

    IEnumerator AnimateScaleIn(Transform objTransform, Vector3 targetScale)
    {
        float duration = 0.3f;
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;
            float smoothT = Mathf.SmoothStep(0, 1, t);
            objTransform.localScale = Vector3.Lerp(Vector3.zero, targetScale, smoothT);
            time += Time.deltaTime;
            yield return null;
        }

        objTransform.localScale = targetScale;
    }
    
    public void RemovePlacedObject()
    {
        if (spawnedObject != null)
        {
            spawnedObject.SetActive(false);
            spawnedObject = null;
            m_NumberOfPlacedObjects = 0;

            UIManager uiManager = FindObjectOfType<UIManager>();
            if (uiManager != null)
            {
                // TODO - rework logic for displaying UI one more time
                uiManager.TestFlipPlacementBool();
                uiManager.AddToQueue(new UXHandle(UIManager.InstructionUI.TapToPlace, UIManager.InstructionGoals.PlacedAnObject));
            }
        }
    }

}
