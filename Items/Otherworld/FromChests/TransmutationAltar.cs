using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;


namespace SOTS.Items.Otherworld.FromChests
{
	public class TransmutationAltar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Transmutation Altar");
			Tooltip.SetDefault("Used for crafting\nAllows the conversion of material and equipment to equivalent but different forms\nAlso counts as a demon altar for crafting\nCrafting using the altar can be undone");
		}
		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 40;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = 9;
			item.value = Item.sellPrice(0, 20, 0, 0);
			item.consumable = true;
			item.createTile = mod.TileType("TransmutationAltarTile");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PrecariousCluster", 1);
			recipe.AddIngredient(null, "TwilightShard", 20);
			recipe.AddIngredient(null, "OtherworldlyAlloy", 20);
			recipe.AddIngredient(null, "HardlightAlloy", 10);
			recipe.AddIngredient(null, "StarlightAlloy", 10);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
	public class TransmutationAltarTile : ModTile
	{
		public override void SetDefaults()
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
			disableSmartCursor = true;
			dustType = mod.DustType("AvaritianDust");
			adjTiles = new int[] { TileID.DemonAltar };
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int drop = mod.ItemType("TransmutationAltar");
			Item.NewItem(i * 16, j * 16, 48, 32, drop);
			ModContent.GetInstance<TransmutationAltarStorage>().Kill(i, j);
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if (Main.tile[i, j].frameX < 18 || Main.tile[i, j].frameX > 35 || Main.tile[i, j].frameY % 36 < 18)
				return;

			r = 1.0f;
			g = 0.1f;
			b = 0.1f;
		}
		public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
		{
			int type = 0;
			if (Main.tile[i, j].frameX >= 18 && Main.tile[i, j].frameX < 36 && Main.tile[i, j].frameY % 36 >= 18)
				type = 1;
			Tile t = Main.tile[i, j];
			if (type == 1) // t.frameX % 54 == 0
			{
				Main.specX[nextSpecialDrawIndex] = i;
				Main.specY[nextSpecialDrawIndex] = j;
				nextSpecialDrawIndex++;
			}
		}
		public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
        {
            draw(i, j, spriteBatch);
		}
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			int left = i;
			int top = j;

			player.showItemIcon2 = mod.ItemType("UndoArrow");
			//player.showItemIconText = "";
			player.noThrow = 2;
			player.showItemIcon = true;
		}
		public override void MouseOverFar(int i, int j)
		{
			MouseOver(i, j);
			Player player = Main.LocalPlayer;
			if (player.showItemIconText == "")
			{
				player.showItemIcon = false;
				player.showItemIcon2 = 0;
			}
		}
		float cooldown = 0f;
		public override bool NewRightClick(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int left = i - tile.frameX / 18;
			int top = j - tile.frameY / 18;
			int index = GetInstance<TransmutationAltarStorage>().Find(left, top);
			TransmutationAltarStorage entity = (TransmutationAltarStorage)TileEntity.ByID[index];

			Vector2 dynamicAddition = new Vector2(3, 0).RotatedBy(MathHelper.ToRadians(Main.GlobalTime * 40));
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
			int amountHas = 0;
			for (int k = 0; k < 50; k++)
			{
				Item item = player.inventory[k];
				if (item.type == entity.itemsArray[0])
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
						if (Main.myPlayer == player.whoAmI)
						{
							int item = Item.NewItem((int)pos.X, (int)pos.Y, 0, 0, entity.itemsArray[l], entity.itemAmountsArray[l]);
							Main.item[item].velocity += new Vector2(-3, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
							NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f, 0.0f, 0.0f, 0, 0, 0);
						}
					}
				}
				if (Main.myPlayer == player.whoAmI)
				{
					Projectile.NewProjectile(pos, Vector2.Zero, mod.ProjectileType("UndoParticles"), 0, 0, Main.myPlayer, i, j);
				}
			}

			return true;
        }
        public bool draw(int i, int j, SpriteBatch spriteBatch)
		{
			cooldown--;
			Tile tile = Main.tile[i, j];
			int left = i - tile.frameX / 18;
			int top = j - tile.frameY / 18;
			int type = 0;
			if (Main.tile[i, j].frameX >= 18 && Main.tile[i, j].frameX < 36 && Main.tile[i, j].frameY % 36 >= 18)
				type = 1;
			int index = GetInstance<TransmutationAltarStorage>().Find(left, top);
			if (index == -1 || type != 1)
			{
				return true;
			}
			TransmutationAltarStorage entity = (TransmutationAltarStorage)TileEntity.ByID[index];
			Color color;
			color = WorldGen.paintColor((int)Main.tile[i, j].color()) * (100f / 255f);
			color.A = 0;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Vector2 dynamicAddition = new Vector2(3, 0).RotatedBy(MathHelper.ToRadians(Main.GlobalTime * 40));
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
			float counter = Main.GlobalTime * (40f / (1 + 0.1f * (totalItems +  entity.itemAmountsArray[0])));
			int[] alpha255Items = new int[] {mod.ItemType("DissolvingAether"), mod.ItemType("DissolvingNature") , mod.ItemType("DissolvingAurora") , mod.ItemType("DissolvingEarth"), mod.ItemType("PrecariousCluster") };
			int currentItem = 0;
			for (int l = 1; l < amountOfUniqueItems; l++)
			{
				texture = Main.itemTexture[entity.itemsArray[l]];
				for (int g = 0; g < entity.itemAmountsArray[l]; g++)
				{
					int frame = entity.itemFrames[l] <= 0 ? 1 : entity.itemFrames[l];
					currentItem++;
					Vector2 pos = new Vector2((float)(i * 16 - (int)Main.screenPosition.X) + 8, (float)(j * 16 - (int)Main.screenPosition.Y) + 8) + zero;
					pos.Y -= 80 + dynamicAddition.Y + (totalItems + entity.itemAmountsArray[0]) * 0.5f;
					Vector2 circularLocation = new Vector2(32 + (totalItems + entity.itemAmountsArray[0]) * 0.5f, 0).RotatedBy(MathHelper.ToRadians(currentItem * 360f / totalItems) + MathHelper.ToRadians(counter));
					pos += circularLocation;
					Vector2 origin = new Vector2(texture.Width / 2, texture.Height / frame / 2);
					float width = texture.Width;
					float height = texture.Height / frame;
					float allocatedArea = 48;
					float scale = 0.8f;
					if (allocatedArea < (float)Math.Sqrt(width * height))
						scale = 0.8f * allocatedArea / (float)Math.Sqrt(width * height);
					for (int k = 0; k < 10; k++)
					{
						float x = Main.rand.Next(-10, 11) * 0.2f;
						float y = Main.rand.Next(-10, 11) * 0.2f;
						Main.spriteBatch.Draw(texture, pos + new Vector2(x, y), new Rectangle(0, 0, texture.Width, texture.Height/frame), color, 0f, origin, scale, SpriteEffects.None, 0f);
						if (k == 9 && !alpha255Items.Contains<int>(entity.itemsArray[l]))
							Main.spriteBatch.Draw(texture, pos, new Rectangle(0, 0, texture.Width, texture.Height / frame), new Color(255, 255, 255), 0f, origin, scale, SpriteEffects.None, 0f);
					}
				}
			}
			texture = Main.itemTexture[entity.itemsArray[0]];
			for (int g = 0; g < entity.itemAmountsArray[0]; g++)
			{
				int frame = entity.itemFrames[0] <= 0 ? 1 : entity.itemFrames[0];
				currentItem++;
				Vector2 pos = new Vector2((float)(i * 16 - (int)Main.screenPosition.X) + 8, (float)(j * 16 - (int)Main.screenPosition.Y) + 8) + zero;
				pos.Y -= 80 + dynamicAddition.Y + (totalItems + entity.itemAmountsArray[0]) * 0.5f;
				Vector2 circularLocation = new Vector2((entity.itemAmountsArray[0] > 1 ? 12 : 0) + entity.itemAmountsArray[0] * 0.5f, 0).RotatedBy(MathHelper.ToRadians(currentItem * 360f / entity.itemAmountsArray[0]) + MathHelper.ToRadians(counter));
				pos += circularLocation;
				Vector2 origin = new Vector2(texture.Width / 2, texture.Height / frame / 2);
				float width = texture.Width;
				float height = texture.Height / frame;
				float allocatedArea = 48;
				float scale = 0.8f;
				if(allocatedArea < (float)Math.Sqrt(width * height))
					scale = 0.8f * allocatedArea / (float)Math.Sqrt(width * height);
				for (int n = 0; n < 10; n++)
				{
					float x = Main.rand.Next(-10, 11) * 0.2f;
					float y = Main.rand.Next(-10, 11) * 0.2f;
					Main.spriteBatch.Draw(texture, pos + new Vector2(x, y), new Rectangle(0, 0, texture.Width, texture.Height / frame), color, 0f, origin, scale, SpriteEffects.None, 0f);
					if (n == 9 && !alpha255Items.Contains<int>(entity.itemsArray[0]))
						Main.spriteBatch.Draw(texture, pos, new Rectangle(0, 0, texture.Width, texture.Height / frame), new Color(255, 255, 255), 0f, origin, scale, SpriteEffects.None, 0f);
				}
			}
			return true;
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
			projectile.alpha = 255;
			projectile.timeLeft = 24;
			projectile.friendly = false;
			projectile.tileCollide = false;
			projectile.width = 36;
			projectile.height = 36;
		}
		public override bool? CanCutTiles()
		{
			return false;
		}
		public override void AI()
		{
			projectile.alpha = 255;
			projectile.Kill();
		}
		public override void Kill(int timeLeft)
		{
			Color white = Color.White;
			white.A = 0;
			Color color;
			color = WorldGen.paintColor((int)Main.tile[(int)projectile.ai[0], (int)projectile.ai[1]].color());
			if ((int)Main.tile[(int)projectile.ai[0], (int)projectile.ai[1]].color() == 0)
				color = new Color(220, 60, 10);
			
			if(projectile.knockBack == 1)
			{
				Main.PlaySound(SoundID.Item4, projectile.Center);
				Vector2 position = projectile.Center;
				for (int k = 0; k < 360; k += 3)
				{
					Vector2 circularLocation = new Vector2(-Main.rand.Next(10), 0).RotatedBy(MathHelper.ToRadians(k));
					circularLocation += new Vector2(-Main.rand.Next(-1, 2), Main.rand.Next(-1, 2));

					int num1 = Dust.NewDust(new Vector2(position.X + circularLocation.X - 4, position.Y + circularLocation.Y - 4), 4, 4, mod.DustType("CopyDust4"), 0, 0, 0, color);
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

					int num1 = Dust.NewDust(new Vector2(position.X + circularLocation.X - 4, position.Y + circularLocation.Y - 4), 4, 4, mod.DustType("CopyDust4"), 0, 0, 0, color);
					Main.dust[num1].velocity = circularLocation * 0.7f;
					Main.dust[num1].noGravity = true;
					Main.dust[num1].fadeIn = 0.4f;
					Main.dust[num1].scale *= 2.5f;
					Main.dust[num1].shader = GameShaders.Armor.GetShaderFromItemId(ItemID.NegativeDye);
				}
			}
			else
			{
				Main.PlaySound(SoundID.Item4, projectile.Center);
				Vector2 position = projectile.Center;
				for (int k = 0; k < 360; k += 3)
				{
					Vector2 circularLocation = new Vector2(-Main.rand.Next(10), 0).RotatedBy(MathHelper.ToRadians(k));
					circularLocation += new Vector2(-Main.rand.Next(-1, 2), Main.rand.Next(-1, 2));

					int num1 = Dust.NewDust(new Vector2(position.X + circularLocation.X - 4, position.Y + circularLocation.Y - 4), 4, 4, mod.DustType("CopyDust4"), 0, 0, 0, color);
					Main.dust[num1].velocity = circularLocation;
					Main.dust[num1].noGravity = true;
					Main.dust[num1].fadeIn = 0.4f;
					Main.dust[num1].scale *= 1.5f;
				}
				for (int k = 0; k < 360; k += 3)
				{
					Vector2 circularLocation = new Vector2(-Main.rand.Next(1, 5), 0).RotatedBy(MathHelper.ToRadians(k));
					circularLocation += new Vector2(-Main.rand.Next(-1, 2), Main.rand.Next(-1, 2));

					int num1 = Dust.NewDust(new Vector2(position.X + circularLocation.X - 4, position.Y + circularLocation.Y - 4), 4, 4, mod.DustType("CopyDust4"), 0, 0, 0, color);
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
		internal int[] itemFrames = new int[20];
		internal int timer = -2;
		internal int style = 0;
		public override void Update()
		{
			if(timer == -2)
			{
				itemsArray[1] = mod.ItemType("HardlightAlloy");
				itemAmountsArray[1] = 1;
				itemFrames[1] = 1;
				itemsArray[2] = mod.ItemType("StarlightAlloy");
				itemAmountsArray[2] = 1;
				itemFrames[2] = 1;
				itemsArray[3] = mod.ItemType("OtherworldlyAlloy");
				itemAmountsArray[3] = 1;
				itemFrames[3] = 1;
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

		public override void NetReceive(BinaryReader reader, bool lightReceive)
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
			for (int i = 0; i < 20; i++)
			{
				itemFrames[i] = reader.ReadInt32();
			}
			//if (!lightReceive)
				//Main.NewText("I received info");
		}

		public override void NetSend(BinaryWriter writer, bool lightSend)
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
			for (int i = 0; i < 20; i++)
			{
				writer.Write(itemFrames[i]);
			}
			//if (!lightSend)
				//Main.NewText("I sent info");
		}

		public override TagCompound Save()
		{
			return new TagCompound
			{
				{"timer", timer},
				{"style", style},
				{"itemsArray", itemsArray},
				{"itemAmountsArray", itemAmountsArray},
				{"itemFrames", itemFrames},
			};
		}

		public override void Load(TagCompound tag)
		{
			timer = tag.Get<int>("timer");
			style = tag.Get<int>("style");
			itemsArray = tag.Get<int[]>("itemsArray");
			itemAmountsArray = tag.Get<int[]>("itemAmountsArray");
			itemFrames = tag.Get<int[]>("itemFrames");
		}

		public override bool ValidTile(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			return tile.active() && tile.type == (ushort)mod.TileType("TransmutationAltarTile") && tile.frameX == 0 && tile.frameY == 0;
		}

		public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
		{
			//Main.NewText("i " + i + " j " + j + " t " + type + " s " + style + " d " + direction);
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				NetMessage.SendTileRange(Main.myPlayer, i, j, 3, 2);
				NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type, 0f, 0, 0, 0);
				return -1;
			}
			return Place(i, j);
		}
	}
}