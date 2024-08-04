using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Void;
using SOTS.Common.GlobalNPCs;
using System.IO;
using SOTS.Prim.Trails;
using SOTS.Prim;
using SOTS.Projectiles.Lightning;
using SOTS.Dusts;
using System;

namespace SOTS.Projectiles.Blades
{    
    public class VorpalThrow : ModProjectile 
    {
		public static Color VorpalColor1 = new Color(56, 159, 135);
		public static Color VorpalColor2 = new Color(86, 226, 100);
		public override string Texture => "SOTS/Projectiles/Blades/VorpalKnifeSlash";
        float rotation = 0;
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.timeLeft);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.timeLeft = reader.ReadInt32();
		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Vertebraeker");
		}
        public override void SetDefaults()
        {
			Projectile.height = 60;
			Projectile.width = 64;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.DamageType = ModContent.GetInstance<VoidMelee>();
			Projectile.timeLeft = 270;
			Projectile.tileCollide = true;
			Projectile.hostile = false;
			Projectile.alpha = 0;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 45; //15 frames, given extra updates
			Projectile.extraUpdates = 2;
			Projectile.netImportant = true;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = 100;
			hitbox = new Rectangle((int)Projectile.Center.X - width / 2, (int)Projectile.Center.Y - width / 2, width, width);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 24;
			height = 24;
            return true;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.HitDirectionOverride = initialDirection;
		}
		bool runOnce = true;
		Vector2 initialVelo;
		Vector2 initialCenter;
		public int initialDirection = 0;
		int soundCounter = 0;
		bool runOnce2 = true;
		public Vector2 newCenter;
        public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			if (runOnce)
			{
				newCenter = Projectile.Center;
				SOTSUtils.PlaySound(SoundID.DD2_GhastlyGlaivePierce, (int)player.Center.X, (int)player.Center.Y, 1.6f, -0.1f);
				runOnce = false;
				initialVelo = Projectile.velocity;
				if(Projectile.velocity.X > 0)
				{
					rotation = MathHelper.ToRadians(180);
					initialDirection = 1;
				}
				else
				{
					rotation = MathHelper.ToRadians(90);
					initialDirection = -1;
				}
				initialCenter = player.Center;
				Projectile.ai[0] = -180 * initialDirection;
				Projectile.scale = 1.2f;
                BladeTrail myTrail = new BladeTrail(Projectile, clockWise: -initialDirection, VorpalColor1.ToVector4(), VorpalColor2.ToVector4(), 108, 2);
                SOTS.primitives.CreateTrail(myTrail);
			}
			else if(soundCounter % 21 == 0)
				SOTSUtils.PlaySound(SoundID.DD2_MonkStaffSwing, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.0f, -0.1f);
			soundCounter++;
			Projectile.ai[0] += 2f * initialDirection;
			if (Projectile.timeLeft >= 180)
			{
				Vector2 initialCenter = this.initialCenter;
				int length = 180;
				double rad = MathHelper.ToRadians(Projectile.ai[0]);
				Vector2 ovalArea = new Vector2(length, 0).RotatedBy(initialVelo.ToRotation());
				Vector2 ovalArea2 = new Vector2(length, 0).RotatedBy((float)rad);
				ovalArea2.Y *= 0.5f * Projectile.ai[1];
				ovalArea2 = ovalArea2.RotatedBy(initialVelo.ToRotation());
				ovalArea.X += ovalArea2.X;
				ovalArea.Y += ovalArea2.Y;
				Vector2 goTo = initialCenter + ovalArea;
				float dist = (Projectile.Center - goTo).Length();
				Vector2 circular = new Vector2(-(dist > 18 ? 18 : dist), 0).RotatedBy((Projectile.Center - goTo).ToRotation());
				Projectile.velocity = circular + new Vector2(0, -1.7f);
				newCenter = Projectile.Center;
			}
			else if (Projectile.timeLeft > 120)
			{
				double rad = MathHelper.ToRadians(Projectile.ai[0]);
				Projectile.velocity *= 0.725f;
				Projectile.velocity += new Vector2(0.6f * initialDirection, 0).RotatedBy(rad);
				Projectile.ai[0] += 2f * initialDirection;
				Projectile.timeLeft = 150;
			}
			else
			{
				if(runOnce2)
				{
					Projectile.netUpdate = true;
					Projectile.damage *= 3;
					SOTSUtils.PlaySound(SoundID.Item62, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.1f, -0.2f);
					if (Main.myPlayer == Projectile.owner)
					{
						for (int j = -1; j <= 1; j++)
							Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + new Vector2(0, 24 + Main.rand.NextFloat(-8, 8)), new Vector2(j * 0.3f + Main.rand.NextFloat(-0.1f, 0.1f), -1), ModContent.ProjectileType<GreenLightning2>(), 0, 0, Main.myPlayer, -1);
                        Planetarium.GenesisCore.LaunchLightningAtNearbyEnemies(Projectile, ModContent.ProjectileType<VorpalLightning>(), 8, 176, Projectile.damage);
					}
					for (int i = 0; i < 360; i += 2)
					{
						if (Main.rand.NextBool(3))
						{
							Vector2 circularLocation = new Vector2(13 * Main.rand.NextFloat(1), 0).RotatedBy(MathHelper.ToRadians(i));
							Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<CopyDust4>());
							dust.noGravity = true;
							dust.velocity *= 0.8f;
							dust.velocity += circularLocation;
							dust.scale *= 1.7f;
							dust.fadeIn = 0.1f;
							dust.color = Color.Lerp(VorpalColor1, VorpalColor2, 1 - Main.rand.NextFloat(1f) * Main.rand.NextFloat(1f));
							dust.alpha = 100;
						}
						else
						{
							Vector2 circularLocation = new Vector2(160 + Main.rand.NextFloat(-12, 12), 0).RotatedBy(MathHelper.ToRadians(i));
							Dust dust = Dust.NewDustDirect(Projectile.Center + circularLocation, 0, 0, ModContent.DustType<CopyDust4>());
							dust.noGravity = true;
							dust.velocity *= 0.5f;
							dust.velocity += circularLocation.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(0.4f, 1.4f);
							dust.scale *= 1.5f;
							dust.fadeIn = 0.1f;
							dust.color = Color.Lerp(VorpalColor1, VorpalColor2, 1 - Main.rand.NextFloat(1f) * Main.rand.NextFloat(1f));
							dust.alpha = 100;
						}
					}
				}
				runOnce2 = false;
				float dist = (Projectile.Center - player.Center).Length();
				float maxSpeed = 12 + (24 * (90 - Projectile.timeLeft) / 90f);
				Vector2 circular = new Vector2(-(dist > maxSpeed ? maxSpeed : dist), 0).RotatedBy((Projectile.Center - player.Center).ToRotation());
				Projectile.velocity = circular;
				Projectile.tileCollide = false;
				if (dist <= 24)
				{
					Projectile.Kill();
				}
			}
			return base.PreAI();
        }
        public override void AI()
		{
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.3f / 255f, (255 - Projectile.alpha) * 0.9f / 255f, (255 - Projectile.alpha) * 0.5f / 255f);
			Player player = Main.player[Projectile.owner];
			rotation += initialDirection * MathHelper.ToRadians(11);
			Projectile.rotation = rotation;
			Projectile.spriteDirection = initialDirection;

			if(Main.rand.NextBool(3))
			{
				Vector2 circular = Main.rand.NextVector2CircularEdge(40, 40);
				circular = circular.RotateRandom(Projectile.rotation) * Main.rand.NextFloat(0.9f, 1.2f);
				float dustScale = 1.0f;
				float rand = Main.rand.NextFloat(0.9f, 1.1f);
				int type = ModContent.DustType<Dusts.CopyDust4>();
				if (Main.rand.NextBool(5))
					type = DustID.GreenTorch;
				Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4) + circular, 0, 0, type);
				dust.velocity *= 0.4f / rand;
				dust.velocity += circular.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(2.4f, 3.0f) * rand;
				dust.noGravity = true;
				dust.scale *= 0.2f / rand;
				dust.scale += 1.2f / rand * dustScale;
				dust.fadeIn = 0.1f;
				if (type == ModContent.DustType<Dusts.CopyDust4>())
					dust.color = Color.Lerp(VorpalColor1, VorpalColor2, Main.rand.NextFloat(1f) * Main.rand.NextFloat(1f));
			}				
			if (Projectile.timeLeft >= 110)
				Projectile.Center += Collision.TileCollision(Projectile.Center - new Vector2(12, 12), Projectile.velocity, 24, 24, true);
			else
				Projectile.Center += Projectile.velocity;
			foreach (PrimTrail trail in SOTS.primitives._trails.ToArray())
			{
				if (trail is BladeTrail fireTrail)
				{
					if (trail.Entity is Projectile proj && proj.whoAmI == Projectile.whoAmI && proj.type == Projectile.type && initialDirection != fireTrail.ClockWiseOrCounterClockwise)
					{
						trail.Update();
					}
				}
			}
		}
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
        public override bool PreDraw(ref Color lightColor)
		{
			if (runOnce)
				return true;
			Texture2D texture2 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Main.spriteBatch.Draw(texture2, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(Color.White), Projectile.rotation + (initialDirection == 1 ? MathHelper.PiOver2 : 0), new Vector2(texture2.Width / 2, texture2.Height / 2), Projectile.scale, initialDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			return false;
		}
        public override void OnKill(int timeLeft)
        {
			if(runOnce2)
            {
				Player player = Main.player[Projectile.owner];
				TeleportPlayer(player);
            }
        }
		public void TeleportPlayer(Player player)
		{
			float distanceMult = (float)Math.Sqrt(player.Center.Distance(Projectile.Center));
			Vector2 toProj = (Projectile.Center - player.Center).SafeNormalize(Vector2.Zero) * (12f + 0.1f * distanceMult);
			if (player.Center.Distance(Projectile.Center) < 3200)
            {
				player.velocity.X *= 0.5f;
				player.velocity.Y *= 0.3f;
				player.velocity += toProj + new Vector2(0, -3);
				player.direction = Math.Sign(toProj.X);
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, toProj, ModContent.ProjectileType<VorpalLightning2>(), Projectile.damage * 2, Projectile.knockBack, Main.myPlayer, Projectile.Center.X, Projectile.Center.Y);
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, toProj, ModContent.ProjectileType<VorpalKnifeSlash>(), Projectile.damage * 2, Projectile.knockBack * 1.5f, Main.myPlayer, 3 * Math.Sign(toProj.X) * player.gravDir, 1f);
				}
			}
			else
			{
				player.direction = Math.Sign(toProj.X);
				if (Main.myPlayer == Projectile.owner)
				{
					for (int j = -1; j <= 1; j++)
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - new Vector2(0, 32 + Main.rand.NextFloat(-8, 8)), new Vector2(j * 0.4f + Main.rand.NextFloat(-0.1f, 0.1f), -1), ModContent.ProjectileType<GreenLightning2>(), 0, 0, Main.myPlayer, -1);
					Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), player.Center, Projectile.velocity, ModContent.ProjectileType<VorpalKnifeSlash>(), Projectile.damage * 2, Projectile.knockBack * 1.5f, player.whoAmI, 1 * player.direction * player.gravDir, 0.8f);
					if (proj.ModProjectile is VorpalKnifeSlash v)
					{
						v.distance = 190;
					}
				}
				player.velocity.X *= 0.12f;
				player.velocity.Y *= 0.08f;
				player.velocity.Y -= 6.66f;
				SOTSUtils.PlaySound(SoundID.Item94, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.05f, 0.2f);
			}
			player.Teleport(Projectile.Center + new Vector2(0, -16), -1, 0);
			player.immune = true;
			player.immuneTime = 60;
		}
    }
}
		