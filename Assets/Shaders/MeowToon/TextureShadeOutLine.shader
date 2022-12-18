Shader "MeowToon/TextureShadeOutLine"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1)
        _MainTex ("Main Texture", 2D) = "white" {}
        _Strength("Strength", Range(0, 1)) = 0.2
        _OutlineWidth ("Outline width", Range (0.0001, 0.03)) = 0.0005
        [HDR] _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
        [Toggle(USE_VERTEX_EXPANSION)] _UseVertexExpansion("Use vertex for Outline", int) = 0
    }

    SubShader
    {
        UsePass "MeowToon/TextureShade/TEXTURE_SHADE"
        Pass
        {
            Cull Front
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature USE_VERTEX_EXPANSION
            #include "UnityCG.cginc"

            float _OutlineWidth;
            float4 _OutlineColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos:SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                float3 n = 0;

                #ifdef USE_VERTEX_EXPANSION
                
                float3 dir = normalize(v.vertex.xyz);
                n = normalize(mul((float3x3) UNITY_MATRIX_IT_MV, dir));
                
                #else
                
                n = normalize(mul((float3x3) UNITY_MATRIX_IT_MV, v.normal));
                
                #endif

                float2 offset = TransformViewToProjection(n.xy);
                o.pos.xy += offset * _OutlineWidth;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }
    }
}