using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace SOTS.Items.Otherworld
{
	public class PotGenerator : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pot Generator");
			Tooltip.SetDefault("Generates pots on top of its surface\nCan be stacked to increase production of the topmost generator\n'No, not that type of pot'");
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture2 = mod.GetTexture("Items/Otherworld/PotGeneratorBase");
			Main.spriteBatch.Draw(texture2, new Vector2(position.X, position.Y), null, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture2 = mod.GetTexture("Items/Otherworld/PotGeneratorBase");
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			Main.spriteBatch.Draw(texture2, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2), null, lightColor * (1f - (item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = mod.GetTexture("Items/Otherworld/PotGeneratorMiniOutline");
			Texture2D texture2 = mod.GetTexture("Items/Otherworld/PotGeneratorMiniFill");
			Color color = new Color(110, 110, 110, 0);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.03f;
				float y = Main.rand.Next(-10, 11) * 0.03f;
				if(k == 0)
					Main.spriteBatch.Draw(texture2, new Vector2(position.X, position.Y), null, color * 0.5f, 0f, origin, scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture,new Vector2(position.X + x, position.Y + y),null, color * (1f - (item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Otherworld/PotGeneratorMiniOutline");
			Texture2D texture2 = mod.GetTexture("Items/Otherworld/PotGeneratorMiniFill");
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.03f;
				float y = Main.rand.Next(-10, 11) * 0.03f;
				if (k == 0)
					Main.spriteBatch.Draw(texture2, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color * 0.5f, rotation, drawOrigin, scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X) + x, (float)(item.Center.Y - (int)Main.screenPosition.Y) + y + 2),null, color * (1f - (item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 36;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = 9;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.consumable = true;
			item.createTile = mod.TileType("PotGeneratorTile");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DissolvingAether", 1);
			recipe.AddIngredient(null, "TwilightShard", 5);
			recipe.AddIngredient(null, "TwilightGel", 30);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}	
	public class PotGeneratorTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileSolidTop[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
			//TileObjectData.newTile.HookPostPlaceEveryone = new PlacementHook(GetInstance<PotTimer>().Hook_AfterPlacement, -1, 0, true);
			//TileObjectData.newTile.HookPlaceOverride = new PlacementHook(GetInstance<PotTimer>().Hook_AfterPlacement, -1, 0, true);
			//TileObjectData.newTile.HookCheck = new PlacementHook(GetInstance<PotTimer>().Hook_AfterPlacement, -1, 0, true);
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(GetInstance<PotTimer>().Hook_AfterPlacement, -1, 0, true);
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.CoordinateHeights = new[] { 16 };
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Digital Display");
			AddMapEntry(new Color(180, 245, 240), name);
			disableSmartCursor = true;
			dustType = mod.DustType("AvaritianDust");
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int drop = mod.ItemType("PotGenerator");
			Item.NewItem(i * 16, j * 16, 32, 16, drop);
			GetInstance<PotTimer>().Kill(i, j);
		}
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.showItemIcon2 = mod.ItemType("PotGenerator");
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
		public override bool NewRightClick(int i, int j)
		{
			Main.mouseRightRelease = true;
			Player player = Main.LocalPlayer;

			Tile tile = Main.tile[i, j];
			int left = i - tile.frameX / 18;
			int top = j - tile.frameY / 18;

			int index = GetInstance<PotTimer>().Find(left, top);
			if (index == -1)
			{
				return false;
			}
			PotTimer entity = (PotTimer)TileEntity.ByID[index];
			int seconds = entity.timer / 60;
			int secondsLeft = 60 - seconds;
			if (Main.tile[i, j - 1].type == Type)
			{
				Main.NewText("Status: N/A");
			}
			else if (entity.timer < 0)
			{
				Main.NewText("Status: Complete");
			}
			else
			{
				Main.NewText("Time left: " + secondsLeft);
			}
			return true;
		}
		public override bool Slope(int i, int j)
		{
			return false;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if (Main.tile[i, j].frameX < 18 || Main.tile[i, j].frameX > 35 || Main.tile[i, j].frameY % 36 < 18)
				return;

			r = 1.2f;
			g = 1.2f;
			b = 1.2f;
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			int left = i - tile.frameX / 18;
			int top = j - tile.frameY / 18;
			int index = GetInstance<PotTimer>().Find(left, top);
			if (index == -1)
			{
				return false;
			}
			PotTimer entity = (PotTimer)TileEntity.ByID[index];
			float ofMax = entity.timer / 3600f;
			ofMax += 0.1f;
			if (entity.timer < 0) ofMax = 0;
			int style = entity.style % 9;
			if (Main.tile[i, j].frameX >= 18)
				return true;
			Texture2D texture = mod.GetTexture("Items/Otherworld/SkyPotsGlowOutline");
			Texture2D texture2 = mod.GetTexture("Items/Otherworld/SkyPotsGlowFill");
			ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)((ulong)i));
			Color color;
			color = WorldGen.paintColor((int)Main.tile[i, j].color());
			color.A = 0;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Vector2 dynamicAddition = new Vector2(0, -4 * ofMax).RotatedBy(MathHelper.ToRadians(entity.timer));
			for (int k = 0; k < 5; k++)
			{
				Vector2 pos = new Vector2((float)(i * 16 - (int)Main.screenPosition.X) + 16, (float)(j * 16 - (int)Main.screenPosition.Y)) + zero;
				pos.Y -= 18 + dynamicAddition.Y;
				if(k == 0)
					Main.spriteBatch.Draw(texture2, pos, new Rectangle(0, 34 * style, 32, 32), color * 0.5f * ofMax, 0f, new Vector2(16, 16), ofMax, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture, pos, new Rectangle(0, 34 * style, 32, 32), color * ofMax, 0f, new Vector2(16, 16), ofMax, SpriteEffects.None, 0f);
			}
			return true;
		}
	}
	public class PotTimer : ModTileEntity
	{
		internal int timer = -2;
		internal int style = 0;
		public override void Update()
		{
			int whereX = Position.X;
			int whereY = Position.Y - 1;
			int amt = 1;
			for (int i = 0; i < 3600; i++)
			{
				if (WorldGen.InWorld(whereX, Position.Y + i, 10) && Main.tile[whereX, Position.Y + i].active() && Main.tile[whereX + 1, Position.Y + i].active() && Main.tile[whereX, Position.Y + i].type == mod.TileType("PotGeneratorTile") && Main.tile[whereX + 1, Position.Y + i].type == mod.TileType("PotGeneratorTile"))
				{
					amt++;
				}
				else
				{
					break;
				}
			}
			for(; amt > 0; amt--)
			{
				if (timer >= 0)
				{
					timer++;
					if (amt == 1)
						NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
				}
				if (timer % 20 == 0)
				{
					Vector2 position = new Vector2(whereX * 16 + 16, whereY * 16);
					if (Main.rand.Next(amt) == 0)
						Projectile.NewProjectile(position, Vector2.Zero, mod.ProjectileType("PotProjectile"), 0, 0, Main.myPlayer, 1, style);
					// Sending 86 aka, TileEntitySharing, triggers NetSend. Think of it like manually calling sync.;
				}
				if (timer >= 3600)
				{
					timer = -1;
					NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
				}
				if (timer == -1)
				{
					timer = -2;

					//WorldGen.PlaceTile(whereX, whereY, mod.TileType("SkyPots"), false, false, -1, style);
					Vector2 position = new Vector2(whereX * 16 + 16, whereY * 16);
					Projectile.NewProjectile(position, Vector2.Zero, mod.ProjectileType("PotProjectile"), 0, 0, Main.myPlayer, 0, style);
					NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
				}
				if (timer == -2 && !Main.tile[whereX, whereY].active() && !Main.tile[whereX + 1, whereY].active() && !Main.tile[whereX, whereY - 1].active() && !Main.tile[whereX + 1, whereY - 1].active())
				{
					timer = 0;
					style = Main.rand.Next(9);
					NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
				}
				if (Main.tile[whereX, whereY].active() || Main.tile[whereX + 1, whereY].active() || Main.tile[whereX, whereY - 1].active() || Main.tile[whereX + 1, whereY - 1].active())
				{
					timer = -2;
					if (amt == 1)
						NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
				}
			}
		}

		public override void NetReceive(BinaryReader reader, bool lightReceive)
		{
			timer = reader.ReadInt32();
			style = reader.ReadInt32();
		}

		public override void NetSend(BinaryWriter writer, bool lightSend)
		{
			writer.Write(timer);
			writer.Write(style);
		}

		public override TagCompound Save()
		{
			return new TagCompound
			{
				{"timer", timer},
				{"style", style},
			};
		}

		public override void Load(TagCompound tag)
		{
			timer = tag.Get<int>("timer");
			style = tag.Get<int>("style");
		}

		public override bool ValidTile(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			return tile.active() && tile.type == (ushort)mod.TileType("PotGeneratorTile") && tile.frameX == 0 && tile.frameY == 0;
		}

		public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
		{
			//Main.NewText("i " + i + " j " + j + " t " + type + " s " + style + " d " + direction);
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				NetMessage.SendTileSquare(Main.myPlayer, i, j, 3);
				NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type, 0f, 0, 0, 0);
				return -1;
			}
			return Place(i, j);
		}
	}
	public class PotProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pot Projectile"); //Do you enjoy how all my net sycning is done via projectiles?
		}
		public override void SetDefaults()
		{
			projectile.alpha = 255;
			projectile.timeLeft = 24;
			projectile.friendly = false;
			projectile.tileCollide = false;
			projectile.width = 26;
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
			int i = (int)projectile.Center.X / 16;
			i--;
			int j = (int)projectile.Center.Y / 16;
			Color white = Color.White;
			white.A = 0;
			Color color;
			color = WorldGen.paintColor((int)Main.tile[i, j + 1].color());
			color.A = 0;
			if (projectile.ai[0] == 0)
			{
				WorldGen.PlaceTile(i, j, mod.TileType("SkyPots"), false, false, -1, (int)projectile.ai[1]);
				Main.PlaySound(SoundID.Item4, projectile.Center);
				Vector2 position = projectile.Center;
				for (int k = 0; k < 360; k += 15)
				{
					Vector2 circularLocation = new Vector2(-4, 0).RotatedBy(MathHelper.ToRadians(k));
					circularLocation += new Vector2(Main.rand.Next(-1, 2), Main.rand.Next(-1, 2));
					int type = DustID.Electric;
					int type2 = 132;
					if (white != color)
						type = type2;
					int num1 = Dust.NewDust(new Vector2(position.X + circularLocation.X - 4, position.Y + circularLocation.Y - 4), 4, 4, type, 0, 0, 0, color);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity = circularLocation;
				}
			}
			if (projectile.ai[0] == 1)
			{
				Vector2 position = projectile.Center;
				for (int k = 0; k < 360; k += 15)
				{
					Vector2 circularLocation = new Vector2(-24, 0).RotatedBy(MathHelper.ToRadians(k));
					circularLocation += 0.5f * new Vector2(Main.rand.Next(-1, 2), Main.rand.Next(-1, 2));
					int type = DustID.Electric;
					int type2 = 132;
					if (white != color)
						type = type2;
					if (Main.rand.Next(30) == 0)
					{
						int num1 = Dust.NewDust(new Vector2(position.X + circularLocation.X - 4, position.Y + circularLocation.Y - 4), 4, 4, type, 0, 0, 0, color);
						Main.dust[num1].noGravity = true;
						Main.dust[num1].velocity = -circularLocation * 0.07f;
					}
				}
			}
		}
	}
}