﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Flails
{
	public abstract class BaseFlailItem : ModItem
	{
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noUseGraphic = true;
			Item.DamageType = DamageClass.Melee;
			Item.channel = true;
			Item.UseSound = SoundID.Item19;
			SafeSetDefaults();
		}

		public virtual void SafeSetDefaults() { }

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] == 0;
	}

	public abstract class BaseFlailProj : ModProjectile
	{
		internal bool released = false;
		internal bool falling = false;
		internal bool strucktile = false;

		private float Timer
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		private float ChargeTime
		{
			get => Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}

		public float MaxChargeTime;
		public Vector2 SpeedMult;
		public Vector2 DamageMult;
		public float spinningdistance;
		public float degreespertick;

		public void SetFlailStats(Vector2 SpeedMult, Vector2 DamageMult, float MaxChargeTime = 2, float spinningdistance = 50, float degreespertick = 10)
		{
			this.SpeedMult = SpeedMult;
			this.DamageMult = DamageMult;
			this.MaxChargeTime = MaxChargeTime;
			this.spinningdistance = spinningdistance;
			this.degreespertick = degreespertick;
			Projectile.netUpdate = true;
		}

		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(34, 34);
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(released);
			writer.Write(falling);
			writer.Write(strucktile);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			released = reader.ReadBoolean();
			falling = reader.ReadBoolean();
			strucktile = reader.ReadBoolean();
		}
		float soundTimer = 0;
		public int Timer2 = 0;
		public override void AI()
		{
			Player Owner = Main.player[Projectile.owner];
			Owner.itemTime = 2;
			Owner.itemAnimation = 2;
			Owner.heldProj = Projectile.whoAmI;
			if (!Owner.channel && !released) //check to see if player stops channelling
			{
				released = true;
				Timer = 0;
				Projectile.netUpdate = true;
			}

			if (Owner.channel && !released) //spinning around the player
			{
				Owner.itemRotation = 0;
				Projectile.velocity = Vector2.Zero;
				Projectile.tileCollide = false;
				Timer2 += 1 * Owner.direction; //spin in direction of player
				Timer++;
				soundTimer += 1f + ChargeTime / MaxChargeTime;
				if (soundTimer >= 30)
				{
					SOTSUtils.PlaySound(SoundID.Item19, Projectile.Center, 0.5f, 0.1f);
					soundTimer -= 30;
				}

				float exitPercent = MathHelper.Clamp(Timer / 30f, 0, 1f);
				ChargeTime = MathHelper.Clamp(Timer / 60, MaxChargeTime / 6, MaxChargeTime);

				float radians = MathHelper.ToRadians(degreespertick * Timer2 * (1 + ChargeTime/MaxChargeTime)) ;
				float distfromplayer = exitPercent * spinningdistance * ((float)Math.Abs(Math.Cos(radians) / 5) + 0.8f); //use a cosine function based on the amount of rotation the flail has gone through to create an ellipse-like pattern
				Vector2 spinningoffset = new Vector2(distfromplayer, 0).RotatedBy(radians);
				Projectile.Center = Owner.MountedCenter + spinningoffset;
				if (Owner.whoAmI == Main.myPlayer)
					Owner.ChangeDir(Math.Sign(Main.MouseWorld.X - Owner.Center.X));
				Projectile.rotation = Projectile.AngleFrom(Owner.MountedCenter) - 1.57f; //update rotation last so it is most accurate

				SpinExtras(Owner);
			}

			else
			{
				Projectile.rotation = Projectile.AngleFrom(Owner.MountedCenter) - 1.57f;
				Owner.ChangeDir(Math.Sign(Projectile.Center.X - Owner.Center.X));
				Owner.itemRotation = MathHelper.WrapAngle(Projectile.AngleFrom(Owner.MountedCenter) - ((Owner.direction < 0) ? MathHelper.Pi : 0));
			}

			float launchspeed = Owner.HeldItem.shootSpeed * MathHelper.Lerp(SpeedMult.X, SpeedMult.Y, ChargeTime / MaxChargeTime);
			if (released && !falling) //basic flail launch, returns after a while
			{
				Projectile.tileCollide = true;
				if(++Timer == 1 && Owner.whoAmI == Main.myPlayer)
				{
					if(Timer > 8)
						Terraria.Audio.SoundEngine.PlaySound(SoundID.Item19, Projectile.Center);
					Projectile.Center = Owner.MountedCenter;
					Projectile.velocity = Owner.DirectionTo(Main.MouseWorld) * launchspeed;
					OnLaunch(Owner);
				}

				if (Timer >= MathHelper.Min(60 * (ChargeTime / MaxChargeTime), 30)) //max out on time halfway through charge
					Return(launchspeed, Owner);

				else
					LaunchExtras(Owner);

				//if (Owner.controlUseItem)
				//{
				//	Projectile.netUpdate = true;
				//	falling = true;
				//	Timer = 0;
				//}
			}

			if(falling) //falling towards ground, returns after hitting ground
			{
				if(strucktile || ++Timer >= 180)
					Return(launchspeed, Owner);

				else
				{
					FallingExtras(Owner);
					Projectile.tileCollide = true;
					if(Projectile.velocity.Y < 16f)
						Projectile.velocity.Y += 0.5f;

					Projectile.velocity.X *= 0.98f;
				}
			}
		}
		public bool returned = false;
		private float returnSpeedUp = 0.01f;
		private void Return(float launchspeed, Player Owner)
		{
			Projectile.tileCollide = false;
			Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(Owner.Center) * launchspeed * 1.5f, 0.05f + returnSpeedUp);
			if (returnSpeedUp < 1)
				returnSpeedUp += 0.005f;
			if (Projectile.Hitbox.Intersects(Owner.Hitbox))
				Projectile.Kill();
			ReturnExtras(Owner);
		}

        #region extra hooks
        public virtual void SpinExtras(Player player) { }

		public virtual void NotSpinningExtras(Player player) { }

		public virtual void OnLaunch(Player player) { }

		public virtual void LaunchExtras(Player player) { NotSpinningExtras(player); }

		public virtual void FallingExtras(Player player) { NotSpinningExtras(player); }

		public virtual void ReturnExtras(Player player) { NotSpinningExtras(player); }

		public virtual void SafeTileCollide(Vector2 oldVelocity) { }

		public virtual void FallingTileCollide(Vector2 oldVelocity) { }
		#endregion

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			Player Owner = Main.player[Projectile.owner];
			if (ChargeTime > 0)
				modifiers.SourceDamage *= MathHelper.Lerp(DamageMult.X, DamageMult.Y, ChargeTime / MaxChargeTime);
			modifiers.HitDirectionOverride = Owner.direction;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (falling)
			{
				strucktile = true;
				FallingTileCollide(oldVelocity);
			}
			Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
			Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
			Projectile.velocity = new Vector2((Projectile.velocity.X != Projectile.oldVelocity.X) ?
				-Projectile.oldVelocity.X / 5 : Projectile.velocity.X,
				(Projectile.velocity.Y != Projectile.oldVelocity.Y) ?
				-Projectile.oldVelocity.Y / 5 : Projectile.velocity.Y);
			SafeTileCollide(oldVelocity);
			Timer = 30;
			return false;
		}

        public override bool PreDrawExtras()
        {
			Texture2D ChainTexture = Mod.Assets.Request<Texture2D>(Texture.Remove(0, Mod.Name.Length + 1) + "_chain").Value;
			Player Owner = Main.player[Projectile.owner];
			int timestodrawchain = Math.Max((int)(Projectile.Distance(Owner.MountedCenter) / ChainTexture.Width), 1);
			for (int i = 0; i < timestodrawchain; i++)
			{
				Vector2 chaindrawpos = Vector2.Lerp(Owner.MountedCenter, Projectile.Center, (i / (float)timestodrawchain));
				float scaleratio = Projectile.Distance(Owner.MountedCenter) / ChainTexture.Width / timestodrawchain;
				Vector2 chainscale = new Vector2(scaleratio, 1);
				Color lightColor = Lighting.GetColor((int)chaindrawpos.X / 16, (int)chaindrawpos.Y / 16);
				Main.spriteBatch.Draw(ChainTexture, chaindrawpos - Main.screenPosition, null, lightColor, Projectile.AngleFrom(Owner.MountedCenter), new Vector2(0, ChainTexture.Height / 2), chainscale, SpriteEffects.None, 0);
			}
			return true;
		}
	}
}
