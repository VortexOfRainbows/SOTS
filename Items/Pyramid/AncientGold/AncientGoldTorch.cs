using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace SOTS.Items.Pyramid.AncientGold
{
	public class AncientGoldTorch : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 16;
			item.maxStack = 99;
			item.holdStyle = 1;
			item.noWet = true;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.consumable = true;
			item.createTile = TileType<AncientGoldTorchTile>();
			item.flame = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.rare = ItemRarityID.LightRed;
			item.value = 50;
		}
		public override void HoldItem(Player player)
		{
			if (Main.rand.Next(player.itemAnimation > 0 ? 40 : 80) == 0)
			{
				Dust.NewDust(new Vector2(player.itemLocation.X + 16f * player.direction, player.itemLocation.Y - 14f * player.gravDir), 4, 4, DustID.Fire);
			}
			Vector2 position = player.RotatedRelativePoint(new Vector2(player.itemLocation.X + 12f * player.direction + player.velocity.X, player.itemLocation.Y - 14f + player.velocity.Y), true);
			Lighting.AddLight(position, 1.1f, 0.9f, 0.9f);
		}
		public override void PostUpdate()
		{
			if (!item.wet)
			{
				Lighting.AddLight((int)((item.position.X + item.width / 2) / 16f), (int)((item.position.Y + item.height / 2) / 16f), 1.1f, 0.9f, 0.9f);
			}
		}
		public override void AutoLightSelect(ref bool dryTorch, ref bool wetTorch, ref bool glowstick)
		{
			dryTorch = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Torch, 3);
			recipe.AddIngredient(ItemType<RoyalGoldBrick>());
			recipe.SetResult(this, 3);
			recipe.AddRecipe();
		}
	}
	public class AncientGoldTorchTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileNoAttach[Type] = true;
			Main.tileNoFail[Type] = true;
			Main.tileWaterDeath[Type] = true;
			TileID.Sets.FramesOnKillWall[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleTorch);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
			TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
			TileObjectData.newAlternate.AnchorAlternateTiles = new[] { 124 };
			TileObjectData.addAlternate(1);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
			TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
			TileObjectData.newAlternate.AnchorAlternateTiles = new[] { 124 };
			TileObjectData.addAlternate(2);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
			TileObjectData.newAlternate.AnchorWall = true;
			TileObjectData.addAlternate(0);
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Ancient Gold Torch");
			AddMapEntry(new Color(255, 220, 100), name);
			disableSmartCursor = true;
			dustType = DustID.GoldCoin;
			drop = ItemType<AncientGoldTorch>();
			disableSmartCursor = true;
			adjTiles = new int[] { TileID.Torches };
			torch = true;
		}
        public override bool CanPlace(int i, int j)
        {
			return Main.tile[i, j].liquid == 0;
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = Main.rand.Next(1, 3);
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Tile tile = Main.tile[i, j];
			if (tile.frameX < 66)
			{
				r = 1.1f;
				g = 0.9f;
				b = 0.9f;
			}
		}
		/*public override void HitWire(int i, int j)
		{
			if (Main.tile[i, j].frameX >= 66)
			{
				Main.tile[i, j].frameX -= 66;
			}
			else
			{
				Main.tile[i, j].frameX += 66;
			}
		}*/
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
		{
			offsetY = 0;
			if (WorldGen.SolidTile(i, j - 1))
			{
				offsetY = 2;
				if (WorldGen.SolidTile(i - 1, j + 1) || WorldGen.SolidTile(i + 1, j + 1))
				{
					offsetY = 4;
				}
			}
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)(uint)i);
			Color color = new Color(110, 90, 90, 0);
			int frameX = Main.tile[i, j].frameX;
			int frameY = Main.tile[i, j].frameY;
			int width = 20;
			int offsetY = 2;
			int height = 20;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			for (int k = 0; k < 7; k++)
			{
				float x = (float)Utils.RandomInt(ref randSeed, -10, 11) * 0.15f;
				float y = (float)Utils.RandomInt(ref randSeed, -10, 1) * 0.35f;
				Main.spriteBatch.Draw(mod.GetTexture("Items/Pyramid/AncientGold/AncientGoldTorchTile_Flame"), new Vector2((float)(i * 16 - (int)Main.screenPosition.X) - (width - 16f) / 2f + x, (float)(j * 16 - (int)Main.screenPosition.Y + offsetY) + y) + zero, new Rectangle(frameX, frameY, width, height), color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
			}
		}
	}
}