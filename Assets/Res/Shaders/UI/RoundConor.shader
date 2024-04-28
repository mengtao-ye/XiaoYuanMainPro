Shader "Custom/UI/RoundConor"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Alpha("Alpha", Range(0, 1)) = 1

        _StencilComp("Stencil Comparison", Float) = 8
        _Stencil("Stencil ID", Float) = 0
        _StencilOp("Stencil Operation", Float) = 0
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255
        _ColorMask("Color Mask", Float) = 15

        _RoundedCorner("Round",Vector) = (8,8,8,8)
        _Width("View Width", Float) = 200
        _Height("View Height", Float) = 200
        _BorderWidth("Border Width", Float) = 1
        _BorderColor("Boader Color", Color) = (1, 0, 0, 1)
    }

        SubShader
        {
            Tags
            {
                "Queue" = "Transparent"
                "IgnoreProjector" = "True"
                "RenderType" = "Transparent"
                "PreviewType" = "Plane"
                "CanUseSpriteAtlas" = "True"
            }

            Stencil
            {
                Ref[_Stencil]
                Comp[_StencilComp]
                Pass[_StencilOp]
                ReadMask[_StencilReadMask]
                WriteMask[_StencilWriteMask]
            }

            Cull Off
            Lighting Off
            ZWrite Off
            ZTest[unity_GUIZTestMode]
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask[_ColorMask]

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"
                #include "UnityUI.cginc"

                #pragma multi_compile __ UNITY_UI_ALPHACLIP

                struct appdata_t
                {
                    float4 vertex :
                    POSITION;
                    float4 color :
                    COLOR;
                    float2 texcoord :
                    TEXCOORD0;
                };

                struct v2f
                {
                    float4 vertex :
                    SV_POSITION;
                    fixed4 color :
                    COLOR;
                    half2 texcoord :
                    TEXCOORD0;
                    float4 worldPosition :
                    TEXCOORD1;
                };
     float _Alpha;

                fixed4 _TextureSampleAdd;
                float4 _ClipRect;

                float4 _RoundedCorner;
                float _Width;
                float _Height;
                float _BorderWidth;
                float4 _BorderColor;

                float4 _MainTex_TexelSize; //����Ĵ�С������û������ֻ�ж�����ɫ

                v2f vert(appdata_t IN)
                {
                    v2f OUT;
                    OUT.worldPosition = IN.vertex;
                    OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                    OUT.texcoord = IN.texcoord;

                    OUT.color = IN.color;
                    return OUT;
                }

                sampler2D _MainTex;

                fixed4 frag(v2f IN) : SV_Target
                {
                    half4 color = IN.color;
                    if (_MainTex_TexelSize.z > 0)
                    {
                        //����������ɫ�������ȡ�� �����Ӷ�����ɫ
                        color = (tex2D(_MainTex, IN.texcoord)) * IN.color;
                    }
                   color.a = color.a = _Alpha;
                   //float width = _MainTex_TexelSize.z;
                   //float height = _MainTex_TexelSize.w;

                   float width = _Width;
                   float height = _Height;

                   if (width <= 0 && _MainTex_TexelSize.z > 0)
                   {
                       //���û�����ȣ��������ֶ����˿�ȣ���������ȶ�ȡ
                       width = _MainTex_TexelSize.z;
                   }
                   if (height <= 0 && _MainTex_TexelSize.w > 0)
                   {
                       //ͬ��
                       height = _MainTex_TexelSize.w;
                   }

                   float border_width = _BorderWidth;
                   half4 border_color = _BorderColor;

                   float x = IN.texcoord.x * width;
                   float y = IN.texcoord.y * height;


                   float arc_size = 0;
                   float r = _RoundedCorner.w;
                   //���½�
                   if (x < r && y < r)
                   {
                       arc_size = (x - r) * (x - r) + (y - r) * (y - r);
                       if (arc_size > r * r)
                       {
                           color.a = 0;
                       }
                       else if (border_width > 0 && arc_size > (r - border_width) * (r - border_width))
                       {
                           color = border_color;
                       }
                   }
                   r = _RoundedCorner.x;
                   //���Ͻ�
                   if (x < r && y >(height - r))
                   {
                       arc_size = (x - r) * (x - r) + (y - (height - r)) * (y - (height - r));
                       if (arc_size > r * r)
                       {
                           color.a = 0;
                       }
                       else if (border_width > 0 && arc_size > (r - border_width) * (r - border_width))
                       {
                           color = border_color;
                       }
                   }
                   r = _RoundedCorner.z;
                   //���½�
                   if (x > (width - r) && y < r)
                   {
                       arc_size = (x - (width - r)) * (x - (width - r)) + (y - r) * (y - r);
                       if (arc_size > r * r)
                       {
                           color.a = 0;
                       }
                       else if (border_width > 0 && arc_size > (r - border_width) * (r - border_width))
                       {
                           color = border_color;
                       }
                   }
                   r = _RoundedCorner.y;
                   //���Ͻ�
                   if (x > (width - r) && y > (height - r))
                   {
                       arc_size = (x - (width - r)) * (x - (width - r)) + (y - (height - r)) * (y - (height - r));
                       if (arc_size > r * r)
                       {
                           color.a = 0;
                       }
                       else if (border_width > 0 && arc_size > (r - border_width) * (r - border_width))
                       {
                           color = border_color;
                       }
                   }

                   if (border_width > 0)
                   {
                       //�±�ֱ������
                       if (x > _RoundedCorner.w && x < (width - _RoundedCorner.z) && y < border_width)
                       {
                           color = border_color;
                       }
                       //�ϱ�ֱ������
                       if (x > _RoundedCorner.x && x < (width - _RoundedCorner.y) && (height - y) < border_width)
                       {
                           color = border_color;
                       }
                       //���ֱ������
                       if (y > _RoundedCorner.w && y < (height - _RoundedCorner.x) && x < border_width)
                       {
                           color = border_color;
                       }
                       //�ұ�ֱ������
                       if (y > _RoundedCorner.z && y < (height - _RoundedCorner.y) && x >(width - border_width))
                       {
                           color = border_color;
                       }
                   }

                   return color;
               }
               ENDCG
           }
        }
}