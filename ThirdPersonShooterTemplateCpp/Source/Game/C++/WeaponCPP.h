#pragma once

#include "Engine/Scripting/Script.h"
#include "Engine/Content/JsonAsset.h"
#include "Engine/Scripting/ScriptingObjectReference.h"
#include "WeaponDataCPP.h"

API_CLASS() class GAME_API WeaponCPP : public Script
{
API_AUTO_SERIALIZATION();
DECLARE_SCRIPTING_TYPE(WeaponCPP);
public:
    API_FIELD(Attributes="EditorOrder(0), AssetReference(typeof(WeaponDataCPP))")
    AssetReference<JsonAsset> WeaponSettings;

    API_FIELD(Attributes = "EditorOrder(1)")
    ScriptingObjectReference<Actor> BulletSpawnPoint;

private:
    float _timeToFire;
    WeaponDataCPP* _data = nullptr;

    // [Script]
    void OnUpdate() override;
    void OnStart() override;
};
