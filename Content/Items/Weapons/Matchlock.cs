using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;

namespace BestFriend.Content.Items.Weapons;

public class MatchlockPistol : BaseGun {
  public override string Texture => BestFriend.ASSET_PATH + "Textures/Items/Weapons/Matchlock";

  public override Dictionary<FireMode, GunStats> Stats { get; } = new() {
    {
      FireMode.Primary, new GunStats {
        damage = 10,
        crit = 5,
        knockback = 1,
        use_time = 24,
        fire_rate = 0
      }
    }
  };

  public override void SetStaticDefaults() {
    CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
  }

  public override void SetDefaults() {
    Item.width = 64;
    Item.height = 24;
    Item.scale = 1f;
    Item.rare = ItemRarityID.White;
    Item.value = Item.sellPrice(0, 0, 0, 69);

    base.SetDefaults();
  }

  public override Vector2? HoldoutOffset() {
    return new Vector2(-4, 2);
  }

  public override void AddRecipes() {
    CreateRecipe()
      .AddRecipeGroup(RecipeGroupID.Wood, 10)
      .Register();

    CreateRecipe()
      .AddIngredient<MatchlockPistol>()
      .AddRecipeGroup(BestFriendRecipes.DemoniteBarRecipeGroupId, 5)
      .AddIngredient(ItemID.IllegalGunParts)
      .Register()
      .ReplaceResult(ItemID.Revolver);
  }
}