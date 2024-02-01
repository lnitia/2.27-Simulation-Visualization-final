Shader "Custom/Vert Shader Blinn-Phong"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainCol("漫反射颜色", color) = (1.0, 1.0, 1.0, 1.0)
        _SpecularCol("高光颜色", color) = (1.0, 1.0, 1.0, 1.0)
        _SpecularPow("高光次幂",range(1, 90)) = 30
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Cull off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            //添加GPU Instance预处理指令
            #pragma multi_compile_instancing

            uniform float3 _MainCol;
            uniform float3 _SpecularCol;
            uniform float _SpecularPow;

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float3 normal : NORMAL;

                //为顶点实例化一个ID
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 color : COLOR;
                float4 posCS : SV_POSITION; //裁剪空间下的坐标
                float4 posWS : TEXCOORD0; //世界空间下的坐标
                float3 nDirWS : TEXCOORD1; //法线向量

                //为顶点实例化一个ID
                UNITY_VERTEX_INPUT_INSTANCE_ID

                //在顶点着色器声明立体目标眼睛字段输出结构(与普通的GPU Instancing不一样的地方，xR的single-Pass-Instanced需要添加这个宏)
                UNITY_VERTEX_OUTPUT_STEREO

            };

            v2f vert (appdata v)
            {
                //初始化顶点信息
                UNITY_SETUP_INSTANCE_ID(v);

                v2f o;

                //在顶点程序中，将实例ID从输入结构复制到输出结构
                UNITY_TRANSFER_INSTANCE_ID(v,o);
                //分配立体目标的眼睛(与普通的GPU Instancing不一样的地方，XR的SingLe-Pass-Instanced需要添加这个宏
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.posCS = UnityObjectToClipPos(v.vertex); //顶点转换到裁剪空间
                o.posWS = mul(unity_ObjectToWorld, v.vertex); //顶点位置位置转化到世界空间
                o.nDirWS = UnityObjectToWorldNormal(v.normal); //法线转换到世界空间
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //初始化顶点信息
                UNITY_SETUP_INSTANCE_ID(i);
                // 准备向量
                float3 nDir = i.nDirWS; //法线向量
                float3 lDir = _WorldSpaceLightPos0.xyz; //光照方向
                float3 vDir = normalize(_WorldSpaceCameraPos.xyz - i.posWS); //视角方向，用相机位置-顶点位置就是视角方向
                float3 hDir = normalize(vDir + lDir); //视角方向和光照方向的中间方向
                //准备点积结果
                float ndotl = dot(nDir, lDir);
                float ndoth = dot(nDir, hDir);
                //光照模型计算
                float3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb; //环境光
                float3 diffuse = _LightColor0.rgb * _MainCol * max(0, ndotl); //漫反射
                float3 specular = _LightColor0.rgb * _SpecularCol * pow(max(0, ndoth), _SpecularPow); //高光
                float3 blinnPhong = ambient + diffuse + specular + i.color; //最终颜色

                return float4(blinnPhong, 1.0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
