using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BestFriend.Content.Items.Ammo;

public class RubberBullet : ModItem {
  public override string Texture => BestFriend.ASSET_PATH + "Textures/Items/Ammo/RubberBullet";

  public override void SetStaticDefaults() {
    Item.ResearchUnlockCount = 99;
  }

  public override void SetDefaults() {
    Item.damage = 1;
    Item.DamageType = DamageClass.Ranged;
    Item.width = 16;
    Item.height = 16;
    Item.maxStack = Item.CommonMaxStack;
    Item.consumable = true; // This marks the item as consumable, making it automatically be consumed when it's used as ammunition, or something else, if possible.
    Item.knockBack = 7.0f;
    Item.value = 1;
    Item.rare = ItemRarityID.White;
    Item.shoot = ModContent.ProjectileType<Projectiles.RubberBullet>(); // The projectile that weapons fire when using this item as ammunition.
    Item.shootSpeed = 1f; // The speed of the projectile.
    Item.ammo = AmmoID.Bullet; // The ammo class this ammo belongs to.
  }

  public override void AddRecipes() {
    CreateRecipe()
      .AddIngredient(ItemID.Gel)
      .AddIngredient(ItemID.StoneBlock)
      .Register();
  }
}