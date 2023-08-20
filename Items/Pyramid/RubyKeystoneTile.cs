using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Dusts;
using SOTS.Projectiles.Pyramid;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Pyramid
{
	public class RubyKeystoneTile : ModTile
	{
		public override void SetStaticDefaults()
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
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(115, 0, 0), name);
			TileID.Sets.DisableSmartCursor[Type] = true;
			DustType = ModContent.DustType<CurseDust3>();
			HitSound = SoundID.NPCHit1;
		}
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
			DrawBack(i, j, spriteBatch);
			DrawGems(i, j, spriteBatch);
            return true;
		}
		public void DrawBack(int i, int j, SpriteBatch spriteBatch)
		{
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Tile tile = Main.tile[i, j];
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Pyramid/RubyKeystoneTileBack").Value;
			if (tile.TileFrameY % 90 == 0 && tile.TileFrameX % 90 == 0) //check for it being the top left tile
			{
				int currentFrame = tile.TileFrameY / 90;
				Rectangle frame = new Rectangle(0, currentFrame * 80, 80, 80);
				spriteBatch.Draw(texture, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero,
					frame, Lighting.GetColor(i, j), 0f, default(Vector2), 1.0f, tile.TileFrameX > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
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
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Pyramid/RubyKeystoneTileGlow").Value;
			if (tile.TileFrameY % 90 == 0 && tile.TileFrameX % 90 == 0) //check for it being the top left tile
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
					Rectangle frame = new Rectangle(0, currentFrame * 80, 80, 80);
					Vector2 rotationAround = new Vector2((2 + mult), 0).RotatedBy(MathHelper.ToRadians(60 * k + counter));
					spriteBatch.Draw(texture, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 1) + zero + rotationAround,
						frame, color, 0f, default(Vector2), 1.0f, tile.TileFrameX > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				}
			}
		}
		public override void RandomUpdate(int i, int j)
		{
			if (Main.netMode != 1 && Main.rand.NextBool(20))
			{
				Tile tile = Main.tile[i, j];
				int left = i - (tile.TileFrameX / 18) % 5;
				int top = j - (tile.TileFrameY / 18) % 5;
				for (int x = left; x < left + 5; x++)
				{
					for (int y = top; y < top + 5; y++)
					{
						if (Main.tile[x, y].TileFrameY < 360)
							Main.tile[x, y].TileFrameY += 90;
					}
				}
				NetMessage.SendTileSquare(-1, left + 2, top + 2, 5);
			}
			base.RandomUpdate(i, j);
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			Tile tile = Main.tile[i, j];
			int left = i - (tile.TileFrameX / 18) % 5;
			int top = j - (tile.TileFrameY / 18) % 5;
			left += 2;
			top += 2;
			if(Main.tile[left, top].TileFrameY >= 360)
			{
				Main.LocalPlayer.AddBuff(ModContent.BuffType<CreativeShock2>(), 480);
				if (Main.netMode != NetmodeID.MultiplayerClient && fail)
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
					Projectile.NewProjectile(new EntitySource_Misc("SOTS:RubyKeystoneWaves"), new Vector2(left, top) * 16 + new Vector2(8, 8), Vector2.Zero, ModContent.ProjectileType<RubyKeystoneIndicator>(), 0, 0, Main.myPlayer);
					if (!active)
						Projectile.NewProjectile(new EntitySource_Misc("SOTS:RubyKeystoneWaves"), new Vector2(left, top) * 16 + new Vector2(8, 8), Vector2.Zero, ModContent.ProjectileType<RubyKeystoneIndicator>(), 0, 0, Main.myPlayer);
				}
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
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<RubyKeystone>(), 1);
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			int type = Main.tile[i, j].TileFrameX / 18 + (Main.tile[i, j].TileFrameY % 90 / 18 * 5);
			if (type != 12 || Main.tile[i, j].TileFrameY < 90)
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
			// DisplayName.SetDefault("Ruby Energy");
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
			Projectile.netImportant = true;
			Projectile.hostile = false;
			Projectile.extraUpdates = 3;
			Projectile.alpha = 255;
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 20; i++)
			{
				int num2 = Dust.NewDust(new Vector2(Projectile.position.X - Projectile.width, Projectile.position.Y - Projectile.height) - new Vector2(5), Projectile.width * 3, Projectile.height * 3, ModContent.DustType<Dusts.CopyDust4>());
				Dust dust = Main.dust[num2];
				dust.color = new Color(255, 10, 30, 40);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 1.75f;
				dust.alpha = Projectile.alpha;
				dust.velocity += Projectile.velocity;
			}
		}
		public override bool ShouldUpdatePosition()
		{
			return true;
		}
		int projID = -1;
		bool runOnce = true;
		bool runOnce2 = true;
		public void MusicCheck()
        {
			Player player = Main.LocalPlayer;
			if(player.Distance(Projectile.Center) <= 480)
            {
				SOTSPlayer.pyramidBattle = true;
            }
        }
		public override void AI()
		{
			MusicCheck();
			if (runOnce)
			{
				bool foundLeader = false;
				for (int k = 0; k < Main.projectile.Length; k++)
				{
					Projectile proj = Main.projectile[k];
					if (Projectile.type == proj.type && proj.active && Projectile.active && Vector2.Distance(proj.Center, Projectile.Center) <= 64f)
					{
						if ((int)proj.ai[0] == 1 && proj != Projectile)
                        {
							foundLeader = true;
							projID = proj.whoAmI;
							break;
						}
					}
				}
				if(!foundLeader) //if there is no leader nearby, designate leader
                {
					Projectile.ai[0] = 1; //designating leader
                }
				runOnce = false;
            }
			int leaderID = projID;
			if(projID == -1 || Projectile.ai[0] == 1)
            {
				leaderID = Projectile.whoAmI;
            }
			Projectile leader = Main.projectile[leaderID];
			int i = (int)leader.Center.X / 16;
			int j = (int)leader.Center.Y / 16;
			Tile current = Framing.GetTileSafely(i, j);
			if (!current.HasTile || current.TileType != ModContent.TileType<RubyKeystoneTile>() || current.TileFrameY < 360 || !leader.active) //making sure the projectile can exist based on leader tile position
			{
				Projectile.Kill();
				Projectile.active = false;
			}
			bool found = false;
			int ofTotal = 0;
			int total = 0;
			for (int k = 0; k < Main.projectile.Length; k++)
			{
				Projectile proj = Main.projectile[k];
				if (Projectile.type == proj.type && proj.active && Projectile.active && Vector2.Distance(proj.Center, leader.Center) <= 64f) //if close to leader
				{
					if (proj == Projectile)
					{
						found = true;
					}
					if (!found)
						ofTotal++;
					if(proj.ai[0] != 1) //if not leader
						total++;
				}
			}
			if (Projectile.ai[0] != 1) //if not leader
				if (Main.netMode != NetmodeID.MultiplayerClient && runOnce2)
				{
					int totalSpawns = total;
					if (!Main.expertMode)
						totalSpawns--;
					runOnce2 = false;
					for(int amt = totalSpawns; amt >= 0; amt--)
					{
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(i, j) * 16 + new Vector2(8, 8), Vector2.Zero, ModContent.ProjectileType<RubySpawnerFinder>(), 0, 0, Main.myPlayer);
					}
				}
			Projectile.timeLeft = 7200;
			if (leader == Projectile) //if this projectile is the leader
			{
				Projectile.ai[0] = 1;
				Projectile.alpha = 255;
				Projectile.ai[1]++;
				if (total >= 6)
				{
					int range = 2;
					if (Main.netMode != 1)
					{
						for (int x = -range; x <= range; x++)
						{
							for (int y = -range; y <= range; y++)
							{
								if (Main.tile[i + x, j + y].TileFrameY >= 360)
									Main.tile[i + x, j + y].TileFrameY = (short)(Main.tile[i + x, j + y].TileFrameY % 90);
								if (Main.netMode == 2)
									NetMessage.SendTileSquare(-1, i, j, 5);

							}
						}
						int item = Item.NewItem(Projectile.GetSource_FromThis(), (int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height, ModContent.ItemType<RubyKeystone>(), 1, false, 0, true);
						NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f, 0.0f, 0.0f, 0, 0, 0);
					}
					SOTSUtils.PlaySound(SoundID.Shatter, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.10f, -0.1f);
					Projectile.Kill();
					Projectile.active = false;
				}
			}
			else
			{
				Projectile.alpha = 0;
				Projectile.ai[0] = -1;
				if (projID != -1 && total >= 1)
				{
					float rotationDist = 36f;
					Vector2 goTo = leader.Center + new Vector2(rotationDist, 0).RotatedBy(MathHelper.ToRadians(ofTotal * (360f / total) + leader.ai[1] * 0.5f)) - Projectile.Center;
					float length = goTo.Length();
					float speed = 4;
					if (speed > length)
					{
						speed = length;
					}
					Projectile.velocity = goTo.SafeNormalize(Vector2.Zero) * speed;
				}
			}
		}
	}
}