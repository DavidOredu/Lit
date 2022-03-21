using UnityEngine;

/// <summary>
/// Class responsible for holding AI sensory data and logic.
/// </summary>
[System.Serializable]
public class AISensor
{
    /// <summary>
    /// The transform object of the sensor, where the sensor lies.
    /// </summary>
    public Transform sensorTransform;

    /// <summary>
    /// If is a linear sensor, determine it's distance.
    /// </summary>
    public float sensorDistance;

    /// <summary>
    /// If is a linear sensor, determine it's direction.
    /// </summary>
    public Vector2 sensorDirection;

    /// <summary>
    /// If is a radial sensor, determine it's radius.
    /// </summary>
    public float sensorRadius;

    /// <summary>
    /// Should the sensor detect all object of the type in its detection range?
    /// </summary>
    public bool detectAll;

    /// <summary>
    /// Layer for objects to be detected by sensor.
    /// </summary>
    public LayerMask whatToDetect;

    /// <summary>
    /// Latest accepted detection made by the sensor.
    /// </summary>
    public dynamic currentDetection;
    /// <summary>
    /// The new detection made by the sensor, waiting to be processed and accepted.
    /// </summary>
    public dynamic newDetection;

    public dynamic cacheDetection;

    /// <summary>
    /// True if a new detection has been made. False else.
    /// </summary>
    public bool hasNewDetection;

    private bool firstDetection;

    /// <summary>
    /// The type of sensor logic used.
    /// </summary>
    public SensorType sensorType;

    /// <summary>
    /// Initialize the AISensor class.
    /// </summary>
    /// <param name="sensorTransform">The transform for the sensor.</param>
    /// <param name="whatToDetect">The LayerMask of what the sensor should detect.</param>
    /// <param name="sensorType">The method of sensing used by the sensor. Is it linear or radial?</param>
    /// <param name="sensorDistance">If is a linear sensor, determine it's distance.</param>
    /// <param name="sensorDirection">If is a linear sensor, determine it's direction.</param>
    /// <param name="sensorRadius">If is a radial sensor, determine it's radius.</param>
    public AISensor(Transform sensorTransform, LayerMask whatToDetect, SensorType sensorType, float sensorDistance = 0, Vector2 sensorDirection = default, float sensorRadius = 0f, bool detectAll = false)
    {
        this.sensorTransform = sensorTransform;
        this.whatToDetect = whatToDetect;
        this.sensorType = sensorType;
        this.sensorDistance = sensorDistance;
        this.sensorDirection = sensorDirection;
        this.sensorRadius = sensorRadius;
        this.detectAll = detectAll;

        switch (sensorType)
        {
            case SensorType.Linear:
                if (detectAll)
                    cacheDetection = new RaycastHit2D[0];
                else
                    cacheDetection = new RaycastHit2D();
                break;
            case SensorType.Radial:
                if (detectAll)
                    cacheDetection = new Collider2D[0];
                else
                    cacheDetection = new Collider2D();
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// Function called to run detection logic.
    /// </summary>
    /// <param name="detectAll">True if should return all detected objects found in the detection path and False if should return the first found.</param>
    public void Detect()
    {
        dynamic detection;
        switch (sensorType)
        {
            case SensorType.Linear:
                detection = LinearSensor(detectAll);
                if (detection.collider != null)
                {
                    newDetection = detection;
                }
                else
                    return;
                break;
            case SensorType.Radial:
                detection = RadarSensor(detectAll);
                if (detection != null)
                    newDetection = detection;
                else
                    return;
                break;
        }
        if (!firstDetection)
        {
            CheckIfIsFirstDetection();
            hasNewDetection = true;
        }
        else if (IsCurrentDetectionNull())
        {
            currentDetection = cacheDetection;
            hasNewDetection = CheckIfHasMadeNewDetection();
        }
        else
            hasNewDetection = CheckIfHasMadeNewDetection();




    }
    /// <summary>
    /// Function called to run specific search in a defined manner.
    /// </summary>
    public void Search()
    {
        switch (sensorType)
        {
            case SensorType.Linear:
                break;
            case SensorType.Radial:
                break;
            default:
                break;
        }
    }
    #region Detection Functions
    /// <summary>
    /// Radar sensor to detect object within a defined radius.
    /// </summary>
    /// <param name="detectAll">should the sensor detect all object of the layermask found?</param>
    /// <returns>Either a "Collider2D" or "Collider2D[]" object. Discarded if returns null.</returns>
    private dynamic RadarSensor(bool detectAll)
    {
        if (detectAll)
            return Physics2D.OverlapCircleAll(sensorTransform.position, sensorRadius, whatToDetect);
        else
            return Physics2D.OverlapCircle(sensorTransform.position, sensorRadius, whatToDetect);
    }

    /// <summary>
    /// Linear sensor to detect object within a path, with a defined distance.
    /// </summary>
    /// <param name="detectAll">should the sensor detect all object of the layermask found?</param>
    /// <returns>Either a "RaycastHit2D" or "RaycastHit2D[]" object. Discarded if returns null.</returns>
    private dynamic LinearSensor(bool detectAll)
    {
        if (detectAll)
            return Physics2D.RaycastAll(sensorTransform.position, sensorDirection, sensorDistance, whatToDetect);
        else
        {
            var detection = Physics2D.Raycast(sensorTransform.position, sensorDirection, sensorDistance, whatToDetect);

            return detection;
        }
    }
    private dynamic LinearSensor(Transform sensorTransform)
    {
        return Physics2D.Raycast(sensorTransform.position, sensorDirection, sensorDistance, whatToDetect);
    }
    #endregion
    private bool CheckIfHasMadeNewDetection()
    {
        //switch (sensorType)
        //{
        //    case SensorType.Linear:
        //         if (!IsNewDetectionNull() && !IsCurrentDetectionNull())
        //            return newDetection.collider.name != currentDetection.collider.name;
        //        else
        //            return false;
        //    case SensorType.Radial:
        //         if (!IsNewDetectionNull() )
        //            return newDetection.name != currentDetection.name;
        //        else
        //            return false;
        //}
        //return default;
        switch (sensorType)
        {
            case SensorType.Linear:
                return newDetection.collider != currentDetection.collider;
            case SensorType.Radial:
                return newDetection != currentDetection;
        }
        return false;
    }
    private void CheckIfIsFirstDetection()
    {
        currentDetection = newDetection;
        firstDetection = true;
    }
    private dynamic LinearSensorSearch(System.Type type, float searchDistance, float searchIncrement)
    {
        Transform transform = sensorTransform;
        float distance = searchDistance;

        for (float i = 0; i < searchDistance; i += searchIncrement)
        {
            var detection = LinearSensor(transform);

            if (detection.GetType() == type)
            {
                return detection;
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z) + ((Vector3)sensorDirection * searchIncrement);
            }
        }
        return false;
    }
    public T GetComponentFromDetection<T>(dynamic detection)
    {
        switch (sensorType)
        {
            case SensorType.Linear:
                return detection.collider.GetComponent<T>();
            case SensorType.Radial:
                return detection.GetComponent<T>();
            default:
                return default;
        }
    }
    public T GetComponentFromCurrentDetection<T>()
    {
        switch (sensorType)
        {
            case SensorType.Linear:
                return currentDetection.collider.GetComponent<T>();
            case SensorType.Radial:
                return currentDetection.GetComponent<T>();
            default:
                return default;
        }
    }
    public bool IsNewDetectionNull()
    {
        switch (sensorType)
        {
            case SensorType.Linear:
                return newDetection.collider == null;
            case SensorType.Radial:
                return newDetection == null;
        }
        return true;
    }
    public bool IsCurrentDetectionNull()
    {
        switch (sensorType)
        {
            case SensorType.Linear:
                return currentDetection.collider == null;
            case SensorType.Radial:
                return currentDetection == null;
        }
        return true;
    }
    /// <summary>
    /// Accepts new detection and sets it as the current detection.
    /// </summary>
    public void AcceptNewDetection()
    {
        currentDetection = newDetection;
        hasNewDetection = false;
    }
    /// <summary>
    /// Check the tag of a new detection to get the specific nature of a layer element.
    /// </summary>
    /// <returns></returns>
    public string GetNewDetectionTag()
    {
        string tag = default;

        switch (sensorType)
        {
            case SensorType.Linear:
                tag = newDetection.collider.tag;
                break;
            case SensorType.Radial:
                tag = newDetection.tag;
                break;
        }
        return tag;
    }
    public string GetCurrentDetectionTag()
    {
        string tag = default;

        switch (sensorType)
        {
            case SensorType.Linear:
                tag = currentDetection.collider.tag;
                break;
            case SensorType.Radial:
                tag = currentDetection.tag;
                break;
        }
        return tag;
    }
    public bool CompareNewDetectionTag(string tagToCompareWith)
    {
        bool doesCompare = default;

        switch (sensorType)
        {
            case SensorType.Linear:
                doesCompare = newDetection.collider.CompareTag(tagToCompareWith);
                break;
            case SensorType.Radial:
                doesCompare = newDetection.CompareTag(tagToCompareWith);
                break;
            default:
                break;
        }
        return doesCompare;
    }
    public bool CompareCurrentDetectionTag(string tagToCompareWith)
    {
        bool doesCompare = default;

        switch (sensorType)
        {
            case SensorType.Linear:
                doesCompare = currentDetection.collider.CompareTag(tagToCompareWith);
                break;
            case SensorType.Radial:
                doesCompare = currentDetection.CompareTag(tagToCompareWith);
                break;
            default:
                break;
        }
        return doesCompare;
    }
    /// <summary>
    /// Used with "detect-all" radial sensors.
    /// </summary>
    /// <returns>The total number of new detected objects found.</returns>
    public int GetNewDetectionCount()
    {
        return newDetection.Length;
    }
    /// <summary>
    /// Used with "detect-all" radial sensors.
    /// </summary>
    /// <returns>The total number of recently accepted detected objects found.</returns>
    public int GetCurrentDetectionCount()
    {
        return currentDetection.Length;
    }
    public enum SensorType
    {
        /// <summary>
        /// This type of sensor detects an object in it's linear path.
        /// </summary>
        Linear,
        /// <summary>
        /// This type of sensor detects an object within a given radius, at the transform's position.
        /// </summary>
        Radial,
        /// <summary>
        /// This type of sensor detects if an object isn't detected.
        /// </summary>
        Null,
    }

    public void GizmosDebug(Vector2 sensorDirection = default, float sensorDistance = default, float sensorRadius = default)
    {
        switch (sensorType)
        {
            case SensorType.Linear:
                Gizmos.DrawLine(sensorTransform.position, sensorTransform.position + (Vector3)(sensorDirection * sensorDistance));
                break;
            case SensorType.Radial:
                Gizmos.DrawWireSphere(sensorTransform.position, sensorRadius);
                break;
            default:
                break;
        }
    }
}
