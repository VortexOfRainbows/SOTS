using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	public class ZeplineLureTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			drop = mod.ItemType("RefractingCrystal");
			AddMapEntry(new Color(120, 90, 90));
			mineResist = 15.5f;
			minPick = 250;
            soundType = 21;
            soundStyle = 2;
			dustType = 32;
		}
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (Main.netMode != 1 && fail && Main.rand.NextBool(15))
				Projectile.NewProjectile(new Vector2(i, j) * 16 + new Vector2(8, 8), Vector2.Zero, mod.ProjectileType("ZeplineLureProjectile"), 0, 0, Main.myPlayer);
			if(!fail && Main.netMode != 2)
			{
				SOTSWorld.SecretFoundMusicTimer = 720;
			}
			base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            base.NumDust(i, j, fail, ref num);
        }
        public override bool CanExplode(int i, int j)
		{
			if (Main.tile[i, j].type == mod.TileType("ZeplineLureTile"))
			{
				return false;
			}
			return false;
		}
		public override bool Slope(int i, int j)
		{
			return false;
		}
	}
	public class ZeplineLureProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zepline Lure");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 50;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Color color = new Color(110, 110, 110, 0);
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				color = projectile.GetAlpha(color) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length) * 0.5f;
				for (int j = 0; j < 5; j++)
				{
					float x = Main.rand.Next(-10, 11) * 0.1f;
					float y = Main.rand.Next(-10, 11) * 0.1f;
					if (!projectile.oldPos[k].Equals(projectile.position))
					{
						Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, projectile.rotation, drawOrigin, (projectile.oldPos.Length - k) / (float)projectile.oldPos.Length, SpriteEffects.None, 0f);
					}
				}
			}
			return false;
		}
		public override void SetDefaults()
		{
			projectile.height = 10;
			projectile.width = 10;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 480;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.extraUpdates = 3;
			projectile.alpha = 255;
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 20; i++)
			{
				int num2 = Dust.NewDust(new Vector2(projectile.position.X - projectile.width, projectile.position.Y - projectile.height) - new Vector2(5), projectile.width * 3, projectile.height * 3, mod.DustType("CopyDust4"));
				Dust dust = Main.dust[num2];
				dust.color = new Color(245, 50, 80, 40);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 1.75f;
				dust.alpha = projectile.alpha;
				dust.velocity += projectile.velocity;
			}
		}
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
		int counter = 0;
		int direction = 0;
		bool started = false;
		public override void AI()
		{
			bool found = false;
			int ofTotal = 0;
			int total = 0;
			int projID = -1;
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile proj = Main.projectile[i];
				if (projectile.type == proj.type && proj.active && projectile.active && proj.owner == projectile.owner)
				{
					if (proj == projectile)
					{
						found = true;
					}
					if (!found)
						ofTotal++;
					total++;
					if (proj.ai[0] == 1 && proj != projectile)
						projID = proj.whoAmI;
				}
			}
			if ((ofTotal == 0 || projectile.ai[0] == 1) && projID == -1 && projectile.ai[0] != -1)
			{
				projectile.ai[0] = 1;
				projectile.alpha = 255;
				projectile.ai[1]++;
				projectile.timeLeft = 7200;
				if (total >= 11 || started)
				{
					started = true;
					int i = (int)projectile.Center.X / 16;
					int j = (int)projectile.Center.Y / 16;
					int range = 2;
					if (counter < 96)
					{
						range = 3;
						projectile.position.Y -= 1;
					}
					else
					{
						if (direction == 0)
                        {
							direction = 1;
							for(int k = 30; k < i; k++)
                            {
								Tile tile = Framing.GetTileSafely(k, j);
								if(tile.active() && tile.type == mod.TileType("OvergrownPyramidTile"))
                                {
									direction = -1;
                                }
                            }
						}
						projectile.position.X += 1 * direction;
						projectile.velocity.X = 5 * direction;
					}
					int count = 0;
					for (int x = -range; x <= range; x++)
					{
						for (int y = -range; y <= range; y++)
						{
							Tile tile = Framing.GetTileSafely(i + x, j + y);
							if (tile.wall == mod.WallType("OvergrownPyramidWallWall"))
							{
								count++;
							}
							if (tile.active() && !Main.tileContainer[tile.type])
							{
								for (int k = 0; k < 3; k++)
								{
									Vector2 pos = new Vector2(i + x, j + y) * 16;
									int num2 = Dust.NewDust(new Vector2(pos.X, pos.Y) - new Vector2(5), 16, 16, mod.DustType("CopyDust4"));
									Dust dust = Main.dust[num2];
									dust.color = new Color(245, 50, 80, 40);
									dust.noGravity = true;
									dust.fadeIn = 0.1f;
									dust.scale *= 1.75f;
									dust.alpha = 0;
									dust.velocity += projectile.velocity;
								}
								WorldGen.KillTile(i + x, j + y, false, false, false);
								if (!Main.tile[i, j].active() && Main.netMode != NetmodeID.SinglePlayer)
									NetMessage.SendData(MessageID.TileChange, -1, -1, null, 0, (float)i, (float)j, 0f, 0, 0, 0);
							}
						}
					}
					if (count >= 25)
						projectile.Kill();
					counter++;
				}
			}
			else
			{
				projectile.timeLeft = 7200;
				projectile.ai[0] = -1;
				if (total > 11 || projID == -1)
					projectile.Kill();
				projectile.alpha = 0;
				if (projID != -1 && total >= 2)
				{
					Projectile proj = Main.projectile[projID];
					if(proj.type != projectile.type || !proj.active)
                    {
						projectile.Kill();
                    }
					Vector2 rotationDist = new Vector2(10 + total * 2, 0).RotatedBy(MathHelper.ToRadians(proj.ai[1] * (3 - total * 0.2f) + (ofTotal % 2) * 90));
					projectile.Center = proj.Center + new Vector2(rotationDist.X, 0).RotatedBy(MathHelper.ToRadians(ofTotal * (360 / (total - 1)) + proj.ai[1]));
					projectile.velocity = proj.velocity;
				}
			}
		}
	}
}