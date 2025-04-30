using FrooxEngine;
using HarmonyLib;
using ResoniteModLoader;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamRecentUsers
{
    public class SteamRecentUsers : ResoniteMod
    {
        public override string Name => "SteamRecentUsers";
        public override string Author => "Dante";
        public override string Version => "1.0.0";
        public override string Link => "https://github.com/YourNameHere/SteamRecentUsers"; 

        public override void OnEngineInit()
        {
            Harmony harmony = new Harmony("es.dant.SteamRecentUsers");
            harmony.PatchAll();
        }

        public static async void OnUserJoined(User user)
        {
            if (user.IsLocalUser)
                return;

            await AddUser(user);

        }

        public static async Task AddUser(User user)
        {
            while (!user.IsPresent)
            {
                await Task.Delay(1000);
            }

            if (user.Metadata.TryGetElement("SteamID", out var steamIdData))
            {
                if (steamIdData.TryGetValue<ulong>(out var steamId))
                {
                    SteamFriends.SetPlayedWith(new CSteamID(steamId));
                }
            }
        }

        [HarmonyPatch(typeof(World), "StartRunning")]
        class World_StartRunning_Patch
        {
            static void Postfix(World __instance)
            {
                __instance.UserJoined += OnUserJoined;
            }
        }
    }
}
