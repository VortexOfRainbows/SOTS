using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.ChestItems;
using SOTS.Items.Fragments;
using SOTS.Items.Otherworld.FromChests;
using SOTS.Void;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Chaos
{
	public class RoseBow : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rose Bow");
			Tooltip.SetDefault("Transforms arrows into spears of light that can travel through walls\nWhen briefly charged, arrows explode for 600% damage\nWhen fully charged, arrows bloom for 1500% damage\nCharging will also conjure thorns, which are launched for 2x100% damage each\nOnly consumes void when charged");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SafeSetDefaults()
		{
			Item.damage = 92;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 44;
			Item.height = 92;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 6f;
			Item.value = Item.sellPrice(0, 20, 0, 0);
			Item.rare = ItemRarityID.Cyan;
			Item.UseSound = null;
			Item.autoReuse = false;
			Item.channel = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Chaos.RoseBow>();
			Item.shootSpeed = 12f;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.useAmmo = AmmoID.Arrow;
		}
        public override bool BeforeDrainMana(Player player)
        {
            return false;
        }
        public override int GetVoid(Player player)
        {
            return 12;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Projectiles.Chaos.RoseBow>(), damage, knockback, player.whoAmI, SOTSPlayer.ApplyAttackSpeedClassModWithGeneric(player, DamageClass.Ranged, Item.useTime), type);
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<TerminalCluster>(), 1).AddIngredient(ModContent.ItemType<PrecariousCluster>(), 1).AddIngredient(ModContent.ItemType<GlazeBow>(), 1).AddIngredient(ModContent.ItemType<SpectreSpiritStorm>(), 1).AddIngredient(ModContent.ItemType<StarshotCrossbow>(), 1).AddIngredient(ItemID.ChlorophyteShotbow, 1).AddIngredient(ModContent.ItemType<PhaseBar>(), 6).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}