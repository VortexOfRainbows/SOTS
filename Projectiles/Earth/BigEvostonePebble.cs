using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Projectiles.Blades;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Earth
{    
    public class BigEvostonePebble : ModProjectile 
    {	
        public override void SetDefaults()
        {
			Projectile.tileCollide = true;
			Projectile.width = 22;
			Projectile.height = 22;
            Projectile.DamageType = ModContent.GetInstance<VoidRanged>();
			Projectile.penetrate = -1;
			Projectile.alpha = 0; 
			Projectile.friendly = true;
			Projectile.timeLeft = 100;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 20;
			Projectile.extraUpdates = 1;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = -oldVelocity.Y;
			}
			Projectile.netUpdate = true;
			return false;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			hitbox.X = (int)Projectile.Center.X;
			hitbox.Y = (int)Projectile.Center.Y;
			hitbox.Width = 32;
			hitbox.Height = 32;
			hitbox.X -= 16;
			hitbox.Y -= 16;
		}
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = ModContent.Request<Texture2D>("SOTS/Projectiles/Earth/BigEvostonePebbleGlow").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			for (int j = 1; j <= 2; j++)
			{
				float scaleMult = 1.25f - j * 0.25f;
				Main.spriteBatch.Draw(texture, drawPos, null, color * (1f / j) * 0.5f, Projectile.rotation, drawOrigin, Projectile.scale * scaleMult, SpriteEffects.None, 0f);
			}
			return true;
		}
		public Color getColor => Color.Lerp(new Color(166, 221, 145), new Color(46, 63, 77), Main.rand.NextFloat(1));
		public override void AI()
        {
			Projectile.spriteDirection = 1;
			Projectile.rotation = Projectile.velocity.ToRotation();
			if(!Main.rand.NextBool(3))
			{
				int num1 = Dust.NewDust(Projectile.Center - new Vector2(10, 10), 12, 12, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[num1];
				Color color2 = getColor;
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale = 0.3f * dust.scale + 0.9f;
				dust.alpha = Projectile.alpha;
				dust.velocity *= 0.4f;
			}
			Projectile.velocity.Y += 0.1f;
			if(Projectile.timeLeft < Projectile.ai[0])
            {
				Projectile.Kill();
            }
		}
		public override void OnKill(int timeLeft)
		{
			for(int i = 0; i < 9; i++)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[num1];
				Color color2 = getColor;
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale = dust.scale * 0.5f + 0.8f;
				dust.alpha = Projectile.alpha;
				dust.velocity *= 1.4f;
			}
			for(int i = 0; i < 12; i++)
            {
				Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Obsidian);
				dust.color = Color.LightGray;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale = dust.scale * 0.5f + 0.6f;
				dust.alpha = Projectile.alpha;
				dust.velocity *= 0.9f;
			}
			if(Main.myPlayer == Projectile.owner)
            {
				for(int i = 0; i < 3; i++)
                {
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2Circular(4.5f, 3) + new Vector2(0, -2), ModContent.ProjectileType<EvostonePebble>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
                }
            }
			SOTSUtils.PlaySound(SoundID.Item62, Projectile.Center, 0.5f, 0.35f, 0.05f);
		}
	}
}