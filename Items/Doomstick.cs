using Microsoft.Xna.Framework;
using SOTS.Buffs;
using SOTS.Items.Fragments;
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
            Tooltip.SetDefault("Fires two shotgun blasts in quick succession\nRight click to launch a 250% damage hook that pulls in enemies\nPulls you toward bosses instead\nKilled enemies drop packs of health and mana");
        }
		public override void SetDefaults()
		{
            Item.damage = 27; 
            Item.ranged = true;  
            Item.width = 58;   
            Item.height = 20;
            Item.useTime = 10; 
            Item.useAnimation = 20;
            Item.reuseDelay = 32;
            Item.useStyle = ItemUseStyleID.HoldingOut;    
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(0, 6, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item36;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shoot = 10;
            Item.shootSpeed = 8.5f;
            Item.useAmmo = AmmoID.Bullet;
            Item.channel = true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2, 0);
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override void HoldItem(Player player)
        {
            SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
            modPlayer.doomDrops = true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if(player.altFunctionUse == 2)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).SafeNormalize(Vector2.Zero);
                perturbedSpeed *= 24f;
                speedX = perturbedSpeed.X;
                speedY = perturbedSpeed.Y;
                damage = (int)(damage * 2f);
                if(player.ownedProjectileCounts[ModContent.ProjectileType<DoomstickHoldOut>()] < 1)
                {
                    int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<DoomstickHoldOut>(), damage, knockBack, player.whoAmI);
                    Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<Doomhook>(), damage, knockBack, player.whoAmI, proj);
                }
                return false;
            }
            Main.PlaySound(Item.UseSound, player.Center);
            int amt = 4;
            for(int i = 0; i < amt; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(20));
                Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X * (0.825f + (.1f * i)), perturbedSpeed.Y * (0.825f + (.1f * i)), type, damage, knockBack, player.whoAmI);
            }
            return false;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.noUseGraphic = true;
                Item.reuseDelay = 0;
            }
            else
            {
                Item.noUseGraphic = false;
                Item.reuseDelay = 32;
            }
            return player.ownedProjectileCounts[ModContent.ProjectileType<DoomstickHoldOut>()] < 1;
        }
        public override bool ConsumeAmmo(Player player)
        {
            return player.altFunctionUse != 2;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Boomstick, 1);
            recipe.AddIngredient(ModContent.ItemType<FragmentOfInferno>(), 4);
            recipe.AddIngredient(ItemID.SoulofFright, 20);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
