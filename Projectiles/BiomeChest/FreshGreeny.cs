using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Projectiles.BiomeChest
{
	public class FreshGreeny : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 5;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.75f);
            Color color = lightColor;
			Rectangle frame = new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, frame, color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            return false;
        }
        public sealed override void SetDefaults()
		{
			Projectile.width = 36;
			Projectile.height = 34;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.friendly = true;
			Projectile.minion = true;
			Projectile.minionSlots = 1f;
			Projectile.penetrate = -1;
            Projectile.localNPCHitCooldown = 20;
            Projectile.usesLocalNPCImmunity = true;
		}
		public override bool? CanCutTiles() => false;
		public override bool MinionContactDamage() => true;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

		}
        private float counter = 0;
        private int frame = 0;
        public override void AI() 
		{
            SlimeAI();
            if(counter % 5 == 0 && Main.myPlayer == Projectile.owner)
            {
                Projectile.netUpdate = true;
            }
			if(Math.Abs(Projectile.velocity.X) > 20f)
			{
				Projectile.velocity.X *= 0.9f;
			}
			Player player = Main.player[Projectile.owner];
			if (player.dead || !player.active) 
				player.ClearBuff(ModContent.BuffType<Buffs.MinionBuffs.FreshGreenyBuff>());
			if (player.HasBuff(ModContent.BuffType<Buffs.MinionBuffs.FreshGreenyBuff>()))
				Projectile.timeLeft = 2;
			FrameUpdate();
			Projectile.frame = frame;
		}	
		public void SlimeAI()
		{

        }
		public void FrameUpdate()
        {
            counter += 0.5f;
			counter += MathF.Sqrt(Math.Abs(Projectile.velocity.X));
            int frameSpeed = 10;
            if (counter >= frameSpeed)
            {
				counter -= frameSpeed;
                frame++;
                if (frame >= Main.projFrames[Projectile.type] - 1)
                {
                    frame = 0;
                }
            }
			if(Projectile.ai[0] == 1) //This is when the slime is flying
			{
				counter = 0;
                frame = 4;
			}
        }
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}
	}
}