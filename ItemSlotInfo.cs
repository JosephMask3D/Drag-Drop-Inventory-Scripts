[System.Serializable]
public class ItemSlotInfo
{
    public Item item;
    public string name;
    public int stacks;

    public ItemSlotInfo(Item newItem, int newStacks)
    {
        item = newItem;
        stacks = newStacks;
    }
}
