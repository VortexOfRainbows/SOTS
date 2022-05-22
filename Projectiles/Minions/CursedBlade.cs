using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;
using SOTS.Projectiles.Pyramid;
using Microsoft.Xna.Framework.Graphics;
using SOTS.NPCs.Boss.Curse;
using Terraria.Audio;

namespace SOTS.Projectiles.Minions
{    
    public class CursedBlade : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Blade");
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
		}
		public sealed override void SetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			//Projectile.minion = true;
			//Projectile.minionSlots = 0f;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.ignoreWater = true;
			Projectile.localNPCHitCooldown = 10;
		}
        public override bool CanHitPlayer(Player target)
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Minions/CursedBladeHilt");
			Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Minions/CursedBladePart");
			Texture2D texture3 = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Minions/CursedBladeEnd");
			Vector2 drawPos = Projectile.Center- Main.screenPosition;
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			int length = (int)(13f * (1 - Projectile.ai[0] / attackTimerMax));
			float rotation = Projectile.rotation;
			for (int i = 0; i < length + 1; i++)
			{
				Color color1 = lightColor;
				if(i != length)
				{
					Vector2 toProj2 = new Vector2(4 + i * 2, 0).RotatedBy(rotation);
					Main.spriteBatch.Draw(texture2, Projectile.Center + toProj2 - Main.screenPosition, null, color1, rotation + MathHelper.Pi / 4, new Vector2(4, 4), Projectile.scale, SpriteEffects.None, 0f);
				}
				else
				{
					Vector2 toProj2 = new Vector2(5 + i * 2, 0).RotatedBy(rotation);
					Main.spriteBatch.Draw(texture3, Projectile.Center + toProj2 - Main.screenPosition, null, color1, rotation + MathHelper.Pi / 4, new Vector2(6, 6), Projectile.scale, SpriteEffects.None, 0f);
				}
			}
			Main.spriteBatch.Draw(texture, drawPos, null, lightColor, rotation + MathHelper.Pi / 4, origin, Projectile.scale * 1.0f, SpriteEffects.None, 0f);
			return false;
		}
		private const int attackTimerMax = 180;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return Projectile.ai[0] >= attackTimerMax + 10 || projHitbox.Intersects(targetHitbox);
        }
        public override bool? CanHitNPC(NPC target)
        {
            return Projectile.ai[0] >= attackTimerMax && target.whoAmI == targetWhoAmI;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
			target.immune[Projectile.owner] = 0;
			canAttack = false;
			Projectile.netUpdate = true;
			Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.Center, Vector2.Zero, ModContent.ProjectileType<CursedStab>(), Projectile.damage, 0, Main.myPlayer, 0, target.whoAmI);
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.alpha);
			writer.Write(canAttack);
			writer.Write(foundTarget);
			writer.Write(targetWhoAmI);
			base.SendExtraAI(writer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.alpha = reader.ReadInt32();
			canAttack = reader.ReadBoolean();
			foundTarget = reader.ReadBoolean();
			targetWhoAmI = reader.ReadInt32();
			base.ReceiveExtraAI(reader);
		}
		public override bool? CanCutTiles()
		{
			return false;
		}
		public override bool MinionContactDamage()
		{
			return true;
		}
		public void Idle()
		{
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			bool found = false;
			int ofTotal = 0;
			int total = 0;
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile proj = Main.projectile[i];
				if (Projectile.type == proj.type && proj.active && Projectile.active && proj.owner == Projectile.owner)
				{
					if (proj == Projectile)
					{
						found = true;
					}
					if (!found)
						ofTotal++;
					total++;
				}
			}
			if (Main.myPlayer == player.whoAmI)
				Projectile.ai[1] = ofTotal;
			Vector2 toLocation;
			Projectile.velocity *= 0.1f;
			toLocation.X = player.Center.X;
			toLocation.Y = player.Center.Y - 64 + Main.player[Projectile.owner].gfxOffY;
			float rotation = modPlayer.orbitalCounter + Projectile.ai[1] / total * 360f;
			Vector2 circular = new Vector2(48, 0).RotatedBy(MathHelper.ToRadians(rotation));
			circular.Y *= 0.2f;
			Vector2 goTo = toLocation + circular;
			goTo -= Projectile.Center;
			Vector2 newGoTo = goTo.SafeNormalize(Vector2.Zero);
			float dist = 9f + goTo.Length() * 0.02f;
			if (dist > goTo.Length())
				dist = goTo.Length();
			Projectile.velocity = newGoTo * dist;
			Projectile.rotation = Projectile.velocity.X * 0.04f + MathHelper.Pi/2;
		}
		bool foundTarget = false;
		int targetWhoAmI = -1;
		bool canAttack = true;
		bool canDoDashSounds = true;
		int counter = 0;
		public void DoDusts()
		{
			float dustMult = (Projectile.ai[0] / (attackTimerMax - 30));
			if (dustMult > 1)
				dustMult = 1;
			if (dustMult > 0)
            {
				float length = 38 * dustMult;
				for(float i = 0; i < length; i++)
                {
					for(int j = -1; j <= 1; j++)
					{
						if (Main.rand.NextBool(3))
						{
							float scaleMult = 0.64f + 0.36f * (1 - (i / length));
							if (j != 0)
							{
								if(i % 2 == 0)
								{
									scaleMult *= 0.8f;
									Vector2 away = new Vector2(10, (10 + i * 1.0f) * 0.4f * j).RotatedBy(Projectile.rotation);
									foamParticleList1.Add(new CurseFoam(Projectile.Center + away, new Vector2(Main.rand.NextFloat(-0.1f, 0.1f), Main.rand.NextFloat(-0.1f, 0.1f)) + away * 0.1f, (0.65f + Main.rand.NextFloat(-0.1f, 0.1f)) * scaleMult, false));
								}
							}
							else
							{
								Vector2 away = new Vector2(10 + i * 1.0f, 0).RotatedBy(Projectile.rotation);
								foamParticleList1.Add(new CurseFoam(Projectile.Center + away, new Vector2(Main.rand.NextFloat(-0.1f, 0.1f), Main.rand.NextFloat(-0.1f, 0.1f)) + away * 0.1f, (0.65f + Main.rand.NextFloat(-0.1f, 0.1f)) * scaleMult, false));
							}
						}
					}
				}
			}
        }
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);

			#region Active check
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<Buffs.MinionBuffs.CursedBlade>());
			}
			if (player.HasBuff(ModContent.BuffType<Buffs.MinionBuffs.CursedBlade>()))
			{
				Projectile.timeLeft = 6;
			}
			#endregion
			#region Find target
			float distanceFromTarget = 400f;

			// This code is required if your minion weapon has the targeting feature
			if (player.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, Projectile.Center);
				float between2 = Vector2.Distance(npc.Center, player.Center);
				if (between2 < distanceFromTarget + 240)
				{
					distanceFromTarget = between;
					foundTarget = true;
					targetWhoAmI = player.MinionAttackTargetNPC;
				}
			}
			if (!foundTarget)
			{
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.CanBeChasedBy())
					{
						float between = Vector2.Distance(npc.Center, Projectile.Center);
						float between2 = Vector2.Distance(npc.Center, player.Center);
						bool inRange = between < distanceFromTarget;
						if (inRange && between2 < distanceFromTarget * 2f)
						{
							distanceFromTarget = between;
							foundTarget = true;
							targetWhoAmI = i;
						}
					}
				}
			}
			#endregion

			#region Movement
			Vector2 idlePosition = player.Center;
			idlePosition.Y -= 96f;
			float speed = 9f;
			if (foundTarget && canAttack && targetWhoAmI != -1 && Main.npc[targetWhoAmI].CanBeChasedBy())
			{
				NPC npc = Main.npc[targetWhoAmI];
				float Twidth = (float)Math.Sqrt(npc.height * npc.width);
				Vector2 goTo = npc.Center - Projectile.Center;
				speed += goTo.Length() * 0.004f;
				float sinM = (float)Math.Sin(MathHelper.ToRadians(Projectile.ai[0] - 35)) * 64;
				if (Projectile.ai[0] > attackTimerMax)
				{
					sinM = -(72 + Twidth);
					speed *= 3;
				}
				else if (Projectile.ai[0] > attackTimerMax - 10)
				{
					if (canDoDashSounds)
					{
						Terraria.Audio.SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 92, 0.7f, 0.25f);
					}
					canDoDashSounds = false;
				}
				float distance = goTo.Length() - (90 + Twidth + sinM);
				if (distance < 4 || Projectile.ai[0] > 10)
				{
					Projectile.ai[0] += 5;
				}
				goTo = goTo.SafeNormalize(Vector2.Zero);
				if (speed > distance)
				{
					speed = distance;
				}
				goTo *= speed;
				Projectile.velocity = goTo;
				Projectile.rotation = (npc.Center - Projectile.Center).ToRotation();
				if(Projectile.ai[0] > attackTimerMax + 10)
                {
					Projectile.ai[0] = attackTimerMax + 10;
				}
			}
			else
			{
				canDoDashSounds = true;
				Projectile.hide = false;
				targetWhoAmI = -1;
				foundTarget = false;
				if (!canAttack && Projectile.ai[0] > 0)
                {
					if(Projectile.ai[0] > attackTimerMax - 10)
						Projectile.ai[0]--;
					else
						Projectile.ai[0] -= 0.6f;
				}
				else
                {
					Projectile.ai[0] = 0;
					canAttack = true;
				}
				if(Projectile.ai[0] < attackTimerMax - 5)
					Idle();
				else
					Projectile.velocity *= 0.985f;
				if (Main.myPlayer == player.whoAmI && (idlePosition - Projectile.Center).Length() > 1200f)
				{
					Projectile.position = idlePosition;
					Projectile.velocity *= 0.1f;
					Projectile.netUpdate = true;
				}
			}
			#endregion
			if (canAttack || Projectile.ai[0] >= attackTimerMax - 5)
				DoDusts();
			Lighting.AddLight(Projectile.Center, 0.55f * ((255 - Projectile.alpha) / 255f), 0.1f * ((255 - Projectile.alpha) / 255f), 0.25f * ((255 - Projectile.alpha) / 255f));

			if (Main.myPlayer == player.whoAmI)
			{
				counter++;
				if(counter % 30 == 0)
					Projectile.netUpdate = true;
			}
			catalogueParticles();
		}
		public List<CurseFoam> foamParticleList1 = new List<CurseFoam>();
		public void catalogueParticles()
		{
			for (int i = 0; i < foamParticleList1.Count; i++)
			{
				CurseFoam particle = foamParticleList1[i];
				particle.Update();
				if (!particle.active)
				{
					particle = null;
					foamParticleList1.RemoveAt(i);
					i--;
				}
				else
				{
					particle.Update();
					if (!particle.active)
					{
						particle = null;
						foamParticleList1.RemoveAt(i);
						i--;
					}
					else if (!particle.noMovement)
						particle.position += Projectile.velocity * 0.975f;
				}
			}
		}
	}
}
		