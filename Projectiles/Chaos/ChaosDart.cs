using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Chaos
{    
    public class ChaosDart : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Dart");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
        public override void SetDefaults()
        {
			projectile.penetrate = -1;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.alpha = 0;
			projectile.width = 24;
			projectile.height = 24;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 360;
			projectile.alpha = 255;
			projectile.extraUpdates = 1;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = 24;
			hitbox = new Rectangle((int)projectile.Center.X - width/2, (int)projectile.Center.Y - width/2, width, width);
            base.ModifyDamageHitbox(ref hitbox);
		}
		public void DrawTrail(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Color color2 = VoidPlayer.pastelAttempt(MathHelper.ToRadians((VoidPlayer.soulColorCounter + k) * 6 + projectile.whoAmI * 18));
				color2.A = 0;
				float scale = ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + new Vector2(12, 12);
				Color color = projectile.GetAlpha(color2) * scale;
				spriteBatch.Draw(texture, drawPos, null, color * 0.5f, projectile.rotation, drawOrigin, projectile.scale * scale, SpriteEffects.None, 0f);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(VoidPlayer.soulColorCounter * 6 + projectile.whoAmI * 18));
			color.A = 0;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			DrawTrail(spriteBatch, lightColor);
			for (int k = 0; k < 5; k++)
			{
				Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition + Main.rand.NextVector2Circular(1, 1), null, projectile.GetAlpha(color), projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		bool runOnce = true;
		public override void AI()
		{
			if (runOnce)
			{
				SoundEngine.PlaySound(SoundLoader.customSoundType, (int)projectile.Center.X, (int)projectile.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Items/StarLaser"), 0.6f, 0.2f + Main.rand.NextFloat(-0.1f, 0.1f));
				DustOut();
				projectile.scale = 0.1f;
				projectile.alpha = 0;
				runOnce = false;
			}
			else if (projectile.scale < 1f)
				projectile.scale += 0.1f;
			else 
				projectile.scale = 1f;
			if(projectile.timeLeft < 9)
            {
				projectile.alpha += 25;
			}
			projectile.rotation = projectile.velocity.ToRotation();
			projectile.velocity += projectile.velocity.SafeNormalize(Vector2.Zero) * 0.04f;
			float grow = 0.5f;
			if (SOTS.Config.lowFidelityMode)
			{
				grow = 1f;
			}
			for (float i = 0; i < 1; i += grow)
			{
				Dust dust2 = Dust.NewDustPerfect(projectile.Center - projectile.velocity * i, ModContent.DustType<Dusts.CopyDust4>(), Main.rand.NextVector2Circular(0.2f, 0.2f));
				dust2.velocity += projectile.velocity * 0.1f;
				dust2.noGravity = true;
				dust2.color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(VoidPlayer.soulColorCounter * 6 + projectile.whoAmI * 18), true);
				dust2.noGravity = true;
				dust2.fadeIn = 0.2f;
				dust2.scale *= 1.7f;
			}
			int target = (int)projectile.ai[0];
			if (target >= 0 && projectile.ai[1] > -1)
			{
				Player player = Main.player[target];
				if (player.active && !player.dead)
				{
					Vector2 toPlayer = player.Center - projectile.Center;
					float homingMult = 0.014f * projectile.timeLeft / 360f - 0.004f;
					if (homingMult < 0)
						homingMult = 0;
					projectile.velocity = Vector2.Lerp(projectile.velocity, toPlayer.SafeNormalize(Vector2.Zero) * (projectile.velocity.Length() + 1.2f), homingMult * (1 + projectile.ai[1]));
				}
			}
		}
        public override void Kill(int timeLeft)
		{
			DustOut();
		}
		public void DustOut()
        {
			for (int i = 0; i < 360; i += 40)
			{
				Vector2 circularLocation = new Vector2(Main.rand.NextFloat(4), 0).RotatedBy(MathHelper.ToRadians(i) + projectile.rotation);
				int dust2 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<Dusts.CopyDust4>());
				Dust dust = Main.dust[dust2];
				dust.velocity = circularLocation * 0.4f;
				dust.velocity += projectile.velocity * 0.2f;
				dust.color = VoidPlayer.pastelAttempt(Main.rand.NextFloat(6.28f), true);
				dust.noGravity = true;
				dust.alpha = 60;
				dust.fadeIn = 0.1f;
				dust.scale *= 2.25f;
			}
		}
	}
}