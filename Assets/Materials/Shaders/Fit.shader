Shader "Hidden/Fit" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _AspectRatio("Aspect Ratio", Float) = 1.0
        _SrcAspectRatio("Source Aspect Ratio", Float) = 1.0
    }
        SubShader{
            Cull Off
            ZWrite On
            ZTest Always
            Blend One OneMinusSrcAlpha

            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                struct appdata {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                sampler2D _MainTex;
                float _AspectRatio;
                float _SrcAspectRatio;

                half4 frag(v2f i) : SV_Target{
                    float2 remappedUv = float2((1.0 - i.uv.y) * _SrcAspectRatio,
                    (i.uv.x + _AspectRatio / 2.0) * _AspectRatio);
                    return tex2D(_MainTex, remappedUv);
                }
                ENDCG
            }
    }
}
