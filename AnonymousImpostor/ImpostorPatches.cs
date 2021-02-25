using System;
using System.Collections;
using System.Collections.Generic;
using HarmonyLib;
using UnhollowerBaseLib;
using UnityEngine;
using Object = Il2CppSystem.Object;

namespace AnonymousImpostor
{
    public class ImpostorPatches
    {
        [HarmonyPatch(typeof(PlayerControl) , nameof(PlayerControl.FindClosestTarget))]
        public static class KillPerformPatch
        {
            static bool Prefix(PlayerControl __instance , ref PlayerControl __result)
            {
                PlayerControl result = null;
                PlayerControl pl = __instance;
                float num = GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)];
                if (!ShipStatus.Instance)
                {
                    AnonymousImpostor.log.LogMessage("PlayerControl.FindClosestTarget was called:Target is null.");
                    return true;
                }

                Vector2 truePosition = pl.GetTruePosition();
                Il2CppSystem.Collections.Generic.List<GameData.PlayerInfo> allPlayers = GameData.Instance.AllPlayers;
                for (int i = 0; i < allPlayers.Count; i++)
                {
                    GameData.PlayerInfo playerInfo = allPlayers[i];
                    if (!playerInfo.Disconnected && playerInfo.PlayerId != pl.PlayerId && !playerInfo.IsDead)
                    {
                        PlayerControl @object = playerInfo.Object;
                        if (@object)
                        {
                            Vector2 vector = @object.GetTruePosition() - truePosition;
                            float magnitude = vector.magnitude;
                            if (magnitude <= num && !PhysicsHelpers.AnyNonTriggersBetween(truePosition, vector.normalized, magnitude, Constants.ShipAndObjectsMask))
                            {
                                result = @object;
                                num = magnitude;
                            }
                        }
                    }
                }

                try
                {
                    AnonymousImpostor.log.LogMessage("PlayerControl.FindClosestTarget was called:Target is " + result.PlayerId + "'s player.");
                }
                catch
                {
                    AnonymousImpostor.log.LogMessage("PlayerControl.FindClosestTarget was called:Target is null.");
                }
                __result = result;
                return false;
            }
        }
        
        [HarmonyPatch(typeof(IntroCutscene.CoBegin__d), nameof(IntroCutscene.CoBegin__d.MoveNext))]
        public static class IntroPatch
        {
            private static void Prefix(IntroCutscene.CoBegin__d __instance)
            {
                if (__instance.isImpostor)
                {
                    __instance.yourTeam.Clear();
                    __instance.yourTeam.Add(PlayerControl.LocalPlayer);
                }
            }
        }

        [HarmonyPatch(typeof(HudManager) , nameof(HudManager.Update))]
        public static class NameUpdatePatch
        {
            static void Postfix(HudManager __instance)
            {
                if (PlayerControl.LocalPlayer.Data.IsImpostor)
                {
                    foreach (var impostor in PlayerControl.AllPlayerControls)
                    {
                        if (impostor.Data.IsImpostor)
                        {
                            impostor.nameText.Color = Color.white;
                        }
                    }
                }

                foreach (var player in PlayerControl.AllPlayerControls)
                {
                    if (MeetingHud.Instance != null)
                        foreach (var playerVoteArea in MeetingHud.Instance.playerStates)
                            if (playerVoteArea.NameText != null && player.PlayerId == playerVoteArea.TargetPlayerId)
                                playerVoteArea.NameText.Color = player.nameText.Color;
                }

            }
        }
    }
}