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
            Item.damage = 130;
            Item.melee = true;  
            Item.width = 64;
            Item.height = 54;  
            Item.useTime = 28; 
            Item.useAnimation = 28;
            Item.useStyle = ItemUseStyleID.SwingThrow;    
            Item.knockBack = 6f;
            Item.value = Item.sellPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item22;
            Item.autoReuse = true;
            Item.shoot = mod.ProjectileType("PlasmaCutter"); 
            Item.shootSpeed = 0f;
			Item.channel = true;
			Item.axe = 200;
            Item.noUseGraphic = true; 
            Item.noMelee = true;
			Item.staff[Item.type] = true; 
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
			Item.useTime = useTime;
			Item.useAnimation = useTime;
			return base.UseTimeMultiplier(player);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			bool summon = true;
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if(proj.active && proj.type == Item.shoot && Main.player[proj.owner] == player)
				{
					summon = false;
				}
			}
			if(player.altFunctionUse != 2)
			{
				Item.UseSound = SoundID.Item22;
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