using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Permafrost;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items.Permafrost
{  
    public class StormSpell : ModItem
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Storm Spell");
			Tooltip.SetDefault("Create an arctic storm targeted on your cursor");
            this.SetResearchCost(1);
        }
        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.DamageType = DamageClass.Magic;
            Item.width = 40;
            Item.height = 50;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.35f;
			Item.shootSpeed = 9;
            Item.value = Item.sellPrice(0, 0, 80, 0);
            Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item92;
			Item.mana = 15;
			Item.crit = 2;
			Item.shoot = ModContent.ProjectileType<IceStorm>();
            Item.noUseGraphic = true;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/Permafrost/StormSpellAnim").Value;
            }
        }
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Diamond, 1).AddIngredient(ModContent.ItemType<FrigidBar>(), 8).AddTile(TileID.Anvils).Register();
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Vector2 toPos = Main.MouseWorld;
			Projectile.NewProjectile(source, toPos.X, toPos.Y, 0, 0, type, damage, knockback, player.whoAmI);
			return false;
		}
    }
}