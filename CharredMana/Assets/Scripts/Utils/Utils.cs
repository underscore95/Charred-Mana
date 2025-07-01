
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

    public static float HashTo01(int hash)
    {
        uint uhash = (uint)hash;
        return (uhash & 0xFFFFFF) / (float)(1 << 24);
    }

    private static int StableHash(int x)
    {
        unchecked
        {
            x = (x ^ 61) ^ (x >> 16);
            x = x + (x << 3);
            x = x ^ (x >> 4);
            x = x * 0x27d4eb2d;
            x = x ^ (x >> 15);
            return x;
        }
    }

    private static int StableHash(int x, int y)
    {
        return StableHash(x * 73856093 ^ y * 19349663);
    }

    private static int StableHash(int x, int y, int z)
    {
        return StableHash(x * 73856093 ^ y * 19349663 ^ z * 83492791);
    }

    private static int StableHash(float f)
    {
        return StableHash(BitConverter.SingleToInt32Bits(f));
    }

    private static int StableHash(float x, float y)
    {
        return StableHash(StableHash(x), StableHash(y));
    }

    private static int StableHash(float x, float y, float z)
    {
        return StableHash(StableHash(x), StableHash(y), StableHash(z));
    }

    public static float DeterministicRandom(float x)
    {
        return HashTo01(StableHash(x));
    }

    public static float DeterministicRandom(float x, float y)
    {
        return HashTo01(StableHash(x, y));
    }

    public static float DeterministicRandom(float x, float y, float z)
    {
        return HashTo01(StableHash(x, y, z));
    }

    public static float DeterministicRandom(int x)
    {
        return HashTo01(StableHash(x));
    }

    public static float DeterministicRandom(int x, int y)
    {
        return HashTo01(StableHash(x, y));
    }

    public static float DeterministicRandom(int x, int y, int z)
    {
        return HashTo01(StableHash(x, y, z));
    }

}