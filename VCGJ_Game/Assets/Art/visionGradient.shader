Shader "Custom/FOVGradientShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,0,0,1)
        _Range ("Range", Float) = 5.0
        _MinAlpha ("Minimum Alpha", Range(0, 1)) = 0.3
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 100

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            fixed4 _Color;
            float _Range;
            float _MinAlpha;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.vertex.xy;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float dist = length(i.uv) / _Range;
                dist = saturate(dist);

                // Simple linear fade
                float finalAlpha = lerp(_Color.a, _Color.a * _MinAlpha, dist);

                return fixed4(_Color.rgb, finalAlpha);
            }
            ENDCG
        }
    }
}
