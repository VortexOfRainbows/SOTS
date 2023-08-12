using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Fragments;
using SOTS.Items.Otherworld.FromChests;
using System;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;
using Terraria.Localization;

namespace SOTS.Items.Otherworld.Furniture
{
	public class PotGenerator : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Otherworld/Furniture/PotGeneratorBase").Value;
			Main.spriteBatch.Draw(texture2, new Vector2(position.X, position.Y), null, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Otherworld/Furniture/PotGeneratorBase").Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture2, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, lightColor * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/Furniture/PotGeneratorMiniOutline").Value;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Otherworld/Furniture/PotGeneratorMiniFill").Value;
			Color color = new Color(110, 110, 110, 0);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.03f;
				float y = Main.rand.Next(-10, 11) * 0.03f;
				if(k == 0)
					Main.spriteBatch.Draw(texture2, new Vector2(position.X, position.Y), null, color * 0.5f, 0f, origin, scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture,new Vector2(position.X + x, position.Y + y),null, color * (1f - (Item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/Furniture/PotGeneratorMiniOutline").Value;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Otherworld/Furniture/PotGeneratorMiniFill").Value;
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.03f;
				float y = Main.rand.Next(-10, 11) * 0.03f;
				if (k == 0)
					Main.spriteBatch.Draw(texture2, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, color * 0.5f, rotation, drawOrigin, scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X) + x, (float)(Item.Center.Y - (int)Main.screenPosition.Y) + y),null, color * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.width = 26;
			Item.height = 36;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.createTile = TileType<PotGeneratorTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemType<DissolvingAether>(), 1).AddIngredient(ItemType<TwilightShard>(), 5).AddIngredient(ItemType<TwilightGel>(), 30).AddTile(TileType<HardlightFabricatorTile>()).Register();
		}
	}	
	public class PotGeneratorTile : ModTile
	{
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			float uniquenessCounter = Main.GlobalTimeWrappedHourly * -100 + (i + j) * 5;
			Tile tile = Main.tile[i, j];
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/Furniture/PotGeneratorTileGlow").Value;
			Rectangle frame = new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16);
			Color color;
			color = WorldGen.paintColor((int)Main.tile[i, j].TileColor) * (100f / 255f);
			color.A = 0;
			float alphaMult = 0.55f + 0.45f * (float)Math.Sin(MathHelper.ToRadians(uniquenessCounter));
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			for (int k = 0; k < 5; k++)
			{
				Vector2 pos = new Vector2((i * 16 - (int)Main.screenPosition.X), (j * 16 - (int)Main.screenPosition.Y)) + zero;
				Vector2 offset = new Vector2(Main.rand.NextFloat(-1, 1f), Main.rand.NextFloat(-1, 1f)) * 0.10f * k;
				Main.spriteBatch.Draw(texture, pos + offset, frame, color * alphaMult * 0.75f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
		public override void SetStaticDefaults()
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
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(180, 245, 240), name);
			TileID.Sets.DisableSmartCursor[Type] = true;
			DustType = DustType<AvaritianDust>();
		}
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return SOTSWorld.downedAdvisor;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int drop = ItemType<PotGenerator>();
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 16, drop);
			GetInstance<PotTimer>().Kill(i, j);
		}
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.cursorItemIconID = ItemType<PotGenerator>();
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
		public override bool RightClick(int i, int j)
		{
			Main.mouseRightRelease = true;
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			int left = i - tile.TileFrameX / 18;
			int top = j - tile.TileFrameY / 18;

			int index = GetInstance<PotTimer>().Find(left, top);
			if (index == -1)
			{
				return false;
			}
			PotTimer entity = (PotTimer)TileEntity.ByID[index];
			int seconds = entity.timer / 60;
			int secondsLeft = 60 - seconds;
			if (Main.tile[i, j - 1].TileType == Type)
			{
				Main.NewText(Language.GetTextValue("Mods.SOTS.PotGeneratorTileText.0"));
			}
			else if (entity.timer < 0)
			{
				Main.NewText(Language.GetTextValue("Mods.SOTS.PotGeneratorTileText.1"));
			}
			else
			{
				Main.NewText(Language.GetTextValue("Mods.SOTS.PotGeneratorTileText.2", secondsLeft));
			}
			return true;
		}
		public override bool Slope(int i, int j)
		{
			return false;
		}
        public override void NearbyEffects(int i, int j, bool closer)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			int left = i - tile.TileFrameX / 18;
			int top = j - tile.TileFrameY / 18;
			closer = true;
			int index = GetInstance<PotTimer>().Find(left, top);
			if (index == -1)
			{
				return;
			}
			PotTimer entity = (PotTimer)TileEntity.ByID[index];
			base.NearbyEffects(i, j, closer);
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if (Main.tile[i, j].TileFrameX < 18 || Main.tile[i, j].TileFrameX > 35 || Main.tile[i, j].TileFrameY % 36 < 18)
				return;

			r = 1.2f;
			g = 1.2f;
			b = 1.2f;
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			int left = i - tile.TileFrameX / 18;
			int top = j - tile.TileFrameY / 18;
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
			if (Main.tile[i, j].TileFrameX >= 18)
				return true;
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/Furniture/SkyPotsGlowOutline").Value;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Otherworld/Furniture/SkyPotsGlowFill").Value;
			Color color;
			color = WorldGen.paintColor((int)Main.tile[i, j].TileColor) * (100f / 255f);
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
		internal int updateTimer = 0;
		public override void Update()
		{
			bool netUpdate = false;
			bool forceUpdate = false;
			int whereX = Position.X;
			int whereY = Position.Y - 1;
			int amt = 1;
			bool tilesAbove = !WorldGen.InWorld(whereX, whereY, 20)|| Main.tile[whereX, whereY].HasTile || Main.tile[whereX + 1, whereY].HasTile || Main.tile[whereX, whereY - 1].HasTile || Main.tile[whereX + 1, whereY - 1].HasTile;
			if (tilesAbove && timer != -2)
			{
				timer = -2;
				netUpdate = true;
				forceUpdate = true;
			}
			else
            {
				for (int i = 1; i < 3600; i++)
				{
					if (WorldGen.InWorld(whereX, Position.Y + i, 10) && Main.tile[whereX, Position.Y + i].HasTile && Main.tile[whereX + 1, Position.Y + i].HasTile && Main.tile[whereX, Position.Y + i].TileType == TileType<PotGeneratorTile>() && Main.tile[whereX + 1, Position.Y + i].TileType == TileType<PotGeneratorTile>())
					{
						amt++;
					}
					else
					{
						break;
					}
				}
				Vector2 position = new Vector2(whereX * 16 + 16, whereY * 16);
				bool playerNear = true;
				if (Main.player.Count(x => x.Distance(position) < 1280f) <= 0)
				{
					playerNear = false;
				}
				for (; amt > 0; amt--)
				{
					if (timer >= 0)
					{
						timer++;
						netUpdate = true;
					}
					if (timer % 20 == 0)
					{
						if (playerNear)
						{
							if (Main.rand.NextBool(amt * 2))
							{
								Projectile.NewProjectile(new EntitySource_Misc("SOTS:PotGenerator"), position, Vector2.Zero, ProjectileType<PotProjectile>(), 0, 0, Main.myPlayer, 1, style);
							}
						}
						// Sending 86 aka, TileEntitySharing, triggers NetSend. Think of it like manually calling sync.;
					}
					if (timer >= 3600)
					{
						timer = -1;
						netUpdate = true;
					}
					if (timer == -1)
					{
						timer = -2;
						//WorldGen.PlaceTile(whereX, whereY, mod.TileType("SkyPots"), false, false, -1, style);
						Projectile.NewProjectile(new EntitySource_Misc("SOTS:PotGenerator"), position, Vector2.Zero, ProjectileType<PotProjectile>(), 0, 0, Main.myPlayer, 0, style);
						netUpdate = true;
					}
					if (timer == -2 && !Main.tile[whereX, whereY].HasTile && !Main.tile[whereX + 1, whereY].HasTile && !Main.tile[whereX, whereY - 1].HasTile && !Main.tile[whereX + 1, whereY - 1].HasTile)
					{
						timer = 0;
						style = Main.rand.Next(9);
						netUpdate = true;
					}
				}
				if (!playerNear)
					netUpdate = false;
			}
			if(netUpdate)
            {
				updateTimer++;
				if(updateTimer % 10 == 0 || forceUpdate)
					NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
			}
		}

        public override void NetReceive(BinaryReader reader)
		{
			timer = reader.ReadInt32();
			style = reader.ReadInt32();
		}

        public override void NetSend(BinaryWriter writer)
        {
			writer.Write(timer);
			writer.Write(style);
		}

		public override void SaveData(TagCompound tag)/* Edit tag parameter rather than returning new TagCompound */
		{
			tag["timer"] = timer;
			tag["style"] = style;
		}

		public override void LoadData(TagCompound tag)
		{
			timer = tag.Get<int>("timer");
			style = tag.Get<int>("style");
		}

		public override bool IsTileValidForEntity(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			return tile.HasTile && tile.TileType == (ushort)ModContent.TileType<PotGeneratorTile>() && tile.TileFrameX == 0 && tile.TileFrameY == 0;
		}

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
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
		public override void SetDefaults() //Do you enjoy how all my net sycning is done via projectiles?
		{
			Projectile.alpha = 255;
			Projectile.timeLeft = 24;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.netImportant = true;
			Projectile.width = 26;
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
			int i = (int)Projectile.Center.X / 16;
			i--;
			int j = (int)Projectile.Center.Y / 16;
			Color white = Color.White;
			white.A = 0;
			Color color;
			color = WorldGen.paintColor((int)Main.tile[i, j + 1].TileColor);
			color.A = 0;
			if (Projectile.ai[0] == 0)
			{
				WorldGen.PlaceTile(i, j, TileType<SkyPots>(), false, false, -1, (int)Projectile.ai[1]);
				if(Main.netMode != NetmodeID.MultiplayerClient)
					NetMessage.SendTileSquare(Main.myPlayer, i, j, 3);
				Terraria.Audio.SoundEngine.PlaySound(SoundID.Item4, Projectile.Center);
				Vector2 position = Projectile.Center;
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
			if (Projectile.ai[0] == 1)
			{
				Vector2 position = Projectile.Center;
				for (int k = 0; k < 360; k += 15)
				{
					Vector2 circularLocation = new Vector2(-24, 0).RotatedBy(MathHelper.ToRadians(k));
					circularLocation += 0.5f * new Vector2(Main.rand.Next(-1, 2), Main.rand.Next(-1, 2));
					int type = DustID.Electric;
					int type2 = 132;
					if (white != color)
						type = type2;
					if (Main.rand.NextBool(30))
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