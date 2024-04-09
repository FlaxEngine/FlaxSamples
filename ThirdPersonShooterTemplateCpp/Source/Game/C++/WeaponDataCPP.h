#pragma once

#include "Engine/Core/ISerializable.h"
#include "Engine/Core/Types/BaseTypes.h"
#include "Engine/Content/Assets/Model.h"
#include "Engine/Scripting/ScriptingType.h"

/// <summary>
/// WeaponDataCPP Json Asset. 
/// </summary>
API_CLASS() class GAME_API WeaponDataCPP : public ISerializable
{
    API_AUTO_SERIALIZATION();
    DECLARE_SCRIPTING_TYPE_NO_SPAWN(WeaponDataCPP);
public:
    API_FIELD(Attributes = "Range(0, 20), EditorOrder(0), EditorDisplay(\"Weapon Data\")")
    float FireRate = 20;

    API_FIELD(Attributes = "Range(1000, 10000), EditorOrder(1), EditorDisplay(\"Weapon Data\")")
    float BulletVelocity = 1000;

    API_FIELD(Attributes = "Limit(0, 20), EditorOrder(2), EditorDisplay(\"Weapon Data\")")
    int BulletLifetime = 10;

    API_FIELD(Attributes="EditorOrder(3), EditorDisplay(\"Weapon Data\")")
    AssetReference<Model> BulletModel;

    API_FIELD(Attributes = "EditorOrder(3), EditorDisplay(\"Weapon Data\")")
    Vector3 BulletScale = Vector3(0.1f);
};
