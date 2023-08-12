using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs.Debuffs;
using SOTS.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Camera
{
	public class DreamLaser : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			ProjectileID.Sets.DrawScreenCheckFluff[Type] = 1600;
		}
		public override void SetDefaults() 
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.timeLeft = 40;
			Projectile.DamageType = ModContent.GetInstance<Void.VoidMagic>();
			Projectile.penetrate = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.localNPCHitCooldown = 45;
			Projectile.usesLocalNPCImmunity = true;
		}
		bool runOnce = true;
		Color color;
		float scale = 0.8f;
		public const float length = 7f;
		public override bool PreAI() 
		{
			if(runOnce)
			{
				color = DreamingFrame.Green1;
				SetPostitions();
				runOnce = false;
				NPC target = Main.npc[Math.Abs((int)Projectile.ai[0])];
				if (Projectile.ai[0] < 0)
				{
					for(int i = 0; i< 20; i++)
                    {
						Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5) - Projectile.velocity.SafeNormalize(Vector2.Zero) * 8, 0, 0, ModContent.DustType<CopyDust4>());
						dust.fadeIn = 0.2f;
						dust.noGravity = true;
						dust.alpha = 100;
						dust.color = color;
						dust.scale *= 1.35f;
						dust.velocity *= 1.2f;
						dust.velocity += Projectile.velocity.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1, 10);
					}
					if (DreamingSmog.isNPCValidTarget(target))
					{//So this only comes from the right/click effect
						target.AddBuff(ModContent.BuffType<DendroChain>(), DendroChainNPCOperators.DendroChainStandardDuration + Projectile.damage / 2);
					}
				}
				return true;
            }
			return true;
		}
        public override void AI()
        {
			if (Projectile.alpha > 0 && Projectile.timeLeft > 29)
			{
				Projectile.alpha -= 28;
			}
			else if (Projectile.alpha < 255 && Projectile.timeLeft < 28)
				Projectile.alpha += 9;
			Projectile.alpha = Math.Clamp(Projectile.alpha, 0, 255);
		}
		//bool collided = false;
        public void SetPostitions()
        {
			float speed = length * scale;
			Vector2 direction = new Vector2(speed, 0).RotatedBy(Projectile.velocity.ToRotation());
			int maxDist = (int)(Projectile.ai[1] / speed);
			Vector2 currentPos = Projectile.Center;
			int k = 0;
			while (maxDist > 0)
			{
				k++;
				posList.Add(currentPos);
				currentPos += direction;
				float size = 1f;
				int rate = 5;
				if (Projectile.ai[0] < 0)
                {
					size = 1.25f;
					rate = 2;
				}
				if (Main.rand.NextBool(rate))
				{
					Dust dust = Dust.NewDustDirect(posList[posList.Count - 1] - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
					dust.fadeIn = 0.2f;
					dust.noGravity = true;
					dust.alpha = 100;
					dust.color = color;
					dust.scale *= 1.3f * size;
					dust.velocity *= 1.6f;
					dust.velocity += direction * size;
				}
				maxDist--;
			}
			SOTSProjectile.DustStar(currentPos, direction.SafeNormalize(Vector2.Zero) * 3.5f, DreamingFrame.Green1 * 0.7f, 0f, 48, 0, 4, 7.25f, 4f, 1f, 0.9f, 0.1f);
		}
        public override bool? CanHitNPC(NPC target)
        {
            return target.whoAmI == Math.Abs((int)Projectile.ai[0]) && Projectile.friendly && (Projectile.timeLeft == 31 || (Projectile.ai[0] < 0 && Projectile.timeLeft == 38));
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			return true;
		}
		List<Vector2> posList = new List<Vector2>();
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
			crit = false; //This projectile will only have an owner in singleplayer, due to the nature of spawning projectiles from NPC death.
        }
        public override bool PreDraw(ref Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin = new Vector2(texture.Width/2, texture.Height/2);
			float alpha;
			Vector2 lastPosition = Projectile.Center;
			for(int i = 0; i < posList.Count; i++)
			{
				Vector2 drawPos = posList[i];
				float sizeMult = (float)Math.Sin(MathHelper.ToRadians(180f * (i + 0.5f) / posList.Count));
				alpha = (float)Math.Clamp(Math.Sqrt(sizeMult), 0, 1);
				Vector2 direction = drawPos - lastPosition;
				lastPosition = drawPos;
				float rotation = i == 0 ? Projectile.velocity.ToRotation() : direction.ToRotation();
				float alphaMult = ((255 - Projectile.alpha) / 255f);
				for (int j = 0; j < 3; j++)
				{
					Vector2 sinusoid = new Vector2(0, alpha * scale * (4 + 10 * alphaMult) * (float)Math.Sin(MathHelper.ToRadians(i * 2 + SOTSWorld.GlobalCounter * 2 + j * 120))).RotatedBy(rotation);
					Color color = this.color * alphaMult * alpha * 0.2f;
					color.A = 0;
					Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition + sinusoid, null, color, rotation, origin, new Vector2(scale * 2, scale * 0.5f * (0.5f + 0.5f * alphaMult)), SpriteEffects.None, 0f);
				}
			}
			return false;
		}
    }
}