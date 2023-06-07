using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.NPCs.Boss;
using SOTS.Projectiles.Blades;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Furniture.Functional
{
	public class HydraulicPress : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.width = 48;
			Item.height = 58;
			Item.rare = ItemRarityID.Orange;
			Item.createTile = ModContent.TileType<HydraulicPressTile>();
		}
	}	
	public class HydraulicPressTile : ModTile
	{
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Texture2D texture = SOTSTile.GetTileDrawTexture(i, j); //hopefully should get paint properly
			Color color = Lighting.GetColor(i, j, Color.White);
			Vector2 pos = new Vector2(i * 16, j * 16) - Main.screenPosition + zero;
			spriteBatch.Draw(texture, pos, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, (tile.TileFrameY / 18 == 7) ? 18 : 16), color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			return false;
		}
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileObsidianKill[Type] = false;
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
			Main.tileWaterDeath[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.Width = 6;
			TileObjectData.newTile.Height = 8;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16, 16, 16, 18 };
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 6, 0);
			TileObjectData.newTile.Origin = new Point16(3, 7);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			AddMapEntry(SOTSTile.EarthenPlatingColor, name);
			DustType = DustID.Iron;
			MineResist = 0.1f;
		}
        public override void NearbyEffects(int i, int j, bool closer)
        {
			if(closer)
            {
				Tile tile = Main.tile[i, j];
				int left = i - (tile.TileFrameX % 108) / 18;
				int top = j - (tile.TileFrameY % 144) / 18;
				if ((tile.TileFrameX == 0 || tile.TileFrameX == 6 * 18) && tile.TileFrameY == 0)
                {
					for(int k = 0; k < 6; k++)
                    {
						for(int h = 0; h < 8; h++)
                        {
							int trueI = left + k;
							int trueJ = top + h;
							tile = Main.tile[trueI, trueJ];
							if(h >= 4 && h <= 6)
                            {
								if(!tile.IsActuated)
                                {
									tile.IsActuated = true;
									NetMessage.SendTileSquare(Main.myPlayer, trueI, trueJ, 1);
								}
							}
							else if(tile.IsActuated)
                            {
								tile.IsActuated = false;
								NetMessage.SendTileSquare(Main.myPlayer, trueI, trueJ, 1);
							}
                        }
                    }
                }
			}
        }
		public static void CheckIfInsideHydraulic(NPC npc)
		{
			if (npc.friendly || !npc.active)
				return;
			int i = (int)npc.Center.X / 16;
			int j = (int)npc.Center.Y / 16;
			Tile tile = Main.tile[i, j];
			if(tile.TileType == ModContent.TileType<HydraulicPressTile>() && tile.HasTile)
            {
				if(tile.IsActuated)
                {
					LaunchHydraulic(npc);
                }
            }
		}
		public static void LaunchHydraulic(NPC npc)
		{
			int i = (int)npc.Center.X / 16;
			int j = (int)npc.Center.Y / 16;
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameX < 108)
			{
				int left = i - (tile.TileFrameX % 108) / 18;
				int top = j - (tile.TileFrameY % 144) / 18;
				for (int k = 0; k < 6; k++)
				{
					for (int h = 0; h < 8; h++)
					{
						int trueI = left + k;
						int trueJ = top + h;
						tile = Main.tile[trueI, trueJ];
						tile.TileFrameX += 108;
					}
				}
				NetMessage.SendTileSquare(Main.myPlayer, left + 3, top + 4, 9);
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Vector2 center = new Vector2(left * 16 + 48, top * 16 + 32);
					Projectile.NewProjectile(new EntitySource_TileInteraction(npc, i, j, "SOTS:Hydraulic"), center, Vector2.Zero, ModContent.ProjectileType<PressProjectile>(), 200, 0, Main.myPlayer);
					//spawn hydraulic projectile here
				}
			}
		}
		public static void ReturnHydraulic(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameX >= 108)
			{
				int left = i - (tile.TileFrameX % 108) / 18;
				int top = j - (tile.TileFrameY % 144) / 18;
				for (int k = 0; k < 6; k++)
				{
					for (int h = 0; h < 8; h++)
					{
						int trueI = left + k;
						int trueJ = top + h;
						tile = Main.tile[trueI, trueJ];
						tile.TileFrameX -= 108;
					}
				}
				NetMessage.SendTileSquare(Main.myPlayer, left + 3, top + 4, 9);
			}
		}
        public override bool CanExplode(int i, int j)
        {
            return true;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			return true;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 2;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16 + 16, j * 16 + 16, 64, 96, ModContent.ItemType<HydraulicPress>());
		}
    }
	public class PressProjectile : ModProjectile
    {
		public override void SetDefaults()
		{
			Projectile.DamageType = DamageClass.Generic;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.width = 88;
			Projectile.height = 64;
			Projectile.timeLeft = 7200;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.hide = false;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
		}
        public override bool PreDraw(ref Color lightColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
			Vector2 origin = new Vector2(texture.Width / 2, 28);
			int height = 10 + (int)(48 - totalDistanceToTravel);
			if (totalDistanceToTravel < 0)
				height = 10 - (int)totalDistanceToTravel;
			if (totalDistanceToTravel == 0)
				height = 10;
			Rectangle showPixels = new Rectangle(0, 0, texture.Width, height);
			Main.spriteBatch.Draw(texture, new Vector2((int)Projectile.Center.X, (int)(Projectile.Center.Y + 2f + (58 - height))) - Main.screenPosition, showPixels, lightColor, 0f, origin, 1f, SpriteEffects.FlipVertically, 0f);
			return false;
        }
        float totalDistanceToTravel = 48f;
        public override void AI()
		{
			int i = (int)Projectile.Center.X / 16;
			int j = (int)Projectile.Center.Y / 16;
			Tile tile = Main.tile[i, j];
			if(!tile.HasTile || tile.TileType != ModContent.TileType<HydraulicPressTile>())
            {
				Projectile.Kill();
				return;
            }
			if(totalDistanceToTravel > 0)
            {
				float speed = Projectile.ai[0] / 1.75f;
				if (speed > totalDistanceToTravel)
					speed = totalDistanceToTravel;
				totalDistanceToTravel -= speed;
				Projectile.position.Y += speed;
				if(totalDistanceToTravel <= 0)
                {
					Projectile.friendly = true;
					Projectile.hostile = true;
					totalDistanceToTravel = -48;
					SOTSUtils.PlaySound(SoundID.Item53, Projectile.Center, 0.9f, -0.5f);
					Color color = ColorHelpers.EarthColor;
					color.A = 0;
					for (int l = -10; l <= 10; l++)
					{
						for(int z = 0; z <= 1; z++)
						{
							Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - Projectile.width / 4 + l, Projectile.position.Y + Projectile.height - 2) - new Vector2(5, 5), Projectile.width / 2, 4, ModContent.DustType<PixelDust>(),
								0, -z * Main.rand.NextFloat(0.4f, 2.5f), 0, color, Main.rand.NextFloat(0.8f, 1.2f));
							dust.noGravity = true;
							dust.velocity.Y *= 0.2f;
							dust.velocity.X = Math.Abs(dust.velocity.X) * (float)Math.Sqrt(Math.Abs(l) + 1) * Main.rand.NextFloat(-1f, 1f) * (2f - z);
							dust.fadeIn = 6;
						}
					}
				}
				Projectile.ai[0]++;
            }
			else
			{
				Projectile.friendly = false;
				Projectile.hostile = false;
			}
			if(totalDistanceToTravel < 0)
			{
				float speed = -Projectile.ai[1] / 50f;
				if (speed < totalDistanceToTravel)
					speed = totalDistanceToTravel;
				totalDistanceToTravel -= speed;
				Projectile.position.Y += speed;
				if (totalDistanceToTravel >= 0)
                {
					totalDistanceToTravel = 0;
                }
				Projectile.ai[1]++;
            }
			if (totalDistanceToTravel == 0)
            {
				if(Projectile.timeLeft > 2)
                {
					ResetHydraulic();
					Projectile.timeLeft = 2;
                }
            }
		}
		public void ResetHydraulic()
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				int i = (int)Projectile.Center.X / 16;
				int j = (int)Projectile.Center.Y / 16;
				Tile tile = Main.tile[i, j];
				if (!tile.HasTile || tile.TileType != ModContent.TileType<HydraulicPressTile>())
				{
					return;
				}
				HydraulicPressTile.ReturnHydraulic(i, j);
			}
		}
        public override void Kill(int timeLeft)
        {
			if(timeLeft > 2)
				ResetHydraulic();
        }
    }
}