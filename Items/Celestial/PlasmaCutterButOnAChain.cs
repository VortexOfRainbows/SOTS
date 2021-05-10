using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;


namespace SOTS.Items.Celestial
{
	public class PlasmaCutterButOnAChain : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Cutter on a Chain");
			Tooltip.SetDefault("'This is utmost wonderful idea'");
		}
		public override void SetDefaults()
		{
            item.damage = 130;
            item.melee = true;  
            item.width = 64;
            item.height = 54;  
            item.useTime = 28; 
            item.useAnimation = 28;
            item.useStyle = ItemUseStyleID.SwingThrow;    
            item.knockBack = 6f;
            item.value = Item.sellPrice(0, 15, 0, 0);
            item.rare = ItemRarityID.Yellow;
            item.UseSound = SoundID.Item22;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("PlasmaCutter"); 
            item.shootSpeed = 0f;
			item.channel = true;
			item.axe = 200;
            item.noUseGraphic = true; 
            item.noMelee = true;
			Item.staff[item.type] = true; 
		}
        public override float UseTimeMultiplier(Player player)
		{
			float distance = Vector2.Distance(player.Center, Main.MouseWorld);
			if (distance < 48)
				distance = 48;
			if (distance > 896)
				distance = 896;
			float spinSpeed = 1 + (2574f / distance);
			int speedMod = (int)(28f - spinSpeed);
			if (speedMod < 6)
				speedMod = 6;
			int useTime = speedMod;
			item.useTime = useTime;
			item.useAnimation = useTime;
			return base.UseTimeMultiplier(player);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			bool summon = true;
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if(proj.active && proj.type == item.shoot && Main.player[proj.owner] == player)
				{
					summon = false;
				}
			}
			if(player.altFunctionUse != 2)
			{
				item.UseSound = SoundID.Item22;
				if(summon)
				{
					Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, (float)Math.Atan2(speedY, speedX) + 90f, 0);
				}
			}
			return false; 
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SanguiteBar", 10);
			recipe.AddIngredient(null, "ChainedPlasma", 1);
			recipe.AddIngredient(ItemID.ButchersChainsaw, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}