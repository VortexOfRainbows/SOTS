using Microsoft.Xna.Framework;
using SOTS.Buffs;
using SOTS.Buffs.MinionBuffs;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Minions;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Permafrost
{
	public class CruiseCaller : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Cruise Caller");
			Tooltip.SetDefault("Summons a fleet of Penguin Copters to fight for you");
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; 
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
			this.SetResearchCost(1);
		}
		public override void SetDefaults() 
		{
			Item.damage = 47;
			Item.knockBack = 4f;
			Item.mana = 20;
			Item.width = 34;
			Item.height = 42;
			Item.useTime = 32;
			Item.useAnimation = 32;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.value = Item.sellPrice(0, 8, 75, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item44;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;
			Item.buffType = ModContent.BuffType<AerialAssistance>();
			Item.shoot = ModContent.ProjectileType<PenguinCopter>();
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			player.AddBuff(Item.buffType, 2);
			player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<DissolvingAurora>(), 1).AddIngredient(ModContent.ItemType<AbsoluteBar>(), 12).AddIngredient(ModContent.ItemType<HelicopterParts>(), 2).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}