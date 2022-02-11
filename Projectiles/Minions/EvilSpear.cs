using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Minions
{    
    public class EvilSpear : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Umbra Spear");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
        public override void SetDefaults()
        {
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.alpha = 0;
			projectile.width = 24;
			projectile.height = 24;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 40;
			projectile.alpha = 255;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 100;
			projectile.extraUpdates = 2;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = 48;
			hitbox = new Rectangle((int)projectile.Center.X - width/2, (int)projectile.Center.Y - width/2, width, width);
            base.ModifyDamageHitbox(ref hitbox);
		}
		public void DrawTrail(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Color color2 = Color.Black;
			for (int i = 0; i < 2; i++)
			{
				for (int k = 0; k < projectile.oldPos.Length; k++)
				{
					float scale = ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
					Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
					Color color = projectile.GetAlpha(color2) * scale;
					spriteBatch.Draw(texture, drawPos, null, color * 0.5f, projectile.rotation, drawOrigin, projectile.scale * scale, SpriteEffects.None, 0f);
				}
				color2 = VoidPlayer.EvilColor * 1f;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Color color = VoidPlayer.EvilColor;
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
				Main.PlaySound(SoundLoader.customSoundType, (int)projectile.Center.X, (int)projectile.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Items/StarLaser"), 0.6f, 0.2f + Main.rand.NextFloat(-0.1f, 0.1f));
				DustOut();
				projectile.scale = 0.1f;
				projectile.alpha = 0;
				runOnce = false;
			}
			else if (projectile.scale < 1f)
				projectile.scale += 0.05f;
			else 
				projectile.scale = 1f;
			if(projectile.timeLeft < 9)
            {
				projectile.alpha += 25;
			}
			projectile.rotation = projectile.velocity.ToRotation();
			projectile.velocity += projectile.velocity.SafeNormalize(Vector2.Zero) * 0.05f;
			Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.RainbowMk2, Main.rand.NextVector2Circular(1, 1));
			dust.velocity *= 0.1f;
			dust.velocity -= projectile.velocity * 0.05f;
			dust.color = VoidPlayer.EvilColor * 1.5f;
			dust.color.A = 160;
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 1.35f;
			int target = (int)projectile.ai[0];
			if (target >= 0 && projectile.ai[1] != -1)
			{
				NPC npc = Main.npc[target];
				if (npc.CanBeChasedBy())
				{
					Vector2 toNPC = npc.Center - projectile.Center;
					projectile.velocity = Vector2.Lerp(projectile.velocity, toNPC.SafeNormalize(Vector2.Zero) * (projectile.velocity.Length() + 2.5f), 0.066f);
				}
			}
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			projectile.ai[1] = -1;
			projectile.netUpdate = true;
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
				int dust2 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, DustID.RainbowMk2);
				Dust dust = Main.dust[dust2];
				dust.velocity = circularLocation * 0.4f;
				dust.velocity += projectile.velocity * 0.2f;
				dust.color = VoidPlayer.EvilColor * 1.5f;
				dust.color.A = 150;
				dust.noGravity = true;
				dust.alpha = 60;
				dust.fadeIn = 0.1f;
				dust.scale *= 2.25f;
			}
		}
	}
}