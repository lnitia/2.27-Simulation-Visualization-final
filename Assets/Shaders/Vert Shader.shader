Shader "Custom/Vert Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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

            //添加GPU Instance预处理指令
            #pragma multi_compile_instancing


            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;

                //为顶点实例化一个ID
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 color : COLOR;
                float4 vertex : SV_POSITION;

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

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //初始化页点信息
                UNITY_SETUP_INSTANCE_ID(i);
                return i.color;
            }
            ENDCG
        }
    }
}
