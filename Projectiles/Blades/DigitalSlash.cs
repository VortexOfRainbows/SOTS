using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.IO;
using SOTS.Utilities;

namespace SOTS.Projectiles.Blades
{    
    public class DigitalSlash : ModProjectile //, IPixellated
    {
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Digital Slash");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;    
		}        
		public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 32; 
            Projectile.timeLeft = 100;
            Projectile.penetrate = -1; 
            Projectile.friendly = true; 
            Projectile.hostile = false; 
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true; 
            Projectile.DamageType = DamageClass.Melee; 
			Projectile.alpha = 0;
			Projectile.hide = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.ownerHitCheck = true;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.damage = Math.Max((int)(Projectile.damage * 0.95f), 1);
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Player player = Main.player[Projectile.owner];
			Vector2 center = player.Center;
			float point = 0f;
			Vector2 previousPosition = Projectile.Center;
			float scale = Projectile.scale * 1f;
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), previousPosition, center, 24f * scale, ref point))
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
			Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Blades/DigitalSlashBlade");
			Texture2D texture3 = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Blades/DigitalSlashBlade2");
			Vector2 toProjectile = Projectile.Center - player.RotatedRelativePoint(player.MountedCenter, true);
			int length = (int)toProjectile.Length() / 2 - 8;
			Vector2 rotateToPosition = relativePoint(toProjectile);
			Vector2 drawPos = player.Center + rotateToPosition - Main.screenPosition;
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);

			int direction = 1;
			if (toCursor.X < 0)
			{
				direction = -1;
				direction *= -(int)FetchDirection;
			}
			else
				direction *= (int)FetchDirection;
			float rotation = toProjectile.ToRotation() + MathHelper.ToRadians(direction == -1 ? -225 : 45);
			for (int i = 0; i < length + 1; i++)
			{
				Color color1 = Color.Lerp(new Color(0, 110, 170), new Color(122, 243, 255), 1f - (float)i / length);
				color1 = color1.MultiplyRGBA(new Color(70, 70, 80, 0));
				Vector2 toProj2 = rotateToPosition + rotateToPosition.SafeNormalize(Vector2.Zero) * (i * 2);
				for (int l = 0; l < 3; l++)
					spriteBatch.Draw(texture3, player.Center + toProj2 - Main.screenPosition + new Vector2(0, Main.rand.NextFloat(0.25f, 1f) * l * 0.5f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360))), null, color1, rotation, origin + new Vector2(0, 3), Projectile.scale, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				if (i < length && i % 4 != 3)
					spriteBatch.Draw(texture2, player.Center + toProj2 - Main.screenPosition, null, Color.Black, rotation, origin + new Vector2(0, 1), 1.05f, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}

			spriteBatch.Draw(texture, drawPos, null, Color.White, rotation, origin, Projectile.scale * 1.2f, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
		}
		float counter = 225;
		float spinSpeed = 0;
		int storeData = -1;
		int storeData2 = -1;
		bool starttrail = false;
        public override void PostAI()
		{
			Player player = Main.player[Projectile.owner];
			if (starttrail)
			{
				if (storeData == -1 && Projectile.owner == Main.myPlayer)
				{
					storeData = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, ModContent.ProjectileType<DigitalTrail>(), (int)(Projectile.damage * 1f) + 1, Projectile.knockBack * 0.5f, Projectile.owner, 1, Projectile.identity);
					Projectile.localAI[1] = storeData;
					Projectile.netUpdate = true;
				}
				if (storeData2 == -1 && Projectile.owner == Main.myPlayer)
				{
					storeData2 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, ModContent.ProjectileType<DigitalTrail>(), (int)(Projectile.damage * 1f) + 1, Projectile.knockBack * 0.5f, Projectile.owner, -1, Projectile.identity);
					Projectile.localAI[0] = storeData2;
					Projectile.netUpdate = true;
				}
			}
			starttrail = true;
			if (Projectile.hide == false && toCursor != Vector2.Zero)
			{
				Vector2 toProjectile = Projectile.Center - player.RotatedRelativePoint(player.MountedCenter, true);
				int direction = 1;
				if (toCursor.X < 0)
					direction = -1;
				Projectile.alpha = 0;
				player.ChangeDir(direction);
				player.heldProj = Projectile.whoAmI;
				player.itemTime = 2;
				player.itemAnimation = 2;
				player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, 0f);
				player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathHelper.WrapAngle(toProjectile.ToRotation() + MathHelper.ToRadians(player.gravDir * -90 + (FetchDirection == -1 ? -15 : 15))));
			}
			Projectile.hide = false;
		}
		Vector2 dustAway = Vector2.Zero;
		Vector2 cursorArea = Vector2.Zero;
		Vector2 toCursor = Vector2.Zero;
		bool runOnce = true;
		float distance = 0;
		float counterOffset;
		float timeLeftCounter = 0;
		public int FetchDirection => Math.Sign(Projectile.ai[0]);
		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			float randMod = Projectile.ai[1];
			if (runOnce)
			{
				SOTSUtils.PlaySound(SoundID.Item71, (int)player.Center.X, (int)player.Center.Y, 0.9f, 1f * randMod);
				if (Main.myPlayer == Projectile.owner)
				{
					cursorArea = Main.MouseWorld;
					Projectile.netUpdate = true;
					distance = Vector2.Distance(player.Center, cursorArea) * randMod;
					if (distance < 120)
						distance = 120;
					if (distance > 320)
						distance = 320;
					toCursor = cursorArea - player.Center;
					spinSpeed = (1.0f + (4.4f / (float)Math.Pow(distance / 100f, 1.9f))) * randMod * 5f * SOTSPlayer.ModPlayer(player).attackSpeedMod * player.GetAttackSpeed(DamageClass.Melee);
				}
				counterOffset = 205 + 45f / randMod;
				float slashOffset = counterOffset * FetchDirection;
				counter = slashOffset;
				runOnce = false;
			}
			return base.PreAI();
        }
        public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			float randMod = Projectile.ai[1];
			float mult = 1f * Projectile.ai[1];
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.2f / 255f, (255 - Projectile.alpha) * 0.7f / 255f, (255 - Projectile.alpha) * 1.0f / 255f);
			if(toCursor != Vector2.Zero)
			{
				float distMult = 10f / (float)Math.Pow(distance, 0.5);
				double deg = counter; 
				double rad = deg * (Math.PI / 180);
				Vector2 ovalArea = new Vector2(distance * 0.25f * mult, 0).RotatedBy(toCursor.ToRotation());
				Vector2 ovalArea2 = new Vector2(distance * 0.75f * mult, 0).RotatedBy((float)rad);
				ovalArea2.Y *= 1f / randMod * distMult;
				ovalArea2 = ovalArea2.RotatedBy(toCursor.ToRotation());
				ovalArea.X += ovalArea2.X;
				ovalArea.Y += ovalArea2.Y;
				Projectile.position = player.Center + ovalArea - new Vector2(Projectile.width/2, Projectile.height/2); 
				dustAway = ovalArea;
				Projectile.rotation = dustAway.ToRotation();
			}
			float iterator2 = (float)Math.Abs(spinSpeed * FetchDirection / randMod);
			timeLeftCounter += iterator2;
			counter += spinSpeed * FetchDirection / randMod;
			if(timeLeftCounter > (235.0f + (4000f / distance)) / randMod) //complete a bigger arc with lower distance
            {
				Projectile.hide = true;
				Projectile.Kill();
            }
			else
            {
				if (dustAway != Vector2.Zero && starttrail)
				{
					float amt = Main.rand.NextFloat(2, 3) * distance / 300f;
					for (int i = 0; i < amt; i++)
					{
						float dustScale = 1f;
						float rand = Main.rand.NextFloat(0.9f, 1.35f);
						int type = ModContent.DustType<Dusts.CodeDust2>();
						if (Main.rand.NextBool(3))
                        {
							type = DustID.Electric;
							dustScale *= 0.3f;
						}
						int num = Dust.NewDust(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12) + dustAway.SafeNormalize(Vector2.Zero) * 32, 16, 16, type);
						Dust dust = Main.dust[num];
						dust.velocity *= 1.2f / rand;
						dust.velocity += dustAway.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1.5f, 2.4f) * rand;
						dust.noGravity = true;
						dust.scale *= 0.5f / rand;
						dust.scale += 2.2f / rand * dustScale;
					}
					Vector2 toProjectile = Projectile.Center - player.RotatedRelativePoint(player.MountedCenter, true);
					for (int i = 0; i < amt / 2; i++)
					{
						float rand = Main.rand.NextFloat(0.9f, 1.35f);
						int type = ModContent.DustType<Dusts.CodeDust2>();
						if (Main.rand.NextBool(2))
						{
							type = DustID.Electric;
							rand *= 0.3f;
						}
						int num = Dust.NewDust(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12) + -toProjectile * Main.rand.NextFloat(0.95f), 16, 16, type);
						Dust dust = Main.dust[num];
						dust.velocity *= 0.15f / rand;
						dust.velocity += dustAway.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(90 * FetchDirection)) * Main.rand.NextFloat(0.5f, 1f) * rand;
						dust.noGravity = true;
						dust.scale *= 0.15f / rand;
						dust.scale += 1.00f * rand;
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
			writer.Write(Projectile.localAI[0]);
			writer.Write(Projectile.localAI[1]);
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
			Projectile.localAI[0] = reader.ReadSingle();
			Projectile.localAI[1] = reader.ReadSingle();
		}
    }
}
		
			