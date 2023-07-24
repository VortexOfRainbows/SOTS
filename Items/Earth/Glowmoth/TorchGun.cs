using Microsoft.Xna.Framework;
using SOTS.Items.Slime;
using SOTS.Projectiles.Earth.Glowmoth;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Earth.Glowmoth
{
    public class TorchGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
            this.SetResearchCost(1);
        }
        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 72;
            Item.height = 32;
            Item.useTime = 8;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.0f;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<TorchEmber>();
            Item.shootSpeed = 6.5f;
            Item.useAmmo = AmmoID.Gel;
            Item.reuseDelay = 12;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool? UseItem(Player player)
        {
            if(player.altFunctionUse == 2)
            {
                Item.useTime = Item.useAnimation;
            }
            else
            {
                Item.useTime = Item.useAnimation / 3;
            }
            return base.UseItem(player);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.altFunctionUse == 2)
                type = ModContent.ProjectileType<TorchBombWood>();
            else
            {
                velocity += Main.rand.NextVector2Circular(1, 1);
            }
            position += velocity.SafeNormalize(Vector2.Zero) * 56;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2f, 4);
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return player.ItemUsesThisAnimation == 0;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient<FireSpitter>(1).AddIngredient(ItemID.FlareGun, 1).AddRecipeGroup("SOTS:SilverBar", 10).AddIngredient<GlowNylon>(100).AddTile(TileID.Anvils).Register();
        }
    }
}
