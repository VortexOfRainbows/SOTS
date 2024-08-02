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

namespace SOTS.Projectiles.Blades
{    
    public class VertebraekerThrow : ModProjectile 
    {
		public override string Texture => "SOTS/Projectiles/Blades/VertebraekerSlash";
        float rotation = 0;
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.tileCollide);
			writer.Write(Projectile.velocity.X);
			writer.Write(Projectile.velocity.Y);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.tileCollide = reader.ReadBoolean();
			Projectile.velocity.X = reader.ReadSingle();
			Projectile.velocity.Y = reader.ReadSingle();
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
			Projectile.timeLeft = 180;
			Projectile.tileCollide = true;
			Projectile.hostile = false;
			Projectile.alpha = 0;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.extraUpdates = 2;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = 110;
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
        public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			if (runOnce)
			{
				SOTSUtils.PlaySound(SoundID.DD2_GhastlyGlaivePierce, (int)player.Center.X, (int)player.Center.Y, 1.6f, -0.1f);
				runOnce = false;
				initialVelo = Projectile.velocity;
				if(Projectile.velocity.X > 0)
				{
					rotation = -MathHelper.ToRadians(90);
					initialDirection = 1;
				}
				else
				{
					rotation = 0;
					initialDirection = -1;
				}
				initialCenter = player.Center;
				Projectile.ai[0] = -180 * initialDirection;
				Projectile.scale = 1.5f;
				BladeTrail myTrail = new BladeTrail(Projectile, clockWise: initialDirection, VertebraekerSlash.vertebraekerRed.ToVector4(), VertebraekerSlash.vertebraekerOrange.ToVector4(), 36, 2);
                SOTS.primitives.CreateTrail(myTrail);
			}
			else if(Projectile.timeLeft % 18 == 0)
				SOTSUtils.PlaySound(SoundID.DD2_MonkStaffSwing, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.2f, 0.2f);
			player.itemAnimation = 3;
			player.itemTime = 3;
			player.itemRotation = MathHelper.WrapAngle((Projectile.Center - player.Center).ToRotation() + (initialDirection == -1 ? MathHelper.ToRadians(180) : 0));
			Projectile.ai[0] += 3f * initialDirection;
			if (Projectile.timeLeft >= 90)
			{
				Vector2 initialCenter = this.initialCenter;
				int length = 120;
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
				Projectile.velocity = circular + new Vector2(0, -1.5f);
			}
			else
            {
				float dist = (Projectile.Center - player.Center).Length();
				float maxSpeed = 16 + (8 * (90 - Projectile.timeLeft) / 90f);
				Vector2 circular = new Vector2(-(dist > maxSpeed ? maxSpeed : dist), 0).RotatedBy((Projectile.Center - player.Center).ToRotation());
				Projectile.velocity = circular;
				Projectile.tileCollide = false;
				if((Projectile.Center - player.Center).Length() <= 12)
                {
					Projectile.Kill();
                }
			}
            return base.PreAI();
        }
        public override void AI()
		{
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.6f / 255f, (255 - Projectile.alpha) * 0.6f / 255f, (255 - Projectile.alpha) * 1.8f / 255f);
			Player player = Main.player[Projectile.owner];
			rotation += -initialDirection * MathHelper.ToRadians(16);
			Projectile.rotation = rotation;
			Projectile.spriteDirection = initialDirection;

			if(Main.rand.NextBool(3))
			{
				for (int i = 0; i < 1 + Main.rand.NextFloat(-0.5f, 0.2f); i++) //generates dust at the end of the blade
				{
					Vector2 circular = Main.rand.NextVector2CircularEdge(70, 70);
					circular = circular.RotateRandom(Projectile.rotation) * Main.rand.NextFloat(0.9f + i * 0.3f, 1.2f);
					float dustScale = 1.0f;
					float rand = Main.rand.NextFloat(0.9f, 1.1f);
					int type = ModContent.DustType<Dusts.CopyDust4>();
					if (Main.rand.NextBool(4))
						type = DustID.RedTorch;
					Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4) + circular, 0, 0, type);
					dust.velocity *= 0.4f / rand;
					dust.velocity += circular.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(3.2f, 3.6f) * rand;
					dust.noGravity = true;
					dust.scale *= 0.2f / rand;
					dust.scale += 1.2f / rand * dustScale;
					dust.fadeIn = 0.1f;
					if (type == ModContent.DustType<Dusts.CopyDust4>())
						dust.color = Color.Lerp(VertebraekerSlash.vertebraekerOrange, VertebraekerSlash.vertebraekerRed, Main.rand.NextFloat(0.9f) * Main.rand.NextFloat(0.9f));
				}
			}				
			if (Projectile.timeLeft >= 110)
				Projectile.Center += Collision.TileCollision(Projectile.Center - new Vector2(12, 12), Projectile.velocity, 24, 24, true);
			else
				Projectile.Center += Projectile.velocity;
			foreach (PrimTrail trail in SOTS.primitives._trails.ToArray())
			{
				if (trail is BladeTrail fireTrail)
				{
					if (trail.Entity is Projectile proj && proj.whoAmI == Projectile.whoAmI && proj.type == Projectile.type && initialDirection == fireTrail.ClockWiseOrCounterClockwise)
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
	}
}
		