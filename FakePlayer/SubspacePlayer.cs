using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Celestial;
using SOTS.Projectiles.Celestial;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SOTS.FakePlayer
{
    public class SubspacePlayer : ModPlayer
    {
        public override void SetStaticDefaults()
        {
        }
        public bool foundItem = false;
        public bool servantActive = false;
        public bool servantIsVanity = false;
        public static SubspacePlayer ModPlayer(Player player)
        {
            return player.GetModPlayer<SubspacePlayer>();
        }
        public override void PostUpdate()
        {
            if(servantActive)
            {
                Summon();
            }
        }
        public int subspaceServantShader = 0;
        public override void ResetEffects()
        {
            subspaceServantShader = 0;
            servantIsVanity = false;
            servantActive = false;
            foundItem = false;
            if (Main.gameMenu || Main.gameInactive) //hopefully prevent projectile entity issues
            {
                return;
            }
            for (int i = 9 + Player.extraAccessorySlots; i < Player.armor.Length; i++) //checking vanity slots
            {
                Item item = Player.armor[i];
                if (item.type == ModContent.ItemType<SubspaceLocket>())
                {
                    servantActive = true;
                    servantIsVanity = true;
                }
                //if (Item.type == ModContent.ItemType<SubspaceLocket>())
                //{
                //    SubspacePlayer.ModPlayer(player).subspaceServantShader = GameShaders.Armor.GetShaderIdFromItemId(player.dye[i].type);
                //}
            }
            //for (int i = 0; i < 10; i++) //iterating through armor + accessories
            //{
            //    Item item = player.armor[i];
            //    if (Item.type == ModContent.ItemType<SubspaceLocket>())
            //    {
            //        SubspacePlayer.ModPlayer(player).subspaceServantShader = GameShaders.Armor.GetShaderIdFromItemId(player.dye[i].type);
            //    }
            //}
        }
        public int Probe = -1;
        public void Summon()
        {
            int type = ModContent.ProjectileType<SubspaceServant>();
            SOTSPlayer.ModPlayer(Player).runPets(ref Probe, type, 0, 0, false);
        }
        public static List<FakePlayer> GetServantFakePlayers(Player player)
        {
            List<FakePlayer> ret = new List<FakePlayer>();
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.ModProjectile is SubspaceServant subServ && proj.owner == player.whoAmI && proj.active)
                {
                    ret.Add(subServ.FakePlayer);
                }
            }
            return ret;
        }
    }
}