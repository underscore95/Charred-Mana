Shader "Unlit/UnlitOutlineCircle"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
{
    Tags { "Queue"="Transparent" "RenderType"="Transparent" }
    LOD 100

    Pass
    {
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile_fog
        #include "UnityCG.cginc"

        struct appdata
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
            float4 color : COLOR;
        };

        struct v2f
        {
            float2 uv : TEXCOORD0;
            UNITY_FOG_COORDS(1)
            float4 vertex : SV_POSITION;
            float4 color : COLOR;
        };

        sampler2D _MainTex;
        float4 _MainTex_ST;
        float _CircleRadius = 0.5; 
        float _CircleOutlineWidth = 0.05;

        v2f vert (appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = TRANSFORM_TEX(v.uv, _MainTex);
            o.color = v.color;
            UNITY_TRANSFER_FOG(o, o.vertex);
            return o;
        }

        fixed4 frag (v2f i) : SV_Target
        {
            // Discard fragmnets outside of circle
            float2 centeredUV = 2.0 * (i.uv - 0.5);
            float dist = length(centeredUV);
            float diff = abs(_CircleRadius - dist);
            if (diff >= _CircleOutlineWidth)
               clip(-1); // discard pixel

            fixed4 col = tex2D(_MainTex, i.uv) * i.color;
            UNITY_APPLY_FOG(i.fogCoord, col);
            return col;
        }
        ENDCG
    }
}

}
