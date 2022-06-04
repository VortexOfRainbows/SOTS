using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Otherworld
{    
    public class MacaroniMoon : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Macaroni Moon");
		}
        public override void SetDefaults()
        {
			Projectile.height = 16;
			Projectile.width = 20;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 7200;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.tileCollide = false;
		}
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			crit = false;
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
		}
		public override void PostDraw(Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < 6; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.25f;
				float y = Main.rand.Next(-10, 11) * 0.25f;
				Main.spriteBatch.Draw(texture, new Vector2((float)(Projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(Projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color * ((255 - Projectile.alpha) / 255f), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
		}
		public override bool PreDraw(ref Color lightColor)
        {
			return false;
		}
		bool foundTarget = false;
		int lastNpc = -1;
		public Vector2 FindTarget()
		{
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = player.GetModPlayer<SOTSPlayer>();
			float distanceFromTarget = 1080f;
			Vector2 targetCenter = Projectile.Center;
			Vector2 cursor = Main.MouseWorld;
			if (!foundTarget)
			{
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.CanBeChasedBy() && npc.active)
					{
						float between = Vector2.Distance(npc.Center, Projectile.Center);
						bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
						bool inRange = between < distanceFromTarget;
						bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);

						if (((closest || !foundTarget) && inRange) && lineOfSight)
						{
							distanceFromTarget = between;
							targetCenter = npc.Center + npc.velocity * 2;
							foundTarget = true;
							lastNpc = i;
						}
					}
				}
			}
			else
			{
				NPC npc = Main.npc[lastNpc];
				if (npc.CanBeChasedBy() && npc.active)
				{
					bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
					targetCenter = npc.Center + npc.velocity * 2;
				}
				else
                {
					foundTarget = false;
                }
			}
			if(targetCenter != Projectile.Center)
				return targetCenter;
			return cursor;	
		}
		float spin = 1800;
		public override void AI()
		{
			Projectile.velocity *= 0.96f;
			if(spin > 0)
            {
				spin -= 3;
				spin *= 0.97f;
            }
			if(spin < 0)
            {
				spin = 0;
            }
			if(Main.myPlayer == Projectile.owner)
			{
				Projectile.ai[0] = (FindTarget() - Projectile.Center).ToRotation();
				Projectile.netUpdate = true;
			}
			Projectile.rotation = Projectile.ai[0] + MathHelper.ToRadians(spin);
			if(spin <= 0)
			{
				Projectile.ai[1]++;
				if (Projectile.ai[1] >= 10)
				{
					SOTSUtils.PlaySound(SoundID.Item9, (int)Projectile.Center.X, (int)Projectile.Center.Y,0.75f);
					if (Main.myPlayer == Projectile.owner)
					{
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(4, 0).RotatedBy(Projectile.rotation), Mod.Find<ModProjectile>("MacaroniBeam").Type, Projectile.damage, Projectile.knockBack, Projectile.owner);
					}
					Projectile.Kill();
				}
			}
		}	
	}
}
		