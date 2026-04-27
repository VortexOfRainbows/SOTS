using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using rail;
using Terraria.Localization;
using SOTS.Helpers;
using SOTS.Items;

namespace SOTS.Projectiles.AbandonedVillage
{    
    public class PintOPunch : ModProjectile 
    {
        public override string Texture => "SOTS/Items/AbandonedVillage/PintOPunch";
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(3);
            AIType = 3;
			Projectile.penetrate = 1;
            Projectile.alpha = 60;
			Projectile.width = 44;
			Projectile.height = 28;
            Projectile.friendly = false;
            Projectile.hide = true;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 14;
			height = 14;
            return true;
        }
        public override void OnKill(int timeLeft)
		{
            if (Projectile.ai[2] > 0)
            {
                SOTSUtils.PlaySound(SoundID.Item175, Projectile.Center, 1.0f, -0.1f);
                int index = (int)Projectile.ai[2] % 5;
                string text = Language.GetTextValue($"Mods.SOTS.Items.PintOPunch.CombatText.{index}");
                CombatText.NewText(Projectile.Hitbox, Color.Red, text, true);
            }
            else
            {
                SOTSUtils.PlaySound(SoundID.Shatter, Projectile.Center, 0.9f, -0.1f);
                for (int i = 0; i < 20; i++)
                {
                    Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Glass, newColor: Color.White);
                    d.velocity *= 1.2f;
                }
                for (int i = 0; i < 25; i++)
                {
                    Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Water_BloodMoon, Scale: 1.4f, newColor: ColorHelper.PintOPunch);
                    d.velocity = Projectile.velocity * Main.rand.NextFloat(.2f) + Main.rand.NextVector2Circular(5, 5);

                    d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BloodWater, Scale: 1.4f, newColor: ColorHelper.PintOPunch);
                    d.velocity = Projectile.velocity * Main.rand.NextFloat(.2f) + Main.rand.NextVector2Circular(4, 4);
                }
            }
        }
        public override void AI()
		{
            if (Projectile.ai[2] > 0)
                Projectile.Kill();
            Projectile.friendly = true;
            Projectile.hide = false;
            for (float i = 0; i < 1; i += 0.34f)
            {
                Vector2 dustSpawnPos = Projectile.Center - new Vector2(Main.rand.NextFloat(-Projectile.width, Projectile.width) / 4f, Projectile.height / 3f).RotatedBy(Projectile.rotation);
                Color c = Color.Red * 0.5f;
                c.A = 0;
                Dust d = Dust.NewDustDirect(dustSpawnPos - new Vector2(4) + Projectile.velocity * i, 0, 0, Main.rand.NextBool() ? DustID.Rain_BloodMoon : DustID.BloodWater, newColor: ColorHelper.PintOPunch);
                d.velocity = d.velocity * 0.225f + Projectile.velocity * 0.225f + new Vector2(0, -Main.rand.NextFloat(1, 2));
                d.scale = d.scale * 0.2f + 1.0f;
                d.alpha = 200;
            }

            if (Projectile.direction != 0)
                Projectile.spriteDirection = Projectile.direction;
		}
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.CritDamage += 3;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit && Projectile.owner == Main.myPlayer)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, Type, 0, 0, Main.myPlayer, ai2: Main.rand.Next(1, 6));
                int heartCount = 0;
                for (int i = 0; i < Main.maxItems; i++)
                {
                    Item item = Main.item[i];
                    if (item.type == ItemID.Heart)
                    {
                        if (item.active)
                            heartCount++;
                        if(heartCount > 30) //If there are more than 30 hearts in the world, spawn no more
                        {
                            return;
                        }
                    }
                }
                Vector2 save = Main.LocalPlayer.Center;
                Main.LocalPlayer.Center = Projectile.Center;
                Main.LocalPlayer.QuickSpawnItemDirect(target.GetSource_OnHurt(Projectile), ItemID.Heart);
                Main.LocalPlayer.Center = save;
            }
        }
    }
}
		
			