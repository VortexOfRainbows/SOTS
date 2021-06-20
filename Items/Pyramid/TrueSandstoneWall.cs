using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	public class TrueSandstoneWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Sandstone Wall");
			Tooltip.SetDefault("'Only the gods could've forged a wall with such power'");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 7;
			item.useStyle = 1;
			item.rare = 5;
			item.consumable = true;
			item.createWall = mod.WallType("TrueSandstoneWallWall");
			item.expert = true;
		}
	}
	public class TrueSandstoneWallWall : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = false;
			dustType = 32;
			drop = mod.ItemType("TrueSandstoneWall");
			AddMapEntry(new Color(155, 110, 55));
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override void KillWall(int i, int j, ref bool fail)
		{
			fail = true;
		}
	}
}