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
			Item.width = 28;
			Item.height = 28;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 7;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = 5;
			Item.consumable = true;
			Item.createWall = mod.WallType("TrueSandstoneWallWall");
			Item.expert = true;
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