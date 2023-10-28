struct Ray
{
    float3 origin;
    float3 direction;
};


void raymarching_float(float3 RayPosition,
                       float3 RayDirection, float3 camerapos, float2 uv, float3 frag_position, out float3 outValue)
{
    const float raymax = 1000.0;
    float3 cameraPos = float3(camerapos.x, camerapos.y, camerapos.z);
    float screenZ = 18.0;
    float3 rayDirection = normalize(frag_position - cameraPos);

    Ray ray;
    ray.origin = cameraPos;
    ray.direction = rayDirection;

    //普通のレイマーチング
    float3 pos = RayPosition;
    float rayHitDistance = 0;
    bool Hit = false;
    for (int i = 0; i < 15; ++i)
    {
        float d = length(pos) - 0.3;
        pos += d * RayDirection;
        rayHitDistance += d;

        if (d < 0.001)
        {
            Hit = true;
            // HitPosition = pos;
            // HitNormal = CalcNormal(pos);
            break;
        }
    }
    //ゴッドレイけいさん
    float fogLitPercent = 0.0f;
    float3 LightDir = float3(1, 1, 0);
    float isShadow = 0.0;
    for (int i = 0; i < 60; ++i)
    {
        float3 testpos = ray.origin + ray.direction * 5.0 * (float(i) / float(60));

        for (int k = 0; k < 10; ++k)
        {
            float d = length(testpos) - 0.3;

            testpos += d * LightDir;
            if (d < 0.001)
            {
                isShadow = 1.0f;
                break;
            }
        }
        float inShadow = (isShadow == 1.0f) ? 1.0f : 0.0f;

        float progress = 1.0f / float(i + 1);
        // fogLitPercent += 0.01 * inShadow;
        fogLitPercent = lerp(fogLitPercent, inShadow, progress);
    }
    float3 c_fogColorUnlit = float3(1, 1, 1);
    float3 c_fogColorLit = float3(0, 0, 0);
    float3 fogColor = lerp(c_fogColorUnlit, c_fogColorLit, fogLitPercent);
    float absorb = exp(-5 * 0.02);
    float3 color = lerp(fogColor, Hit, absorb);

    // outValue = fogLitPercent;
    outValue = fogLitPercent;
}
