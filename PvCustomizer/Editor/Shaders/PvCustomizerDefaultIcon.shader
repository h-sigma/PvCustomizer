Shader "Hidden/PvCustomizer/DefaultIcon"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Tint ("Tint", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "PreviewType" = "Plane"
        }
        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ScaleTiling;
            float4 _Tint;
            float _TintAmount;
            int _Invert;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * _MainTex_ScaleTiling.xy + _MainTex_ScaleTiling.zw;
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float4 _mainTexSample = tex2D(_MainTex, i.uv);
                fixed4 col = _mainTexSample * i.color;

                col.rgb = lerp(col.rgb, col.rgb * _Tint, saturate(_TintAmount));
                if (_Invert == 1)
                {
                    col.rgb = 1 - col.rgb;
                }
                return col;
            }
            ENDCG
        }
    }
}