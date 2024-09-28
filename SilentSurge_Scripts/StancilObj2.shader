Shader "Custom/StancilObj2"
{
	Properties
    {
        _Color ("Color", Color) = (0,0,0,0)
        //_MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Alpha ("Alpha", Range(0,1)) = 1.0
        // _Glossiness ("Smoothness", Range(0,1)) = 0.5
        // _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
        #pragma surface surf Standard alpha:blend
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        // half _Glossiness;
        // half _Metallic;
        fixed4 _Color;
        float _Alpha;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // o.Metallic = _Metallic;
            // o.Smoothness = _Glossiness;
            o.Alpha =  _Alpha;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
