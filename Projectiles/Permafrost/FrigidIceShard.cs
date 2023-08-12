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
			// DisplayName.SetDefault("Ice Shard");
			Main.projFrames[Projectile.type] = 3;
		}
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(ProjectileID.IceSpike);
			Projectile.width = 24;
			Projectile.height = 14;
			Projectile.alpha = 0;
			Projectile.tileCollide = false;
			Projectile.hide = true;
		}
		int counter = 0;
        public override bool PreAI()
        {
			if(Projectile.hide)
			{
				SOTSUtils.PlaySound(SoundID.Item17, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.1f, 0.1f);
				Projectile.hide = false;
				Projectile.frame = Projectile.whoAmI % 3;
			}
			return true;
        }
        public override void AI()
		{
			Projectile.scale = 0.8f;
			Projectile.rotation = Projectile.velocity.ToRotation();
			counter++;
			if(counter > 10 || (Projectile.velocity.Y > 0 && counter > 3))
			{
				Projectile.tileCollide = true;
			}
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = 8;
			hitbox = new Rectangle((int)Projectile.Center.X - width/2, (int)Projectile.Center.Y - width/2, width, width);
            base.ModifyDamageHitbox(ref hitbox);
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 6;
			height = 6;
            return true;
        }
        public override void Kill(int timeLeft)
		{
			for (int a = 0; a < 12; a++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position - new Vector2(5), Projectile.width, Projectile.height, ModContent.DustType<ModIceDust>());
				dust.scale *= 1.1f;
				dust.velocity = dust.velocity * 0.8f + Projectile.velocity * 0.5f;
			}
			Vector2 velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * 8;
			int i = (int)(Projectile.Center.X + velocity.X) / 16;
			int j = (int)(Projectile.Center.Y + velocity.Y) / 16;
			if (Main.tile[i, j].HasTile && Main.tile[i,j].TileType == ModContent.TileType<FrigidIceTile>())
			{
				WorldGen.KillTile(i, j, false, false, false);
				if (!Main.tile[i, j].HasTile && Main.netMode != NetmodeID.SinglePlayer)
				{
					NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, (float)i, (float)j, 0f, 0, 0, 0);
				}
			}
		}
	}
}
		
			