Shader "MeowToon/ColorShade"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1)
        _Strength("Strength", Range(0, 1)) = 0.4
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha 
        Pass
        {
            Name "COLOR_SHADE"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float _Strength;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.uv = v.uv;
                return o;
            }

            float4 _Color;

            fixed4 frag(v2f i) : SV_Target
            {
                float3 l = normalize(_WorldSpaceLightPos0.xyz);
                float3 n = normalize(i.worldNormal);
                float interpolation = step(dot(n, l), 0);
                float4 final_color = lerp(_Color, (1 - _Strength) * _Color, interpolation);
                final_color.a = _Color.a;
                return final_color;
            }
            ENDCG
        }
    }
}