using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{
    public class InfernoHook : ModProjectile
    {
        bool pull = false;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Inferno Hook");

        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.GemHookDiamond);
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.friendly = false;
        }
        public int storeData1 = -1;
        public int storeData2 = -1;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(storeData2);
            writer.Write(storeData1);
            base.SendExtraAI(writer);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            storeData2 = reader.ReadInt32();
            storeData1 = reader.ReadInt32();
            base.ReceiveExtraAI(reader);
        }
        public override void PostAI()
        {
            if (storeData1 == -1 && Projectile.owner == Main.myPlayer)
            {
                storeData1 = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, mod.ProjectileType("InfernoTrail"), (int)(Projectile.damage * 1f) + 1, Projectile.knockBack, Projectile.owner, -1, Projectile.whoAmI);
                storeData2 = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, mod.ProjectileType("InfernoTrail"), (int)(Projectile.damage * 1f) + 1, Projectile.knockBack, Projectile.owner, 1, Projectile.whoAmI);
                Projectile.netUpdate = true;
            }
            if(Projectile.velocity.Length() > 1)
            {
                int num1 = Dust.NewDust(new Vector2(Projectile.position.X - 4, Projectile.position.Y - 4), Projectile.width, Projectile.height, 6);
                Main.dust[num1].noGravity = true;
                Main.dust[num1].scale = 1.55f;
                Main.dust[num1].velocity *= 0.2f;
            }
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 16;
            height = 16;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
        public override bool? SingleGrappleHook(Player player)
        {
            return true;
        }
        public override void GrapplePullSpeed(Player player, ref float speed)
        {
            speed = 18.5f;
            base.GrapplePullSpeed(player, ref speed);
        }
        public override float GrappleRange()
        {
            return 510f; //30 distance
        }
        public override void GrappleRetreatSpeed(Player player, ref float speed)
        {
            speed = 30f;
        }
        public override bool PreDrawExtras(SpriteBatch spriteBatch)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Otherworld/InfernoHookChain");    //this where the chain of grappling hook is drawn
                                                                                                          //change YourModName with ur mod name/ and CustomHookPr_Chain with the name of ur one
            Vector2 position = Projectile.Center;
            Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
            Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?();
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
                    Microsoft.Xna.Framework.Color color2 = Lighting.GetColor((int)position.X / 16, (int)((double)position.Y / 16.0));
                    color2 = Projectile.GetAlpha(color2);
                    Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1f, SpriteEffects.None, 0.0f);
                }
            }
            return false;
        }
        public override void AI()
        {
            Projectile.scale = 1.2f;
            Projectile.rotation -= MathHelper.ToRadians(45);
            Projectile.spriteDirection = 1;
        }
    }
}