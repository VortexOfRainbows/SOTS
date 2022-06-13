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
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
        public override void SetDefaults()
        {
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.alpha = 0;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 360;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 1;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = 24;
			hitbox = new Rectangle((int)Projectile.Center.X - width/2, (int)Projectile.Center.Y - width/2, width, width);
            base.ModifyDamageHitbox(ref hitbox);
		}
		public void DrawTrail()
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Color color2 = VoidPlayer.pastelAttempt(MathHelper.ToRadians((VoidPlayer.soulColorCounter + k) * 6 + Projectile.whoAmI * 18));
				color2.A = 0;
				float scale = ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + new Vector2(12, 12);
				Color color = Projectile.GetAlpha(color2) * scale;
				Main.spriteBatch.Draw(texture, drawPos, null, color * 0.5f, Projectile.rotation, drawOrigin, Projectile.scale * scale, SpriteEffects.None, 0f);
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(VoidPlayer.soulColorCounter * 6 + Projectile.whoAmI * 18));
			color.A = 0;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			DrawTrail();
			for (int k = 0; k < 5; k++)
			{
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + Main.rand.NextVector2Circular(1, 1), null, Projectile.GetAlpha(color), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		bool runOnce = true;
		public override void AI()
		{
			if (runOnce)
			{
				SOTSUtils.PlaySound(new Terraria.Audio.SoundStyle("SOTS/Sounds/Items/StarLaser"), (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.6f, 0.2f + Main.rand.NextFloat(-0.1f, 0.1f));
				DustOut();
				Projectile.scale = 0.1f;
				Projectile.alpha = 0;
				runOnce = false;
			}
			else if (Projectile.scale < 1f)
				Projectile.scale += 0.1f;
			else 
				Projectile.scale = 1f;
			if(Projectile.timeLeft < 9)
            {
				Projectile.alpha += 25;
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.velocity += Projectile.velocity.SafeNormalize(Vector2.Zero) * 0.04f;
			float grow = 0.5f;
			if (SOTS.Config.lowFidelityMode)
			{
				grow = 1f;
			}
			for (float i = 0; i < 1; i += grow)
			{
				Dust dust2 = Dust.NewDustPerfect(Projectile.Center - Projectile.velocity * i, ModContent.DustType<Dusts.CopyDust4>(), Main.rand.NextVector2Circular(0.2f, 0.2f));
				dust2.velocity += Projectile.velocity * 0.1f;
				dust2.noGravity = true;
				dust2.color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(VoidPlayer.soulColorCounter * 6 + Projectile.whoAmI * 18), true);
				dust2.noGravity = true;
				dust2.fadeIn = 0.2f;
				dust2.scale *= 1.7f;
			}
			int target = (int)Projectile.ai[0];
			if (target >= 0 && Projectile.ai[1] > -1)
			{
				Player player = Main.player[target];
				if (player.active && !player.dead)
				{
					Vector2 toPlayer = player.Center - Projectile.Center;
					float homingMult = 0.014f * Projectile.timeLeft / 360f - 0.004f;
					if (homingMult < 0)
						homingMult = 0;
					Projectile.velocity = Vector2.Lerp(Projectile.velocity, toPlayer.SafeNormalize(Vector2.Zero) * (Projectile.velocity.Length() + 1.2f), homingMult * (1 + Projectile.ai[1]));
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
				Vector2 circularLocation = new Vector2(Main.rand.NextFloat(4), 0).RotatedBy(MathHelper.ToRadians(i) + Projectile.rotation);
				int dust2 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<Dusts.CopyDust4>());
				Dust dust = Main.dust[dust2];
				dust.velocity = circularLocation * 0.4f;
				dust.velocity += Projectile.velocity * 0.2f;
				dust.color = VoidPlayer.pastelAttempt(Main.rand.NextFloat(6.28f), true);
				dust.noGravity = true;
				dust.alpha = 60;
				dust.fadeIn = 0.1f;
				dust.scale *= 2.25f;
			}
		}
	}
}