using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
 
namespace SOTS.Items.Pyramid
{
    public class Aten : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aten");
			Tooltip.SetDefault("'The defunct god... now in flail form'");
		}
        public override void SetDefaults()
        {
            item.damage = 19;
            item.width = 34;
            item.height = 34;
            item.value = Item.sellPrice(0, 1, 50, 0);
            item.rare = 4;
            item.noMelee = true;
            item.useStyle = 5;
            item.useAnimation = 60;
            item.useTime = 60;
            item.knockBack = 4.5f;
            item.noUseGraphic = true; 
            item.shoot = mod.ProjectileType("AtenProj");
            item.shootSpeed = 14.5f;
            item.UseSound = SoundID.Item1;
            item.melee = true; 
            item.channel = true;
        }
    }
}