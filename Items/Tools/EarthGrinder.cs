using Microsoft.Xna.Framework;
using SOTS.Buffs;
using SOTS.Dusts;
using SOTS.Projectiles.Blades;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Tools
{
	public class EarthGrinder : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.damage = 20;
            Item.DamageType = DamageClass.Melee;
			Item.width = 56;   
            Item.height = 58;   
			Item.useTurn = true;
			Item.useTime = 6;
			Item.useAnimation = 24;
			Item.hammer = 55;
			Item.axe = 16;
			Item.knockBack = 5f;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.tileBoost = 3;
			Item.autoReuse = true;

			Item.shoot = ModContent.ProjectileType<EarthGrinderSlash>();
			Item.shootSpeed = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.channel = true;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(hitbox.TopLeft(), hitbox.Width, hitbox.Height, ModContent.DustType<PixelDust>(), player.direction * 2, 0f);
                dust.velocity *= 0.3f;
                dust.scale = 1f;
                dust.fadeIn = 10f;
                dust.color = ColorHelpers.EarthColor;
                dust.color.A = 0;
            }
        }
        public override void AddRecipes()
		{
			//CreateRecipe(1).AddIngredient(ModContent.ItemType<FrigidBar>(), 16).AddTile(TileID.Anvils).Register();
		}
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.noMelee = true;
                Item.noUseGraphic = true;
                //Item.useStyle = ItemUseStyleID.Shoot; //Switching use styles breaks the functionality of this item for subspace servants
                Item.channel = false;
            }
            else
            {
                Item.noMelee = false;
                Item.noUseGraphic = false;
                //Item.useStyle = ItemUseStyleID.Swing;
                Item.channel = true;
            }
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			if(player.altFunctionUse == 2)
				Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 2 * SOTSUtils.SignNoZero(velocity.X) * player.gravDir, 1);
            return false;
        }
    }
}
