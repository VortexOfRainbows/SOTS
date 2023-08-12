using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Fragments;
using SOTS.NPCs.Boss;
using SOTS.Projectiles.Blades;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
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
        public override void AddRecipes()
		{
			Mod CALAMITY;
			bool calamity = ModLoader.TryGetMod("CalamityMod", out CALAMITY);
			if(calamity)
			{
				CreateRecipe(1).AddIngredient(CALAMITY, "MysteriousCircuitry", 10)
					.AddIngredient(CALAMITY, "DubiousPlating", 10)
					.AddIngredient(ModContent.ItemType<EarthenPlating>(), 10)
					.AddTile(TileID.Anvils).Register();
			}
			else
			{
				CreateRecipe(1).AddRecipeGroup("SOTS:CrushingComponents", 1)
					.AddIngredient(ModContent.ItemType<EarthenPlating>(), 20)
					.AddTile(TileID.Anvils).Register();
			}
		}
    }	
	public class HydraulicPressTile : ModTile
	{
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Main.tileBlockLight[Type] = false;
			Main.tileNoSunLight[Type] = false;
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
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = false;
			Main.tileLavaDeath[Type] = false;
			Main.tileWaterDeath[Type] = false;
			Main.tileBlockLight[Type] = false;
			Main.tileNoSunLight[Type] = false;
			TileID.Sets.DrawsWalls[Type] = true;
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
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(SOTSTile.EarthenPlatingColor, name);
			DustType = DustID.Iron;
			MineResist = 0.1f;
		}
        public override void NearbyEffects(int i, int j, bool closer)
		{
			if (closer)
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
								if(!tile.IsActuated && tile.TileType == Type)
                                {
									tile.IsActuated = true;
									NetMessage.SendTileSquare(Main.myPlayer, trueI, trueJ, 1);
								}
							}
							else if(tile.IsActuated && tile.TileType == Type)
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
			if (npc.friendly || !npc.active || npc.dontTakeDamage)
				return;
			int i = (int)npc.Center.X / 16;
			int j = (int)npc.Center.Y / 16;
			if (!WorldGen.InWorld(i, j, 20))
				return;
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
			if (Main.netMode != NetmodeID.MultiplayerClient)
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
					NetMessage.SendTileSquare(-1, left + 3, top + 4, 9);
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
				NetMessage.SendTileSquare(-1, left + 3, top + 4, 9);
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
			Projectile.netImportant = true;
			Projectile.hide = true;
		}
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
			behindNPCs.Add(index);
        }
        public override bool CanHitPlayer(Player target)
		{
			return true;
        }
        Vector2 initialPosition = Vector2.Zero;
        public override bool PreDraw(ref Color lightColor)
		{
			if(initialPosition == Vector2.Zero)
            {
				initialPosition = Projectile.Center;
            }
			int i = (int)initialPosition.X / 16;
			int j = (int)initialPosition.Y / 16;
			lightColor = Lighting.GetColor(i, j);
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
			Vector2 origin = new Vector2(texture.Width / 2, 28);
			int height = 10 + (int)(48 - totalDistanceToTravel);
			if (totalDistanceToTravel < 0)
				height = 10 - (int)totalDistanceToTravel;
			if (totalDistanceToTravel == 0)
				height = 10;
			Rectangle showPixels = new Rectangle(0, 0, texture.Width, height);
			Main.spriteBatch.Draw(texture, new Vector2((int)Projectile.Center.X, (int)(Projectile.Center.Y + (58 - height)) + 2.5f) - Main.screenPosition, showPixels, lightColor, 0f, origin, 1f, SpriteEffects.FlipVertically, 0f);
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
				float speed = Projectile.ai[0] * 1f;
				if (speed > totalDistanceToTravel)
					speed = totalDistanceToTravel;
				totalDistanceToTravel -= speed;
				Projectile.position.Y += speed;
				if(totalDistanceToTravel <= 0)
                {
					Projectile.friendly = true;
					Projectile.hostile = true;
					totalDistanceToTravel = -48;
					SOTSUtils.PlaySound(SoundID.Item53, Projectile.Center, 0.9f, -0.6f);
					Color color = ColorHelpers.EarthColor;
					color.A = 0;
					for (int l = 0; l < 25; l++)
					{
						for(int z = 0; z <= 1; z++)
						{
							Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - Projectile.width / 4, Projectile.position.Y + Projectile.height - 2) - new Vector2(5, 5), Projectile.width / 2, 4, ModContent.DustType<PixelDust>(),
								0, -z * Main.rand.NextFloat(0.4f, 2.5f), 0, color, Main.rand.NextFloat(0.8f, 1.2f));
							dust.noGravity = true;
							dust.velocity.Y *= 0.2f;
							dust.velocity.X = Math.Abs(dust.velocity.X) * (float)Math.Sqrt(l + 1) * Main.rand.NextFloat(-1f, 1f) * (2f - z);
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
				float speed = -0.1f -Projectile.ai[1] / 80f;
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
				if(Projectile.timeLeft > 3)
                {
					ResetHydraulic();
					Projectile.timeLeft = 3;
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