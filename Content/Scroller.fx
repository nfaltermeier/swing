// Scrolling Texture Shader - v2 (Tilemap Support)
// By Nick Monaco

#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;

float Time;
float2 ScrollDirection;	// Relies on being normalized
float ScrollSpeed;

float2 TilemapDims;		// Dimensions in tiles
float2 TilemapCoords;	// Tile coordinates of where it should be localized

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
	MinFilter = None;
	MagFilter = None;
	MipFilter = None;
	AddressU = Wrap;
	AddressV = Wrap;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};


// Returns the sign of a value, but if 0 then returns 1
float2 OneSign(float2 value) {
	float2 signVec = sign(value);
	if (signVec.x == 0) signVec.x = 1;
	if (signVec.y == 0) signVec.y = 1;
	return signVec;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	// Get the time to be above 1 and out of the first cycle
	float realTime = 1 + Time + (1.0f / ScrollSpeed);

	// Get the amount we're scrolling the system
	float2 scrollAmount = float2(realTime * ScrollSpeed * ScrollDirection.x,
								 realTime * ScrollSpeed * -ScrollDirection.y);

	// Get the size of a tile (in both dimensions)
	float2 scale = 1.0f / TilemapDims;

	// Get the tile we're looping
	float2 tileStartPos = TilemapCoords / TilemapDims;

	// Get how much we move on the image
	float2 pixelTilemapPos = input.TextureCoordinates / TilemapDims;

	// Check if the negative modifiers need to be added
	if (scrollAmount.x < 0) { 
		scrollAmount.x += 1; 
		tileStartPos.x += scale.x;
	}
	if (scrollAmount.y < 0) {
		scrollAmount.y += 1;
		tileStartPos.y += scale.y;
	}

	// Create the final position we draw
	float2 finalCoords = tileStartPos + ((pixelTilemapPos + scrollAmount) % (scale * OneSign(scrollAmount)));

	// Return that pixel of the image
	return tex2D(SpriteTextureSampler, finalCoords) * input.Color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};