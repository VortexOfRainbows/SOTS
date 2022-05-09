using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Nature
{    
    public class GrowTree : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tree Grow");
		}
        public override void SetDefaults()
        {
			Projectile.width = 72;
			Projectile.height = 64;
            Main.projFrames[Projectile.type] = 5;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 60;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.ranged = false;
			Projectile.magic = true;
            Projectile.netImportant = true;
			Projectile.alpha = 35;
		}
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(Projectile.rotation);
			writer.Write(Projectile.spriteDirection);
			writer.Write(Projectile.frame);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			Projectile.rotation = reader.ReadSingle();
			Projectile.spriteDirection = reader.ReadInt32();
			Projectile.frame = reader.ReadInt32();
		}
		public override bool PreDraw(ref Color lightColor) 
		{
			return true;
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) 
		{
			Vector2 direction = new Vector2(24,0).RotatedBy(Projectile.rotation - MathHelper.ToRadians(90));
			if(direction.X > 0)
				hitDirection = 1;
			if(direction.X < 0)
				hitDirection = -1;
		}
		private bool loaded = false;
		public override bool PreAI()
		{
			if(!loaded)
			{
				Projectile.netUpdate = true;
				loaded = true;
			}
			return true;
		}
		public override void AI()
        {
			Player player = Main.player[Projectile.owner];
			Projectile.alpha += 2;
			float distance = 24 - Projectile.ai[1]/3;
			if(Projectile.ai[0] == 1)
			{
				distance = 18 - Projectile.ai[1]/3;
			}
			if(Projectile.ai[0] == 9)
			{
				distance = 38 - Projectile.ai[1]/3;
			}
			Vector2 direction = new Vector2(distance,0).RotatedBy(Projectile.rotation - MathHelper.ToRadians(90 - Projectile.ai[1]));
			if(Projectile.timeLeft < 58)
			{
				if(Projectile.ai[0] != 10 && !Projectile.friendly)
				{
					if(Projectile.owner == Main.myPlayer)
					{
						
						int Probe = Projectile.NewProjectile(Projectile.Center.X + direction.X, Projectile.Center.Y + direction.Y, 0, 0, ModContent.ProjectileType<GrowTree>(), Projectile.damage, Projectile.knockBack, player.whoAmI, Projectile.ai[0] + 1, Projectile.ai[1]);
							Main.projectile[Probe].rotation = Projectile.rotation - Projectile.ai[1];
						
						if(Main.projectile[Probe].ai[0] != 10)
							Main.projectile[Probe].frame = 4;
								
						if(Main.projectile[Probe].ai[0] == 1)
							Main.projectile[Probe].frame = 3;

						if (Main.projectile[Probe].ai[0] >= 5 && Main.projectile[Probe].ai[0] <= 9 && Main.projectile[Probe].frame == 4)
						{
							int type = Main.rand.Next(3);
							if(type == 0 && Projectile.frame != 1)
								Main.projectile[Probe].frame = 1;
							if(type == 1 && Projectile.frame != 2)
								Main.projectile[Probe].frame = 2;
						}
					}
				}
				Projectile.friendly = true;
			}
				
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough) 
		{
			width = 4;
			height = 4;
			fallThrough = true;
			return true;
		}
		public override void Kill(int timeLeft)
        {
			for(int i = 0; i < 15; i++)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 2);
				Main.dust[num1].noGravity = true;
			}
		}
	}
}
		