using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.ChestItems;
using SOTS.Items.Earth.Glowmoth;
using SOTS.Items.Fragments;
using SOTS.Items.Planetarium;
using SOTS.Items.Planetarium.FromChests;
using SOTS.Items.Planetarium.Furniture;
using SOTS.Projectiles;
using SOTS.Void;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
    public class TorchGunMk2 : VoidItem
    {
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Items/TorchGunMk2Glow").Value;
            Color color = Color.White;
            Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
            Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
        }
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
            this.SetResearchCost(1);
        }
        public override void SafeSetDefaults()
        {
            Item.damage = 30;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 66;
            Item.height = 34;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.0f;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<TorchBombMk2>();
            Item.shootSpeed = 6.5f;
            Item.useAmmo = ModContent.ItemType<TorchBomb>();
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/TorchGunMk2Glow").Value;
                Item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -4;
                Item.GetGlobalItem<ItemUseGlow>().glowOffsetY = -2;
            }
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position += velocity.SafeNormalize(Vector2.Zero) * 52;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int count = player.altFunctionUse == 2 ? 3 : 1;
            Vector2 destination = Main.MouseWorld;
            Projectile.NewProjectile(source, position, velocity, Item.shoot, damage, knockback, player.whoAmI, destination.X, destination.Y, player.altFunctionUse == 2 ? 0 : 2);
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, -2);
        }
        public override bool BeforeConsumeAmmo(Player player)
        {
            return player.altFunctionUse == 0;
        }
        public override bool NeedsAmmo(Player player)
        {
            return player.altFunctionUse == 0;
        }
        public override bool BeforeDrainVoid(Player player)
        {
            return player.altFunctionUse == 0;
        }
        public override int GetVoid(Player player)
        {
            return 40;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient<PhaseCannon>(1).AddIngredient<CoconutGun>(1).AddIngredient<TorchGun>(1)
                .AddIngredient<DissolvingNether>(1)
                .AddIngredient<OtherworldlyAlloy>(20)
                .AddTile(ModContent.TileType<HardlightFabricatorTile>()).Register();
        }
    }
}
