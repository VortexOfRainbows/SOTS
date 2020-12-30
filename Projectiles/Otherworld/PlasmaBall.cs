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
    	int counter = 50;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Ball");
            Main.projFrames[projectile.type] = 8;
        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 15;
            projectile.width = 30;
			projectile.height = 30;
			projectile.penetrate = -1;
			projectile.timeLeft = 3000;
			projectile.friendly = true;
			projectile.melee = true;
            projectile.alpha = 0;
        }
        public override bool PreDrawExtras(SpriteBatch spriteBatch)
        {
            return false;
        }
        public void Draw()
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            Color color = new Color(60, 60, 60, 0);
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < 8; k++)
            {
                float x = Main.rand.Next(-10, 11) * 0.65f;
                float y = Main.rand.Next(-10, 11) * 0.65f;
                Main.spriteBatch.Draw(texture, new Vector2((float)(projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(projectile.Center.Y - (int)Main.screenPosition.Y) + y), new Rectangle(0, 30 * projectile.frame, projectile.width, projectile.height), color * (1f - (projectile.alpha / 255f)), projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[projectile.owner];
            target.immune[player.whoAmI] = 14;
            if (projectile.owner == Main.myPlayer)
            {
                int npcIndex = -1;
                double distanceTB = 400;
                for (int i = 0; i < 200; i++) //find first enemy
                {
                    NPC npc = Main.npc[i];
                    if (!npc.friendly && npc.lifeMax > 5 && npc.active && !npc.dontTakeDamage)
                    {
                        if (npcIndex != i && target.whoAmI != i && npc.whoAmI != (int)projectile.ai[0])
                        {
                            float disX = projectile.Center.X - npc.Center.X;
                            float disY = projectile.Center.Y - npc.Center.Y;
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
                        Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("PlasmaLightningZap"), (int)(projectile.damage * 0.7f) + 1, target.whoAmI, projectile.owner, npc.whoAmI, 3);
                    }
                }
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Draw();
            Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Otherworld/InfernoHookChain");
            Vector2 position = projectile.Center;
            Vector2 mountedCenter = Main.player[projectile.owner].MountedCenter;
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
                    color2 = projectile.GetAlpha(color2);
                    Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1f, SpriteEffects.None, 0.0f);
                }
            }
			return true;
        }
        bool hasStopped = false;
        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];

            Vector2 toPlayer = player.Center - projectile.Center;
            float distance = toPlayer.Length();
            projectile.velocity *= 0.9f;

            if(projectile.Center.X - player.Center.X > 0)
                projectile.direction = 1;
            else
                projectile.direction = -1;

            player.ChangeDir(projectile.direction);
            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = MathHelper.WrapAngle((float)Math.Atan2(projectile.Center.Y - player.Center.Y, projectile.Center.X - player.Center.X) + MathHelper.ToRadians(projectile.direction == -1 ? 180 : 0));
            if (!player.channel || hasStopped)
            {
                projectile.velocity *= 0.7f;
                hasStopped = true;
                projectile.velocity += new Vector2(-8, 0).RotatedBy(Math.Atan2(projectile.Center.Y - player.Center.Y, projectile.Center.X - player.Center.X));
                projectile.tileCollide = false;
                if (distance < 14)
                {
                    projectile.Kill();
                }
                return false;
            }
            else
            {
                projectile.timeLeft = 36000;
            }

            Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 1.25f / 255f, (255 - projectile.alpha) * 1.35f / 255f, (255 - projectile.alpha) * 1.5f / 255f);
            projectile.frameCounter++;
            if (projectile.frameCounter >= 5)
            {
                projectile.frameCounter = 0;
                projectile.frame = (projectile.frame + 1) % 8;
            }
            if (Main.myPlayer == projectile.owner)
            {
                projectile.netUpdate = true;
                Vector2 projectileSpeed = projectile.velocity.SafeNormalize(Vector2.Zero) * projectile.velocity.Length();
                Vector2 add = Main.MouseWorld - projectile.Center;
                float toPlayerL = (projectile.Center - player.Center).Length();
                if (toPlayerL > 0)
                {
                    add = add.SafeNormalize(Vector2.Zero) * (1f / (float)Math.Sqrt(toPlayerL)) * 20f;
                    projectile.velocity += add;
                }
            }
            projectile.velocity += new Vector2(-1.1f, 0).RotatedBy(Math.Atan2(projectile.Center.Y - player.Center.Y, projectile.Center.X - player.Center.X));
            return false;
        }
        public override void AI()
		{

        }
    }
}