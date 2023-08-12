using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Audio;

namespace SOTS.Projectiles.Otherworld
{    
    public class ArclightBomb : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Arclight Bomb");
		}
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(48);
            AIType = 48; 
			// Projectile.thrown = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			// Projectile.magic = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			// Projectile.melee = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 900;
			Projectile.alpha = 0;
		}
        public override void PostDraw(Color lightColor)
        {
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Otherworld/ArclightBomb");
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			if (modPlayer.rainbowGlowmasks)
			{
				Color color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0);
				for (int k = 0; k < 3; k++)
				{
					Main.spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				}
			}
			else
			{
				Color color = Color.White;
				Main.spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
		}
		public override void AI()
		{
			if(Projectile.timeLeft >= 800)
			{
				Projectile.scale = 0.01f * Main.rand.Next(102, 141);
				Projectile.timeLeft = Main.rand.Next(32, 46);
			}
		}
		public override void Kill(int timeLeft)
        {
			SOTSUtils.PlaySound(SoundID.NPCHit53, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.625f);
			for (int i = 0; i < 13; i++)
			{
				var num371 = Dust.NewDust(Projectile.Center - new Vector2(5) - new Vector2(10, 10), 24, 24, ModContent.DustType<Dusts.CopyDust4>(), 0, 0, 100, default, 1.6f);
				Dust dust = Main.dust[num371];
				dust.velocity += Projectile.velocity * 0.1f;
				dust.noGravity = true;
				dust.color = Color.Lerp(new Color(160, 200, 220, 100), new Color(120, 140, 180, 100), new Vector2(-0.5f, 0).RotatedBy(Main.rand.Next(360)).X + 0.5f);
				dust.noGravity = true;
				dust.fadeIn = 0.2f;
				dust.alpha = Projectile.alpha;
			}
			if (Projectile.owner == Main.myPlayer)
			{
				for(int i = 0; i < Main.rand.Next(2) + 2; i++)
				{
					Vector2 circular = new Vector2(1.65f, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
					Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, circular, ModContent.ProjectileType<ArcColumn>(), (int)(Projectile.damage * 0.8f), 0, Projectile.owner, 1 + Main.rand.Next(2));
				}
			}
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			if (Projectile.owner == Main.myPlayer)
			{
				int npcIndex = -1;
				double distanceTB = 216;
				for (int i = 0; i < 200; i++) //find first enemy
				{
					NPC npc = Main.npc[i];
					if (!npc.friendly && npc.lifeMax > 5 && npc.active && !npc.dontTakeDamage)
					{
						if (npcIndex != i && target.whoAmI != i && npc.whoAmI != (int)Projectile.ai[0])
						{
							float disX = Projectile.Center.X - npc.Center.X;
							float disY = Projectile.Center.Y - npc.Center.Y;
							double dis = Math.Sqrt(disX * disX + disY * disY);
							if (dis < distanceTB)
							{
								distanceTB = dis;
								npcIndex = i;
							}
						}
					}
				}
				if (npcIndex != -1)
				{
					NPC npc = Main.npc[npcIndex];
					if (!npc.friendly && npc.lifeMax > 5 && npc.active && !npc.dontTakeDamage)
					{
						Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ArcLightningZap>(), (int)(Projectile.damage * 0.8f) + 1, target.whoAmI, Projectile.owner, npc.whoAmI, 2);
					}
				}
			}
		}
	}
}
			