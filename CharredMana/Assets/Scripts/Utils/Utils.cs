
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

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

    public static Vector3 ToV3(Vector2 xy, int z = 0)
    {
        return new(xy.x, xy.y, z);
    }

    // returns value between 0 and 1
    // imagine it like something fading in, staying for a bit at full opacity, then fading out, where this returns the alpha and elapsed is time since started fading in
    public static float GetTransitionValueInOut(float elapsed, float transitionIn, float stay, float transitionOut)
    {
        if (elapsed <= 0 || elapsed >= transitionIn + stay + transitionOut) return 0;
        if (elapsed >= transitionIn && elapsed <= transitionIn + stay) return 1;
        if (elapsed < transitionIn) return Mathf.InverseLerp(0, transitionIn, elapsed);
        else return Mathf.InverseLerp(transitionOut, 0, elapsed - transitionIn - stay);
    }

    public static System.Random CreateRandom()
    {
        return new((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
    }

    // Returns a uniform float between 0 and 1, deterministic.
    public static float WhiteNoise(int x, int y)
    {
        uint seed = (uint)(x * 374761393 + y * 668265263);
        seed ^= seed >> 13;
        seed *= 1274126177;
        seed ^= seed >> 16;
        return (seed & 0xFFFFFF) / (float)0x1000000;
    }

    // Returns a uniform float between 0 and 1, deterministic.
    public static float WhiteNoise(int x)
    {
        uint seed = (uint)(x * 374761393);
        seed ^= seed >> 13;
        seed *= 1274126177;
        seed ^= seed >> 16;
        return (seed & 0xFFFFFF) / (float)0x1000000;
    }
}