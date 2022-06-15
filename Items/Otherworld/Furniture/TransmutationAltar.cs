using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Fragments;
using SOTS.Items.Otherworld.FromChests;
using SOTS.Items.Pyramid;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;


namespace SOTS.Items.Otherworld.Furniture
{
	public class TransmutationAltar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Transmutation Altar");
			Tooltip.SetDefault("Used for crafting\nAllows the conversion of material and equipment to equivalent but different forms\nAlso counts as a demon altar for crafting\nCrafting using the altar can be undone");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.width = 36;
			Item.height = 40;
			Item.rare = ItemRarityID.Cyan;
			Item.value = Item.sellPrice(0, 20, 0, 0);
			Item.createTile = TileType<TransmutationAltarTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemType<PrecariousCluster>(), 1).AddIngredient(ItemType<TwilightShard>(), 20).AddIngredient(ItemType<OtherworldlyAlloy>(), 20).AddIngredient(ItemType<HardlightAlloy>(), 10).AddIngredient(ItemType<StarlightAlloy>(), 10).AddTile(TileType<HardlightFabricatorTile>()).Register();
		}
		public static void AddTransmutationRecipes(Mod mod) //called in SOTS
		{
			AddDirectExchangeRecipe(mod, ItemID.CopperBar, ItemID.TinBar);
			AddDirectExchangeRecipe(mod, ItemID.IronBar, ItemID.LeadBar);
			AddDirectExchangeRecipe(mod, ItemID.SilverBar, ItemID.TungstenBar);
			AddDirectExchangeRecipe(mod, ItemID.GoldBar, ItemID.PlatinumBar);

			Recipe recipe = mod.CreateRecipe(ItemID.IronOre, 1);
			recipe.AddIngredient(ItemID.CopperOre, 3);
			recipe.AddTile(TileType<TransmutationAltarTile>());
			recipe.Register();

			recipe = mod.CreateRecipe(ItemID.SilverOre, 1);
			recipe.AddIngredient(ItemID.IronOre, 3);
			recipe.AddTile(TileType<TransmutationAltarTile>());
			recipe.Register();

			recipe = mod.CreateRecipe(ItemID.GoldOre, 1);
			recipe.AddIngredient(ItemID.SilverOre, 3);
			recipe.AddTile(TileType<TransmutationAltarTile>());
			recipe.Register();

			recipe = mod.CreateRecipe(ItemID.LeadOre, 1);
			recipe.AddIngredient(ItemID.TinOre, 3);
			recipe.AddTile(TileType<TransmutationAltarTile>());
			recipe.Register();

			recipe = mod.CreateRecipe(ItemID.TungstenOre, 1);
			recipe.AddIngredient(ItemID.LeadOre, 3);
			recipe.AddTile(TileType<TransmutationAltarTile>());
			recipe.Register();

			recipe = mod.CreateRecipe(ItemID.PlatinumOre, 1);
			recipe.AddIngredient(ItemID.TungstenOre, 3);
			recipe.AddTile(TileType<TransmutationAltarTile>());
			recipe.Register();

			AddDirectExchangeRecipe(mod, ItemID.DemoniteBar, ItemID.CrimtaneBar);
			AddDirectExchangeRecipe(mod, ItemID.CobaltBar, ItemID.PalladiumBar);
			AddDirectExchangeRecipe(mod, ItemID.MythrilBar, ItemID.OrichalcumBar);
			AddDirectExchangeRecipe(mod, ItemID.AdamantiteBar, ItemID.TitaniumBar);
			AddDirectExchangeRecipe(mod, ItemID.BandofStarpower, ItemID.PanicNecklace);
		}
		public static void AddDirectExchangeRecipe(Mod mod, int item1, int item2)
		{
			Recipe recipe = mod.CreateRecipe(item2, 1);
			recipe.AddIngredient(item1, 1);
			recipe.AddTile(TileType<TransmutationAltarTile>());
			recipe.Register();
			recipe = mod.CreateRecipe(item1, 1);
			recipe.AddIngredient(item2, 1);
			recipe.AddTile(TileType<TransmutationAltarTile>());
			recipe.Register();
		}
	}
	public class TransmutationAltarTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(GetInstance<TransmutationAltarStorage>().Hook_AfterPlacement, -1, 0, true);
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Transmutation Altar");
			AddMapEntry(new Color(125, 55, 55), name);
			TileID.Sets.DisableSmartCursor[Type] = true;
			DustType = DustType<AvaritianDust>();
			AdjTiles = new int[] { TileID.DemonAltar };
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int drop = ItemType<TransmutationAltar>();
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 32, drop);
			ModContent.GetInstance<TransmutationAltarStorage>().Kill(i, j);
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if (Main.tile[i, j].TileFrameX < 18 || Main.tile[i, j].TileFrameX > 35 || Main.tile[i, j].TileFrameY % 36 < 18)
				return;

			r = 1.0f;
			g = 0.1f;
			b = 0.1f;
		}
		public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
		{
			int type = 0;
			if (Main.tile[i, j].TileFrameX >= 18 && Main.tile[i, j].TileFrameX < 36 && Main.tile[i, j].TileFrameY % 36 >= 18)
				type = 1;
			Tile t = Main.tile[i, j];
			if (type == 1) // t.frameX % 54 == 0
			{
				Main.instance.TilesRenderer.AddSpecialLegacyPoint(new Point(i, j));
			}
		}
		public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
        {
            draw(i, j, spriteBatch);
		}
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.cursorItemIconID = ModContent.ItemType<UndoArrow>();
			//player.cursorItemIconText = "";
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
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
		float cooldown = 0f;
		public override bool RightClick(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int left = i - tile.TileFrameX / 18;
			int top = j - tile.TileFrameY / 18;
			int index = GetInstance<TransmutationAltarStorage>().Find(left, top);
			TransmutationAltarStorage entity = (TransmutationAltarStorage)TileEntity.ByID[index];

			Vector2 dynamicAddition = new Vector2(3, 0).RotatedBy(MathHelper.ToRadians(Main.GlobalTimeWrappedHourly * 40));
			int amountOfUniqueItems = 1;
			int totalItems = 0;
			for (int l = 1; l < entity.itemsArray.Length; l++)
			{
				if (entity.itemsArray[l] != 0)
				{
					amountOfUniqueItems++;
					totalItems += entity.itemAmountsArray[l];
				}
			}
			Vector2 pos = new Vector2((float)(left * 16 + 24), (float)(top * 16 + 24));
			pos.Y -= 80 + dynamicAddition.Y + (totalItems + entity.itemAmountsArray[0]) * 0.5f;
			Main.mouseRightRelease = false;
			Player player = Main.LocalPlayer;
			int amountNeeded = entity.itemAmountsArray[0];
			int middleItemType = entity.itemsArray[0];
			int amountHas = 0;
			for (int k = 0; k < 50; k++)
			{
				Item item = player.inventory[k];
				if (item.type == middleItemType)
				{
					amountHas += item.stack;
				}
			}
			if (amountHas >= amountNeeded && entity.itemsArray[0] != 0)
			{
				for (int l = 0; l < amountNeeded; l++)
					player.ConsumeItem(entity.itemsArray[0]);

				for (int l = 1; l < entity.itemsArray.Length; l++)
				{
					if (entity.itemsArray[l] != 0)
					{
						if (Main.myPlayer == player.whoAmI && (middleItemType != ItemType<RoyalRubyShard>() || (entity.itemsArray[l] != ModContent.ItemType<PrecariousCluster>())))
						{
							int item = Item.NewItem(new EntitySource_TileInteraction(player, i, j), (int)pos.X, (int)pos.Y, 0, 0, entity.itemsArray[l], entity.itemAmountsArray[l]);
							Main.item[item].velocity += new Vector2(-3, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
							NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f, 0.0f, 0.0f, 0, 0, 0);
						}
					}
				}
				if (Main.myPlayer == player.whoAmI)
				{
					Projectile.NewProjectile(new EntitySource_TileInteraction(player, i, j), pos, Vector2.Zero, ModContent.ProjectileType<UndoParticles>(), 0, 0, Main.myPlayer, i, j);
				}
			}

			return true;
        }
        public bool draw(int i, int j, SpriteBatch spriteBatch)
		{
			cooldown--;
			Tile tile = Main.tile[i, j];
			int left = i - tile.TileFrameX / 18;
			int top = j - tile.TileFrameY / 18;
			int type = 0;
			if (Main.tile[i, j].TileFrameX >= 18 && Main.tile[i, j].TileFrameX < 36 && Main.tile[i, j].TileFrameY % 36 >= 18)
				type = 1;
			int index = GetInstance<TransmutationAltarStorage>().Find(left, top);
			if (index == -1 || type != 1)
			{
				return true;
			}
			TransmutationAltarStorage entity = (TransmutationAltarStorage)TileEntity.ByID[index];
			Color color;
			color = WorldGen.paintColor((int)Main.tile[i, j].TileColor) * (100f / 255f);
			color.A = 0;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Vector2 dynamicAddition = new Vector2(3, 0).RotatedBy(MathHelper.ToRadians(Main.GlobalTimeWrappedHourly * 40));
			int amountOfUniqueItems = 1;
			int totalItems = 0;
			for (int l = 1; l < entity.itemsArray.Length; l++)
			{
				if (entity.itemsArray[l] != 0)
				{
					amountOfUniqueItems++;
					totalItems += entity.itemAmountsArray[l];
				}
			}
			Texture2D texture;
			float counter = Main.GlobalTimeWrappedHourly * (40f / (1 + 0.1f * (totalItems +  entity.itemAmountsArray[0])));
			//int[] alpha255Items = new int[] {mod.ItemType("DissolvingAether"), mod.ItemType("DissolvingNature") , mod.ItemType("DissolvingAurora") , mod.ItemType("DissolvingEarth"), mod.ItemType("PrecariousCluster") };
			int currentItem = 0;
			for (int l = 1; l < amountOfUniqueItems; l++)
			{
				texture = Terraria.GameContent.TextureAssets.Item[entity.itemsArray[l]].Value;
				for (int g = 0; g < entity.itemAmountsArray[l]; g++)
				{
					DrawAnimation anim = Main.itemAnimations[entity.itemsArray[l]];
					int frameCount = 1;
					int ticksPerFrame = 1;
					if (anim != null)
					{
						frameCount = anim.FrameCount;
						ticksPerFrame = anim.TicksPerFrame;
					}
					currentItem++;
					Vector2 pos = new Vector2((float)(i * 16 - (int)Main.screenPosition.X) + 8, (float)(j * 16 - (int)Main.screenPosition.Y) + 8) + zero;
					pos.Y -= 80 + dynamicAddition.Y + (totalItems + entity.itemAmountsArray[0]) * 0.5f;
					Vector2 circularLocation = new Vector2(32 + (totalItems + entity.itemAmountsArray[0]) * 0.5f, 0).RotatedBy(MathHelper.ToRadians(currentItem * 360f / totalItems) + MathHelper.ToRadians(counter));
					pos += circularLocation;
					DrawItem(texture, pos, frameCount, ticksPerFrame, color);
				}
			}
			texture = Terraria.GameContent.TextureAssets.Item[entity.itemsArray[0]].Value;
			for (int g = 0; g < entity.itemAmountsArray[0]; g++)
			{
				DrawAnimation anim = Main.itemAnimations[entity.itemsArray[0]];
				int frameCount = 1;
				int ticksPerFrame = 1;
				if (anim != null)
				{
					frameCount = anim.FrameCount;
					ticksPerFrame = anim.TicksPerFrame;
				}
				currentItem++;
				Vector2 pos = new Vector2((float)(i * 16 - (int)Main.screenPosition.X) + 8, (float)(j * 16 - (int)Main.screenPosition.Y) + 8) + zero;
				pos.Y -= 80 + dynamicAddition.Y + (totalItems + entity.itemAmountsArray[0]) * 0.5f;
				Vector2 circularLocation = new Vector2((entity.itemAmountsArray[0] > 1 ? 12 : 0) + entity.itemAmountsArray[0] * 0.5f, 0).RotatedBy(MathHelper.ToRadians(currentItem * 360f / entity.itemAmountsArray[0]) + MathHelper.ToRadians(counter));
				pos += circularLocation;
				DrawItem(texture, pos, frameCount, ticksPerFrame, color);
			}
			return true;
		}
		public void DrawItem(Texture2D texture, Vector2 pos, int frameCount, int ticksPerFrame, Color color)
		{
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / frameCount / 2);
			float width = texture.Width;
			float height = texture.Height / frameCount;
			float allocatedArea = 48;
			float scale = 0.8f;
			if (allocatedArea < (float)Math.Sqrt(width * height))
				scale = 0.8f * allocatedArea / (float)Math.Sqrt(width * height);
			for (int n = 0; n < 6; n++)
			{
				Vector2 augment = new Vector2(Main.rand.NextFloat(1.5f, 2.5f), 0).RotatedBy(MathHelper.ToRadians(n * 60));
				Main.spriteBatch.Draw(texture, pos + augment, new Rectangle(0, texture.Height / frameCount * ((int)Main.GameUpdateCount / ticksPerFrame % frameCount), texture.Width, texture.Height / frameCount), color, 0f, origin, scale, SpriteEffects.None, 0f);
			}
		}
	}
	public class UndoParticles : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Undo"); //Do you enjoy how all my net sycning is done via projectiles?
		}
		public override void SetDefaults()
		{
			Projectile.alpha = 255;
			Projectile.timeLeft = 24;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.width = 36;
			Projectile.height = 36;
		}
		public override bool? CanCutTiles()
		{
			return false;
		}
		public override void AI()
		{
			Projectile.alpha = 255;
			Projectile.Kill();
		}
		public override void Kill(int timeLeft)
		{
			Color white = Color.White;
			white.A = 0;
			Color color;
			color = WorldGen.paintColor((int)Main.tile[(int)Projectile.ai[0], (int)Projectile.ai[1]].TileColor);
			if ((int)Main.tile[(int)Projectile.ai[0], (int)Projectile.ai[1]].TileColor == 0)
				color = new Color(220, 60, 10);
			
			if(Projectile.knockBack == 1)
			{
				Terraria.Audio.SoundEngine.PlaySound(SoundID.Item4, Projectile.Center);
				Vector2 position = Projectile.Center;
				for (int k = 0; k < 360; k += 3)
				{
					Vector2 circularLocation = new Vector2(-Main.rand.Next(10), 0).RotatedBy(MathHelper.ToRadians(k));
					circularLocation += new Vector2(-Main.rand.Next(-1, 2), Main.rand.Next(-1, 2));

					int num1 = Dust.NewDust(new Vector2(position.X + circularLocation.X - 4, position.Y + circularLocation.Y - 4), 4, 4, DustType<CopyDust4>(), 0, 0, 0, color);
					Main.dust[num1].velocity = circularLocation;
					Main.dust[num1].noGravity = true;
					Main.dust[num1].fadeIn = 0.4f;
					Main.dust[num1].scale *= 1.5f;
					Main.dust[num1].shader = GameShaders.Armor.GetShaderFromItemId(ItemID.NegativeDye);
				}
				for (int k = 0; k < 360; k += 3)
				{
					Vector2 circularLocation = new Vector2(-Main.rand.Next(1, 5), 0).RotatedBy(MathHelper.ToRadians(k));
					circularLocation += new Vector2(-Main.rand.Next(-1, 2), Main.rand.Next(-1, 2));

					int num1 = Dust.NewDust(new Vector2(position.X + circularLocation.X - 4, position.Y + circularLocation.Y - 4), 4, 4, DustType<CopyDust4>(), 0, 0, 0, color);
					Main.dust[num1].velocity = circularLocation * 0.7f;
					Main.dust[num1].noGravity = true;
					Main.dust[num1].fadeIn = 0.4f;
					Main.dust[num1].scale *= 2.5f;
					Main.dust[num1].shader = GameShaders.Armor.GetShaderFromItemId(ItemID.NegativeDye);
				}
			}
			else
			{
				Terraria.Audio.SoundEngine.PlaySound(SoundID.Item4, Projectile.Center);
				Vector2 position = Projectile.Center;
				for (int k = 0; k < 360; k += 3)
				{
					Vector2 circularLocation = new Vector2(-Main.rand.Next(10), 0).RotatedBy(MathHelper.ToRadians(k));
					circularLocation += new Vector2(-Main.rand.Next(-1, 2), Main.rand.Next(-1, 2));

					int num1 = Dust.NewDust(new Vector2(position.X + circularLocation.X - 4, position.Y + circularLocation.Y - 4), 4, 4, DustType<CopyDust4>(), 0, 0, 0, color);
					Main.dust[num1].velocity = circularLocation;
					Main.dust[num1].noGravity = true;
					Main.dust[num1].fadeIn = 0.4f;
					Main.dust[num1].scale *= 1.5f;
				}
				for (int k = 0; k < 360; k += 3)
				{
					Vector2 circularLocation = new Vector2(-Main.rand.Next(1, 5), 0).RotatedBy(MathHelper.ToRadians(k));
					circularLocation += new Vector2(-Main.rand.Next(-1, 2), Main.rand.Next(-1, 2));

					int num1 = Dust.NewDust(new Vector2(position.X + circularLocation.X - 4, position.Y + circularLocation.Y - 4), 4, 4, DustType<CopyDust4>(), 0, 0, 0, color);
					Main.dust[num1].velocity = circularLocation * 0.7f;
					Main.dust[num1].noGravity = true;
					Main.dust[num1].fadeIn = 0.4f;
					Main.dust[num1].scale *= 2.5f;
				}
			}
		}
	}
	public class TransmutationAltarStorage : ModTileEntity
	{
		internal bool netUpdate = false;
		internal int[] itemsArray = new int[20];
		internal int[] itemAmountsArray = new int[20];
		internal int timer = -2;
		internal int style = 0;
		public override void Update()
		{
			if(timer == -2)
			{
				itemsArray[1] = ItemType<HardlightAlloy>();
				itemAmountsArray[1] = 1;
				itemsArray[2] = ItemType<StarlightAlloy>();
				itemAmountsArray[2] = 1;
				itemsArray[3] = ItemType<OtherworldlyAlloy>();
				itemAmountsArray[3] = 1;
				timer = -1;
				netUpdate = true;
			}
			if (netUpdate)
			{
				//Main.NewText("I just updated!");
				netUpdate = false;
				NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
			}
		}

        public override void NetReceive(BinaryReader reader)
        {
			netUpdate = reader.ReadBoolean();
			timer = reader.ReadInt32();
			style = reader.ReadInt32();
			for (int i = 0; i < 20; i++)
			{
				itemsArray[i] = reader.ReadInt32();
			}
			for (int i = 0; i < 20; i++)
			{
				itemAmountsArray[i] = reader.ReadInt32();
			}
			//if (!lightReceive)
				//Main.NewText("I received info");
		}

        public override void NetSend(BinaryWriter writer)
        {
			writer.Write(netUpdate);
			writer.Write(timer);
			writer.Write(style);
			for (int i = 0; i < 20; i++)
			{
				writer.Write(itemsArray[i]);
			}
			for (int i = 0; i < 20; i++)
			{
				writer.Write(itemAmountsArray[i]);
			}
			//if (!lightSend)
				//Main.NewText("I sent info");
		}

		public override void SaveData(TagCompound tag)/* Edit tag parameter rather than returning new TagCompound */
		{
			tag["timer"] = timer;
			tag["style"] = style;
			tag["itemsArray"] = itemsArray;
			tag["itemAmountsArray"] = itemAmountsArray;
		}

		public override void LoadData(TagCompound tag)
		{
			timer = tag.Get<int>("timer");
			style = tag.Get<int>("style");
			itemsArray = tag.Get<int[]>("itemsArray");
			itemAmountsArray = tag.Get<int[]>("itemAmountsArray");
		}

		public override bool IsTileValidForEntity(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			return tile.HasTile && tile.TileType == (ushort)TileType<TransmutationAltarTile>() && tile.TileFrameX == 0 && tile.TileFrameY == 0;
		}

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {
			//Main.NewText("i " + i + " j " + j + " t " + type + " s " + style + " d " + direction);
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				NetMessage.SendTileSquare(Main.myPlayer, i, j, 3, 2);
				NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type, 0f, 0, 0, 0);
				return -1;
			}
			return Place(i, j);
		}
	}
}