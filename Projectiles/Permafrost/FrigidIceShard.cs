using System;
using Microsoft.Xna.Framework;
using SOTS.Dusts;
using SOTS.Items.Permafrost;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Permafrost 
{    
    public class FrigidIceShard : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Shard");
			Main.projFrames[projectile.type] = 3;
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(ProjectileID.IceSpike);
			projectile.width = 24;
			projectile.height = 14;
			projectile.alpha = 0;
			projectile.tileCollide = false;
			projectile.hide = true;
		}
		int counter = 0;
        public override bool PreAI()
        {
			if(projectile.hide)
			{
				Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 17, 1.1f, 0.1f);
				projectile.hide = false;
				projectile.frame = projectile.whoAmI % 3;
			}
			return true;
        }
        public override void AI()
		{
			projectile.scale = 0.8f;
			projectile.rotation = projectile.velocity.ToRotation();
			counter++;
			if(counter > 10 || (projectile.velocity.Y > 0 && counter > 3))
			{
				projectile.tileCollide = true;
			}
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = 8;
			hitbox = new Rectangle((int)projectile.Center.X - width/2, (int)projectile.Center.Y - width/2, width, width);
            base.ModifyDamageHitbox(ref hitbox);
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
			width = 6;
			height = 6;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
        public override void Kill(int timeLeft)
		{
			for (int a = 0; a < 12; a++)
			{
				Dust dust = Dust.NewDustDirect(projectile.position - new Vector2(5), projectile.width, projectile.height, ModContent.DustType<ModIceDust>());
				dust.scale *= 1.1f;
				dust.velocity = dust.velocity * 0.8f + projectile.velocity * 0.5f;
			}
			Vector2 velocity = projectile.velocity.SafeNormalize(Vector2.Zero) * 8;
			int i = (int)(projectile.Center.X + velocity.X) / 16;
			int j = (int)(projectile.Center.Y + velocity.Y) / 16;
			if (Main.tile[i, j].active() && Main.tile[i,j].type == ModContent.TileType<FrigidIceTile>())
			{
				WorldGen.KillTile(i, j, false, false, false);
				if (!Main.tile[i, j].active() && Main.netMode != NetmodeID.SinglePlayer)
				{
					NetMessage.SendData(MessageID.TileChange, -1, -1, null, 0, (float)i, (float)j, 0f, 0, 0, 0);
				}
			}
		}
	}
}
		
			