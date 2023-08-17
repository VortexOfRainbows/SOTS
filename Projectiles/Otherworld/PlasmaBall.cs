using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Otherworld
{
    public class PlasmaBall : ModProjectile
    {	
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Plasma Ball");
            Main.projFrames[Projectile.type] = 8;
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
			Projectile.height = 30;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 3000;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override bool PreDrawExtras()
        {
            return false;
        }
        public void Draw()
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Color color = new Color(60, 60, 60, 0);
            Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < 8; k++)
            {
                float x = Main.rand.Next(-10, 11) * 0.65f;
                float y = Main.rand.Next(-10, 11) * 0.65f;
                Main.spriteBatch.Draw(texture, new Vector2((float)(Projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(Projectile.Center.Y - (int)Main.screenPosition.Y) + y), new Rectangle(0, 30 * Projectile.frame, Projectile.width, Projectile.height), color * (1f - (Projectile.alpha / 255f)), Projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            target.immune[player.whoAmI] = 14;
            if (Projectile.owner == Main.myPlayer)
            {
                int npcIndex = -1;
                double distanceTB = 400;
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
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<PlasmaLightningZap>(), (int)(Projectile.damage * 0.7f) + 1, target.whoAmI, Projectile.owner, npc.whoAmI, 3);
                    }
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Draw();
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Otherworld/InfernoHookChain");
            Vector2 position = Projectile.Center;
            Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
            Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?();
            Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
            float num1 = (float)texture.Height;
            Vector2 vector2_4 = mountedCenter - position;
            float rotation = (float)Math.Atan2((double)vector2_4.Y, (double)vector2_4.X) - 1.57f;
            bool flag = true;
            if (float.IsNaN(position.X) && float.IsNaN(position.Y))
                flag = false;
            if (float.IsNaN(vector2_4.X) && float.IsNaN(vector2_4.Y))
                flag = false;
            while (flag)
            {
                if ((double)vector2_4.Length() < (double)num1 + 1.0)
                {
                    flag = false;
                }
                else
                {
                    Vector2 vector2_1 = vector2_4;
                    vector2_1.Normalize();
                    position += vector2_1 * num1;
                    vector2_4 = mountedCenter - position;
                    Color color2 = Lighting.GetColor((int)position.X / 16, (int)((double)position.Y / 16.0));
                    color2 = Projectile.GetAlpha(color2);
                    Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1f, SpriteEffects.None, 0.0f);
                }
            }
			return true;
        }
        bool hasStopped = false;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];

            Vector2 toPlayer = player.Center - Projectile.Center;
            float distance = toPlayer.Length();
            if (Main.myPlayer == Projectile.owner)
            {
                Projectile.netUpdate = true;
                Projectile.velocity *= 0.9f;
            }

            if(Projectile.Center.X - player.Center.X > 0)
                Projectile.direction = 1;
            else
                Projectile.direction = -1;

            player.ChangeDir(Projectile.direction);
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            if (!player.channel || hasStopped)
            {
                if (Main.myPlayer == Projectile.owner)
                    Projectile.velocity *= 0.7f;
                hasStopped = true;
                if (Main.myPlayer == Projectile.owner)
                    Projectile.velocity += new Vector2(-8, 0).RotatedBy(Math.Atan2(Projectile.Center.Y - player.Center.Y, Projectile.Center.X - player.Center.X));
                Projectile.tileCollide = false;
                if (distance < 14)
                {
                    Projectile.Kill();
                }
                return false;
            }
            else
            {
                Projectile.timeLeft = 36000;
            }

            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 1.25f / 255f, (255 - Projectile.alpha) * 1.35f / 255f, (255 - Projectile.alpha) * 1.5f / 255f);
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 8;
            }
            if (Main.myPlayer == Projectile.owner)
            {
                Vector2 projectileSpeed = Projectile.velocity.SafeNormalize(Vector2.Zero) * Projectile.velocity.Length();
                Vector2 add = Main.MouseWorld - Projectile.Center;
                float toPlayerL = (Projectile.Center - player.Center).Length();
                if (toPlayerL > 0)
                {
                    add = add.SafeNormalize(Vector2.Zero) * (1f / (float)Math.Sqrt(toPlayerL)) * 20f;
                    Projectile.velocity += add;
                }
            }
            if (Main.myPlayer == Projectile.owner)
            {
                Projectile.velocity += new Vector2(-1.1f, 0).RotatedBy(Math.Atan2(Projectile.Center.Y - player.Center.Y, Projectile.Center.X - player.Center.X));
            }
            return false;
        }
        public override void AI()
		{

        }
    }
}