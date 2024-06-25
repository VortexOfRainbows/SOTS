using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs.ConduitBoosts;
using SOTS.Common.Systems;
using SOTS.Items.Conduit;
using SOTS.Items.Fragments;
using SOTS.Items.Planetarium;
using SOTS.Items.Pyramid;
using SOTS.Items.Secrets;
using SOTS.Void;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SOTS
{
	public static class ConduitHelper
	{
		public static void preDrawBeforePlayers()
		{
			bool hasDrawnToAcediaPortalNature = false;
			bool hasDrawnToAcediaPortalEarth = false;
            bool hasDrawnToAvaritiaPortalChaos = false;
            bool hasDrawnToAvaritiaPortalOtherworld = false;
            //bool hasDrawnToDreamLamp = false;
            float AcediaPortalMiddleAlpha = 0.0f;
            float AvaritiaPortalMiddleAlpha = 0.0f;
            foreach (ConduitCounterTE tileEntity in TileEntity.ByID.Values.OfType<ConduitCounterTE>())
			{
				if (tileEntity.ConduitTile != null)
				{
					for (int j = 0; j < Main.player.Length; j++)
					{
						Player player = Main.player[j];
						if (player.active)
						{
							float mult = 1f;
							if (player.HasBuff(tileEntity.ConduitTile.BuffType) && j == Main.myPlayer)
							{
								int buffIndex = player.FindBuffIndex(tileEntity.ConduitTile.BuffType);
								float timer = player.buffTime[buffIndex] - 30; //starts at 60, goes to 0
								timer = Math.Clamp(timer, 0, 60);
								float sinusoid = (float)Math.Sin(MathHelper.ToRadians(180f * (1 - timer / 60f)));
								mult += sinusoid;
							}
							tileEntity.DrawConduitToLocation(tileEntity.Position.X, tileEntity.Position.Y, player.Center, 0.9f * mult);
						}
					}
					if (ImportantTilesWorld.AcediaPortal.HasValue)
					{
						int x = ImportantTilesWorld.AcediaPortal.Value.X;
						int y = ImportantTilesWorld.AcediaPortal.Value.Y;
						Tile tile = Main.tile[x, y];
						bool nature = tileEntity.ConduitTile.DissolvingTileType == ModContent.TileType<DissolvingNatureTile>();
						bool earthen = tileEntity.ConduitTile.DissolvingTileType == ModContent.TileType<DissolvingEarthTile>();
                        if (tile.HasUnactuatedTile && tile.TileType == ModContent.TileType<AcediaGatewayTile>() &&
							(nature || earthen))
						{
							Vector2 acediaPortal = new Vector2(x * 16, y * 16) + new Vector2(8, 8);
							bool succeededDraw = tileEntity.DrawConduitToLocation(tileEntity.Position.X, tileEntity.Position.Y, acediaPortal, 1f, ColorHelpers.AcediaColor);
							if (nature && !hasDrawnToAcediaPortalNature && succeededDraw) //This way, it only draws the acedia portal glow once, no matter how many conduits
							{
								float Percent = tileEntity.tileCountDissolving / 20f;
								Percent *= Percent;
								hasDrawnToAcediaPortalNature = true;
                                DrawGatewayGlowmask(x, y, Main.spriteBatch, Percent, -1);
								AcediaPortalMiddleAlpha += Percent * 0.5f;
							}
							if (earthen && !hasDrawnToAcediaPortalEarth && succeededDraw) //This way, it only draws the acedia portal glow once, no matter how many conduits
							{
								float Percent = tileEntity.tileCountDissolving / 20f;
								Percent *= Percent;
								hasDrawnToAcediaPortalEarth = true;
                                DrawGatewayGlowmask(x, y, Main.spriteBatch, Percent, 1);
								AcediaPortalMiddleAlpha += Percent * 0.5f;
							}
						}
					}
					if(ImportantTilesWorld.AvaritiaPortal.HasValue)
                    {
                        int x = ImportantTilesWorld.AvaritiaPortal.Value.X;
                        int y = ImportantTilesWorld.AvaritiaPortal.Value.Y;
                        Tile tile = Main.tile[x, y];
                        bool chaos = tileEntity.ConduitTile.DissolvingTileType == ModContent.TileType<DissolvingBrillianceTile>();
                        bool otherworld = tileEntity.ConduitTile.DissolvingTileType == ModContent.TileType<DissolvingAetherTile>();
                        if (tile.HasUnactuatedTile && tile.TileType == ModContent.TileType<AvaritianGatewayTile>() &&
                            (chaos || otherworld))
                        {
                            Vector2 avaritiaPortal = new Vector2(x * 16, y * 16) + new Vector2(8, 8);
                            bool succeededDraw = tileEntity.DrawConduitToLocation(tileEntity.Position.X, tileEntity.Position.Y, avaritiaPortal, 1f, ColorHelpers.OtherworldColor);
                            if (otherworld && !hasDrawnToAvaritiaPortalOtherworld && succeededDraw) //This way, it only draws the acedia portal glow once, no matter how many conduits
                            {
                                float Percent = tileEntity.tileCountDissolving / 20f;
                                Percent *= Percent;
                                hasDrawnToAvaritiaPortalOtherworld = true;
                                DrawGatewayGlowmask(x, y, Main.spriteBatch, Percent, -1);
                                AvaritiaPortalMiddleAlpha += Percent * 0.5f;
                            }
                            if (chaos && !hasDrawnToAvaritiaPortalChaos && succeededDraw) //This way, it only draws the acedia portal glow once, no matter how many conduits
                            {
                                float Percent = tileEntity.tileCountDissolving / 20f;
                                Percent *= Percent;
                                hasDrawnToAvaritiaPortalChaos = true;
                                DrawGatewayGlowmask(x, y, Main.spriteBatch, Percent, 1);
                                AvaritiaPortalMiddleAlpha += Percent * 0.5f;
                            }
                        }
                    }
					if (ImportantTilesWorld.dreamLamp.HasValue)
					{
						int x = ImportantTilesWorld.dreamLamp.Value.X;
						int y = ImportantTilesWorld.dreamLamp.Value.Y;
						Tile tile = Main.tile[x, y];
						if (tile.HasUnactuatedTile && tile.TileType == ModContent.TileType<ForgottenLampTile>())
						{
							Vector2 dreamLamp = new Vector2(x * 16, y * 16) + new Vector2(8, 8);
							tileEntity.DrawConduitToLocation(tileEntity.Position.X, tileEntity.Position.Y, dreamLamp, 1f, ColorHelpers.DreamLampColor);
						}
					}
					tileEntity.DrawConduitAura(tileEntity.Position.X, tileEntity.Position.Y);
                }
			}
			if (ImportantTilesWorld.AcediaPortal.HasValue && (hasDrawnToAcediaPortalNature || hasDrawnToAcediaPortalEarth))
			{
				int x = ImportantTilesWorld.AcediaPortal.Value.X;
				int y = ImportantTilesWorld.AcediaPortal.Value.Y;
				if (AcediaPortalMiddleAlpha > 0.0f)
                    DrawGatewayGlowmask(x, y, Main.spriteBatch, AcediaPortalMiddleAlpha, 0);
            }
            if (ImportantTilesWorld.AvaritiaPortal.HasValue && (hasDrawnToAvaritiaPortalChaos || hasDrawnToAvaritiaPortalOtherworld))
            {
                int x = ImportantTilesWorld.AvaritiaPortal.Value.X;
                int y = ImportantTilesWorld.AvaritiaPortal.Value.Y;
                if (AcediaPortalMiddleAlpha > 0.0f)
					DrawGatewayGlowmask(x, y, Main.spriteBatch, AvaritiaPortalMiddleAlpha, 0);
            }
        }
		public static void DrawPlayerEffectOutline(SpriteBatch spriteBatch, Player player)
		{
			if (player.HasBuff<NatureBoosted>())
			{
				int buffIndex = player.FindBuffIndex(ModContent.BuffType<NatureBoosted>());
				float timer = player.buffTime[buffIndex] - 30; //starts at 60, goes to 0
				if (timer > 0)
				{
					Color color = ColorHelpers.natureColor;
					float percent = 1 - timer / 60f;
					DrawConduitCircleFull(player.Center, percent, color);
				}
			}
			if (player.HasBuff<EarthBoosted>())
			{
				int buffIndex = player.FindBuffIndex(ModContent.BuffType<EarthBoosted>());
				float timer = player.buffTime[buffIndex] - 30; //starts at 60, goes to 0
				if (timer > 0)
				{
					Color color = ColorHelpers.EarthColor;
					float percent = 1 - timer / 60f;
					DrawConduitCircleFull(player.Center, percent, color);
				}
			}
		}
		public static void DrawConduitCircleFull(Vector2 position, float percent, Color color)
		{
			color.A = 0;
			SOTSProjectile.DrawStar(position, color, 0.3f * percent, MathHelper.PiOver4, 0f, 1, 52f * (float)Math.Sqrt((1 - percent)), 0, 1f, 600, 0, 1);
		}
        public static void DrawGatewayGlowmask(int x, int y, SpriteBatch spriteBatch, float percent = 1, int side = -1)
        {
            Tile tile = Main.tile[x, y];
            x = x - tile.TileFrameX / 18;
            y = y - tile.TileFrameY / 18;
            string variant = side == -1 ? "Left" : side == 0 ? "Middle" : "Right";
            int maximumGlow = 8;
            maximumGlow = (int)(maximumGlow * percent + 0.5f);
            Texture2D texture = ModContent.Request<Texture2D>("SOTS/Items/Conduit/Portal/AcediaGatewayTileGlow" + variant).Value;
            Texture2D textureMask = ModContent.Request<Texture2D>("SOTS/Items/Conduit/Portal/AcediaGatewayTileGlowMask" + variant).Value;
            Color defaultColor = new Color(120, 100, 130, 0);
            Color alternatingColor = ColorHelpers.AcediaColor * 0.65f;
            if (tile.TileType == ModContent.TileType<AvaritianGatewayTile>())
			{
                texture = ModContent.Request<Texture2D>("SOTS/Items/Conduit/Portal/AvaritianGatewayTileGlow" + variant).Value;
                textureMask = ModContent.Request<Texture2D>("SOTS/Items/Conduit/Portal/AvaritianGatewayTileGlowMask" + variant).Value;
                defaultColor = new Color(120, 100, 130, 0);
                alternatingColor = ColorHelpers.OtherworldColor * 0.65f;
            }
            for (int twice = 0; twice < 2; twice++)
            {
                for (int i = x; i < x + 9; i++)
                {
                    for (int j = y; j < y + 9; j++)
                    {
                        tile = Main.tile[i, j];
                        float uniquenessCounter = Main.GlobalTimeWrappedHourly * -100 + (i + j) * 6;
                        float lerpMult = 0.5f + 0.5f * (float)Math.Sin(MathHelper.ToRadians(-uniquenessCounter));
                        Color color = Color.Lerp(defaultColor, alternatingColor, lerpMult);
                        color.A = 0;
                        Rectangle frame = new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16);
                        Vector2 pos = new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y);
                        if (twice == 1)
                        {
                            for (int k = 0; k <= maximumGlow; k++)
                            {
                                float offset = 2f * percent;
                                Vector2 circular = new Vector2(offset, 0).RotatedBy(MathHelper.TwoPi * k / maximumGlow + MathHelper.ToRadians(SOTSWorld.GlobalCounter * side * 0.75f));
                                spriteBatch.Draw(texture, pos + circular + new Vector2(0, 2), frame, color * percent * 0.45f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                            }
                        }
                        else
                        {
                            color = Color.White;
                            spriteBatch.Draw(textureMask, pos + new Vector2(0, 2), frame, color * 0.75f * percent, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        }
                    }
                }
            }
        }
    }
}
