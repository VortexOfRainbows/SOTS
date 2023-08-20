using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs.ConduitBoosts;
using SOTS.Common.Systems;
using SOTS.Dusts;
using SOTS.Items.Fragments;
using SOTS.Items.Secrets;
using SOTS.Void;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Conduit
{
	public class NatureConduit : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(100);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Blue;
			Item.Size = new Vector2(42, 42);
			Item.value = Item.buyPrice(0, 0, 40, 0);
			Item.createTile = ModContent.TileType<NatureConduitTile>();
		}
	}
	public class NatureConduitTile : ConduitTile
	{
		public override int DissolvingTileType => ModContent.TileType<DissolvingNatureTile>();
		public override Texture2D GlowTexture => ModContent.Request<Texture2D>("SOTS/Items/Conduit/NatureConduitTileGlow").Value;
		public override Color elementalColor => ColorHelpers.natureColor;
		public override int BuffType => ModContent.BuffType<NatureBoosted>();
		public override void SafeSetStaticDefaults()
		{
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(SOTSTile.NaturePlatingColor, name);
			DustType = DustID.Tungsten;
		}
	}
	public class EarthenConduit : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(100);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Blue;
			Item.Size = new Vector2(42, 42);
			Item.value = Item.buyPrice(0, 0, 40, 0);
			Item.createTile = ModContent.TileType<EarthenConduitTile>();
		}
	}
	public class EarthenConduitTile : ConduitTile
	{
		public override int DissolvingTileType => ModContent.TileType<DissolvingEarthTile>();
		public override Texture2D GlowTexture => ModContent.Request<Texture2D>("SOTS/Items/Conduit/EarthenConduitTileGlow").Value;
		public override Color elementalColor => ColorHelpers.EarthColor;
		public override int BuffType => ModContent.BuffType<EarthBoosted>();
		public override void SafeSetStaticDefaults()
		{
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(SOTSTile.EarthenPlatingColor, name);
			DustType = DustID.Iron;
		}
	}
	public abstract class ConduitTile : ModTile
	{
		public sealed override void SetStaticDefaults()
		{
			TileID.Sets.DrawsWalls[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = false; //sunlight passes through these pipes
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<ConduitCounterTE>().Hook_AfterPlacement, -1, 0, true);
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
			TileObjectData.newTile.AnchorRight = AnchorData.Empty;
			TileObjectData.newTile.AnchorLeft = AnchorData.Empty;
			TileObjectData.newTile.AnchorTop = AnchorData.Empty;
			TileObjectData.addTile(Type);
			MineResist = 20f;
			MinPick = 300;
			HitSound = SoundID.Tink;
			SafeSetStaticDefaults();
		}
		public virtual void SafeSetStaticDefaults()
		{
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(66, 93, 77), name);
			DustType = DustID.Tungsten;
		}
		public sealed override bool CreateDust(int i, int j, ref int type)
		{
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameY == 82)
			{
				for (int k = -2; k <= 2; k++)
				{
					for (int h = -2; h <= 2; h++)
					{
						if (Math.Abs(h) != 2 || Math.Abs(k) != 2) //will not check outer 4 corners
						{
							if (k != 0 || h != 0) //will not check the very center
							{
								if (Main.rand.NextBool(3))
									Dust.NewDust(new Vector2(i + k, j + h) * 16, 16, 16, DustID.Lead);
							}
						}
					}
				}
			}
			return base.CreateDust(i, j, ref type);
		}
		public sealed override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			return true;
		}
		public sealed override bool Slope(int i, int j)
		{
			return false;
		}
		public sealed override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			return false;
		}
		public sealed override bool CanPlace(int i, int j)
		{
			for (int i2 = i - 2; i2 <= i + 2; i2++)
			{
				for (int j2 = j - 2; j2 <= j + 2; j2++)
				{
					Tile tile = Framing.GetTileSafely(i2, j2);
					if (tile.TileType == Type)
					{
						return false;
					}
				}
			}
			return true;
		}
		public sealed override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
		{
			Main.instance.TilesRenderer.AddSpecialLegacyPoint(i, j);
		}
        public override IEnumerable<Item> GetItemDrops(int i, int j)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			if (tile.TileFrameY == 82)
			{
				yield return new Item(ModContent.ItemType<ConduitChassis>(), 20);
			}
			yield return new Item(TileLoader.GetItemDropFromTypeAndStyle(Type, 0));
		}
		public sealed override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			if(!(ModContent.GetModTile(tile.TileType) is ConduitTile))
			{
				return;
			}
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Texture2D texture = Terraria.GameContent.TextureAssets.Tile[Type].Value;
			Vector2 location = new Vector2(i * 16, j * 16) + new Vector2(8, 8);
			Color color = Lighting.GetColor(i, j, WorldGen.paintColor(Main.tile[i, j].TileColor));
			Vector2 origin = new Vector2(40, 40);
			DrawGlowingParts(i, j, spriteBatch, 1);
			spriteBatch.Draw(texture, location + zero - Main.screenPosition, new Rectangle(16 + tile.TileFrameX, tile.TileFrameY, 80, 80), color, 0f, origin, 1f, SpriteEffects.None, 0f);
			DrawGlowingParts(i, j, spriteBatch, 0);
			DrawGlowingParts(i, j, spriteBatch, 2);
		}
		public sealed override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (!fail && !effectOnly && !noItem)
			{
				for (int k = -2; k <= 2; k++)
				{
					for (int h = -2; h <= 2; h++)
					{
						if (Math.Abs(h) != 2 || Math.Abs(k) != 2) //will not check outer 4 corners
						{
							if (k != 0 || h != 0) //will not check the very center
							{
								int x = i + k;
								int y = j + h;
								Tile residualTile = Main.tile[x, y];
								if (residualTile.HasTile)
								{
									if (residualTile.TileType == DissolvingTileType)
									{
										WorldGen.KillTile(x, y);
										NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, x, y);
									}
								}
							}
						}
					}
				}
				ModContent.GetInstance<ConduitCounterTE>().Kill(i, j); //This should be done automatically, but I'll run it anyway
			}
		}
		public void DrawGlowingParts(int i, int j, SpriteBatch spriteBatch, int type = 0)
		{
			Tile tile = Main.tile[i, j];
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			int index = ModContent.GetInstance<ConduitCounterTE>().Find(i, j);
			if (index == -1)
				return;
			ConduitCounterTE entity = (ConduitCounterTE)TileEntity.ByID[index];
			Vector2 location = new Vector2(i * 16, j * 16) + new Vector2(8, 8) + zero;
			float dissolving = (float)entity.tileCountDissolving;
			float colorMultiplier = dissolving / 20f;
			Color colorWhite = Color.White * colorMultiplier;
			Color colorGlow = new Color(100, 100, 100, 0) * colorMultiplier;
			if (colorMultiplier <= 0)
				return;
			if (type == 0)
			{
				Texture2D texture = GlowTexture;
				Vector2 origin = new Vector2(40, 40);
				for (int k = 0; k < 8; k++)
				{
					Vector2 circular = new Vector2(1 + 3 * colorMultiplier, 0).RotatedBy(MathHelper.PiOver4 * k + SOTSWorld.GlobalCounter * MathHelper.Pi / 180f);
					spriteBatch.Draw(texture, location + circular - Main.screenPosition, new Rectangle(16 + tile.TileFrameX, tile.TileFrameY, 80, 80), colorGlow * 0.75f, 0f, origin, 1f, SpriteEffects.None, 0f);
				}
				spriteBatch.Draw(texture, location - Main.screenPosition, new Rectangle(16 + tile.TileFrameX, tile.TileFrameY, 80, 80), colorWhite, 0f, origin, 1f, SpriteEffects.None, 0f);
			}
			else if (type == 1)
			{
				Texture2D spiral = ModContent.Request<Texture2D>("SOTS/Common/GlobalNPCs/FreezeSpiral3").Value;
				Vector2 origin = spiral.Size() / 2;
				float desiredWidth = 80f * colorMultiplier;
				float sizeMult = desiredWidth / spiral.Width;
				colorGlow = elementalColor * colorMultiplier;
				colorGlow.A = 0;
				for (int k = -2; k <= 2; k++)
				{
					if (k != 0)
						spriteBatch.Draw(spiral, location - Main.screenPosition, null, colorGlow * (1f / Math.Abs(k)), SOTSWorld.GlobalCounter * MathHelper.Pi / 180f * k, origin, sizeMult, SpriteEffects.None, 0f);
				}
			}
			else if (type == 2)
			{
				Texture2D texture = Mod.Assets.Request<Texture2D>("Effects/Masks/Extra_49").Value;
				Vector2 origin = texture.Size() / 2;
				float desiredWidth = 110f * colorMultiplier * colorMultiplier;
				float sizeMult = desiredWidth / texture.Width;
				Color color = elementalColor;
				color.A = 0;
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.Identity);
				SOTS.GodrayShader.Parameters["distance"].SetValue(4);
				SOTS.GodrayShader.Parameters["colorMod"].SetValue(color.ToVector4());
				SOTS.GodrayShader.Parameters["noise"].SetValue(Mod.Assets.Request<Texture2D>("TrailTextures/noise").Value);
				SOTS.GodrayShader.Parameters["rotation"].SetValue(MathHelper.ToRadians(SOTSWorld.GlobalCounter / -4f));
				SOTS.GodrayShader.Parameters["opacity2"].SetValue(colorMultiplier * colorMultiplier);
				SOTS.GodrayShader.CurrentTechnique.Passes[0].Apply();
				Main.spriteBatch.Draw(texture, location - Main.screenPosition, null, Color.White, 0f, origin, sizeMult * colorMultiplier, SpriteEffects.None, 0f);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Camera.Rasterizer, null, Main.Camera.GameViewMatrix.TransformationMatrix);
                colorGlow = elementalColor * colorMultiplier;
                colorGlow.A = 0;
                desiredWidth = 18f;
                sizeMult = desiredWidth / texture.Width;
				for (int k = 0; k <= 2; k++)
					Main.spriteBatch.Draw(texture, location - Main.screenPosition, null, colorGlow, 0f, origin, sizeMult * colorMultiplier, SpriteEffects.None, 0f);
			}
		}
		public virtual int DissolvingTileType => ModContent.TileType<DissolvingBrillianceTile>();
		public virtual Texture2D GlowTexture => ModContent.Request<Texture2D>("SOTS/Items/Conduit/NatureConduitTileGlow").Value;
		public virtual Color elementalColor => ColorHelpers.natureColor;
		public virtual int BuffType => ModContent.BuffType<NatureBoosted>();
		public sealed override void NearbyEffects(int i, int j, bool closer)
		{
			int left = i;
			int top = j;
			int index = ModContent.GetInstance<ConduitCounterTE>().Find(left, top);
			if (index == -1)
				return;
			ConduitCounterTE entity = (ConduitCounterTE)TileEntity.ByID[index];
			int Chassis = entity.tileCountChassis;
			int dissolving = entity.tileCountDissolving;
			entity.NearbyClientUpdate(i, j);
			if (Chassis != entity.tileCountChassis || dissolving != entity.tileCountDissolving)
			{
				if (dissolving < entity.tileCountDissolving)
				{
					if (entity.tileCountDissolving == 20 && dissolving >= 18)
					{
						SOTSUtils.PlaySound(SoundID.Item37, i * 16, j * 16, 0.75f, 0.1f);
					}
					else if (dissolving != -1)
						SOTSUtils.PlaySound(SoundID.Item35, i * 16, j * 16, 0.50f + 0.01f * entity.tileCountDissolving, 0.2f - 0.01f * entity.tileCountDissolving);
				}
				else if (Chassis < entity.tileCountChassis)
				{
					if (entity.tileCountChassis == 20 && Chassis >= 18)
					{
						SOTSUtils.PlaySound(SoundID.Item37, i * 16, j * 16, 0.75f, 0.1f);
					}
					else if (Chassis != -1)
						SOTSUtils.PlaySound(SoundID.Item52, i * 16, j * 16, 0.6f, -0.5f + 0.025f * entity.tileCountChassis);
				}
				//Main.NewText("D: " + entity.tileCountDissolving + "\nC: " + entity.tileCountChassis);
			}
			if (entity.tileCountDissolving >= 20)
			{
				Player myPlayer = Main.LocalPlayer;
				float distance = Vector2.Distance(myPlayer.Center, new Vector2(i * 16 + 8, j * 16 + 8));
				if (distance <= 240)
				{
					if (!myPlayer.HasBuff(BuffType))
						myPlayer.AddBuff(BuffType, 90, false);
				}
				if (ImportantTilesWorld.dreamLamp.HasValue && BuffType == ModContent.BuffType<NatureBoosted>())
				{
					Vector2 dreamLamp = new Vector2(ImportantTilesWorld.dreamLamp.Value.X * 16 + 8, ImportantTilesWorld.dreamLamp.Value.Y * 16 + 8);
					distance = Vector2.Distance(dreamLamp, new Vector2(i * 16 + 8, j * 16 + 8));
					if (distance <= 640)
					{
						bool DoesProjectileExist = false;
						for (int a = 0; a < 1000; a++)
						{
							Projectile proj = Main.projectile[a];
							if (proj.active && proj.type == ModContent.ProjectileType<ForgottenLampProjectile>())
							{
								DoesProjectileExist = true;
								break;
							}
						}
						if (!DoesProjectileExist)
						{
							Projectile.NewProjectile(new EntitySource_TileUpdate(i, j, "SOTS:Conduit"), dreamLamp, Vector2.Zero, ModContent.ProjectileType<ForgottenLampProjectile>(), 0, 0, Main.myPlayer, i, j);
						}
					}
				}
			}
		}
	}
	public class ConduitCounterTE : ModTileEntity
	{
		private ConduitTile CTile = ModContent.GetInstance<ConduitTile>();
		public ConduitTile ConduitTile => CTile == null ? ModContent.GetInstance<ConduitTile>() : CTile;
		public int tileCountDissolving = -1;
		public int tileCountChassis = -1;
		public bool DrawConduitToLocation(int i, int j, Vector2 destination, float alphaScale = 1f, Color lerpColor = default)
		{
			if (Main.netMode == NetmodeID.Server || tileCountDissolving <= 0)
				return false;
			Vector2 startingLocation = new Vector2(i * 16 + 8, j * 16 + 8);
			Vector2 toStart = startingLocation - destination;
			destination += toStart.SafeNormalize(Vector2.Zero) * 4;
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[ModContent.ProjectileType<Projectiles.Camera.DreamLaser>()].Value;
			float length = Vector2.Distance(startingLocation, destination);
			float alphaMult = tileCountDissolving / 20f;
			float alpha = 2.2f * (alphaMult - (length / 800f)); //at about 440 -> max brightness
			alpha = MathHelper.Clamp(alpha * alphaMult, 0, 1);
			if (alpha <= 0)
				return false;
			Vector2 origin = new Vector2(0, texture.Height / 2);
			float textureSize = texture.Width;
			float density = 0.25f;
			float max = length / (textureSize * density);
			int totalHelixes = 1 + tileCountDissolving / 5;
			for (float v = 0; v < max; v += 0.5f)
			{
				Color color = ConduitTile.elementalColor;
				if (lerpColor != default)
				{
					color = Color.Lerp(color, lerpColor, (float)Math.Pow(v / max, 2));
				}
				Vector2 drawPos = Vector2.Lerp(startingLocation, destination, v / max);
				Vector2 direction = destination - startingLocation;
				float rotation = direction.ToRotation();
				float heightOfHelix = (float)Math.Sin(v / max * MathHelper.Pi);
				float otherAlphaMult = (float)Math.Sqrt(heightOfHelix);
				for (int b = 0; b < totalHelixes; b++)
				{
					float scaleY = 1;
					float size = 4;
					if (b >= 3)
					{
						size = 6;
						if (b == 3)
							scaleY *= 0.55f;
						else
							scaleY *= 0.45f;
					}
					float sinusoidMod = 1 + (float)Math.Sin(MathHelper.ToRadians(b * SOTSWorld.GlobalCounter * 3 * scaleY * scaleY + v * 8f / b * scaleY));
					float sizeOfHelix = sinusoidMod * heightOfHelix * alpha * size * alphaMult;
					Vector2 sinusoid = new Vector2(0, sizeOfHelix * (float)Math.Sin(MathHelper.ToRadians(v * 6 + SOTSWorld.GlobalCounter * 2 * scaleY + b * 120))).RotatedBy(rotation);
					Color color2 = color * alpha * 0.33f;
					color2.A = 0;
					Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition + sinusoid, null, color2 * otherAlphaMult * alphaScale, rotation, origin, new Vector2(density, scaleY * 0.5f), SpriteEffects.None, 0f);
				}
			}
			return alpha >= 0.2f;
		}
		public void NearbyClientUpdate(int i, int j) //Normally, TileEntity.Update() is run for singleplayer/server... This will be run for singleplayer/client, and syncing is made that way thusly.
		{
			tileCountDissolving = 0;
			tileCountChassis = 0;
			Tile tile = Main.tile[i, j];
			if (ConduitTile == ModContent.GetInstance<ConduitTile>())
			{
				ModTile mTile = ModContent.GetModTile(tile.TileType);
				if (mTile is ConduitTile cTile)
					CTile = cTile;
			}
			for (int k = -2; k <= 2; k++)
			{
				for (int h = -2; h <= 2; h++)
				{
					if (Math.Abs(h) != 2 || Math.Abs(k) != 2) //will not check outer 4 corners
					{
						if (k != 0 || h != 0) //will not check the very center
						{
							int x = i + k;
							int y = j + h;
							Tile residualTile = Main.tile[x, y];
							if (tile.TileFrameY != 0)
							{
								if (residualTile.HasTile)
								{
									if (residualTile.TileType == ConduitTile.DissolvingTileType)
									{
										tileCountDissolving++;
									}
									else
									{
										//BreakTile(x, y);
									}
								}
							}
							else if (residualTile.HasTile && residualTile.TileType == ConduitTile.DissolvingTileType)
							{
								BreakTile(x, y);
							}
							else if (residualTile.HasTile && residualTile.TileType == ModContent.TileType<ConduitChassisTile>())
							{
								tileCountChassis++;
							}
						}
					}
				}
			}
			ConvertToActiveState(i, j);
		}
		public void ConvertToActiveState(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			if (tileCountChassis >= 20)
			{
				tile.TileFrameY = 82;
				for (int k = -2; k <= 2; k++)
				{
					for (int h = -2; h <= 2; h++)
					{
						if (Math.Abs(h) != 2 || Math.Abs(k) != 2) //will not check outer 4 corners
						{
							if (k != 0 || h != 0) //will not check the very center
							{
								int x = i + k;
								int y = j + h;
								WorldGen.KillTile(x, y, noItem: true);
							}
						}
					}
				}
				NetMessage.SendTileSquare(Main.myPlayer, i, j, 5);
			}
		}
		public void BreakTile(int i, int j, bool noDrop = false)
		{
			WorldGen.KillTile(i, j, noItem: noDrop);
			NetMessage.SendData(MessageID.TileManipulation, Main.myPlayer, Main.myPlayer, null, 0, i, j);
		}
		public override bool IsTileValidForEntity(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			return tile.HasTile && ModContent.GetModTile(tile.TileType) is ConduitTile;
		}
		public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
		{
			//Main.NewText("i " + i + " j " + j + " t " + type + " s " + style + " d " + direction);
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				NetMessage.SendTileSquare(Main.myPlayer, i, j, 5);
				NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type, 0f, 0, 0, 0);
				return -1;
			}
			return Place(i, j);
		}
	}
}