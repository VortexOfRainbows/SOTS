using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Pyramid
{
	public class CurseballTome : ModItem
	{	int index1 = -1;
		int index2 = -1;
		int index3 = -1;
		int index4 = -1;
		int rot = 0;
		bool inInventory = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Curseball Tome");
			Tooltip.SetDefault("Surrounds you with curseballs");
		}
		public override void SetDefaults()
		{
            item.damage = 18; 
            item.magic = true; 
            item.width = 30;   
            item.height = 36;   
            item.useTime = 25;   
            item.useAnimation = 25;
            item.useStyle = 5;    
            item.noMelee = true;  
            item.knockBack = 2.5f;
            item.value = Item.sellPrice(0, 2, 25, 0);
            item.rare = 5;
            item.UseSound = SoundID.Item8;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("FriendlyCurseBall"); 
            item.shootSpeed = 9.5f;
			item.mana = 16;
			item.reuseDelay = 20;
		}
        public override bool CanUseItem(Player player)
		{
			if(inInventory)
				return true;
			return false;
		}
		public override void UpdateInventory(Player player)
		{
			inInventory = true;
			rot++;
			if(index1 != -1)
			{
				Projectile proj = Main.projectile[index1];
				if(proj.type == item.shoot)
				{
					Vector2 rotatePos = new Vector2(64, 0).RotatedBy(MathHelper.ToRadians(rot));
					proj.position.X = rotatePos.X + player.Center.X - proj.width/2;
					proj.position.Y = rotatePos.Y + player.Center.Y - proj.height/2;
				}
			}
			if(index2 != -1)
			{
				Projectile proj = Main.projectile[index2];
				if(proj.type == item.shoot)
				{
					Vector2 rotatePos = new Vector2(64, 0).RotatedBy(MathHelper.ToRadians(rot + 90));
					proj.position.X = rotatePos.X + player.Center.X - proj.width/2;
					proj.position.Y = rotatePos.Y + player.Center.Y - proj.height/2;
				}
			}
			if(index3 != -1)
			{
				Projectile proj = Main.projectile[index3];
				if(proj.type == item.shoot)
				{
					Vector2 rotatePos = new Vector2(64, 0).RotatedBy(MathHelper.ToRadians(rot + 180));
					proj.position.X = rotatePos.X + player.Center.X - proj.width/2;
					proj.position.Y = rotatePos.Y + player.Center.Y - proj.height/2;
				}
			}
			if(index4 != -1)
			{
				Projectile proj = Main.projectile[index4];
				if(proj.type == item.shoot)
				{
					Vector2 rotatePos = new Vector2(64, 0).RotatedBy(MathHelper.ToRadians(rot + 270));
					proj.position.X = rotatePos.X + player.Center.X - proj.width/2;
					proj.position.Y = rotatePos.Y + player.Center.Y - proj.height/2;
				}
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "CursedMatter", 6);
			recipe.AddIngredient(ItemID.Ruby, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			inInventory = false;
			if(index1 != -1 && Main.projectile[index1].type == type && Main.projectile[index1].friendly && Main.projectile[index1].active)
				Main.projectile[index1].velocity = new Vector2(speedX, speedY);
			
				index1 = Projectile.NewProjectile(position.X, position.Y, 0, 0, type, damage, knockBack, player.whoAmI);
						
			if(index2 != -1 && Main.projectile[index2].type == type && Main.projectile[index2].friendly && Main.projectile[index2].active)
				Main.projectile[index2].velocity = new Vector2(speedX, speedY);
			
				index2 = Projectile.NewProjectile(position.X, position.Y, 0, 0, type, damage, knockBack, player.whoAmI);
			
			if(index3 != -1 && Main.projectile[index3].type == type && Main.projectile[index3].friendly && Main.projectile[index3].active)
				Main.projectile[index3].velocity = new Vector2(speedX, speedY);
			
				index3 = Projectile.NewProjectile(position.X, position.Y, 0, 0, type, damage, knockBack, player.whoAmI);
						
			if(index4 != -1 && Main.projectile[index4].type == type && Main.projectile[index4].friendly && Main.projectile[index4].active)
				Main.projectile[index4].velocity = new Vector2(speedX, speedY);
			
				index4 = Projectile.NewProjectile(position.X, position.Y, 0, 0, type, damage, knockBack, player.whoAmI);
			return false; 
		}
	}
}
