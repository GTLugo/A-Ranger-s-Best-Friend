using BestFriend.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace BestFriend.Content.Items.Accessories;

public class PerfectSync : ModItem {
  public override string Texture => BestFriend.ASSET_PATH + "Textures/Items/Accessories/PerfectSync";

  public override void SetStaticDefaults() {
    CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
  }

  public override void SetDefaults() {
    Item.width = 28;
    Item.height = 28;
    Item.accessory = true;
    Item.rare = ItemRarityID.Lime;
  }

  public override void UpdateAccessory(Player player, bool hide_visual) {
    var abilities = player.GetModPlayer<BestFriendAbilitiesPlayer>();
    player.maxMinions += 2;
    player.GetDamage(DamageClass.Generic) += 0.10f;
  }

  public override void AddRecipes() {
    CreateRecipe()
      .AddIngredient(ItemID.AvengerEmblem)
      .AddIngredient(ItemID.PygmyNecklace)
      .AddTile(TileID.TinkerersWorkbench)
      .Register();
  }
}