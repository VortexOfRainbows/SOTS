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
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			Main.projPet[Projectile.type] = false;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = false;
		}
		
        public override void SetDefaults()
        {
			Projectile.width = 26;
			Projectile.height = 26;
            Main.projFrames[Projectile.type] = 1;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.timeLeft = 300;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.minion = true;
			Projectile.alpha = 0;
            Projectile.netImportant = true;
		}
		public int FindClosestEnemy()
		{
			Player player = Main.player[Projectile.owner];
			float minDist = 600;
			int target2 = -1;
			float dX = 0f;
			float dY = 0f;
			float distance = 0;
			if (player.HasMinionAttackTargetNPC)
			{
				NPC target = Main.npc[player.MinionAttackTargetNPC];
				bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, target.position, target.width, target.height);
				dX = target.Center.X - Projectile.Center.X;
				dY = target.Center.Y - Projectile.Center.Y;
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
					bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, target.position, target.width, target.height);
					dX = target.Center.X - Projectile.Center.X;
					dY = target.Center.Y - Projectile.Center.Y;
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
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			if (Projectile.timeLeft > 100)
			{
				Projectile.timeLeft = 300;
			}
			if (player.whoAmI == Main.myPlayer)
			{
				if (modPlayer.orbitalCounter % 60 == 0)
				{
					Projectile.netUpdate = true;
				}
				Vector2 playerCursor = Main.MouseWorld;
				
				float shootToX = playerCursor.X - Projectile.Center.X;
				float shootToY = playerCursor.Y - Projectile.Center.Y;
						
				Projectile.tileCollide = true;
				if(FindClosestEnemy() == -1)
				{
					Projectile.rotation = (float)Math.Atan2((double)shootToY, (double)shootToX) + MathHelper.ToRadians(45);
				}
				else
				{
					Projectile.ai[1] += 1;
					NPC target = Main.npc[FindClosestEnemy()];
					shootToX = target.Center.X - Projectile.Center.X;
					shootToY = target.Center.Y - Projectile.Center.Y;
					Projectile.rotation = (float)Math.Atan2((double)shootToY, (double)shootToX) + MathHelper.ToRadians(45);
					if(Projectile.ai[1] >= targetValue)
					{
						targetValue = Main.rand.Next(120,240);
						Projectile.ai[1] = 0;
						LaunchLaser(target.Center);
					}
				}
			}
			Vector2 initialLoop = new Vector2(164, 0).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter * 2.15f + 45 * (int)Projectile.ai[0]));

			if((int)Projectile.ai[0] % 2 == 0)
			{
				initialLoop.X /= 2.0f;
			}
			else
			{
				initialLoop.Y /= 2.0f;
			}

			Vector2 properLoop = new Vector2(initialLoop.X, initialLoop.Y).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter * 2.15f));
			Projectile.position.X = properLoop.X + player.Center.X - Projectile.width / 2;
			Projectile.position.Y = properLoop.Y + player.Center.Y - Projectile.height / 2;
		}
		public void LaunchLaser(Vector2 area)
		{
			Player player  = Main.player[Projectile.owner];
			int laser = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, mod.ProjectileType("BrightRedLaser"), Projectile.damage, 0, Projectile.owner, area.X, area.Y);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(Projectile.rotation);
			writer.Write(Projectile.spriteDirection);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			Projectile.rotation = reader.ReadSingle();
			Projectile.spriteDirection = reader.ReadInt32();
		}
	}
}
		