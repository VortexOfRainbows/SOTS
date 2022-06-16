using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Buffs.MinionBuffs;
using SOTS.Items.Otherworld;
using SOTS.Items.Otherworld.FromChests;
using SOTS.Projectiles.Minions;
using SOTS.Void;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpiritStaves
{
	public class EvilSpiritStaff : VoidItem
	{
		public override void SetStaticDefaults() 
		{
			Tooltip.SetDefault("Summons an Evil Spirit to fight for you\nAttacks with a flurry of Evil Bolts, dealing 50% damage each\nEnds in an explosion that deals 100% damage, always critical strikes");
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; 
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults() 
		{
			Item.damage = 66;
			Item.knockBack = 4f;
			Item.width = 40;
			Item.height = 42;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(0, 15, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item44;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;
			Item.buffType = ModContent.BuffType<EvilSpiritAid>();
			Item.shoot = ModContent.ProjectileType<EvilSpirit>();
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			player.AddBuff(Item.buffType, 2);
			player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Fragments.DissolvingUmbra>(), 1).AddIngredient(ModContent.ItemType<Pyramid.CursedBlade>(), 1).AddIngredient(ItemID.SpectreBar, 10).AddRecipeGroup("SOTS:EvilBar", 10).AddIngredient(ItemID.SoulofSight, 5).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}