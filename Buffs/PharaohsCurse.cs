using SOTS.Achievements;
using SOTS.Items.Pyramid;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class PharaohsCurse : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
			Main.debuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
		{
			bool update = true;
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (NPC.downedBoss2 || modPlayer.weakerCurse)
			{
				update = false;
                if(player.whoAmI == Main.myPlayer)
                    ModContent.GetInstance<IntoThePyramid>().EnterPyramidCondition.Complete();
            }
            int tileBehindX = (int)(player.Center.X / 16);
			int tileBehindY = (int)(player.Center.Y / 16);
			Tile tile = Framing.GetTileSafely(tileBehindX, tileBehindY);
            if (update)
            {
                if (SOTSWall.unsafePyramidWall.Contains(tile.WallType) || tile.WallType == (ushort)ModContent.WallType<TrueSandstoneWallWall>())
                {
                    player.lifeRegen -= 100;
                }
            }
            modPlayer.weakerCurse = false;
		}

    }
}