using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Buffs.MinionBuffs;
using SOTS.Projectiles.Minions;
using SOTS.Void;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpiritStaves
{
	public class NatureSpiritStaff : VoidItem
	{
		public override void SetStaticDefaults() 
		{
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; 
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults() 
		{
			Item.damage = 12;
			Item.knockBack = 4f;
			Item.width = 38;
			Item.height = 42;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(0, 1, 50, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item44;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;
			Item.buffType = ModContent.BuffType<NatureSpiritAid>();
			Item.shoot = ModContent.ProjectileType<NatureSpirit>();
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			player.AddBuff(Item.buffType, 2);
			player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Fragments.DissolvingNature>(), 1).AddRecipeGroup("SOTS:SilverBar", 12).AddIngredient(ItemID.JungleSpores, 4).AddTile(TileID.Anvils).Register();
		}
	}
}