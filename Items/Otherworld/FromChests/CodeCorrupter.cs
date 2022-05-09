using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld.FromChests
{
	public class CodeCorrupter : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Code Corrupter");
			Tooltip.SetDefault("Fires a short ranged and a long ranged blast\nThe short ranged blast has a 70% chance to destabilize enemies while the long ranged blast has a 30% chance to destabalize enemies\nDestabilized enemies gain a 5% flat chance to be critically striked\nThis is calculated after normal crits, allowing some attacks to double crit\nDestabilization is permanent and can stack, but the chance of applying it gets lower with each level already applied");
		}
		public override void SetDefaults()
		{
			Item.noMelee = true;
			Item.damage = 36;  
            Item.DamageType = DamageClass.Magic;    
            Item.width = 46;  
            Item.height = 30;   
            Item.useTime = 24;  
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 6f;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.UseSound = SoundID.Item96;
            Item.autoReuse = true;
            Item.shoot = mod.ProjectileType("CodeBurst"); 
            Item.shootSpeed = 16.5f;
			Item.reuseDelay = 8;
			Item.mana = 14;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/CodeCorrupterGlow").Value;
				Item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -1;
			}
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/CodeCorrupterGlow").Value;
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position.X += speedX * 3f;
			position.Y += speedY * 3f;
			Projectile.NewProjectile(position, new Vector2(speedX, speedY) * 0.75f, mod.ProjectileType("CodeVolley"), damage, knockBack, player.whoAmI) ;
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "HardlightAlloy", 16);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
