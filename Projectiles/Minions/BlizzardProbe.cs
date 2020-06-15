using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Diagnostics;

namespace SOTS.Projectiles.Minions
{    
    public class BlizzardProbe : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blizzard Probe");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;  
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			Main.projPet[projectile.type] = false;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = false;
		}
		
        public override void SetDefaults()
        {
			projectile.width = 26;
			projectile.height = 26;
            Main.projFrames[projectile.type] = 1;
			projectile.penetrate = -1;
			projectile.friendly = false;
			projectile.timeLeft = 960;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.minion = true;
			projectile.alpha = 0;
            projectile.netImportant = true;
		}
		public int FindClosestEnemy()
		{
			Player player = Main.player[projectile.owner];
			float minDist = 600;
			int target2 = -1;
			float dX = 0f;
			float dY = 0f;
			float distance = 0;
			if (player.HasMinionAttackTargetNPC)
			{
				NPC target = Main.npc[player.MinionAttackTargetNPC];
				bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, target.position, target.width, target.height);
				dX = target.Center.X - projectile.Center.X;
				dY = target.Center.Y - projectile.Center.Y;
				distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
				if (distance < minDist && lineOfSight)
				{
					minDist = distance;
					target2 = player.MinionAttackTargetNPC;
				}
			}
			for (int i = 0; i < Main.npc.Length - 1; i++)
			{
				NPC target = Main.npc[i];
				if (target.CanBeChasedBy())
				{
					bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, target.position, target.width, target.height);
					dX = target.Center.X - projectile.Center.X;
					dY = target.Center.Y - projectile.Center.Y;
					distance = (float) Math.Sqrt((double)(dX * dX + dY * dY));
					if(distance < minDist && lineOfSight)
					{
						minDist = distance;
						target2 = i;
					}
				}
			}
			return target2;
		}
		int targetValue = 120;
		public override void AI()
        {
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			if (player.whoAmI == Main.myPlayer)
			{
				if (modPlayer.orbitalCounter % 60 == 0)
				{
					projectile.netUpdate = true;
				}
				Vector2 playerCursor = Main.MouseWorld;
				
				float shootToX = playerCursor.X - projectile.Center.X;
				float shootToY = playerCursor.Y - projectile.Center.Y;
						
				projectile.tileCollide = true;
				if(FindClosestEnemy() == -1)
				{
					projectile.rotation = (float)Math.Atan2((double)shootToY, (double)shootToX) + MathHelper.ToRadians(45);
				}
				else
				{
					projectile.ai[1] += 1;
					NPC target = Main.npc[FindClosestEnemy()];
					shootToX = target.Center.X - projectile.Center.X;
					shootToY = target.Center.Y - projectile.Center.Y;
					projectile.rotation = (float)Math.Atan2((double)shootToY, (double)shootToX) + MathHelper.ToRadians(45);
					if(projectile.ai[1] >= targetValue)
					{
						targetValue = Main.rand.Next(120,240);
						projectile.ai[1] = 0;
						LaunchLaser(target.Center);
					}
				}
			}
			Vector2 initialLoop = new Vector2(164, 0).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter * 2.15f + 45 * (int)projectile.ai[0]));

			if((int)projectile.ai[0] % 2 == 0)
			{
				initialLoop.X /= 2.0f;
			}
			else
			{
				initialLoop.Y /= 2.0f;
			}

			Vector2 properLoop = new Vector2(initialLoop.X, initialLoop.Y).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter * 2.15f));
			projectile.position.X = properLoop.X + player.Center.X - projectile.width / 2;
			projectile.position.Y = properLoop.Y + player.Center.Y - projectile.height / 2;

		}
		public void LaunchLaser(Vector2 area)
		{
			Player player  = Main.player[projectile.owner];
			int laser = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("BrightRedLaser"), projectile.damage, 0, projectile.owner, area.X, area.Y);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(projectile.rotation);
			writer.Write(projectile.spriteDirection);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			projectile.rotation = reader.ReadSingle();
			projectile.spriteDirection = reader.ReadInt32();
		}
	}
}
		