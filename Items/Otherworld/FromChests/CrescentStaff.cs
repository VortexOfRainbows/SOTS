using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Void;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace SOTS.Items.Otherworld.FromChests
{
	public class CrescentStaff : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crescent Staff");
			Tooltip.SetDefault("Cast a wave of Crescents that each lock onto an enemy, and then steal life and mana from them");
		}
		public override void SafeSetDefaults()
		{
            item.damage = 36;
            item.ranged = true;
            item.width = 40;
            item.height = 40;
            item.useTime = 10; 
            item.useAnimation = 30;
            item.useStyle = 5;    
            item.noMelee = true;
			item.knockBack = 2f;
			item.value = Item.sellPrice(0, 3, 80, 0);
			item.rare = ItemRarityID.LightPurple;
			item.UseSound = SoundID.Item8;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("MacaroniMoon");
            item.shootSpeed = 4.25f;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/Otherworld/FromChests/CrescentStaffGlow");
			}
			Item.staff[item.type] = true;
		}
		int projectileNum = 0;
		int highestProjectileNum = 0;
		public override bool BeforeUseItem(Player player)
		{
			projectileNum = 0;
			return base.BeforeUseItem(player);
		}
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Otherworld/FromChests/CrescentStaffGlow");
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			base.PostDrawInWorld(spriteBatch, lightColor, alphaColor, rotation, scale, whoAmI);
        }
        public override void GetVoid(Player player)
		{
			voidMana = 8;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians((speedX < 0 ? -1 : 1) * (-20f + 20f * projectileNum)));
			speedX = perturbedSpeed.X;
			speedY = perturbedSpeed.Y;
			position += new Vector2(speedX, speedY) * 6;

			projectileNum++;
			if(highestProjectileNum < projectileNum)
				highestProjectileNum = projectileNum;
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PlatinumSoulStaff", 1);
			recipe.AddIngredient(null, "StarlightAlloy", 8);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
