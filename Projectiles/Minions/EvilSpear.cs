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
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
        public override void SetDefaults()
        {
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.alpha = 0;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 40;
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 100;
			Projectile.extraUpdates = 2;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = 48;
			hitbox = new Rectangle((int)Projectile.Center.X - width/2, (int)Projectile.Center.Y - width/2, width, width);
            base.ModifyDamageHitbox(ref hitbox);
		}
		public void DrawTrail()
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Color color2 = Color.Black;
			for (int i = 0; i < 2; i++)
			{
				for (int k = 0; k < Projectile.oldPos.Length; k++)
				{
					float scale = ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
					Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
					Color color = Projectile.GetAlpha(color2) * scale;
					Main.spriteBatch.Draw(texture, drawPos, null, color * 0.5f, Projectile.rotation, drawOrigin, Projectile.scale * scale, SpriteEffects.None, 0f);
				}
				color2 = ColorHelpers.EvilColor * 1f;
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Color color = ColorHelpers.EvilColor;
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
				Projectile.scale += 0.05f;
			else 
				Projectile.scale = 1f;
			if(Projectile.timeLeft < 9)
            {
				Projectile.alpha += 25;
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.velocity += Projectile.velocity.SafeNormalize(Vector2.Zero) * 0.05f;
			Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.RainbowMk2, Main.rand.NextVector2Circular(1, 1));
			dust.velocity *= 0.1f;
			dust.velocity -= Projectile.velocity * 0.05f;
			dust.color = ColorHelpers.EvilColor * 1.5f;
			dust.color.A = 160;
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 1.35f;
			int target = (int)Projectile.ai[0];
			if (target >= 0 && Projectile.ai[1] != -1)
			{
				NPC npc = Main.npc[target];
				if (npc.CanBeChasedBy())
				{
					Vector2 toNPC = npc.Center - Projectile.Center;
					Projectile.velocity = Vector2.Lerp(Projectile.velocity, toNPC.SafeNormalize(Vector2.Zero) * (Projectile.velocity.Length() + 2.5f), 0.066f);
				}
			}
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Projectile.ai[1] = -1;
			Projectile.netUpdate = true;
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
				int dust2 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, DustID.RainbowMk2);
				Dust dust = Main.dust[dust2];
				dust.velocity = circularLocation * 0.4f;
				dust.velocity += Projectile.velocity * 0.2f;
				dust.color = ColorHelpers.EvilColor * 1.5f;
				dust.color.A = 150;
				dust.noGravity = true;
				dust.alpha = 60;
				dust.fadeIn = 0.1f;
				dust.scale *= 2.25f;
			}
		}
	}
}