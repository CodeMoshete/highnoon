using UnityEngine;

public class ShakeGameObjectAction : CustomAction
{
    private const float MULT_A = 50f;
    private const float MULT_B = 65f;
    private const float MULT_C = 125f;

    public Transform Target;
    public float Duration;
    public float Intensity = 0.1f;
    public bool AffectX;
    public bool AffectY;
    public bool AffectZ;

    private Vector3 initialPos;
    private float currentTime;
    private float totalDuration;
    private bool isInitialized;

    private float yOffset;
    private float zOffset;

    public override void Initiate()
    {
        totalDuration = currentTime = Duration;
        isInitialized = true;
        initialPos = Target.transform.localPosition;
        yOffset = Random.Range(0f, 2 * Mathf.PI);
        zOffset = Random.Range(0f, 2 * Mathf.PI);
    }

    public void Update ()
    {
        if (isInitialized)
        {
            // y = (h(x - d) ^ 2(\cos(ax) +\sin(bx) +\ \sin\left(cx\right))) / d ^ 2

            currentTime -= Time.deltaTime;
            if (currentTime <= 0f)
            {
                isInitialized = false;
                Target.transform.localPosition = initialPos;
            }

            float pct = 1f - (currentTime / totalDuration);
            Vector3 objectOffset = Vector3.zero;
            if (AffectX)
            {
                objectOffset.x = (Intensity * Mathf.Pow(pct - 1f, 2f) * (Mathf.Cos(MULT_A * pct) + Mathf.Sin(MULT_B * pct) + Mathf.Sin(MULT_C * pct))) / 1f;
            }

            if (AffectY)
            {
                objectOffset.y = (Intensity * Mathf.Pow(pct - 1f, 2f) * (Mathf.Cos(MULT_A * (pct + yOffset)) + Mathf.Sin(MULT_B * (pct + yOffset)) + Mathf.Sin(MULT_C * (pct + yOffset)))) / 1f;
            }

            if (AffectZ)
            {
                objectOffset.z = (Intensity * Mathf.Pow(pct - 1f, 2f) * (Mathf.Cos(MULT_A * (pct + zOffset)) + Mathf.Sin(MULT_B * (pct + zOffset)) + Mathf.Sin(MULT_C * (pct + zOffset)))) / 1f;
            }

            Target.transform.localPosition = initialPos + objectOffset;
        }
    }
}
