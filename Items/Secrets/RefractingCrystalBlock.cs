using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Pyramid.PyramidWalls;
using SOTS.Dusts;
using SOTS.Items.Pyramid;
using Terraria.DataStructures;

namespace SOTS.Items.Secrets
{
	public class RefractingCrystalBlock : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Weird Crystal Block");
			Tooltip.SetDefault("'You shouldn't have this'");
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Red;
			Item.createTile = ModContent.TileType<RefractingCrystalBlockTile>();
		}
	}
	public class RefractingCrystalBlockTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			ItemDrop = ModContent.ItemType<RefractingCrystal>();
			AddMapEntry(new Color(120, 90, 90));
			MineResist = 15.5f;
			MinPick = 9999;
			HitSound = SoundID.Tink;
			DustType = 32;
		}
		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (Main.netMode != NetmodeID.MultiplayerClient && fail && Main.rand.NextBool(15))
				Projectile.NewProjectile(new EntitySource_Misc("SOTS:RefractingCrystalBlock"), new Vector2(i, j) * 16 + new Vector2(8, 8), Vector2.Zero, ModContent.ProjectileType<RefractingCrystalProj>(), 0, 0, Main.myPlayer);
			if (!fail && Main.netMode != NetmodeID.Server)
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
			return false;
		}
		public override bool Slope(int i, int j)
		{
			return false;
		}
	}
	public class RefractingCrystalProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Refracting Crystal Counter");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Color color = new Color(110, 110, 110, 0);
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				color = Projectile.GetAlpha(color) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * 0.5f;
				for (int j = 0; j < 5; j++)
				{
					float x = Main.rand.Next(-10, 11) * 0.1f;
					float y = Main.rand.Next(-10, 11) * 0.1f;
					if (!Projectile.oldPos[k].Equals(Projectile.position))
					{
						Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, Projectile.rotation, drawOrigin, (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length, SpriteEffects.None, 0f);
					}
				}
			}
			return false;
		}
		public override void SetDefaults()
		{
			Projectile.height = 10;
			Projectile.width = 10;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 480;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.extraUpdates = 3;
			Projectile.alpha = 255;
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 20; i++)
			{
				int num2 = Dust.NewDust(new Vector2(Projectile.position.X - Projectile.width, Projectile.position.Y - Projectile.height) - new Vector2(5), Projectile.width * 3, Projectile.height * 3, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[num2];
				dust.color = new Color(245, 50, 80, 40);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 1.75f;
				dust.alpha = Projectile.alpha;
				dust.velocity += Projectile.velocity;
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
				if (Projectile.type == proj.type && proj.active && Projectile.active && proj.owner == Projectile.owner)
				{
					if (proj == Projectile)
					{
						found = true;
					}
					if (!found)
						ofTotal++;
					total++;
					if (proj.ai[0] == 1 && proj != Projectile)
						projID = proj.whoAmI;
				}
			}
			if ((ofTotal == 0 || Projectile.ai[0] == 1) && projID == -1 && Projectile.ai[0] != -1)
			{
				Projectile.ai[0] = 1;
				Projectile.alpha = 255;
				Projectile.ai[1]++;
				Projectile.timeLeft = 7200;
				if (total >= 11 || started)
				{
					started = true;
					int i = (int)Projectile.Center.X / 16;
					int j = (int)Projectile.Center.Y / 16;
					int range = 2;
					if (counter < 96)
					{
						range = 3;
						Projectile.position.Y -= 1;
					}
					else
					{
						if (direction == 0)
						{
							direction = 1;
							for (int k = 30; k < i; k++)
							{
								Tile tile = Framing.GetTileSafely(k, j);
								if (tile.HasTile && tile.TileType == ModContent.TileType<OvergrownPyramidTile>())
								{
									direction = -1;
								}
							}
						}
						Projectile.position.X += 1 * direction;
						Projectile.velocity.X = 5 * direction;
					}
					int count = 0;
					for (int x = -range; x <= range; x++)
					{
						for (int y = -range; y <= range; y++)
						{
							Tile tile = Framing.GetTileSafely(i + x, j + y);
							if (tile.WallType == ModContent.WallType<UnsafeOvergrownPyramidWallWall>())
							{
								count++;
							}
							if (tile.HasTile && !Main.tileContainer[tile.TileType])
							{
								for (int k = 0; k < 3; k++)
								{
									Vector2 pos = new Vector2(i + x, j + y) * 16;
									int num2 = Dust.NewDust(new Vector2(pos.X, pos.Y) - new Vector2(5), 16, 16, ModContent.DustType<CopyDust4>());
									Dust dust = Main.dust[num2];
									dust.color = new Color(245, 50, 80, 40);
									dust.noGravity = true;
									dust.fadeIn = 0.1f;
									dust.scale *= 1.75f;
									dust.alpha = 0;
									dust.velocity += Projectile.velocity;
								}
								WorldGen.KillTile(i + x, j + y, false, false, false);
								if (!Main.tile[i, j].HasTile && Main.netMode != NetmodeID.SinglePlayer)
									NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, (float)i, (float)j, 0f, 0, 0, 0);
							}
						}
					}
					if (count >= 25)
						Projectile.Kill();
					counter++;
				}
			}
			else
			{
				Projectile.timeLeft = 7200;
				Projectile.ai[0] = -1;
				if (total > 11 || projID == -1)
					Projectile.Kill();
				Projectile.alpha = 0;
				if (projID != -1 && total >= 2)
				{
					Projectile proj = Main.projectile[projID];
					if (proj.type != Projectile.type || !proj.active)
					{
						Projectile.Kill();
					}
					Vector2 rotationDist = new Vector2(10 + total * 2, 0).RotatedBy(MathHelper.ToRadians(proj.ai[1] * (3 - total * 0.2f) + (ofTotal % 2) * 90));
					Projectile.Center = proj.Center + new Vector2(rotationDist.X, 0).RotatedBy(MathHelper.ToRadians(ofTotal * (360 / (total - 1)) + proj.ai[1]));
					Projectile.velocity = proj.velocity;
				}
			}
		}
	}
}