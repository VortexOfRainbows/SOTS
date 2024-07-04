using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using SOTS.Common.ModPlayers;
using SOTS.Void;
using Terraria.Localization;

namespace SOTS.Buffs.ConduitBoosts
{
    public class Attuned : ModBuff
    {
        public int GetTier(Player player)
        {
            int total = 0;
            ConduitPlayer CP = player.ConduitPlayer();
            if(CP.NatureBoosted)
                total++;
            if (CP.EarthBoosted)
                total++;
            if (CP.PermafrostBoosted)
                total++;
            if (CP.OtherworldBoosted)
                total++;
            if (CP.TideBoosted)
                total++;
            if (CP.EvilBoosted)
                total++;
            if (CP.InfernoBoosted)
                total++;
            if (CP.ChaosBoosted)
                total++;
            return total;
        }
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = true;
            Main.persistentBuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            int tier = GetTier(player);
            if(tier <= 0)
            {
                player.DelBuff(buffIndex);
                return;
            }
            else
            {
                VoidPlayer vPlayer = player.VoidPlayer();
                int lifeMax = 0;
                int voidMax = 0;
                if(tier == 1)
                    lifeMax = 20;
                if(tier == 2)
                    lifeMax = voidMax = 20;
                if (tier == 3)
                    lifeMax = voidMax = 40;
                if (tier == 4)
                {
                    lifeMax = 60;
                    voidMax = 40;
                }
                if (tier == 5)
                {
                    lifeMax = voidMax = 60;
                }
                if (tier == 6)
                    lifeMax = voidMax = 80;
                if (tier == 7)
                {
                    lifeMax = 100;
                    voidMax = 80;
                }
                if (tier == 8)
                    lifeMax = voidMax = 100;
                player.statLifeMax2 += lifeMax;
                vPlayer.voidMeterMax2 += voidMax;
                player.buffTime[buffIndex] = 3600;
            }
        }
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            int tier = GetTier(Main.LocalPlayer);
            if(tier != 0)
            {
                buffName += " (" + tier + ")";
                tip = Language.GetTextValue("Mods.SOTS.Buffs.Attuned.Tier" + tier);
            }
            rare = tier;
        }
        public override bool RightClick(int buffIndex)
        {
            Player player = Main.LocalPlayer;
            ConduitPlayer CP = player.ConduitPlayer();
            CP.NaturePower = CP.EarthPower = CP.PermafrostPower = CP.OtherworldPower = CP.TidePower = CP.EvilPower = CP.InfernoPower = CP.ChaosPower = 0;
            return true;
        }
        public override void PostDraw(SpriteBatch spriteBatch, int buffIndex, BuffDrawParams drawParams)
        {
            ConduitPlayer CP = Main.LocalPlayer.ConduitPlayer();
            Texture2D texture = ModContent.Request<Texture2D>("SOTS/Buffs/ConduitBoosts/AttunedTiers").Value;
            if (CP.NatureBoosted)
                DrawCell(spriteBatch, texture, drawParams, ColorHelpers.NatureColor, 0);
            if (CP.EarthBoosted)
                DrawCell(spriteBatch, texture, drawParams, ColorHelpers.EarthColor, 1);
            if (CP.PermafrostBoosted)
                DrawCell(spriteBatch, texture, drawParams, ColorHelpers.PermafrostColor, 2);
            if (CP.OtherworldBoosted)
                DrawCell(spriteBatch, texture, drawParams, ColorHelpers.PurpleOtherworldColor, 3);
            if (CP.TideBoosted)
                DrawCell(spriteBatch, texture, drawParams, ColorHelpers.TideColor, 4);
            if (CP.EvilBoosted)
                DrawCell(spriteBatch, texture, drawParams, ColorHelpers.RedEvilColor, 5);
            if (CP.InfernoBoosted)
                DrawCell(spriteBatch, texture, drawParams, ColorHelpers.Inferno1, 6);
            if (CP.ChaosBoosted)
                DrawCell(spriteBatch, texture, drawParams, ColorHelpers.ChaosPink, 7);
        }
        private void DrawCell(SpriteBatch spriteBatch, Texture2D texture, BuffDrawParams drawParams, Color color, int frame)
        {
            Rectangle frameRect = new Rectangle(0, frame * texture.Height / 8, texture.Width, texture.Height / 8);
            color = color.MultiplyRGBA(drawParams.DrawColor);
            color.A = 50;
            for (int i = 0; i < 6; i++)
            {
                Vector2 circular = new Vector2(1.5f, 0).RotatedBy(i / 3f * MathHelper.Pi + MathHelper.ToRadians(SOTSWorld.GlobalCounter));
                spriteBatch.Draw(texture, drawParams.Position + circular, frameRect, color * 0.65f, 0f, Vector2.Zero, 1f, 0, 0);
            }
            spriteBatch.Draw(texture, drawParams.Position, frameRect, drawParams.DrawColor, 0f, Vector2.Zero, 1f, 0, 0);
            spriteBatch.Draw(texture, drawParams.Position, frameRect, color * 1.0f, 0f, Vector2.Zero, 1f, 0, 0);
        }
    }
}