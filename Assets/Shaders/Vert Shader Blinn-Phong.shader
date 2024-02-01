Shader "Custom/Vert Shader Blinn-Phong"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainCol("��������ɫ", color) = (1.0, 1.0, 1.0, 1.0)
        _SpecularCol("�߹���ɫ", color) = (1.0, 1.0, 1.0, 1.0)
        _SpecularPow("�߹����",range(1, 90)) = 30
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

            //���GPU InstanceԤ����ָ��
            #pragma multi_compile_instancing

            uniform float3 _MainCol;
            uniform float3 _SpecularCol;
            uniform float _SpecularPow;

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float3 normal : NORMAL;

                //Ϊ����ʵ����һ��ID
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 color : COLOR;
                float4 posCS : SV_POSITION; //�ü��ռ��µ�����
                float4 posWS : TEXCOORD0; //����ռ��µ�����
                float3 nDirWS : TEXCOORD1; //��������

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

                o.posCS = UnityObjectToClipPos(v.vertex); //����ת�����ü��ռ�
                o.posWS = mul(unity_ObjectToWorld, v.vertex); //����λ��λ��ת��������ռ�
                o.nDirWS = UnityObjectToWorldNormal(v.normal); //����ת��������ռ�
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //��ʼ��������Ϣ
                UNITY_SETUP_INSTANCE_ID(i);
                // ׼������
                float3 nDir = i.nDirWS; //��������
                float3 lDir = _WorldSpaceLightPos0.xyz; //���շ���
                float3 vDir = normalize(_WorldSpaceCameraPos.xyz - i.posWS); //�ӽǷ��������λ��-����λ�þ����ӽǷ���
                float3 hDir = normalize(vDir + lDir); //�ӽǷ���͹��շ�����м䷽��
                //׼��������
                float ndotl = dot(nDir, lDir);
                float ndoth = dot(nDir, hDir);
                //����ģ�ͼ���
                float3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb; //������
                float3 diffuse = _LightColor0.rgb * _MainCol * max(0, ndotl); //������
                float3 specular = _LightColor0.rgb * _SpecularCol * pow(max(0, ndoth), _SpecularPow); //�߹�
                float3 blinnPhong = ambient + diffuse + specular + i.color; //������ɫ

                return float4(blinnPhong, 1.0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
