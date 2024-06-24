using System;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace SOTS.Buffs.ConduitBoosts
{
    public class BrillianceBoosted : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.buffTime[buffIndex] > 90)
                player.buffTime[buffIndex] = 90;
            if (player.buffTime[buffIndex] == 31 && Main.myPlayer == player.whoAmI)
            {
                SOTSUtils.PlaySound(SoundID.Item30, player.Center, 0.75f, 0.25f);
                for(int i = 0; i < 24; i++)
                {
                    Dust dust = Dust.NewDustDirect(player.Center, 0, 0, ModContent.DustType<Dusts.AlphaDrainDust>(), 0, 0, 0, ColorHelpers.ChaosPink, 1.1f);
                    dust.scale *= 1.4f;
                    dust.velocity *= 0.6f;
                    dust.velocity += new Vector2(0, 9f).RotatedBy(MathHelper.TwoPi * i / 24f) / dust.scale;
                    dust.fadeIn = 0.1f;
                    dust.noGravity = true;
                }
            }
            else if (player.buffTime[buffIndex] < 30)
                player.buffTime[buffIndex] = 30;
            int buffAmt = (int)(20 * (1 - (player.buffTime[buffIndex] - 30) / 60f));
            if (Main.myPlayer != player.whoAmI) 
                buffAmt = 20;
            player.statLifeMax2 += Math.Clamp(buffAmt, 0, 20);
        }
    }
}