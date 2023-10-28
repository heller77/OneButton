#define c_lightDir normalize(float3(1.0f, 1.0f, 1.2f))

struct Ray
{
    float3 origin;
    float3 direction;
};

struct RayHitInfo
{
    float dist;
    float3 normal;
    float3 diffuse;
};

float sphereDist(float3 position, float radius)
{
    return length(position) - radius;
}

RayHitInfo sceneDist(float3 position)
{
    RayHitInfo hitinfo;

    float distance = sphereDist(position, 3.5);

    hitinfo.dist = distance;
    return hitinfo;
}


bool RayVsSphere(in float3 rayPos, in float3 rayDir, inout RayHitInfo info, in float4 sphere, in float3 diffuse)
{
    //get the vector from the center of this sphere to where the ray begins.
    float3 m = rayPos - sphere.xyz;

    //get the dot product of the above vector and the ray's vector
    float b = dot(m, rayDir);

    float c = dot(m, m) - sphere.w * sphere.w;

    //exit if r's origin outside s (c > 0) and r pointing away from s (b > 0)
    if (c > 0.0 && b > 0.0)
        return false;

    //calculate discriminant
    float discr = b * b - c;

    //a negative discriminant corresponds to ray missing sphere
    if (discr < 0.0)
        return false;

    //ray now found to intersect sphere, compute smallest t value of intersection
    bool fromInside = false;
    float dist = -b - sqrt(discr);
    if (dist < 0.0f)
    {
        fromInside = true;
        dist = -b + sqrt(discr);
    }

    if (dist > 0.0f && dist < info.dist)
    {
        info.dist = dist;
        info.normal = normalize((rayPos + rayDir * dist) - sphere.xyz) * (fromInside ? -1.0f : 1.0f);
        info.diffuse = diffuse;
        return true;
    }

    return false;
}

RayHitInfo RayVsScene(in float3 rayPos, in float3 rayDir)
{
    const float c_rayMaxDist = 10000.0f;

    RayHitInfo hitInfo;
    hitInfo.dist = c_rayMaxDist;


    // some floating spheres to cast shadows
    RayVsSphere(rayPos, rayDir, hitInfo, float4(0.0f, 0.0f, 0.0f, 1), float3(1.0f, 0.0f, 0.0f));
    RayVsSphere(rayPos, rayDir, hitInfo, float4(1.0f, 2.0f, 0.0f, 1), float3(1.0f, 0.0f, 0.0f));
    return hitInfo;
}


// レイがぶつかった位置における法線を取得
float3 getNormal(float3 position)
{
    float delta = 0.0001;
    float fx = sceneDist(position).dist - sceneDist(float3(position.x - delta, position.y, position.z)).dist;
    float fy = sceneDist(position).dist - sceneDist(float3(position.x, position.y - delta, position.z)).dist;
    float fz = sceneDist(position).dist - sceneDist(float3(position.x, position.y, position.z - delta)).dist;
    return normalize(float3(fx, fy, fz));
}

float3 GetColorForRay(in float3 rayPos, in float3 rayDir, out float hitDistance)
{
    const float c_hitNormalNudge = 0.1f;

    const float c_rayMaxDist = 10000.0f;
    const float3 c_lightColor = float3(1.0f, 0.8f, 0.5f);
    const float3 c_lightAmbient = float3(0.05f, 0.05f, 0.05f);

    // trace primary ray
    RayHitInfo hitInfo = RayVsScene(rayPos, rayDir);

    // set the hitDistance out parameter
    hitDistance = hitInfo.dist;

    if (hitInfo.dist == c_rayMaxDist)
        return 0;

    // trace shadow ray
    float3 hitPos = rayPos + rayDir * hitInfo.dist;
    hitPos += hitInfo.normal * c_hitNormalNudge;
    RayHitInfo shadowHitInfo = RayVsScene(hitPos, c_lightDir);
    float shadowTerm = (shadowHitInfo.dist == c_rayMaxDist) ? 1.0f : 0.0f;

    // do diffuse lighting
    float dp = clamp(dot(hitInfo.normal, c_lightDir), 0.0f, 1.0f);
    return c_lightAmbient * hitInfo.diffuse + dp * hitInfo.diffuse * c_lightColor * shadowTerm;
}


float3 ApplyFog(float3 rayPos, float3 rayDir, float3 pixelColor, float rayHitTime)
{
    float3 lightDir = float3(-1.0, -1.0, -1.0);
    const float c_rayMaxDist = 10000.0f;
    int c_numRayMarchSteps = 16;
    const float3 c_fogColorLit = float3(1.0f, 1.0f, 1.0f);
    const float3 c_fogColorUnlit = float3(0.0f, 0.0f, 0.0f);

    // calculate how much of the ray is in direct light by taking a fixed number of steps down the ray
    // and calculating the percent.
    // Note: in a rasterizer, you'd replace the RayVsScene raytracing with a shadow map lookup!
    float fogLitPercent = 0.0f;
    float startRayOffset = 0.5f;

    for (int i = 0; i < c_numRayMarchSteps; ++i)
    {
        float3 testPos = rayPos + rayDir * rayHitTime * ((float(i) + startRayOffset) / float(c_numRayMarchSteps));
        //ここライトの場所考えてない？
        RayHitInfo shadowHitInfo;
        shadowHitInfo.dist = 1;
        // for (int k = 0; k < c_numRayMarchSteps; k++)
        // {
        //     shadowHitInfo = sceneDist(testPos + -1 * lightDir * shadowHitInfo.dist);
        //     if (k == c_numRayMarchSteps - 1)
        //     {
        //         shadowHitInfo.dist = c_rayMaxDist;
        //         break;
        //     }
        // }


        shadowHitInfo = RayVsScene(testPos, lightDir);
        float inShadow = (shadowHitInfo.dist == c_rayMaxDist) ? 1.0f : 0.0f;
        float progress = 1.0f / float(i + 1);
        fogLitPercent = lerp(fogLitPercent, inShadow, progress);
    }

    float3 fogColor = lerp(c_fogColorUnlit, c_fogColorLit, fogLitPercent);
    float absorb = exp(-rayHitTime * 0.3);
    return lerp(fogColor, pixelColor, absorb);
}

// ray march from the camera to the depth of what the ray hit to do some simple scattering
float3 ApplyFog2(in float3 rayPos, in float3 rayDir, in float3 pixelColor, in float rayHitTime)
{
    const float c_rayMaxDist = 10000.0f;
    int c_numRayMarchSteps = 16;
    const float3 c_fogColorLit = float3(1.0f, 1.0f, 1.0f);
    const float3 c_fogColorUnlit = float3(0.0f, 0.0f, 0.0f);


    float startRayOffset = 0.5f;


    // calculate how much of the ray is in direct light by taking a fixed number of steps down the ray
    // and calculating the percent.
    // Note: in a rasterizer, you'd replace the RayVsScene raytracing with a shadow map lookup!
    float fogLitPercent = 0.0f;
    for (int i = 0; i < c_numRayMarchSteps; ++i)
    {
        float3 testPos = rayPos + rayDir * rayHitTime * ((float(i) + startRayOffset) / float(c_numRayMarchSteps));
        RayHitInfo shadowHitInfo = RayVsScene(testPos, c_lightDir);
        fogLitPercent = lerp(fogLitPercent, (shadowHitInfo.dist == c_rayMaxDist) ? 1.0f : 0.0f, 1.0f / float(i + 1));
    }

    float3 fogColor = lerp(c_fogColorUnlit, c_fogColorLit, fogLitPercent);
    float absorb = exp(-rayHitTime * 0.02f);
    // absorb = 0.1;
    return lerp(fogColor, pixelColor, absorb);
}


void raymarching_float(float2 uv, float3 camerapos, out float3 outValue)
{
    // 縦横のうち短いほうが-1～1になるような値を計算する
    float3 pos = float3(uv, -15);
    // float3 camerapos = float3(0.5, 0.5, -19);

    Ray ray;
    ray.origin = float3(camerapos);
    ray.direction = normalize(pos - ray.origin);

    float3 color = 0.1;
    // for (int i = 0; i < 10; i++)
    // {
    //     RayHitInfo hitinfo = sceneDist(ray.origin);
    //     float distance = hitinfo.dist;
    //
    //     // if (i == 0)
    //     // {
    //     //     color = distance;
    //     //     break;
    //     // }
    //
    //     if (distance < 0.0001)
    //     {
    //         // 距離が十分に短かったら衝突したと判定して色を計算する
    //         float3 normal = getNormal(ray.origin);
    //         color = dot(lightDir, normal);
    //         break;
    //     }
    //     // レイを進める
    //     ray.origin += ray.direction * distance;
    // }

    float rayHitTime;
    float3 pixelColor = GetColorForRay(camerapos, ray.direction, rayHitTime);

    pixelColor = ApplyFog2(camerapos, ray.direction, pixelColor, rayHitTime);
    outValue = pixelColor;
}
