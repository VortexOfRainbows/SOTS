using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Projectiles.Celestial;
using System.Collections.Generic;
using SOTS.Buffs;
using SOTS.Projectiles.Planetarium;
using SOTS.Projectiles.Pyramid;
using SOTS.Buffs.MinionBuffs;

namespace SOTS.Projectiles.Inferno
{
	public class SpectralWisp : WispMinion
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Spectral Wisp");
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = false;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SafeSetDefaults()
		{
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
		}
		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.SourceDamage *= 0.75f;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Celestial/SubspaceLingeringFlame");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Color color;
			for (int i = 0; i < particleList.Count; i++)
			{
				color = new Color(60, 90, 125, 0);
				Vector2 drawPos = particleList[i].position - Main.screenPosition;
				color = Projectile.GetAlpha(color) * (0.35f + 0.65f * particleList[i].scale);
				for (int j = 0; j < 2; j++)
				{
					float x = Main.rand.NextFloat(-2f, 2f);
					float y = Main.rand.NextFloat(-2f, 2f);
					Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, particleList[i].rotation, drawOrigin, particleList[i].scale * 1.25f, SpriteEffects.None, 0f);
				}
			}
			texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Inferno/SpectralWispOutline");
			drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			color = new Color(100, 100, 100, 0);
			for (int k = 0; k < 12; k++)
			{
				Vector2 drawPos = Projectile.Center - Main.screenPosition;
				Vector2 circular = new Vector2(Main.rand.NextFloat(0, 3), 0).RotatedBy(Math.PI / 6 * k);
				Main.spriteBatch.Draw(texture, drawPos + circular, null, color * 0.9f, Projectile.rotation, drawOrigin, Projectile.scale * 1.125f, SpriteEffects.None, 0f);
			}
			drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				color = new Color(100, 100, 100, 0) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale * 0.1f + 0.70f * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0f);
			}
			float modifier = (float)Math.Sin(MathHelper.ToRadians(Projectile.ai[1] * 4)) * 1.5f;
			Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition + new Vector2(0, (int)modifier * 1.2f), null, new Color(255, 255, 255), Projectile.rotation, drawOrigin, Projectile.scale * 0.8f, SpriteEffects.None, 0f);
			return false;
		}
	}
	public abstract class WispMinion : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Spectral Wisp");
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = false;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
        public sealed override void SetDefaults()
        {
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 0;
            Projectile.netImportant = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
			Projectile.ignoreWater = true;
			Projectile.minion = false;
			Projectile.ContinuouslyUpdateDamageStats = true;
			Projectile.DamageType = ModContent.GetInstance<Void.VoidSummon>();
			SafeSetDefaults();
		}
		public virtual void SafeSetDefaults()
        {

        }
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
			target.immune[Projectile.owner] = 0;
		}
        public int FindClosestEnemy()
		{
			Player player = Main.player[Projectile.owner];
			float minDist = enemyRange;
			int target2 = -1;
			/*if (player.HasMinionAttackTargetNPC)
			{
				NPC target = Main.npc[player.MinionAttackTargetNPC];
				bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, target.position, target.width, target.height);
				dX = target.Center.X - Projectile.Center.X;
				dY = target.Center.Y - Projectile.Center.Y;
				distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
				if (distance < minDist && lineOfSight)
				{
					minDist = distance;
					target2 = player.MinionAttackTargetNPC;
				}
			}*/
			Vector2 current = Projectile.Center;
			for(int k = 0; k < 70; k++)
            {
				Vector2 goOutFromPlayer = Projectile.Center - player.Center - new Vector2(0, 2);
				goOutFromPlayer = goOutFromPlayer.SafeNormalize(Vector2.Zero);
				int length = 2 * (k + 8);
				for (int i = 0; i < Main.npc.Length; i++)
				{
					NPC target = Main.npc[i];
					if (target.CanBeChasedBy())
					{
						Rectangle hitbox = new Rectangle((int)current.X - length / 2, (int)current.Y - length / 2, length, length);
						float distance = Vector2.Distance(target.Center, Projectile.Center);
						if (distance < minDist && target.Hitbox.Intersects(hitbox))
						{
							minDist = distance;
							target2 = i;
						}
					}
				}
				current += goOutFromPlayer * 16;
				int i2 = (int)current.X / 16;
				int j = (int)current.Y / 16;
				Tile tile = Framing.GetTileSafely(i2, j);
				if (!WorldGen.InWorld(i2, j, 20) || (tile.HasTile && !Main.tileSolidTop[tile.TileType] && Main.tileSolid[tile.TileType]))
				{
					break;
				}
			}
			return target2;
		}
		public int currentTarget = -1;
		public float attackCounter = 0;
        public override bool PreAI()
		{
			if(Main.netMode != NetmodeID.Server)
			{
				for (int i = 0; i < (SOTS.Config.lowFidelityMode ? 1 : 1 + Main.rand.Next(2)); i++)
				{
					Vector2 rotational = new Vector2(0, -3.6f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-40f, 40f)));
					rotational.X *= 0.25f;
					rotational.Y *= 1f;
					particleList.Add(new FireParticle(Projectile.Center - rotational * 1.2f, rotational, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(0.75f, 1.0f)));
				}
				cataloguePos();
			}
			return base.PreAI();
		}
		public List<FireParticle> particleList = new List<FireParticle>();
		public void cataloguePos()
		{
			for (int i = 0; i < particleList.Count; i++)
			{
				FireParticle particle = particleList[i];
				particle.Update();
				particle.position += Projectile.velocity * 0.9f;
				if (!particle.active)
				{
					particleList.RemoveAt(i);
					i--;
				}
			}
		}
		public virtual void ActiveCheck(Player player)
		{
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<Virtuous>());
			}
			if (player.HasBuff(ModContent.BuffType<Virtuous>()))
			{
				Projectile.timeLeft = 6;
			}
		}
		public void FindPassivePosition(ref Vector2 goTo, ref Vector2 toLocation)
		{
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			int ofTotal = 0;
			int total = 0;
			if (Main.myPlayer == player.whoAmI)
			{
				bool found = false;
				for (int i = 0; i < Main.projectile.Length; i++)
				{
					Projectile proj = Main.projectile[i];
					if ((proj.type == Projectile.type || proj.ModProjectile as WispMinion != null) && proj.active && Projectile.active && proj.owner == Projectile.owner)
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
				Projectile.ai[0] = ofTotal;
				storeTotal = total;
				if ((int)Projectile.ai[1] % 30 == 0)
					Projectile.netUpdate = true;
			}
			ofTotal = (int)Projectile.ai[0];
			total = storeTotal;
			Projectile.ai[1]++;
			int direction = 1;
			int increment = 2;
			float multiplier = 1f;
			int ringSize = 6;
			while (ofTotal >= ringSize)
			{
				ofTotal -= ringSize;
				total -= ringSize;
				ringSize += increment;
				increment += 2;
				direction *= -1;
				multiplier *= 0.775f;
			}
			if (total >= ringSize)
				total = ringSize;
			Vector2 orbit = new Vector2(8 + increment * 16, 0).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter * 2f * multiplier * direction + (ofTotal * 360f / total)));
			toLocation = player.Center + new Vector2(0, 2) + orbit;
			goTo = toLocation - Projectile.Center;
		}
		public virtual Vector2 FindAttackPosition(ref Vector2 goTo, ref Vector2 toLocation, int targetID)
		{
			NPC target = Main.npc[targetID];
			Vector2 toNPC = target.Center - toLocation;
			float length = toNPC.Length() - (float)Math.Sqrt(target.width * target.height) * 0.75f - 100;
			Vector2 middle = toLocation + toNPC.SafeNormalize(Vector2.Zero) * length * 0.5f;
			Vector2 rotationalPosition = new Vector2(-length * 0.5f, 0).RotatedBy(MathHelper.ToRadians(attackCounter * 2));
			int direction2 = toNPC.X > 0 ? 1 : -1;
			rotationalPosition.Y *= 0.2f * direction2;
			rotationalPosition = rotationalPosition.RotatedBy(toNPC.ToRotation());
			toLocation = middle + rotationalPosition;
			goTo = toLocation - Projectile.Center;
			return toNPC;
		}
		public bool hasAttacked = false;
		public int storeTotal = 0;
		public float attackCounterSpeed = 2f;
		public float attackCounterCooldown = 40;
		public float normalSpeed = 10f;
		public float enemyRange = 640f;
		public float midCounterMult = 0.25f;
		public virtual void DoAttack(Vector2 toNPC)
		{
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + toNPC.SafeNormalize(Vector2.Zero) * 40, toNPC.SafeNormalize(Vector2.Zero) * 5, ModContent.ProjectileType<SpectralWispLaser>(), Projectile.damage, 1f, Main.myPlayer, 0, 0);
		}
		public sealed override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if (Projectile.owner == player.whoAmI)
				ActiveCheck(player);
			else
				Projectile.timeLeft = 6;
			Vector2 goTo = Projectile.Center;
			Vector2 toLocation = Projectile.Center;
			FindPassivePosition(ref goTo, ref toLocation);
			if (currentTarget == -1)
			{
				currentTarget = FindClosestEnemy();
			}
			else
			{
				NPC target = Main.npc[currentTarget];
				if (target.CanBeChasedBy() && attackCounter >= 0)
				{
					Vector2 toNPC = FindAttackPosition(ref goTo, ref toLocation, currentTarget);
					if (attackCounter < 80)
					{
						attackCounter += attackCounterSpeed;
						if (attackCounter > 80)
							attackCounter = 80;
					}
					else if (attackCounter < 100)
					{
						attackCounter += attackCounterSpeed * midCounterMult;
						if (attackCounter >= 90 && !hasAttacked && Main.myPlayer == Projectile.owner)
						{
							hasAttacked = true;
							DoAttack(toNPC);
						}
					}
					else
					{
						hasAttacked = false;
						attackCounter += attackCounterSpeed;
					}
					if (attackCounter >= 178)
						attackCounter = -attackCounterCooldown;
				}
				else
				{
					currentTarget = -1;
				}
			}
			if(attackCounter < 2)
            {
				attackCounter++;
			}
			float dist = goTo.Length();
			float speed = normalSpeed + dist / 16f;
			if (speed > dist)
				speed = dist;
			Projectile.velocity = goTo.SafeNormalize(Vector2.Zero) * speed;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Celestial/SubspaceLingeringFlame");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Color color;
			for (int i = 0; i < particleList.Count; i++)
			{
				color = new Color(60, 90, 125, 0);
				Vector2 drawPos = particleList[i].position - Main.screenPosition;
				color = Projectile.GetAlpha(color) * (0.35f + 0.65f * particleList[i].scale);
				for (int j = 0; j < 2; j++)
				{
					float x = Main.rand.NextFloat(-2f, 2f);
					float y = Main.rand.NextFloat(-2f, 2f);
					Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, particleList[i].rotation, drawOrigin, particleList[i].scale * 1.25f, SpriteEffects.None, 0f);
				}
			}
			texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Inferno/SpectralWispOutline");
			drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			color = new Color(100, 100, 100, 0);
			for (int k = 0; k < 12; k++)
			{
				Vector2 drawPos = Projectile.Center - Main.screenPosition;
				Vector2 circular = new Vector2(Main.rand.NextFloat(0, 3), 0).RotatedBy(Math.PI / 6 * k);
				Main.spriteBatch.Draw(texture, drawPos + circular, null, color * 0.9f, Projectile.rotation, drawOrigin, Projectile.scale * 1.125f, SpriteEffects.None, 0f);
			}
			drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) 
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				color = new Color(100, 100, 100, 0) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale * 0.1f + 0.70f * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0f);
			}
			float modifier = (float)Math.Sin(MathHelper.ToRadians(Projectile.ai[1] * 4)) * 1.5f;
			Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition + new Vector2(0, (int)modifier * 1.2f), null, new Color(255, 255, 255), Projectile.rotation, drawOrigin, Projectile.scale * 0.8f, SpriteEffects.None, 0f);
			return false;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(storeTotal);
			writer.Write(currentTarget);
			writer.Write(Projectile.rotation);
			writer.Write(Projectile.spriteDirection);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			storeTotal = reader.ReadInt32();
			currentTarget = reader.ReadInt32();
			Projectile.rotation = reader.ReadSingle();
			Projectile.spriteDirection = reader.ReadInt32();
		}
	}
	public class LemegetonWispRed : WispMinion
	{
		public override void ActiveCheck(Player player)
		{
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<InfernalDefense>());
			}
			if (player.HasBuff(ModContent.BuffType<InfernalDefense>()))
			{
				Projectile.timeLeft = 6;
			}
		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Lemegeton Wisp");
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = false;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			attackCounterSpeed = 5f;
			attackCounterCooldown = 90f;
			midCounterMult = 0.75f;
			enemyRange = 480f;
		}
		public override void SafeSetDefaults()
		{
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			int heal = 1;
			if (player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center.X, Projectile.Center.Y, 0, 0, Mod.Find<ModProjectile>("HealProj").Type, 0, 0, player.whoAmI, heal, -1);
			}
		}
        public override void DoAttack(Vector2 toNPC)
        {
			//doing nothing here
		}
		public override Vector2 FindAttackPosition(ref Vector2 goTo, ref Vector2 toLocation, int targetID)
		{
			NPC target = Main.npc[targetID];
			Vector2 toNPC = target.Center - toLocation;
			float length = toNPC.Length() - (float)Math.Sqrt(target.width * target.height) * 0.2f;
			Vector2 middle = toLocation + toNPC.SafeNormalize(Vector2.Zero) * length * 0.5f;
			Vector2 rotationalPosition = new Vector2(-length * 0.5f, 0).RotatedBy(MathHelper.ToRadians(attackCounter * 2));
			int direction2 = toNPC.X > 0 ? 1 : -1;
			rotationalPosition.Y *= 0.2f * direction2;
			rotationalPosition = rotationalPosition.RotatedBy(toNPC.ToRotation());
			toLocation = middle + rotationalPosition;
			goTo = toLocation - Projectile.Center;
			return toNPC;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Celestial/SubspaceLingeringFlame");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Color color;
			for (int i = 0; i < particleList.Count; i++)
			{
				color = new Color(255, 82, 97, 0);
				Vector2 drawPos = particleList[i].position - Main.screenPosition;
				color = Projectile.GetAlpha(color) * (0.35f + 0.65f * particleList[i].scale);
				for (int j = 0; j < 2; j++)
				{
					float x = Main.rand.NextFloat(-2f, 2f);
					float y = Main.rand.NextFloat(-2f, 2f);
					Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, particleList[i].rotation, drawOrigin, particleList[i].scale * 1.25f, SpriteEffects.None, 0f);
				}
			}
			texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Inferno/LemegetonWispRedOutline");
			drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			color = new Color(100, 100, 100, 0);
			for (int k = 0; k < 12; k++)
			{
				Vector2 drawPos = Projectile.Center - Main.screenPosition;
				Vector2 circular = new Vector2(Main.rand.NextFloat(0, 3), 0).RotatedBy(Math.PI / 6 * k);
				Main.spriteBatch.Draw(texture, drawPos + circular, null, color * 0.9f, Projectile.rotation, drawOrigin, Projectile.scale * 1.125f, SpriteEffects.None, 0f);
			}
			drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				color = new Color(100, 100, 100, 0) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale * 0.1f + 0.70f * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0f);
			}
			float modifier = (float)Math.Sin(MathHelper.ToRadians(Projectile.ai[1] * 4)) * 1.5f;
			Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition + new Vector2(0, (int)modifier * 1.2f), null, new Color(255, 255, 255), Projectile.rotation, drawOrigin, Projectile.scale * 0.8f, SpriteEffects.None, 0f);
			return false;
		}
	}
	public class LemegetonWispGreen : WispMinion
	{
		public override void ActiveCheck(Player player)
		{
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<InfernalDefense>());
			}
			if (player.HasBuff(ModContent.BuffType<InfernalDefense>()))
			{
				Projectile.timeLeft = 6;
			}
		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Lemegeton Wisp");
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = false;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			attackCounterSpeed = 6f;
			attackCounterCooldown = 30f;
			midCounterMult = 0.8f;
			enemyRange = 960f;
		}
		public override Vector2 FindAttackPosition(ref Vector2 goTo, ref Vector2 toLocation, int targetID)
		{
			NPC target = Main.npc[targetID];
			Vector2 toNPC = target.Center - toLocation;
			float length = toNPC.Length() - (float)Math.Sqrt(target.width * target.height) * 0.75f - 64;
			Vector2 middle = toLocation + toNPC.SafeNormalize(Vector2.Zero) * length * 0.5f;
			Vector2 rotationalPosition = new Vector2(-length * 0.5f, 0).RotatedBy(MathHelper.ToRadians(attackCounter * 2));
			int direction2 = toNPC.X > 0 ? 1 : -1;
			rotationalPosition.Y *= 0.2f * direction2;
			rotationalPosition = rotationalPosition.RotatedBy(toNPC.ToRotation());
			toLocation = middle + rotationalPosition;
			goTo = toLocation - Projectile.Center;
			return toNPC;
		}
		public override void SafeSetDefaults()
		{
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
		}
		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.SourceDamage *= 0.5f;
		}
		public override void DoAttack(Vector2 toNPC)
		{
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + toNPC.SafeNormalize(Vector2.Zero) * 24, toNPC.SafeNormalize(Vector2.Zero) * 5, ModContent.ProjectileType<SpectralWispLaser>(), Projectile.damage, 1f, Main.myPlayer, -1, 0);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Celestial/SubspaceLingeringFlame");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Color color;
			for (int i = 0; i < particleList.Count; i++)
			{
				color = new Color(104, 229, 101, 0);
				Vector2 drawPos = particleList[i].position - Main.screenPosition;
				color = Projectile.GetAlpha(color) * (0.35f + 0.65f * particleList[i].scale);
				for (int j = 0; j < 2; j++)
				{
					float x = Main.rand.NextFloat(-2f, 2f);
					float y = Main.rand.NextFloat(-2f, 2f);
					Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, particleList[i].rotation, drawOrigin, particleList[i].scale * 1.25f, SpriteEffects.None, 0f);
				}
			}
			texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Inferno/LemegetonWispGreenOutline");
			drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			color = new Color(100, 100, 100, 0);
			for (int k = 0; k < 12; k++)
			{
				Vector2 drawPos = Projectile.Center - Main.screenPosition;
				Vector2 circular = new Vector2(Main.rand.NextFloat(0, 3), 0).RotatedBy(Math.PI / 6 * k);
				Main.spriteBatch.Draw(texture, drawPos + circular, null, color * 0.9f, Projectile.rotation, drawOrigin, Projectile.scale * 1.125f, SpriteEffects.None, 0f);
			}
			drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				color = new Color(100, 100, 100, 0) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale * 0.1f + 0.70f * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0f);
			}
			float modifier = (float)Math.Sin(MathHelper.ToRadians(Projectile.ai[1] * 4)) * 1.5f;
			Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition + new Vector2(0, (int)modifier * 1.2f), null, new Color(255, 255, 255), Projectile.rotation, drawOrigin, Projectile.scale * 0.8f, SpriteEffects.None, 0f);
			return false;
		}
	}
	public class LemegetonWispPurple : WispMinion
	{
		public override void ActiveCheck(Player player)
		{
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<InfernalDefense>());
			}
			if (player.HasBuff(ModContent.BuffType<InfernalDefense>()))
			{
				Projectile.timeLeft = 6;
			}
		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Lemegeton Wisp");
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = false;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			attackCounterSpeed = 2.5f;
			attackCounterCooldown = 120f;
			midCounterMult = 0.33f;
			enemyRange = 720f;
		}
		public override void SafeSetDefaults()
		{
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
		}
		public override Vector2 FindAttackPosition(ref Vector2 goTo, ref Vector2 toLocation, int targetID)
		{
			NPC target = Main.npc[targetID];
			Vector2 toNPC = target.Center - toLocation;
			float length = toNPC.Length() - (float)Math.Sqrt(target.width * target.height) * 0.8f - 160;
			Vector2 middle = toLocation + toNPC.SafeNormalize(Vector2.Zero) * length * 0.5f;
			Vector2 rotationalPosition = new Vector2(-length * 0.5f, 0).RotatedBy(MathHelper.ToRadians(attackCounter * 2));
			int direction2 = toNPC.X > 0 ? 1 : -1;
			rotationalPosition.Y *= 0.2f * direction2;
			rotationalPosition = rotationalPosition.RotatedBy(toNPC.ToRotation());
			toLocation = middle + rotationalPosition;
			goTo = toLocation - Projectile.Center;
			return toNPC;
		}
		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.SourceDamage *= 0.5f;
		}
		public override void DoAttack(Vector2 toNPC)
		{
			for(int i = -1; i <= 1; i++)
			{
				Vector2 rotateVelo = toNPC.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(10 * i));
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + toNPC.SafeNormalize(Vector2.Zero) * 24, rotateVelo * 4.5f, ModContent.ProjectileType<PurpleHomingBolt>(), Projectile.damage, 1f, Main.myPlayer);
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Celestial/SubspaceLingeringFlame");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Color color;
			for (int i = 0; i < particleList.Count; i++)
			{
				color = new Color(160, 95, 198, 0);
				Vector2 drawPos = particleList[i].position - Main.screenPosition;
				color = Projectile.GetAlpha(color) * (0.35f + 0.65f * particleList[i].scale);
				for (int j = 0; j < 2; j++)
				{
					float x = Main.rand.NextFloat(-2f, 2f);
					float y = Main.rand.NextFloat(-2f, 2f);
					Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, particleList[i].rotation, drawOrigin, particleList[i].scale * 1.25f, SpriteEffects.None, 0f);
				}
			}
			texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Inferno/LemegetonWispPurpleOutline");
			drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			color = new Color(100, 100, 100, 0);
			for (int k = 0; k < 12; k++)
			{
				Vector2 drawPos = Projectile.Center - Main.screenPosition;
				Vector2 circular = new Vector2(Main.rand.NextFloat(0, 3), 0).RotatedBy(Math.PI / 6 * k);
				Main.spriteBatch.Draw(texture, drawPos + circular, null, color * 0.9f, Projectile.rotation, drawOrigin, Projectile.scale * 1.125f, SpriteEffects.None, 0f);
			}
			drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				color = new Color(100, 100, 100, 0) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale * 0.1f + 0.70f * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0f);
			}
			float modifier = (float)Math.Sin(MathHelper.ToRadians(Projectile.ai[1] * 4)) * 1.5f;
			Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition + new Vector2(0, (int)modifier * 1.2f), null, new Color(255, 255, 255), Projectile.rotation, drawOrigin, Projectile.scale * 0.8f, SpriteEffects.None, 0f);
			return false;
		}
	}
	public class WispOrange : WispMinion
	{
		public override void ActiveCheck(Player player)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if ((modPlayer.petFreeWisp + 1) != Projectile.damage)
			{
				Projectile.Kill();
			}
			if (Projectile.timeLeft > 100)
			{
				Projectile.timeLeft = 300;
			}
		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Will o'");
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = false;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			normalSpeed = 20f;
			attackCounterSpeed = 8f;
			attackCounterCooldown = 6f;
			midCounterMult = 0.1f;
		}
		public override Vector2 FindAttackPosition(ref Vector2 goTo, ref Vector2 toLocation, int targetID)
		{
			NPC target = Main.npc[targetID];
			Vector2 toNPC = target.Center - toLocation;
			float length = toNPC.Length() - (float)Math.Sqrt(target.width * target.height) * 0.75f - 64;
			Vector2 middle = toLocation + toNPC.SafeNormalize(Vector2.Zero) * length * 0.5f;
			Vector2 rotationalPosition = new Vector2(-length * 0.5f, 0).RotatedBy(MathHelper.ToRadians(attackCounter * 2));
			int direction2 = toNPC.X > 0 ? 1 : -1;
			rotationalPosition.Y *= 0.2f * direction2;
			rotationalPosition = rotationalPosition.RotatedBy(toNPC.ToRotation());
			toLocation = middle + rotationalPosition;
			goTo = toLocation - Projectile.Center;
			return toNPC;
		}
		public override void SafeSetDefaults()
		{
			Projectile.usesLocalNPCImmunity = true;
			Projectile.ContinuouslyUpdateDamageStats = false;
			Projectile.localNPCHitCooldown = 30;
		}
		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.SourceDamage *= 0.5f;
		}
		public override void DoAttack(Vector2 toNPC)
		{
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + toNPC.SafeNormalize(Vector2.Zero) * 24, toNPC.SafeNormalize(Vector2.Zero) * 5, ModContent.ProjectileType<OrangeWispLaser>(), Projectile.damage, 1f, Main.myPlayer, -1, 0);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Celestial/SubspaceLingeringFlame");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Color color;
			for (int i = 0; i < particleList.Count; i++)
			{
				color = new Color(235, 60, 0, 0);;
				Vector2 drawPos = particleList[i].position - Main.screenPosition;
				color = Projectile.GetAlpha(color) * (0.35f + 0.65f * particleList[i].scale);
				for (int j = 0; j < 2; j++)
				{
					float x = Main.rand.NextFloat(-2f, 2f);
					float y = Main.rand.NextFloat(-2f, 2f);
					Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, particleList[i].rotation, drawOrigin, particleList[i].scale * 1.25f, SpriteEffects.None, 0f);
				}
			}
			texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Inferno/WispOrangeOutline");
			drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			color = new Color(100, 100, 100, 0);
			for (int k = 0; k < 12; k++)
			{
				Vector2 drawPos = Projectile.Center - Main.screenPosition;
				Vector2 circular = new Vector2(Main.rand.NextFloat(0, 3), 0).RotatedBy(Math.PI / 6 * k);
				Main.spriteBatch.Draw(texture, drawPos + circular, null, color * 0.9f, Projectile.rotation, drawOrigin, Projectile.scale * 1.125f, SpriteEffects.None, 0f);
			}
			drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				color = new Color(100, 100, 100, 0) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale * 0.1f + 0.70f * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0f);
			}
			float modifier = (float)Math.Sin(MathHelper.ToRadians(Projectile.ai[1] * 4)) * 1.5f;
			Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition + new Vector2(0, (int)modifier * 1.2f), null, new Color(255, 255, 255), Projectile.rotation, drawOrigin, Projectile.scale * 0.8f, SpriteEffects.None, 0f);
			return false;
		}
	}
}
		