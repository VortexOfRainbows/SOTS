using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Void;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Earth
{
	
	public class PixelBlaster : ModProjectile
    {
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Earth/PixelBlasterGlow");
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Color color = Color.White;
            Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Type].Value, drawPos, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            float alphaMult = 0.025f;
            if (Projectile.ai[0] >= 20)
                alphaMult = 0.25f;
            if (Projectile.ai[0] >= 40)
                alphaMult = 0.5f;
            for (int i = 0; i < 8; i++)
            {
                Vector2 circular = new Vector2(0, 8 * alphaMult).RotatedBy(i * MathHelper.TwoPi / 8f + SOTSWorld.GlobalCounter * MathHelper.TwoPi / 180f);
                Main.spriteBatch.Draw(texture, drawPos + circular, null, new Color(110, 110, 90, 0) * alphaMult, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            }
            Main.spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            return false;
        }
		public override void SetDefaults() 
		{
			Projectile.width = 22;
			Projectile.height = 44;
            Projectile.aiStyle = 20;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.DamageType = ModContent.GetInstance<Void.VoidGeneric>();
            Projectile.timeLeft = 6;
            Projectile.hide = true;
            Projectile.alpha = 255;
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 vector2_1 = player.RotatedRelativePoint(Main.player[Projectile.owner].MountedCenter, true);
            if (Main.myPlayer == Projectile.owner)
            {
                float num1 = player.inventory[player.selectedItem].shootSpeed * Projectile.scale;
                Vector2 vector2_2 = vector2_1;
                float num2 = (float)((double)Main.mouseX + Main.screenPosition.X - vector2_2.X);
                float num3 = (float)((double)Main.mouseY + Main.screenPosition.Y - vector2_2.Y);
                if ((double)Main.player[Projectile.owner].gravDir == -1.0)
                    num3 = (float)((double)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector2_2.Y);
                float num5 = (float)Math.Sqrt(num2 * (double)num2 + num3 * (double)num3);
                float num6 = num1 / num5;
                float num7 = num2 * num6;
                float num8 = num3 * num6;

                if ((double)num7 != Projectile.velocity.X || (double)num8 != Projectile.velocity.Y)
                    Projectile.netUpdate = true;
                Projectile.velocity.X = num7;
                Projectile.velocity.Y = num8;
            }
            Projectile.hide = false;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.position.X = vector2_1.X - (Projectile.width / 2);
            Projectile.position.Y = vector2_1.Y - (Projectile.height / 2);
            Projectile.rotation = (float)(Projectile.velocity.ToRotation() + MathHelper.PiOver2);
            if (!Projectile.hide)
            {
                player.heldProj = Projectile.whoAmI;
                Projectile.alpha = 0;
                if (player.channel)
                {
                    Projectile.timeLeft = 2;
                    player.itemAnimation = 2;
                    player.itemTime = 2;
                    player.ChangeDir(Projectile.direction);
                    player.heldProj = Projectile.whoAmI;
                    player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation + MathHelper.Pi);
                    player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation + MathHelper.Pi);
                }
                else
                {
                    Projectile.Kill();
                    return false;
                }
            }
            return false;
        }
        public override void PostAI()
        {
            Player player = Main.player[Projectile.owner];
            Color color = ColorHelpers.EarthColor;
            color.A = 0;
            Vector2 gunTip = Projectile.Center + new Vector2(42, -2 * Projectile.direction).RotatedBy(Projectile.velocity.ToRotation());
            Projectile.ai[0]++;
            if (Projectile.ai[0] == 20)
            {
                SOTSUtils.PlaySound(new Terraria.Audio.SoundStyle("SOTS/Sounds/Items/PerfectStarCharge"), (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.2f, -0.5f);
                for (int i = 0; i < 12; i++)
                {
                    Vector2 circular = new Vector2(8, 0).RotatedBy(i / 12f * MathHelper.TwoPi);
                    Dust dust = Dust.NewDustDirect(gunTip - new Vector2(5, 5) + circular * 4, 0, 0, ModContent.DustType<PixelDust>(), 0, 0, 0, color, 0.8f);
                    dust.fadeIn = 9;
                    dust.velocity *= 0.2f;
                    dust.velocity += circular * -0.5f * Main.rand.NextFloat(0.8f, 1.1f) + player.velocity * 1.15f + Projectile.velocity * 0.04f;
                }
            }
            if (Projectile.ai[0] == 40)
            {
                SOTSUtils.PlaySound(new Terraria.Audio.SoundStyle("SOTS/Sounds/Items/PerfectStarCharge"), (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.2f, -0.3f);
                for (int i = 0; i < 20; i++)
                {
                    Vector2 circular = new Vector2(12, 0).RotatedBy(i / 20f * MathHelper.TwoPi);
                    Dust dust = Dust.NewDustDirect(gunTip - new Vector2(5, 5) + circular * 3, 0, 0, ModContent.DustType<PixelDust>(), 0, 0, 0, color, 1f);
                    dust.fadeIn = 9;
                    dust.velocity *= 0.2f;
                    dust.velocity += circular * -0.45f * Main.rand.NextFloat(0.8f, 1.1f) * Main.rand.NextFloat(0.8f, 1.1f) + player.velocity * 1.15f + Projectile.velocity * 0.04f;
                }
            }
            if (Projectile.ai[0] >= 60)
            {
                if (Projectile.ai[0] == 60)
                {
                    SOTSUtils.PlaySound(Terraria.ID.SoundID.Item92, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.0f, 0.4f);
                    for (int i = 0; i < 24; i++)
                    {
                        Vector2 circular = new Vector2(9, 0).RotatedBy(i / 24f * MathHelper.TwoPi);
                        Dust dust = Dust.NewDustDirect(gunTip - new Vector2(5, 5) + circular * 1 * Main.rand.NextFloat(0.1f, 1.0f), 0, 0, ModContent.DustType<PixelDust>(), 0, 0, 0, color, 1.25f);
                        dust.fadeIn = 9;
                        dust.velocity *= 0.2f;
                        dust.velocity += circular * 0.6f * Main.rand.NextFloat(0.1f, 1.0f) + player.velocity * 1.15f + Projectile.velocity * Main.rand.NextFloat(0.2f, 0.8f);
                    }
                }
                else if (Projectile.ai[0] % 20 == 0)
                    SOTSUtils.PlaySound(Terraria.ID.SoundID.Item15, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.76f, 0.3f);
                if (Main.myPlayer == Projectile.owner)
                {
                    if (Projectile.ai[0] == 60)
                    {
                        Item item = player.HeldItem;
                        VoidItem vItem = item.ModItem as VoidItem;
                        if (vItem != null)
                            vItem.DrainMana(player);
                    }
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), gunTip, Projectile.velocity, ModContent.ProjectileType<PixelLaser>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0, (Projectile.ai[0] == 60 ? -1 : 0));
                }
            }
        }
    }
}