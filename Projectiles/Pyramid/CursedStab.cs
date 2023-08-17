using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.NPCs.Boss.Curse;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Pyramid;
using System;

namespace SOTS.Projectiles.Pyramid
{    
    public class CursedStab : ModProjectile 
    {	          
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Gas Slash");
		}
        public override void SetDefaults()
        {
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.friendly = true;
			Projectile.timeLeft = 75;
			Projectile.hostile = false;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.netImportant = true;
			Projectile.tileCollide = false;
			Projectile.hide = true;
			Projectile.localNPCHitCooldown = 3;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.minion = false;
			Projectile.DamageType = ModContent.GetInstance<Void.VoidSummon>();
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			if(Projectile.timeLeft <= 5)
            {
				int width = 160;
				hitbox = new Rectangle((int)Projectile.Center.X - width / 2, (int)Projectile.Center.Y - width / 2, width, width);
            }
            base.ModifyDamageHitbox(ref hitbox);
        }
        public override bool? CanHitNPC(NPC target)
        {
			bool canHit = target.whoAmI == (int)Projectile.ai[1] || Projectile.timeLeft <= 5;
			return canHit ? base.CanHitNPC(target) : false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			target.immune[Projectile.owner] = 0;
			if (target.life <= 0 && target.whoAmI == (int)Projectile.ai[1])
            {
				Projectile.ai[1] = -1;
				Projectile.netUpdate = true;
            }
		}
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			int forceDamage = modifiers.GetDamage(Projectile.damage, false, false, 0);
			int life = target.life - forceDamage;
			if (life > 0 && Projectile.timeLeft > 5) //If the npc would still live a full damage attack
			{
				modifiers.SourceDamage *= 0.5f;
				modifiers.DisableCrit();
			}
			else //If the npc would die to the attack, make that attack a crit (for synergy purposes)
				modifiers.SetCrit();
		}
        bool runOnce = true;
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
		int counter = 0;
        public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (Projectile.ai[1] < 0 || Projectile.timeLeft <= 3)
			{
				if (Projectile.timeLeft > 2)
				{
					Projectile.timeLeft = 2;
					Projectile.friendly = true;
				}
				return false;
            }
			NPC target = Main.npc[(int)Projectile.ai[1]];
			if(target.CanBeChasedBy())
			{
				Projectile.Center = target.Center;
			}
			else
			{
				if(Projectile.timeLeft > 2)
				{
					Projectile.timeLeft = 2;
					Projectile.friendly = true;
				}
				return false;
			}
			Projectile.ai[0]++;
			if(Projectile.ai[0] % 7 == 0)
            {
				counter++;
				float currentDegrees = Projectile.ai[0] * 3f;
				Projectile.friendly = true;
				if (modPlayer.foamParticleList1 != null)
				{
					int direction = ((Projectile.whoAmI + counter) % 2 * 2 - 1);
					Vector2 circular = new Vector2(0, (int)Math.Sqrt(target.width * target.height) * 0.36f + 56).RotatedBy(MathHelper.ToRadians(currentDegrees * direction + Main.rand.NextFloat(-25f, 25f)));
					Vector2 spawnLoc = circular + Projectile.Center + target.velocity * 0.5f;
					Vector2 velocity = -circular * 0.21f + target.velocity * 0.1f;
					for (float i = 0; i < 1; i += 0.02f)
					{
						float sin = (float)Math.Sin(i * MathHelper.Pi);
						float scaleMult = 0.5f + 0.8f * sin;
						modPlayer.foamParticleList1.Add(new CurseFoam(spawnLoc + velocity * 0.1f * i, new Vector2(Main.rand.NextFloat(-0.1f, 0.1f), Main.rand.NextFloat(-0.1f, 0.1f)) + velocity * i, 0.75f * scaleMult, true));
					}
				}
				SOTSUtils.PlaySound(SoundID.Item71, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.7f, 1.1f);
			}
			else
            {
				Projectile.friendly = false;
            }
			if (runOnce)
			{
				runOnce = false;
			}
			return base.PreAI();
        }
        public override void Kill(int timeLeft)
		{
			Player player = Main.player[Projectile.owner];
			if(player.active)
			{
				SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
				for(int i = 0; i < 3; i++)
				{
					float mult2 = 1 - i * 0.1f;
					for (int j = 0; j < 30; j++)
					{
						float mult = Main.rand.NextFloat(0.877f, 1.33f) * mult2;
						Vector2 rotational = new Vector2(0, -Main.rand.NextFloat(3.5f, 12f) * mult).RotatedBy(MathHelper.ToRadians(j * 12f)) + Projectile.velocity * 0.5f;
						modPlayer.foamParticleList1.Add(new CurseFoam(Projectile.Center, rotational, 1.25f / mult, true));
					}
				}
			}
			SOTSUtils.PlaySound(SoundID.Item62, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.9f, 0.4f);
		}
	}
}
		