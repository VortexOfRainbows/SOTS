using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.IO;
using SOTS.Utilities;
using SOTS.Void;
using SOTS.Prim.Trails;

namespace SOTS.Projectiles.Blades
{    
    public class ToothAcheSlash : ModProjectile //, IPixellated
    {
		public static Color toothAcheLime = new Color(174, 213, 56);
		public static Color toothAcheGreen = new Color(110, 132, 22);
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			target.AddBuff(BuffID.Poisoned, 180);
        }
        public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;    
		}        
		public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32; 
            Projectile.timeLeft = 100;
            Projectile.penetrate = -1; 
            Projectile.friendly = true; 
            Projectile.hostile = false; 
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true; 
            Projectile.DamageType = ModContent.GetInstance<VoidMelee>(); 
			Projectile.alpha = 0;
			Projectile.hide = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 4;
			Projectile.ownerHitCheck = true;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Player player = Main.player[Projectile.owner];
			Vector2 center = player.Center;
			float point = 0f;
			Vector2 previousPosition = Projectile.Center;
			float scale = Projectile.scale * 1f;
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), previousPosition + dustAway.SafeNormalize(Vector2.Zero) * 36, center, 40f * scale, ref point))
			{
				return true;
			}
			return false;
		}
		public override bool ShouldUpdatePosition()
        {
            return false;
        }
		public Vector2 relativePoint(Vector2 toArea, float length = 24)
        {
			Vector2 velo = Vector2.Zero;
			float num1 = length * Projectile.scale;
			float num2 = toArea.X;
			float num3 = toArea.Y;
			float num5 = (float)Math.Sqrt(num2 * (double)num2 + num3 * (double)num3);
			float num6 = num1 / num5;
			float num7 = num2 * num6;
			float num8 = num3 * num6;
			velo.X = num7;
			velo.Y = num8;
			return velo;
		}
        public override bool PreDraw(ref Color lightColor)
		{
			Draw(Main.spriteBatch);
			return false;
		}
		public void Draw(SpriteBatch spriteBatch)
        {
			Player player = Main.player[Projectile.owner];
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 toProjectile = Projectile.Center - player.RotatedRelativePoint(player.MountedCenter, true);
			int length = (int)toProjectile.Length();
			Vector2 rotateToPosition = relativePoint(toProjectile, 20);
			Vector2 drawPos = player.Center + rotateToPosition - Main.screenPosition;
			Vector2 origin = new Vector2(12, 60);

			int direction = 1;
			if (toCursor.X < 0)
			{
				direction = -1;
				direction *= -(int)FetchDirection;
			}
			else
				direction *= (int)FetchDirection;
			if(direction == -1)
			origin = new Vector2(texture.Width - 12, 60);
			float standardSwordLength = (float)Math.Sqrt(texture.Width * texture.Width + texture.Height * texture.Height) - 16;
			float scaleMultiplier = length / standardSwordLength;
			float rotation = toProjectile.ToRotation() + MathHelper.ToRadians(direction == -1 ? -225 : 45);
			spriteBatch.Draw(texture, drawPos, null, Color.White, rotation, origin, 0.1f + 1f * scaleMultiplier, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
		}
		float counter = 225;
		float spinSpeed = 0;
        public override void PostAI()
		{
			Player player = Main.player[Projectile.owner];
			if (Projectile.hide == false && toCursor != Vector2.Zero)
			{
				Vector2 toProjectile = Projectile.Center - player.RotatedRelativePoint(player.MountedCenter, true);
				int direction = 1;
				if (toCursor.X < 0)
					direction = -1;
				Projectile.alpha = 0;
				player.ChangeDir(direction);
				player.heldProj = Projectile.whoAmI;
				player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, 0f);
				player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathHelper.WrapAngle(toProjectile.ToRotation() + MathHelper.ToRadians(player.gravDir * -90 + (FetchDirection == -1 ? -12 : 12))));
				player.itemTime = 4;
				player.itemAnimation = 4;
			}
			Projectile.hide = false;
		}
		Vector2 dustAway = Vector2.Zero;
		Vector2 cursorArea = Vector2.Zero;
		Vector2 toCursor = Vector2.Zero;
		bool runOnce = true;
		public float distance = 0;
		float counterOffset;
		float timeLeftCounter = 0;
		public int GetArcLength()
		{
			Player player = Main.player[Projectile.owner];
			Vector2 toProjectile = Projectile.Center - player.RotatedRelativePoint(player.MountedCenter, true);
			int length = (int)toProjectile.Length();
			return length;
		}
        public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			float randMod = Projectile.ai[1];
			int AbsAI0 = (int)Math.Abs(Projectile.ai[0]);
			if (runOnce)
			{
				SOTS.primitives.CreateTrail(new FireTrail(Projectile, clockWise: FetchDirection, toothAcheLime.ToVector4(), toothAcheGreen.ToVector4(), 20, 1));
				SOTSUtils.PlaySound(SoundID.Item71, (int)player.Center.X, (int)player.Center.Y, 0.75f, 0.75f * randMod);
				if (Main.myPlayer == Projectile.owner)
				{
					cursorArea = Main.MouseWorld;
					Projectile.netUpdate = true;
					if(distance == 0)
					{
						distance = Vector2.Distance(player.Center, cursorArea) * randMod;
						if (distance < 108)
							distance = 108;
						if (distance > 124)
							distance = 124;
					}
					toCursor = cursorArea - player.Center;
					spinSpeed = (2.2f + (1.2f / (float)Math.Pow(distance / 120f, 2f))) * randMod * 5f * (0.7f + 0.3f * (SOTSPlayer.ModPlayer(player).attackSpeedMod * player.GetAttackSpeed(DamageClass.Melee)));
					if (AbsAI0 == 2)
						spinSpeed *= 0.6f;
				}
				counterOffset = 200 + 15f / randMod;
				float slashOffset = counterOffset * FetchDirection;
				counter = slashOffset;
				runOnce = false;
			}
			return base.PreAI();
		}
		public int FetchDirection => Math.Sign(Projectile.ai[0]);
		public override void Kill(int timeLeft)
		{
			Player player = Main.player[Projectile.owner];
			if (Projectile.owner == Main.myPlayer && !player.dead)
			{
				int AbsAI0 = (int)Math.Abs(Projectile.ai[0]);
				if (AbsAI0 > 1)
				{
					AbsAI0--;
					float speedBonus = 0f;
					if(AbsAI0 == 3)
                    {
						speedBonus = 0.3f;
					}
					if (AbsAI0 == 2)
					{
						speedBonus = -0.3f;
					}
					if (AbsAI0 == 1)
					{
						speedBonus = 0.1f;
					}
					int damage = Projectile.damage;
					if (AbsAI0 == 1)
					{
						damage = (int)(damage * 1.0f);
						Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), player.Center, (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 9f, ModContent.ProjectileType<ToothAcheThrow>(), (int)(damage * 0.8f), Projectile.knockBack, player.whoAmI, 0, FetchDirection);
					}
					else
					{
						Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), player.Center, Projectile.velocity, Type, damage, Projectile.knockBack, player.whoAmI, -FetchDirection * AbsAI0, Projectile.ai[1] + speedBonus);
						if (proj.ModProjectile is ToothAcheSlash a)
						{
							a.distance = distance;
							if (AbsAI0 == 3)
								a.distance = distance * 1.08f + 24;
							else if (AbsAI0 == 2)
								a.distance = distance * 0.9f;
							else	
								a.distance = distance * 0.95f + 16;
						}
					}
				}
			}
		}
        public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			float randMod = Projectile.ai[1];
			float mult = 0.7f + 0.3f * Projectile.ai[1];
			int AbsAI0 = (int)Math.Abs(Projectile.ai[0]);
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.2f / 255f, (255 - Projectile.alpha) * 0.7f / 255f, (255 - Projectile.alpha) * 1.0f / 255f);
			if(toCursor != Vector2.Zero)
			{
				float yDistMult = (float)Math.Pow(distance / 124f, 0.5);
				double deg = counter; 
				double rad = deg * (Math.PI / 180);
				Vector2 ovalArea = new Vector2(distance * 0.25f * mult, 0).RotatedBy(toCursor.ToRotation()); //center a point somewhat distant from the player
				Vector2 ovalArea2 = new Vector2(distance * 0.75f * mult, 0).RotatedBy((float)rad); //create a circle
				ovalArea2.Y *= 0.85f / randMod * yDistMult; //turn circle into an oval by compressing the y value
				if (AbsAI0 == 3)
					ovalArea2.Y *= 0.4f;
				if (AbsAI0 == 2)
					ovalArea2.Y *= 0.8f;
				ovalArea2 = ovalArea2.RotatedBy(toCursor.ToRotation());
				ovalArea.X += ovalArea2.X;
				ovalArea.Y += ovalArea2.Y;
				Projectile.position = player.Center + ovalArea - new Vector2(Projectile.width/2, Projectile.height/2); 
				dustAway = ovalArea;
				Projectile.rotation = dustAway.ToRotation();
			}
			float incremendAmount = spinSpeed * FetchDirection;
			float iterator2 = (float)Math.Abs(incremendAmount);
			timeLeftCounter += iterator2;
			counter += incremendAmount;
			float totalSwipeDegrees = (270.0f + (1800f / distance));
			if (AbsAI0 == 3)
				totalSwipeDegrees += 10;
			if (timeLeftCounter > totalSwipeDegrees) //complete a bigger arc with a lower distance
            {
				Projectile.hide = true;
				Projectile.Kill();
            }
			else
            {
				if (dustAway != Vector2.Zero)
				{
					float amt = Main.rand.NextFloat(1.0f, 1.4f) * distance / 180f;
					for (int i = 0; i < amt * 0.6f; i++) //generates dust at the end of the blade
					{
						float dustScale = 1f;
						float rand = Main.rand.NextFloat(0.9f, 1.1f);
						int type = ModContent.DustType<Dusts.CopyDust4>();
						if (Main.rand.NextBool(5))
							type = DustID.JungleSpore;
						Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12) + dustAway.SafeNormalize(Vector2.Zero) * 24, 16, 16, type);
						dust.velocity *= 0.8f / rand;
						dust.velocity += dustAway.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1.2f, 2.0f) * rand;
						dust.noGravity = true;
						dust.scale *= 0.2f / rand;
						dust.scale += 1.3f / rand * dustScale;
						dust.fadeIn = 0.1f;
						if (type == ModContent.DustType<Dusts.CopyDust4>())
							dust.color = Color.Lerp(toothAcheGreen, toothAcheLime, Main.rand.NextFloat(0.9f) * Main.rand.NextFloat(0.9f));
					}
					Vector2 toProjectile = Projectile.Center - player.RotatedRelativePoint(player.MountedCenter, true);
					for (int i = 0; i < amt; i++) //generates dust throughout the length of the blade
					{
						float rand = Main.rand.NextFloat(0.9f, 1.1f);
						int type = ModContent.DustType<Dusts.CopyDust4>();
						if (Main.rand.NextBool(3))
							type = DustID.JungleSpore;
						Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12) + (toProjectile.SafeNormalize(Vector2.Zero)) * 24 - toProjectile * Main.rand.NextFloat(0.95f), 16, 16, type);
						dust.velocity *= 0.1f / rand;
						dust.velocity += dustAway.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(90 * FetchDirection)) * Main.rand.NextFloat(0.4f, 0.9f) * rand;
						dust.noGravity = true;
						dust.scale *= 0.2f / rand;
						dust.scale += 1.1f * rand;
						dust.fadeIn = 0.1f;
						if (type == ModContent.DustType<Dusts.CopyDust4>())
							dust.color = Color.Lerp(toothAcheLime, toothAcheGreen, Main.rand.NextFloat(0.9f) * Main.rand.NextFloat(0.9f));
					}
				}
			}
		}
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(counter);
			writer.Write(spinSpeed);
			writer.Write(toCursor.X);
			writer.Write(toCursor.Y);
			writer.Write(cursorArea.X);
			writer.Write(cursorArea.Y);
			writer.Write(distance);
		}
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			counter = reader.ReadSingle();
			spinSpeed = reader.ReadSingle();
			toCursor.X = reader.ReadSingle();
			toCursor.Y = reader.ReadSingle();
			cursorArea.X = reader.ReadSingle();
			cursorArea.Y = reader.ReadSingle();
			distance = reader.ReadSingle();
		}
    }
}
		
			