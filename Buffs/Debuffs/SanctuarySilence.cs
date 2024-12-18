using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs.Debuffs
{
    public class SanctuarySilence : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
			Main.debuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
		{

        }
        //public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams)
        //{
        //    Player player = Main.LocalPlayer; //player is always local for buff drawing
        //    int ID = SOTSPlayer.ModPlayer(player).UniqueVisionNumber % 8;
        //    if (!Main.dedServ)
        //    {
        //        if (player.HasBuff(ModContent.BuffType<DilationSickness>()))
        //        {
        //            drawParams.Texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Buffs/DilationSickness/DilationSickness" + ID);
        //        }
        //    }
        //    return true;
        //}
    }
}