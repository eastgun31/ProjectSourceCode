Shader "Custom/StancilMask"
{
	Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        // _Glossiness ("Smoothness", Range(0,1)) = 0.5
        // _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry-100" }
        //ColorMask 0
        ZWrite off
        //Cull off
        LOD 200


        Stencil
        {
            Ref 1
            Pass replace
        }

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        // half _Glossiness;
        // half _Metallic;
        fixed4 _Color;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            if (c.a > 0.5) // 시야 내의 영역
            {
                // 시야 내의 영역은 투명하게 설정합니다.
                o.Albedo = c.rgb;
                o.Alpha = 0;
            }
            else // 시야 외의 영역
            {
                // 시야 외의 영역은 검정색으로 설정합니다.
                o.Albedo = fixed3(0, 0, 0); // 검정색으로 설정합니다.
                o.Alpha = 1; // 불투명하게 설정합니다.
            }
        }
        ENDCG
    }
    FallBack "Diffuse"
}
