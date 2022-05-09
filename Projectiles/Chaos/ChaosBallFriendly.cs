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
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
        public override void SetDefaults()
        {
			Projectile.penetrate = 1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.ranged = true;
			Projectile.alpha = 0;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 480;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 1;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = 20;
			hitbox = new Rectangle((int)Projectile.Center.X - width/2, (int)Projectile.Center.Y - width/2, width, width);
            base.ModifyDamageHitbox(ref hitbox);
		}
		public void DrawTrail(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Color blendColor = Color.White;
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Color color2 = VoidPlayer.pastelAttempt(MathHelper.ToRadians((VoidPlayer.soulColorCounter + k) * 6 + Projectile.whoAmI * 18), blendColor);
				color2.A = 0;
				float scale = ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Vector2 drawPos = Projectile.oldPos[k] + new Vector2(12, 12) - Main.screenPosition;
				Color color = Projectile.GetAlpha(color2) * scale;
				spriteBatch.Draw(texture, drawPos, null, color * 0.5f, Projectile.rotation, drawOrigin, Projectile.scale * scale, SpriteEffects.None, 0f);
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Color blendColor = Color.White;
			Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(VoidPlayer.soulColorCounter * 6 + Projectile.whoAmI * 18), blendColor);
			color.A = 0;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			DrawTrail(spriteBatch, lightColor);
			for (int k = 0; k < 5; k++)
			{
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + Main.rand.NextVector2Circular(1, 1), null, Projectile.GetAlpha(color), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
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
				SoundEngine.PlaySound(SoundLoader.customSoundType, (int)Projectile.Center.X, (int)Projectile.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Items/StarLaser"), 0.6f, 0.2f + Main.rand.NextFloat(-0.1f, 0.1f));
				DustOut();
				Projectile.scale = 0.1f;
				Projectile.alpha = 0;
				runOnce = false;
			}
			else if (Projectile.scale < 1f)
				Projectile.scale += 0.1f;
			else 
				Projectile.scale = 1f;
			if(Projectile.timeLeft < 13)
            {
				Projectile.alpha += 20;
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.velocity += Projectile.velocity.SafeNormalize(Vector2.Zero) * 0.05f;
			float homingRange = (float)(180 + 64 * Math.Sqrt(counter));
			if (homingRange > 640)
				homingRange = 640;
			int target = SOTSNPCs.FindTarget_Basic(Projectile.Center, homingRange, this);
			if (target >= 0)
			{
				NPC npc = Main.npc[target];
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * (Projectile.velocity.Length() + 3), 0.06f);
			}
		}
        public override void Kill(int timeLeft)
		{
			if(Main.myPlayer == Projectile.owner)
			{
				for(int i = 0; i < 6; i++)
                {
					Projectile.NewProjectile(Projectile.Center, Main.rand.NextVector2CircularEdge(3, 3), ModContent.ProjectileType<FriendlyChaosEraser>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
				}
			}
		}
		public void DustOut()
		{
			Color blendColor = Color.White;
			for (int i = 0; i < 360; i += 45)
			{
				Vector2 circularLocation = new Vector2(Main.rand.NextFloat(5), 0).RotatedBy(MathHelper.ToRadians(i) + Projectile.rotation);
				int dust2 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<Dusts.CopyDust4>());
				Dust dust = Main.dust[dust2];
				dust.velocity = circularLocation * 0.7f;
				dust.velocity += Projectile.velocity * 0.7f;
				dust.color = VoidPlayer.pastelAttempt(Main.rand.NextFloat(6.28f), blendColor);
				dust.noGravity = true;
				dust.alpha = 60;
				dust.fadeIn = 0.1f;
				dust.scale *= 2.4f;
			}
		}
	}
}