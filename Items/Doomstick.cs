using Microsoft.Xna.Framework;
using SOTS.Buffs;
using SOTS.Items.Fragments;
using SOTS.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
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
            this.SetResearchCost(1);
        }
		public override void SetDefaults()
		{
            Item.damage = 27; 
            Item.DamageType = DamageClass.Ranged;  
            Item.width = 58;   
            Item.height = 20;
            Item.useTime = 10; 
            Item.useAnimation = 20;
            Item.reuseDelay = 32;
            Item.useStyle = ItemUseStyleID.Shoot;    
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
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if(player.altFunctionUse == 2)
            {
                Vector2 perturbedSpeed = velocity.SafeNormalize(Vector2.Zero);
                perturbedSpeed *= 24f;
                velocity.X = perturbedSpeed.X;
                velocity.Y = perturbedSpeed.Y;
                damage = (int)(damage * 2f);
                if(player.ownedProjectileCounts[ModContent.ProjectileType<DoomstickHoldOut>()] < 1)
                {
                    int proj = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<DoomstickHoldOut>(), damage, knockback, player.whoAmI);
                    Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Doomhook>(), damage, knockback, player.whoAmI, proj);
                }
                return false;
            }
            SoundEngine.PlaySound((SoundStyle)Item.UseSound, player.Center);
            int amt = 4;
            for(int i = 0; i < amt; i++)
            {
                Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(20));
                Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X * (0.825f + (.1f * i)), perturbedSpeed.Y * (0.825f + (.1f * i)), type, damage, knockback, player.whoAmI);
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
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return player.altFunctionUse != 2;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Boomstick, 1).AddIngredient<FragmentOfInferno>(4).AddIngredient(ItemID.SoulofFlight, 20).AddIngredient(ItemID.IllegalGunParts, 1).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}
