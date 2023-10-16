using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Projectiles.Laser;
using SOTS.Common.GlobalNPCs;

namespace SOTS.Projectiles.Minions
{    
    public class BlizzardProbe : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = false;
            Main.projPet[Projectile.type] = false;
            Main.projFrames[Projectile.type] = 1;
        }
        public override void SetDefaults()
        {
			Projectile.DamageType = DamageClass.Summon;
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.timeLeft = 300;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 0;
            Projectile.netImportant = true;
		}
		public override void AI()
        {
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			float speedRotation = Projectile.velocity.ToRotation();
            float targetRotation = speedRotation;
			if (Projectile.timeLeft > 100)
			{
				Projectile.timeLeft = 300;
            }
            int targetID = SOTSNPCs.FindTarget_Basic(player.Center + new Vector2(0, -16), 720, needsLOS: true);
            if (targetID != -1 && Projectile.Center.Distance(player.Center) < 1440)
            {
                Projectile.ai[1] += 1;
                NPC target = Main.npc[targetID];
                Vector2 shootTo = target.Center - Projectile.Center;
                targetRotation = shootTo.ToRotation();
                if ((int)Projectile.ai[1] % 90 == 0 && player.whoAmI == Main.myPlayer)
                {
                    LaunchLaser(target.Center);
                    Projectile.netUpdate = true;
                }
            }
            Vector2 initialLoop = new Vector2(164, 0).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter * 1.5f + 45 * Projectile.ai[0] + Projectile.ai[0] % 2 * 45));

			if((int)Projectile.ai[0] % 2 == 0)
			{
				initialLoop.X *= 0.25f;
			}
			else
			{
				initialLoop.Y *= 0.25f;
			}
			float firingProgress = (Projectile.ai[1] % 90 - 50f) / 40f;
            if (targetID == -1)
				firingProgress = 0f;
            firingProgress = MathHelper.Clamp(firingProgress, 0, 1f);
            if (FireProgress > firingProgress)
				FireProgress = MathHelper.Lerp(FireProgress, firingProgress, 0.1f);
            else
                FireProgress = MathHelper.Lerp(FireProgress, firingProgress, firingProgress);
            Vector2 properLoop = new Vector2(initialLoop.X, initialLoop.Y).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter * 1.5f)) + player.Center;
			Vector2 travelToLoopPosition = properLoop - Projectile.Center;
			float length = travelToLoopPosition.Length();
			float speed = 15f + length * 0.05f;
			if(speed > length)
			{
				speed = length;
			}
			speed *= (1 - FireProgress * 0.8f);
            Projectile.velocity *= 0.95f * (1 - FireProgress * 0.8f);
			Projectile.velocity = Vector2.Lerp(Projectile.velocity, travelToLoopPosition.SafeNormalize(Vector2.Zero) * speed, 0.06f);
			if (targetRotation != speedRotation)
			{
				Projectile.rotation = MathHelper.Lerp(Projectile.rotation, targetRotation, 0.1f);
			}
			else
				Projectile.rotation = speedRotation;
		}
		public float FireProgress;
        public void LaunchLaser(Vector2 area)
		{
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + new Vector2(12, 0).RotatedBy(Projectile.rotation), Vector2.Zero, ModContent.ProjectileType<BrightRedLaser>(), Projectile.damage, 1f, Projectile.owner, area.X, area.Y);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D probeTexture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Texture2D glowTexture = ModContent.Request<Texture2D>("SOTS/Projectiles/Minions/BlizzardProbeGlow").Value;
            Vector2 drawOrigin = new Vector2(probeTexture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) 
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				float alphaMult = (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length;
                Color color = Projectile.GetAlpha(lightColor) * alphaMult;
				Main.spriteBatch.Draw(probeTexture, drawPos, null, color, Projectile.rotation + MathHelper.PiOver4, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(glowTexture, drawPos, null, Color.White * alphaMult * 0.5f, Projectile.rotation + MathHelper.PiOver4, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            Main.spriteBatch.Draw(probeTexture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + MathHelper.PiOver4, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(glowTexture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + MathHelper.PiOver4, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            if (FireProgress != 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    Vector2 circular = new Vector2(16 * (1 - FireProgress), 0).RotatedBy(Projectile.rotation + i * MathHelper.PiOver4);
                    Main.spriteBatch.Draw(glowTexture, circular + Projectile.Center - Main.screenPosition, null, new Color(100, 50, 60, 0) * FireProgress, Projectile.rotation + MathHelper.PiOver4, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
                }
            }
            return false;
		}
		public override void SendExtraAI(BinaryWriter writer) 
		{

		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	

		}
	}
}
		