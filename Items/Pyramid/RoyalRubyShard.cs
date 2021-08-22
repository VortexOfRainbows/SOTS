using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Pyramid
{
	public class RoyalRubyShard : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Keystone Shard");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 26;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.value = Item.sellPrice(0, 0, 22, 50);
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = ItemRarityID.LightRed;
			item.consumable = true;
			item.createTile = mod.TileType("RoyalRubyShardTile");
		}
		public override void PostUpdate()
		{
			Lighting.AddLight(item.Center, 60 / 255f, 7 / 255f, 20 / 255f);
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frameNotUsed, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = mod.GetTexture("Items/Pyramid/TaintedKeystoneShard");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			position += drawOrigin * scale;
			float counter = Main.GlobalTime * 160;
			float mult = new Vector2(-1f, 0).RotatedBy(MathHelper.ToRadians(counter)).X;
			for (int i = 0; i < 6; i++)
			{
				Color color = new Color(255, 0, 0, 0);
				switch (i)
				{
					case 0:
						color = new Color(255, 0, 0, 0);
						break;
					case 1:
						color = new Color(255, 50, 0, 0);
						break;
					case 2:
						color = new Color(255, 100, 0, 0);
						break;
					case 3:
						color = new Color(255, 150, 0, 0);
						break;
					case 4:
						color = new Color(255, 200, 0, 0);
						break;
					case 5:
						color = new Color(255, 250, 0, 0);
						break;
				}
				Vector2 rotationAround = new Vector2((3 + mult) * scale, 0).RotatedBy(MathHelper.ToRadians(60 * i + counter));
				Main.spriteBatch.Draw(texture, new Vector2(position.X, position.Y) + rotationAround, null, color, 0f, drawOrigin, scale * 1.1f, SpriteEffects.None, 0f);
			}
			texture = mod.GetTexture("Items/Pyramid/RoyalRubyShard");
			Main.spriteBatch.Draw(texture, new Vector2(position.X, position.Y), null, drawColor, 0f, drawOrigin, scale * 1.0f, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture2 = mod.GetTexture("Items/Pyramid/TaintedKeystoneShard");
			Vector2 drawOrigin = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
			float counter = Main.GlobalTime * 160;
			float mult = new Vector2(-2.5f, 0).RotatedBy(MathHelper.ToRadians(counter)).X;
			for (int i = 0; i < 6; i++)
			{
				Color color = new Color(255, 0, 0, 0);
				switch (i)
				{
					case 0:
						color = new Color(255, 0, 0, 0);
						break;
					case 1:
						color = new Color(255, 40, 0, 0);
						break;
					case 2:
						color = new Color(255, 80, 0, 0);
						break;
					case 3:
						color = new Color(255, 120, 0, 0);
						break;
					case 4:
						color = new Color(255, 160, 0, 0);
						break;
					case 5:
						color = new Color(255, 200, 0, 0);
						break;
				}
				Vector2 rotationAround2 = 0.5f * new Vector2((6 + mult) * scale, 0).RotatedBy(MathHelper.ToRadians(60 * i + counter));
				Main.spriteBatch.Draw(texture2, rotationAround2 + item.Center - Main.screenPosition + new Vector2(0, 2), null, color, rotation, drawOrigin, scale * 1.1f, SpriteEffects.None, 0f);
			}
			texture2 = mod.GetTexture("Items/Pyramid/RoyalRubyShard");
			Main.spriteBatch.Draw(texture2, item.Center - Main.screenPosition + new Vector2(0, 2), null, lightColor, rotation, drawOrigin, scale * 1.1f, SpriteEffects.None, 0f);
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(ItemID.Ruby, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 3);
			recipe.AddIngredient(ModContent.ItemType<Fragments.FragmentOfNature>(), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(ItemID.Emerald, 3);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 3);
			recipe.AddIngredient(ModContent.ItemType<Fragments.FragmentOfEarth>(), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(ItemID.Topaz, 3);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 3);
			recipe.AddIngredient(ModContent.ItemType<Fragments.FragmentOfPermafrost>(), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(ItemID.Diamond, 3);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 3);
			recipe.AddIngredient(ModContent.ItemType<Fragments.FragmentOfOtherworld>(), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(ItemID.Amethyst, 3);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 3);
			recipe.AddIngredient(ModContent.ItemType<Fragments.FragmentOfTide>(), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(ItemID.Sapphire, 3);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 3);
			recipe.AddIngredient(ModContent.ItemType<Fragments.FragmentOfInferno>(), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(ItemID.Amber, 3);
			recipe.AddRecipe();
		}
	}
	public class RoyalRubyShardTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileNoFail[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileObsidianKill[Type] = true;
			drop = mod.ItemType("RoyalRubyShard");
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Keystone Shard");
			AddMapEntry(new Color(211, 69, 74), name);
			soundType = 2;
			soundStyle = 27;
			dustType = 12;
		}
        public override bool CanExplode(int i, int j)
		{
			return true;
		}
        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
			bool canLive = ModifyFrames(i, j);
			if(!canLive)
            {
				WorldGen.KillTile(i, j);
				if (Main.netMode != NetmodeID.SinglePlayer)
				{
					NetMessage.SendData(17, -1, -1, null, 0, i, j, 0f, 0, 0, 0);
				}
			}
            return false;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.5f;
			g = 0.0375f;
			b = 0.125f;
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Texture2D texture = Main.tileTexture[tile.type];
			Vector2 drawOffSet = Vector2.Zero;
			if(tile.frameY == 0) //below is active
				drawOffSet.Y += 2;
			if (tile.frameY == 18) //above is active
				drawOffSet.Y -= 2;
			if (tile.frameY == 36) //right is active
				drawOffSet.X += 2;
			if (tile.frameY == 54) //left is active
				drawOffSet.X -= 2;
			Vector2 location = new Vector2(i * 16, j * 16) + drawOffSet;
			Color color2 = Lighting.GetColor(i, j, WorldGen.paintColor(tile.color()));
			Texture2D texture2 = mod.GetTexture("Items/Pyramid/TaintedKeystoneShardTile");
			float counter = Main.GlobalTime * 160;
			float mult = new Vector2(-1f, 0).RotatedBy(MathHelper.ToRadians(counter)).X;
			Rectangle frame = new Rectangle(tile.frameX, tile.frameY, 16, 16);
			for (int k = 0; k < 6; k++)
			{
				Color color = new Color(255, 0, 0, 0);
				switch (k)
				{
					case 0:
						color = new Color(255, 0, 0, 0);
						break;
					case 1:
						color = new Color(255, 40, 0, 0);
						break;
					case 2:
						color = new Color(255, 80, 0, 0);
						break;
					case 3:
						color = new Color(255, 120, 0, 0);
						break;
					case 4:
						color = new Color(255, 160, 0, 0);
						break;
					case 5:
						color = new Color(255, 200, 0, 0);
						break;
				}
				Vector2 rotationAround2 = new Vector2(2 + mult, 0).RotatedBy(MathHelper.ToRadians(60 * k + counter));
				Main.spriteBatch.Draw(texture2, location + zero - Main.screenPosition + rotationAround2, frame, color, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(texture, location + zero - Main.screenPosition, frame, color2, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			return false;
		}
		public override bool CanPlace(int i, int j)
		{
			return TileIsCapable(i, j + 1) || TileIsCapable(i, j - 1) || TileIsCapable(i + 1, j) || TileIsCapable(i - 1, j);
		}
		private bool TileIsCapable(Tile tile)
		{
			return tile.active() && Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type] && tile.slope() == 0 && !tile.halfBrick() && !tile.inActive();
		}
		private bool TileIsCapable(int i, int j)
        {
			return TileIsCapable(Main.tile[i, j]);
        }
		public bool ModifyFrames(int i, int j, bool randomize = false)
		{
			bool flag = true;
			if (TileIsCapable(i, j + 1)) //checks if below tile is active
			{
				Main.tile[i, j].frameY = 0;
			}
			else if (TileIsCapable(i - 1, j)) //checks if left tile is active
			{
				Main.tile[i, j].frameY = 54;
			}
			else if (TileIsCapable(i + 1, j)) //checks if right tile is active
			{
				Main.tile[i, j].frameY = 36;
			}
			else if (TileIsCapable(i, j - 1)) //checks if above tile is active
			{
				Main.tile[i, j].frameY = 18;
			}
			else
            {
				flag = false;
			}
			if(flag && randomize)
			{
				Main.tile[i, j].frameX = (short)(WorldGen.genRand.Next(18) * 18);
				WorldGen.SquareTileFrame(i, j, true);
				NetMessage.SendTileSquare(-1, i, j, 2, TileChangeType.None);
				//NetMessage.SendData(17, -1, -1, null, 1, i, j, Type);
			}
			return flag;
		}
		public override void PlaceInWorld(int i, int j, Item item)
		{
			ModifyFrames(i, j, true);
		}
	}
}