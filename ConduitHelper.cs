using Microsoft.Xna.Framework;
using SOTS.Buffs.ConduitBoosts;
using SOTS.Common.Systems;
using SOTS.Items.Conduit;
using SOTS.Items.Pyramid;
using SOTS.Items.Secrets;
using System;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

public static class ConduitHelper
{
    public static void preDrawBeforePlayers()
	{
		bool hasDrawnToAcediaPortal = false;
		float AcediaPortalMiddleAlpha = 0.0f; //Later, another portal will be resposible for completing this value
		foreach (ConduitCounterTE tileEntity in TileEntity.ByID.Values.OfType<ConduitCounterTE>())
		{
			for (int j = 0; j < Main.player.Length; j++)
			{
				Player player = Main.player[j];
				if (player.active)
				{
					float mult = 1f;
					if (player.HasBuff<NatureBoosted>() && j == Main.myPlayer)
					{
						int buffIndex = player.FindBuffIndex(ModContent.BuffType<NatureBoosted>());
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
				if (tile.HasUnactuatedTile && tile.TileType == ModContent.TileType<AcediaGatewayTile>())
				{
					Vector2 acediaPortal = new Vector2(x * 16, y * 16) + new Vector2(8, 8);
					bool succeededDraw = tileEntity.DrawConduitToLocation(tileEntity.Position.X, tileEntity.Position.Y, acediaPortal, 1f, ColorHelpers.AcediaColor);
					if (!hasDrawnToAcediaPortal && succeededDraw) //This way, it only draws the acedia portal glow once, no matter how many conduits
					{
						float Percent = tileEntity.tileCountDissolving / 20f;
						Percent *= Percent;
						hasDrawnToAcediaPortal = true;
						AcediaGatewayTile.DrawGlowmask(x, y, Main.spriteBatch, Percent, -1);
						AcediaPortalMiddleAlpha += Percent * 0.5f;
					}
				}
			}
			if (ImportantTilesWorld.dreamLamp.HasValue)
			{
				int x = ImportantTilesWorld.dreamLamp.Value.X;
				int y = ImportantTilesWorld.dreamLamp.Value.Y;
				Tile tile = Main.tile[x, y];
				if(tile.HasUnactuatedTile && tile.TileType == ModContent.TileType<ForgottenLampTile>())
				{
					Vector2 dreamLamp = new Vector2(x * 16, y * 16) + new Vector2(8, 8);
					tileEntity.DrawConduitToLocation(tileEntity.Position.X, tileEntity.Position.Y, dreamLamp, 1f, ColorHelpers.DreamLampColor);
				}
			}
		}
		if (ImportantTilesWorld.AcediaPortal.HasValue && hasDrawnToAcediaPortal)
		{
			int x = ImportantTilesWorld.AcediaPortal.Value.X;
			int y = ImportantTilesWorld.AcediaPortal.Value.Y;
			if (AcediaPortalMiddleAlpha > 0.0f)
				AcediaGatewayTile.DrawGlowmask(x, y, Main.spriteBatch, AcediaPortalMiddleAlpha, 0);
		}
	}
}