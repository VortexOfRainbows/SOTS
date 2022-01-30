using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Dusts;

namespace SOTS.Items.OreItems
{
	public class PlatinumScythe : VoidItem
	{ 	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Platinum Scythe");
			Tooltip.SetDefault("Attacks permanently curse enemies for 4 damage per second, stacking up to 10 times");
		}
        public override void SafeSetDefaults()
		{
            item.damage = 32;  
            item.melee = true; 
            item.width = 48;    
            item.height = 48;  
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = 1;   
            item.autoReuse = true; 
			item.useTurn = true;
            item.knockBack = 4f;
			item.value = Item.sellPrice(0, 0, 35, 0);
            item.rare = ItemRarityID.Green;
            item.UseSound = SoundID.Item71;
			item.crit = 11;
		}
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextBool(12))
			{
				Dust dust = Dust.NewDustDirect(hitbox.Location.ToVector2() - new Vector2(5f), hitbox.Width, hitbox.Height, ModContent.DustType<CopyDust4>(), 0, -2, 200, new Color(), 1f);
				dust.velocity *= 0.4f;
				dust.color = new Color(100, 100, 255, 120);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 1.5f;
			}
		}
		public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
		{
			//Now modified in DebuffNPC
		}
		public override void GetVoid(Player player)
		{
			voidMana = 10;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.PlatinumBar, 15);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
