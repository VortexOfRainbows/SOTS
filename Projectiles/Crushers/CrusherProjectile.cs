using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Crushers
{    
    public abstract class CrusherProjectile : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crusher Arm");
		}
		public sealed override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.timeLeft = 6004;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 0;
			SafeSetDefaults();
		}
		public virtual void SafeSetDefaults()
        {

        }

		public Vector2[] arms = new Vector2[2];
		bool[] setsActive = new bool[1];
		int consumedVoid = 0;
		///These are for modification
		public int trailLength = 0;
		public List<Vector2>[] stored;
		public float maxDamage = 5; //total damage %, 1 for no increase, 5 for 500% at max charge, etc
		public int chargeTime = 180; //charge time in frames
		public int minExplosions = 3; //amount of explosions minimum
		public int maxExplosions = 5; //amount of explosions max
		public float explosiveRange = 48; //distance between each explosion
		public float releaseTime = 120; //how long in frames until auto-release
		public float armDist = 15;
		public float finalDist = 165;
		public float exponentReduction = 0.5f;
		public float minDamage = 0f;
		public int minExplosionSpread = 1;
		public int maxExplosionSpread = 1;
		public float spreadDeg = 15f;
		public float armAngle = 45f;
		///Make sure to change the released projectile down near the bottom

		public float minTimeBeforeRelease = 5;
		public float accSpeed = 0.3f; //speed of the retraction, scales exponentially
		public float initialExplosiveRange = 48; //distance between player and first explosion (also the distance between the player and the crusher)

		///DO NOT MODIFY
		bool released = false;
		float rotationTimer = 0; //assume finalDist is full rotation, 0 has not been rotated
		int explosive = 1;
		int explosiveShotgun = 1;
		float currentCharge = 0; //how close are we to chargeTime?
		float initiateTimer = 0; //how close are we to releaseTime?
		float accelerateAmount = 0;

		int initialDamage; 
		bool runOnce = true; 
		public virtual void VoidConsumption(float charge, ref int consumedAmt)
		{
			Player player = Main.player[Projectile.owner];
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			Item item = player.HeldItem;
			VoidItem vItem = item.ModItem as VoidItem;
			if(vItem != null)
			{
				if (charge >= 0.25f && consumedAmt == 0)
				{
					consumedAmt++;
					if (!(vPlayer.CrushResistor && Main.rand.NextBool(3)))
						vItem.DrainMana(player);
				}
				if (charge >= 0.50f && consumedAmt == 1)
				{
					consumedAmt++;
					vItem.DrainMana(player);
				}
				if (charge >= 0.75f && consumedAmt == 2)
				{
					consumedAmt++;
					if (!vPlayer.CrushCapacitor)
						vItem.DrainMana(player);
				}
			}
		}
		public virtual bool CanCharge()
        {
			return true;
        }
		public sealed override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			if (runOnce)
			{
				//if(trailLength > 0)
					//trailLength++;
				stored = new List<Vector2>[arms.Length];
				for(int i = 0; i < stored.Length; i++)
                {
					stored[i] = new List<Vector2>();
                }
				setsActive = new bool[arms.Length / 2];
				for (int i = 0; i < setsActive.Length; i++)
					setsActive[i] = false;
				for (int i = 0; i < arms.Length; i++)
                {
					arms[i] = Projectile.Center;
				}
				runOnce = false;
				initialDamage = Projectile.damage;
			}
			if (Projectile.owner == Main.myPlayer)
			{
				Vector2 cursorArea = Main.MouseWorld;
				if (counter % 3 == 0)
				{
					Projectile.netUpdate = true;
				}
				counter++;
				Projectile.ai[0] = cursorArea.X;
				Projectile.ai[1] = cursorArea.Y;
			}
			if (!released) //not charged and not released
			{
				if(CanCharge())
				{
					float chargeSpeedMult = (1f / player.GetAttackSpeed(DamageClass.Melee) + vPlayer.voidSpeed - 1 + SOTSPlayer.ModPlayer(player).attackSpeedMod - 1) * vPlayer.CrushTransformer;
					currentCharge += 1 * chargeSpeedMult;
					float chargePercentage = currentCharge / chargeTime;
					chargePercentage = (float)Math.Pow(chargePercentage, exponentReduction);
					if (chargePercentage > 1)
					{
						initiateTimer += 1 * chargeSpeedMult;
						chargePercentage = 1;
					}
					int prev = consumedVoid;
					VoidConsumption(chargePercentage, ref consumedVoid);
					if (prev != consumedVoid)
					{
						SOTSUtils.PlaySound(SoundID.Item15, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.0f + 0.1f * consumedVoid);
					}
					int trueMaxExplosions = maxExplosions + vPlayer.BonusCrushRangeMax;
					int trueMinExplosions = minExplosions + vPlayer.BonusCrushRangeMin;
					float explosiveCount = trueMaxExplosions - trueMinExplosions;
					explosiveCount *= chargePercentage;
					explosiveCount += 0.3f;
					explosive = (int)explosiveCount + trueMinExplosions;

					float spreadCount = maxExplosionSpread - minExplosionSpread;
					spreadCount *= chargePercentage;
					spreadCount += 0.3f;
					explosiveShotgun = (int)spreadCount + minExplosionSpread;
					rotationTimer = chargePercentage * finalDist; //making the rotation timer proportional to the charge time completed
					float increaseDamage = minDamage + ((maxDamage - minDamage) * chargePercentage);
					if(ModContent.ProjectileType<SubspaceCrusher>() != Projectile.type)
						Projectile.damage = (int)(initialDamage * increaseDamage);
				}
			}
			Vector2 goToArea = new Vector2(Projectile.ai[0], Projectile.ai[1]) - player.Center;
			Vector2 normalized = goToArea.SafeNormalize(new Vector2(1, 0));
			normalized *= Projectile.velocity.Length();
			Projectile.Center = player.Center + normalized;
			Projectile.velocity = normalized;
			Projectile.rotation = Projectile.velocity.ToRotation();
			player.heldProj = Projectile.whoAmI;
			return Projectile.active;
		}
        public sealed override bool ShouldUpdatePosition()
        {
            return false;
        }
        public virtual int ExplosionType()
        {
			return ModContent.ProjectileType<PinkCrush>();
        }
		public virtual bool UseCustomExplosionEffect(float x, float y, float dist, float rotation, float chargePercent, int indexNumber)
        {
			return false;
        }
		int counter = 0;
		public sealed override void AI()
		{
			Player player = Main.player[Projectile.owner];
			float shootToX = Projectile.ai[0] - player.Center.X;
			float shootToY = Projectile.ai[1] - player.Center.Y;
			double direction = Math.Atan2((double)-shootToY, (double)-shootToX);
			double degDirection = direction	* 180/Math.PI;
						
			if(initiateTimer >= releaseTime)
			{
				released = true;
			}
			if(player.channel || Projectile.timeLeft > 6001 || currentCharge < minTimeBeforeRelease)
			{
				Projectile.timeLeft = 6000;
				Projectile.alpha = 0;
			}
			else
			{
				released = true;
			}
			if (Projectile.hide == false)
			{
				player.ChangeDir(Projectile.direction);
				player.heldProj = Projectile.whoAmI;
				player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
				player.itemTime = 2;
				player.itemAnimation = 2;
				Projectile.alpha = 0;
			}
			for (int i = 0; i < arms.Length; i++)
			{
				bool flip = i % 2 == 1;
				int set = i / 2;
				float deg;
				float charge = rotationTimer;
				charge -= set * armDist;
				if (charge > 0 && !released)
				{
					setsActive[set] = true;
				}
				if (flip)
				{
					deg = charge + 5 + (float)degDirection; //add rotate
				}
				else //when not flip
				{
					deg = -charge - 5 + (float)degDirection; //subtract rotate
				}
				arms[i] = new Vector2(-initialExplosiveRange, 0).RotatedBy(MathHelper.ToRadians(deg));
				if(trailLength > 0)
				{
					cataloguePos(i, player.Center + arms[i]);
				}
			}
			if (released)
			{
				accelerateAmount += accSpeed;
				int amt = setsActive.Length;
				for(int j = 0; j < amt; j++)
				{
					float charge = rotationTimer;
					charge -= j * armDist;
					if (charge <= 0 && setsActive[j]) //collision
					{
						setsActive[j] = false;
						if (Projectile.owner == Main.myPlayer)
						{
							int shotGun = explosiveShotgun - 1;
							for(int k = -shotGun; k <= shotGun; k++)
							{
								double rad1 = direction + MathHelper.ToRadians(spreadDeg * k);
								for (int i = 0; i < explosive; i++)
								{
									double distance = (explosiveRange * i) + initialExplosiveRange;
									float positionX = player.Center.X - (int)(Math.Cos(rad1) * distance);
									float positionY = player.Center.Y - (int)(Math.Sin(rad1) * distance);
									float charge2 = currentCharge / finalDist;
									if (charge2 > 1)
										charge2 = 1f;
									if (!UseCustomExplosionEffect(positionX, positionY, (float)distance, (float)rad1, charge2, i))
										Projectile.NewProjectile(Projectile.GetSource_FromThis(), positionX, positionY, Projectile.velocity.X, Projectile.velocity.Y, ExplosionType(), Projectile.damage, Projectile.knockBack, Main.myPlayer, initialDamage, 0f);
								}
							}
						}
						ExplosionSound();
					}
				}
				if (!setsActive[0])
					Projectile.Kill();
				rotationTimer -= accelerateAmount; //start to close the crushers
			}
		}
		public virtual void ExplosionSound()
		{
			SOTSUtils.PlaySound(SoundID.Item14, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.1f);
		}
		public virtual Texture2D ArmTexture(int handNum, int direction)
        {
			return Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
		}
		public void cataloguePos(int arm, Vector2 next)
		{
			List<Vector2> trail = stored[arm];
			trail.Add(next);
			if (trail.Count > trailLength) // || (trail.Count > 1 && trail[trail.Count - 2] == trail[trail.Count - 1]))
				trail.RemoveAt(0);
			stored[arm] = trail;
		}
		public sealed override bool PreDraw(ref Color lightColor)
		{
			if (runOnce)
				return false;
			if (Projectile.type == ModContent.ProjectileType<HellbreakerCrusher>() || Projectile.type == ModContent.ProjectileType<SubspaceCrusher>())
				lightColor = Color.White;
			Player player = Main.player[Projectile.owner];
			//VoidPlayer modPlayer = VoidPlayer.ModPlayer(player);
			Vector2 cursorArea = new Vector2(Projectile.ai[0], Projectile.ai[1]);
			float shootToX = cursorArea.X - player.Center.X;
			float shootToY = cursorArea.Y - player.Center.Y;
			double direction = Math.Atan2(-shootToY, -shootToX);
			double degDirection = direction * 180 / Math.PI;
			for (int i = 0; i < arms.Length; i++)
			{
				Vector2 pos = arms[i];
				Texture2D texture = ArmTexture(i, Projectile.direction);
				Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);

				float rotation;
				bool flip = i % 2 == 1;
				int set = i / 2;
				float deg;
				float charge = rotationTimer;
				charge -= set * armDist;
				if (flip)
				{
					deg = charge + 5 + (float)degDirection; //add rotate
				}
				else //when not flip
				{
					deg = -charge - 5 + (float)degDirection; //subtract rotate
				}
				rotation = MathHelper.ToRadians(deg + (flip ? (360 - armAngle) : (180 + armAngle)));
				if (setsActive[set] && charge > 0 && pos != Projectile.Center)
				{
					List<Vector2> trail = stored[i];
					for (int j = 1; j < trail.Count; j++)
					{
						Color color = Projectile.GetAlpha(lightColor) * ((float)j / trailLength) * 0.4f;
						Vector2 pos2 = trail[j];
						Main.spriteBatch.Draw(texture, pos2 - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), color, rotation, origin, 1.05f, !flip ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
					}
					Main.spriteBatch.Draw(texture, player.Center + pos - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), Projectile.GetAlpha(lightColor), rotation, origin, 1.05f, !flip ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
				}
			}
			Texture2D headTexture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin2 = new Vector2(headTexture.Width / 2, headTexture.Height / 2);
			Main.spriteBatch.Draw(headTexture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, headTexture.Width, headTexture.Height), lightColor, Projectile.rotation + MathHelper.ToRadians(45), origin2, 1.05f, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			return false;
        }
    }
}
		