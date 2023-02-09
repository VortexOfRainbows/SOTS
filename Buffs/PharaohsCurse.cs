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
            DisplayName.SetDefault("Pharaoh's Curse");
			Description.SetDefault("Something is watching you, spawn rates increased");   
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
			}
			int tileBehindX = (int)(player.Center.X / 16);
			int tileBehindY = (int)(player.Center.Y / 16);
			Tile tile = Framing.GetTileSafely(tileBehindX, tileBehindY);
			if (SOTSWall.unsafePyramidWall.Contains(tile.WallType) || tile.WallType == (ushort)ModContent.WallType<TrueSandstoneWallWall>())
			{
				if (update)
				{
					player.lifeRegen -= 100;
				}
			}
			modPlayer.weakerCurse = false;
		}

    }
}