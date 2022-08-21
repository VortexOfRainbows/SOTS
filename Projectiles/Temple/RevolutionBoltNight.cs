using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Dusts;
using Terraria.Audio;
using SOTS.Common.GlobalNPCs;

namespace SOTS.Projectiles.Temple
{    
    public class RevolutionBoltNight : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2; //also saves rotation and spritedriection  
		}
        public override void SetDefaults()
        {
			Projectile.DamageType = ModContent.GetInstance<Void.VoidMagic>();
			Projectile.friendly = true;
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.timeLeft = 3600;
			Projectile.extraUpdates = 1;
			Projectile.penetrate = -1;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 1;
			Projectile.localNPCHitCooldown = 20;
			Projectile.usesLocalNPCImmunity = true;
		}
		bool runOnce = true;
		float reachY = 0;
		public override bool PreAI()
		{
			if (Projectile.ai[1] < 0)
			{
				if (runOnce)
				{
					runOnce = false;
					Projectile.scale = 0.66f;
					reachY = Projectile.ai[0];
					Projectile.ai[0] = 0;
					Projectile.tileCollide = false;
				}
			}
			if(Projectile.Center.Y > reachY - 16)
            {
				Projectile.tileCollide = true;
            }
			return true;
		}
		public override void AI()
        {
            if (Projectile.ai[0] == -1)
            {
				Projectile.friendly = false;
				Projectile.velocity *= 0;
				Projectile.tileCollide = false;
				if(Projectile.timeLeft > 20)
				{
					for (int i = 0; i < 360; i += 45)
					{
						Vector2 circularLocation = new Vector2(-4, 0).RotatedBy(MathHelper.ToRadians(i));
						Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
						dust.noGravity = true;
						dust.velocity *= 0.3f;
						dust.velocity += circularLocation * 0.1f;
						dust.scale *= 1.3f;
						dust.fadeIn = 0.1f;
						dust.color = new Color(103, 126, 164, 0);
						if(Projectile.scale < 0.8f)
							dust.color = new Color(163, 65, 165, 0);
						dust.alpha = 70;
					}
					Projectile.timeLeft = 20;
				}
			}
			else 
			{
                if (Projectile.ai[1] >= 0)
				{
					float homingRange = (float)(32 + 32 * Math.Sqrt(Projectile.ai[1]));
					if (homingRange > 640)
						homingRange = 640;
					int target = SOTSNPCs.FindTarget_Basic(Projectile.Center, homingRange, this);
					if (target >= 0)
					{
						NPC npc = Main.npc[target];
						Projectile.velocity = Vector2.Lerp(Projectile.velocity, (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * (Projectile.velocity.Length() + 2), 0.04f);
					}
				}
				Projectile.ai[1]++;
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.ai[0] = -1;
			Projectile.netUpdate = true;
			if (runOnce)
			{
				runOnce = false;
				ReleaseProjectiles();
			}
		}
		public void ReleaseProjectiles()
		{
			if (Main.myPlayer == Projectile.owner)
			{
				for (int i = 0; i < 1 + Main.rand.Next(2); i++)
				{
					Vector2 perturbedSpeed = new Vector2(0, 11).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-10, 10))) + Main.rand.NextVector2Circular(3, 3);
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + new Vector2(0, -900), perturbedSpeed, Projectile.type, (int)(Projectile.damage * 0.33f), Projectile.knockBack, Projectile.owner, Projectile.Center.Y, -Main.rand.Next(12, 20));
				}
			}
		}
		public override bool? CanHitNPC(NPC target)
        {
            return (Projectile.ai[0] == -1 || Projectile.ai[1] < 0) ? false : base.CanHitNPC(target);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.ai[0] = -1;
			Projectile.netUpdate = true;
			if (runOnce && Projectile.ai[1] > 0)
				ReleaseProjectiles();
			return false;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			int starting = 0;
			bool drawMain = false;
			if(Projectile.timeLeft <= 20)
            {
				starting = 20 - Projectile.timeLeft;
			}
			else
			{
				drawMain = true;
			}
			for (int k = starting; k < Projectile.oldPos.Length; k++)
			{
				float scale = ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Color toChange = new Color(40, 55, 100, 0);
				if(!runOnce)
					toChange = new Color(70, 25, 100, 0);
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + new Vector2(Projectile.width / 2, Projectile.gfxOffY + Projectile.height / 2);
				Color color = Projectile.GetAlpha(toChange) * scale;
				float rotation = Projectile.oldRot[k];
				float stretch = 2.0f;
				Main.spriteBatch.Draw(texture, drawPos, null, color * 0.9f, rotation + MathHelper.PiOver2, new Vector2(texture.Width / 2, 6), new Vector2(Projectile.scale * scale, stretch), SpriteEffects.None, 0f);
			}
			if(drawMain)
			{
				for (int i = 0; i < 4; i++)
				{
					Color toChange = new Color(80, 100, 200, 0);
					if (!runOnce)
						toChange = new Color(160, 40, 200, 0);
					Vector2 offset = new Vector2(1, 0).RotatedBy(MathHelper.PiOver2 * i);
					Main.spriteBatch.Draw(texture, Projectile.Center + offset - Main.screenPosition, null, toChange, Projectile.rotation + MathHelper.PiOver2, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
				}
			}
			return false;
		}
	}
}
		