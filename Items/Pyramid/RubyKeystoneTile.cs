using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Pyramid
{
	public class RubyKeystoneTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
			Main.tileWaterDeath[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style5x4);
			TileObjectData.newTile.Height = 5;
			TileObjectData.newTile.Width = 5;
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16 };
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 5, 0); 
			TileObjectData.newTile.Origin = new Point16(2, 4);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Strange Keystone");
			AddMapEntry(new Color(115, 0, 0), name);
			disableSmartCursor = true;
			dustType = ModContent.DustType<CurseDust3>();
			soundType = SoundID.NPCHit;
			soundStyle = 1;
		}
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
			DrawGems(i, j, spriteBatch);
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
			float counter = Main.GlobalTime * 120;
			float mult = new Vector2(-1f, 0).RotatedBy(MathHelper.ToRadians(counter / 2f)).X;
			Texture2D texture = mod.GetTexture("Items/Pyramid/RubyKeystoneTileGlow");
			if (tile.frameY % 90 == 0 && tile.frameX % 90 == 0) //check for it being the top left tile
			{
				int currentFrame = tile.frameY / 90;
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
					Rectangle frame = new Rectangle(0, currentFrame * 80, 80, 80);
					Vector2 rotationAround = new Vector2((2 + mult), 0).RotatedBy(MathHelper.ToRadians(60 * k + counter));
					spriteBatch.Draw(texture, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 1) + zero + rotationAround,
						frame, color, 0f, default(Vector2), 1.0f, tile.frameX > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				}
			}
		}
		public override void RandomUpdate(int i, int j)
		{
			if (Main.netMode != 1)
			{
				Tile tile = Main.tile[i, j];
				int left = i - (tile.frameX / 18) % 5;
				int top = j - (tile.frameY / 18) % 5;
				for (int x = left; x < left + 5; x++)
				{
					for (int y = top; y < top + 5; y++)
					{
						if (Main.tile[x, y].frameY < 360)
							Main.tile[x, y].frameY += 90;
					}
				}
				NetMessage.SendTileSquare(-1, left + 2, top + 2, 5);
			}
			base.RandomUpdate(i, j);
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			Tile tile = Main.tile[i, j];
			int left = i - (tile.frameX / 18) % 5;
			int top = j - (tile.frameY / 18) % 5;
			left += 2;
			top += 2;
			if (Main.netMode != NetmodeID.MultiplayerClient && fail && Main.tile[left, top].frameY >= 360)
			{
				bool active = false;
				for (int l = 0; l < Main.projectile.Length; l++)
				{
					Projectile proj = Main.projectile[l];
					if (proj.active && proj.type == ModContent.ProjectileType<RubyKeystoneIndicator>() && Vector2.Distance(proj.Center, new Vector2(left, top) * 16 + new Vector2(8, 8)) < 16)
					{
						active = true;
					}
				}
				Projectile.NewProjectile(new Vector2(left, top) * 16 + new Vector2(8, 8), Vector2.Zero, ModContent.ProjectileType<RubyKeystoneIndicator>(), 0, 0, Main.myPlayer);
				if (!active)
					Projectile.NewProjectile(new Vector2(left, top) * 16 + new Vector2(8, 8), Vector2.Zero, ModContent.ProjectileType<RubyKeystoneIndicator>(), 0, 0, Main.myPlayer);
			}
		}
		public override bool CanExplode(int i, int j)
        {
            return false;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			return false;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 2;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int drop = ModContent.ItemType<RubyKeystone>();
			Item.NewItem(i * 16, j * 16, 80, 80, drop);
		}
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			int type = Main.tile[i, j].frameX / 18 + (Main.tile[i, j].frameY % 90 / 18 * 5);
			if (type != 12)
				return;
			r = 1.1f;
			g = 0.1f;
			b = 0.3f;
		}
	}
	public class RubyKeystoneIndicator : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ruby Energy");
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
				dust.color = new Color(255, 10, 30, 40);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 1.75f;
				dust.alpha = projectile.alpha;
				dust.velocity += projectile.velocity;
			}
		}
		public override bool ShouldUpdatePosition()
		{
			return true;
		}
		int projID = -1;
		bool runOnce = true;
		public override void AI()
		{
			if (runOnce)
			{
				bool foundLeader = false;
				for (int k = 0; k < Main.projectile.Length; k++)
				{
					Projectile proj = Main.projectile[k];
					if (projectile.type == proj.type && proj.active && projectile.active && Vector2.Distance(proj.Center, projectile.Center) <= 64f)
					{
						if ((int)proj.ai[0] == 1 && proj != projectile)
                        {
							foundLeader = true;
							projID = proj.whoAmI;
						}
					}
				}
				if(!foundLeader) //if there is no leader nearby, designate leader
                {
					projectile.ai[0] = 1; //designating leader
                }
				runOnce = false;
            }
			int leaderID = projID;
			if(projID == -1 || projectile.ai[0] == 1)
            {
				leaderID = projectile.whoAmI;
            }
			Projectile leader = Main.projectile[leaderID];
			int i = (int)leader.Center.X / 16;
			int j = (int)leader.Center.Y / 16;
			Tile current = Framing.GetTileSafely(i, j);
			if (!current.active() || current.type != ModContent.TileType<RubyKeystoneTile>() || current.frameY < 360 || !leader.active) //making sure the projectile can exist based on leader tile position
			{
				projectile.Kill();
				projectile.active = false;
			}
			bool found = false;
			int ofTotal = 0;
			int total = 0;
			for (int k = 0; k < Main.projectile.Length; k++)
			{
				Projectile proj = Main.projectile[k];
				if (projectile.type == proj.type && proj.active && projectile.active && Vector2.Distance(proj.Center, leader.Center) <= 64f) //if close to leader
				{
					if (proj == projectile)
					{
						found = true;
					}
					if (!found)
						ofTotal++;
					if(proj.ai[0] != 1) //if not leader
						total++;
				}
			}
			projectile.timeLeft = 7200;
			if (leader == projectile) //if this projectile is the leader
			{
				projectile.ai[0] = 1;
				projectile.alpha = 255;
				projectile.ai[1]++;
				if (total >= 10)
				{
					int range = 2;
					for (int x = -range; x <= range; x++)
					{
						for (int y = -range; y <= range; y++)
						{
							if (Main.netMode != 1)
							{
								if (Main.tile[i + x, j + y].frameY >= 360)
									Main.tile[i + x, j + y].frameY = (short)(Main.tile[i + x, j + y].frameY % 90);
								if(Main.netMode == 2)
								NetMessage.SendTileSquare(-1, i, j, 5);
							}
						}
					}
					projectile.Kill();
					projectile.active = false;
				}
			}
			else
			{
				projectile.alpha = 0;
				projectile.ai[0] = -1;
				if (projID != -1 && total >= 1)
				{
					float rotationDist = 36f;
					Vector2 goTo = leader.Center + new Vector2(rotationDist, 0).RotatedBy(MathHelper.ToRadians(ofTotal * (360f / total) + leader.ai[1] * 0.5f)) - projectile.Center;
					float length = goTo.Length();
					float speed = 4;
					if (speed > length)
					{
						speed = length;
					}
					projectile.velocity = goTo.SafeNormalize(Vector2.Zero) * speed;
				}
			}
		}
	}
}