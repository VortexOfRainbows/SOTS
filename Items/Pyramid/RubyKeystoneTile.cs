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
		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			int left = i - Main.tile[i, j].frameX / 18; 
			int top = j - Main.tile[i, j].frameY / 18;
			left += 2;
			top += 2;
			if (Main.netMode != NetmodeID.MultiplayerClient && fail)
				Projectile.NewProjectile(new Vector2(left, top) * 16 + new Vector2(8, 8), Vector2.Zero, ModContent.ProjectileType<RubyKeystoneIndicator>(), 0, 0, Main.myPlayer);
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
			int type = Main.tile[i, j].frameX / 18 + (Main.tile[i, j].frameY / 18 * 5);
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
					if (projectile.type == proj.type && proj.active && projectile.active)
					{
						if (proj.ai[0] == 1 && Vector2.Distance(proj.Center, projectile.Center) <= 42f && proj != projectile)
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
			if (!current.active() || current.type != ModContent.TileType<RubyKeystoneTile>() || current.frameY > 72 || !leader.active) //making sure the projectile can exist based on leader tile position
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
				if (projectile.type == proj.type && proj.active && projectile.active && Vector2.Distance(proj.Center, leader.Center) <= 42f) //if close to leader
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
							Tile tile = Framing.GetTileSafely(i + x, j + y);
							if (tile.active() && !Main.tileContainer[tile.type])
							{
								WorldGen.KillTile(i + x, j + y, false, false, false);
								if (!Main.tile[i, j].active() && Main.netMode != NetmodeID.SinglePlayer)
									NetMessage.SendData(MessageID.TileChange, -1, -1, null, 0, (float)i, (float)j, 0f, 0, 0, 0);
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