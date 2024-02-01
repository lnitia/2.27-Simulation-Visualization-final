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

            //���GPU InstanceԤ����ָ��
            #pragma multi_compile_instancing


            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;

                //Ϊ����ʵ����һ��ID
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 color : COLOR;
                float4 vertex : SV_POSITION;

                //Ϊ����ʵ����һ��ID
                UNITY_VERTEX_INPUT_INSTANCE_ID

                //�ڶ�����ɫ����������Ŀ���۾��ֶ�����ṹ(����ͨ��GPU Instancing��һ���ĵط���xR��single-Pass-Instanced��Ҫ��������)
                UNITY_VERTEX_OUTPUT_STEREO

            };

            v2f vert (appdata v)
            {
                //��ʼ��������Ϣ
                UNITY_SETUP_INSTANCE_ID(v);

                v2f o;

                //�ڶ�������У���ʵ��ID������ṹ���Ƶ�����ṹ
                UNITY_TRANSFER_INSTANCE_ID(v,o);
                //��������Ŀ����۾�(����ͨ��GPU Instancing��һ���ĵط���XR��SingLe-Pass-Instanced��Ҫ��������
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //��ʼ��ҳ����Ϣ
                UNITY_SETUP_INSTANCE_ID(i);
                return i.color;
            }
            ENDCG
        }
    }
}
