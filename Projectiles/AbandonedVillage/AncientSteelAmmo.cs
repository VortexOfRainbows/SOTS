using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using SOTS.Projectiles.Evil;

namespace SOTS.Projectiles.AbandonedVillage
{
	public abstract class AncientSteelAmmo : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 14;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = SOTSUtils.WhitePixel;
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 lastPosition = Projectile.Center;
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				float scale = 1f - 0.5f * (k / (float)Projectile.oldPos.Length);
				Vector2 drawPos = Projectile.oldPos[k] + new Vector2(Projectile.width / 2, Projectile.height / 2);
				Color color = new Color(100, 100, 100, 0) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				float lengthTowards = Vector2.Distance(lastPosition, drawPos) / texture.Height / scale;
				Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, color * scale, Projectile.rotation, drawOrigin, new Vector2(TrailScale(), lengthTowards) * scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				lastPosition = drawPos;
			}
			return true;
		}
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Lerp(lightColor, Color.White, 0.4f) * (1 - Projectile.alpha / 255f);
        }
        public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(off);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			off = reader.ReadBoolean();
		}
		public virtual float TrailScale() => 2f;
		public bool off = false;
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			//if(hit.Crit && Main.myPlayer == Projectile.owner)
			//{
                //float Direction = Projectile.velocity.ToRotation();
                //Projectile.NewProjectile(Projectile.GetSource_OnHit(target), 
					//Projectile.Center, Projectile.velocity.SNormalize() * 1, ModContent.ProjectileType<BloodSpark>(), hit.SourceDamage, hit.Knockback, Main.myPlayer, (Direction + MathHelper.PiOver2) * 180f / MathHelper.Pi, -1);
            //}
            off = true;
			Projectile.friendly = false;
			Projectile.netUpdate = true;
        }
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.friendly = false;
			off = true;
			Projectile.netUpdate = true;
			return false;
		}
		private bool runOnce = true;
		public sealed override bool PreAI()
		{
			if (off)
            {
				if (Projectile.timeLeft > 14)
					Projectile.timeLeft = 14;
				if(runOnce && Projectile.alpha < 255)
				{
					int count = (Projectile.width + Projectile.height) / 5;
                    for (int i = 0; i < count; i++)
                    {
						Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12), 16, 16, DustID.Titanium);
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale *= 1.2f;
						dust.velocity *= 0.7f;
						dust.velocity += Projectile.velocity * Main.rand.NextFloat(0.6f);
					}
					SOTSUtils.PlaySound(SoundID.Tink, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.8f, 0.4f);
					runOnce = false;
				}
				Projectile.friendly = false;
				Projectile.alpha = 255;
				Projectile.tileCollide = false;
				Projectile.velocity *= 0f;
				return false;
			}
			return true;
		}
	}

	public class AncientSteelArrow : AncientSteelAmmo
    {
        public override void SetDefaults()
		{
			Projectile.CloneDefaults(1);
			AIType = 1;
			Projectile.width = 14;
			Projectile.height = 32;
			Projectile.hide = true;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.light = 0;
			Projectile.extraUpdates = 1;
		}
        public override float TrailScale()
        {
			return 4.5f;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox = new Rectangle((int)Projectile.Center.X - 12, (int)Projectile.Center.Y - 12, 24, 24);
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = true;
			width = 12;
			height = 12;
			return true;
		}
		public override void AI()
		{
			Projectile.spriteDirection = Projectile.direction;
			Projectile.hide = false;
			Projectile.scale = 0.9f;
			Projectile.velocity.Y += 0.03f;
		}
	}
	public class AncientSteelBullet : AncientSteelAmmo
	{
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Bullet);
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.width = 10;
			Projectile.height = 24;
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.light = 0;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox = new Rectangle((int)Projectile.Center.X - 8, (int)Projectile.Center.Y - 8, 16, 16);
			base.ModifyDamageHitbox(ref hitbox);
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 8;
			height = 8;
			return true;
		}
		public override void AI()
		{
			Projectile.scale = 0.75f;
			Projectile.spriteDirection = Projectile.direction;
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
			if (Projectile.alpha > 0)
				Projectile.alpha -= 20;
			else
				Projectile.alpha = 0;
		}
	}
}
		
			