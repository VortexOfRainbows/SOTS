using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items;
using SOTS.NPCs.Boss.Curse;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.Projectiles.Tide
{
	public class PlasmaShrimp : ModProjectile
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Backup Bow Visual");
		}
		public sealed override void SetDefaults()
		{
			Projectile.width = 50;
			Projectile.height = 40;
			Projectile.tileCollide = false;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 300;
			Projectile.netImportant = true;
			Projectile.alpha = 255;
		}
        public override void PostDraw(Color lightColor)
		{
			Texture2D Stexture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Tide/PlasmaShrimpStem");
			Player player = Main.player[Projectile.owner];
			Color color = Projectile.GetAlpha(lightColor);
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
			Vector2 drawOrigin = new Vector2(19 - player.direction * player.gravDir, 36);
			Vector2 drawOriginStem = new Vector2(18, 31);
			Vector2 mousePos = new Vector2(Projectile.ai[0], Projectile.ai[1]);
			float Rotation = (mousePos - Projectile.Center).ToRotation();
			Vector2 direction = new Vector2(1, 0).RotatedBy(Rotation);
			if (direction.X < 0)
			{
				Projectile.spriteDirection = -1;
			}
			else
            {
				Projectile.spriteDirection = 1;
            }
			SpriteEffects spriteEffects = (Projectile.spriteDirection * player.gravDir) == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None;
			if(Projectile.spriteDirection * player.gravDir == -1)
				drawOrigin = new Vector2(19 + player.direction * player.gravDir, texture.Height - 36);
			Vector2 round = (Projectile.Center - Main.screenPosition);
			if (SOTSPlayer.ModPlayer(player).PlasmaShrimpVanity)
			{
				int otherOtherFlip = 1;
				float rotation = 0f;
				if (player.gravDir == -1)
                {
					rotation = -MathHelper.Pi;
					otherOtherFlip = -1;
				}
				if ((player.direction * otherOtherFlip) == -1)
					drawOriginStem = new Vector2(Stexture.Width - 18, 31);
				Main.spriteBatch.Draw(Stexture, new Vector2((int)round.X, (int)round.Y) + new Vector2(0, player.gfxOffY), null, color, rotation, drawOriginStem, Projectile.scale, (player.direction * otherOtherFlip) == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}				
			Main.spriteBatch.Draw(texture, new Vector2((int)round.X, (int)round.Y) + new Vector2(0, player.gfxOffY), null, color, Rotation, drawOrigin, Projectile.scale,spriteEffects, 0f);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			return false;
		}
		public Color GetColor()
		{
			return Color.Purple;
        }
		public void FindPosition()
		{
			Vector2 mousePos = new Vector2(Projectile.ai[0], Projectile.ai[1]);
			float rotation = 0f;
			if(mousePos.X > 0 && mousePos.Y > 0)
            {
				rotation = (mousePos - Projectile.Center).ToRotation();
			}
			Player player = Main.player[Projectile.owner];
			if (Main.myPlayer != Projectile.owner)
				Projectile.timeLeft = 20;
			Vector2 idlePosition = new Vector2(-38, 0) * player.direction;
			//Projectile.ai[0]++;
			Projectile.rotation = idlePosition.ToRotation() - (Projectile.spriteDirection == 1 ? MathHelper.Pi : 0);
			idlePosition = player.Center + idlePosition;
			idlePosition.Y -= 30 * player.gravDir;
			Projectile.Center = idlePosition;
		}
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			FindPosition();
			if(Main.myPlayer == Projectile.owner)
            {
				Projectile.ai[0] = Main.MouseWorld.X;
				Projectile.ai[1] = Main.MouseWorld.Y;
				if (SOTSWorld.GlobalCounter % 5 == 0)
					Projectile.netUpdate = true;
            }
			/*if (player.itemAnimation > 0)
			{
				itemAlpha = -0.48f;
			}*/
			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 25;
			}
			else
				Projectile.alpha = 0;
			Lighting.AddLight(Projectile.Center, GetColor().ToVector3() * 0.25f);
		}
	}
}