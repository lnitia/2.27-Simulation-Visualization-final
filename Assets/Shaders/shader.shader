Shader "Custom/Shader"
{
    Properties
    {
      _MainTex("Base(RGB)",2D) = "white"{}
    }
        SubShader
    {
      Tags{"RenderType" = "Opaque"}
      LOD 200

      Cull off

      CGPROGRAM
      #pragma surface surf Lambert vertex:vert//(1)

      sampler2D _MainTex;

      struct Input
      {
        float2 uv_MainTex;
        float4 vertColor;//(2)
      };

      void vert(inout appdata_full v , out Input o)//(3)
      {
        UNITY_INITIALIZE_OUTPUT(Input,o);//(4)
        o.vertColor = v.color;//(5)
      }

      void surf(Input IN,inout SurfaceOutput o)
      {
        o.Albedo = IN.vertColor.rgb * tex2D(_MainTex,IN.uv_MainTex).rgb;//(6)
      }
      ENDCG

    }
        FallBack"diffuse"
}