using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Helpers;
using SOTS.Void;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Pyramid.GhostPepper 
{    
    public class SoulofLooting : ModProjectile 
    {
		private int Owner => (int)Projectile.ai[0];
		public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 8;
        }
        public override void SetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.netImportant = true;
			Projectile.timeLeft = 900;
			Projectile.hostile = false;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.alpha = 0;
		}
        public override void PostDraw(Color lightColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>(GhostPepper.IsAlternate ? "Projectiles/Pyramid/GhostPepper/BrownFeather" : "Projectiles/Pyramid/GhostPepper/SoulofLooting").Value;
			Color c = ColorHelper.SoulLootingColor;
			c.A = 0;
            if (!GhostPepper.IsAlternate)
			{
                Vector2 drawOrigin1 = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f * 0.125f);
                Vector2 drawPos2 = Projectile.Center - Main.screenPosition;
				Rectangle frame = new Rectangle(0, 22 * Projectile.frame, 22, 22);
                for (int i = 0; i < 6; i++)
                {
                    Vector2 circular = new Vector2(4f, 0).RotatedBy(MathHelper.ToRadians(SOTSWorld.GlobalCounter * 3f + 60 * i));
                    Main.spriteBatch.Draw(texture, drawPos2 + circular, frame, Projectile.GetAlpha(c) * 0.75f, Projectile.rotation, drawOrigin1, Projectile.scale, SpriteEffects.None, 0f);
                }
                Main.spriteBatch.Draw(texture, drawPos2, frame, Projectile.GetAlpha(ColorHelper.SoulLootingColor * 1.5f), Projectile.rotation, drawOrigin1, Projectile.scale, SpriteEffects.None, 0f);
            }
            else
            {
                Vector2 sinusoid = new Vector2(0, 4 * (float)Math.Cos(1.7f * MathHelper.ToRadians(SOTSWorld.GlobalCounter)));
                float rotation = 15 * (float)Math.Sin(1f * MathHelper.ToRadians(SOTSWorld.GlobalCounter)) * MathHelper.Pi / 180f;
                Vector2 drawOrigin1 = texture.Size() / 2;
                Vector2 drawPos2 = Projectile.Center + sinusoid - Main.screenPosition;
				for(int i = 0; i < 6; i++)
				{
					Vector2 circular = new Vector2(4f, 0).RotatedBy(MathHelper.ToRadians(SOTSWorld.GlobalCounter * 3f + 60 * i));
					Main.spriteBatch.Draw(texture, drawPos2 + circular, null, Projectile.GetAlpha(c), Projectile.rotation + rotation, drawOrigin1, Projectile.scale, SpriteEffects.None, 0f);
                }
                Main.spriteBatch.Draw(texture, drawPos2, null, Projectile.GetAlpha(Color.White), Projectile.rotation + rotation, drawOrigin1, Projectile.scale, SpriteEffects.None, 0f);
            }
        }
        public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(ColorHelper.SoulLootingColor.R, ColorHelper.SoulLootingColor.G, ColorHelper.SoulLootingColor.B) * 1f / 255f);
			Projectile.frameCounter++;																																						
			if (Projectile.frameCounter >= 5)
			{
				Projectile.frameCounter = 0;
				Projectile.frame = (Projectile.frame + 1) % 8;
			}
			Projectile.ai[1]++;
			if ((int)Projectile.ai[1] % 6 == 0)
				Projectile.alpha++;
			Projectile.velocity *= 0.95f;
			Player player = Main.player[Owner];
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			int pepperID = -1;
			int count = player.ownedProjectileCounts[Type];
			for(int i = 0; i < Main.projectile.Length; i++)
            {
				Projectile proj = Main.projectile[i];
				if(proj.type == ModContent.ProjectileType<GhostPepper>() && proj.active && proj.owner == Owner)
                {
					pepperID = proj.whoAmI;
					break;
                }
            }
			if(count > 30 || voidPlayer.lootingSouls >= voidPlayer.voidMeterMax2 - voidPlayer.VoidMinionConsumption)
            {
                --player.ownedProjectileCounts[Type];
                Projectile.Kill();
            }
			if(Main.rand.NextBool(3))
			{
				Color c = ColorHelper.SoulLootingColor;
				c.A = 0;
                PixelDust.Spawn(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextVector2Circular(3, 3), Projectile.GetAlpha(c), 8);
            }
            Collect(player, pepperID);
		}
		public void Collect(Player player, int proj)
        {
			if(proj != -1)
			{
				Projectile pepper = Main.projectile[proj];
				if (pepper.Hitbox.Intersects(Projectile.Hitbox))
                {
					Projectile.Kill();
                }
			}
			if (player.Hitbox.Intersects(Projectile.Hitbox))
			{
				Projectile.Kill();
			}
		}
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override void OnKill(int timeLeft)
		{
			int particlesR = 60;
			Player player = Main.player[Owner];
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			if (voidPlayer.lootingSouls < voidPlayer.voidMeterMax2 - voidPlayer.VoidMinionConsumption)
			{
				SOTSUtils.PlaySound(SoundID.Grab, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.3f);
				voidPlayer.lootingSouls++;
				voidPlayer.SendClientChanges(voidPlayer);
				var index = CombatText.NewText(Projectile.Hitbox, ColorHelper.SoulLootingColor.MultiplyRGB(Color.White), 1);
				if (Main.netMode == NetmodeID.Server && index != 100)
				{
					var combatText = Main.combatText[index];
					NetMessage.SendData(MessageID.CombatTextInt, -1, -1, null, (int)combatText.color.PackedValue, combatText.position.X, combatText.position.Y, (float)1, 0, 0, 0);
				}
				particlesR = 20;
			}
			for (int i = 0; i < 360; i += particlesR)
			{
				Vector2 rotationalPos = new Vector2(6, 0).RotatedBy(MathHelper.ToRadians(i));
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(5) + rotationalPos, 22, 22, ModContent.DustType<CopyDust4>(), Alpha: Projectile.alpha, newColor: ColorHelper.SoulLootingColor * 0.75f);
				dust.noGravity = true;
                dust.velocity = dust.velocity * 0.05f + rotationalPos * 0.4f;
				dust.scale = dust.scale * 0.5f + 1f;
				dust.fadeIn = 0.1f;
			}
			base.OnKill(timeLeft);
        }
    }
}
		
			