using Microsoft.Xna.Framework;
using SOTS.Buffs;
using SOTS.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class Doomstick : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Doomstick");
            Tooltip.SetDefault("");
        }
		public override void SetDefaults()
		{
            item.damage = 21; 
            item.ranged = true;  
            item.width = 58;   
            item.height = 20;
            item.useTime = 12; 
            item.useAnimation = 24;
            item.reuseDelay = 44;
            item.useStyle = ItemUseStyleID.HoldingOut;    
            item.knockBack = 3f;
            item.value = Item.sellPrice(0, 6, 0, 0);
            item.rare = ItemRarityID.Pink;
            item.UseSound = SoundID.Item36;
            item.autoReuse = true;
            item.noMelee = true;
            item.shoot = 10;
            item.shootSpeed = 8.5f;
            item.useAmmo = AmmoID.Bullet;
            item.channel = true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2, 0);
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if(player.altFunctionUse == 2)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).SafeNormalize(Vector2.Zero);
                perturbedSpeed *= 24f;
                speedX = perturbedSpeed.X;
                speedY = perturbedSpeed.Y;
                type = ModContent.ProjectileType<DoomstickHoldOut>();
                return player.ownedProjectileCounts[ModContent.ProjectileType<DoomstickHoldOut>()] < 1;
            }
            Main.PlaySound(item.UseSound, player.Center);
            int amt = 3 + Main.rand.Next(2);
            for(int i = 0; i < amt; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10));
                Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X * (0.825f + (.1f * i)), perturbedSpeed.Y * (0.825f + (.1f * i)), type, damage, knockBack, player.whoAmI);
            }
            return false;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.noUseGraphic = true;
                item.reuseDelay = 0;
            }
            else
            {
                item.noUseGraphic = false;
                item.reuseDelay = 44;
            }
            return player.ownedProjectileCounts[ModContent.ProjectileType<DoomstickHoldOut>()] < 1;
        }
        public override bool ConsumeAmmo(Player player)
        {
            return player.altFunctionUse != 2;
        }
    }
}
