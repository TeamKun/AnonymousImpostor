
using System;
using System.Collections;
using System.Collections.Generic;
using HarmonyLib;
using UnhollowerBaseLib;
using UnityEngine;
using Object = Il2CppSystem.Object;

namespace AnonymousImpostor
{
    public static class Extensions
    {
        public static Vector3 Inv(this Vector3 a)
        {
            return new Vector3(1f / a.x, 1f / a.y, 1f / a.z);
        }
        
        public static void BeginImpostor(IntroCutscene introCutscene)
        {
                introCutscene.ImpostorText.gameObject.SetActive(false);
                introCutscene.Title.Text =
                    DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Impostor,
                        (Il2CppReferenceArray<Object>) Array.Empty<object>());
                introCutscene.Title.Color = Palette.ImpostorRed;

                GameData.PlayerInfo data = PlayerControl.LocalPlayer.Data;
                if (data != null)
                {
                    int num = -1;
                    int num2 = 1 / 2;
                    float num3 = 1f - (float) num2 * 0.075f;
                    float num4 = 1f - (float) num2 * 0.035f;
                    float num5 = -8;
                    PoolablePlayer poolablePlayer =
                        UnityEngine.Object.Instantiate<PoolablePlayer>(introCutscene.PlayerPrefab, introCutscene.transform);
                    poolablePlayer.transform.localPosition = new Vector3((float) (num * num2) * num4,
                        introCutscene.BaseY + (float) num2 * 0.15f, num5 + (float) num2 * 0.01f) * 1.5f;
                    Vector3 vector = new Vector3(num3, num3, num3) * 1.5f;
                    poolablePlayer.transform.localScale = vector;
                    poolablePlayer.SetFlipX(false);
                    PlayerControl.SetPlayerMaterialColors((int) data.ColorId, poolablePlayer.Body);
                    //DestroyableSingleton<HatManager>.Instance.SetSkin(poolablePlayer.SkinSlot, data.SkinId);
                    poolablePlayer.HatSlot.SetHat(data.HatId, (int) data.ColorId);
                    PlayerControl.SetPetImage(data.PetId, (int) data.ColorId, poolablePlayer.PetSlot);
                    TextRenderer nameText = poolablePlayer.NameText;
                    nameText.Text = data.PlayerName;
                    nameText.transform.localScale = vector.Inv();
                }
        }
    }
}