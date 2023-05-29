using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Earth;
using SOTS.NPCs.Boss;
using SOTS.Projectiles.Blades;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Furniture.Functional
{
	public class Mineralarium : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.width = 48;
			Item.height = 42;
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<BigCrystalTile>();
		}
	}	
	/*public class BigCrystalTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileObsidianKill[Type] = false;
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
			Main.tileWaterDeath[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style5x4);
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.Width = 13;
			TileObjectData.newTile.Height = 14;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16 };
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 10, 2);
			TileObjectData.newTile.Origin = new Point16(7, 13);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			AddMapEntry(new Color(237, 255, 193), name);
			MinPick = 250;
			DustType = ModContent.DustType<VibrantDust>();
			HitSound = SoundID.Item27;
			MineResist = 0.1f;
		}
        public override bool CanExplode(int i, int j)
        {
            return true;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			return true;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 2;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int width = 6 * 16;
			int height = 7 * 16;
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16 + width, j * 16 + height, 16, 16, ModContent.ItemType<BigCrystal>());
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.27f;
			g = 0.33f;
			b = 0.15f;
		}
		public bool isHoverInCorrectLocation(int i, int j)
        {
			Tile tile = Main.tile[i, j];
			int tileFrameX = tile.TileFrameX / 18;
			int tileFrameY = tile.TileFrameY / 18;
			if(tileFrameX >= 5 && tileFrameX <= 7 && tileFrameY <= 10)
            {
				return true;
            }
			return false;
        }
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			if(isHoverInCorrectLocation(i, j))
			{
				player.cursorItemIconID = ItemID.BreakerBlade;
				//player.cursorItemIconText = "";
				player.noThrow = 2;
				player.cursorItemIconEnabled = true;
			}
			else
			{
				if (player.cursorItemIconText == "")
				{
					player.cursorItemIconEnabled = false;
					player.cursorItemIconID = 0;
				}
			}
		}
		public override void MouseOverFar(int i, int j)
		{
			MouseOver(i, j);
			Player player = Main.LocalPlayer;
			if (player.cursorItemIconText == "")
			{
				player.cursorItemIconEnabled = false;
				player.cursorItemIconID = 0;
			}
		}
        public override bool RightClick(int i, int j)
		{
			if (!isHoverInCorrectLocation(i, j))
				return false;
			Tile tile = Main.tile[i, j];
			Player player = Main.LocalPlayer;
			if(player.ConsumeItem(ItemID.BreakerBlade, true))
			{
				int Left = i - tile.TileFrameX / 18;
				int Top = j - tile.TileFrameY / 18;
				Projectile.NewProjectile(new EntitySource_TileInteraction(player, i, j, "SOTS:BigCrystalTrade"), new Vector2(Left + 6, Top + 6) * 16 + new Vector2(8, 8), Vector2.Zero, ModContent.ProjectileType<ColossusSpawnAnimation>(), 0, 0, Main.myPlayer, Top * 16 + 8, 0);
            }
			return base.RightClick(i, j);
        }
    }*/
}