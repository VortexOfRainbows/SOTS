using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SOTS.Buffs.DilationSickness
{
    public class DilationSickness : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams)
        {
            Player player = Main.LocalPlayer; //player is always local for buff drawing
            int ID = SOTSPlayer.ModPlayer(player).UniqueVisionNumber % 8;
            if (!Main.dedServ)
            {
                if (player.HasBuff(ModContent.BuffType<DilationSickness>()))
                {
                    drawParams.Texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Buffs/DilationSickness/DilationSickness" + ID);
                }
            }
            return true;
        }
    }
}