using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Chaos
{    
    public class ChaosArrow1 : ModProjectile 
    {
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(projectile.friendly);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			projectile.friendly = reader.ReadBoolean();
        }
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Arrow");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 30;
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
			projectile.timeLeft = 360;
			projectile.alpha = 255;
			projectile.extraUpdates = 2;
			projectile.ranged = true;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 30;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = 24;
			hitbox = new Rectangle((int)projectile.Center.X - width/2, (int)projectile.Center.Y - width/2, width, width);
            base.ModifyDamageHitbox(ref hitbox);
		}
		public void DrawTrail(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Chaos/SupernovaLaser");
			Vector2 drawOrigin = new Vector2(0, texture.Height * 0.5f);
			Vector2 original = projectile.Center;
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				float scale = ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				Color color2 = VoidPlayer.ChaosPink;
				color2.A = 0;
				Color color = projectile.GetAlpha(color2) * scale;
				Vector2 drawPos = projectile.oldPos[k] + new Vector2(projectile.width / 2, projectile.height / 2);
				Vector2 towards = original - drawPos;
				float lengthTowards = towards.Length() / texture.Width;
				if(projectile.oldPos[k] != projectile.position && projectile.oldPos[k].X > 0 && projectile.oldPos[k].Y > 0)
				{
					for (int i = -1; i <= 1; i++)
					{
						Vector2 offset = new Vector2(0, 1 * i).RotatedBy(towards.ToRotation());
						spriteBatch.Draw(texture, offset + drawPos - Main.screenPosition, null, color * 0.6f, towards.ToRotation(), drawOrigin, new Vector2(lengthTowards, scale * projectile.scale * 0.8f), SpriteEffects.None, 0f);
					}
                }
				original = drawPos;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(VoidPlayer.soulColorCounter * 6 + projectile.whoAmI * 18));
			color.A = 0;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			DrawTrail(spriteBatch, lightColor);
			if(projectile.friendly)
				for (int k = 0; k < 4; k++)
				{
					Vector2 offset = new Vector2(2, 0).RotatedBy(MathHelper.PiOver2 * k + projectile.velocity.ToRotation());
					Main.spriteBatch.Draw(texture, offset + projectile.Center - Main.screenPosition + Main.rand.NextVector2Circular(1, 1), null, color * 0.7f, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
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
				projectile.scale += 0.1f;
			else 
				projectile.scale = 1f;
			if(projectile.timeLeft < 9)
            {
				projectile.alpha += 25;
			}
			if (projectile.timeLeft <= 30)
				projectile.friendly = false;
			if (!projectile.friendly)
			{
				if(projectile.timeLeft > 30)
                {
					DustOut();
					projectile.timeLeft = 30;
                }
				projectile.velocity *= 0;
			}
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			projectile.friendly = false;
			projectile.netUpdate = true;
        }
        public void DustOut()
        {
			for (int i = 0; i < 4; i++)
			{
				Vector2 circularLocation = Main.rand.NextVector2Circular(4, 4);
				int dust2 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<Dusts.CopyDust4>());
				Dust dust = Main.dust[dust2];
				dust.velocity = circularLocation * 0.4f;
				dust.velocity += projectile.velocity * 0.2f;
				dust.color = VoidPlayer.ChaosPink;
				dust.noGravity = true;
				dust.alpha = 60;
				dust.fadeIn = 0.1f;
				dust.scale *= 2.25f;
			}
		}
	}
}