using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Projectiles.Earth;
using SOTS.Void;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Earth.Glowmoth
{
	public class IlluminantAxe : ModItem
    {
        public Texture2D glowTexture => Mod.Assets.Request<Texture2D>("Items/Earth/Glowmoth/IlluminantAxeGlow").Value;
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
            Main.spriteBatch.Draw(glowTexture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, Color.White, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
        }
        public override void SetStaticDefaults()
		{
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
            this.SetResearchCost(1);
        }
		public override void SetDefaults()
		{
            Item.damage = 17; 
            Item.DamageType = DamageClass.Melee;  
            Item.width = 54;   
            Item.height = 46;
            Item.useTime = 16; 
            Item.useAnimation = 32;
            Item.useStyle = ItemUseStyleID.Swing;    
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.axe = 12;
            Item.channel = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Earth.IlluminantAxe>();
            Item.shootSpeed = 12f;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = glowTexture;
            }
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(8))
            {
                int num1 = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.GlowingMushroom, player.direction * 4, -1f, 150, default(Color), 1.3f);
                Main.dust[num1].velocity *= 0.4f;
                Main.dust[num1].scale = 0.8f;
            }
            else if (Main.rand.NextBool(6))
            {
                int num2 = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<Dusts.CopyDust4>());
                Dust dust = Main.dust[num2];
                dust.color = ColorHelpers.VibrantColorAttempt(Main.rand.NextFloat(180, 360), true);
                dust.noGravity = true;
                dust.fadeIn = 0.1f;
                dust.scale *= 1.44f;
                dust.velocity += new Vector2(player.direction * 1, -0.25f);
            }
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity *= (SOTSPlayer.ModPlayer(player).attackSpeedMod);
        }
        int counter = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if(player.altFunctionUse == 2)
            {
                for (int i = 0; i < 1000; i++)
                {
                    Projectile proj = Main.projectile[i];
                    if (proj.type == type && proj.active && proj.owner == player.whoAmI && proj.ModProjectile is Projectiles.Earth.IlluminantAxe axe)
                    {
                        axe.AI3 = 3;
                        return false;
                    }
                }
                return true;
            }
            else
            {
                counter++;
                if (counter % 4 == 1)
                    Projectile.NewProjectile(source, position + velocity, 0.2f * velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-12, 12))), ModContent.ProjectileType<IlluminantBolt>(), (int)(damage * 0.6f), knockback * 0.2f, Main.myPlayer, Main.rand.NextFloat(180, 360));
            }
            return false;
        }
        public override float UseSpeedMultiplier(Player player)
        {
            return 1f;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.noMelee = true;
                Item.noUseGraphic = true;
                Item.UseSound = SoundID.Item19;
                Item.axe = 0;
                Item.useTime = 32;
            }
            else
            {
                Item.noMelee = false;
                Item.noUseGraphic = false;
                Item.UseSound = SoundID.Item1;
                Item.axe = 12;
                Item.useTime = 16;
                return player.ownedProjectileCounts[Item.shoot] < 1;
            }
            return true;
        }
    }
}
