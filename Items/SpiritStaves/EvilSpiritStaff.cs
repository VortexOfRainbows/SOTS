using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Buffs.MinionBuffs;
using SOTS.Items.Otherworld;
using SOTS.Items.Otherworld.FromChests;
using SOTS.Projectiles.Minions;
using SOTS.Void;
using Terraria;
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
		}
		public override void SafeSetDefaults() 
		{
			Item.damage = 66;
			Item.knockBack = 4f;
			Item.width = 40;
			Item.height = 42;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.SwingThrow;
			Item.value = Item.sellPrice(0, 15, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item44;
			Item.noMelee = true;
			Item.summon = true;
			Item.buffType = ModContent.BuffType<EvilSpiritAid>();
			Item.shoot = ModContent.ProjectileType<EvilSpirit>();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) 
		{
			player.AddBuff(Item.buffType, 2);
			position = Main.MouseWorld;
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Fragments.DissolvingUmbra>(), 1);
			recipe.AddIngredient(ModContent.ItemType<Pyramid.CursedBlade>(), 1);
			recipe.AddIngredient(ItemID.SpectreBar, 10);
			recipe.AddRecipeGroup("SOTS:EvilBar", 10);
			recipe.AddIngredient(ItemID.SoulofSight, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}