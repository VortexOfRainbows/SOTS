using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;

namespace SOTS.Items.Secrets
{
	public class RefractingCrystal : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Refracting Crystal");
			Tooltip.SetDefault("");
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frameNotUsed, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Secrets/RefractingCrystal").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			position += new Vector2(11 * scale, 9 * scale);
			float counter = Main.GlobalTimeWrappedHourly * 160;
			//int bonus = (int)(counter / 360f);
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
						color = new Color(255, 140, 0, 0);
						break;
					case 2:
						color = new Color(255, 255, 0, 0);
						break;
					case 3:
						color = new Color(0, 255, 0, 0);
						break;
					case 4:
						color = new Color(0, 0, 255, 0);
						break;
					case 5:
						color = new Color(140, 0, 255, 0);
						break;
				}
				Rectangle frame = new Rectangle(0, 0, 20, 18);
				Vector2 rotationAround = new Vector2((4 + mult) * scale, 0).RotatedBy(MathHelper.ToRadians(60 * i + counter));
				Main.spriteBatch.Draw(texture, new Vector2((float)(position.X), (float)(position.Y)) + rotationAround, frame, color * 1f, 0f, drawOrigin, scale * 1.1f, SpriteEffects.None, 0f);
			}
			return true;
		}
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
			int i = (int)Item.Center.X / 16;
			int j = (int)Item.Center.Y / 16;
			Tile tile = Framing.GetTileSafely(i, j);
			if (tile.WallType > 0 || j > Main.rockLayer)
            {
				return true;
            }
			bool day = Main.dayTime;
			if(Main.raining || !day)
            {
				return true;
			}
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Secrets/RefractingCrystal").Value;
			float time = (float)Main.time;
			float maxTime = 54000f;
			float percent = (float)time / (float)maxTime;
			float lightIntesity = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(180f * percent)).Y;
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Secrets/WhitePixel").Value;
			Vector2 drawOrigin;
			float counter = time * 0.024f;
			float mult = new Vector2(-11.8f, 0).RotatedBy(MathHelper.ToRadians(180f * percent)).Y;
			for (i = 0; i < 6; i++)
			{
				drawOrigin = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
				Color color = new Color(255, 0, 0, 0);
				switch (i)
				{
					case 0:
						color = new Color(255, 0, 0, 0);
						break;
					case 1:
						color = new Color(255, 140, 0, 0);
						break;
					case 2:
						color = new Color(255, 255, 0, 0);
						break;
					case 3:
						color = new Color(0, 255, 0, 0);
						break;
					case 4:
						color = new Color(0, 0, 255, 0);
						break;
					case 5:
						color = new Color(140, 0, 255, 0);
						break;
				}
				Vector2 rotateAroundArea = new Vector2(0, 2) + (new Vector2(6f, 0).RotatedBy(MathHelper.ToRadians(180 * percent)));
				Vector2 rotationAround = rotateAroundArea + new Vector2((12 + mult) * scale, 0).RotatedBy(MathHelper.ToRadians(60 * i + counter));
				Vector2 rotationAround2 = 0.5f * new Vector2((12 + mult * 0.5f) * scale, 0).RotatedBy(MathHelper.ToRadians(60 * i + counter));
				float rotation2 = rotationAround.ToRotation() - MathHelper.ToRadians(90);
				float dist = 2;
				float scale2 = 1 + 0.5f * lightIntesity;
				Main.spriteBatch.Draw(texture2, rotationAround2 + Item.Center - Main.screenPosition + new Vector2(0, 2), null, color * lightIntesity, rotation, drawOrigin, scale * 1.1f, SpriteEffects.None, 0f);
				for (int k = 0; k < 51; k++)
				{
					scale2 += 0.1f + 0.01f * lightIntesity;
					dist += 2;
					Vector2 fromCenter = Item.Center + new Vector2(0, dist * scale).RotatedBy(rotation2);
					int width = (int)(2 * scale2);
					int height = 2;
					drawOrigin = new Vector2(width * 0.5f, 0);
					Main.spriteBatch.Draw(texture, fromCenter - Main.screenPosition, new Rectangle(0, 0, width, height), color * lightIntesity * ((51f - k) / 51f), rotation + rotation2, drawOrigin, 1, SpriteEffects.None, 0f);
				}
			}

			return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
        }
		int counter = 0;
        public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			int i = (int)Item.Center.X / 16;
			int j = (int)Item.Center.Y / 16;
			j += 4;
			Tile tile = Framing.GetTileSafely(i, j);
			bool day = Main.dayTime;
			if (tile.WallType > 0 || j > Main.rockLayer || Main.raining || !day)
			{
				return;
			}
			float time = (float)Main.time;
			float midDay = 27000f;
			if((time > midDay - 750 && time < midDay + 750) || counter > 0)
            {
				if(tile.TileType == ModContent.TileType<StrangeKeystoneTile>())
                {
					int frameX = tile.TileFrameX / 16;
					int frameY = tile.TileFrameY / 16;
					if (frameX <= 2)
					{
						counter++;
						int i2 = i - frameX;
						int j2 = j - frameY;
						for (int k = 0; k < 3; k++)
						{
							for (int h = 0; h < 4; h++)
							{
								i2 = i - frameX + k;
								j2 = j - frameY + h;
								tile = Framing.GetTileSafely(i2, j2);
								if(counter >= 300)
								{
									if (tile.TileType == ModContent.TileType<StrangeKeystoneTile>())
									{
										if(h == 2 && k == 1)
										{
											Vector2 middlePos = new Vector2(i2 * 16 + 8, j2 * 16 + 8);
											if (Main.netMode != 1)
											{
												Projectile.NewProjectile(Item.GetSource_Misc("SOTS:StrangeKeystoneRefractingCrystalInteraction"), middlePos, Vector2.Zero, ModContent.ProjectileType<StrangeKeystoneExplosion>(), 0, 0, Main.myPlayer);
												int item = Item.NewItem(Item.GetSource_Misc("SOTS:StrangeKeystoneRefractingCrystalInteraction"), (int)middlePos.X, (int)middlePos.Y, 0, 0, ModContent.ItemType<PhotonGeyser>(), 1);
												NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f, 0.0f, 0.0f, 0, 0, 0);
											}
										}
										tile.TileFrameX += 54;
										if (Main.netMode == 2)
										{
											NetMessage.SendTileSquare(-1, i2, j2, 5, TileChangeType.None);
										}
									}
								}
								else
								{
									if(Main.rand.Next(450) <= counter)
									{
										WorldGen.KillTile(i2, j2, true, true, false);
										if(Main.rand.NextBool(2))
											Terraria.Audio.SoundEngine.PlaySound(SoundID.Tink, new Vector2(i2 * 16 + 8, j2 * 16 + 8));
									}
								}
							}
						}
					}
					else
					{
						counter = 0;
					}
				}
				else
				{
					counter = 0;
				}
			}
		}
        public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 18;
			Item.maxStack = 999;
			Item.value = Item.sellPrice(1, 0, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
		}
	}
	public class StrangeKeystoneExplosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Strange Keystone Explosion");
		}
		public override void SetDefaults()
		{
			Projectile.timeLeft = 5;
			Projectile.penetrate = -1;
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = false;
			Projectile.friendly = false;
			Projectile.hostile = false;
		}
		bool runOnce = true;
		public override void AI()
		{
			if (runOnce)
			{
				runOnce = false;
				if (Main.netMode != 2)
				{
					SOTSWorld.SecretFoundMusicTimer = 720;
				}
				SOTSUtils.PlaySound(SoundID.Item14, Projectile.Center);
				int cc = 0;
				for (int l = 0; l < 360; l += 3)
				{
					int type = cc % 7;
					Color color = Color.White;
					cc += 1 + Main.rand.Next(2);
					switch (type)
					{
						case 0:
							color = new Color(255, 0, 0, 0);
							break;
						case 1:
							color = new Color(255, 140, 0, 0);
							break;
						case 2:
							color = new Color(255, 255, 0, 0);
							break;
						case 3:
							color = new Color(0, 255, 0, 0);
							break;
						case 4:
							color = new Color(0, 255, 255, 0);
							break;
						case 5:
							color = new Color(0, 0, 255, 0);
							break;
						case 6:
							color = new Color(140, 0, 255, 0);
							break;
					}
					Vector2 circular = new Vector2(16, 0).RotatedBy(MathHelper.ToRadians(l));
					for (int u = 1; u < 3; u++)
					{
						Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5) + circular, 0, 0, ModContent.DustType<CopyDust4>());
						dust.fadeIn = 0.2f;
						dust.noGravity = true;
						dust.alpha = 50;
						dust.color = color;
						dust.scale *= 1.75f * u;
						dust.velocity *= 0.5f;
						dust.velocity += circular * 0.4f / u;
					}
				}
			}
		}
	}
}