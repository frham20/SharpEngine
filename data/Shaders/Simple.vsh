uniform float4x4 gViewProjMatrix : register(c0);
uniform float4x4 gWorldMatrix : register(c4);
uniform float4x4 gWorldInvTransMatrix : register(c8);
uniform float4x4 gViewInvMatrix : register(c12);

struct VS_INPUT
{
	float3 pos : POSITION;
	float3 normal : NORMAL;
	float2 uv : TEXCOORD0;
};

struct VS_OUTPUT
{
	float4 pos : POSITION;
	float4 color : COLOR0;
	float2 uv : TEXCOORD0;
	float3 normal : TEXCOORD1;
};

VS_OUTPUT VertexShaderMain(VS_INPUT IN)
{
	VS_OUTPUT OUT;
	
	//generate pre-shader
	float4x4 preWorldViewProjMatrix = mul(gWorldMatrix, gViewProjMatrix);
	
	OUT.normal = normalize(mul(gWorldInvTransMatrix, IN.normal));
	OUT.color = float4(1.0,1.0,1.0,1.0);
	OUT.pos = mul(preWorldViewProjMatrix, float4(IN.pos, 1.0));
	OUT.uv = IN.uv;

	return OUT;
}
