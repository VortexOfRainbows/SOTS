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
			this.SetResearchCost(1);
		}
        public override void SafeSetDefaults()
		{
            Item.damage = 32;  
            Item.DamageType = DamageClass.Melee; 
            Item.width = 48;    
            Item.height = 48;  
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;   
            Item.autoReuse = true; 
			Item.useTurn = true;
            Item.knockBack = 4f;
			Item.value = Item.sellPrice(0, 0, 35, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item71;
			Item.crit = 11;
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
		public override int GetVoid(Player player)
		{
			return 10;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.PlatinumBar, 15).AddTile(TileID.Anvils).Register();
		}
	}
}
