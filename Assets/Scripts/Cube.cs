using UnityEngine;

public enum SliceDirection
{
    Up, Down, Left, Right, Forward, Backward
}

public class Cube : MonoBehaviour
{
    public SliceDirection requiredSliceDirection = SliceDirection.Down; // Default slice direction

    // Method to check if the slicing direction is correct
    public bool IsCorrectSliceDirection(Vector3 sliceVelocity)
    {
        Vector3 localSliceDirection = transform.InverseTransformDirection(sliceVelocity.normalized);

        // Check if the local slice direction aligns with the required slice direction
        switch (requiredSliceDirection)
        {
            case SliceDirection.Up:
                return localSliceDirection.y > 0.9;
            case SliceDirection.Down:
                return localSliceDirection.y < -0.9;
            case SliceDirection.Left:
                return localSliceDirection.x < -0.9;
            case SliceDirection.Right:
                return localSliceDirection.x > 0.9;
            case SliceDirection.Forward:
                return localSliceDirection.z > 0.9;
            case SliceDirection.Backward:
                return localSliceDirection.z < -0.9;
            default:
                return false;
        }
    }
}