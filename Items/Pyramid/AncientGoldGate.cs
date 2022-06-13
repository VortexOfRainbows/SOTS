using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.NPCs.Boss.Curse;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Pyramid
{
	public class AncientGoldGate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Gold Pillar");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 44;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.consumable = true;
			Item.createTile = ModContent.TileType<AncientGoldGateTile>();
			Item.placeStyle = 4;
		}
	}	
	public class AncientGoldGateTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			TileID.Sets.DrawsWalls[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileBlockLight[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Height = 5;
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16 };
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 2;
			TileObjectData.newTile.StyleMultiplier = 2;
			TileObjectData.newTile.DrawYOffset = 0;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 2, 0); 
			TileObjectData.newTile.Origin = new Point16(1, 4);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Strange Pillar");
			AddMapEntry(new Color(220, 180, 25), name);
			TileID.Sets.DisableSmartCursor[Type] = true;
			DustType = DustID.GoldCoin;
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameY < 270)
				DrawGems(i, j, spriteBatch);
			/* 
			// Flips the sprite
			SpriteEffects effects = SpriteEffects.None;
			Texture2D texture = Terraria.GameContent.TextureAssets.Item[Type].Value;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			} 
			Main.spriteBatch.Draw(texture,
				new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
				new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16),
				Lighting.GetColor(i, j), 0f, default(Vector2), 1f, effects, 0f); */

			return true;
		}
		public void DrawGems(int i, int j, SpriteBatch spriteBatch)
		{
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Tile tile = Main.tile[i, j];
			float counter = Main.GlobalTimeWrappedHourly * 120;
			float mult = new Vector2(-1f, 0).RotatedBy(MathHelper.ToRadians(counter / 2f)).X;
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Pyramid/AncientGoldGateGems").Value;
			if (tile.TileFrameY % 90 == 0 && tile.TileFrameX % 36 == 0) //check for it being the top left tile
			{
				int currentFrame = tile.TileFrameY / 90;
				for (int k = 0; k < 6; k++)
				{
					Color color = new Color(255, 0, 0, 0);
					switch (k)
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
					for (int c = currentFrame; c >= 1; c--)
					{
						Rectangle frame = new Rectangle(0, 0, 0, 0);
						if (c == 1)
						{
							frame = new Rectangle(0, 49, 32, 18);
						}
						if (c == 2)
						{
							frame = new Rectangle(0, 31, 32, 18);
						}
						if (c == 3)
						{
							frame = new Rectangle(0, 13, 32, 18);
						}
						Vector2 rotationAround = new Vector2((2 + mult), 0).RotatedBy(MathHelper.ToRadians(60 * k + counter));
						spriteBatch.Draw(texture, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + frame.Y) + zero + rotationAround,
							frame, color, 0f, default(Vector2), 1.0f, tile.TileFrameX > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
					}
				}
			}
		}
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameY < 270)
			{
				player.cursorItemIconID = ModContent.ItemType<RubyKeystone>();
				player.noThrow = 2;
				player.cursorItemIconEnabled = true;
			}
			else
			{
				player.cursorItemIconID = 0;
				player.cursorItemIconEnabled = false;
			}
		}
		public override void MouseOverFar(int i, int j)
		{
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			MouseOver(i, j);
			if (tile.TileFrameY < 270)
			{
				if (player.cursorItemIconText == "")
				{
					player.cursorItemIconEnabled = false;
					player.cursorItemIconID = 0;
				}
			}
			else
			{
				player.cursorItemIconID = 0;
				player.cursorItemIconEnabled = false;
			}
		}
        public override bool RightClick(int i, int j)
		{
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			Main.mouseRightRelease = false;
			int key = ModContent.ItemType<RubyKeystone>();
			bool able = false;
			if (!able && tile.TileFrameY < 270)
				able = player.ConsumeItem(key);
			int left = i - (tile.TileFrameX / 18) % 2;
			int top = j - (tile.TileFrameY / 18) % 5;
			if (able)
			{
				SOTSUtils.PlaySound(SoundID.Item4, (int)player.Center.X, (int)player.Center.Y, 1.0f, 0.3f);
				for (int x = left; x < left + 2; x++)
				{
					for (int y = top; y < top + 5; y++)
					{
						if (Main.tile[x, y].TileFrameY < 270)
						{
							Main.tile[x, y].TileFrameY += 90;
							//NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, x, y, 0f, 0, 0, 0);
							NetMessage.SendTileSquare(-1, x, y, 2);
						}
					}
				}
			}
			Vector2 Center = new Vector2(left * 16 + 8, top * 16 + 8);
			bool active = false;
			for (int l = 0; l < Main.projectile.Length; l++)
            {
				Projectile proj = Main.projectile[l];
				if(proj.active && proj.type == ModContent.ProjectileType<AncientGoldGateGems>() && Vector2.Distance(proj.Center, Center) < 16)
                {
					active = true;
                }
            }
			tile = Main.tile[left, top];
			if (tile.TileFrameY >= 270 && tile.TileFrameY < 360 && !active)
			{
				int direction = 1;
				if (tile.TileFrameX >= 36)
					direction = -1;
				for (int h = 1; h <= 2; h++)
					Projectile.NewProjectile(new EntitySource_TileInteraction(player, left, top),new Vector2(left, top) * 16, Vector2.Zero, ModContent.ProjectileType<AncientGoldGateGems>(), 0, 0, Main.myPlayer, direction * h);
			}
			return true;
        }
        public override bool Slope(int i, int j)
        {
            return false;
        }
        public override bool CanExplode(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			return tile.TileFrameY >= 360;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			Tile tile = Main.tile[i, j];
			int left = i - (tile.TileFrameX / 18) % 2;
			int top = j - (tile.TileFrameY / 18) % 5;
			Tile tileUp = Main.tile[left, top - 1];
			Tile tileDown = Main.tile[left, top + 5];
			Tile tileUp2 = Main.tile[left + 1, top - 1];
			Tile tileDown2 = Main.tile[left + 1, top + 5];
			bool surrounded = (tileUp.TileType == ModContent.TileType<TrueSandstoneTile>() && tileDown.TileType == ModContent.TileType<TrueSandstoneTile>()) || (tileUp2.TileType == ModContent.TileType<TrueSandstoneTile>() && tileDown2.TileType == ModContent.TileType<TrueSandstoneTile>());
			return tile.TileFrameY >= 360 && (!surrounded || !NPC.AnyNPCs(ModContent.NPCType<PharaohsCurse>()));
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 3;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Tile tile = Main.tile[i, j];
			if (frameY < 360)
			{
				Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 80, ModContent.ItemType<TaintedKeystoneShard>(), 3);
			}
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 80, ModContent.ItemType<AncientGoldGate>());
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Tile tile = Main.tile[i, j];
			if((tile.TileFrameY == 144 || tile.TileFrameY == 234 || tile.TileFrameY == 216 || tile.TileFrameY == 324 || tile.TileFrameY == 306 || tile.TileFrameY == 288) && (tile.TileFrameX == 0 || tile.TileFrameX == 54))
			{
				r = 0.6f;
				g = 0.05f;
				b = 0.15f;
			}
		}
	}
	public class AncientGoldGateGems : ModProjectile
	{
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
			behindNPCsAndTiles.Add(index);
		}
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gems");
		}
		public override bool PreDraw(ref Color lightColor)
		{
			int i = (int)Projectile.Center.X / 16;
			int j = (int)Projectile.Center.Y / 16;
			DrawGems(i, j, Main.spriteBatch);
			return false;
		}
		public override void SetDefaults()
		{
			Projectile.height = 32;
			Projectile.width = 80;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 720;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 255;
			Projectile.hide = true;
		}
		public override void Kill(int timeLeft)
		{
			int uniqueC = (int)Projectile.ai[0];
			if (Math.Abs(uniqueC) == 1)
            {
				int i = (int)Projectile.Center.X / 16;
				int j = (int)Projectile.Center.Y / 16;
				WorldGen.KillTile(i, j, false, false, false);
				if (!Main.tile[i, j].HasTile && Main.netMode != NetmodeID.SinglePlayer)
					NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, (float)i, (float)j, 0f, 0, 0, 0);
				Vector2 center = Projectile.Center + new Vector2(16, 40);
				SOTSUtils.PlaySound(SoundID.Item14, (int)center.X, (int)center.Y, 1.25f, -0.25f);
				for (int k = 0; k < 12; k++)
				{
					int goreIndex = Gore.NewGore(Projectile.GetSource_FromThis(), center - new Vector2(32, 32) + new Vector2(Main.rand.NextFloat(-16, 16f), Main.rand.NextFloat(-16, 64f)), default(Vector2), Main.rand.Next(61, 64), 1f);
					Main.gore[goreIndex].scale = 0.95f;
				}
				for (int k = -1; k <= 1; k += 2)
				{
					for (j = 0; j < 40 - k * 5; j++)
					{
						Vector2 direction = new Vector2(Main.rand.NextFloat(1, 2) * k * uniqueC, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-45, 45)));
						int num1 = Dust.NewDust(center - new Vector2(12, 12), 16, 16, 198);
						Main.dust[num1].noGravity = true;
						Main.dust[num1].velocity *= 1.15f;
						Main.dust[num1].velocity += direction;
						Main.dust[num1].velocity.X *= 4;
						Main.dust[num1].velocity.Y *= 1.0f;
						Main.dust[num1].scale = 1.5f;

						num1 = Dust.NewDust(center - new Vector2(12, 12), 16, 16, 91);
						Main.dust[num1].noGravity = true;
						Main.dust[num1].velocity *= 0.9f;
						Main.dust[num1].velocity += direction;
						Main.dust[num1].velocity.X *= 3f;
						Main.dust[num1].velocity.Y *= 0.75f;
						Main.dust[num1].scale = 2.25f;

						num1 = Dust.NewDust(center - new Vector2(12, 12), 16, 16, 198);
						Main.dust[num1].noGravity = true;
						Main.dust[num1].velocity *= 0.75f;
						Main.dust[num1].velocity += direction;
						Main.dust[num1].velocity.X *= 2f;
						Main.dust[num1].velocity.Y *= 0.5f;
						Main.dust[num1].scale = 3f;
					}
				}
			}
		}
		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		public override void AI()
		{
			int uniqueC = (int)Projectile.ai[0];
			Projectile.hide = Math.Abs(uniqueC) != 2;
			Projectile.ai[1]++;	
		}
		public void DrawGems(int i, int j, SpriteBatch spriteBatch)
		{
			float counter = Main.GlobalTimeWrappedHourly * 120;
			float mult = new Vector2(-1f, 0).RotatedBy(MathHelper.ToRadians(counter / 2f)).X;
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			float fadeOutMult = Projectile.ai[1] / 720f;
			int ACounter = (int)(255 * fadeOutMult);
			int uniqueC = (int)Projectile.ai[0];
			if (Math.Abs(uniqueC) == 2)
			{
				spriteBatch.Draw(texture, new Vector2(i * 16 - (int)Main.screenPosition.X + 16, j * 16 - (int)Main.screenPosition.Y),
					null, new Color(0, 0, 0, ACounter - 50), 0f, new Vector2(16, 0), 1.0f, uniqueC < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
			else
			{
				for (int k = 0; k < 6; k++)
				{
					Color fColor = new Color(ACounter, ACounter, ACounter, ACounter / 3);
					Color color = new Color(ACounter, ACounter, ACounter, ACounter);
					switch (k)
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
					color = Color.Lerp(color, fColor, fadeOutMult);
					for (int c = 3; c >= 1; c--)
					{
						Rectangle frame = new Rectangle(0, 0, 0, 0);
						if (c == 1)
						{
							frame = new Rectangle(0, 49, 32, 18);
						}
						if (c == 2)
						{
							frame = new Rectangle(0, 31, 32, 18);
						}
						if (c == 3)
						{
							frame = new Rectangle(0, 13, 32, 18);
						}
						Vector2 rotationAround = new Vector2((2 + mult * (1 - fadeOutMult) + 3 * fadeOutMult), 0).RotatedBy(MathHelper.ToRadians(60 * k + counter));
						spriteBatch.Draw(texture, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + frame.Y) + rotationAround,
							frame, color, 0f, default(Vector2), 1.0f, uniqueC < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
					}
				}
			}
		}
	}
}