using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.NPCs;
using SOTS.Void;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Chaos
{    
    public class ChaosBallFriendly : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Ball");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
        public override void SetDefaults()
        {
			projectile.penetrate = 1;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.alpha = 0;
			projectile.width = 24;
			projectile.height = 24;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;
			projectile.timeLeft = 480;
			projectile.alpha = 255;
			projectile.extraUpdates = 1;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = 20;
			hitbox = new Rectangle((int)projectile.Center.X - width/2, (int)projectile.Center.Y - width/2, width, width);
            base.ModifyDamageHitbox(ref hitbox);
		}
		public void DrawTrail(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Color blendColor = Color.White;
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Color color2 = VoidPlayer.pastelAttempt(MathHelper.ToRadians((VoidPlayer.soulColorCounter + k) * 6 + projectile.whoAmI * 18), blendColor);
				color2.A = 0;
				float scale = ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				Vector2 drawPos = projectile.oldPos[k] + new Vector2(12, 12) - Main.screenPosition;
				Color color = projectile.GetAlpha(color2) * scale;
				spriteBatch.Draw(texture, drawPos, null, color * 0.5f, projectile.rotation, drawOrigin, projectile.scale * scale, SpriteEffects.None, 0f);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Color blendColor = Color.White;
			Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(VoidPlayer.soulColorCounter * 6 + projectile.whoAmI * 18), blendColor);
			color.A = 0;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			DrawTrail(spriteBatch, lightColor);
			for (int k = 0; k < 5; k++)
			{
				Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition + Main.rand.NextVector2Circular(1, 1), null, projectile.GetAlpha(color), projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		int counter = 0;
		bool runOnce = true;
		public override void AI()
		{
			counter++;
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
			if(projectile.timeLeft < 13)
            {
				projectile.alpha += 20;
			}
			projectile.rotation = projectile.velocity.ToRotation();
			projectile.velocity += projectile.velocity.SafeNormalize(Vector2.Zero) * 0.05f;
			float homingRange = (float)(180 + 64 * Math.Sqrt(counter));
			if (homingRange > 640)
				homingRange = 640;
			int target = SOTSNPCs.FindTarget_Basic(projectile.Center, homingRange, this);
			if (target >= 0)
			{
				NPC npc = Main.npc[target];
				projectile.velocity = Vector2.Lerp(projectile.velocity, (npc.Center - projectile.Center).SafeNormalize(Vector2.Zero) * (projectile.velocity.Length() + 3), 0.06f);
			}
		}
        public override void Kill(int timeLeft)
		{
			if(Main.myPlayer == projectile.owner)
			{
				for(int i = 0; i < 6; i++)
                {
					Projectile.NewProjectile(projectile.Center, Main.rand.NextVector2CircularEdge(3, 3), ModContent.ProjectileType<FriendlyChaosEraser>(), projectile.damage, projectile.knockBack, projectile.owner);
				}
			}
		}
		public void DustOut()
		{
			Color blendColor = Color.White;
			for (int i = 0; i < 360; i += 45)
			{
				Vector2 circularLocation = new Vector2(Main.rand.NextFloat(5), 0).RotatedBy(MathHelper.ToRadians(i) + projectile.rotation);
				int dust2 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<Dusts.CopyDust4>());
				Dust dust = Main.dust[dust2];
				dust.velocity = circularLocation * 0.7f;
				dust.velocity += projectile.velocity * 0.7f;
				dust.color = VoidPlayer.pastelAttempt(Main.rand.NextFloat(6.28f), blendColor);
				dust.noGravity = true;
				dust.alpha = 60;
				dust.fadeIn = 0.1f;
				dust.scale *= 2.4f;
			}
		}
	}
}