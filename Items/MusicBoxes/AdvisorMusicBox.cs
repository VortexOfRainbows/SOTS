using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using SOTS.Items.Trophies;

namespace SOTS.Items.MusicBoxes
{
	public class AdvisorMusicBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Music Box (Advisor)");
		}
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.SwingThrow;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = mod.TileType("AdvisorMusicBoxTile");
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 100000;
			Item.accessory = true;
		}
		/*public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "AdvisorTrophy", 1);
			recipe.AddIngredient(ItemID.MusicBox);
			recipe.AddTile(TileID.HeavyWorkBench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}*/
	}
	public class AdvisorMusicBoxTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileObsidianKill[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(Type);
			disableSmartCursor = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Music Box");
			AddMapEntry(new Color(191, 142, 111), name);
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 16, 48, mod.ItemType("AdvisorMusicBox"));
		}
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.showItemIcon = true;
			player.showItemIcon2 = mod.ItemType("AdvisorMusicBox");
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)((ulong)i));
			Color color = new Color(80, 80, 80, 0);
			int frameX = Main.tile[i, j].frameX / 18;
			int frameY = Main.tile[i, j].frameY / 18;
			if (frameX >= 2)
			{
				Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
				if (Main.drawToScreen)
				{
					zero = Vector2.Zero;
				}
				for (int k = 0; k < 5; k++)
				{
					float x = (float)Utils.RandomInt(ref randSeed, -10, 11) * 0.1f;
					float y = (float)Utils.RandomInt(ref randSeed, -10, 11) * 0.1f;
					if (k <= 1)
					{
						x = 0;
						y = 0;
					}
					Main.spriteBatch.Draw(mod.GetTexture("Items/MusicBoxes/AdvisorMusicBoxGlow"), new Vector2(i * 16 - Main.screenPosition.X + x, j * 16 - Main.screenPosition.Y + y + 2) + zero, new Rectangle(frameX * 18, frameY * 18, 16, 16), color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
				}
			}
		}
	}
}