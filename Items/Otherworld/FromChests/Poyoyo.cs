using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Otherworld.Furniture;
using Terraria;                
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items.Otherworld.FromChests
{
    public class Poyoyo : ModItem
    {
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/PoyoyoGlow").Value;
            Color color = Color.White;
            Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
            Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
        }
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Poyo-yo");
			Tooltip.SetDefault("Leaves behind a rainbow trail that does 60% damage");
            this.SetResearchCost(1);
        }
        public override void SetDefaults()
        {
            Item.damage = 30;
            Item.width = 30;
            Item.height = 26;
            Item.DamageType = DamageClass.Melee; 
            Item.useTime = 25;  
            Item.useAnimation = 25;   
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.channel = true;
            Item.knockBack = 2f;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.autoReuse = false; 
            Item.shoot = ModContent.ProjectileType<Projectiles.Otherworld.Poyoyo>(); 
            Item.noUseGraphic = true; 
            Item.noMelee = true;
            Item.UseSound = SoundID.Item1;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient<StarlightAlloy>(8).AddTile(ModContent.TileType<HardlightFabricatorTile>()).Register();
        }
    }
}