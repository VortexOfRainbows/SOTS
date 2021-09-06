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
			DisplayName.SetDefault("Gas Slash");
		}
        public override void SetDefaults()
        {
			projectile.width = 40;
			projectile.height = 40;
			projectile.friendly = true;
			projectile.timeLeft = 75;
			projectile.hostile = false;
			projectile.alpha = 255;
			projectile.penetrate = -1;
			projectile.netImportant = true;
			projectile.tileCollide = false;
			projectile.hide = true;
			projectile.localNPCHitCooldown = 3;
			projectile.usesLocalNPCImmunity = true;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			if(projectile.timeLeft <= 5)
            {
				int width = 160;
				hitbox = new Rectangle((int)projectile.Center.X - width / 2, (int)projectile.Center.Y - width / 2, width, width);
            }
            base.ModifyDamageHitbox(ref hitbox);
        }
        public override bool? CanHitNPC(NPC target)
        {
			bool canHit = target.whoAmI == (int)projectile.ai[1] || projectile.timeLeft <= 5;
			return canHit ? base.CanHitNPC(target) : false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[projectile.owner];
			target.immune[projectile.owner] = 0;
			if (target.life <= 0 && target.whoAmI == (int)projectile.ai[1])
            {
				projectile.ai[1] = -1;
				projectile.netUpdate = true;
            }
		}
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			int life = target.life - (damage / 2 - (target.defense + 1) / 2);
			if (life > 0 && projectile.timeLeft > 5)
			{
				damage = (int)(damage * 0.5f);
				crit = false;
			}
			else
				crit = true;
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
        bool runOnce = true;
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
		int counter = 0;
        public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (projectile.ai[1] < 0 || projectile.timeLeft <= 3)
			{
				if (projectile.timeLeft > 2)
				{
					projectile.timeLeft = 2;
					projectile.friendly = true;
				}
				return false;
            }
			NPC target = Main.npc[(int)projectile.ai[1]];
			if(target.CanBeChasedBy())
			{
				projectile.Center = target.Center;
			}
			else
			{
				if(projectile.timeLeft > 2)
				{
					projectile.timeLeft = 2;
					projectile.friendly = true;
				}
				return false;
			}
			projectile.ai[0]++;
			if(projectile.ai[0] % 7 == 0)
            {
				counter++;
				float currentDegrees = projectile.ai[0] * 3f;
				projectile.friendly = true;
				if (modPlayer.foamParticleList1 != null)
				{
					int direction = ((projectile.whoAmI + counter) % 2 * 2 - 1);
					Vector2 circular = new Vector2(0, (int)Math.Sqrt(target.width * target.height) * 0.36f + 56).RotatedBy(MathHelper.ToRadians(currentDegrees * direction + Main.rand.NextFloat(-25f, 25f)));
					Vector2 spawnLoc = circular + projectile.Center + target.velocity * 0.5f;
					Vector2 velocity = -circular * 0.21f + target.velocity * 0.1f;
					for (float i = 0; i < 1; i += 0.02f)
					{
						float sin = (float)Math.Sin(i * MathHelper.Pi);
						float scaleMult = 0.5f + 0.8f * sin;
						modPlayer.foamParticleList1.Add(new CurseFoam(spawnLoc + velocity * 0.1f * i, new Vector2(Main.rand.NextFloat(-0.1f, 0.1f), Main.rand.NextFloat(-0.1f, 0.1f)) + velocity * i, 0.75f * scaleMult, true));
					}
				}
				Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 71, 0.7f, 1.1f);
			}
			else
            {
				projectile.friendly = false;
            }
			if (runOnce)
			{
				runOnce = false;
			}
			return base.PreAI();
        }
        public override void Kill(int timeLeft)
		{
			Player player = Main.player[projectile.owner];
			if(player.active)
			{
				SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
				for(int i = 0; i < 3; i++)
				{
					float mult2 = 1 - i * 0.1f;
					for (int j = 0; j < 30; j++)
					{
						float mult = Main.rand.NextFloat(0.877f, 1.33f) * mult2;
						Vector2 rotational = new Vector2(0, -Main.rand.NextFloat(3.5f, 12f) * mult).RotatedBy(MathHelper.ToRadians(j * 12f)) + projectile.velocity * 0.5f;
						modPlayer.foamParticleList1.Add(new CurseFoam(projectile.Center, rotational, 1.25f / mult, true));
					}
				}
			}
			Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 62, 0.9f, 0.4f);
		}
	}
}
		