
using System.Collections;
using UnityEngine;

public static class Utils
{
    public static Vector2Int ToV2I(Vector3 v)
    {
        return new Vector2Int((int)v.x, (int)v.y);
    }

    public static void SetXY(ref Vector3 v, Vector2Int xy)
    {
        v.x = xy.x;
        v.y = xy.y;
    }

    public static bool StringNotEmpty(string str)
    {
        return str != null && str.Length > 0;
    }

    public static IEnumerator MoveRigidBody(Rigidbody2D rb, Vector2 displacement)
    {
        rb.linearVelocity = displacement / PlayerController.TURN_CHANGE_COOLDOWN;
        yield return new WaitForSeconds(PlayerController.TURN_CHANGE_COOLDOWN);
        rb.linearVelocity = Vector2.zero;
    }
}