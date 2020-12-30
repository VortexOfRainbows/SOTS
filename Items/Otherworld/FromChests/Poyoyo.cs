using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;                    
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items.Otherworld.FromChests
{
    public class Poyoyo : ModItem
    {
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = mod.GetTexture("Items/Otherworld/FromChests/PoyoyoGlow");
            Color color = Color.White;
            Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
            Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
        }
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Poyo-yo");
			Tooltip.SetDefault("Leaves behind a rainbow trail that does 60% damage");
		}
        public override void SetDefaults()
        {
            item.damage = 30;
            item.width = 30;
            item.height = 26;
            item.melee = true; 
            item.useTime = 25;  
            item.useAnimation = 25;   
            item.useStyle = 5;
            item.channel = true;
            item.knockBack = 2f;
            item.value = Item.sellPrice(0, 4, 0, 0);
            item.rare = ItemRarityID.LightPurple;
            item.autoReuse = false; 
            item.shoot = mod.ProjectileType("Poyoyo"); 
            item.noUseGraphic = true; 
            item.noMelee = true;
            item.UseSound = SoundID.Item1;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "StarlightAlloy", 8);
            recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}