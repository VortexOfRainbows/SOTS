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
			writer.Write(Projectile.friendly);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			Projectile.friendly = reader.ReadBoolean();
        }
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Arrow");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
        public override void SetDefaults()
        {
			Projectile.penetrate = -3;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.alpha = 0;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 360;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 2;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = 24;
			hitbox = new Rectangle((int)Projectile.Center.X - width/2, (int)Projectile.Center.Y - width/2, width, width);
            base.ModifyDamageHitbox(ref hitbox);
		}
		public void DrawTrail()
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Chaos/SupernovaLaser");
			Vector2 drawOrigin = new Vector2(0, texture.Height * 0.5f);
			Vector2 original = Projectile.Center;
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				float scale = ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Color color2 = VoidPlayer.ChaosPink;
				color2.A = 0;
				Color color = Projectile.GetAlpha(color2) * scale;
				Vector2 drawPos = Projectile.oldPos[k] + new Vector2(Projectile.width / 2, Projectile.height / 2);
				Vector2 towards = original - drawPos;
				float lengthTowards = towards.Length() / texture.Width;
				if(Projectile.oldPos[k] != Projectile.position && Projectile.oldPos[k].X > 0 && Projectile.oldPos[k].Y > 0)
				{
					for (int i = -1; i <= 1; i++)
					{
						Vector2 offset = new Vector2(0, 1 * i).RotatedBy(towards.ToRotation());
						Main.spriteBatch.Draw(texture, offset + drawPos - Main.screenPosition, null, color * 0.6f, towards.ToRotation(), drawOrigin, new Vector2(lengthTowards, scale * Projectile.scale * 0.8f), SpriteEffects.None, 0f);
					}
                }
				original = drawPos;
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(VoidPlayer.soulColorCounter * 6 + Projectile.whoAmI * 18));
			color.A = 0;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			DrawTrail();
			if(Projectile.friendly)
				for (int k = 0; k < 4; k++)
				{
					Vector2 offset = new Vector2(2, 0).RotatedBy(MathHelper.PiOver2 * k + Projectile.velocity.ToRotation());
					Main.spriteBatch.Draw(texture, offset + Projectile.Center - Main.screenPosition + Main.rand.NextVector2Circular(1, 1), null, color * 0.7f, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
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
			if (Projectile.timeLeft <= 30)
				Projectile.friendly = false;
			if (!Projectile.friendly)
			{
				if(Projectile.timeLeft > 30)
                {
					DustOut();
					Projectile.timeLeft = 30;
                }
				Projectile.velocity *= 0;
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			if(Projectile.penetrate >= -1)
			{
				Projectile.friendly = false;
				Projectile.netUpdate = true;
			}	
			else
            {
				Projectile.penetrate++;
            }
        }
        public void DustOut()
        {
			for (int i = 0; i < 4; i++)
			{
				Vector2 circularLocation = Main.rand.NextVector2Circular(4, 4);
				int dust2 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<Dusts.CopyDust4>());
				Dust dust = Main.dust[dust2];
				dust.velocity = circularLocation * 0.4f;
				dust.velocity += Projectile.velocity * 0.2f;
				dust.color = VoidPlayer.ChaosPink;
				dust.noGravity = true;
				dust.alpha = 60;
				dust.fadeIn = 0.1f;
				dust.scale *= 2.25f;
			}
		}
	}
}